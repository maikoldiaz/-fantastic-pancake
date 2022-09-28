import { apiService } from '../../../common/services/apiService';

export const OFFICIAL_DELTA_SAVE_FILTER = 'OFFICIAL_DELTA_SAVE_FILTER';
export const saveOfficialDeltaFilter = data => {
    return {
        type: OFFICIAL_DELTA_SAVE_FILTER,
        data
    };
};

export const OFFICIAL_DELTA_SELECT_ELEMENT = 'OFFICIAL_DELTA_SELECT_ELEMENT';
export const onSelectedElement = element => {
    return {
        type: OFFICIAL_DELTA_SELECT_ELEMENT,
        element
    };
};

export const RESET_OFFICIAL_DELTA_FILTER = 'RESET_OFFICIAL_DELTA_FILTER';
export const resetBalancePerNodeFilter = () => {
    return {
        type: RESET_OFFICIAL_DELTA_FILTER
    };
};

export const RECEIVE_SUBMIT_FOR_APPROVAL = 'RECEIVE_SUBMIT_FOR_APPROVAL';
export const receiveSubmitForApproval = sendForApprovalResponse => {
    return {
        type: RECEIVE_SUBMIT_FOR_APPROVAL,
        sendForApprovalResponse
    };
};

export const REQUEST_SUBMIT_FOR_APPROVAL = 'REQUEST_SUBMIT_FOR_APPROVAL';
export const submitForApproval = approvalrequest => {
    return {
        type: REQUEST_SUBMIT_FOR_APPROVAL,
        fetchConfig: {
            path: apiService.ticket.sendDeltaNodeForApproval(),
            method: 'POST',
            success: receiveSubmitForApproval,
            body: approvalrequest
        }
    };
};

export const RECEIVE_SUBMIT_FOR_REOPEN = 'RECEIVE_SUBMIT_FOR_REOPEN';
export const receiveSubmitForReopen = reopenNodesList => {
    return {
        type: RECEIVE_SUBMIT_FOR_REOPEN,
        reopenNodesList
    };
};

export const RECEIVE_REOPEN = 'RECEIVE_REOPEN';
export const receiveReopen = reopenConfirmation => {
    return {
        type: RECEIVE_REOPEN,
        reopenConfirmation
    };
};

export const REQUEST_SUBMIT_FOR_REOPEN = 'REQUEST_SUBMIT_FOR_REOPEN';
export const submitForReopen = deltaNodeId => {
    return {
        type: REQUEST_SUBMIT_FOR_REOPEN,
        fetchConfig: {
            path: apiService.ticket.sendDeltaNodeForReopen(deltaNodeId),
            success: receiveSubmitForReopen
        }
    };
};

export const SUBMIT_REOPEN = 'SUBMIT_REOPEN';
export const reopenDeltaNode = reopenItems => {
    return {
        type: SUBMIT_REOPEN,
        fetchConfig: {
            path: apiService.ticket.reopenDeltaNode(),
            method: 'POST',
            success: receiveReopen,
            body: reopenItems
        }
    };
};

export const SUBMISSION_TYPE = 'SUBMISSION_TYPE';
export const setSubmissionType = type => {
    return {
        type: SUBMISSION_TYPE,
        submissionType: type
    };
};

export const SET_REPORT_TOGGLER = 'SET_REPORT_TOGGLER';
export const setReportToggler = value => {
    return {
        type: SET_REPORT_TOGGLER,
        nodeReportToggler: value
    };
};

export const RESET_DELTA_NODE_SOURCE = 'RESET_DELTA_NODE_SOURCE';
export const resetDeltaNodeSource = source => {
    return {
        type: RESET_DELTA_NODE_SOURCE,
        source
    };
};

export const RECEIVE_DELTA_NODE_DETAILS = 'RECEIVE_DELTA_NODE_DETAILS';
export const receiveDeltaNode = json => {
    return {
        type: RECEIVE_DELTA_NODE_DETAILS,
        node: json.value.length > 0 ? json.value[0] : {}
    };
};

export const REQUEST_DELTA_NODE_DETAILS = 'REQUEST_DELTA_NODE_DETAILS';
export const requestDeltaNode = deltaNodeId => {
    return {
        type: REQUEST_DELTA_NODE_DETAILS,
        fetchConfig: {
            path: apiService.officialDelta.queryByDeltaNodeId(deltaNodeId),
            success: receiveDeltaNode,
            notFound: true
        }
    };
};

export const SET_FLOW_TRIGGER_TOGGLER = 'SET_FLOW_TRIGGER_TOGGLER';
export const setFlowTriggerToggler = toggle => {
    return {
        type: SET_FLOW_TRIGGER_TOGGLER,
        flowReportToggler: toggle
    };
};

export const resetNodeFilter = () => {
    return {
        type: RESET_OFFICIAL_DELTA_FILTER
    };
};

export const RESET_FLOW_NODE_FILTER = 'RESET_FLOW_NODE_FILTER';
export const resetFlowNodeFilter = () => {
    return {
        type: RESET_FLOW_NODE_FILTER
    };
};

export const RESET_FLOW_REPORT_TRIGGER = 'RESET_FLOW_REPORT_TRIGGER';
export const resetFlowReportToggler = () => {
    return {
        type: RESET_FLOW_REPORT_TRIGGER
    };
};

const REQUEST_DELTA_NODE_MOVEMENTS = 'REQUEST_DELTA_NODE_MOVEMENTS';
export const RECEIVE_DELTA_NODE_MOVEMENTS = 'RECEIVE_DELTA_NODE_MOVEMENTS';
export const receiveDeltaNodeMovements = data => {
    return {
        type: RECEIVE_DELTA_NODE_MOVEMENTS,
        data: data.value
    };
};

export const requestDeltaNodeMovements = (startTime, endTime, nodeId) => {
    return {
        type: REQUEST_DELTA_NODE_MOVEMENTS,
        fetchConfig: {
            path: apiService.officialDelta.queryManualMovementsByDeltaNodeId(startTime, endTime, nodeId),
            success: receiveDeltaNodeMovements
        }
    };
};

const SAVE_DELTA_NODE_MOVEMENTS = 'SAVE_DELTA_NODE_MOVEMENTS';
export const RECEIVE_SAVE_DELTA_NODE_MOVEMENTS = 'RECEIVE_SAVE_DELTA_NODE_MOVEMENTS';
export const receiveSaveManualMovementsOk = () => {
    return {
        type: RECEIVE_SAVE_DELTA_NODE_MOVEMENTS,
        status: true
    };
};
export const receiveSaveManualMovementsReset = () => {
    return {
        type: RECEIVE_SAVE_DELTA_NODE_MOVEMENTS,
        status: null
    };
};
export const receiveSaveManualMovementsFail = () => {
    return {
        type: RECEIVE_SAVE_DELTA_NODE_MOVEMENTS,
        status: false
    };
};

export const saveManualMovements = (deltaNodeId, manualMovementsId) => {
    return {
        type: SAVE_DELTA_NODE_MOVEMENTS,
        fetchConfig: {
            path: apiService.officialDelta.setManualMovementsByTicketAndConsolidateMovements(deltaNodeId),
            method: 'PUT',
            success: receiveSaveManualMovementsOk,
            failure: receiveSaveManualMovementsFail,
            body: manualMovementsId
        }
    };
};
