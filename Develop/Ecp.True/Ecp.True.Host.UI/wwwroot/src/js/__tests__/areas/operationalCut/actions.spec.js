import * as actions from '../../../modules/transportBalance/cutOff/actions';
import { apiService } from '../../../common/services/apiService';

describe('Actions for Operational cut', () => {
    it('should set pending transaction errors.', () => {
        const SET_PENDING_TRANSACTION_ERRORS = 'SET_PENDING_TRANSACTION_ERRORS';
        const pendingTransactionErrors = {};
        const action = actions.setPendingTransactionErrors(pendingTransactionErrors);

        expect(action.type).toEqual(SET_PENDING_TRANSACTION_ERRORS);
        expect(action.pendingTransactionErrors).toEqual(pendingTransactionErrors);
    });

    it('should set unbalances.', () => {
        const SET_UNBALANCES = 'SET_UNBALANCES';
        const unbalances = {};
        const action = actions.setUnbalances(unbalances);

        expect(action.type).toEqual(SET_UNBALANCES);
        expect(action.unbalances).toEqual(unbalances);
    });

    it('should set Transfer Point Movements', () => {
        const SET_TRANSFER_POINT_MOVEMENTS = 'SET_TRANSFER_POINT_MOVEMENTS';
        const officialMovements = {};
        const action = actions.setTransferPointMovements(officialMovements);

        expect(action.type).toEqual(SET_TRANSFER_POINT_MOVEMENTS);
        expect(action.officialMovements).toEqual(officialMovements);
    });

    it('should receive last ticket.', () => {
        const RECEIVE_LAST_TICKET = 'RECEIVE_LAST_TICKET';
        const ticket = {};
        const action = actions.receiveLastTicket(ticket);

        expect(action.type).toEqual(RECEIVE_LAST_TICKET);
        expect(action.ticket).toEqual(ticket);
    });

    it('should set ticket information', () => {
        const actionType = 'SET_TICKET_INFO';
        const ticket = {};
        const action = actions.setTicketInfo(ticket);

        expect(action.type).toEqual(actionType);
        expect(action.ticket).toEqual(ticket);
    });

    it('Should request last ticket', () => {
        const actionType = 'REQUEST_LAST_TICKET';
        const lastTicket = {};

        const action = actions.requestLastTicket();

        expect(action.type).toEqual(actionType);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ticket.getLastTicket());
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(lastTicket);
        const RECEIVE_LAST_TICKET = 'RECEIVE_LAST_TICKET';
        expect(receiveAction.type).toEqual(RECEIVE_LAST_TICKET);
    });

    it('Request pending transacton errors.', () => {
        const actionType = 'REQUEST_PENDING_TRANSACTION_ERRORS';
        const ticket = {};
        const name = 'test';
        const action = actions.requestPendingTransactions(apiService.operationalCutOff.getPendingTransactionErrors(), ticket, name);

        expect(action.type).toEqual(actionType);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.operationalCutOff.getPendingTransactionErrors());
        expect(action.fetchConfig.success).toBeDefined();
        expect(action.fetchConfig.failure).toBeDefined();
        const receivedAction = action.fetchConfig.success([], name);
        expect(receivedAction.type).toEqual('RECEIVE_GRID_DATA');
    });

    it('Request unbalances.', () => {
        const actionType = 'REQUEST_UNBALANCES';
        const ticket = {};
        const name = 'test';
        const action = actions.requestUnbalances(apiService.operationalCutOff.getUnbalances(), ticket, name);

        expect(action.type).toEqual(actionType);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.operationalCutOff.getUnbalances());
        expect(action.fetchConfig.success).toBeDefined();
        const receivedAction = action.fetchConfig.success([], name);
        expect(receivedAction.type).toEqual('RECEIVE_GRID_DATA');
    });

    it('Request save ticket.', () => {
        const actionType = 'REQUEST_SAVE_OPERATIONAL_CUTOFF';
        const operationalCutOff = { ticket: { categoryElementId: 1 } };
        const action = actions.requestSaveTicket(operationalCutOff);

        expect(action.type).toEqual(actionType);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.operationalCutOff.saveOperationalCutOff());
        expect(action.fetchConfig.success).toBeDefined();
        const receivedAction = action.fetchConfig.success();
        expect(receivedAction.type).toEqual('RECEIVE_SAVE_OPERATIONAL_CUTOFF');
    });

    it('should init CutOff', () => {
        const INIT_CUTOFF_PROCESS = 'INIT_CUTOFF_PROCESS';
        const action = actions.initCutOff();
        expect(action.type).toEqual(INIT_CUTOFF_PROCESS);
    });

    it('should increment CutOff', () => {
        const CUTOFF_PROCESS_START = 'CUTOFF_PROCESS_START';
        const action = actions.incrementCutOff();
        expect(action.type).toEqual(CUTOFF_PROCESS_START);
    });

    it('should add comment', () => {
        const ADD_CUTOFF_COMMENT = 'ADD_CUTOFF_COMMENT';
        const comments = '';
        const action = actions.addComment(comments);
        expect(action.type).toEqual(ADD_CUTOFF_COMMENT);
        expect(action.comment).toEqual(comments);
    });

    it('should init add Comment', () => {
        const INIT_ADD_CUTOFF_COMMENT = 'INIT_ADD_CUTOFF_COMMENT';

        const ticketObject = {
            preText: '',
            name: '',
            postText: '',
            count: 1
        };
        const action = actions.intAddComment(ticketObject.name, ticketObject.preText, ticketObject.postText, ticketObject.count);
        expect(action.type).toEqual(INIT_ADD_CUTOFF_COMMENT);
        expect(action.preText).toEqual(ticketObject.preText);
        expect(action.name).toEqual(ticketObject.name);
        expect(action.postText).toEqual(ticketObject.postText);
        expect(action.count).toEqual(ticketObject.count);
    });

    it('should confirm cut off', () => {
        const CONFIRM_CUT_OFF = 'CONFIRM_CUT_OFF';
        const action = actions.confirmCutoff();
        expect(action.type).toEqual(CONFIRM_CUT_OFF);
    });
    it('should receive ticket details', () => {
        const RECEIVE_TICKET_DETAILS = 'RECEIVE_TICKET_DETAILS';
        const ticket = { ticketId: 100 };
        const action = actions.receiveTicketDetails(ticket);
        expect(action.type).toEqual(RECEIVE_TICKET_DETAILS);
        expect(action.ticket).toEqual(ticket);
    });
    it('should request ticket details', () => {
        const REQUEST_TICKET_DETAILS = 'REQUEST_TICKET_DETAILS';
        const ticketId = 100;

        const action = actions.requestTicketDetails(ticketId);

        expect(action.type).toEqual(REQUEST_TICKET_DETAILS);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ticket.getTicketInformation(ticketId));
        expect(action.fetchConfig.success).toBeDefined();
        const receivedAction = action.fetchConfig.success();
        expect(receivedAction.type).toEqual('RECEIVE_TICKET_DETAILS');
    });
    it('should reset ticket Details', () => {
        const RESET_TICKET_DETAILS = 'RESET_TICKET_DETAILS';
        const action = actions.resetTicketDetails();
        expect(action.type).toEqual(RESET_TICKET_DETAILS);
    });
    it('should initialize ticket error', () => {
        const SET_TICKET_ERROR = 'SET_TICKET_ERROR';
        const ticket = { ticketId: 100 };
        const action = actions.initializeTicketError(ticket);
        expect(action.type).toEqual(SET_TICKET_ERROR);
        expect(action.ticket).toEqual(ticket);
    });
    it('should initialize official points error', () => {
        const GET_OFFICIAL_POINT_ERROR = 'GET_OFFICIAL_POINT_ERROR';
        const row = { sapTrackingId: 1 };

        const action = actions.getSapTrackingErrors(row);

        expect(action.type).toEqual(GET_OFFICIAL_POINT_ERROR);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.operationalCutOff.getSapTrackingErrors(row.sapTrackingId));
        expect(action.fetchConfig.success).toBeDefined();
        const receivedAction = action.fetchConfig.success();
        expect(receivedAction.type).toEqual('SET_OFFICIAL_POINT_ERROR');
    });
    it('should init inventories validations', () => {
        const INIT_INVENTORIES_VALIDATIONS = 'INIT_INVENTORIES_VALIDATIONS';
        const action = actions.initInventoriesValidations();
        expect(action.type).toEqual(INIT_INVENTORIES_VALIDATIONS);
    });

    it('should request inventories validations', () => {
        const REQUEST_INVENTORIES_VALIDATIONS = 'REQUEST_INVENTORIES_VALIDATIONS';

        const action = actions.requestInventoriesValidations();

        expect(action.type).toEqual(REQUEST_INVENTORIES_VALIDATIONS);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.operationalCutOff.validateInitialInventory());
        expect(action.fetchConfig.success).toBeDefined();
        const receivedAction = action.fetchConfig.success();
        expect(receivedAction.type).toEqual('RECEIVE_INVENTORIES_VALIDATIONS');
    });

    it('should request Transfer Point Movements', () => {
        const url = '';
        const ticket = {
            categoryElementId: 10,
            startDate: '2020-05-06',
            endDate: '2020-05-26'
        };
        const name = 'officialPoints';
        const REQUEST_TRANSFER_POINT_MOVEMENTS = 'REQUEST_TRANSFER_POINT_MOVEMENTS';
        const action = actions.requestTransferPointMovements(url, ticket, name);

        expect(action.type).toEqual(REQUEST_TRANSFER_POINT_MOVEMENTS);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(url);
        expect(action.fetchConfig.body).toEqual(ticket);
        expect(action.fetchConfig.success).toBeDefined();

        const receivedAction = action.fetchConfig.success([], name);
        expect(receivedAction.type).toEqual('RECEIVE_GRID_DATA');
    });

    it('should request update cut off comment', () => {
        const body = {};
        const REQUEST_UPDATE_CUTOFF_COMMENT = 'REQUEST_UPDATE_CUTOFF_COMMENT';
        const action = actions.requestUpdateCutOffComment(body);

        expect(action.type).toEqual(REQUEST_UPDATE_CUTOFF_COMMENT);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.operationalCutOff.updateComment());
        expect(action.fetchConfig.body).toEqual(body);
        expect(action.fetchConfig.success).toBeDefined();
        expect(action.fetchConfig.failure).toBeDefined();

        const receivedSuccessAction = action.fetchConfig.success();
        expect(receivedSuccessAction.type).toEqual('RECEIVE_UPDATE_CUTOFF_COMMENT');
        const receivedFailureAction = action.fetchConfig.failure();
        expect(receivedFailureAction.type).toEqual('RECEIVE_UPDATE_CUTOFF_COMMENT_FAILURE');
    });

    it('should request first time nodes', () => {
        const REQUEST_FIRSTTIME_NODES = 'REQUEST_FIRSTTIME_NODES';

        const action = actions.requestFirstTimeNodes();

        expect(action.type).toEqual(REQUEST_FIRSTTIME_NODES);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.operationalCutOff.getFirstTimeNodes());
        expect(action.fetchConfig.success).toBeDefined();
        const receivedAction = action.fetchConfig.success();
        expect(receivedAction.type).toEqual('RECEIVE_FIRSTTIME_NODES');
    });
});
