import { SubmissionError } from 'redux-form';
import { serverValidator } from '../../../common/services/serverValidator';
import { resourceProvider } from '../../../common/services/resourceProvider';
import { constants } from '../../../common/services/constants';

const ticketValidator = (function () {
    return {
        async validateCutoffDeltaCalculation(props, segment, isModal) {
            const isFromModal = isModal;
            const segmentId = segment;
            props.hideError();
            props.showLoader();
            const data = await serverValidator.validateDeltaProcessingStatus(segmentId, false);
            props.hideLoader();
            if (data.message && (data.message === constants.CalculationType.Cutoff || data.message === constants.CalculationType.Delta)) {
                if (isFromModal) {
                    props.closeModal();
                }

                props.setIsReady(false);
                const error = resourceProvider.read(`valMsg${data.message}InProgressForSegment`);

                props.showError(error, resourceProvider.read('noInventoriesMovementsForDeltaValMsgTitle'));
                throw new SubmissionError({
                    _error: error
                });
            }
        }

    };
}());

export { ticketValidator };
