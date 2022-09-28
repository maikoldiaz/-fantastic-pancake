import { serverValidator } from './../../../common/services/serverValidator';
import { resourceProvider } from './../../../common/services/resourceProvider';
import { showLoader, hideLoader, showError } from './../../../common/actions';

export const asyncValidate = (values, dispatch, props) => {
    const previousErrors = props.asyncErrors;
    const rejected = true;
    const transaction = { blockNumber: values.blockNumber, transactionHash: values.transactionHash };

    dispatch(showLoader());

    return serverValidator.validateBlockchainTransactionExists(transaction)
        .then(data => {
            if (data && data.body.errorCodes) {
                dispatch(hideLoader());
                dispatch(showError(data.body.errorCodes[0].message, true, resourceProvider.read('error')));
                throw Object.assign({},
                    previousErrors,
                    rejected,
                    { blockchainSearch: '' });
            } else {
                dispatch(hideLoader());
                return previousErrors;
            }
        });
};
