import { SubmissionError } from 'redux-form';
import { serverValidator } from '../../../common/services/serverValidator';
import { resourceProvider } from '../../../common/services/resourceProvider';
import { constants } from '../../../common/services/constants';
import { OfficialDeltaInProgressMessage } from './components/progressMessage.jsx';

const ticketValidator = (function () {
    return {
        async validateOfficialDeltaInProgress(props, segment, isModal) {
            const isFromModal = isModal;
            const segmentId = segment;
            props.hideError();
            props.showLoader();

            const data = await serverValidator.validateOfficialDeltaProcessingStatus(segmentId);
            props.hideLoader();
            if (data.message && (data.message === constants.CalculationType.OfficialDelta)) {
                if (isFromModal) {
                    props.closeModal();
                }
                props.setIsValid(true);
                const error = resourceProvider.read(`officialDeltaInProgress`);
                props.showErrorComponent(OfficialDeltaInProgressMessage, resourceProvider.read('officialDeltaCannotContinue'));
                throw new SubmissionError({ _error: error });
            }
        },
        async validatePreviousOfficialPeriod(props, ticket, isModal) {
            const isFromModal = isModal;
            props.hideError();
            props.showLoader();

            const data = await serverValidator.validatePreviousOfficialPeriod(ticket);
            props.hideLoader();
            if (data.body) {
                if (isFromModal) {
                    props.closeModal();
                }
                props.setIsValid(true);
                const error = resourceProvider.read(`officialDeltaUnapprovedNodes`);
                props.showError(error, resourceProvider.read('officialDeltaCannotContinue'));
                throw new SubmissionError({ _error: error });
            }
        }

    };
}());

export { ticketValidator };
