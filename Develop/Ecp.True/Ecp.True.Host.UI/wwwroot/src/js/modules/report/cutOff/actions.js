import { apiService } from '../../../common/services/apiService';
import { utilities } from '../../../common/services/utilities';
import { constants } from '../../../common/services/constants';

export const CUTOFF_REPORT_FILTER_ON_SELECT_CATEGORY = 'CUTOFF_REPORT_FILTER_ON_SELECT_CATEGORY';
export const cutOffReportFilterOnSelectCategory = selectedCategory => {
    return {
        type: CUTOFF_REPORT_FILTER_ON_SELECT_CATEGORY,
        selectedCategory
    };
};

export const CUTOFF_REPORT_FILTER_ON_SELECT_ELEMENT = 'CUTOFF_REPORT_FILTER_ON_SELECT_ELEMENT';
export const cutOffReportFilterOnSelectElement = selectedElement => {
    return {
        type: CUTOFF_REPORT_FILTER_ON_SELECT_ELEMENT,
        selectedElement
    };
};

export const REQUEST_SEARCH_NODES = 'REQUEST_SEARCH_NODES';
export const RECEIVE_SEARCH_NODES = 'RECEIVE_SEARCH_NODES';
export const CLEAR_SEARCH_NODES = 'CLEAR_SEARCH_NODES';

export const clearSearchNodes = () => {
    return {
        type: CLEAR_SEARCH_NODES
    };
};

export const receiveSearchNodes = json => {
    const nodes = json.value ? json.value.map(x => x.node) : [];
    return {
        type: RECEIVE_SEARCH_NODES,
        nodes
    };
};

export const requestSearchNodes = (elementId, searchText) => {
    return {
        type: REQUEST_SEARCH_NODES,
        fetchConfig: {
            path: apiService.node.searchNodeTags(elementId, searchText),
            success: receiveSearchNodes,
            failure: clearSearchNodes
        }
    };
};

export const CUTOFF_REPORT_RESET_FIELDS = 'CUTOFF_REPORT_RESET_FIELDS';

export const cutOffReportResetFields = () => {
    return {
        type: CUTOFF_REPORT_RESET_FIELDS
    };
};


export const SAVE_CUTOFF_REPORT_FILTER = 'SAVE_CUTOFF_REPORT_FILTER';
export const saveCutOffReportFilter = filters => {
    return {
        type: SAVE_CUTOFF_REPORT_FILTER,
        filters
    };
};

export const clearCutOffReportFilter = () => {
    return {
        type: SAVE_CUTOFF_REPORT_FILTER,
        filters: null
    };
};

export const REQUEST_FINAL_TICKET = 'REQUEST_FINAL_TICKET';
export const RECEIVE_FINAL_TICKET = 'RECEIVE_FINAL_TICKET';

export const receiveFinalTicket = json => {
    let ticket = null;
    if (json.value.length) {
        ticket = json.value[0];
        if (!ticket.endDate) {
            ticket.endDate = ticket.ownershipTicket ? ticket.ownershipTicket.endDate : ticket.unbalanceTicket.endDate;
        }
    }
    return {
        type: RECEIVE_FINAL_TICKET,
        ticket: ticket
    };
};

export const requestFinalTicket = (categoryId, elementId, ticketTypeId) => {
    return {
        type: REQUEST_FINAL_TICKET,
        fetchConfig: {
            path: categoryId === constants.Category.System ?
                apiService.ticket.getFinalSystemTicket(elementId, ticketTypeId) :
                apiService.ticket.getFinalSegmentTicket(elementId, ticketTypeId),
            success: receiveFinalTicket
        }
    };
};

export const REQUEST_NONOPERATIONAL_SEGMENT_OWNERSHIP = 'REQUEST_NONOPERATIONAL_SEGMENT_OWNERSHIP';
export const RECEIVE_NONOPERATIONAL_SEGMENT_OWNERSHIP = 'RECEIVE_NONOPERATIONAL_SEGMENT_OWNERSHIP';
export const REQUEST_OPERATIONAL_DATA_WITHOUT_CUTOFF = 'REQUEST_OPERATIONAL_DATA_WITHOUT_CUTOFF';
export const RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF = 'RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF';
export const RECEIVE_ERROR_OPERATIONAL_DATA_WITHOUT_CUTOFF = 'RECEIVE_ERROR_OPERATIONAL_DATA_WITHOUT_CUTOFF';
export const REQUEST_OPERATIONAL_DATA_WITHOUT_CUTOFF_STATUS = 'REQUEST_OPERATIONAL_DATA_WITHOUT_CUTOFF_STATUS';
export const RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF_STATUS = 'RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF_STATUS';
export const REFRESH_STATUS = 'REFRESH_STATUS';
export const CLEAR_STATUS = 'CLEAR_STATUS';

