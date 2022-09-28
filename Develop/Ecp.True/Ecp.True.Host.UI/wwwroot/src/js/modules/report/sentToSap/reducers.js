import {
    BUILD_SEND_TO_SAP_REPORT_EXECUTION_FILTERS,
    RESET_PERIOD_SEND_TO_SAP_REPORT,
    RECEIVE_SAP_REPORT_OFFICIAL_PERIODS,
    FAILED_RECEIVE_SENT_TO_SAP_REPORT,
    RECEIVE_SENT_TO_SAP_REPORT
} from './actions';
import { dateService } from '../../../common/services/dateService';
import { constants } from '../../../common/services/constants';

const sentToSapInitialState = {
    filters: null,
    officialPeriods: {
        defaultYear: null,
        officialPeriods: []
    },
    reportToggler: false,
    errorSaveToggler: false
};

export const sentToSapReport = (state = sentToSapInitialState, action = {}) => {
    switch (action.type) {
    case '@@redux-form/CHANGE': {
        if (action.meta.form === 'sentToSapReport') {
            if (action.meta.field === 'scenario') {
                return Object.assign({}, state, { scenario: action.payload, scenarioChangeToggler: !state.scenarioChangeToggler });
            }
            if (action.meta.field === 'segment') {
                return Object.assign({}, state, { segment: action.payload, segmentChangeToggler: !state.segmentChangeToggler });
            }
        }
        return state;
    }
    case BUILD_SEND_TO_SAP_REPORT_EXECUTION_FILTERS: {
        return Object.assign({}, state, {
            filters: {
                executionId: action.execution.executionId,
                categoryName: 'Segmento',
                elementName: action.execution.segment,
                nodeName: action.execution.nodeName,
                initialDate: dateService.parse(action.execution.startDate),
                finalDate: dateService.parse(action.execution.endDate),
                reportType: constants.Report['SendToSapStatesReport' || action.execution.name]
            }
        });
    }
    case RESET_PERIOD_SEND_TO_SAP_REPORT: {
        return Object.assign({}, state, {
            officialPeriods: {
                defaultYear: null,
                officialPeriods: []
            }
        });
    }
    case RECEIVE_SAP_REPORT_OFFICIAL_PERIODS: {
        return Object.assign({}, state, {
            officialPeriods: action.periods
        });
    }
    case RECEIVE_SENT_TO_SAP_REPORT:
        return Object.assign({},
            state,
            {
                reportToggler: !state.reportToggler,
                executionId: action.executionId,
                filters: state.filters,
                status: action.status
            });
    case FAILED_RECEIVE_SENT_TO_SAP_REPORT:
        return Object.assign({}, state,
            {
                errorSaveToggler: !state.errorSaveToggler,
                status: action.status
            });
    default:
        return state;
    }
};
