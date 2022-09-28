import { dataService } from './../../../../modules/transportBalance/nodeOwnership/services/dataService';
import { constants } from '../../../../common/services/constants';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { DateCell } from '../../../../common/components/grid/gridCells.jsx';
import { utilities } from '../../../../common/services/utilities';
import { dateService } from '../../../../common/services/dateService';
import { change } from 'redux-form';
import classNames from 'classnames/bind';
import { requestSourceNodes, requestSourceProducts, requestDestinationProducts, requestDestinationNodes } from '../../../../modules/transportBalance/nodeOwnership/actions';

describe('data service tests',
    () => {
        it('should build movement object',
            () => {
                const mockMomentCallback = jest.fn();
                dateService.now = jest.fn(() => mockMomentCallback);
                mockMomentCallback.toDate = jest.fn(() => 'some date');
                utilities.getSessionId = jest.fn(() => 'MovementId');
                const inputObject = [
                    {
                        transactionId: 1,
                        movementTypeId: -1,
                        operationalDate: '11-Abr-20',
                        netVolume: 100,
                        unitId: 1,
                        variableTypeId: 1,
                        reasonId: 1,
                        comment: 'test',
                        status: 'create',
                        movementId: '1234',
                        sourceNodeId: 1,
                        sourceProductId: 100002089,
                        contractId: 1234,
                        destinationNodeId: 1,
                        destinationProductId: 100002089,
                        ownerId: 1,
                        ownershipPercentage: 33,
                        ownershipVolume: 33.33,
                        ownershipFunction: 'function',
                        ruleVersion: 1
                    }
                ];
                const outputObject = {
                    ticketId: 1,
                    movementTransactionId: 1,
                    movementTypeId: -1,
                    operationalDate: '11-Abr-20',
                    netStandardVolume: 100,
                    measurementUnit: 1,
                    variableTypeId: 1,
                    reasonId: 1,
                    comment: 'test',
                    eventType: 'Insert',
                    movementId: '1234',
                    movementSource: {
                        sourceNodeId: 1,
                        sourceProductId: 100002089
                    },
                    movementContractId: 1234,
                    movementDestination: {
                        destinationNodeId: 1,
                        destinationProductId: 100002089
                    },
                    period: {
                        movementTransactionId: 1
                    },
                    ownerships: [
                        {
                            ticketId: 1,
                            ownerId: 1,
                            ownershipPercentage: 33,
                            ownershipVolume: 33.33,
                            appliedRule: 'function',
                            ruleVersion: 1,
                            movementTransactionId: 1,
                            executionDate: 'some date'
                        }
                    ]
                };
                expect(dataService.buildMovement(inputObject, 1)).toEqual(outputObject);
            });

        it('should build inventory object',
            () => {
                const inputObject = [
                    {
                        transactionId: 1,
                        reasonId: 1,
                        comment: 'test',
                        status: 'create',
                        movementId: 1234,
                        ownerId: 1,
                        ownershipPercentage: 33,
                        ownershipVolume: 33.33,
                        ownershipFunction: 'function',
                        ruleVersion: 1
                    }
                ];
                const outputObject = {
                    reasonId: 1,
                    comment: 'test',
                    ownership: {
                        ticketId: 1,
                        inventoryProductId: 1,
                        eventType: 'Insert',
                        ownerships: [{
                            ticketId: 1,
                            ownerId: 1,
                            ownershipPercentage: 33,
                            ownershipVolume: 33.33,
                            appliedRule: 'function',
                            ruleVersion: 1,
                            inventoryProductId: 1
                        }]
                    }
                };
                expect(dataService.buildInventory(inputObject, 1)).toEqual(outputObject);
            });

        it('should compare status with eq and or',
            () => {
                const statusToCheck = 'test';
                const statusArray = ['test', 'test1', 'test2'];
                expect(dataService.compareStatus(statusToCheck, statusArray, 'eq', 'or')).toEqual(true);
            });

        it('should compare status with ne and or',
            () => {
                const statusToCheck = 'test';
                const statusArray = ['test', 'test1'];
                expect(dataService.compareStatus(statusToCheck, statusArray, 'ne', 'or')).toEqual(true);
            });

        it('should compare status with eq and and',
            () => {
                const statusToCheck = 'test';
                const statusArray = ['test', 'test1', 'test2'];
                expect(dataService.compareStatus(statusToCheck, statusArray, 'eq', 'and')).toEqual(false);
            });

        it('should compare status with ne and and',
            () => {
                const statusToCheck = 'test';
                const statusArray = ['test', 'test'];
                expect(dataService.compareStatus(statusToCheck, statusArray, 'ne', 'and')).toEqual(false);
            });
        it('should build columns for interface variable type',
            () => {
                const columns = [];
                const props = {};
                const date = () => <DateCell />;
                columns.push(gridUtils.buildTextColumn('movementType', props, null, 'movementType'));
                columns.push(gridUtils.buildDateColumn('operationalDate', props, date, 'operationalDate'));
                columns.push(gridUtils.buildTextColumn('sourceProduct', props, null, 'sourceProduct'));
                columns.push(gridUtils.buildTextColumn('destinationProduct', props, null, 'destinationProduct'));
                columns.push(gridUtils.buildDecimalColumn('netVolume', props, 'volume'));
                columns.push(gridUtils.buildTextColumn('unit', props, null, 'unit'));
                columns.push(gridUtils.buildTextColumn('ownershipFunction', props, null, 'ownershipFunction'));
                columns.push(gridUtils.buildDecimalColumn('ownershipVolume', props, 'ownershipVolume'));
                columns.push(gridUtils.buildDecimalColumn('ownershipPercentage', props, 'ownershipPercentage'));

                expect(JSON.stringify(dataService.buildVariableTypeColumns(constants.VariableType.Interface, date, props))).toEqual(JSON.stringify(columns));
            });
        it('should build columns for tolerance variable type',
            () => {
                const columns = [];
                const props = {};
                const date = () => <DateCell />;
                columns.push(gridUtils.buildTextColumn('movementType', props, null, 'movementType'));
                columns.push(gridUtils.buildDateColumn('operationalDate', props, date, 'operationalDate'));
                columns.push(gridUtils.buildTextColumn('sourceNode', props, null, 'sourceNode'));
                columns.push(gridUtils.buildTextColumn('destinationNode', props, null, 'destinationNode'));
                columns.push(gridUtils.buildTextColumn('sourceProduct', props, null, 'sourceProduct'));
                columns.push(gridUtils.buildTextColumn('destinationProduct', props, null, 'destinationProduct'));
                columns.push(gridUtils.buildDecimalColumn('netVolume', props, 'volume'));
                columns.push(gridUtils.buildTextColumn('unit', props, null, 'unit'));
                columns.push(gridUtils.buildTextColumn('ownershipFunction', props, null, 'ownershipFunction'));
                columns.push(gridUtils.buildDecimalColumn('ownershipVolume', props, 'ownershipVolume'));
                columns.push(gridUtils.buildDecimalColumn('ownershipPercentage', props, 'ownershipPercentage'));

                expect(JSON.stringify(dataService.buildVariableTypeColumns(constants.VariableType.Tolerance, date, props))).toEqual(JSON.stringify(columns));
            });

        it('should build columns for UnidentifiedLoss variable type',
            () => {
                const columns = [];
                const props = {};
                const date = () => <DateCell />;
                columns.push(gridUtils.buildTextColumn('movementType', props, null, 'movementType'));
                columns.push(gridUtils.buildDateColumn('operationalDate', props, date, 'operationalDate'));
                columns.push(gridUtils.buildTextColumn('sourceNode', props, null, 'sourceNode'));
                columns.push(gridUtils.buildTextColumn('destinationNode', props, null, 'destinationNode'));
                columns.push(gridUtils.buildTextColumn('sourceProduct', props, null, 'sourceProduct'));
                columns.push(gridUtils.buildTextColumn('destinationProduct', props, null, 'destinationProduct'));
                columns.push(gridUtils.buildDecimalColumn('netVolume', props, 'volume'));
                columns.push(gridUtils.buildTextColumn('unit', props, null, 'unit'));
                columns.push(gridUtils.buildTextColumn('ownershipFunction', props, null, 'ownershipFunction'));
                columns.push(gridUtils.buildDecimalColumn('ownershipVolume', props, 'ownershipVolume'));
                columns.push(gridUtils.buildDecimalColumn('ownershipPercentage', props, 'ownershipPercentage'));

                expect(JSON.stringify(dataService.buildVariableTypeColumns(constants.VariableType.UnidentifiedLoss, date, props))).toEqual(JSON.stringify(columns));
            });
        it('should build columns for Output variable type',
            () => {
                const columns = [];
                const props = {};
                const date = () => <DateCell />;
                columns.push(gridUtils.buildTextColumn('movementType', props, null, 'movementType'));
                columns.push(gridUtils.buildDateColumn('operationalDate', props, date, 'operationalDate'));
                columns.push(gridUtils.buildTextColumn('destinationNode', props, null, 'destinationNode'));
                columns.push(gridUtils.buildTextColumn('destinationProduct', props, null, 'destinationProduct'));
                columns.push(gridUtils.buildDecimalColumn('netVolume', props, 'volume'));
                columns.push(gridUtils.buildTextColumn('unit', props, null, 'unit'));
                columns.push(gridUtils.buildTextColumn('ownershipFunction', props, null, 'ownershipFunction'));
                columns.push(gridUtils.buildDecimalColumn('ownershipVolume', props, 'ownershipVolume'));
                columns.push(gridUtils.buildDecimalColumn('ownershipPercentage', props, 'ownershipPercentage'));

                expect(JSON.stringify(dataService.buildVariableTypeColumns(constants.VariableType.Output, date, props))).toEqual(JSON.stringify(columns));
            });
        it('should build columns for IdentifiedLoss variable type',
            () => {
                const columns = [];
                const props = {};
                const date = () => <DateCell />;
                columns.push(gridUtils.buildTextColumn('movementType', props, null, 'movementType'));
                columns.push(gridUtils.buildDateColumn('operationalDate', props, date, 'operationalDate'));
                columns.push(gridUtils.buildTextColumn('destinationNode', props, null, 'destinationNode'));
                columns.push(gridUtils.buildTextColumn('destinationProduct', props, null, 'destinationProduct'));
                columns.push(gridUtils.buildDecimalColumn('netVolume', props, 'volume'));
                columns.push(gridUtils.buildTextColumn('unit', props, null, 'unit'));
                columns.push(gridUtils.buildTextColumn('ownershipFunction', props, null, 'ownershipFunction'));
                columns.push(gridUtils.buildDecimalColumn('ownershipVolume', props, 'ownershipVolume'));
                columns.push(gridUtils.buildDecimalColumn('ownershipPercentage', props, 'ownershipPercentage'));

                expect(JSON.stringify(dataService.buildVariableTypeColumns(constants.VariableType.IdentifiedLoss, date, props))).toEqual(JSON.stringify(columns));
            });
        it('should build columns for Input variable type',
            () => {
                const columns = [];
                const props = {};
                const date = () => <DateCell />;
                columns.push(gridUtils.buildTextColumn('movementType', props, null, 'movementType'));
                columns.push(gridUtils.buildDateColumn('operationalDate', props, date, 'operationalDate'));
                columns.push(gridUtils.buildTextColumn('sourceNode', props, null, 'sourceNode'));
                columns.push(gridUtils.buildTextColumn('sourceProduct', props, null, 'sourceProduct'));
                columns.push(gridUtils.buildDecimalColumn('netVolume', props, 'volume'));
                columns.push(gridUtils.buildTextColumn('unit', props, null, 'unit'));
                columns.push(gridUtils.buildTextColumn('ownershipFunction', props, null, 'ownershipFunction'));
                columns.push(gridUtils.buildDecimalColumn('ownershipVolume', props, 'ownershipVolume'));
                columns.push(gridUtils.buildDecimalColumn('ownershipPercentage', props, 'ownershipPercentage'));

                expect(JSON.stringify(dataService.buildVariableTypeColumns(constants.VariableType.Input, date, props))).toEqual(JSON.stringify(columns));
            });
        it('should build columns for InitialInventory variable type',
            () => {
                const columns = [];
                const props = {};
                const date = () => <DateCell />;
                columns.push(gridUtils.buildDateColumn('operationalDate', props, date, 'operationalDate'));
                columns.push(gridUtils.buildTextColumn('tankName', props, null, 'tankName'));
                columns.push(gridUtils.buildDecimalColumn('netVolume', props, 'volume'));
                columns.push(gridUtils.buildTextColumn('unit', props, null, 'unit'));
                columns.push(gridUtils.buildTextColumn('ownershipFunction', props, null, 'ownershipFunction'));
                columns.push(gridUtils.buildDecimalColumn('ownershipVolume', props, 'ownershipVolume'));
                columns.push(gridUtils.buildDecimalColumn('ownershipPercentage', props, 'ownershipPercentage'));

                expect(JSON.stringify(dataService.buildVariableTypeColumns(constants.VariableType.InitialInventory, date, props))).toEqual(JSON.stringify(columns));
            });
        it('should build columns for FinalInventory variable type',
            () => {
                const columns = [];
                const props = {};
                const date = () => <DateCell />;
                columns.push(gridUtils.buildDateColumn('operationalDate', props, date, 'operationalDate'));
                columns.push(gridUtils.buildTextColumn('tankName', props, null, 'tankName'));
                columns.push(gridUtils.buildDecimalColumn('netVolume', props, 'volume'));
                columns.push(gridUtils.buildTextColumn('unit', props, null, 'unit'));
                columns.push(gridUtils.buildTextColumn('ownershipFunction', props, null, 'ownershipFunction'));
                columns.push(gridUtils.buildDecimalColumn('ownershipVolume', props, 'ownershipVolume'));
                columns.push(gridUtils.buildDecimalColumn('ownershipPercentage', props, 'ownershipPercentage'));

                expect(JSON.stringify(dataService.buildVariableTypeColumns(constants.VariableType.FinalInventory, date, props))).toEqual(JSON.stringify(columns));
            });
        it('should build node details filter for product',
            () => {
                const inputObject = {
                    productId: 1,
                    product: 'product'
                };
                const outputObject = {
                    product: {
                        productId: inputObject.productId,
                        product: inputObject.product
                    }
                };
                expect(dataService.buildNodeDetailsFilter(inputObject)).toEqual(outputObject);
            });
        it('should build node details filter for variable',
            () => {
                const inputObject = {
                    variableTypeId: 1,
                    name: 'variable'
                };
                const outputObject = {
                    variableType: {
                        variableTypeId: inputObject.variableTypeId,
                        name: inputObject.name
                    }
                };
                expect(dataService.buildNodeDetailsFilter(inputObject)).toEqual(outputObject);
            });
        it('should build node details filter for owner',
            () => {
                const inputObject = {
                    elementId: 1,
                    name: 'element'
                };
                const outputObject = {
                    owner: {
                        elementId: inputObject.elementId,
                        name: inputObject.name
                    }
                };
                expect(dataService.buildNodeDetailsFilter(inputObject)).toEqual(outputObject);
            });
        it('should build error check object',
            () => {
                const inputObject = {
                    sourceNodes: {
                        sourceNode: {
                            nodeId: 1,
                            name: 'name'
                        }
                    },
                    destinationNodes: {
                        destinationNode: {
                            nodeId: 1,
                            name: 'name'
                        }
                    },
                    sourceProduct: {
                        product: {
                            productId: 1,
                            name: 'name'
                        }
                    },
                    destinationProduct: {
                        product: {
                            productId: 1,
                            name: 'name'
                        }
                    }
                };
                const outputObject = {
                    sourceNodeId: 1,
                    sourceNode: 'name',
                    destinationNodeId: 1,
                    destinationNode: 'name',
                    sourceProductId: 1,
                    sourceProduct: 'name',
                    destinationProductId: 1,
                    destinationProduct: 'name'
                };
                expect(dataService.buildErrorCheckObject(inputObject)).toEqual(outputObject);
            });

        it('should build Movement Inventory Ownership Array',
            () => {
                const movementInventoryOwnershipData = [
                    {
                        netVolume: 100,
                        ownershipVolume: '33.33',
                        ownershipPercentage: '33.33',
                        ownerId: 1,
                        ownerName: 'name',
                        color: '#fff'
                    }
                ];

                const values = {
                    reasonForChange: {
                        elementId: 1,
                        name: 'name'
                    },
                    comment: 'test',
                    movementType: {
                        name: 'name',
                        elementId: 1
                    },
                    unit: {
                        name: 'name',
                        elementId: 12
                    },
                    variable: {
                        variableTypeId: 1
                    },
                    contract: {
                        contractId: 1,
                        documentNumber: 1011,
                        position: 'position'
                    }
                };

                const outputObject = [
                    {
                        reasonId: values.reasonForChange.elementId,
                        reason: values.reasonForChange.name,
                        comment: values.comment,
                        status: constants.Modes.Create,
                        movementType: values.movementType.name,
                        unit: values.unit.name,
                        unitId: values.unit.elementId,
                        variableTypeId: values.variable.variableTypeId,
                        movementTypeId: values.movementType.elementId,
                        netVolume: movementInventoryOwnershipData[0].netVolume,
                        ownershipVolume: movementInventoryOwnershipData[0].ownershipVolume,
                        ownershipPercentage: movementInventoryOwnershipData[0].ownershipPercentage,
                        ownerId: movementInventoryOwnershipData[0].ownerId,
                        ownerName: movementInventoryOwnershipData[0].ownerName,
                        color: movementInventoryOwnershipData[0].color,
                        contractId: !utilities.isNullOrUndefined(values.contract) ? values.contract.contractId : null,
                        documentNumber: !utilities.isNullOrUndefined(values.contract) ? values.contract.documentNumber : null,
                        position: !utilities.isNullOrUndefined(values.contract) ? values.contract.position : null
                    }
                ];

                expect(dataService.buildMovementInventoryOwnershipArray(movementInventoryOwnershipData, values)).toEqual(outputObject);
            });

        it('should build Movement Inventory Ownership Object for movement',
            () => {
                const movementInventoryOwnershipData = {
                    transactionId: 1,
                    movementType: 2,
                    operationalDate: new Date().toLocaleString(),
                    sourceNode: 1234,
                    destinationNode: 1234,
                    sourceProduct: 10002049,
                    destinationProduct: 10002049,
                    netVolume: 100,
                    unit: 'Bbl',
                    sourceProductId: 1,
                    destinationProductId: 1,
                    sourceNodeId: 1,
                    destinationNodeId: 1,
                    variableTypeId: 1,
                    movementId: 1,
                    documentNumber: 7000,
                    position: 1,
                    contractId: 1
                };

                const outputObject = {
                    transactionId: movementInventoryOwnershipData.transactionId,
                    movementType: movementInventoryOwnershipData.movementType,
                    operationalDate: movementInventoryOwnershipData.operationalDate,
                    sourceNode: movementInventoryOwnershipData.sourceNode,
                    destinationNode: movementInventoryOwnershipData.destinationNode,
                    sourceProduct: movementInventoryOwnershipData.sourceProduct,
                    destinationProduct: movementInventoryOwnershipData.destinationProduct,
                    netVolume: movementInventoryOwnershipData.netVolume,
                    unit: movementInventoryOwnershipData.unit,
                    sourceProductId: movementInventoryOwnershipData.sourceProductId,
                    destinationProductId: movementInventoryOwnershipData.destinationProductId,
                    sourceNodeId: movementInventoryOwnershipData.sourceNodeId,
                    destinationNodeId: movementInventoryOwnershipData.destinationNodeId,
                    variableTypeId: movementInventoryOwnershipData.variableTypeId,
                    movementId: movementInventoryOwnershipData.movementId,
                    documentNumber: movementInventoryOwnershipData.documentNumber,
                    position: movementInventoryOwnershipData.position,
                    contractId: movementInventoryOwnershipData.contractId
                };

                expect(dataService.buildMovementInventoryOwnershipObject(movementInventoryOwnershipData, true)).toEqual(outputObject);
            });


        it('should build Movement Inventory Ownership Object for inventory',
            () => {
                const movementInventoryOwnershipData = {
                    transactionId: 1,
                    movementType: 2,
                    operationalDate: new Date().toLocaleString(),
                    sourceNode: 1234,
                    destinationNode: 1234,
                    sourceProduct: 10002049,
                    destinationProduct: 10002049,
                    netVolume: 100,
                    unit: 'Bbl',
                    sourceProductId: 1,
                    destinationProductId: 1,
                    sourceNodeId: 1,
                    destinationNodeId: 1,
                    variableTypeId: 1,
                    tankName: 'name'
                };

                const outputObject = {
                    transactionId: movementInventoryOwnershipData.transactionId,
                    movementType: movementInventoryOwnershipData.movementType,
                    operationalDate: movementInventoryOwnershipData.operationalDate,
                    sourceNode: movementInventoryOwnershipData.sourceNode,
                    destinationNode: movementInventoryOwnershipData.destinationNode,
                    sourceProduct: movementInventoryOwnershipData.sourceProduct,
                    destinationProduct: movementInventoryOwnershipData.destinationProduct,
                    netVolume: movementInventoryOwnershipData.netVolume,
                    unit: movementInventoryOwnershipData.unit,
                    sourceProductId: movementInventoryOwnershipData.sourceProductId,
                    destinationProductId: movementInventoryOwnershipData.destinationProductId,
                    sourceNodeId: movementInventoryOwnershipData.sourceNodeId,
                    destinationNodeId: movementInventoryOwnershipData.destinationNodeId,
                    variableTypeId: movementInventoryOwnershipData.variableTypeId,
                    tankName: movementInventoryOwnershipData.tankName
                };

                expect(dataService.buildMovementInventoryOwnershipObject(movementInventoryOwnershipData, false)).toEqual(outputObject);
            });

        it('should build Movement Data Table without contract',
            () => {
                const operativeInformation = {
                    operationalDate: new Date().toLocaleString(),
                    sourceNode: 1234,
                    destinationNode: 1234,
                    sourceProduct: 10002049,
                    destinationProduct: 10002049,
                    netVolume: 100,
                    unit: 'Bbl',
                    movementId: 1,
                    documentNumber: 7000,
                    ownershipFunction: 'func'
                };

                const movementDataFirstTable = [
                    { key: 'operationDate', value: operativeInformation.operationalDate, isDate: true },
                    { key: 'sourceNode', value: operativeInformation.sourceNode },
                    { key: 'destinationNode', value: operativeInformation.destinationNode },
                    { key: 'prodOrigen', value: operativeInformation.sourceProduct },
                    { key: 'prodDestino', value: operativeInformation.destinationProduct }];

                const movementDataSecondTable = [
                    { key: 'documentNumber', value: operativeInformation.documentNumber, isDashedGridCell: true },
                    { key: 'position', value: operativeInformation.position, isDashedGridCell: true },
                    { key: 'volOperativo', value: operativeInformation.netVolume, isVolume: true },
                    { key: 'units', value: operativeInformation.unit },
                    { key: 'ilFunction', value: operativeInformation.ownershipFunction, isOwnershipFunction: true }];

                const movementDataThirdTable = [];

                const outputObject = [ movementDataFirstTable, movementDataSecondTable, movementDataThirdTable ];

                expect(dataService.buildMovementDataTable(operativeInformation, false, null)).toEqual(outputObject);
            });

        it('should build Movement Data Table with contract',
            () => {
                const operativeInformation = {
                    operationalDate: new Date().toLocaleString(),
                    sourceNode: 1234,
                    destinationNode: 1234,
                    sourceProduct: 10002049,
                    destinationProduct: 10002049,
                    netVolume: 100,
                    unit: 'Bbl',
                    movementId: 1,
                    documentNumber: 7000,
                    ownershipFunction: 'func'
                };

                const selectedContract = {
                    startDate: new Date().toLocaleString(),
                    endDate: new Date().toLocaleString(),
                    owner1: { name: 'name' },
                    owner2: { name: 'name' },
                    volume: 100,
                    measurementUnitDetail: { name: 'name' }
                };

                const movementDataFirstTable = [
                    { key: 'operationDate', value: operativeInformation.operationalDate, isDate: true },
                    { key: 'sourceNode', value: operativeInformation.sourceNode },
                    { key: 'destinationNode', value: operativeInformation.destinationNode },
                    { key: 'prodOrigen', value: operativeInformation.sourceProduct },
                    { key: 'prodDestino', value: operativeInformation.destinationProduct }];

                const movementDataSecondTable = [
                    { key: 'documentNumber', value: operativeInformation.documentNumber, isDashedGridCell: true },
                    { key: 'position', value: operativeInformation.position, isDashedGridCell: true },
                    { key: 'volOperativo', value: operativeInformation.netVolume, isVolume: true },
                    { key: 'units', value: operativeInformation.unit },
                    { key: 'ilFunction', value: operativeInformation.ownershipFunction, isOwnershipFunction: true }];

                const movementDataThirdTable = [
                    { key: 'initialDate', value: selectedContract.startDate, isDate: true },
                    { key: 'finalDate', value: selectedContract.endDate, isDate: true },
                    { key: 'owner1', value: selectedContract.owner1.name },
                    { key: 'owner2', value: selectedContract.owner2.name },
                    { key: 'value', value: selectedContract.volume },
                    { key: 'units', value: selectedContract.measurementUnitDetail.name }];

                const outputObject = [ movementDataFirstTable, movementDataSecondTable, movementDataThirdTable ];

                expect(dataService.buildMovementDataTable(operativeInformation, true, selectedContract)).toEqual(outputObject);
            });

        it('should build Inventory Data Table',
            () => {
                const operativeInformation = {
                    operationalDate: new Date().toLocaleString(),
                    sourceNode: 1234,
                    sourceProduct: 10002049,
                    netVolume: 100,
                    unit: 'Bbl',
                    ownershipFunction: 'func',
                    tankName: 'name'
                };

                const outputObject = [
                    { key: 'operationDate', value: operativeInformation.operationalDate, isDate: true },
                    { key: 'node', value: operativeInformation.sourceNode },
                    { key: 'tank', value: operativeInformation.tankName },
                    { key: 'product', value: operativeInformation.sourceProduct },
                    { key: 'volume', value: operativeInformation.netVolume, isVolume: true },
                    { key: 'units', value: operativeInformation.unit },
                    { key: 'ownershipFunction', value: operativeInformation.ownershipFunction, isOwnershipFunction: true }
                ];

                expect(dataService.buildInventoryDataTable(operativeInformation)).toEqual(outputObject);
            });

        it('should get dispatch actions for Input variable type ',
            () => {
                const params = {
                    selection: {
                        variableTypeId: constants.VariableType.Input
                    },
                    destinationNode: [],
                    nodeId: 1
                };
                const fieldName = 'variable';

                const actions = {
                    netVolume: [],
                    unit: [],
                    variable: [
                        change('createMovement', `sourceProduct`, null),
                        change('createMovement', `destinationProduct`, null),
                        change('createMovement', `movementType`, null),
                        change('createMovement', `contract`, null),
                        requestSourceNodes(params.nodeId),
                        change('createMovement', `sourceNodes`, null),
                        change('createMovement', `destinationNodes`, params.destinationNode[0]),
                        requestDestinationProducts(params.nodeId)
                    ],
                    sourceNodes: [
                        requestSourceProducts(params.selection.nodeId),
                        change('createMovement', `sourceProduct`, null)
                    ],
                    destinationNodes: [
                        requestDestinationProducts(params.selection.nodeId),
                        change('createMovement', `destinationProduct`, null)
                    ],
                    sourceProduct: [],
                    destinationProduct: [],
                    movementType: [],
                    contract: []
                };

                expect(JSON.stringify(dataService.getDispatchActions(fieldName, params))).toEqual(JSON.stringify(actions[fieldName]));
            });

        it('should get dispatch actions for Output variable type ',
            () => {
                const params = {
                    selection: {
                        variableTypeId: constants.VariableType.Output
                    },
                    sourceNode: [],
                    nodeId: 1
                };
                const fieldName = 'variable';

                const actions = {
                    netVolume: [],
                    unit: [],
                    variable: [
                        change('createMovement', `sourceProduct`, null),
                        change('createMovement', `destinationProduct`, null),
                        change('createMovement', `movementType`, null),
                        change('createMovement', `contract`, null),
                        change('createMovement', `sourceNodes`, params.sourceNode[0]),
                        requestSourceProducts(params.nodeId),
                        requestDestinationNodes(params.nodeId),
                        change('createMovement', `destinationNodes`, null)
                    ],
                    sourceNodes: [
                        requestSourceProducts(params.selection.nodeId),
                        change('createMovement', `sourceProduct`, null)
                    ],
                    destinationNodes: [
                        requestDestinationProducts(params.selection.nodeId),
                        change('createMovement', `destinationProduct`, null)
                    ],
                    sourceProduct: [],
                    destinationProduct: [],
                    movementType: [],
                    contract: []
                };

                expect(JSON.stringify(dataService.getDispatchActions(fieldName, params))).toEqual(JSON.stringify(actions[fieldName]));
            });

        it('should get dispatch actions for IdentifiedLoss variable type ',
            () => {
                const params = {
                    selection: {
                        variableTypeId: constants.VariableType.IdentifiedLoss
                    },
                    sourceNode: [],
                    nodeId: 1
                };
                const fieldName = 'variable';

                const actions = {
                    netVolume: [],
                    unit: [],
                    variable: [
                        change('createMovement', `sourceProduct`, null),
                        change('createMovement', `destinationProduct`, null),
                        change('createMovement', `movementType`, null),
                        change('createMovement', `contract`, null),
                        requestSourceProducts(params.nodeId),
                        change('createMovement', `sourceNodes`, params.sourceNode[0]),
                        change('createMovement', `destinationNodes`, null)
                    ],
                    sourceNodes: [
                        requestSourceProducts(params.selection.nodeId),
                        change('createMovement', `sourceProduct`, null)
                    ],
                    destinationNodes: [
                        requestDestinationProducts(params.selection.nodeId),
                        change('createMovement', `destinationProduct`, null)
                    ],
                    sourceProduct: [],
                    destinationProduct: [],
                    movementType: [],
                    contract: []
                };

                expect(JSON.stringify(dataService.getDispatchActions(fieldName, params))).toEqual(JSON.stringify(actions[fieldName]));
            });

        it('should get dispatch actions for Interface variable type ',
            () => {
                const params = {
                    selection: {
                        variableTypeId: constants.VariableType.Interface
                    },
                    sourceNode: [],
                    destinationNode: [],
                    nodeId: 1
                };
                const fieldName = 'variable';

                const actions = {
                    netVolume: [],
                    unit: [],
                    variable: [
                        change('createMovement', `sourceProduct`, null),
                        change('createMovement', `destinationProduct`, null),
                        change('createMovement', `movementType`, null),
                        change('createMovement', `contract`, null),
                        requestSourceProducts(params.nodeId),
                        change('createMovement', `sourceNodes`, params.sourceNode[0]),
                        requestDestinationProducts(params.nodeId),
                        change('createMovement', `destinationNodes`, params.destinationNode[0])
                    ],
                    sourceNodes: [
                        requestSourceProducts(params.selection.nodeId),
                        change('createMovement', `sourceProduct`, null)
                    ],
                    destinationNodes: [
                        requestDestinationProducts(params.selection.nodeId),
                        change('createMovement', `destinationProduct`, null)
                    ],
                    sourceProduct: [],
                    destinationProduct: [],
                    movementType: [],
                    contract: []
                };

                expect(JSON.stringify(dataService.getDispatchActions(fieldName, params))).toEqual(JSON.stringify(actions[fieldName]));
            });

        it('should get dispatch actions for Tolerance variable type ',
            () => {
                const params = {
                    selection: {
                        variableTypeId: constants.VariableType.Tolerance
                    },
                    sourceNode: [],
                    destinationNode: [],
                    nodeId: 1
                };
                const fieldName = 'variable';

                const actions = {
                    netVolume: [],
                    unit: [],
                    variable: [
                        change('createMovement', `sourceProduct`, null),
                        change('createMovement', `destinationProduct`, null),
                        change('createMovement', `movementType`, null),
                        change('createMovement', `contract`, null),
                        change('createMovement', `sourceNodes`, null),
                        change('createMovement', `destinationNodes`, null)
                    ],
                    sourceNodes: [
                        requestSourceProducts(params.selection.nodeId),
                        change('createMovement', `sourceProduct`, null)
                    ],
                    destinationNodes: [
                        requestDestinationProducts(params.selection.nodeId),
                        change('createMovement', `destinationProduct`, null)
                    ],
                    sourceProduct: [],
                    destinationProduct: [],
                    movementType: [],
                    contract: []
                };

                expect(JSON.stringify(dataService.getDispatchActions(fieldName, params))).toEqual(JSON.stringify(actions[fieldName]));
            });

        it('should get dispatch actions for no variable type ',
            () => {
                const params = {
                    selection: {
                        nodeId: 1,
                        variableTypeId: null
                    },
                    sourceNode: [],
                    destinationNode: [],
                    nodeId: 1
                };
                const fieldName = 'variable';

                const actions = {
                    netVolume: [],
                    unit: [],
                    variable: [
                        change('createMovement', `sourceProduct`, null),
                        change('createMovement', `destinationProduct`, null),
                        change('createMovement', `movementType`, null),
                        change('createMovement', `contract`, null)
                    ],
                    sourceNodes: [
                        requestSourceProducts(params.selection.nodeId),
                        change('createMovement', `sourceProduct`, null)
                    ],
                    destinationNodes: [
                        requestDestinationProducts(params.selection.nodeId),
                        change('createMovement', `destinationProduct`, null)
                    ],
                    sourceProduct: [],
                    destinationProduct: [],
                    movementType: [],
                    contract: []
                };

                expect(JSON.stringify(dataService.getDispatchActions(fieldName, params))).toEqual(JSON.stringify(actions[fieldName]));
            });
        it('should get dispatch actions for field name netVolume ',
            () => {
                const params = {
                    selection: {
                        nodeId: 1,
                        variableTypeId: null
                    }
                };
                const fieldName = 'netVolume';

                const actions = {
                    netVolume: [],
                    unit: [],
                    variable: [
                        change('createMovement', `sourceProduct`, null),
                        change('createMovement', `destinationProduct`, null),
                        change('createMovement', `movementType`, null),
                        change('createMovement', `contract`, null)
                    ],
                    sourceNodes: [
                        requestSourceProducts(params.selection.nodeId),
                        change('createMovement', `sourceProduct`, null)
                    ],
                    destinationNodes: [
                        requestDestinationProducts(params.selection.nodeId),
                        change('createMovement', `destinationProduct`, null)
                    ],
                    sourceProduct: [],
                    destinationProduct: [],
                    movementType: [],
                    contract: []
                };

                expect(JSON.stringify(dataService.getDispatchActions(fieldName, params))).toEqual(JSON.stringify(actions[fieldName]));
            });

        it('should update Movement Inventory Ownership Object for movement',
            () => {
                const mode = constants.Modes.Create;
                const values = {
                    reasonForChange: {
                        elementId: 1,
                        name: 'name'
                    },
                    comment: 'test'
                };
                const movementInventoryOwnershipData = [
                    {
                        transactionId: 1,
                        movementType: 2,
                        operationalDate: new Date().toLocaleString(),
                        sourceNode: 1234,
                        destinationNode: 1234,
                        sourceProduct: 10002049,
                        destinationProduct: 10002049,
                        netVolume: 100,
                        unit: 'Bbl',
                        sourceProductId: 1,
                        destinationProductId: 1,
                        sourceNodeId: 1,
                        destinationNodeId: 1,
                        variableTypeId: 1,
                        tankName: 'name'
                    }
                ];

                const outputObject = [
                    {
                        transactionId: 1,
                        movementType: 2,
                        operationalDate: new Date().toLocaleString(),
                        sourceNode: 1234,
                        destinationNode: 1234,
                        sourceProduct: 10002049,
                        destinationProduct: 10002049,
                        netVolume: 100,
                        unit: 'Bbl',
                        sourceProductId: 1,
                        destinationProductId: 1,
                        sourceNodeId: 1,
                        destinationNodeId: 1,
                        variableTypeId: 1,
                        tankName: 'name',
                        reasonId: values.reasonForChange.elementId,
                        reason: values.reasonForChange.name,
                        comment: values.comment,
                        status: mode
                    }
                ];

                expect(dataService.updateMovementInventoryOwnershipObject(values, movementInventoryOwnershipData, true, mode)).toEqual(outputObject);
            });

        it('should update Movement Inventory Ownership Object for inventory',
            () => {
                const mode = constants.Modes.Update;
                const values = {
                    reasonForChange: {
                        elementId: 1,
                        name: 'name'
                    },
                    comment: 'test'
                };
                const movementInventoryOwnershipData = [
                    {
                        transactionId: 1,
                        movementType: 2,
                        operationalDate: new Date().toLocaleString(),
                        sourceNode: 1234,
                        destinationNode: 1234,
                        sourceProduct: 10002049,
                        destinationProduct: 10002049,
                        netVolume: 100,
                        unit: 'Bbl',
                        sourceProductId: 1,
                        destinationProductId: 1,
                        sourceNodeId: 1,
                        destinationNodeId: 1,
                        variableTypeId: 1,
                        tankName: 'name',
                        status: constants.Modes.Create
                    }
                ];

                const outputObject = [
                    {
                        transactionId: 1,
                        movementType: 2,
                        operationalDate: new Date().toLocaleString(),
                        sourceNode: 1234,
                        destinationNode: 1234,
                        sourceProduct: 10002049,
                        destinationProduct: 10002049,
                        netVolume: 100,
                        unit: 'Bbl',
                        sourceProductId: 1,
                        destinationProductId: 1,
                        sourceNodeId: 1,
                        destinationNodeId: 1,
                        variableTypeId: 1,
                        tankName: 'name',
                        reasonId: values.reasonForChange.elementId,
                        reason: values.reasonForChange.name,
                        comment: values.comment,
                        status: constants.Modes.Create
                    }
                ];

                expect(dataService.updateMovementInventoryOwnershipObject(values, movementInventoryOwnershipData, false, mode)).toEqual(outputObject);
            });

            it('should update Movement Inventory Ownership Object for new movement',
            () => {
                const mode = constants.Modes.Create;
                const values = {
                    reasonForChange: {
                        elementId: 1,
                        name: 'name'
                    },
                    comment: 'test'
                };
                const movementInventoryOwnershipData = [
                    {
                        transactionId: -1,
                        movementType: 2,
                        operationalDate: new Date().toLocaleString(),
                        sourceNode: 1234,
                        destinationNode: 1234,
                        sourceProduct: 10002049,
                        destinationProduct: 10002049,
                        netVolume: 100,
                        unit: 'Bbl',
                        sourceProductId: 1,
                        destinationProductId: 1,
                        sourceNodeId: 1,
                        destinationNodeId: 1,
                        variableTypeId: 1,
                        tankName: 'name'
                    }
                ];

                const outputObject = [
                    {
                        transactionId: -1,
                        movementType: 2,
                        operationalDate: new Date().toLocaleString(),
                        sourceNode: 1234,
                        destinationNode: 1234,
                        sourceProduct: 10002049,
                        destinationProduct: 10002049,
                        netVolume: 100,
                        unit: 'Bbl',
                        sourceProductId: 1,
                        destinationProductId: 1,
                        sourceNodeId: 1,
                        destinationNodeId: 1,
                        variableTypeId: 1,
                        tankName: 'name',
                        reasonId: values.reasonForChange.elementId,
                        reason: values.reasonForChange.name,
                        comment: values.comment,
                        status: mode
                    }
                ];

                expect(dataService.updateMovementInventoryOwnershipObject(values, movementInventoryOwnershipData, true, mode)).toEqual(outputObject);
            });

        it('should group By Array',
            () => {
                const inputObject = [
                    {
                        transactionId: 1,
                        sourceNode: 1234
                    },
                    {
                        transactionId: 2,
                        sourceNode: 1234
                    },
                    {
                        transactionId: 1,
                        sourceNode: 1234
                    }
                ];

                const outputObject = [
                    {
                        key: 1,
                        values: [
                            {
                                transactionId: 1,
                                sourceNode: 1234
                            },
                            {
                                transactionId: 1,
                                sourceNode: 1234
                            }
                        ]
                    },
                    {
                        key: 2,
                        values: [
                            {
                                transactionId: 2,
                                sourceNode: 1234
                            }
                        ]
                    }
                ];

                expect(dataService.groupByArray(inputObject, 'transactionId')).toEqual(outputObject);
            });

        it('should return status icon class',
            () => {
                const status = constants.OwnershipNodeStatus.LOCKED;
                const outputClass = classNames('fas m-r-1', 'fa-lock');

                expect(dataService.getStatusIconClass(status)).toEqual(outputClass);
            });
    });
