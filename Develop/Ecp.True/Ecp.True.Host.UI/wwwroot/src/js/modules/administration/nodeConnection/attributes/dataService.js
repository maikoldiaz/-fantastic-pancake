import { resourceProvider } from '../../../../common/services/resourceProvider';

const dataService = (function () {
    return {
        buildNodeCostCenterObject: values => {
            let costCenterIdValue = values.costCenter.costCenterId;
            if (!values.costCenter.costCenterId) {
                costCenterIdValue = values.costCenter.elementId;
            }
            return {
                sourceNodeId: values.sourceNode.sourceNodeId,
                destinationNodeId: values.destinationNode.destinationNodeId,
                movementTypeId: values.movementType.movementTypeId,
                costCenterId: costCenterIdValue,
                isActive: values.isActive,
                nodeCostCenterId: values.nodeCostCenterId,
                rowVersion: values.rowVersion
            };
        },

        buildInitialValues: row => {
            return {
                sourceNode: {
                    sourceNodeId: row.sourceNodeId,
                    name: row.sourceNode.name
                },
                destinationNode: {
                    destinationNodeId: row.destinationNodeId,
                    name: row.destinationNode === null ? null : row.destinationNode.name
                },
                movementType: {
                    movementTypeId: row.movementTypeId,
                    name: row.movementTypeCategoryElement.name
                },
                costCenter: {
                    costCenterId: row.costCenterId,
                    name: row.costCenterCategoryElement.name
                },
                isActive: row.isActive,
                nodeCostCenterId: row.nodeCostCenterId,
                rowVersion: row.rowVersion
            };
        },

        getDuplicateNodeCostCenter: (nodeCostCenterDuplicates, movementTypeAndCostCenter) => {
            const totalUnSaved = nodeCostCenterDuplicates.length;
            const totalToSaved = movementTypeAndCostCenter.length;

            /**
             * Positions:
             * [0] -> Cost center id
             * [1] -> Movement Type id
             * [2] -> Is active
             */
            const flatNodeCostCenterDuplicates = nodeCostCenterDuplicates.map(costCenter => [costCenter.costCenterId, costCenter.movementTypeId, costCenter.isActive]);

            const result = flatNodeCostCenterDuplicates
                .map(costCenter => movementTypeAndCostCenter
                    .filter(costCenterFiltered => costCenterFiltered.costCenter.elementId === costCenter[0] && costCenterFiltered.movementType.elementId === costCenter[1])
                    .map(r => ({
                        costCenterName: r.costCenter.name,
                        movementTypeName: r.movementType.name,
                        status: costCenter[2]
                    }))
                )
                .flat();

            return {
                result,
                totalUnSaved,
                totalToSaved
            };
        },

        getDuplicateNodeConnection: (nodeConnectionDuplicates, nodeConnections) => {
            const totalUnSaved = nodeConnectionDuplicates.length;
            const totalToSaved = nodeConnections.length;

            /**
             * Positions:
             * [0] -> source node  id
             * [1] -> destination node id
             */
            const flatNodeConnectionDuplicates = nodeConnectionDuplicates.map(nc => [nc.sourceNodeId, nc.destinationNodeId]);

            const result = flatNodeConnectionDuplicates
                .map(flatNC => nodeConnections
                    .filter(nodeConnectionFilter => nodeConnectionFilter.sourceNode.nodeId === flatNC[0] && nodeConnectionFilter.destinationNode.nodeId === flatNC[1])
                    .map(r => ({
                        sourceNodeName: r.sourceNode.name,
                        sourceSegmentName: r.sourceSegment.name,
                        destinationNodeName: r.destinationNode.name,
                        destinationSegmentName: r.destinationSegment.name
                    }))
                )
                .flat();

            return {
                result,
                totalUnSaved,
                totalToSaved
            };
        },

        validationsForRequiredFieldArrayItems: (values, { fieldName, requiredItems }) => {
            const errors = {};
            const getRequiredItems = (item = {}) => {
                const result = {};
                requiredItems.forEach(element => {
                    const exists = item[element];
                    if (!exists) {
                        result[element] = resourceProvider.read('required');
                    }
                });
                return result;
            };
            const fieldNameVal = values[fieldName];
            if (!fieldNameVal) {
                errors[fieldName] = [getRequiredItems()];
            } else {
                const result = [];
                values[fieldName].forEach(v => {
                    result.push(getRequiredItems(v));
                });
                errors[fieldName] = result;
            }
            return errors;
        }
    };
}());

export { dataService };
