import { SubmissionError } from 'redux-form';
import { serverValidator } from '../../../../common/services/serverValidator';
import { dateService } from '../../../../common/services/dateService';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';

const sapFormTicketValidator = (function () {
    return {
        async validateAsync(props, formValues) {
            const { scenario, initialDate, finalDate, segment, nodes, owner } = formValues;
            const isOperational = scenario.value === constants.ScenarioTypeId.operational;

            const startDate = isOperational
                ? dateService.parseToDate(initialDate)
                : dateService.parseToDate(formValues.periods.start);

            const endDate = isOperational
                ? dateService.parseToDate(finalDate)
                : dateService.parseToDate(formValues.periods.end);

            if (isOperational) {
                if (startDate > endDate) {
                    const error = resourceProvider.read('DATES_INCONSISTENT');
                    props.showError(error);
                    throw new SubmissionError({
                        _error: error
                    });
                }
                if (props.initSapFormTicket.validateRangeDate) {
                    const difference = dateService.getDiff(endDate, startDate, 'd');
                    if (difference >= props.initSapFormTicket.dateRange) {
                        const error = resourceProvider.readFormat('LOWER_AND_EQUAL_DAYS_FOR_PERIOD_RANGEVALIDATION', [props.initSapFormTicket.dateRange]);
                        props.showError(error);
                        throw new SubmissionError({
                            _error: error
                        });
                    }
                }
            }

            props.showLoader();
            const nodeValidationRequest = {
                ticketTypeId: constants.TicketType.LogisticMovements,
                categoryElementId: segment.elementId,
                startDate: startDate,
                endDate: endDate,
                scenarioTypeId: scenario.value,
                ownerId: owner.elementId,
                ticketNodes: nodes.filter(node => node.nodeId !== 0).map(node => ({ nodeid: node.nodeId }))
            };

            const nodesHaveTicketResponse = await serverValidator.validateSapTicket(nodeValidationRequest);
            props.hideLoader();

            if (!nodesHaveTicketResponse || nodesHaveTicketResponse.status !== 200 || nodesHaveTicketResponse.body.length === 0) {
                const error = resourceProvider.read('systemErrorMessage');
                props.showError(error);
                throw new SubmissionError({
                    _error: error
                });
            }

            const validations = nodesHaveTicketResponse.body;
            const nodeActiveInBatch = validations.find(node => node.isActiveInBatch);
            if (!utilities.isNullOrUndefined(nodeActiveInBatch)) {
                const error = nodeValidationRequest.ticketNodes.length === 0
                    ? resourceProvider.readFormat('allTicketNodesAreBeingUsed', [
                        segment.name,
                        nodeActiveInBatch.ticketStatusName
                    ])
                    : resourceProvider.readFormat('someTicketNodesAreBeingUsed', [
                        nodeActiveInBatch.nodeName,
                        nodeActiveInBatch.ticketStatusName
                    ]);

                props.showError(error);
                throw new SubmissionError({
                    _error: error
                });
            }

            props.saveSapTicketValidations(validations);
        }
    };
}());

export { sapFormTicketValidator };
