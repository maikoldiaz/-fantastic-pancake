import { ownersService } from './../../../../modules/transportBalance/nodeOwnership/services/ownersService';
import { requestOwnersForMovement, requestOwnersForInventory, setMovementInventoryOwnershipData } from '../../../../modules/transportBalance/nodeOwnership/actions';
import { showError } from '../../../../common/actions';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dataService } from './../../../../modules/transportBalance/nodeOwnership/services/dataService';
import { constants } from '../../../../common/services/constants';

describe('owners service', () => {
    it('should get the owners data for movementOwnershipDetails', () => {
        const props = {
            movementInventoryOwnershipData: [{ sourceNodeId: 78, destinationNodeId: 92, sourceProductId: 178, destinationProductId: 189, variableTypeId: 5 }]
        };
        expect(JSON.stringify(ownersService.getOwnersData(props, 'movementOwnershipDetails'))).toEqual(JSON.stringify(requestOwnersForMovement(78, 92, 178)));
    });
    it('should get the owners data for sourceNodeId greater than zero for movementOwnershipDetails', () => {
        const props = {
            movementInventoryOwnershipData: [{ sourceNodeId: 99, destinationNodeId: 0, sourceProductId: 178, destinationProductId: 189, variableTypeId: 5 }]
        };
        expect(JSON.stringify(ownersService.getOwnersData(props, 'movementOwnershipDetails'))).toEqual(JSON.stringify(requestOwnersForInventory(99, 178)));
    });
    it('should get the owners data for  destinaionNodeId greater than zero for movementOwnershipDetails', () => {
        const props = {
            movementInventoryOwnershipData: [{ sourceNodeId: 0, destinationNodeId: 99, sourceProductId: 178, destinationProductId: 189, variableTypeId: 5 }]
        };
        expect(JSON.stringify(ownersService.getOwnersData(props, 'movementOwnershipDetails'))).toEqual(JSON.stringify(requestOwnersForInventory(99, 189)));
    });
    it('should get the owners data for inventoryOwnershipDetails', () => {
        const props = {
            movementInventoryOwnershipData: [{ sourceNodeId: 10, destinationNodeId: 99, sourceProductId: 178, destinationProductId: 189, variableTypeId: 5 }]
        };
        expect(JSON.stringify(ownersService.getOwnersData(props, 'inventoryOwnershipDetails'))).toEqual(JSON.stringify(requestOwnersForInventory(10, 178)));
    });
    it('should add the owners data for movementOwnershipDetails', () => {
        const props = {
            movementInventoryOwnershipData: [{ sourceNodeId: 10, destinationNodeId: 99, sourceProductId: 178, destinationProductId: 189, variableTypeId: 1, ownerId: 25 }],
            inventoryOwners: [{ ownerId: 25 }]
        };
        expect(JSON.stringify(ownersService.addOwnersData(props, 'movementOwnershipDetails')))
            .toEqual(JSON.stringify(showError(resourceProvider.read('onOwnersAvailableMessage'), true, resourceProvider.read('onOwnersAvailableTitle'))));
    });
    it('should add the owners data for inventoryOwnershipDetails', () => {
        const props = {
            movementInventoryOwnershipData: [{ sourceNodeId: 10, destinationNodeId: 99, sourceProductId: 178, destinationProductId: 189, variableTypeId: 1, ownerId: 25 }],
            inventoryOwners: [{ ownerId: 25, owner: { name: 'testName', color: 'testColor' } }]
        };
        expect(JSON.stringify(ownersService.addOwnersData(props, 'inventoryOwnershipDetails')))
            .toEqual(JSON.stringify(showError(resourceProvider.read('noOwnersAvailableMessageForInventory'), true, resourceProvider.read('noOwnersAvailableTitle'))));
    });
    it('should add the owners data for movementOwnershipDetails with ownersData', () => {
        const props = {
            movementInventoryOwnershipData: [{ sourceNodeId: 10, destinationNodeId: 99, sourceProductId: 178, destinationProductId: 189, variableTypeId: 1, netVolume: 200, ownerId: 30 }],
            inventoryOwners: [{ ownerId: 25, owner: { name: 'testName', color: 'testColor' }, ownershipPercentage: 50 }]
        };
        const movementInventoryOwnershipDataTest = [{ sourceNodeId: 10, destinationNodeId: 99, sourceProductId: 178, destinationProductId: 189, variableTypeId: 1, netVolume: 200, ownerId: 30 }];
        let ownershipInventoryMovementDataObject = dataService.buildMovementInventoryOwnershipObject(movementInventoryOwnershipDataTest[0], true);
        ownershipInventoryMovementDataObject = Object.assign({}, ownershipInventoryMovementDataObject, {
            ownershipVolume: 50 * movementInventoryOwnershipDataTest[0].netVolume / 100,
            ownershipPercentage: parseInt(50, 10),
            ownershipFunction: 'Propiedad Manual',
            ownerId: 25,
            ownerName: 'testName',
            status: constants.Modes.Create,
            color: 'testColor' || constants.DefaultColorCode,
            ruleVersion: 1
        });
        const ownershipInventoryMovementDataTest = movementInventoryOwnershipDataTest;
        ownershipInventoryMovementDataTest.push(ownershipInventoryMovementDataObject);
        expect(JSON.stringify(ownersService.addOwnersData(props, 'movementOwnershipDetails'))).toEqual(JSON.stringify(setMovementInventoryOwnershipData(ownershipInventoryMovementDataTest)));
    });

    it('should add the owners data for inventoryOwnershipDetails with ownersData', () => {
        const props = {
            movementInventoryOwnershipData: [{ sourceNodeId: 10, destinationNodeId: 99, sourceProductId: 178, destinationProductId: 189, variableTypeId: 1, netVolume: 200, ownerId: 30 }],
            inventoryOwners: [{ ownerId: 25, owner: { name: 'testName', color: 'testColor' }, ownershipPercentage: 50 }]
        };
        const movementInventoryOwnershipDataTest = [{ sourceNodeId: 10, destinationNodeId: 99, sourceProductId: 178, destinationProductId: 189, variableTypeId: 1, netVolume: 200, ownerId: 30 }];
        let ownershipInventoryMovementDataObject = dataService.buildMovementInventoryOwnershipObject(movementInventoryOwnershipDataTest[0], false);
        ownershipInventoryMovementDataObject = Object.assign({}, ownershipInventoryMovementDataObject, {
            ownershipVolume: 50 * movementInventoryOwnershipDataTest[0].netVolume / 100,
            ownershipPercentage: parseInt(50, 10),
            ownershipFunction: 'Propiedad Manual',
            ownerId: 25,
            ownerName: 'testName',
            status: constants.Modes.Create,
            color: 'testColor' || constants.DefaultColorCode,
            ruleVersion: 1
        });
        const ownershipInventoryMovementDataTest = movementInventoryOwnershipDataTest;
        ownershipInventoryMovementDataTest.push(ownershipInventoryMovementDataObject);
        expect(JSON.stringify(ownersService.addOwnersData(props, 'inventoryOwnershipDetails'))).toEqual(JSON.stringify(setMovementInventoryOwnershipData(ownershipInventoryMovementDataTest)));
    });
});
