import { constants } from '../../../common/services/constants';
import {
    TRANSACTIONS_AUDIT_SAVE_FILTER,
    TRANSACTIONS_AUDIT_SELECT_ELEMENT,
    RESET_TRANSACTIONS_AUDIT_FILTER,
    SET_BACK_NAVIGATION_TRANSACTIONS_AUDIT,
    RESET_BACK_NAVIGATION_STRANSACTIONS_AUDIT } from './actions';
import { dateService } from '../../../common/services/dateService';

const transactionAuditInitialState = {
    initialValues: {
        reportType: constants.Report.MovementAuditReport
    }
};

export const transactionsAudit = (state = transactionAuditInitialState, action = {}) => {
    switch (action.type) {
    case TRANSACTIONS_AUDIT_SELECT_ELEMENT:
        return Object.assign({},
            state,
            {
                formValues: {
                    element: action.element,
                    reportType: constants.Report.MovementAuditReport
                }
            });
    case TRANSACTIONS_AUDIT_SAVE_FILTER:
        return Object.assign({},
            state,
            {
                filters: {
                    elementName: action.data.element.name,
                    elementId: action.data.element.elementId,
                    initialDate: dateService.convertToColombian(action.data.initialDate),
                    finalDate: dateService.convertToColombian(action.data.finalDate),
                    reportType: action.data.reportType
                },
                formValues: {
                    element: action.data.element,
                    initialDate: action.data.initialDate,
                    finalDate: action.data.finalDate,
                    reportType: action.data.reportType
                }
            });
    case RESET_TRANSACTIONS_AUDIT_FILTER:
        return Object.assign({},
            state,
            {
                initialValues: {
                    initialDate: null,
                    finalDate: null,
                    element: null,
                    reportType: constants.Report.MovementAuditReport
                },
                filters: {
                    initialDate: null,
                    finalDate: null,
                    elementId: null,
                    reportType: constants.Report.MovementAuditReport
                }
            });
    case SET_BACK_NAVIGATION_TRANSACTIONS_AUDIT:
        return Object.assign({}, state, { backNavigation: true, initialValues: state.formValues });
    case RESET_BACK_NAVIGATION_STRANSACTIONS_AUDIT:
        return Object.assign({}, state, { backNavigation: false });
    default:
        return state;
    }
};
