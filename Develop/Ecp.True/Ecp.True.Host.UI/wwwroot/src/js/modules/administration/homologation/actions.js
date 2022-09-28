import { apiService } from './../../../common/services/apiService.js';

export const REQUEST_HOMOLOGATION_GROUP = 'REQUEST_HOMOLOGATION_GROUP';
export const RECEIVE_HOMOLOGATION_GROUP = 'RECEIVE_HOMOLOGATION_GROUP';
export const REQUEST_HOMOLOGATION_OBJECT_TYPES = 'REQUEST_HOMOLOGATION_OBJECT_TYPES';
export const RECEIVE_HOMOLOGATION_OBJECT_TYPES = 'RECEIVE_HOMOLOGATION_OBJECT_TYPES';
export const REQUEST_HOMOLOGATION_DATA_MAPPINGS = 'REQUEST_HOMOLOGATION_DATA_MAPPINGS';
export const RECEIVE_HOMOLOGATION_DATA_MAPPINGS = 'RECEIVE_HOMOLOGATION_DATA_MAPPINGS';
export const REQUEST_DELETE_HOMOLOGATION = 'REQUEST_DELETE_HOMOLOGATION';
export const RECEIVE_DELETE_HOMOLOGATION = 'RECEIVE_DELETE_HOMOLOGATION';
export const INIT_HOMOLOGATION_GROUP = 'INIT_HOMOLOGATION_GROUP';
export const INIT_HOMOLOGATION_GROUP_DATA = 'INIT_HOMOLOGATION_GROUP_DATA';
export const CREATE_HOMOLOGATION_OBJECT_TYPES = 'CREATE_HOMOLOGATION_OBJECT_TYPES';
export const UPDATE_HOMOLOGATION_OBJECT_TYPES = 'UPDATE_HOMOLOGATION_OBJECT_TYPES';
export const REQUEST_SEARCH_DATA = 'REQUEST_SEARCH_DATA';
export const RECEIVE_SEARCH_DATA = 'RECEIVE_SEARCH_DATA';
export const CLEAR_SEARCH_DATA = 'CLEAR_SEARCH_DATA';
export const REQUEST_CREATE_UPDATE_HOMOLOGATION_GROUP = 'REQUEST_CREATE_UPDATE_HOMOLOGATION_GROUP';
export const RECEIVE_CREATE_UPDATE_HOMOLOGATION_GROUP = 'RECEIVE_CREATE_UPDATE_HOMOLOGATION_GROUP';
export const RECEIVE_SEARCH_DATA_ERROR = 'RECEIVE_SEARCH_DATA_ERROR';

export const receiveHomologationObjectTypes = data => {
    return {
        type: RECEIVE_HOMOLOGATION_OBJECT_TYPES,
        data
    };
};

export const requestHomologationObjectTypes = () => {
    return {
        type: REQUEST_HOMOLOGATION_OBJECT_TYPES,
        fetchConfig: {
            path: apiService.homologation.getHomologationObjects(),
            success: receiveHomologationObjectTypes
        }
    };
};


export const receiveHomologationDataMappings = dataMappings => {
    return {
        type: RECEIVE_HOMOLOGATION_DATA_MAPPINGS,
        dataMappings
    };
};

export const requestHomologationDataMappings = homologationGroupId => {
    return {
        type: REQUEST_HOMOLOGATION_DATA_MAPPINGS,
        fetchConfig: {
            path: apiService.homologation.getHomologationDataMappingsByGroup(homologationGroupId),
            success: receiveHomologationDataMappings
        }
    };
};

const receiveDeleteHomologation = () => {
    return {
        type: RECEIVE_DELETE_HOMOLOGATION,
        status: true
    };
};

const receiveDeleteHomologationFailed = () => {
    return {
        type: RECEIVE_DELETE_HOMOLOGATION,
        status: false
    };
};

export const deleteHomologationGroup = data => {
    return {
        type: REQUEST_DELETE_HOMOLOGATION,
        fetchConfig: {
            path: apiService.homologation.deleteHomologationGroup(),
            method: 'DELETE',
            body: data,
            success: receiveDeleteHomologation,
            failure: receiveDeleteHomologationFailed
        }
    };
};


export const initHomologationGroup = (mode, homologationGroup) => {
    return {
        type: INIT_HOMOLOGATION_GROUP,
        homologationGroup,
        mode
    };
};

export const initHomologationGroupData = homologationGroupData => {
    return {
        type: INIT_HOMOLOGATION_GROUP_DATA,
        homologationGroupData
    };
};

export const createHomologationObjectTypes = objectTypes => {
    return {
        type: CREATE_HOMOLOGATION_OBJECT_TYPES,
        objectTypes
    };
};

export const updateHomologationObjectTypes = objectType => {
    return {
        type: UPDATE_HOMOLOGATION_OBJECT_TYPES,
        objectType
    };
};

export const receiveSearchData = data => {
    return {
        type: RECEIVE_SEARCH_DATA,
        data
    };
};

export const clearSearchData = () => {
    return {
        type: CLEAR_SEARCH_DATA
    };
};

export const requestSearchData = (searchText, pathType, categoryId) => {
    return {
        type: REQUEST_SEARCH_DATA,
        name,
        fetchConfig: {
            path: apiService.homologation.searchHomologationGroupData(searchText, pathType, categoryId),
            success: json => receiveSearchData(json.value),
            failure: clearSearchData
        }
    };
};

export const receiveHomologationGroup = json => {
    return {
        type: RECEIVE_HOMOLOGATION_GROUP,
        data: json && json.value.length === 1 ? json.value[0] : {}
    };
};

export const requestHomologationGroup = homologationGroupId => {
    return {
        type: REQUEST_HOMOLOGATION_GROUP,
        fetchConfig: {
            path: apiService.homologation.getHomologationGroup(homologationGroupId),
            success: receiveHomologationGroup,
            notFound: true
        }
    };
};

export const receiveCreateUpdateHomologationGroup = () => {
    return {
        type: RECEIVE_CREATE_UPDATE_HOMOLOGATION_GROUP,
        status: true
    };
};

export const requestCreateUpdateHomologationGroup = homologationGroup => {
    return {
        type: REQUEST_CREATE_UPDATE_HOMOLOGATION_GROUP,
        name,
        fetchConfig: {
            path: apiService.homologation.saveCreateUpdateHomologationGroup(),
            body: homologationGroup,
            success: receiveCreateUpdateHomologationGroup
        }
    };
};
