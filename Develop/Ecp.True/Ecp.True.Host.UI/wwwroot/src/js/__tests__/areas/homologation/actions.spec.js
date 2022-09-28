import * as actions from '../../../modules/administration/homologation/actions';
import { apiService } from '../../../common/services/apiService';
import { constants } from '../../../common/services/constants';
import { systemConfigService } from '../../../common/services/systemConfigService';

it('should receive homologation object types', () => {
    const data = {};
    const action = actions.receiveHomologationObjectTypes(data);
    const RECEIVE_HOMOLOGATION_OBJECT_TYPES = 'RECEIVE_HOMOLOGATION_OBJECT_TYPES';
    const mock_action = {
        type: RECEIVE_HOMOLOGATION_OBJECT_TYPES,
        data
    };
    expect(action).toEqual(mock_action);
});

it('should request homologation object types', () => {
    const REQUEST_HOMOLOGATION_OBJECT_TYPES = 'REQUEST_HOMOLOGATION_OBJECT_TYPES';
    const action = actions.requestHomologationObjectTypes();

    expect(action.type).toEqual(REQUEST_HOMOLOGATION_OBJECT_TYPES);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.homologation.getHomologationObjects());
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_HOMOLOGATION_OBJECT_TYPES = 'RECEIVE_HOMOLOGATION_OBJECT_TYPES';
    expect(receiveAction.type).toEqual(RECEIVE_HOMOLOGATION_OBJECT_TYPES);
    expect(receiveAction.data).toEqual(true);
});

it('should receive homologation data mappings', () => {
    const dataMappings = {};
    const action = actions.receiveHomologationDataMappings(dataMappings);
    const RECEIVE_HOMOLOGATION_DATA_MAPPINGS = 'RECEIVE_HOMOLOGATION_DATA_MAPPINGS';
    const mock_action = {
        type: RECEIVE_HOMOLOGATION_DATA_MAPPINGS,
        dataMappings
    };
    expect(action).toEqual(mock_action);
});

it('should request homologation data mappings', () => {
    const REQUEST_HOMOLOGATION_DATA_MAPPINGS = 'REQUEST_HOMOLOGATION_DATA_MAPPINGS';
    const action = actions.requestHomologationDataMappings(1);

    expect(action.type).toEqual(REQUEST_HOMOLOGATION_DATA_MAPPINGS);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.homologation.getHomologationDataMappingsByGroup(1));
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_HOMOLOGATION_DATA_MAPPINGS = 'RECEIVE_HOMOLOGATION_DATA_MAPPINGS';
    expect(receiveAction.type).toEqual(RECEIVE_HOMOLOGATION_DATA_MAPPINGS);
    expect(receiveAction.dataMappings).toEqual(true);
});

it('should request delete homologation group', () => {
    const REQUEST_DELETE_HOMOLOGATION = 'REQUEST_DELETE_HOMOLOGATION';
    const method = 'DELETE';
    const action = actions.deleteHomologationGroup(1);

    expect(action.type).toEqual(REQUEST_DELETE_HOMOLOGATION);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.homologation.deleteHomologationGroup(1));
    expect(action.fetchConfig.method).toEqual(method);

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const RECEIVE_DELETE_HOMOLOGATION = 'RECEIVE_DELETE_HOMOLOGATION';
    expect(receiveAction.type).toEqual(RECEIVE_DELETE_HOMOLOGATION);
    expect(receiveAction.status).toEqual(true);

    expect(action.fetchConfig.failure).toBeDefined();
    const receiveFailureAction = action.fetchConfig.failure(true);
    expect(receiveFailureAction.type).toEqual(RECEIVE_DELETE_HOMOLOGATION);
    expect(receiveFailureAction.status).toEqual(false);
});

it('should initialize homologation groups', () => {
    const mode = constants.Modes.Update;
    const homologationGroup = {};
    const action = actions.initHomologationGroup(mode, homologationGroup);
    const INIT_HOMOLOGATION_GROUP = 'INIT_HOMOLOGATION_GROUP';
    const mock_action = {
        type: INIT_HOMOLOGATION_GROUP,
        homologationGroup,
        mode
    };
    expect(action).toEqual(mock_action);
});

it('should initialize homologation group data', () => {
    const homologationGroupData = {};
    const action = actions.initHomologationGroupData(homologationGroupData);
    const INIT_HOMOLOGATION_GROUP_DATA = 'INIT_HOMOLOGATION_GROUP_DATA';
    const mock_action = {
        type: INIT_HOMOLOGATION_GROUP_DATA,
        homologationGroupData
    };
    expect(action).toEqual(mock_action);
});

