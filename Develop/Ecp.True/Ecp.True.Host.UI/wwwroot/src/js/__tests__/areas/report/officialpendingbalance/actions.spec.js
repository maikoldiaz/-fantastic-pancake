import * as actions from '../../../../modules/report/officialPendingBalance/actions';
import { apiService } from '../../../../common/services/apiService';

it('should save pending initial balance', () => {
    const OFFICIAL_PENDING_BALANCE_SAVE_FILTER = 'OFFICIAL_PENDING_BALANCE_SAVE_FILTER';
    const data = 'Data';
    const action = actions.saveOfficialPendingBalanceFilter(data);
    expect(action.type).toEqual(OFFICIAL_PENDING_BALANCE_SAVE_FILTER);
    expect(action.data).toEqual('Data');
});

it('should select segment', () => {
    const OFFICIAL_PENDING_BALANCE_SELECT_ELEMENT = 'OFFICIAL_PENDING_BALANCE_SELECT_ELEMENT';
    const element = 'element';
    const action = actions.onSelectedElement(element);
    expect(action.type).toEqual(OFFICIAL_PENDING_BALANCE_SELECT_ELEMENT);
    expect(action.element).toEqual('element');
});

it('should reset official pending balance report filters', () => {
    const RESET_OFFICIAL_PENDING_BALANCE_FILTER = 'RESET_OFFICIAL_PENDING_BALANCE_FILTER';
    const action = actions.resetPendingBalanceFilter();
    expect(action.type).toEqual(RESET_OFFICIAL_PENDING_BALANCE_FILTER);
});
