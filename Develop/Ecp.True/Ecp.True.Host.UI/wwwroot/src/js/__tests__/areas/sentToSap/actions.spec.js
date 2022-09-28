import * as actions from '../../../modules/dailyBalance/sentToSap/actions';
import { systemConfigService } from '../../../common/services/systemConfigService';

describe('Actions for sentToSap', () => {
    it('should get ticket', () => {
        const ticketId = 1;
        const json = {
            ticketId: 1
        };
        const action = actions.requestGetTicket(ticketId);

        expect(action.type).toEqual(actions.REQUEST_GET_TICKET);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(json);
        expect(receiveAction.type).toEqual(actions.RECEIVE_GET_TICKET);
        expect(receiveAction.ticket).toEqual(json);
    });

    it('should forward to sap', () => {
        const logisticsMovements = {
            ticketId: 1,
            movements: [1, 2]
        };
        const action = actions.requestForwardToSap(logisticsMovements);

        expect(action.type).toEqual(actions.REQUEST_FORWARD_MOVEMENTS);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success();
        expect(receiveAction.type).toEqual(actions.COMPLETE_FORWARD_PROCESS);
    });

    it('should get movements', () => {
        const request = {
            name: 'movements',
            ticketId: 1,
            data: jest.fn(data => data)
        };
        const json = {}

        const action = actions.requestMovements(request.name, request.ticketId, request.data);

        expect(action.type).toEqual(actions.REQUEST_MOVEMENTS);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(json);
        expect(receiveAction.items).toEqual(json);
        expect(receiveAction.type).toEqual('RECEIVE_GRID_DATA');
    });

    it('should confirm sent to sap', () => {
        const action = actions.confirmSentToSap();
        expect(action.type).toEqual(actions.CONFIRM_SENT_SAP);
    });

    it('should confirm init sent to sap', () => {
        const action = actions.initSentToSap();
        expect(action.type).toEqual(actions.INIT_SENTTOSAP_PROCESS);
    });

    it('should init movements confirmation', () => {
        const request = {
            name: 'movements',
            selectedMovements: [],
            countTotalMovements: 1
        };

        const action = actions.initMovementsConfirmation(request.name, request.selectedMovements, request.countTotalMovements);
        expect(action.type).toEqual(actions.INIT_MOVEMENTS_CONFIRMATION);
    });

    it('should request send movements success', () => {
        const movements = [];

        const action = actions.requestSendMovements(movements);
        expect(action.type).toEqual(actions.REQUEST_SEND_MOVEMENTS);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success();
        expect(receiveAction.type).toEqual(actions.COMPLETE_SENTTOSAP_PROCESS);
    });

    it('should request send movements failure', () => {
        const movements = [];
        const response = {
            errorCodes: [{
                message: 'Error'
            }]
        };

        const action = actions.requestSendMovements(movements);
        expect(action.type).toEqual(actions.REQUEST_SEND_MOVEMENTS);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.failure).toBeDefined();

        const receiveAction = action.fetchConfig.failure(response);
        expect(receiveAction.type).toEqual(actions.RECEIVE_FAILURE_SENTTOSAP_PROCESS);
    });

    it('should get official periods', () => {
        const segmentId = 1;
        const years = 1;
        const periods = '';

        const action = actions.getOfficialPeriods(segmentId, years);
        expect(action.type).toEqual(actions.REQUEST_SAP_OFFICIAL_PERIODS);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(periods);
        expect(receiveAction.type).toEqual(actions.RECEIVE_SAP_OFFICIAL_PERIODS);
    });

    it('should save confirm wizard data', () => {
        const ticketId = 1;

        const action = actions.saveConfirmWizardData(ticketId);
        expect(action.type).toEqual(actions.SAVE_CONFIRM_WIZARD_DATA);
    });

    it('should reset confirm wizard data', () => {
        const action = actions.resetConfirmWizardData();
        expect(action.type).toEqual(actions.RESET_CONFIRM_WIZARD_DATA);
    });

    it('should request nodes in ticket success', () => {
        const ticketId = 1;
        const json = {};

        const action = actions.requestNodesInTicket(ticketId);
        expect(action.type).toEqual(actions.REQUEST_NODES_IN_TICKET);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(json);
        expect(receiveAction.type).toEqual(actions.RECEIVE_NODES_IN_TICKET);
    });

    it('should request nodes in ticket failure', () => {
        const ticketId = 1;

        const action = actions.requestNodesInTicket(ticketId);
        expect(action.type).toEqual(actions.REQUEST_NODES_IN_TICKET);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.failure).toBeDefined();

        const receiveAction = action.fetchConfig.failure();
        expect(receiveAction.type).toEqual(actions.RECEIVE_FAILURE_NODES_IN_TICKET);
    });

    it('should reset official periods', () => {
        const action = actions.resetOfficialPeriods();
        expect(action.type).toEqual(actions.RESET_SAP_OFFICIAL_PERIODS);
    });

    it('should request get ticket failure', () => {
        const ticketId = 1;

        const action = actions.requestGetTicket(ticketId);
        expect(action.type).toEqual(actions.REQUEST_GET_TICKET);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.failure).toBeDefined();

        const receiveAction = action.fetchConfig.failure();
        expect(receiveAction.type).toEqual(actions.RECEIVE_FAILURE_GET_TICKET);
    });

    it('should confirm cancel batch ', () => {
        const action = actions.confirmCancelBatch();
        expect(action.type).toEqual(actions.CONFIRM_CANCEL_BATCH);
    });

    it('should request cancel batch success', () => {
        const ticketId = 1;
        const response = {};

        const action = actions.requestCancelBatch(ticketId);
        expect(action.type).toEqual(actions.REQUEST_CANCEL_BATCH);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(response);
        expect(receiveAction.type).toEqual(actions.COMPLETE_CANCEL_BATCH_PROCESS);
    });

    it('should request cancel batch failure', () => {
        const ticketId = 1;
        const response = {
            errorCodes: [{
                message: 'Error'
            }]
        };

        const action = actions.requestCancelBatch(ticketId);
        expect(action.type).toEqual(actions.REQUEST_CANCEL_BATCH);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.failure).toBeDefined();

        const receiveAction = action.fetchConfig.failure(response);
        expect(receiveAction.type).toEqual(actions.RECEIVE_FAILURE_CANCEL_BATCH_PROCESS);
    });

    it('should reset sap from data', () => {
        const action = actions.resetSapFromData();
        expect(action.type).toEqual(actions.RESET_SAP_FORM_DATA);
    });

    it('should set sap ticket info', () => {
        const ticket = {};

        const action = actions.setSapTicketInfo(ticket);
        expect(action.type).toEqual(actions.SET_SAP_TICKET_INFO);
    });

    it('should receive sap ticket validations', () => {
        const validations = {};

        const action = actions.receiveSapTicketValidations(validations);
        expect(action.type).toEqual(actions.RECEIVE_SAP_TICKET_VALIDATIONS);
    });

    it('should resetSapTicketValidations', () => {
        const action = actions.resetSapTicketValidations();
        expect(action.type).toEqual(actions.RESET_SAP_TICKET_VALIDATIONS);
    });

    it('should request save ticket success', () => {
        const ticket = {};

        const action = actions.requestSaveTicket(ticket);
        expect(action.type).toEqual(actions.REQUEST_SAP_TICKET);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success();
        expect(receiveAction.type).toEqual(actions.RECEIVE_SAP_TICKET);
    });

    it('should request save ticket failure', () => {
        const ticket = {};

        const action = actions.requestSaveTicket(ticket);
        expect(action.type).toEqual(actions.REQUEST_SAP_TICKET);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.failure).toBeDefined();

        const receiveAction = action.fetchConfig.failure();
        expect(receiveAction.type).toEqual(actions.RECEIVE_SAP_TICKET_FAILURE);
    });

    it('should request search nodes success', () => {
        const elementId = 1;
        const searchText = 'text';
        const json = {};
        systemConfigService.getAutocompleteItemsCount = jest.fn(key => key);;

        const action = actions.requestSearchNodes(elementId, searchText);
        expect(action.type).toEqual(actions.REQUEST_SAP_SEARCH_NODES);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(json);
        expect(receiveAction.type).toEqual(actions.RECEIVE_SAP_SEARCH_NODES);
    });

    it('should request search nodes failure', () => {
        const elementId = 1;
        const searchText = 'text';
        systemConfigService.getAutocompleteItemsCount = jest.fn(key => key);;

        const action = actions.requestSearchNodes(elementId, searchText);
        expect(action.type).toEqual(actions.REQUEST_SAP_SEARCH_NODES);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.failure).toBeDefined();

        const receiveAction = action.fetchConfig.failure();
        expect(receiveAction.type).toEqual(actions.CLEAR_SAP_SEARCH_NODES);
    });
});
