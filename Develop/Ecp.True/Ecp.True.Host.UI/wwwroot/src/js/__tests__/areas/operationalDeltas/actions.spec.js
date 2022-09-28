import * as actions from '../../../modules/transportBalance/operationalDelta/actions';
import { apiService } from '../../../common/services/apiService';

describe('Actions for Operational Delta', () => {
    it('should init Delta Segment.', () => {
        const INIT_DELTA_SEGMENT = 'INIT_DELTA_SEGMENT';
        const segment = {};
        const action = actions.initDeltaSegment(segment);

        expect(action.type).toEqual(INIT_DELTA_SEGMENT);
        expect(action.segment).toEqual(segment);
    });

    it('should init Start Date.', () => {
        const INIT_DELTA_START_DATE = 'INIT_DELTA_START_DATE';
        const startDate = '04-06-2010';
        const action = actions.initDeltaStartDate(startDate);

        expect(action.type).toEqual(INIT_DELTA_START_DATE);
        expect(action.startDate).toEqual(startDate);
    });

    it('should set Delta End Date', () => {
        const SET_DELTA_END_DATE = 'SET_DELTA_END_DATE';
        const action = actions.setDeltaEndDate();
        expect(action.type).toEqual(SET_DELTA_END_DATE);
    });


    it('should init Delta Ticket', () => {
        const INIT_DELTA_TICKET = 'INIT_DELTA_TICKET';
        const action = actions.initDeltaTicket();
        expect(action.type).toEqual(INIT_DELTA_TICKET);
    });

    it('should set Delta Ticket', () => {
        const SET_DELTA_TICKET = 'SET_DELTA_TICKET';
        const action = actions.setDeltaTicket();
        expect(action.type).toEqual(SET_DELTA_TICKET);
    });

    it('Should request End Date', () => {
        const REQUEST_END_DATE = 'REQUEST_END_DATE';
        const segmentId = '01';
        const endDate = '04-06-2010';

        const action = actions.requestEndDate();

        expect(action.type).toEqual(REQUEST_END_DATE);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownership.getLastOwnershipPerformedDateForSelectedSegment());
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(endDate);
        const RECEIVE_END_DATE = 'RECEIVE_END_DATE';
        expect(receiveAction.type).toEqual(RECEIVE_END_DATE);
    });

    it('should receive End Date ticket.', () => {
        const RECEIVE_END_DATE = 'RECEIVE_END_DATE';
        const date = '04-06-2010';
        const action = actions.receivedEndDate(date);

        expect(action.type).toEqual(RECEIVE_END_DATE);
        expect(action.date).toEqual(date);
    });

    it('Should request Pending Inventories', () => {
        const REQUEST_PENDING_INVENTORIES = 'REQUEST_PENDING_INVENTORIES';
        const apiURL = apiService.operationalDelta.getPendingInventories();
        const deltaTicket = {};
        const name = 'pendingInventoriesGrid'

        const action = actions.requestPendingInventories(apiURL, deltaTicket, name);

        expect(action.type).toEqual(REQUEST_PENDING_INVENTORIES);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.operationalDelta.getPendingInventories());
        expect(action.fetchConfig.success).toBeDefined();
    });

    it('Should request Pending Movements', () => {
        const REQUEST_PENDING_MOVEMENTS = 'REQUEST_PENDING_MOVEMENTS';
        const apiURL = apiService.operationalDelta.getPendingMovements();
        const deltaTicket = {};
        const name = 'pendingMovementsGrid'

        const action = actions.requestPendingMovements(apiURL, deltaTicket, name);

        expect(action.type).toEqual(REQUEST_PENDING_MOVEMENTS);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.operationalDelta.getPendingMovements());
        expect(action.fetchConfig.success).toBeDefined();
    });

    it('Should Save Delta Calculation Ticket', () => {
        const SAVE_DELTA_CALCULATION = 'SAVE_DELTA_CALCULATION';
        const apiURL = apiService.operationalDelta.saveOperationalDelta();
        const deltaTicket = {};
        const action = actions.saveDeltaCalculation(deltaTicket);

        expect(action.type).toEqual(SAVE_DELTA_CALCULATION);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiURL);
        expect(action.fetchConfig.success).toBeDefined();
    });

    it('should receive Delta Calculation Status.', () => {
        const RECEIVE_SAVE_DELTA_CALCULATION = 'RECEIVE_SAVE_DELTA_CALCULATION';
        const status = 'Success';

        const action = actions.receiveSaveDeltaCalculation(status);

        expect(action.type).toEqual(RECEIVE_SAVE_DELTA_CALCULATION);
        expect(action.status).toEqual(status);
    });
});
