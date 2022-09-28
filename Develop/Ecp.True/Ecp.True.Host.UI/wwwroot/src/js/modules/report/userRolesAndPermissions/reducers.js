import {
    BUILD_USER_ROLE_PERMISSION_REPORT_FILTER,
    CLEAN_USER_ROLE_PERMISSION_REPORT_FILTER,
    RECEIVE_USER_ROLE_PERMISSION_REPORT,
    FAILED_USER_ROLE_PERMISSION_REPORT
} from './actions';
import { constants } from '../../../common/services/constants';

export const userRolesAndPermissions = (state = {}, action = {}) => {
    switch (action.type) {
    case CLEAN_USER_ROLE_PERMISSION_REPORT_FILTER: {
        return Object.assign({}, state, {
            filters: null
        });
    }
    case BUILD_USER_ROLE_PERMISSION_REPORT_FILTER: {
        return Object.assign({}, state, {
            filters: {
                executionId: action.execution.executionId,
                reportType: constants.Report[action.execution.name],
                reportTypeName: action.execution.name
            }
        });
    }
    case RECEIVE_USER_ROLE_PERMISSION_REPORT: {
        return Object.assign({}, state, {
            receiveReportToggler: !state.receiveReportToggler
        });
    }
    case FAILED_USER_ROLE_PERMISSION_REPORT: {
        return Object.assign({}, state, {
            failureReportToggler: !state.failureReportToggler
        });
    }
    default:
        return state;
    }
};
