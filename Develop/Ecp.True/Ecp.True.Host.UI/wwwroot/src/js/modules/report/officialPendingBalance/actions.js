export const OFFICIAL_PENDING_BALANCE_SAVE_FILTER = 'OFFICIAL_PENDING_BALANCE_SAVE_FILTER';
export const saveOfficialPendingBalanceFilter = data => {
    return {
        type: OFFICIAL_PENDING_BALANCE_SAVE_FILTER,
        data
    };
};

export const OFFICIAL_PENDING_BALANCE_SELECT_ELEMENT = 'OFFICIAL_PENDING_BALANCE_SELECT_ELEMENT';
export const onSelectedElement = element => {
    return {
        type: OFFICIAL_PENDING_BALANCE_SELECT_ELEMENT,
        element
    };
};

export const RESET_OFFICIAL_PENDING_BALANCE_FILTER = 'RESET_OFFICIAL_PENDING_BALANCE_FILTER';
export const resetPendingBalanceFilter = () => {
    return {
        type: RESET_OFFICIAL_PENDING_BALANCE_FILTER
    };
};
