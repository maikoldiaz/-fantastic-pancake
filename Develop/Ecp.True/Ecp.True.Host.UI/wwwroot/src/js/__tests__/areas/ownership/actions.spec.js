import * as actions from '../../../modules/transportBalance/ownership/actions';
import { apiService } from '../../../common/services/apiService';

describe('Actions for ownership balance', () => {
    it('should request Ownership Calculation Dates', () => {
        const actionType = 'REQUEST_OWNERSHIP_CALCULATION_DATES';
        const segmentId = 1;
        const dates = {};
        const action = actions.requestOwnershipCalculationDates(segmentId);

        expect(action.type).toEqual(actionType);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownership.getDates(segmentId));
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(dates);
        expect(receiveAction.type).toEqual(actions.RECEIVE_OWNERSHIP_CALCULATION_DATES);
        expect(receiveAction.dates).toEqual(dates);
    });

    it('should set Ownership Calculation Info', () => {
        const actionType = 'SET_OWNERSHIP_CALCULATION_INFO';
        const ticket = {};
        const action = actions.setOwnershipCalculationInfo(ticket);

        expect(action.type).toEqual(actionType);
        expect(action.ticket).toEqual(ticket);
    });

    it('should request Ownership Calculation Validations', () => {
        const actionType = 'REQUEST_OWNERSHIP_CALCULATION_VALIDATIONS';
        const data = null;
        const validations = {};

        const action = actions.requestOwnershipCalculationValidations(data);

        expect(action.type).toEqual(actionType);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownership.validateNodes());
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(validations);
        expect(receiveAction.type).toEqual(actions.RECEIVE_OWNERSHIP_CALCULATION_VALIDATIONS);
        expect(receiveAction.validations).toEqual(validations);
    });

    it('should execute Ownership Calculation', () => {
        const actionType = 'CREATE_OWNERSHIP_CALCULATION';
        const ticket = {};
        const status = null;

        const action = actions.executeOwnershipCalculation(ticket);

        expect(action.type).toEqual(actionType);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownership.saveOwnershipCalculation());
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(status);
        expect(receiveAction.type).toEqual(actions.RECEIVE_CREATE_OWNERSHIP_CALCULATION);
        expect(receiveAction.status).toEqual(status);
    });

    it('should request REQUEST_CONCILIATION_TICKET', () => {
        const ticketId = 123;
        const action = actions.requestConciliation(ticketId);

        expect(action.type).toEqual(actions.REQUEST_CONCILIATION_TICKET);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.conciliation.requestConciliation(ticketId));
        expect(action.fetchConfig.success).toBeDefined();
        expect(action.fetchConfig.failure).toBeDefined();

        const receiveSuccessAction = action.fetchConfig.success();
        expect(receiveSuccessAction.type).toEqual(actions.RECEIVE_CONCILIATION_TICKET);

        const receiveFailAction = action.fetchConfig.failure({});
        expect(receiveFailAction.type).toEqual(actions.FAILURE_CONCILIATION_TICKET);
        expect(receiveFailAction.response).toEqual({});
    });

    it('should request REQUEST_OWNERSHIPNODE_TICKET_DATA', () => {
        const ticketId = 123;
        const action = actions.requestOwnershipNodeData(ticketId);

        expect(action.type).toEqual(actions.REQUEST_OWNERSHIPNODE_TICKET_DATA);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownershipNode.queryByTicketId(ticketId));
        expect(action.fetchConfig.success).toBeDefined();
        expect(action.fetchConfig.failure).toBeDefined();

        const receiveSuccessAction = action.fetchConfig.success({});
        expect(receiveSuccessAction.type).toEqual(actions.RECEIVE_OWNERSHIPNODE_TICKET_DATA);
        expect(receiveSuccessAction.ownershipNodesData).toBeDefined();

        const receiveFailAction = action.fetchConfig.failure();
        expect(receiveFailAction.type).toEqual(actions.FAILURE_OWNERSHIPNODE_TICKET_DATA);
    });

    it('should request REQUEST_LAST_OPERATIONAL_CONCILIATION_TICKET', () => {
        const statusFilter = ['APPROVED'];
        const action = actions.requestLastOperationalTicket(statusFilter);

        expect(action.type).toEqual(actions.REQUEST_LAST_OPERATIONAL_CONCILIATION_TICKET);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ticket.getLastOperationalTicketsPerSegmentWithStatus(statusFilter));
        expect(action.fetchConfig.success).toBeDefined();

        const receiveSuccessAction = action.fetchConfig.success({});
        expect(receiveSuccessAction.type).toEqual(actions.RECEIVE_LAST_OPERATIONAL_CONCILIATION_TICKET);
        expect(receiveSuccessAction.ticket).toBeDefined();
    });
});