it('should create homologation object types', () => {
    const objectTypes = {};
    const action = actions.createHomologationObjectTypes(objectTypes);
    const CREATE_HOMOLOGATION_OBJECT_TYPES = 'CREATE_HOMOLOGATION_OBJECT_TYPES';
    const mock_action = {
        type: CREATE_HOMOLOGATION_OBJECT_TYPES,
        objectTypes
    };
    expect(action).toEqual(mock_action);
});

it('should update homologation object types', () => {
    const objectType = {};
    const action = actions.updateHomologationObjectTypes(objectType);
    const UPDATE_HOMOLOGATION_OBJECT_TYPES = 'UPDATE_HOMOLOGATION_OBJECT_TYPES';
    const mock_action = {
        type: UPDATE_HOMOLOGATION_OBJECT_TYPES,
        objectType
    };
    expect(action).toEqual(mock_action);
});

it('should receive search data', () => {
    const data = {};
    const action = actions.receiveSearchData(data);
    const RECEIVE_SEARCH_DATA = 'RECEIVE_SEARCH_DATA';
    const mock_action = {
        type: RECEIVE_SEARCH_DATA,
        data
    };
    expect(action).toEqual(mock_action);
});

it('should clear search data', () => {
    const action = actions.clearSearchData();
    const CLEAR_SEARCH_DATA = 'CLEAR_SEARCH_DATA';
    const mock_action = {
        type: CLEAR_SEARCH_DATA
    };
    expect(action).toEqual(mock_action);
});

it('should request search data', () => {
    systemConfigService.getAutocompleteItemsCount = jest.fn(() => 5);
    
    const REQUEST_SEARCH_DATA = 'REQUEST_SEARCH_DATA';
    const searchText = ' search';
    const pathType = 'type';
    const categoryId = 1;
    const action = actions.requestSearchData(searchText, pathType, categoryId);

    expect(action.type).toEqual(REQUEST_SEARCH_DATA);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.homologation.searchHomologationGroupData(searchText, pathType, categoryId));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const RECEIVE_SEARCH_DATA = 'RECEIVE_SEARCH_DATA';
    expect(receiveAction.type).toEqual(RECEIVE_SEARCH_DATA);

    expect(action.fetchConfig.failure).toBeDefined();
    const receiveFailureAction = action.fetchConfig.failure(true);
    const CLEAR_SEARCH_DATA = 'CLEAR_SEARCH_DATA';
    expect(receiveFailureAction.type).toEqual(CLEAR_SEARCH_DATA);
});

it('should receive homologation group', () => {
    const data = { value: [] };
    const action = actions.receiveHomologationGroup(data);
    const RECEIVE_HOMOLOGATION_GROUP = 'RECEIVE_HOMOLOGATION_GROUP';
    const mock_action = {
        type: RECEIVE_HOMOLOGATION_GROUP,
        data: {}
    };
    expect(action).toEqual(mock_action);
});

it('should request homologation group', () => {
    const REQUEST_HOMOLOGATION_GROUP = 'REQUEST_HOMOLOGATION_GROUP';
    const action = actions.requestHomologationGroup(1);

    expect(action.type).toEqual(REQUEST_HOMOLOGATION_GROUP);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.homologation.getHomologationGroup(1));
    expect(action.fetchConfig.success).toBeDefined();
});

it('should receive create update homologation group', () => {
    const action = actions.receiveCreateUpdateHomologationGroup();
    const RECEIVE_CREATE_UPDATE_HOMOLOGATION_GROUP = 'RECEIVE_CREATE_UPDATE_HOMOLOGATION_GROUP';
    const mock_action = {
        type: RECEIVE_CREATE_UPDATE_HOMOLOGATION_GROUP,
        status: true
    };
    expect(action).toEqual(mock_action);
});

it('should request create update homologation group', () => {
    const REQUEST_CREATE_UPDATE_HOMOLOGATION_GROUP = 'REQUEST_CREATE_UPDATE_HOMOLOGATION_GROUP';
    const action = actions.requestCreateUpdateHomologationGroup({});

    expect(action.type).toEqual(REQUEST_CREATE_UPDATE_HOMOLOGATION_GROUP);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.homologation.saveCreateUpdateHomologationGroup());
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_CREATE_UPDATE_HOMOLOGATION_GROUP = 'RECEIVE_CREATE_UPDATE_HOMOLOGATION_GROUP';
    expect(receiveAction.type).toEqual(RECEIVE_CREATE_UPDATE_HOMOLOGATION_GROUP);
});
