import { SubmissionError } from 'redux-form';
import { serverValidator } from '../../../../common/services/serverValidator';
import { utilities } from '../../../../common/services/utilities';
import { dateService } from '../../../../common/services/dateService';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';

const ticketValidator = (function () {
    return {
        async validateAsync(props, initialDate, finalDate, segmentId) {
            const startDate = dateService.parseFieldToISOString(initialDate);
            const endDate = dateService.parseFieldToISOString(finalDate);
            if (dateService.compare(startDate, endDate, true) === 1) {
                const error = resourceProvider.read('invalidDateRange');
                props.showError(error);
                throw new SubmissionError({
                    _error: error
                });
            }
            if (utilities.isNullOrUndefined(props.systemConfig.controlLimit)) {
                const error = resourceProvider.read('balanceControlLimitRequired');
                props.showError(error);
                throw new SubmissionError({
                    _error: resourceProvider.read('balanceControlLimitRequired')
                });
            }

            if (utilities.isNullOrUndefined(props.systemConfig.standardUncertaintyPercentage)) {
                const error = resourceProvider.read('balanceStandardUncertaintyRequired');
                props.showError(error);
                throw new SubmissionError({
                    _error: error
                });
            }

            if (utilities.isNullOrUndefined(props.systemConfig.acceptableBalancePercentage)) {
                const error = resourceProvider.read('balanceAcceptableBalanceRequired');
                props.showError(error);
                throw new SubmissionError({
                    _error: error
                });
            }

            props.showLoader();
            const cutoffTicket = {
                categoryElementId: segmentId,
                startDate: startDate,
                endDate: endDate,
                ticketTypeId: constants.TicketType.Cutoff
            };

            const cutoffTicketExists = await serverValidator.validateUniqueSegmentTicket(cutoffTicket);
            props.hideLoader();
            if (cutoffTicketExists.body) {
                const error = resourceProvider.read('cutoffAlreadyRunning');
                props.showError(error);
                throw new SubmissionError({
                    _error: error
                });
            }

            props.showLoader();
            const deltaTicketExists = await serverValidator.validateDeltaTicket(segmentId);
            props.hideLoader();
            if (deltaTicketExists) {
                const error = resourceProvider.read('deltaAlreadyRunning');
                props.showError(error);
                throw new SubmissionError({
                    _error: error
                });
            }

            const categoryElementId = segmentId;
            const ticket = { startDate, endDate, categoryElementId };

            props.showLoader();
            const data = await serverValidator.validateTicket(ticket);
            props.hideLoader();
            if (!data.body) {
                const error = resourceProvider.read('movementInventoryNotExists');
                props.showError(error);
                throw new SubmissionError({
                    _error: error
                });
            }
        }
    };
}());

export { ticketValidator };
