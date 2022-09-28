import { serverValidator } from './../../../common/services/serverValidator';
import { resourceProvider } from './../../../common/services/resourceProvider';
import { showLoader, hideLoader, showError } from './../../../common/actions';
import { constants } from './../../../common/services/constants';

export const asyncValidate = (values, dispatch, props) => {
    const previousErrors = props.asyncErrors;

    if (props.mode !== constants.Modes.Create) {
        return Promise.resolve().then(() => {
            return previousErrors;
        });
    }
    const rejected = true;
    const transformation = {
        messageTypeId: props.currentTab === 'movements' ? 'Movement' : 'Inventory',
        origin: {
            sourceNodeId: values.originSourceNode.nodeId,
            destinationNodeId: values.origin.destinationNode && values.origin.destinationNode.nodeId,
            sourceProductId: values.origin.sourceProduct.productId,
            destinationProductId: values.origin.destinationProduct && values.origin.destinationProduct.productId,
            measurementUnitId: values.origin.measurementUnit.elementId
        }
    };

    dispatch(showLoader());
    return serverValidator.validateTransformation(transformation)
        .then(data => {
            if (data.body && data.body.length > 0) {
                dispatch(hideLoader());
                dispatch(showError(resourceProvider.read('sourceTransformationDuplicate'), true, resourceProvider.read('sourceTransformationDuplicateTitle')));
                throw Object.assign({},
                    previousErrors,
                    rejected,
                    { name: resourceProvider.read('sourceTransformationDuplicate') });
            }

            dispatch(hideLoader());
            return previousErrors;
        });
};
