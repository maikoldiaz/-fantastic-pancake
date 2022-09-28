import { apiService } from '../../../common/services/apiService';
import { receiveGridData } from '../../../common/components/grid/actions';
import { utilities } from '../../../common/services/utilities';
import { resourceProvider } from '../../../common/services/resourceProvider';

export const REQUEST_MOVEMENTS = 'REQUEST_MOVEMENTS';
export const INIT_MOVEMENTS_CONFIRMATION = 'INIT_MOVEMENTS_CONFIRMATION';
export const CONFIRM_SENT_SAP = 'CONFIRM_SENT_SAP';
export const INIT_SENTTOSAP_PROCESS = 'INIT_SENTTOSAP_PROCESS';
export const REQUEST_SEND_MOVEMENTS = 'REQUEST_SEND_MOVEMENTS';
export const COMPLETE_SENTTOSAP_PROCESS = 'COMPLETE_SENTTOSAP_PROCESS';
export const RECEIVE_FAILURE_SENTTOSAP_PROCESS = 'RECEIVE_FAILURE_SENTTOSAP_PROCESS';
export const SAVE_CONFIRM_WIZARD_DATA = 'SAVE_CONFIRM_WIZARD_DATA';
export const RESET_CONFIRM_WIZARD_DATA = 'RESET_CONFIRM_WIZARD_DATA';
export const REQUEST_NODES_IN_TICKET = 'REQUEST_NODES_IN_TICKET';
export const RECEIVE_FAILURE_NODES_IN_TICKET = 'RECEIVE_FAILURE_NODES_IN_TICKET';
export const RECEIVE_NODES_IN_TICKET = 'RECEIVE_NODES_IN_TICKET';
export const REQUEST_GET_TICKET = 'REQUEST_GET_TICKET';
export const RECEIVE_GET_TICKET = 'RECEIVE_GET_TICKET';
export const RECEIVE_FAILURE_GET_TICKET = 'RECEIVE_FAILURE_GET_TICKET';
export const CONFIRM_CANCEL_BATCH = 'CONFIRM_CANCEL_BATCH';
export const REQUEST_CANCEL_BATCH = 'REQUEST_CANCEL_BATCH';
export const COMPLETE_CANCEL_BATCH_PROCESS = 'COMPLETE_CANCEL_BATCH_PROCESS';
export const RECEIVE_FAILURE_CANCEL_BATCH_PROCESS = 'RECEIVE_FAILURE_CANCEL_BATCH_PROCESS';
export const REQUEST_SAP_OFFICIAL_PERIODS = 'REQUEST_SAP_OFFICIAL_PERIODS';
export const RECEIVE_SAP_OFFICIAL_PERIODS = 'RECEIVE_SAP_OFFICIAL_PERIODS';
export const RESET_SAP_OFFICIAL_PERIODS = 'RESET_SAP_OFFICIAL_PERIODS';
export const CLEAR_SAP_SEARCH_NODES = 'CLEAR_SAP_SEARCH_NODES';
export const RECEIVE_SAP_SEARCH_NODES = 'RECEIVE_SAP_SEARCH_NODES';
export const REQUEST_SAP_SEARCH_NODES = 'REQUEST_SAP_SEARCH_NODES';
export const RESET_SAP_FORM_DATA = 'RESET_SAP_FORM_DATA';
export const SET_SAP_TICKET_INFO = 'SET_SAP_TICKET_INFO';
export const RECEIVE_SAP_TICKET_VALIDATIONS = 'RECEIVE_SAP_TICKET_VALIDATIONS';
export const RESET_SAP_TICKET_VALIDATIONS = 'RESET_SAP_TICKET_VALIDATIONS';
export const RECEIVE_SAP_TICKET = 'RECEIVE_SAP_TICKET';
export const RECEIVE_SAP_TICKET_FAILURE = 'RECEIVE_SAP_TICKET_FAILURE';
export const REQUEST_SAP_TICKET = 'REQUEST_SAP_TICKET';
export const REQUEST_FORWARD_MOVEMENTS = 'REQUEST_FORWARD_MOVEMENTS';
export const COMPLETE_FORWARD_PROCESS = 'COMPLETE_FORWARD_PROCESS';

export const initMovementsConfirmation = (name, selectedMovements, countTotalMovements) => {
    return {
        type: INIT_MOVEMENTS_CONFIRMATION,
        name,
        selectedMovements,
        countTotalMovements
    };
};

export const confirmSentToSap = () => {
    return {
        type: CONFIRM_SENT_SAP
    };
};

export const initSentToSap = () => {
    return {
        type: INIT_SENTTOSAP_PROCESS
    };
};

export const requestMovements = (name, ticketId, transformData = data => data) => {
    return {
        type: REQUEST_MOVEMENTS,
        fetchConfig: {
            path: apiService.logistic.getMovementsDetail(ticketId),
            success: json => receiveGridData(transformData(json), name)
        }
    };
};

const completeSentMovements = () => {
    return {
        type: COMPLETE_SENTTOSAP_PROCESS
    };
};

const receiveFailureSentToSap = response => {
    const errorMessage = response && response.errorCodes && response.errorCodes.length > 0 ? response.errorCodes[0].message : 'systemErrorMessage';
    return {
        type: RECEIVE_FAILURE_SENTTOSAP_PROCESS,
        errorMessage: utilities.isNullOrWhitespace(errorMessage) ? resourceProvider.read('systemErrorMessage') : resourceProvider.read(errorMessage)
    };
};

