import { resourceProvider } from '../../../../common/services/resourceProvider.js';
import { constants } from '../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';
import { SubmissionError } from 'redux-form';
import { showError } from '../../../../common/actions';
import { dispatcher } from '../../../../common/store/dispatcher';

const movementValidator = (function () {
    const showAndThrowError = (errorMessage, errorTitle = '') => {
        const error = resourceProvider.read(errorMessage);
        dispatcher.dispatch(showError(error, true, resourceProvider.read(errorTitle)));
        throw new SubmissionError({ _error: error });
    };

    const sumPercentage = (total, num) => {
        return parseFloat(total ? parseFloat(total).toFixed(2) : 0) + parseFloat(num ? parseFloat(num).toFixed(2) : 0);
    };

    const validateMovement = (formValues, movementInventoryOwnershipData, errorCheckObject) => {
        if (movementInventoryOwnershipData.some(x => utilities.isNullOrWhitespace(x.ownershipVolume))) {
            showAndThrowError('inventoryVolumeError');
        }
        if (movementInventoryOwnershipData.some(x => utilities.isNullOrWhitespace(x.ownershipPercentage))) {
            showAndThrowError('inventoryPercentageError');
        }

        if ((formValues.variable.variableTypeId === constants.VariableType.Tolerance || formValues.variable.variableTypeId === constants.VariableType.UnidentifiedLoss)) {
            if ((errorCheckObject.sourceNodeId > 0 && errorCheckObject.destinationNodeId > 0) || (errorCheckObject.sourceProductId > 0 && errorCheckObject.destinationProductId > 0) ||
                (errorCheckObject.sourceNodeId > 0 && errorCheckObject.sourceProductId <= 0) || (errorCheckObject.destinationNodeId > 0 && errorCheckObject.destinationProductId <= 0)) {
                showAndThrowError('selectSourceOrDestinationNode');
            }
        }

        const decimalLength = formValues.netVolume.split('.').length > 1 && formValues.netVolume.split('.')[1].length > 2;

        if (decimalLength || parseFloat(formValues.netVolume) <= 0) {
            showAndThrowError('allowPositiveAndTwoDecimals');
        }

        if (movementInventoryOwnershipData.length > 0) {
            const totalPercentage = movementInventoryOwnershipData.map(a=>a.ownershipPercentage);
            if (utilities.parseFloat(totalPercentage.reduce(sumPercentage)) !== utilities.parseFloat(100)) {
                showAndThrowError('percentageValidationMessage');
            }

            const totalVolume = movementInventoryOwnershipData.reduce((a, b) => parseFloat(a) + parseFloat(b.ownershipVolume ? parseFloat(b.ownershipVolume).toFixed(2) : 0), 0);
            if (parseFloat(totalVolume).toFixed(2) !== parseFloat(movementInventoryOwnershipData[0].netVolume).toFixed(2)) {
                showAndThrowError('movementOwnershipError');
            }
        } else if (formValues.variable.variableTypeId === constants.VariableType.Input || formValues.variable.variableTypeId === constants.VariableType.Output) {
            showAndThrowError('nodeConnectionOwnerNotAvailable', 'ownerNotAvailable');
        } else {
            showAndThrowError('nodeProductOwnerNotAvailable', 'ownerNotAvailable');
        }

        return true;
    };

    return {
        validateMovement: (formValues, movementInventoryOwnershipData, errorCheckObject) => {
            return validateMovement(formValues, movementInventoryOwnershipData, errorCheckObject);
        }
    };
}());

export { movementValidator };
