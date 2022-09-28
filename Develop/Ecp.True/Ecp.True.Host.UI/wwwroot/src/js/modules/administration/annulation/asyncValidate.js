import { serverValidator } from './../../../common/services/serverValidator';
import { resourceProvider } from './../../../common/services/resourceProvider';
import { showLoader, hideLoader, showError } from './../../../common/actions';
import { dataService } from './dataService';
import { constants } from '../../../common/services/constants';

export const asyncValidate = (values, dispatch, props) => {
    const previousErrors = props.asyncErrors;
    const rejected = true;
    const annulation = dataService.buildAnnulationObject(values, props.mode, props.initialValues);

    dispatch(showLoader());

    return serverValidator.validateAnnulation(annulation)
        .then(data => {
            if (data && data.body && data.body.type) {
                if (data.body.type === constants.Annulations.Sections.Source) {
                    dispatch(hideLoader());
                    dispatch(showError(resourceProvider.readFormat('annulationSourceExists', [data.body.annulation]), true, resourceProvider.read('annulationExistsTitle')));
                    throw Object.assign({},
                        previousErrors,
                        rejected,
                        { movementType: '' });
                }
                dispatch(hideLoader());
                dispatch(showError(resourceProvider.readFormat('annulationExists', [data.body.source]), true, resourceProvider.read('annulationExistsTitle')));
                throw Object.assign({},
                    previousErrors,
                    rejected,
                    { movementType: '' });
            } else {
                dispatch(hideLoader());
                return previousErrors;
            }
        });
};
