import * as actions from '../../../../modules/report/transactionsAudit/actions';
import { transactionsAudit } from '../../../../modules/report/transactionsAudit/reducers';
import { constants } from '../../../../common/services/constants';
import { dateService } from '../../../../common/services/dateService';

describe('Reducers for transactionsAudit', () => {
    const inventoryAuditInitialState = {
        initialValues: {
            reportType: constants.Report.MovementAuditReport,
            element: {}
        },
        filters: {
            initialDate: null,
            finalDate: null,
            elementId: null,
            reportType: constants.Report.MovementAuditReport
        }
    };
    it('should handle action TRANSACTIONS_AUDIT_SELECT_ELEMENT',
        function () {
            const action = {
                type: actions.TRANSACTIONS_AUDIT_SELECT_ELEMENT,
                element: { elementId: 1, name: 'element' }
            };
            expect(transactionsAudit(inventoryAuditInitialState, action).formValues.reportType).toEqual(constants.Report.MovementAuditReport);
            expect(transactionsAudit(inventoryAuditInitialState, action).formValues.element.elementId).toEqual(action.element.elementId);
            expect(transactionsAudit(inventoryAuditInitialState, action).formValues.element.name).toEqual(action.element.name);
        });


    it('should handle action TRANSACTIONS_AUDIT_SAVE_FILTER',
        function () {
            const action = {
                type: actions.TRANSACTIONS_AUDIT_SAVE_FILTER,
                data: {
                    element: {
                        name: 'some name',
                        elementId: 1
                    },
                    initialDate: dateService.now(),
                    finalDate: dateService.now(),
                    reportType: constants.Report.InventoryAuditReport
                }
            };

            expect(transactionsAudit(inventoryAuditInitialState, action).filters.elementName).toEqual(action.data.element.name);
            expect(transactionsAudit(inventoryAuditInitialState, action).filters.elementId).toEqual(action.data.element.elementId);
            expect(transactionsAudit(inventoryAuditInitialState, action).filters.initialDate).toEqual(dateService.convertToColombian(action.data.initialDate));
            expect(transactionsAudit(inventoryAuditInitialState, action).filters.finalDate).toEqual(dateService.convertToColombian(action.data.finalDate));
            expect(transactionsAudit(inventoryAuditInitialState, action).filters.reportType).toEqual(action.data.reportType);

            expect(transactionsAudit(inventoryAuditInitialState, action).formValues.reportType).toEqual(action.data.reportType);
            expect(transactionsAudit(inventoryAuditInitialState, action).formValues.element.name).toEqual(action.data.element.name);
            expect(transactionsAudit(inventoryAuditInitialState, action).formValues.element.elementId).toEqual(action.data.element.elementId);
            expect(transactionsAudit(inventoryAuditInitialState, action).formValues.initialDate).toEqual(action.data.initialDate);
            expect(transactionsAudit(inventoryAuditInitialState, action).formValues.finalDate).toEqual(action.data.finalDate);
        });

    it('should handle action RESET_TRANSACTIONS_AUDIT_FILTER',
        function () {
            const action = {
                type: actions.RESET_TRANSACTIONS_AUDIT_FILTER,
                data: {
                    initialDate: null,
                    finalDate: null,

                    element: {
                        name: null,
                        elementId: null
                    },
                    reportType: constants.Report.MovementAuditReport
                }
            };
            expect(transactionsAudit(inventoryAuditInitialState, action).filters.elementName).toEqual(action.data.name);
            expect(transactionsAudit(inventoryAuditInitialState, action).filters.elementId).toEqual(action.data.element.elementId);
            expect(transactionsAudit(inventoryAuditInitialState, action).filters.initialDate).toEqual(action.data.initialDate);
            expect(transactionsAudit(inventoryAuditInitialState, action).filters.finalDate).toEqual(action.data.finalDate);
            expect(transactionsAudit(inventoryAuditInitialState, action).filters.reportType).toEqual(action.data.reportType);
            expect(transactionsAudit(inventoryAuditInitialState, action).initialValues.reportType).toEqual(action.data.reportType);
        });

    it('should handle action SET_BACK_NAVIGATION_TRANSACTIONS_AUDIT',
        function () {
            const action = {
                type: actions.SET_BACK_NAVIGATION_TRANSACTIONS_AUDIT
            };
            const newState = Object.assign({}, inventoryAuditInitialState, { backNavigation: true, initialValues: inventoryAuditInitialState.formValues });
            expect(transactionsAudit(inventoryAuditInitialState, action)).toEqual(newState);
        });

    it('should handle action RESET_BACK_NAVIGATION_STRANSACTIONS_AUDIT',
        function () {
            const action = {
                type: actions.SET_BACK_NAVIGATION_TRANSACTIONS_AUDIT
            };
            const newState = Object.assign({}, inventoryAuditInitialState, { backNavigation: true, initialValues: inventoryAuditInitialState.formValues });
            expect(transactionsAudit(inventoryAuditInitialState, action)).toEqual(newState);
        });

    it('should return default state',
        function () {
            const action = {
                type: 'dummy Value'
            };
            const newState = Object.assign({}, inventoryAuditInitialState);
            expect(transactionsAudit(inventoryAuditInitialState, action)).toEqual(newState);
        });
});
