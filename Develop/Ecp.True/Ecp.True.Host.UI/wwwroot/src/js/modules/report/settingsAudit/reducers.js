import { SETTINGS_AUDIT_REPORT_SAVE_FILTER, SETTINGS_AUDIT_REPORT_RESET_FILTER, SET_BACK_NAVIGATION_SETTINGS_AUDIT, RESET_BACK_NAVIGATION_SETTINGS_AUDIT } from './actions';
import { constants } from '../../../common/services/constants';
import { dateService } from '../../../common/services/dateService';

const filterInitialState = {
    initialValues: {
        initialDate: null,
        finalDate: null
    },
    backNavigation: false
};

export const settingsAuditReport = (state = filterInitialState, action = {}) => {
    switch (action.type) {
    case SETTINGS_AUDIT_REPORT_SAVE_FILTER:
        return Object.assign({},
            state,
            {
                filters: {
                    initialDate: dateService.convertToColombian(action.filters.initialDate),
                    finalDate: dateService.convertToColombian(action.filters.finalDate),
                    reportType: constants.Report.SettingsAuditReport
                },
                formValues: {
                    initialDate: action.filters.initialDate,
                    finalDate: action.filters.finalDate
                }
            });
    case SETTINGS_AUDIT_REPORT_RESET_FILTER:
        return Object.assign({},
            state,
            {
                filters: {
                    initialDate: null,
                    finalDate: null,
                    reportType: null
                },
                initialValues: {
                    initialDate: null,
                    finalDate: null
                }
            });
    case SET_BACK_NAVIGATION_SETTINGS_AUDIT:
        return Object.assign({}, state, { backNavigation: true, initialValues: state.formValues });
    case RESET_BACK_NAVIGATION_SETTINGS_AUDIT:
        return Object.assign({}, state, { backNavigation: false });
    default:
        return state;
    }
};
