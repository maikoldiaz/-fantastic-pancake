import {
    OFFICIAL_DELTA_SAVE_FILTER,
    OFFICIAL_DELTA_SELECT_ELEMENT,
    RECEIVE_DELTA_NODE_DETAILS,
    RESET_OFFICIAL_DELTA_FILTER,
    RECEIVE_SUBMIT_FOR_APPROVAL,
    RECEIVE_SUBMIT_FOR_REOPEN,
    RECEIVE_REOPEN,
    SUBMISSION_TYPE,
    RESET_DELTA_NODE_SOURCE,
    RECEIVE_DELTA_NODE_MOVEMENTS,
    RECEIVE_SAVE_DELTA_NODE_MOVEMENTS
} from './actions';
import { SET_DELTA_NODE_SOURCE } from '../../dailyBalance/officialDeltaPerNode/actions';

import { dateService } from '../../../common/services/dateService';
import { constants } from '../../../common/services/constants';

const filterInitialState = {
    filterSettings: {},
    isSaveForm: null
};

export const officialDeltaNode = (state = filterInitialState, action = {}) => {
    switch (action.type) {
    case OFFICIAL_DELTA_SELECT_ELEMENT:
        return Object.assign({},
            state,
            {
                formValues: {
                    element: action.element
                }
            });
    case OFFICIAL_DELTA_SAVE_FILTER:
        return Object.assign({},
            state,
            {
                filters: action.data,
                formValues: {
                    element: action.data.element
                }
            });
    case RECEIVE_DELTA_NODE_DETAILS:
        return Object.assign({}, state, {
            filters: {
                elementName: action.node.segment,
                elementId: action.node.segmentId,
                nodeName: action.node.nodeName,
                nodeId: action.node.nodeId,
                reportType: constants.Report.OfficialBalancePerNodeReport,
                initialDateShort: dateService.format(action.node.startDate),
                finalDateShort: dateService.format(action.node.endDate),
                initialDate: dateService.parseToDate(dateService.convertToColombian(action.node.startDate), constants.DateFormat.LongDate),
                finalDate: dateService.parseToDate(dateService.convertToColombian(action.node.endDate), constants.DateFormat.LongDate),
                nodeStatus: action.node.status,
                ticketStatus: action.node.ticketStatus,
                deltaNodeId: action.node.deltaNodeId
            },
            reportToggler: !state.reportToggler
        });
    case RECEIVE_SUBMIT_FOR_APPROVAL:
        return Object.assign({},
            state,
            {
                sendForApprovalResponse: action.sendForApprovalResponse,
                approveToggler: !state.approveToggler
            });
    case RECEIVE_SUBMIT_FOR_REOPEN:
        return Object.assign({},
            state,
            {
                reopenNodesList: action.reopenNodesList,
                reopenToggler: !state.reopenToggler
            });
    case RECEIVE_REOPEN:
        return Object.assign({},
            state,
            {
                reopenConfirmation: action.reopenConfirmation,
                reopenConfirmationToggler: !state.reopenConfirmationToggler
            });
    case SUBMISSION_TYPE:
        return Object.assign({},
            state,
            {
                submissionType: action.submissionType
            });
    case RESET_OFFICIAL_DELTA_FILTER:
        return Object.assign({},
            state,
            {
                filters: null
            });
    case SET_DELTA_NODE_SOURCE:
        return Object.assign({},
            state,
            {
                source: action.source
            });
    case RESET_DELTA_NODE_SOURCE:
        return Object.assign({},
            state,
            {
                source: null
            });
    case RECEIVE_DELTA_NODE_MOVEMENTS:
        return Object.assign({},
            state,
            {
                manualMovements: action.data
            });
    case RECEIVE_SAVE_DELTA_NODE_MOVEMENTS:
        return Object.assign({},
            state,
            {
                isSaveForm: action.status
            });
    default:
        return state;
    }
};
