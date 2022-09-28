import { SubmissionError } from 'redux-form';
import { serverValidator } from '../../../common/services/serverValidator';
import { resourceProvider } from '../../../common/services/resourceProvider';
import { hideLoader, hideNotification, showError, showLoader } from '../../../common/actions';
import { dispatcher } from '../../../common/store/dispatcher';

const blockValidator = (function () {
    return {
        async validateBlockRange(request) {
            dispatcher.dispatch(hideNotification());
            dispatcher.dispatch(showLoader());

            const data = await serverValidator.validateBlockRangeExists(request);
            dispatcher.dispatch(hideLoader());
            if (data && data.body.errorCodes) {
                dispatcher.dispatch(hideLoader());
                dispatcher.dispatch(showError(data.body.errorCodes[0].message, false, resourceProvider.read('error')));
                throw new SubmissionError({ _error: data.body.errorCodes[0].message });
            }
        }
    };
}());

export { blockValidator };
