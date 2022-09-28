import { apiService } from '../../../common/services/apiService';
import { utilities } from '../../../common/services/utilities';
import { resourceProvider } from '../../../common/services/resourceProvider';
import { receiveGridData } from '../../../common/components/grid/actions';

export const REQUEST_LAST_TICKET = 'REQUEST_LAST_TICKET';
export const RECEIVE_LAST_TICKET = 'RECEIVE_LAST_TICKET';
export const SET_TICKET_INFO = 'SET_TICKET_INFO';
export const SET_PENDING_TRANSACTION_ERRORS = 'SET_PENDING_TRANSACTION_ERRORS';
export const SET_UNBALANCES = 'SET_UNBALANCES';
export const SET_TRANSFER_POINT_MOVEMENTS = 'SET_TRANSFER_POINT_MOVEMENTS';
export const REQUEST_PENDING_TRANSACTION_ERRORS = 'REQUEST_PENDING_TRANSACTION_ERRORS';
export const REQUEST_TRANSFER_POINT_MOVEMENTS = 'REQUEST_TRANSFER_POINT_MOVEMENTS';
export const REQUEST_UNBALANCES = 'REQUEST_UNBALANCES';
export const REQUEST_SAVE_OPERATIONAL_CUTOFF = 'REQUEST_SAVE_OPERATIONAL_CUTOFF';
export const RECEIVE_SAVE_OPERATIONAL_CUTOFF = 'RECEIVE_SAVE_OPERATIONAL_CUTOFF';
export const COMPLETE_CUTOFF_PROCESS = 'COMPLETE_CUTOFF_PROCESS';
export const INIT_CUTOFF_PROCESS = 'INIT_CUTOFF_PROCESS';
export const INCREMENT_CUTOFF_PROCESS = 'CUTOFF_PROCESS_START';
export const ADD_CUTOFF_COMMENT = 'ADD_CUTOFF_COMMENT';
export const INIT_ADD_CUTOFF_COMMENT = 'INIT_ADD_CUTOFF_COMMENT';
export const CONFIRM_CUT_OFF = 'CONFIRM_CUT_OFF';
export const REQUEST_TICKET_DETAILS = 'REQUEST_TICKET_DETAILS';
export const RECEIVE_TICKET_DETAILS = 'RECEIVE_TICKET_DETAILS';
export const RESET_TICKET_DETAILS = 'RESET_TICKET_DETAILS';
export const SET_TICKET_ERROR = 'SET_TICKET_ERROR';
export const INIT_INVENTORIES_VALIDATIONS = 'INIT_INVENTORIES_VALIDATIONS';
export const REQUEST_INVENTORIES_VALIDATIONS = 'REQUEST_INVENTORIES_VALIDATIONS';
export const RECEIVE_INVENTORIES_VALIDATIONS = 'RECEIVE_INVENTORIES_VALIDATIONS';
export const RECEIVE_FAILURE_SAVE_OPERATIONAL_CUTOFF = 'RECEIVE_FAILURE_SAVE_OPERATIONAL_CUTOFF';
export const SET_OFFICIAL_POINT_ERROR = 'SET_OFFICIAL_POINT_ERROR';
export const REQUEST_UPDATE_CUTOFF_COMMENT = 'REQUEST_UPDATE_CUTOFF_COMMENT';
export const RECEIVE_UPDATE_CUTOFF_COMMENT = 'RECEIVE_UPDATE_CUTOFF_COMMENT';
export const RECEIVE_UPDATE_CUTOFF_COMMENT_FAILURE = 'RECEIVE_UPDATE_CUTOFF_COMMENT_FAILURE';
export const GET_OFFICIAL_POINT_ERROR = 'GET_OFFICIAL_POINT_ERROR';
export const REQUEST_FIRSTTIME_NODES = 'REQUEST_FIRSTTIME_NODES';
export const RECEIVE_FIRSTTIME_NODES = 'RECEIVE_FIRSTTIME_NODES';

export const setPendingTransactionErrors = pendingTransactionErrors => {
    return {
        type: SET_PENDING_TRANSACTION_ERRORS,
        pendingTransactionErrors
    };
};

