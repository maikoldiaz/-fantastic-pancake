import { serverValidator } from './../../../../common/services/serverValidator';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { showLoader, hideLoader } from './../../../../common/actions';

export const asyncValidate = (values, dispatch, props) => {
    const previousErrors = props.asyncErrors;
    const rejected = true;
    const name = values.name;
    const categoryId = values.category ? values.category.categoryId : null;
    if (name) {
        if (name === props.initialValues.name && categoryId === props.initialValues.category.categoryId) {
            return Promise.resolve().then(() => {
                return previousErrors;
            });
        }

        dispatch(showLoader());
        const element = { categoryId, name, isActive: true };
        return serverValidator.validateElementName(element)
            .then(data => {
                if (data && data.body && data.body.errorCodes.length > 0) {
                    dispatch(hideLoader());
                    throw Object.assign({},
                        previousErrors,
                        rejected,
                        { name: resourceProvider.read('elementNameAlreadyExists') });
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
