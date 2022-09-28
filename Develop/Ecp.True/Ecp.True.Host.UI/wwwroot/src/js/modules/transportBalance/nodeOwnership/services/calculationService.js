import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';

const calculationService = (function () {
    const variableTypes = {
        interface: { name: 'interface', value: constants.VariableType.Interface },
        tolerance: { name: 'tolerance', value: constants.VariableType.Tolerance },
        unidentifiedLosses: { name: 'unidentifiedLosses', value: constants.VariableType.UnidentifiedLoss },
        initialInventory: { name: 'initialInventory', value: constants.VariableType.InitialInventory },
        inputs: { name: 'inputs', value: constants.VariableType.Input },
        outputs: { name: 'outputs', value: constants.VariableType.Output },
        identifiedLosses: { name: 'identifiedLosses', value: constants.VariableType.IdentifiedLoss },
        finalInventory: { name: 'finalInventory', value: constants.VariableType.FinalInventory }
    };
    const getTotalSum = (rows, key) => {
        const keyValues = rows.data.map(a => a[key]);
        const total = keyValues.reduce((a, b) => {
            return (parseFloat(a) + parseFloat(b));
        });
        return total;
    };
    const isSpecialMovement = variableType => {
        return utilities.matchesAny(variableType, [variableTypes.interface.name, variableTypes.tolerance.name, variableTypes.unidentifiedLosses.name]);
    };
    const calculateOwnershipPerVariableType = (variableType, data, nodeId) => {
        const helper = {};
        const result = data.reduce((r, o) => {
            let key = '';

            if ((isSpecialMovement(variableType) || variableType === variableTypes.inputs.name) && o.destinationNodeId === nodeId) {
                key = `${o.destinationProductId}-${o.ownerId}`;

                if (!helper[key]) {
                    helper[key] = Object.assign({},
                        {
                            productId: o.destinationProductId,
                            product: o.destinationProduct,
                            owner: o.ownerName,
                            [variableType]: o.ownershipVolume,
                            measurementUnit: o.unit
                        });
                    r.push(helper[key]);
                } else {
                    helper[key][variableType] = parseFloat(helper[key][variableType]) + parseFloat(o.ownershipVolume);
                }
            }

            if (variableType !== variableTypes.inputs.name && o.sourceNodeId === nodeId) {
                key = `${o.sourceProductId}-${o.ownerId}`;

                if (!helper[key]) {
                    helper[key] = {
                        productId: o.sourceProductId,
                        product: o.sourceProduct,
                        owner: o.ownerName,
                        [variableType]: isSpecialMovement(variableType) ? -o.ownershipVolume : o.ownershipVolume,
                        measurementUnit: o.unit
                    };
                    r.push(helper[key]);
                } else if (isSpecialMovement(variableType)) {
                    helper[key][variableType] = parseFloat(helper[key][variableType]) - parseFloat(o.ownershipVolume);
                } else {
                    helper[key][variableType] = parseFloat(helper[key][variableType]) + parseFloat(o.ownershipVolume);
                }
            }
            return r;
        }, []);

        return result;
    };
    const updateOwnershipPerVariableType = (variableType, summaryData, data) => {
        data.forEach(item => {
            const val = summaryData.findIndex(x => x.productId === item.productId && x.owner === item.owner);
            if (val > -1) {
                summaryData[val][variableType] = utilities.parseFloat(item[variableType]);
            } else {
                summaryData.push(item);
            }
        });
    };
    return {
        sumOwnershipBalanceGroup: vals => {
            return vals.reduce((a, b) => {
                return ((utilities.isNullOrUndefined(a) ? 0 : parseFloat(a)) + (utilities.isNullOrUndefined(b) ? 0 : parseFloat(b))).toFixed(2);
            }, 0);
        },
        calculateTotalControlPercent: rows => {
            if (rows.data.length > 0) {
                return rows.data.reduce((a, b) => ({
                    volumeTotal: parseFloat(a.volumeTotal) + parseFloat(b.volume),
                    inputsTotal: parseFloat(a.inputsTotal) + parseFloat(b.inputs)
                }), { volumeTotal: 0, inputsTotal: 0 });
            }
            return null;
        },
        calculateControlPercent: data => {
            const row = data.row;
            let inputs = 0;
            let volume = 0;
            if (row) {
                volume = parseFloat(row.volume);
                inputs = parseFloat(row.inputs);
                return {
                    inputs: inputs,
                    volume: volume
                };
            }
            return null;
        },
        aggregatedTextColTemplate: row => {
            return ((row && row.length > 0) ? row[0] : null);
        },
        getTotalSum: (rows, key) => {
            return getTotalSum(rows, key);
        },
        sumTotalOwnershipBalance: (rows, key) => {
            if (rows.data.length === 1) {
                const totalSum = (parseFloat(rows.data[0][key])).toFixed(2);
                return totalSum;
            }
            if (rows.data.length > 1) {
                return getTotalSum(rows, key);
            }
            return null;
        },
        calculate: (nodeId, inventories, summary) => {
            const calculatedData = {};
            Object.keys(variableTypes).forEach(variableType => {
                if (variableType === variableTypes.inputs.name || variableType === variableTypes.outputs.name) {
                    calculatedData[variableType] = calculateOwnershipPerVariableType(variableType,
                        inventories.filter(item => (item.variableTypeId === variableTypes.inputs.value || item.variableTypeId === variableTypes.outputs.value)), nodeId);
                } else if (variableType !== variableTypes.initialInventory.name) {
                    calculatedData[variableType] = calculateOwnershipPerVariableType(variableType,
                        inventories.filter(item => item.variableTypeId === variableTypes[variableType].value), nodeId);
                }
            });

            summary.forEach(item => {
                item.inputs = 0;
                item.outputs = 0;
                item.identifiedLosses = 0;
                item.interface = 0;
                item.tolerance = 0;
                item.unidentifiedLosses = 0;
            });

            Object.keys(variableTypes).forEach(variableType => {
                if (variableType !== variableTypes.initialInventory.name) {
                    updateOwnershipPerVariableType(variableType, summary, calculatedData[variableType]);
                }
            });

            summary.forEach(item => {
                item.volume = (parseFloat(item.initialInventory) || 0) + (parseFloat(item.inputs) || 0) - (parseFloat(item.outputs) || 0) - (parseFloat(item.finalInventory) || 0) -
                    (parseFloat(item.identifiedLosses) || 0) + (parseFloat(item.interface) || 0) + (parseFloat(item.tolerance) || 0) + (parseFloat(item.unidentifiedLosses) || 0);
                item.volume = Math.abs(parseFloat(item.volume)).toFixed(2) === '0.00' ? 0 : parseFloat(item.volume);
            });
        },
        sumTotalVolumePercentageOwnership: (rows, key) => {
            if (rows.data.length === 1) {
                return rows.data[0][key];
            }
            if (rows.data.length > 1) {
                return rows.data.reduce((a, b) => parseFloat(a) + parseFloat(b[key] ? parseFloat(b[key]).toFixed(2) : 0), 0);
            }
            return null;
        }
    };
}());

export { calculationService };

