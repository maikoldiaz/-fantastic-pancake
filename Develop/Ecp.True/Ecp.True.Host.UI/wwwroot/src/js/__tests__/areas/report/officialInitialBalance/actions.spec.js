import * as actions from '../../../../modules/report/officialInitialBalance/actions';
import { apiService } from '../../../../common/services/apiService';

it('should save official initial balance', () => {
    const OFFICIAL_INITIAL_BALANCE_SAVE_FILTER = 'OFFICIAL_INITIAL_BALANCE_SAVE_FILTER';
    const data = 'Data';
    const action = actions.saveOfficialInitialBalanceFilter(data);
    expect(action.type).toEqual(OFFICIAL_INITIAL_BALANCE_SAVE_FILTER);
    expect(action.data).toEqual('Data');
});

it('should select segment', () => {
    const OFFICIAL_INITIAL_BALANCE_SELECT_ELEMENT = 'OFFICIAL_INITIAL_BALANCE_SELECT_ELEMENT';
    const element = 'element';
    const action = actions.onSelectedElement(element);
    expect(action.type).toEqual(OFFICIAL_INITIAL_BALANCE_SELECT_ELEMENT);
    expect(action.element).toEqual('element');
});

it('should reset official initial balance report filters', () => {
    const RESET_OFFICIAL_INITIAL_BALANCE_FILTER = 'RESET_OFFICIAL_INITIAL_BALANCE_FILTER';
    const action = actions.resetOfficialInitialBalanceFilter();
    expect(action.type).toEqual(RESET_OFFICIAL_INITIAL_BALANCE_FILTER);
});

it('should save official initial balance filter data request', () => {
    const SAVE_OFFICIAL_INITIAL_BALANCE = 'SAVE_OFFICIAL_INITIAL_BALANCE';
    const action = actions.saveOfficialInitialBalance();
    expect(action.type).toEqual(SAVE_OFFICIAL_INITIAL_BALANCE);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.ticket.postOfficialInitialBalanceReport());
    expect(action.fetchConfig.success).toBeDefined();
});

it('should receive Official Initial Balance', () => {
    const action = actions.receiveOfficialInitialBalance({});
    expect(action.type).toEqual('RECEIVE_OFFICIAL_INITIAL_BALANCE');
});

it('should request Official Initial Balance status', () => {
    const executionId = 1;
    const reportType = 3;
    const REQUEST_OFFICIAL_INITIAL_BALANCE_STATUS = 'REQUEST_OFFICIAL_INITIAL_BALANCE_STATUS';
    const action = actions.requestOfficialInitialBalanceStatus(executionId, reportType);
    expect(action.type).toEqual(REQUEST_OFFICIAL_INITIAL_BALANCE_STATUS);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.ticket.requestReportExecutionStatus(executionId, reportType));
    expect(action.fetchConfig.success).toBeDefined();
});

it('should receive Official Initial Balance status', () => {
    const action = actions.receiveOfficialInitialBalanceStatus({});
    expect(action.type).toEqual('RECEIVE_OFFICIAL_INITIAL_BALANCE_STATUS');
});

it('should refresh status', () => {
    const action = actions.refreshStatus({});
    expect(action.type).toEqual('REFRESH_STATUS');
});

it('should clear status', () => {
    const action = actions.clearStatus({});
    expect(action.type).toEqual('CLEAR_STATUS');
});
