import { resourceProvider } from "../../../../common/services/resourceProvider";
import { dataService } from "../../../../modules/administration/nodeConnection/attributes/dataService";

describe('node connection data services tests', () => {

    it('should test validations for form undefined', () => {
        const data = {
            values: {
                testForm: undefined
            },
            fieldName: 'testForm',
            requiredItems: ['test', 'test1'],
            requiredMessage: resourceProvider.read('required')
        };
        const expected = {
            testForm: [
                { test: data.requiredMessage, test1: data.requiredMessage }
            ]
        };

        const result = dataService.validationsForRequiredFieldArrayItems(
            data.values,
            {
                fieldName: data.fieldName,
                requiredItems: data.requiredItems
            });

        expect(result).toEqual(expected);
    });

    it('should test validations for form - almost one item required', () => {
        const data = {
            values: {
                testForm: [undefined, {}, { test: 'test' }, { test1: 'test' }]
            },
            fieldName: 'testForm',
            requiredItems: ['test', 'test1'],
            requiredMessage: resourceProvider.read('required')
        };
        const expected = {
            testForm: [
                { test: data.requiredMessage, test1: data.requiredMessage },
                { test: data.requiredMessage, test1: data.requiredMessage },
                { test1: data.requiredMessage },
                { test: data.requiredMessage },
            ]
        };

        const result = dataService.validationsForRequiredFieldArrayItems(
            data.values,
            {
                fieldName: data.fieldName,
                requiredItems: data.requiredItems
            });

        expect(result).toEqual(expected);
    });

    it('should test Node Cost Center duplicates', () => {
        const nodeCostCenterDuplicates = [{
            costCenterId: 1,
            movementTypeId: 1,
            isActive: true
        }];

        const movementTypeAndCostCenter = [{
            costCenter: { name: 'test costCenter', elementId: 1 },
            movementType: { name: 'test movementType', elementId: 1 }
        }];

        const expected = {
            result: [{
                costCenterName: 'test costCenter',
                movementTypeName: 'test movementType',
                status: true
            }],
            totalUnSaved: 1,
            totalToSaved: 1
        };

        const result = dataService.getDuplicateNodeCostCenter(nodeCostCenterDuplicates, movementTypeAndCostCenter);
        expect(result).toEqual(expected);

    });


    it('should test Node connection duplicates', () => {
        const nodeConnectionDuplicates = [{
            sourceNodeId: 1,
            destinationNodeId: 2
        }];

        const nodeConnections = [{
            sourceNode: { nodeId: 1, name: 'test source node' },
            destinationNode: { nodeId: 2, name: 'test destination node' },
            sourceSegment: { name: 'test source segment' },
            destinationSegment: { name: 'test destination segment' }

        }];

        const expected = {
            result: [{
                sourceNodeName: 'test source node',
                destinationNodeName: 'test destination node',
                sourceSegmentName: 'test source segment',
                destinationSegmentName: 'test destination segment'
            }],
            totalUnSaved: 1,
            totalToSaved: 1
        };


        const result = dataService.getDuplicateNodeConnection(nodeConnectionDuplicates, nodeConnections);
        expect(result).toEqual(expected);

    });

    it('should test build initial values', () => {
        const data = {
            sourceNodeId: 1,
            destinationNodeId: 2,
            movementTypeId: 1,
            costCenterId: 1,
            nodeCostCenterId: 1,
            isActive: true,
            rowVersion: 'test',
            sourceNode: { name: 'test' },
            destinationNode: { name: 'test' },
            movementTypeCategoryElement: { name: 'test' },
            costCenterCategoryElement: { name: 'test' }
        };
        const expected = {
            sourceNode: {
                sourceNodeId: 1,
                name: 'test'
            },
            destinationNode: {
                destinationNodeId: 2,
                name: 'test'
            },
            movementType: {
                movementTypeId: 1,
                name: 'test'
            },
            costCenter: {
                costCenterId: 1,
                name: 'test'
            },
            isActive: true,
            nodeCostCenterId: 1,
            rowVersion: 'test'
        };

        const result = dataService.buildInitialValues(data);

        expect(result).toEqual(expected);


    });

    it('should test build node cost center Object', () => {

        const data = {
            costCenter: { costCenterId: 1 },
            sourceNode: { sourceNodeId: 1 },
            destinationNode: { destinationNodeId: 2 },
            movementType: { movementTypeId: 1 },
            isActive: true,
            nodeCostCenterId: 1,
            rowVersion: 'test'
        };

        const expected = {
            sourceNodeId: 1,
            destinationNodeId: 2,
            movementTypeId: 1,
            costCenterId: 1,
            isActive: true,
            nodeCostCenterId: 1,
            rowVersion: 'test'
        };

        const result = dataService.buildNodeCostCenterObject(data);

        expect(result).toEqual(expected);

    });


    it('should test build node cost center Object - costCenterId does not exists', () => {

        const data = {
            costCenter: { elementId: 1 },
            sourceNode: { sourceNodeId: 1 },
            destinationNode: { destinationNodeId: 2 },
            movementType: { movementTypeId: 1 },
            isActive: true,
            nodeCostCenterId: 1,
            rowVersion: 'test'
        };

        const expected = {
            sourceNodeId: 1,
            destinationNodeId: 2,
            movementTypeId: 1,
            costCenterId: 1,
            isActive: true,
            nodeCostCenterId: 1,
            rowVersion: 'test'
        };

        const result = dataService.buildNodeCostCenterObject(data);

        expect(result).toEqual(expected);

    });


});