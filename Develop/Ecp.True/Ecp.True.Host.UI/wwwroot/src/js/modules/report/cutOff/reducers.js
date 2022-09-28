import {
    CUTOFF_REPORT_FILTER_ON_SELECT_CATEGORY,
    RECEIVE_SEARCH_NODES, CUTOFF_REPORT_FILTER_ON_SELECT_ELEMENT,
    CLEAR_SEARCH_NODES,
    SAVE_CUTOFF_REPORT_FILTER,
    RECEIVE_FINAL_TICKET,
    RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF,
    RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF_STATUS,
    REFRESH_STATUS,
    CLEAR_STATUS,
    CUTOFF_REPORT_RESET_FIELDS,
    RECEIVE_OWNERSHIP_NODE_DETAILS,
    PARSE_ENCODED_REPORT_FILTERS,
    NAVIGATE_TO_REPORTS_GRID,
    BUILD_CUTOFF_REPORT_EXECUTION_FILTERS,
    RECEIVE_ERROR_OPERATIONAL_DATA_WITHOUT_CUTOFF,
    CLEAR_CUTOFF_NODES
} from './actions';
import { dateService } from '../../../common/services/dateService';
import { constants } from '../../../common/services/constants';

// Category element reducer
const categoryElementFilterInitialState = {
    selectedCategory: null,
    selectedElement: null,
    searchedNodes: [],
    ticket: null
};

export const cutOffReport = (state = categoryElementFilterInitialState, action = {}) => {
    switch (action.type) {
    case CUTOFF_REPORT_FILTER_ON_SELECT_CATEGORY:
        return Object.assign({},
            state,
            {
                selectedCategory: action.selectedCategory
            });
    case CUTOFF_REPORT_FILTER_ON_SELECT_ELEMENT:
        return Object.assign({},
            state,
            {
                selectedElement: action.selectedElement
            });
    case RECEIVE_SEARCH_NODES:
        return Object.assign({},
            state,
            {
                searchedNodes: action.nodes
            });
    case CLEAR_SEARCH_NODES:
        return Object.assign({},
            state,
            {
                searchedNodes: []
            });
    case SAVE_CUTOFF_REPORT_FILTER:
        return Object.assign({},
            state,
            {
                filters: action.filters
            });
    case RECEIVE_FINAL_TICKET:
        return Object.assign({},
            state,
            {
                ticket: action.ticket
            });
    case RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF:
        return Object.assign({},
            state,
            {
                reportToggler: !state.reportToggler,
                executionId: action.executionId,
                filters: Object.assign({}, state.filters, { executionId: action.executionId })
            });
    case RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF_STATUS:
        return Object.assign({},
            state,
            {
                receiveStatusToggler: !state.receiveStatusToggler,
                status: action.status
            });
    case REFRESH_STATUS:
        return Object.assign({},
            state,
            {
                refreshStatusToggler: !state.refreshStatusToggler
            });
    case CLEAR_STATUS:
        return Object.assign({},
            state,
            {
                operationalDataWithoutCutoffStatus: null
            });
    case CUTOFF_REPORT_RESET_FIELDS:
        return Object.assign({},
            state,
            {
                selectedCategory: null,
                selectedElement: null,
                searchedNodes: []
            });
    case RECEIVE_OWNERSHIP_NODE_DETAILS:
        return Object.assign({}, state, {
            filters: {
                categoryName: action.node.categoryName,
                elementName: action.node.segment,
                nodeName: action.node.nodeName,
                initialDate: dateService.parse(action.node.ticketStartDate),
                finalDate: dateService.parse(action.node.ticketFinalDate),
                reportType: constants.Report.WithOwner
            }
        });
    case PARSE_ENCODED_REPORT_FILTERS: {
        action.filters.initialDate = dateService.parse(action.filters.initialDate);
        action.filters.finalDate = dateService.parse(action.filters.finalDate);
        return Object.assign({}, state, {
            filters: action.filters
        });
    }
    case NAVIGATE_TO_REPORTS_GRID:
        return Object.assign({}, state, { navigateToggler: !state.navigateToggler });
    case BUILD_CUTOFF_REPORT_EXECUTION_FILTERS: {
        return Object.assign({}, state, {
            filters: {
                executionId: action.execution.executionId,
                categoryName: action.execution.categoryId === constants.Category.Segment ? 'Segmento' : 'Sistema',
                elementName: action.execution.segment || action.execution.system,
                nodeName: action.execution.nodeName,
                initialDate: dateService.parse(action.execution.startDate),
                finalDate: dateService.parse(action.execution.endDate),
                reportType: constants.Report[action.execution.name]
            }
        });
    }
    case RECEIVE_ERROR_OPERATIONAL_DATA_WITHOUT_CUTOFF: {
        return Object.assign({}, state, { errorSaveCutOffToggler: !state.errorSaveCutOffToggler });
    }
    case CLEAR_CUTOFF_NODES: {
        return Object.assign({}, state, {
            clearSelectedNode: action.status,
            clearSelectedNodeToggler: action.status === true ? !state.clearSelectedNodeToggler : state.clearSelectedNodeToggler
        });
    }
    default:
        return state;
    }
};
