import {
    RECEIVE_HOMOLOGATION_OBJECT_TYPES,
    RECEIVE_DELETE_HOMOLOGATION,
    RECEIVE_HOMOLOGATION_DATA_MAPPINGS,
    INIT_HOMOLOGATION_GROUP,
    INIT_HOMOLOGATION_GROUP_DATA,
    CREATE_HOMOLOGATION_OBJECT_TYPES,
    UPDATE_HOMOLOGATION_OBJECT_TYPES,
    RECEIVE_CREATE_UPDATE_HOMOLOGATION_GROUP,
    RECEIVE_SEARCH_DATA,
    CLEAR_SEARCH_DATA,
    RECEIVE_HOMOLOGATION_GROUP
} from './actions.js';

import { constants } from './../../../common/services/constants';

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

function buildHomologationObjectTypes(objs) {
    return objs.map(v => Object.assign({}, v, { isRequiredMapping: false }));
}

function buildHomologationDataMappings(objs) {
    return objs.map(v => Object.assign({}, v, { tempId: v.homologationDataMappingId }));
}

function reBuildHomologationObjectTypes(objs, obj) {
    const data = [...objs];
    const updatedObjectTypes = data.map(v => {
        if (v.homologationObjectTypeId === obj.homologationObjectTypeId) {
            return Object.assign({}, v, obj);
        }
        return v;
    });
    return updatedObjectTypes;
}

function buildHomologationGroup(data) {
    const homologationObjectTypes = data.homologationObjects.map(v => Object.assign({}, v, { name: v.homologationObjectType.name }));
    return {
        sourceSystem: data.homologation.sourceSystem,
        destinationSystem: data.homologation.destinationSystem,
        group: data.group,
        homologationObjectTypes: homologationObjectTypes,
        rowVersion: data.rowVersion
    };
}

export const homologations = function (state = initialState, action = {}) {
    switch (action.type) {
    case RECEIVE_HOMOLOGATION_OBJECT_TYPES:
        return Object.assign({},
            state,
            {
                objectTypes: buildHomologationObjectTypes(action.data),
                objectTypesToggler: !state.objectTypesToggler
            });
    case RECEIVE_DELETE_HOMOLOGATION:
    case RECEIVE_CREATE_UPDATE_HOMOLOGATION_GROUP:
        return Object.assign({},
            state,
            {
                status: action.status,
                refreshToggler: action.status === true ? !state.refreshToggler : state.refreshToggler,
                conflictToggler: action.status === false ? !state.conflictToggler : state.conflictToggler
            });
    case INIT_HOMOLOGATION_GROUP:
        return Object.assign({},
            state,
            {
                homologationGroup: Object.assign({}, initialState.homologationGroup, action.homologationGroup),
                mode: action.mode
            });
    case INIT_HOMOLOGATION_GROUP_DATA:
        return Object.assign({},
            state,
            {
                dataMapping: action.homologationGroupData
            });
    case CREATE_HOMOLOGATION_OBJECT_TYPES:
        return Object.assign({},
            state,
            {
                homologationGroup: Object.assign({}, state.homologationGroup, {
                    homologationObjectTypes: action.objectTypes
                })
            });
    case UPDATE_HOMOLOGATION_OBJECT_TYPES:
        return Object.assign({},
            state,
            {
                homologationGroup: Object.assign({}, state.homologationGroup, {
                    homologationObjectTypes: reBuildHomologationObjectTypes(state.homologationGroup.homologationObjectTypes, action.objectType)
                })
            });
    case RECEIVE_SEARCH_DATA: {
        return Object.assign({},
            state,
            {
                dataMappings: action.data
            });
    }
    case CLEAR_SEARCH_DATA:
        return Object.assign({},
            state,
            {
                dataMappings: []
            });
    case RECEIVE_HOMOLOGATION_GROUP:
        return Object.assign({},
            state,
            {
                homologationGroup: Object.assign({}, buildHomologationGroup(action.data)),
                homologationGroupToggler: !state.homologationGroupToggler,
                mode: constants.Modes.Update
            });
    case RECEIVE_HOMOLOGATION_DATA_MAPPINGS:
        return Object.assign({},
            state,
            {
                homologationGroup: Object.assign({}, state.homologationGroup, {
                    homologationDataMappings: buildHomologationDataMappings(action.dataMappings)
                }),
                dataMappingsToggler: !state.dataMappingsToggler
            });
    default:
        return state;
    }
};
