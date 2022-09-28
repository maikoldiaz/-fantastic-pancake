export const SETTINGS_AUDIT_REPORT_SAVE_FILTER = 'SETTINGS_AUDIT_REPORT_SAVE_FILTER';
export const settingsAuditReportSaveReportFilter = filters => {
    return {
        type: SETTINGS_AUDIT_REPORT_SAVE_FILTER,
        filters
    };
};

export const SETTINGS_AUDIT_REPORT_RESET_FILTER = 'SETTINGS_AUDIT_REPORT_RESET_FILTER';
export const settingsAuditReportResetReportFilter = () => {
    return {
        type: SETTINGS_AUDIT_REPORT_RESET_FILTER
    };
};

export const SET_BACK_NAVIGATION_SETTINGS_AUDIT = 'SET_BACK_NAVIGATION_SETTINGS_AUDIT';
export const setBackNavigation = () => {
    return {
        type: SET_BACK_NAVIGATION_SETTINGS_AUDIT
    };
};

export const RESET_BACK_NAVIGATION_SETTINGS_AUDIT = 'RESET_BACK_NAVIGATION_SETTINGS_AUDIT';
export const resetBackNavigation = () => {
    return {
        type: RESET_BACK_NAVIGATION_SETTINGS_AUDIT
    };
};
