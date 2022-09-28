import * as actions from '../../../modules/administration/homologation/actions';
import { homologations } from '../../../modules/administration/homologation/reducers';
import { constants } from '../../../common/services/constants';

describe('Reducer for transform settings', () => {
    const initialState = {
        objectTypes: [],
        dataMappings: [],
        dataMapping: {},
        refreshToggler: false,
        homologationGroup: {
            sourceSystem: {},
            destinationSystem: {},
            group: {},
            homologationObjectTypes: [],
            homologationDataMappings: []
        },
        mode: constants.Modes.Create
    };

    it('should RECEIVE_HOMOLOGATION_OBJECT_TYPES', () => {
        const objectTypesInput = [{ type: 'type' }];
        const action = {
            type: actions.RECEIVE_HOMOLOGATION_OBJECT_TYPES,
            data: objectTypesInput
        };

        const objectTypesOutput = [{ type: 'type', isRequiredMapping: false }];
        const newState = Object.assign({}, initialState, {
            objectTypes: objectTypesOutput,
            objectTypesToggler: !initialState.objectTypesToggler
        });

        expect(homologations(initialState, action)).toEqual(newState);
    });

    it('should RECEIVE_DELETE_HOMOLOGATION with status true', () => {
        const status = true;
        const action = {
            type: actions.RECEIVE_DELETE_HOMOLOGATION,
            status
        };

        const newState = Object.assign({}, initialState, {
            status: action.status,
            refreshToggler: !initialState.refreshToggler,
            conflictToggler: initialState.conflictToggler
        });

        expect(homologations(initialState, action)).toEqual(newState);
    });
    it('should RECEIVE_DELETE_HOMOLOGATION with status false', () => {
        const status = false;
        const action = {
            type: actions.RECEIVE_DELETE_HOMOLOGATION,
            status
        };

        const newState = Object.assign({}, initialState, {
            status: action.status,
            refreshToggler: initialState.refreshToggler,
            conflictToggler: !initialState.conflictToggler
        });

        expect(homologations(initialState, action)).toEqual(newState);
    });

    it('should RECEIVE_CREATE_UPDATE_HOMOLOGATION_GROUP with status true', () => {
        const status = true;
        const action = {
            type: actions.RECEIVE_DELETE_HOMOLOGATION,
            status
        };

        const newState = Object.assign({}, initialState, {
            status: action.status,
            refreshToggler: !initialState.refreshToggler,
            conflictToggler: initialState.conflictToggler
        });

        expect(homologations(initialState, action)).toEqual(newState);
    });
    it('should RECEIVE_CREATE_UPDATE_HOMOLOGATION_GROUP with status false', () => {
        const status = false;
        const action = {
            type: actions.RECEIVE_DELETE_HOMOLOGATION,
            status
        };

        const newState = Object.assign({}, initialState, {
            status: action.status,
            refreshToggler: initialState.refreshToggler,
            conflictToggler: !initialState.conflictToggler
        });

        expect(homologations(initialState, action)).toEqual(newState);
    });

    it('should INIT_HOMOLOGATION_GROUP', () => {
        const homologationGroup = {};
        const mode = constants.Modes.Update;
        const action = {
            type: actions.INIT_HOMOLOGATION_GROUP,
            homologationGroup,
            mode
        };

        const newState = Object.assign({}, initialState, {
            homologationGroup: Object.assign({}, initialState.homologationGroup, homologationGroup),
            mode: mode
        });

        expect(homologations(initialState, action)).toEqual(newState);
    });

    it('should INIT_HOMOLOGATION_GROUP_DATA', () => {
        const homologationGroupData = {
            destinationSystem: {},
            group: {},
            homologationDataMappings: {},
            homologationObjectTypes: {},
            sourceSystem: {}
        };
        const action = {
            type: actions.INIT_HOMOLOGATION_GROUP_DATA,
            homologationGroupData
        };

        const newState = Object.assign({}, initialState, {
            dataMapping: homologationGroupData
        });

        expect(homologations(initialState, action)).toEqual(newState);
    });

    it('should CREATE_HOMOLOGATION_OBJECT_TYPES', () => {
        const objectTypesInput = [{ type: 'type' }];
        const action = {
            type: actions.CREATE_HOMOLOGATION_OBJECT_TYPES,
            objectTypes: objectTypesInput
        };

        const newState = Object.assign({}, initialState, {
            homologationGroup: Object.assign({}, initialState.homologationGroup, {
                homologationObjectTypes: action.objectTypes
            })
        });

        expect(homologations(initialState, action)).toEqual(newState);
    });

    it('should UPDATE_HOMOLOGATION_OBJECT_TYPES', () => {
        const objectTypesInput = { homologationObjectTypeId: 1, type: 'newType' };
        const state = Object.assign({}, initialState, {
            homologationGroup: {
                homologationObjectTypes: [{ homologationObjectTypeId: 1, type: 'type' }, { homologationObjectTypeId: 2, type: 'type' }]
            }
        });
        const objectTypesOutput = [{ homologationObjectTypeId: 1, type: 'newType' }, { homologationObjectTypeId: 2, type: 'type' }];
        const action = {
            type: actions.UPDATE_HOMOLOGATION_OBJECT_TYPES,
            objectType: objectTypesInput
        };

        const newState = Object.assign({}, state, {
            homologationGroup: Object.assign({}, state.homologationGroup, {
                homologationObjectTypes: objectTypesOutput
            })
        });

        expect(homologations(state, action)).toEqual(newState);
    });

    it('should RECEIVE_SEARCH_DATA', () => {
        const data = [];
        const action = {
            type: actions.RECEIVE_SEARCH_DATA,
            data
        };

        const newState = Object.assign({}, initialState, { dataMappings: action.data });
        expect(homologations(initialState, action)).toEqual(newState);
    });

    it('should CLEAR_SEARCH_DATA', () => {
        const action = {
            type: actions.CLEAR_SEARCH_DATA
        };

        const newState = Object.assign({}, initialState, { dataMappings: [] });
        expect(homologations(initialState, action)).toEqual(newState);
    });

    it('should RECEIVE_HOMOLOGATION_GROUP', () => {
        const homologationGroupData = {
            homologation: {
                destinationSystem: {},
                sourceSystem: {}
            },
            rowVersion: 'rowversion',
            group: {},
            homologationObjects: [ { homologationObjectType: { name: 'name' } } ]
        };
        const homologationGroupDataOutput = {
            destinationSystem: {},
            sourceSystem: {},
            group: {},
            homologationObjectTypes: [ { homologationObjectType: { name: 'name' }, name: 'name' } ],
            rowVersion: 'rowversion'
        };
        const action = {
            type: actions.RECEIVE_HOMOLOGATION_GROUP,
            data: homologationGroupData
        };

        const newState = Object.assign({}, initialState, {
            homologationGroup: Object.assign({}, homologationGroupDataOutput),
            homologationGroupToggler: !initialState.homologationGroupToggler,
            mode: constants.Modes.Update
        });

        expect(homologations(initialState, action)).toEqual(newState);
    });

    it('should RECEIVE_HOMOLOGATION_DATA_MAPPINGS', () => {
        const mappingInput = [{ homologationDataMappingId: 1, type: 'mapping' }];
        const mappingOutput = [{ homologationDataMappingId: 1, type: 'mapping', tempId: 1 }];
        const action = {
            type: actions.RECEIVE_HOMOLOGATION_DATA_MAPPINGS,
            dataMappings: mappingInput
        };

        const newState = Object.assign({}, initialState, {
            homologationGroup: Object.assign({}, initialState.homologationGroup, {
                homologationDataMappings: mappingOutput
            }),
            dataMappingsToggler: !initialState.dataMappingsToggler
        });

        expect(homologations(initialState, action)).toEqual(newState);
    });

    it('should return default state', () => {
        const action = {
            type: 'test'
        };

        expect(homologations(initialState, action)).toEqual(initialState);
    });
});
