import { change } from 'redux-form';
import {
    getTransferDestinationNodes,
    getNodeStorageLocations,
    getTransferProducts,
    getLogisticCenter,
    setStorageLocation,
    resetOnDestinationNodeChange,
    resetOnSourceNodeChange
} from './logistics/actions';
import { utilities } from '../../../common/services/utilities';

const actionProvider = (function () {
    const getSourceNodeActions = value => {
        const commonActions = [
            change('createTransferPointLogistics', 'destinationNode', null),
            change('createTransferPointLogistics', 'sourceProduct', null),
            change('createTransferPointLogistics', 'sourceStorageLocation', null),
            change('createTransferPointLogistics', 'destinationProduct', null),
            change('createTransferPointLogistics', 'destinationStorageLocation', null),
            resetOnSourceNodeChange()
        ];
        if (!utilities.isNullOrUndefined(value)) {
            return [
                ...commonActions,
                getTransferDestinationNodes(value.sourceNodeId),
                getNodeStorageLocations(value.sourceNodeId, true),
                getTransferProducts(value.sourceNodeId, true),
                getLogisticCenter(value.sourceNode.name, true)
            ];
        }
        return commonActions;
    };

    const getDestinationNodeActions = value => {
        const commonActions = [
            change('createTransferPointLogistics', 'destinationProduct', null),
            change('createTransferPointLogistics', 'destinationStorageLocation', null),
            resetOnDestinationNodeChange()
        ];
        if (!utilities.isNullOrUndefined(value)) {
            return [
                ...commonActions,
                getNodeStorageLocations(value.destinationNode.nodeId, false),
                getTransferProducts(value.destinationNode.nodeId, false),
                getLogisticCenter(value.destinationNode.name, false)
            ];
        }
        return commonActions;
    };

    const getlogisticsActions = (fieldName, value) => {
        if (fieldName === 'sourceNode') {
            return getSourceNodeActions(value);
        } else if (fieldName === 'destinationNode') {
            return getDestinationNodeActions(value);
        } else if ((fieldName === 'sourceStorageLocation' || fieldName === 'destinationStorageLocation') && !utilities.isNullOrUndefined(value)) {
            return [ setStorageLocation(value, fieldName === 'sourceStorageLocation') ];
        }
        return [];
    };

    return {
        getlogisticsActions: (fieldName, value) => {
            return getlogisticsActions(fieldName, value);
        }
    };
}());

export { actionProvider };