export const requestSendMovements = movements => {
    return {
        type: REQUEST_SEND_MOVEMENTS,
        fetchConfig: {
            path: apiService.logistic.confirmMovements(),
            success: response => completeSentMovements(response),
            failure: response => receiveFailureSentToSap(response),
            body: Object.assign({}, movements),
            method: 'POST'
        }
    };
};

const receiveOfficialPeriods = periods => {
    return {
        type: RECEIVE_SAP_OFFICIAL_PERIODS,
        periods
    };
};

export const getOfficialPeriods = (segmentId, years) => {
    return {
        type: REQUEST_SAP_OFFICIAL_PERIODS,
        fetchConfig: {
            path: apiService.officialDelta.getOfficialPeriods(segmentId, years, true),
            success: receiveOfficialPeriods
        }
    };
};

export const saveConfirmWizardData = ticketId => {
    return {
        type: SAVE_CONFIRM_WIZARD_DATA,
        data: ticketId
    };
};

export const resetConfirmWizardData = () => {
    return {
        type: RESET_CONFIRM_WIZARD_DATA
    };
};

export const receiveNodesInTicket = tickets => {
    return {
        type: RECEIVE_NODES_IN_TICKET,
        tickets
    };
};

export const receivefailureNodesInTicket = () => {
    return {
        type: RECEIVE_FAILURE_NODES_IN_TICKET
    };
};

export const requestNodesInTicket = ticketId => {
    return {
        type: REQUEST_NODES_IN_TICKET,
        fetchConfig: {
            path: apiService.ticket.getTicketWithNodes(ticketId),
            success: json => receiveNodesInTicket(json),
            failure: () => receivefailureNodesInTicket()
        }
    };
};

export const resetOfficialPeriods = () => {
    return {
        type: RESET_SAP_OFFICIAL_PERIODS
    };
};

export const clearSearchNodes = () => {
    return {
        type: CLEAR_SAP_SEARCH_NODES
    };
};

const receiveSearchNodes = json => {
    const nodes = json.value ? json.value.map(x => x.node) : [];
    return {
        type: RECEIVE_SAP_SEARCH_NODES,
        nodes
    };
};

export const requestSearchNodes = (elementId, searchText) => {
    return {
        type: REQUEST_SAP_SEARCH_NODES,
        fetchConfig: {
            path: apiService.node.searchNodesOfSegment(elementId, searchText),
            success: json => receiveSearchNodes(json),
            failure: () => clearSearchNodes()
        }
    };
};

const receiveGetTicket = ticket => {
    return {
        type: RECEIVE_GET_TICKET,
        ticket
    };
};

const receiveFailureGetTicket = () => {
    return {
        type: RECEIVE_FAILURE_GET_TICKET
    };
};

export const requestGetTicket = ticketId => {
    return {
        type: REQUEST_GET_TICKET,
        fetchConfig: {
            path: apiService.ticket.getTicketByTicketId(ticketId),
            method: 'GET',
            success: receiveGetTicket,
            failure: receiveFailureGetTicket
        }
    };
};

export const confirmCancelBatch = () => {
    return {
        type: CONFIRM_CANCEL_BATCH
    };
};

const completeCancelBatch = () => {
    return {
        type: COMPLETE_CANCEL_BATCH_PROCESS
    };
};

const receiveFailureCancelBatch = response => {
    const errorMessage = response && response.errorCodes && response.errorCodes.length > 0 ? response.errorCodes[0].message : 'systemErrorMessage';
    return {
        type: RECEIVE_FAILURE_CANCEL_BATCH_PROCESS,
        errorMessage: utilities.isNullOrWhitespace(errorMessage) ? resourceProvider.read('systemErrorMessage') : resourceProvider.read(errorMessage)
    };
};

export const requestCancelBatch = ticketId => {
    return {
        type: REQUEST_CANCEL_BATCH,
        fetchConfig: {
            path: apiService.logistic.cancelBatch(ticketId),
            success: response => completeCancelBatch(response),
            failure: response => receiveFailureCancelBatch(response),
            method: 'POST'
        }
    };
};

export const resetSapFromData = () => {
    return {
        type: RESET_SAP_FORM_DATA
    };
};

export const setSapTicketInfo = ticket => {
    return {
        type: SET_SAP_TICKET_INFO,
        ticket
    };
};

export const receiveSapTicketValidations = validations => {
    return {
        type: RECEIVE_SAP_TICKET_VALIDATIONS,
        validations
    };
};

export const resetSapTicketValidations = () => {
    return {
        type: RESET_SAP_TICKET_VALIDATIONS
    };
};

const receiveTicketSuccessful = () => {
    return {
        type: RECEIVE_SAP_TICKET
    };
};

const receiveticketFailure = () => {
    return {
        type: RECEIVE_SAP_TICKET_FAILURE
    };
};

export const requestSaveTicket = ticket => {
    return {
        type: REQUEST_SAP_TICKET,
        fetchConfig: {
            path: apiService.operationalCutOff.saveOperationalCutOff(),
            body: ticket,
            success: receiveTicketSuccessful,
            failure: receiveticketFailure
        }
    };
};

const completeForwardMovements = () => {
    return {
        type: COMPLETE_FORWARD_PROCESS
    };
};

export const requestForwardToSap = logisticsMovements => {
    return {
        type: REQUEST_FORWARD_MOVEMENTS,
        fetchConfig: {
            path: apiService.logistic.forwardMovements(),
            body: logisticsMovements,
            method: 'POST',
            success: completeForwardMovements
        }
    };
};
