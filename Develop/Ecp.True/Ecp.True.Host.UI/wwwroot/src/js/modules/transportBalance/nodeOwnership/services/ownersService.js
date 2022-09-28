import { requestOwnersForMovement, requestOwnersForInventory, setMovementInventoryOwnershipData } from '../actions';
import { constants } from '../../../../common/services/constants';
import { dataService } from './dataService';
import { showError } from '../../../../common/actions';
import { resourceProvider } from '../../../../common/services/resourceProvider';

const ownersService = (function () {
    const movementOwnershipDetails = 'movementOwnershipDetails';
    const inventoryOwnershipDetails = 'inventoryOwnershipDetails';

    const getOwners = (props, ownershipDetails) => {
        if (ownershipDetails === movementOwnershipDetails) {
            const sourceNodeId = props.movementInventoryOwnershipData[0].sourceNodeId;
            const destinationNodeId = props.movementInventoryOwnershipData[0].destinationNodeId;
            const sourceProductId = props.movementInventoryOwnershipData[0].sourceProductId;
            const destinationProductId = props.movementInventoryOwnershipData[0].destinationProductId;

            if (sourceNodeId > 0 && destinationNodeId > 0) {
                return requestOwnersForMovement(sourceNodeId, destinationNodeId, sourceProductId);
            } else if (sourceNodeId > 0) {
                return requestOwnersForInventory(sourceNodeId, sourceProductId);
            }
            return requestOwnersForInventory(destinationNodeId, destinationProductId);
        }
        return requestOwnersForInventory(props.movementInventoryOwnershipData[0].sourceNodeId, props.movementInventoryOwnershipData[0].sourceProductId);
    };

    const addOwners = (props, ownershipDetails) => {
        let newOwnerExist = false;
        const ownershipInventoryMovementData = props.movementInventoryOwnershipData;
        let ownersData = null;
        if (ownershipDetails === movementOwnershipDetails) {
            const variableTypeId = props.movementInventoryOwnershipData[0].variableTypeId;
            if (variableTypeId === constants.VariableType.Input ||
                variableTypeId === constants.VariableType.Output) {
                ownersData = props.movementOwners;
            } else {
                ownersData = props.inventoryOwners;
            }
        }
        if (ownershipDetails === inventoryOwnershipDetails) {
            ownersData = props.inventoryOwners;
        }

        if (ownersData && ownersData.length > 0) {
            ownersData.forEach(x => {
                if (!ownershipInventoryMovementData.find(item => item.ownerId === x.ownerId)) {
                    newOwnerExist = true;
                    let ownershipInventoryMovementDataObject = ownershipDetails === movementOwnershipDetails ?
                        dataService.buildMovementInventoryOwnershipObject(props.movementInventoryOwnershipData[0], true) :
                        dataService.buildMovementInventoryOwnershipObject(props.movementInventoryOwnershipData[0], false);
                    ownershipInventoryMovementDataObject = Object.assign({}, ownershipInventoryMovementDataObject, {
                        ownershipVolume: x.ownershipPercentage * props.movementInventoryOwnershipData[0].netVolume / 100,
                        ownershipPercentage: parseInt(x.ownershipPercentage, 10),
                        ownershipFunction: 'Propiedad Manual',
                        ownerId: x.ownerId,
                        ownerName: x.owner.name,
                        status: constants.Modes.Create,
                        color: x.owner.color || constants.DefaultColorCode,
                        ruleVersion: 1
                    });
                    ownershipInventoryMovementData.push(ownershipInventoryMovementDataObject);
                }
            });
        }

        if (!ownersData || !newOwnerExist) {
            return ownershipDetails === movementOwnershipDetails ?
                showError(resourceProvider.read('onOwnersAvailableMessage'), true, resourceProvider.read('onOwnersAvailableTitle')) :
                showError(resourceProvider.read('noOwnersAvailableMessageForInventory'), true, resourceProvider.read('noOwnersAvailableTitle'));
        }

        return setMovementInventoryOwnershipData(ownershipInventoryMovementData);
    };

    return {
        getOwnersData: (props, ownershipDetails) => {
            return getOwners(props, ownershipDetails);
        },

        addOwnersData: (props, ownershipDetails) => {
            return addOwners(props, ownershipDetails);
        }

    };
}());

export { ownersService };
