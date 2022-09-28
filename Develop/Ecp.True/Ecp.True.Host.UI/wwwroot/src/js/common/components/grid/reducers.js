import {
    INIT_GRID,
    RECEIVE_GRID_DATA,
    REFRESH_GRID_DATA,
    SELECT_GRID_DATA,
    SELECT_ALL_GRID_DATA,
    CLEAR_GRID_DATA,
    REFRESH_SILENT_GRID_DATA,
    REMOVE_GRID_DATA,
    UPDATE_GRID_PAGE_ITEMS,
    CLEAR_GRID_SELECTION,
    RESET_PAGE_INDEX,
    APPLY_GRID_PAGE_FILTER,
    APPLY_FILTER,
    DISABLE_ITEMS_GRID,
    RESET_FILTER,
    ADD_UPDATE_GRID_DATA
} from './actions';

function buildSelection(state, keyField, keyValue) {
    const items = state.items;
    const pageItems = state.config.odata === false ? state.pageItems : state.items;
    const selection = state.selection;

    const selectIndex = selection.findIndex(x => x[keyField].toString() === keyValue);
    let result = [...selection];

    // Un select
    if (selectIndex >= 0) {
        result = [
            ...selection.slice(0, selectIndex),
            ...selection.slice(selectIndex + 1)
        ];
    } else {
        const itemIndex = items.findIndex(x => x[keyField].toString() === keyValue);
        result.push(items[itemIndex]);
    }

    return { selection: result, selectAll: result.length === pageItems.length };
}

function buildAllSelection(items, pageItems, selectAll, odata = true) {
    if (selectAll === false) {
        return [];
    }

    if (odata === true) {
        return items.filter(item => !item.disabled);
    }

    // eslint-disable-next-line no-underscore-dangle
    return pageItems.map(r => r._original).filter(item => !item.disabled);
}

function buildDisabled(existingItems, keyField, keyValues) {
    const result = [];
    existingItems.forEach(x => {
        const y = Object.assign({}, x);
        y.disabled = keyValues.includes(y[keyField]);
        result.push(y);
    });

    return result;
}

function buildRemoved(existingItems, existingSelection, keyField, itemsToRemoved) {
    const itemIds = itemsToRemoved.map(x => x[keyField]);
    return { items: existingItems.filter(x => !itemIds.includes(x[keyField])), selection: existingSelection.filter(x => !itemIds.includes(x[keyField])) };
}

function buildAdded(existingItems, existingSelection, keyField, itemToBeAdded) {
    const index = existingItems.findIndex(x => x[keyField] === itemToBeAdded[keyField]);
    const items = [...existingItems];
    if (index < 0) {
        items.push(itemToBeAdded);
    } else {
        items[index] = itemToBeAdded;
    }

    const updatedSelection = existingSelection.map(x => {
        return x[keyField] === itemToBeAdded[keyField] ? Object.assign({}, x, itemToBeAdded) : x;
    });

    return { items: items, selection: updatedSelection };
}

export const grid = (state = {}, action = {}) => {
    switch (action.type) {
    case INIT_GRID: {
        if (action.config.resume === true && state[action.config.name]) {
            return Object.assign({}, state);
        }

        return Object.assign({},
            state,
            {
                [action.config.name]: {
                    config: action.config,
                    selection: [],
                    selectAll: false,
                    items: [],
                    pageItems: [],
                    totalItems: 0,
                    filterValues: action.config.defaultFilter || {},
                    pageFilters: {}
                }
            });
    }
    case RECEIVE_GRID_DATA: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    items: action.items,
                    totalItems: action.totalItems,
                    selection: state[action.name].selectAll === true ? [...action.items.filter(item => !item.disabled)] : [],
                    receiveDataToggler: !state[action.name].receiveDataToggler
                })
            });
    }
    case REFRESH_GRID_DATA: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    refreshToggler: !state[action.name].refreshToggler,
                    selectAll: action.resetSelectAll ? false : state[action.name].selectAll
                })
            });
    }
    case REFRESH_SILENT_GRID_DATA: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    refreshSilentToggler: !state[action.name].refreshSilentToggler
                })
            });
    }
    case SELECT_GRID_DATA: {
        const gridState = state[action.name];
        const select = buildSelection(gridState, gridState.config.idField, action.key);
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, gridState, {
                    selection: select.selection,
                    selectAll: select.selectAll
                })
            });
    }
    case SELECT_ALL_GRID_DATA: {
        const gridState = state[action.name];
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, gridState, {
                    selectAll: action.selectAll,
                    selection: buildAllSelection(gridState.items, gridState.pageItems, action.selectAll, gridState.config.odata)
                })
            });
    }
    case CLEAR_GRID_DATA: {
        const gridState = state[action.name];
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, gridState, {
                    selection: [],
                    selectAll: false,
                    items: [],
                    totalItems: 0
                })
            });
    }
    case APPLY_FILTER: {
        const gridState = state[action.name];
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, gridState, {
                    filterValues: Object.assign({}, gridState.filterValues, action.filterValues),
                    refreshToggler: !gridState.refreshToggler
                })
            });
    }
    case APPLY_GRID_PAGE_FILTER: {
        const gridState = state[action.name];
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, gridState, {
                    pageFilters: Object.assign({}, gridState.pageFilters, action.pageFilters),
                    isDecimal: action.isDecimal,
                    refreshToggler: !gridState.refreshToggler
                })
            });
    }
    case RESET_FILTER: {
        const gridState = state[action.name];
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, gridState, {
                    filterValues: action.value || {},
                    resetToggler: !gridState.resetToggler
                })
            });
    }
    case DISABLE_ITEMS_GRID: {
        const gridState = state[action.name];
        const result = buildDisabled(gridState.items, action.keyField, action.keyValues);
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, gridState, {
                    items: result
                })
            });
    }
    case REMOVE_GRID_DATA: {
        const gridState = state[action.name];
        const result = buildRemoved(gridState.items, gridState.selection, gridState.config.idField, action.items);
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, gridState, {
                    items: result.items,
                    selection: result.selection,
                    totalItems: result.items.length
                })
            });
    }
    case ADD_UPDATE_GRID_DATA: {
        const gridState = state[action.name];
        const result = buildAdded(gridState.items, gridState.selection, action.keyField, action.item);
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, gridState, {
                    items: result.items,
                    totalItems: result.items.length,
                    selection: result.selection
                })
            });
    }
    case CLEAR_GRID_SELECTION: {
        const gridState = state[action.name];
        const togglerName = action.toggler ? `${action.toggler}Toggler` : 'clearSelectionToggler';
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, gridState, {
                    selection: [],
                    selectAll: false,
                    [togglerName]: !gridState[togglerName]
                })
            });
    }
    case UPDATE_GRID_PAGE_ITEMS: {
        const gridState = state[action.name];
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, gridState, {
                    pageItems: action.pageItems
                })
            });
    }
    case RESET_PAGE_INDEX: {
        const gridState = state[action.name];
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, gridState, {
                    resetPageIndexToggler: !gridState.resetPageIndexToggler
                })
            });
    }
    default:
        return state;
    }
};

