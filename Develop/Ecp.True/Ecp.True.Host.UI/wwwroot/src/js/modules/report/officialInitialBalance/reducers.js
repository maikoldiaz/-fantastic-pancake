import {
    OFFICIAL_INITIAL_BALANCE_SAVE_FILTER,
    OFFICIAL_INITIAL_BALANCE_SELECT_ELEMENT,
    RESET_OFFICIAL_INITIAL_BALANCE_FILTER,
    RECEIVE_OFFICIAL_INITIAL_BALANCE,
    RECEIVE_OFFICIAL_INITIAL_BALANCE_STATUS,
    REFRESH_STATUS,
    CLEAR_STATUS,
    BUILD_INITIAL_REPORT_EXECUTION_FILTERS,
    RECEIVE_ERROR_OFFICIAL_INITIAL_BALANCE,
    CLEAR_OFFICIAL_INITIAL_NODES
} from './actions';
import { dateService } from '../../../common/services/dateService';
import { constants } from '../../../common/services/constants';
import { NAVIGATE_TO_REPORTS_GRID } from '../cutOff/actions';

const pendingBalanceInitialState = {
    filterSettings: {}
};

export const initialBalance = (state = pendingBalanceInitialState, action = {}) => {
    switch (action.type) {
    case OFFICIAL_INITIAL_BALANCE_SELECT_ELEMENT:
        return Object.assign({},
            state,
            {
                formValues: {
                    element: action.element
                }
            });
    case OFFICIAL_INITIAL_BALANCE_SAVE_FILTER:
        return Object.assign({},
            state,
            {
                filters: action.data,
                formValues: {
                    element: action.data ? action.data.element : null
                }
            });
    case RESET_OFFICIAL_INITIAL_BALANCE_FILTER:
        return Object.assign({},
            state,
            {
                filters: {
                    elementId: null
                }
            });
    case RECEIVE_OFFICIAL_INITIAL_BALANCE:
        return Object.assign({},
            state,
            {
                reportToggler: !state.reportToggler,
                executionId: action.executionId,
                filters: Object.assign({}, state.filters, { executionId: action.executionId })
            });
    case RECEIVE_OFFICIAL_INITIAL_BALANCE_STATUS:
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
                officialInitialBalanceStatus: null
            });
    case NAVIGATE_TO_REPORTS_GRID:
        return Object.assign({}, state, { navigateToggler: !state.navigateToggler });
    case BUILD_INITIAL_REPORT_EXECUTION_FILTERS: {
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
    case RECEIVE_ERROR_OFFICIAL_INITIAL_BALANCE:
        return Object.assign({}, state, { errorSaveToggler: !state.errorSaveToggler });
    case CLEAR_OFFICIAL_INITIAL_NODES: {
        return Object.assign({}, state, {
            clearSelectedNode: action.status,
            clearSelectedNodeToggler: action.status === true ? !state.clearSelectedNodeToggler : state.clearSelectedNodeToggler
        });
    } default:
        return state;
    }
};