export const receiveOperationalDataWithoutCutOff = executionId => {
    return {
        type: RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF,
        executionId
    };
};

export const receiveOperationalDataWithoutCutOffStatus = status => {
    return {
        type: RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF_STATUS,
        status
    };
};

export const receiveErrorOperationalDataWithoutCutOff = () => {
    return {
        type: RECEIVE_ERROR_OPERATIONAL_DATA_WITHOUT_CUTOFF
    };
};

export const refreshStatus = () => {
    return {
        type: REFRESH_STATUS
    };
};

export const clearStatus = () => {
    return {
        type: CLEAR_STATUS
    };
};

export const requestNonOperationalSegmentOwnership = body => {
    return {
        type: REQUEST_NONOPERATIONAL_SEGMENT_OWNERSHIP,
        fetchConfig: {
            path: apiService.ticket.postNonOperationalSegmentOwnership(),
            body,
            success: receiveOperationalDataWithoutCutOff
        }
    };
};

export const requestOperationalDataWithoutCutOff = body => {
    return {
        type: REQUEST_OPERATIONAL_DATA_WITHOUT_CUTOFF,
        fetchConfig: {
            path: apiService.ticket.postOperationalDataWithoutCutOff(),
            body,
            success: receiveOperationalDataWithoutCutOff,
            failure: receiveErrorOperationalDataWithoutCutOff
        }
    };
};

export const requestOperationalDataWithoutCutoffStatus = executionId => {
    return {
        type: REQUEST_OPERATIONAL_DATA_WITHOUT_CUTOFF_STATUS,
        fetchConfig: {
            path: apiService.ticket.requestReportExecutionStatus(executionId),
            success: receiveOperationalDataWithoutCutOffStatus,
            showProgress: false
        }
    };
};

export const REQUEST_OWNERSHIP_NODE_DETAILS = 'REQUEST_OWNERSHIP_NODE_DETAILS';
export const RECEIVE_OWNERSHIP_NODE_DETAILS = 'RECEIVE_OWNERSHIP_NODE_DETAILS';

export const receiveOwnershipNode = json => {
    return {
        type: RECEIVE_OWNERSHIP_NODE_DETAILS,
        node: json.value.length > 0 ? json.value[0] : {}
    };
};

export const requestOwnershipNode = nodeId => {
    return {
        type: REQUEST_OWNERSHIP_NODE_DETAILS,
        fetchConfig: {
            path: apiService.ownershipNode.queryByOwnershipNodeId(nodeId),
            success: receiveOwnershipNode,
            notFound: true
        }
    };
};

export const PARSE_ENCODED_REPORT_FILTERS = 'PARSE_ENCODED_REPORT_FILTERS';
export const parseReportFilters = encoded => {
    return {
        type: PARSE_ENCODED_REPORT_FILTERS,
        filters: JSON.parse(utilities.base64Decode(encoded))
    };
};

export const NAVIGATE_TO_REPORTS_GRID = 'NAVIGATE_TO_REPORTS_GRID';
export const navigateToReportsGrid = () => {
    return {
        type: NAVIGATE_TO_REPORTS_GRID
    };
};

export const BUILD_CUTOFF_REPORT_EXECUTION_FILTERS = 'BUILD_CUTOFF_REPORT_EXECUTION_FILTERS';
export const buildReportFilters = execution => {
    return {
        type: BUILD_CUTOFF_REPORT_EXECUTION_FILTERS,
        execution
    };
};

export const CLEAR_CUTOFF_NODES = 'CLEAR_CUTOFF_NODES';
export const clearSelectedNode = status => {
    return {
        type: CLEAR_CUTOFF_NODES,
        status
    };
};

