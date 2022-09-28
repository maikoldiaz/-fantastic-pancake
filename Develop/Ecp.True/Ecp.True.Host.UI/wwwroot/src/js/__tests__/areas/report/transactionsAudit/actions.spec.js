import * as actions from '../../../../modules/report/transactionsAudit/actions';

it('should save Inventory Audit chart', () => {
    const TRANSACTIONS_AUDIT_SAVE_FILTER = 'TRANSACTIONS_AUDIT_SAVE_FILTER';
    const filters = { elementId: 5, name: 'CASTILLA' };
    const action = actions.saveTransactionsAuditFilter(filters);

    expect(action.type).toEqual(TRANSACTIONS_AUDIT_SAVE_FILTER);
    expect(action.data).toEqual(filters);
});

it('should get selected element', () => {
    const TRANSACTIONS_AUDIT_SELECT_ELEMENT = 'TRANSACTIONS_AUDIT_SELECT_ELEMENT';
    const selectedElement = 'Transporte';
    const action = actions.onSelectedElement(selectedElement);

    expect(action.type).toEqual(TRANSACTIONS_AUDIT_SELECT_ELEMENT);
    expect(action.element).toEqual(selectedElement);
});

it('should reset Inventory Audit chart', () => {
    const RESET_TRANSACTIONS_AUDIT_FILTER = 'RESET_TRANSACTIONS_AUDIT_FILTER';
    const action = actions.resetTransactionsAuditFilter();

    expect(action.type).toEqual(RESET_TRANSACTIONS_AUDIT_FILTER);
});

it('should set setting audit report back navigation', () => {
    const SET_BACK_NAVIGATION_TRANSACTIONS_AUDIT = 'SET_BACK_NAVIGATION_TRANSACTIONS_AUDIT';
    const action = actions.setBackNavigation();

    expect(action.type).toEqual(SET_BACK_NAVIGATION_TRANSACTIONS_AUDIT);
});

it('should reset setting audit report back navigation', () => {
    const RESET_BACK_NAVIGATION_STRANSACTIONS_AUDIT = 'RESET_BACK_NAVIGATION_STRANSACTIONS_AUDIT';
    const action = actions.resetBackNavigation();

    expect(action.type).toEqual(RESET_BACK_NAVIGATION_STRANSACTIONS_AUDIT);
});

