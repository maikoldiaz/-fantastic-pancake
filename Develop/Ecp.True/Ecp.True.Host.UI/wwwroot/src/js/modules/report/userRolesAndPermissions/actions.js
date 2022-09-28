import { apiService } from '../../../common/services/apiService';

export const BUILD_USER_ROLE_PERMISSION_REPORT_FILTER = 'BUILD_USER_ROLE_PERMISSION_REPORT_FILTER';
export const CLEAN_USER_ROLE_PERMISSION_REPORT_FILTER = 'CLEAN_USER_ROLE_PERMISSION_REPORT_FILTER';
export const REQUEST_USER_ROLE_PERMISSION_REPORT = 'REQUEST_USER_ROLE_PERMISSION_REPORT';
export const RECEIVE_USER_ROLE_PERMISSION_REPORT = 'RECEIVE_USER_ROLE_PERMISSION_REPORT';
export const FAILED_USER_ROLE_PERMISSION_REPORT = 'FAILED_USER_ROLE_PERMISSION_REPORT';

export const buildUserRolePermissionReportFilter = execution => {
    return {
        type: BUILD_USER_ROLE_PERMISSION_REPORT_FILTER,
        execution
    };
};

export const cleanUserRolePermissionReportFilter = () => {
    return {
        type: CLEAN_USER_ROLE_PERMISSION_REPORT_FILTER
    };
};

const receivedUserRolePermissionReport = () => {
    return {
        type: RECEIVE_USER_ROLE_PERMISSION_REPORT
    };
};
const failedUserRolePermissionReport = () => {
    return {
        type: FAILED_USER_ROLE_PERMISSION_REPORT
    };
};
export const requestUserRolePermissionReport = body => {
    return {
        type: REQUEST_USER_ROLE_PERMISSION_REPORT,
        fetchConfig: {
            path: apiService.reports.requestUserRolePermissionReport(),
            method: 'POST',
            body,
            success: receivedUserRolePermissionReport,
            failure: failedUserRolePermissionReport
        }
    };
};
