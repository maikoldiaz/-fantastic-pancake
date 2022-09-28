import { serverValidator } from './../../../../common/services/serverValidator';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { showLoader, hideLoader } from './../../../../common/actions';
import { utilities } from './../../../../common/services/utilities.js';

export const asyncValidate = (values, dispatch, props) => {
    const previousErrors = props.asyncErrors;
    const rejected = true;
    const name = values.name;

    if (!utilities.isNullOrUndefined(name)) {
        if (name === props.initialValues.existingName) {
            return Promise.resolve().then(() => {
                return previousErrors;
            });
        }

        dispatch(showLoader());
        return serverValidator.validateNodeName(name)
            .then(data => {
                if (data && data.errorCodes.length > 0) {
                    dispatch(hideLoader());
                    throw Object.assign({},
                        previousErrors,
                        rejected,
                        { name: resourceProvider.read('nodeNameAlreadyExists') });
                }

                dispatch(hideLoader());
                return previousErrors;
            });
    }

    return Promise.resolve().then(() => {
        return previousErrors;
    });
};