export const setUnbalances = unbalances => {
    return {
        type: SET_UNBALANCES,
        unbalances
    };
};

export const setTransferPointMovements = officialMovements => {
    return {
        type: SET_TRANSFER_POINT_MOVEMENTS,
        officialMovements
    };
};

export const receiveLastTicket = ticket => {
    return {
        type: RECEIVE_LAST_TICKET,
        ticket
    };
};

export const setTicketInfo = ticket => {
    return {
        type: SET_TICKET_INFO,
        ticket
    };
};

const completeCutOff = ticket => {
    if (ticket.count === 0) {
        return {
            type: RECEIVE_FAILURE_SAVE_OPERATIONAL_CUTOFF,
            errorMessage: resourceProvider.read('systemErrorMessage')
        };
    }
    return {
        type: COMPLETE_CUTOFF_PROCESS,
        ticketId: ticket.value[0].ticketId
    };
};

const receiveSaveTicket = segmentId => {
    return {
        type: RECEIVE_SAVE_OPERATIONAL_CUTOFF,
        fetchConfig: {
            path: apiService.ticket.getLastTicket(segmentId),
            success: completeCutOff
        }
    };
};

const receiveFailureSaveTicket = json => {
    const errorMessage = json && json.errorCodes && json.errorCodes.length > 0 ? json.errorCodes[0].message : 'systemErrorMessage';
    return {
        type: RECEIVE_FAILURE_SAVE_OPERATIONAL_CUTOFF,
        errorMessage: utilities.isNullOrWhitespace(errorMessage) ? resourceProvider.read('systemErrorMessage') : resourceProvider.read(errorMessage)
    };
};

export const requestSaveTicket = (operationalCutOff, sessionId) => {
    return {
        type: REQUEST_SAVE_OPERATIONAL_CUTOFF,
        fetchConfig: {
            path: apiService.operationalCutOff.saveOperationalCutOff(),
            success: () => receiveSaveTicket(operationalCutOff.ticket.categoryElementId),
            failure: json => receiveFailureSaveTicket(json),
            body: Object.assign({}, operationalCutOff, { sessionId })
        }
    };
};

export const requestLastTicket = segmentId => {
    return {
        type: REQUEST_LAST_TICKET,
        segmentId,
        fetchConfig: {
            path: apiService.ticket.getLastTicket(segmentId),
            success: receiveLastTicket
        }
    };
};

export const receiveUpdateCutOffComment = status => {
    return {
        type: RECEIVE_UPDATE_CUTOFF_COMMENT,
        status
    };
};

export const receiveUpdateCutOffCommentFailure = status => {
    return {
        type: RECEIVE_UPDATE_CUTOFF_COMMENT_FAILURE,
        status
    };
};

export const requestUpdateCutOffComment = body => {
    return {
        type: REQUEST_UPDATE_CUTOFF_COMMENT,
        fetchConfig: {
            path: apiService.operationalCutOff.updateComment(),
            body,
            success: receiveUpdateCutOffComment,
            failure: receiveUpdateCutOffCommentFailure
        }
    };
};

const sortErrors = json => {
    json.sort(function (e1, e2) {
        let v1 = utilities.isNullOrUndefined(e1.pendingTransaction.systemTypeId) ? '' : e1.pendingTransaction.systemTypeId;
        let v2 = utilities.isNullOrUndefined(e2.pendingTransaction.systemTypeId) ? '' : e2.pendingTransaction.systemTypeId;

        if (v2 > v1) {
            return -1;
        }

        if (v2 < v1) {
            return 1;
        }

        v1 = Number(e1.pendingTransaction.volume);
        v2 = Number(e2.pendingTransaction.volume);

        if (v2 > v1) {
            return 1;
        }

        if (v2 < v1) {
            return -1;
        }

        return 0;
    });

    return json;
};

export const requestPendingTransactions = (path, ticket, name) => {
    return {
        type: REQUEST_PENDING_TRANSACTION_ERRORS,
        fetchConfig: {
            path,
            body: ticket,
            success: json => receiveGridData(sortErrors(json), name),
            failure: () => receiveGridData([], name)
        }
    };
};

