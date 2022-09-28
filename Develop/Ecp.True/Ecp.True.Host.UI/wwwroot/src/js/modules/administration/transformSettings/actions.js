import { apiService } from './../../../common/services/apiService';

const REQUEST_GET_SOURCENODES = 'REQUEST_GET_SOURCENODES';
export const RECEIVE_GET_SOURCENODES = 'RECEIVE_GET_SOURCENODES';

const REQUEST_GET_DESTINATIONNODES = 'REQUEST_GET_DESTINATIONNODES';
export const RECEIVE_GET_DESTINATIONNODES = 'RECEIVE_GET_DESTINATIONNODES';

const REQUEST_GET_SOURCEPRODUCTS = 'REQUEST_GET_SOURCEPRODUCTS';
export const RECEIVE_GET_SOURCEPRODUCTS = 'RECEIVE_GET_SOURCEPRODUCTS';

const REQUEST_GET_DESTINATIONPRODUCTS = 'REQUEST_GET_DESTINATIONPRODUCTS';
export const RECEIVE_GET_DESTINATIONPRODUCTS = 'RECEIVE_GET_DESTINATIONPRODUCTS';

const REQUEST_SEARCH_SOURCENODES = 'REQUEST_SEARCH_SOURCENODES';
export const RECEIVE_SEARCH_SOURCENODES = 'RECEIVE_SEARCH_SOURCENODES';

export const CLEAR_SEARCH_SOURCENODES = 'CLEAR_SEARCH_SOURCENODES';

const REQUEST_CREATE_TRANSFORMATION = 'REQUEST_CREATE_TRANSFORMATION';
export const RECEIVE_CREATE_TRANSFORMATION = 'RECEIVE_CREATE_TRANSFORMATION';

const REQUEST_UPDATE_TRANSFORMATION = 'REQUEST_UPDATE_TRANSFORMATION';
export const RECEIVE_UPDATE_TRANSFORMATION = 'RECEIVE_UPDATE_TRANSFORMATION';

export const RESET_TRANSFORMATION_POPUP = 'RESET_TRANSFORMATION_POPUP';
export const CLEAR_TRANSFORMATION_DATA = 'CLEAR_TRANSFORMATION_DATA';

const REQUEST_DELETE_TRANSFORMATION = 'REQUEST_DELETE_TRANSFORMATION';
export const RECEIVE_DELETE_TRANSFORMATION = 'RECEIVE_DELETE_TRANSFORMATION';

const REQUEST_GET_TRANSFORMATION_INFO = 'REQUEST_GET_TRANSFORMATION_INFO';
export const RECEIVE_GET_TRANSFORMATION_INFO = 'RECEIVE_GET_TRANSFORMATION_INFO';

export const REQUEST_EDIT_INITIALIZE_TRANSFORMATION = 'REQUEST_EDIT_INITIALIZE_TRANSFORMATION';

const receiveGetSourceNodes = (json, units, mode) => {
    return {
        type: RECEIVE_GET_SOURCENODES,
        sourceNodes: json.value,
        units,
        mode
    };
};

const receiveGetDestinationNodes = (json, name) => {
    return {
        type: RECEIVE_GET_DESTINATIONNODES,
        destinationNodes: json,
        name
    };
};

const receiveGetSourceProducts = (json, name) => {
    return {
        type: RECEIVE_GET_SOURCEPRODUCTS,
        sourceProducts: json.value,
        name
    };
};

const receiveGetDestinationProducts = (json, name) => {
    return {
        type: RECEIVE_GET_DESTINATIONPRODUCTS,
        destinationProducts: json.value,
        name
    };
};

const receiveSearchSourceNodes = (json, name) => {
    return {
        type: RECEIVE_SEARCH_SOURCENODES,
        searchedSourceNodes: json.value,
        name
    };
};

const receiveCreateTransformation = status => {
    return {
        type: RECEIVE_CREATE_TRANSFORMATION,
        status
    };
};

const receiveUpdateTransformation = status => {
    return {
        type: RECEIVE_UPDATE_TRANSFORMATION,
        status
    };
};

const receiveDeleteTransformation = status => {
    return {
        type: RECEIVE_DELETE_TRANSFORMATION,
        status
    };
};

const receiveGetTransformationInfo = (json, transformation) => {
    return {
        type: RECEIVE_GET_TRANSFORMATION_INFO,
        transformation,
        origin: json.origin,
        destination: json.destination
    };
};

export const getSourceNodes = (units, mode) => {
    return {
        type: REQUEST_GET_SOURCENODES,
        fetchConfig: {
            path: apiService.node.queryActive(),
            success: json => receiveGetSourceNodes(json, units, mode)
        }
    };
};

export const getDestinationNodes = (sourceNodeId, name) => {
    return {
        type: REQUEST_GET_DESTINATIONNODES,
        name,
        fetchConfig: {
            path: apiService.nodeConnection.getDestinationNodes(sourceNodeId),
            success: json => receiveGetDestinationNodes(json, name)
        }
    };
};

export const getSourceProducts = (sourceNodeId, name) => {
    return {
        type: REQUEST_GET_SOURCEPRODUCTS,
        name,
        fetchConfig: {
            path: apiService.node.queryNodeProducts(sourceNodeId),
            success: json => receiveGetSourceProducts(json, name)
        }
    };
};

export const getDestinationProducts = (destinationNodeId, name) => {
    return {
        type: REQUEST_GET_DESTINATIONPRODUCTS,
        name,
        fetchConfig: {
            path: apiService.node.queryNodeProducts(destinationNodeId),
            success: json => receiveGetDestinationProducts(json, name)
        }
    };
};

//  Auto complete search

export const clearSearchSourceNodes = name => {
    return {
        type: CLEAR_SEARCH_SOURCENODES,
        name
    };
};

export const requestSearchSourceNodes = (searchText, name) => {
    return {
        type: REQUEST_SEARCH_SOURCENODES,
        name,
        fetchConfig: {
            path: apiService.node.searchNodes(searchText),
            success: json => receiveSearchSourceNodes(json, name),
            failure: () => clearSearchSourceNodes(name)
        }
    };
};


export const createTransformation = transformation => {
    return {
        type: REQUEST_CREATE_TRANSFORMATION,
        fetchConfig: {
            path: apiService.transformation.create(),
            success: receiveCreateTransformation,
            body: transformation
        }
    };
};

export const updateTransformation = transformation => {
    return {
        type: REQUEST_UPDATE_TRANSFORMATION,
        fetchConfig: {
            path: apiService.transformation.create(),
            method: 'PUT',
            success: receiveUpdateTransformation,
            body: transformation
        }
    };
};

export const getTransformationInfo = transformation => {
    return {
        type: REQUEST_GET_TRANSFORMATION_INFO,
        fetchConfig: {
            path: apiService.transformation.getInfo(transformation.transformationId),
            success: json => receiveGetTransformationInfo(json, transformation)
        }
    };
};

export const deleteTransformation = data => {
    return {
        type: REQUEST_DELETE_TRANSFORMATION,
        fetchConfig: {
            path: apiService.transformation.delete(),
            method: 'DELETE',
            body: data,
            success: receiveDeleteTransformation
        }
    };
};

export const resetTransformationPopup = () => {
    return {
        type: RESET_TRANSFORMATION_POPUP
    };
};

export const clearTransformationData = name => {
    return {
        type: CLEAR_TRANSFORMATION_DATA,
        name: name
    };
};

export const initializeEdit = transformation => {
    return {
        type: REQUEST_EDIT_INITIALIZE_TRANSFORMATION,
        transformation
    };
};
