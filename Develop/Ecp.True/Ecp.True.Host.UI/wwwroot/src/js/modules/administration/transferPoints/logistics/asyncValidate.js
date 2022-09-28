import { serverValidator } from './../../../../common/services/serverValidator';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { showLoader, hideLoader, showError } from './../../../../common/actions';

export const asyncValidate = (values, dispatch, props) => {
    const previousErrors = props.asyncErrors;
    const rejected = true;
    const transferPoint = {
        transferPoint: values.transferPoint.name,
        logisticSourceCenter: props.sourceStorageLocation,
        logisticDestinationCenter: props.destinationStorageLocation,
        sourceProduct: values.sourceProduct.product.name,
        destinationProduct: values.destinationProduct.product.name,
        isDeleted: false
    };

    dispatch(showLoader());

    return serverValidator.validateLogisticsTransferPoint(transferPoint)
        .then(data => {
            if (data && data.body && data.body.errorCodes.length > 0) {
                dispatch(hideLoader());
                dispatch(showError(resourceProvider.read('transferPointOperationalExistsMessage'), true, resourceProvider.read('transferPointOperationalExistsTitle')));
                throw Object.assign({},
                    previousErrors,
                    rejected,
                    { transferPoint: '' });
            } else {
                dispatch(hideLoader());
                return previousErrors;
            }
        });
};
