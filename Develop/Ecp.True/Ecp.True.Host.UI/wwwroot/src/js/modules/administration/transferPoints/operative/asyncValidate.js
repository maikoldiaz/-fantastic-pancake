import { serverValidator } from './../../../../common/services/serverValidator';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { showLoader, hideLoader, showError } from './../../../../common/actions';
import { constants } from '../../../../common/services/constants';

export const asyncValidate = (values, dispatch, props) => {
    const previousErrors = props.asyncErrors;
    const rejected = true;
    const transferPoint = {
        transferPoint: values.transferPoint.name,
        sourceField: values.sourceField,
        fieldWaterProduction: values.fieldWaterProduction,
        relatedSourceField: values.relatedSourceField,
        destinationNode: values.destinationNode.destinationNode.name,
        destinationNodeType: props.destinationNodeType,
        movementType: values.movementType.name,
        sourceNode: values.sourceNode.sourceNode.name,
        sourceNodeType: props.sourceNodeType,
        sourceProduct: values.sourceProduct.product.name,
        sourceProductType: values.sourceProductType.name,
        isDeleted: false
    };
    if (props.mode === constants.Modes.Create) {
        dispatch(showLoader());
        return serverValidator.validateOperativeTransferPoint(transferPoint)
            .then(data => {
                if (data && data.body && data.body.errorCodes.length > 0) {
                    dispatch(hideLoader());
                    dispatch(dispatch(showError(resourceProvider.read('transferPointOperationalExistsMessage'), true, resourceProvider.read('transferPointOperationalExistsTitle'))));
                    throw Object.assign({},
                        previousErrors,
                        rejected,
                        { transferPoint: '' });
                } else {
                    dispatch(hideLoader());
                    return previousErrors;
                }
            });
    }

    return Promise.resolve().then(() => {
        return previousErrors;
    });
};
