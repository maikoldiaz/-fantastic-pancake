import { serverValidator } from './../../../../common/services/serverValidator';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { showLoader, hideLoader } from './../../../../common/actions';

export const asyncValidate = (values, dispatch, props) => {
    const previousErrors = props.asyncErrors;
    const rejected = true;
    const name = values.name;
    if (name) {
        if (name === props.initialValues.name) {
            return Promise.resolve().then(() => {
                return previousErrors;
            });
        }

        dispatch(showLoader());
        return serverValidator.validateCategoryName(name)
            .then(data => {
                if (data && data.errorCodes.length > 0) {
                    dispatch(hideLoader());
                    throw Object.assign({},
                        previousErrors,
                        rejected,
                        { name: resourceProvider.read('categoryNameAlreadyExists') });
                }

                dispatch(hideLoader());
                return previousErrors;
            });
    }

    return Promise.resolve().then(() => {
        return previousErrors;
    });
};
