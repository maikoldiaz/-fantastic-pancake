export const TRANSACTIONS_AUDIT_SAVE_FILTER = 'TRANSACTIONS_AUDIT_SAVE_FILTER';
export const saveTransactionsAuditFilter = data => {
    return {
        type: TRANSACTIONS_AUDIT_SAVE_FILTER,
        data
    };
};

export const TRANSACTIONS_AUDIT_SELECT_ELEMENT = 'TRANSACTIONS_AUDIT_SELECT_ELEMENT';
export const onSelectedElement = element => {
    return {
        type: TRANSACTIONS_AUDIT_SELECT_ELEMENT,
        element
    };
};

export const RESET_TRANSACTIONS_AUDIT_FILTER = 'RESET_TRANSACTIONS_AUDIT_FILTER';
export const resetTransactionsAuditFilter = () => {
    return {
        type: RESET_TRANSACTIONS_AUDIT_FILTER
    };
};

export const SET_BACK_NAVIGATION_TRANSACTIONS_AUDIT = 'SET_BACK_NAVIGATION_TRANSACTIONS_AUDIT';
export const setBackNavigation = () => {
    return {
        type: SET_BACK_NAVIGATION_TRANSACTIONS_AUDIT
    };
};

export const RESET_BACK_NAVIGATION_STRANSACTIONS_AUDIT = 'RESET_BACK_NAVIGATION_STRANSACTIONS_AUDIT';
export const resetBackNavigation = () => {
    return {
        type: RESET_BACK_NAVIGATION_STRANSACTIONS_AUDIT
    };
};
