import { utilities } from '../../services/utilities';

export const INIT_GRID = 'INIT_GRID';
export const FETCH_GRID_DATA = 'FETCH_GRID_DATA';
export const REFRESH_GRID_DATA = 'REFRESH_GRID_DATA';
export const RECEIVE_GRID_DATA = 'RECEIVE_GRID_DATA';
export const SELECT_GRID_DATA = 'SELECT_GRID_DATA';
export const SELECT_ALL_GRID_DATA = 'SELECT_ALL_GRID_DATA';
export const CLEAR_GRID_DATA = 'CLEAR_GRID_DATA';
export const REFRESH_SILENT_GRID_DATA = 'REFRESH_SILENT_GRID_DATA';
export const REMOVE_GRID_DATA = 'REMOVE_GRID_DATA';
export const ADD_UPDATE_GRID_DATA = 'ADD_UPDATE_GRID_DATA';
export const DISABLE_ITEMS_GRID = 'DISABLE_ITEMS_GRID';
export const CLEAR_GRID_SELECTION = 'CLEAR_GRID_SELECTION';
export const UPDATE_GRID_PAGE_ITEMS = 'UPDATE_GRID_PAGE_ITEMS';
export const APPLY_GRID_PAGE_FILTER = 'APPLY_GRID_PAGE_FILTER';
export const RESET_PAGE_INDEX = 'RESET_PAGE_INDEX';
export const RESET_FILTER = 'RESET_FILTER';
export const APPLY_FILTER = 'APPLY_FILTER';

export const configureGrid = config => {
    return {
        type: INIT_GRID,
        config
    };
};

export const receiveGridData = (json, name) => {
    return {
        type: RECEIVE_GRID_DATA,
        items: utilities.isNullOrUndefined(json.value) ? json : json.value,
        totalItems: utilities.isNullOrUndefined(json.count) ? json.length : json.count,
        name
    };
};

export const fetchGridData = (apiUrl, name, notFound = false) => {
    return {
        type: FETCH_GRID_DATA,
        fetchConfig: {
            path: apiUrl,
            success: json => receiveGridData(json, name),
            failure: () => receiveGridData({ value: [], count: 0 }, name),
            notFound
        }
    };
};

export const fetchGridDataSilent = (apiUrl, name) => {
    return {
        type: FETCH_GRID_DATA,
        fetchConfig: {
            path: apiUrl,
            success: json => receiveGridData(json, name),
            failure: () => receiveGridData({ value: [], count: 0 }, name),
            showProgress: false
        }
    };
};

export const refreshGrid = (name, resetSelectAll = false) => {
    return {
        type: REFRESH_GRID_DATA,
        name,
        resetSelectAll
    };
};

export const applyPageFilter = (name, pageFilters, isDecimal) => {
    return {
        type: APPLY_GRID_PAGE_FILTER,
        name,
        pageFilters,
        isDecimal
    };
};

export const refreshSilent = name => {
    return {
        type: REFRESH_SILENT_GRID_DATA,
        name
    };
};

export const selectGridData = (key, name) => {
    return {
        type: SELECT_GRID_DATA,
        name,
        key
    };
};

export const selectAllGridData = (selectAll, name) => {
    return {
        type: SELECT_ALL_GRID_DATA,
        name,
        selectAll
    };
};

export const clearGrid = name => {
    return {
        type: CLEAR_GRID_DATA,
        name
    };
};

export const removeGridData = (name, items) => {
    return {
        type: REMOVE_GRID_DATA,
        name,
        items
    };
};

export const addUpdateGridData = (name, keyField, item) => {
    return {
        type: ADD_UPDATE_GRID_DATA,
        name,
        keyField,
        item
    };
};

export const disableGridItems = function (name, keyField, keyValues) {
    return {
        type: DISABLE_ITEMS_GRID,
        name,
        keyField,
        keyValues
    };
};

export const updatePageItems = (name, pageItems) => {
    return {
        type: UPDATE_GRID_PAGE_ITEMS,
        name,
        pageItems
    };
};

export const clearSelection = (name, toggler) => {
    return {
        type: CLEAR_GRID_SELECTION,
        name,
        toggler
    };
};

export const resetPageIndex = name => {
    return {
        type: RESET_PAGE_INDEX,
        name
    };
};

export const applyFilter = function (name, filterValues) {
    return {
        type: APPLY_FILTER,
        name,
        filterValues
    };
};

export const resetFilter = function (name, value) {
    return {
        type: RESET_FILTER,
        name,
        value
    };
};
