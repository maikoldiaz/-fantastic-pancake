import * as actions from '../../../modules/administration/annulation/actions';
import { apiService } from '../../../common/services/apiService';
import { constants } from '../../../common/services/constants';

it('should initialize types', () => {
    const action = actions.initTypes();
    const INIT_ANNULATION_TYPES = 'INIT_ANNULATION_TYPES';
    const mock_action = {
        type: INIT_ANNULATION_TYPES
    };
    expect(action).toEqual(mock_action);
});

it('should update types', () => {
    const name = constants.Annulations.Sections.Source;
    const value = { elementId: 1 };
    const field = constants.Annulations.Fields.Movement;
    const action = actions.updateTypes(name, value, field);
    const UPDATE_ANNULATION_TYPES = 'UPDATE_ANNULATION_TYPES';
    const mock_action = {
        type: UPDATE_ANNULATION_TYPES,
        name,
        value,
        field
    };
    expect(action).toEqual(mock_action);
});

it('should reset types', () => {
    const name = constants.Annulations.Sections.Source;
    const field = constants.Annulations.Fields.Movement;
    const action = actions.resetTypes(name, field);
    const RESET_ANNULATION_TYPES = 'RESET_ANNULATION_TYPES';
    const mock_action = {
        type: RESET_ANNULATION_TYPES,
        name,
        field
    };
    expect(action).toEqual(mock_action);
});

it('should request create annulation', () => {
    const value = {};
    const SAVE_ANNULATION = 'SAVE_ANNULATION';
    const action = actions.saveAnnulation(value);

    expect(action.type).toEqual(SAVE_ANNULATION);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.annulation.create());
    expect(action.fetchConfig.body).toEqual(value);
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_SAVE_ANNULATION = 'RECEIVE_SAVE_ANNULATION';
    expect(receiveAction.type).toEqual(RECEIVE_SAVE_ANNULATION);
    expect(receiveAction.status).toEqual(true);
});

it('should receive save annulation', () => {
    const status = {};
    const action = actions.receiveSaveAnnulation(status);
    const RECEIVE_SAVE_ANNULATION = 'RECEIVE_SAVE_ANNULATION';
    const mock_action = {
        type: RECEIVE_SAVE_ANNULATION,
        status
    };
    expect(action).toEqual(mock_action);
});

it('should init annulation row', () => {
    const initialValues = {};
    const action = actions.initAnnulation(initialValues);
    const INIT_ANNULATION = 'INIT_ANNULATION';
    const mock_action = {
        type: INIT_ANNULATION,
        initialValues
    };
    expect(action).toEqual(mock_action);
});
