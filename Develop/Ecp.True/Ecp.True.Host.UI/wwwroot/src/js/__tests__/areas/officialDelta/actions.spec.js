import * as actions from '../../../modules/dailyBalance/officialDelta/actions';
import { apiService } from '../../../common/services/apiService';

it('should initialize official delta process', () => {
    const action = actions.initOfficialDelta();
    const INIT_OFFICIAL_DELTA_TICKET_PROCESS = 'INIT_OFFICIAL_DELTA_TICKET_PROCESS';
    const mock_action = {
        type: INIT_OFFICIAL_DELTA_TICKET_PROCESS
    };
    expect(action).toEqual(mock_action);
});

it('should request unapproved nodes for son', () => {
    const ticket = {};
    const REQUEST_OFFICIAL_DELTA_VALIDATION_DATA = 'REQUEST_OFFICIAL_DELTA_VALIDATION_DATA';
    const action = actions.getOfficialDeltaValidationData(ticket);

    expect(action.type).toEqual(REQUEST_OFFICIAL_DELTA_VALIDATION_DATA);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.officialDelta.getSonUnapproveNodes());
    expect(action.fetchConfig.body).toEqual(ticket);
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_OFFICIAL_DELTA_VALIDATION_DATA = 'RECEIVE_OFFICIAL_DELTA_VALIDATION_DATA';
    expect(receiveAction.type).toEqual(RECEIVE_OFFICIAL_DELTA_VALIDATION_DATA);
    expect(receiveAction.unapprovedNodes).toEqual(true);
});

it('should set is valid', () => {
    const isValid = false;
    const action = actions.setIsValid(isValid);
    const SET_IS_VALID = 'SET_IS_VALID';
    const mock_action = {
        type: SET_IS_VALID,
        isValid
    };
    expect(action).toEqual(mock_action);
});

it('should set official delta ticket', () => {
    const ticket = {};
    const action = actions.setOfficialDeltaTicketInfo(ticket);
    const SET_OFFICIAL_DELTA_TICKET = 'SET_OFFICIAL_DELTA_TICKET';
    const mock_action = {
        type: SET_OFFICIAL_DELTA_TICKET,
        ticket
    };
    expect(action).toEqual(mock_action);
});

it('should request save official delta ticket', () => {
    const ticket = {};
    const REQUEST_OFFICIAL_DELTA_TICKET = 'REQUEST_OFFICIAL_DELTA_TICKET';
    const action = actions.saveOfficialDelta(ticket);

    expect(action.type).toEqual(REQUEST_OFFICIAL_DELTA_TICKET);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.officialDelta.saveOfficialDelta());
    expect(action.fetchConfig.body).toEqual(ticket);
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_OFFICIAL_DELTA_TICKET = 'RECEIVE_OFFICIAL_DELTA_TICKET';
    expect(receiveAction.type).toEqual(RECEIVE_OFFICIAL_DELTA_TICKET);
});

it('should get official periods', () => {
    const segmentId = 10;
    const years = 5;
    const REQUEST_OFFICIAL_PERIODS = 'REQUEST_OFFICIAL_PERIODS';
    const action = actions.getOfficialPeriods(segmentId, years);

    expect(action.type).toEqual(REQUEST_OFFICIAL_PERIODS);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.officialDelta.getOfficialPeriods(segmentId, years, false));
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_OFFICIAL_PERIODS = 'RECEIVE_OFFICIAL_PERIODS';
    expect(receiveAction.type).toEqual(RECEIVE_OFFICIAL_PERIODS);
    expect(receiveAction.periods).toEqual(true);
});

it('should reset official periods', () => {
    const action = actions.resetOfficialPeriods();
    const RESET_OFFICIAL_PERIODS = 'RESET_OFFICIAL_PERIODS';
    const mock_action = {
        type: RESET_OFFICIAL_PERIODS
    };
    expect(action).toEqual(mock_action);
});