export const requestTransferPointMovements = (path, ticket, name) => {
    return {
        type: REQUEST_TRANSFER_POINT_MOVEMENTS,
        fetchConfig: {
            path,
            body: {
                categoryElementId: ticket.categoryElementId,
                startDate: ticket.startDate,
                endDate: ticket.endDate
            },
            success: json => receiveGridData(json, name)
        }
    };
};

const sortUnbalances = json => {
    json.sort(function (e1, e2) {
        let v1 = utilities.isNullOrUndefined(e1.nodeName) ? '' : e1.nodeName;
        let v2 = utilities.isNullOrUndefined(e2.nodeName) ? '' : e2.nodeName;

        if (v2 > v1) {
            return -1;
        }

        if (v2 < v1) {
            return 1;
        }

        v1 = Number(e1.unbalancePercentage);
        v2 = Number(e2.unbalancePercentage);

        if (v2 > v1) {
            return 1;
        }

        if (v2 < v1) {
            return -1;
        }

        return 0;
    });

    return json;
};

export const requestUnbalances = (path, ticket, officialMovements, name, firstTimeNodes) => {
    return {
        type: REQUEST_UNBALANCES,
        fetchConfig: {
            path,
            body: {
                ticket: ticket,
                transferPoints: officialMovements,
                firstTimeNodes: firstTimeNodes

            },
            success: json => receiveGridData(sortUnbalances(json), name)
        }
    };
};

export const initCutOff = () => {
    return {
        type: INIT_CUTOFF_PROCESS
    };
};

export const incrementCutOff = () => {
    return {
        type: INCREMENT_CUTOFF_PROCESS
    };
};
// add comment
export const addComment = comment => {
    return {
        type: ADD_CUTOFF_COMMENT,
        comment
    };
};

export const intAddComment = (name, preText, postText, count) => {
    return {
        type: INIT_ADD_CUTOFF_COMMENT,
        name,
        preText,
        postText,
        count
    };
};

export const confirmCutoff = () => {
    return {
        type: CONFIRM_CUT_OFF
    };
};


export const receiveTicketDetails = ticket => {
    return {
        type: RECEIVE_TICKET_DETAILS,
        ticket
    };
};

export const requestTicketDetails = ticketId => {
    return {
        type: REQUEST_TICKET_DETAILS,
        fetchConfig: {
            path: apiService.ticket.getTicketInformation(ticketId),
            success: receiveTicketDetails
        }
    };
};

export const resetTicketDetails = () => {
    return {
        type: RESET_TICKET_DETAILS
    };
};

export const initializeTicketError = ticket => {
    return {
        type: SET_TICKET_ERROR,
        ticket
    };
};

export const initInventoriesValidations = () => {
    return {
        type: INIT_INVENTORIES_VALIDATIONS
    };
};

export const receiveInventoriesValidations = validations => {
    return {
        type: RECEIVE_INVENTORIES_VALIDATIONS,
        validations
    };
};

export const receiveFirstTimeNodes = nodeIds => {
    return {
        type: RECEIVE_FIRSTTIME_NODES,
        nodeIds
    };
};

export const requestInventoriesValidations = ticket => {
    return {
        type: 'REQUEST_INVENTORIES_VALIDATIONS',
        fetchConfig: {
            path: apiService.operationalCutOff.validateInitialInventory(),
            method: 'PUT',
            body: ticket,
            success: json => receiveInventoriesValidations(json)
        }
    };
};

export const setSapTrackingErrors = (errors, row) => {
    return {
        type: SET_OFFICIAL_POINT_ERROR,
        errors,
        row
    };
};

export const getSapTrackingErrors = row => {
    return {
        type: GET_OFFICIAL_POINT_ERROR,
        fetchConfig: {
            path: apiService.operationalCutOff.getSapTrackingErrors(row.sapTrackingId),
            method: 'GET',
            success: json => setSapTrackingErrors(json, row)
        }
    };
};

export const requestFirstTimeNodes = ticket => {
    return {
        type: REQUEST_FIRSTTIME_NODES,
        fetchConfig: {
            path: apiService.operationalCutOff.getFirstTimeNodes(),
            method: 'POST',
            body: ticket,
            success: receiveFirstTimeNodes
        }
    };
};
