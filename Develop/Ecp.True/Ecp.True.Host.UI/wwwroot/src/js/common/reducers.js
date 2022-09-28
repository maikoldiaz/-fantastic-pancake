import {
    BOOTSTRAP_APP_SUCCESS,
    BOOTSTRAP_APP_FAILED,
    APP_READY,
    APP_MENU_TOGGLE,
    APP_MENU_ITEM_TOGGLE,
    APP_MENU_CURRENT_MODULE,
    OPEN_MODAL,
    OPEN_MESSAGE_ONLY_MODAL,
    CLOSE_MODAL,
    ADD_COMMENT,
    INIT_ADD_COMMENT,
    SHOW_LOADER,
    HIDE_LOADER,
    INIT_TAB,
    CHANGE_TAB,
    INIT_FLYOUT,
    OPEN_FLYOUT,
    CLOSE_FLYOUT,
    RECEIVE_CATEGORY_ELEMENTS,
    GET_CATEGORY_ELEMENTS,
    REQUEST_CATEGORY_ELEMENTS,
    CATEGORY_ELEMENTS_FILTER_RESET_FIELDS,
    INIT_WIZARD,
    WIZARD_SET_STEP,
    WIZARD_NEXT_STEP,
    WIZARD_PREV_STEP,
    INIT_CATEGORY_ELEMENT_FILTER,
    CATEGORY_ELEMENT_FILTER_ON_SELECT_CATEGORY,
    SAVE_CATEGORY_ELEMENT_FILTER,
    SHOW_NOTIFICATION,
    INVOKE_NOTIFICATION_BUTTON_CLICK,
    HIDE_NOTIFICATION,
    CONFIGURE_DUAL_SELECT,
    INIT_ALL_ITEMS,
    INIT_TARGET_ITEMS,
    UPDATE_TARGET_ITEMS,
    CLEAR_DUAL_SELECT_SEARCH_TEXT,
    SELECT_SOURCE_ITEM,
    SELECT_TARGET_ITEM,
    SEARCH_SOURCE_ITEMS,
    SEARCH_TARGET_ITEMS,
    MOVE_SOURCE_ITEMS,
    MOVE_ALL_SOURCE_ITEMS,
    MOVE_TARGET_ITEMS,
    MOVE_ALL_TARGET_ITEMS,
    DISABLE_PAGE_ACTIONS,
    HIDE_PAGE_ACTIONS,
    SHOW_HIDE_PAGE_ACTION,
    TOGGLE_PAGE_ACTIONS,
    SAVE_UPLOADFILE_FILTER,
    OPEN_CONFIRM,
    CLOSE_CONFIRM,
    RECEIVE_LOGISTIC_CENTERS,
    RECEIVE_STORAGE_LOCATIONS,
    GET_STORAGE_LOCATIONS,
    RECEIVE_REPORT_CONFIG,
    ENABLE_DISABLE_PAGE_ACTION,
    REQUEST_CATEGORIES,
    RECEIVE_CATEGORIES,
    GET_CATEGORIES,
    GET_LOGISTIC_CENTERS,
    REQUEST_LOGISTIC_CENTERS,
    REQUEST_STORAGE_LOCATIONS,
    REQUEST_SYSTEM_TYPES,
    RECEIVE_SYSTEM_TYPES,
    GET_SYSTEM_TYPES,
    REQUEST_VARIABLE_TYPES,
    RECEIVE_VARIABLE_TYPES,
    GET_VARIABLE_TYPES,
    REQUEST_ORIGIN_TYPES,
    RECEIVE_ORIGIN_TYPES,
    GET_ORIGIN_TYPES,
    RECEIVE_USERS,
    RECEIVE_FILE_READACCESSINFO,
    RECEIVE_FILE_READACCESSINFO_BY_CONTAINER,
    NODE_FILTER_ON_SELECT_CATEGORY,
    NODE_FILTER_RECEIVE_SEARCH_NODES,
    NODE_FILTER_ON_SELECT_ELEMENT,
    NODE_FILTER_CLEAR_SEARCH_NODES,
    NODE_FILTER_RESET_FIELDS,
    TOGGLE_COLOR_PICKER,
    SET_COLOR_PICKER,
    RESET_COLOR_PICKER,
    SET_ICON_ID,
    RESET_ICON_PICKER,
    RECEIVE_FLOW_CONFIG,
    RECEIVE_NODE_RULES,
    RECEIVE_NODEPRODUCT_RULES,
    RECEIVE_NODECONNECTIONPRODUCT_RULES,
    TRIGGER_BULK_UPDATE_POPUP,
    TOGGLE_BULK_UPDATE_POPUP,
    SET_SELECTED_NODE,
    INIT_SYNC_RULES,
    RECEIVE_RULES_SYNC_PROGRESS,
    REQUEST_SYNC_RULES,
    RECEIVE_SYNC_RULES,
    SET_VARIABLE_STATE,
    REPORT_REQUEST_COMPLETED,
    RESET_PAGE_ACTION,
    OPEN_COMPONENT_MODAL,
    RECEIVE_DATE_RANGES,
    RESET_DATE_RANGES,
    ENABLE_DISABLE_VIEW_REPORT_BUTTON,
    RECEIVE_BULK_UPDATE_POPUP,
    REFRESH_ANY_REPORT_BY_KEY
} from './actions';
import { utilities } from './services/utilities';
import { scenarioService } from './services/scenarioService';
import { systemConfigService } from './services/systemConfigService';
import { optionService } from './services/optionService';
import { constants } from './services/constants';
import { ai } from './telemetry/telemetryService';
import { supportConfigService } from './services/supportConfigService';

function buildMenuCollapsibleState(existingScenarios, scenario) {
    const result = Object.assign({}, existingScenarios);

    Object.keys(existingScenarios).forEach(x => {
        if (!utilities.isNullOrUndefined(scenario) && scenario.scenario === result[x].scenario) {
            result[x].isCollapsed = !existingScenarios[x].isCollapsed;
        } else {
            result[x].isCollapsed = true;
        }
    });

    return result;
}

export const root = function (state = { context: {}, scenarios: {} }, action = {}) {
    switch (action.type) {
    case BOOTSTRAP_APP_SUCCESS: {
        scenarioService.initialize(action.data.scenarios);
        systemConfigService.initialize(action.data.systemConfig);
        supportConfigService.initialize(action.data.supportConfig);
        ai.initialize(action.data.instrumentationKey);
        return Object.assign({},
            state,
            {
                appToggler: !state.appToggler,
                scenarios: scenarioService.getScenarios(),
                context: action.data.context,
                systemConfig: action.data.systemConfig
            });
    }
    case BOOTSTRAP_APP_FAILED: {
        return Object.assign({}, state, { error: true });
    }
    case APP_READY:
        return Object.assign({},
            state,
            {
                appReady: true,
                appReadyToggler: !state.appReadyToggler
            });
    case APP_MENU_ITEM_TOGGLE: {
        const scenario = state.scenarios[action.scenario];
        return Object.assign({},
            state,
            {
                scenarios: Object.assign({}, state.scenarios, buildMenuCollapsibleState(state.scenarios, scenario)),
                isOpen: true
            });
    }
    case APP_MENU_TOGGLE: {
        return Object.assign({},
            state,
            {
                scenarios: Object.assign({}, state.scenarios, buildMenuCollapsibleState(state.scenarios, { scenario: null })),
                isOpen: !state.isOpen
            });
    }
    case APP_MENU_CURRENT_MODULE: {
        return Object.assign({},
            state,
            {
                currentModule: action.moduleName
            });
    }
    default:
        return state;
    }
};

// Modal Reducer

export const modal = (state = {}, action = {}) => {
    switch (action.type) {
    case CLOSE_MODAL: {
        return Object.assign({},
            state,
            {
                isOpen: false
            });
    }
    case OPEN_MODAL:
        return Object.assign({},
            state,
            {
                modalKey: action.modalKey,
                title: action.title,
                isOpen: true,
                mode: action.mode,
                className: action.className,
                messageOnly: false,
                titleClassName: action.titleClassName,
                bodyClassName: action.bodyClassName
            });
    case OPEN_MESSAGE_ONLY_MODAL: {
        return Object.assign({},
            state,
            {
                messageOnly: true,
                isOpen: true,
                message: action.message,
                title: action.options.title || 'confirmation',
                className: action.options.className || 'ep-modal ep-modal--confirm',
                bodyClassName: action.options.bodyClassName || 'ep-modal__body',
                canCancel: action.options.canCancel === true,
                cancelAction: action.options.cancelAction,
                acceptAction: action.options.acceptAction,
                acceptActionAndClose: action.options.acceptActionAndClose,
                closeAction: action.options.closeAction,
                cancelActionTitle: action.options.cancelActionTitle || 'cancel',
                acceptActionTitle: action.options.acceptActionTitle || 'accept',
                titleClassName: action.options.titleClassName
            });
    }
    case OPEN_COMPONENT_MODAL: {
        return Object.assign({},
            state,
            {
                component: action.component,
                title: action.title,
                isOpen: true,
                mode: action.mode,
                className: action.className,
                messageOnly: false,
                titleClassName: action.titleClassName
            });
    }
    default:
        return state;
    }
};

// Confirm

// add comment
const confirmModalInitialState = {
    title: 'confirmation',
    isOpen: false,
    message: null,
    data: null
};

export const confirmModal = (state = confirmModalInitialState, action = {}) => {
    switch (action.type) {
    case OPEN_CONFIRM: {
        return Object.assign({},
            state,
            {
                isOpen: true,
                message: action.message,
                title: utilities.isNullOrWhitespace(action.title) ? state.title : action.title,
                data: action.data ? action.data : null,
                shouldShowCancelButton: action.shouldShowCancelButton,
                cancelButtonText: action.cancelButtonText
            });
    }
    case CLOSE_CONFIRM:
        return Object.assign({},
            confirmModalInitialState);
    default:
        return state;
    }
};

export const addComment = (state = {}, action = {}) => {
    switch (action.type) {
    case ADD_COMMENT: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    comment: action.comment,
                    commentToggler: !state[action.name].commentToggler
                })
            });
    }
    case INIT_ADD_COMMENT:
        return Object.assign({},
            state,
            {
                name: action.name,
                [action.name]: Object.assign({}, state[action.name] ? state[action.name] : {}, {
                    comment: '',
                    message: action.message,
                    component: action.component,
                    placeholder: action.placeholder,
                    required: action.required,
                    commentToggler: !utilities.isNullOrUndefined(state[action.name]) ? state[action.name].commentToggler : false
                })
            });
    default:
        return state;
    }
};

// Loader
export const loader = (state = { counter: 0 }, action = {}) => {
    switch (action.type) {
    case SHOW_LOADER:
        return Object.assign({},
            state,
            {
                counter: state.counter + 1
            });

    case HIDE_LOADER: {
        return Object.assign({},
            state,
            {
                counter: state.counter - 1
            });
    }
    default:
        return state;
    }
};

// Tab
export const tabs = (state = {}, action = {}) => {
    switch (action.type) {
    case INIT_TAB:
    case CHANGE_TAB:
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    activeTab: action.activeTab
                })
            });
    default:
        return state;
    }
};

// Flyout Reducer

export const flyout = (state = {}, action = {}) => {
    switch (action.type) {
    case INIT_FLYOUT: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    isOpen: action.isOpen
                })
            });
    }
    case OPEN_FLYOUT: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    isOpen: true
                })
            });
    }
    case CLOSE_FLYOUT: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    isOpen: false
                })
            }
        );
    }
    default:
        return state;
    }
};

// Shared reducer
const sharedInitialState = {
    categories: [],
    allCategories: [],
    homologationCategories: [],
    categoryElements: [],
    groupedCategoryElements: [],
    operationalSegments: [],
    users: [],
    progressStatus: {
        categoryElements: 0,
        allCategories: 0,
        logisticCenters: 0,
        storageLocations: 0,
        systemTypes: 0,
        variableTypes: 0,
        originTypes: 0
    },

    registrationActionTypes: {
        1: 'insert',
        2: 'update',
        3: 'delete',
        4: 'reInject'
    },
    logisticCenters: [],
    storageLocations: [],
    systemTypes: [],
    fileTypes: [],
    variableTypes: [],
    originTypes: [],
    readAccessInfoByContainer: {}
};

function buildTogglerState(state, action, property) {
    return state.progressStatus[property] === 0 || action.forceFetch === true ? !state[`${property}Toggler`] : state[`${property}Toggler`];
}

export const shared = (state = sharedInitialState, action = {}) => {
    switch (action.type) {
    case REQUEST_CATEGORY_ELEMENTS: {
        return Object.assign({},
            state,
            {
                progressStatus: utilities.changeState(state.progressStatus, 'categoryElements', 1)
            });
    }
    case RECEIVE_CATEGORY_ELEMENTS: {
        let categories = action.items.value.map(x => x.category);
        categories = categories.filter((e, i, self) =>
            i === self.findIndex(t => t.categoryId === e.categoryId));
        const groupedCategoryElements = utilities.normalizedGroupBy(action.items.value, 'categoryId');
        const operationalSegments = groupedCategoryElements[constants.Category.Segment].filter(x => x.isOperationalSegment && x.isActive);
        return Object.assign({},
            state,
            {
                categories: categories,
                categoryElements: action.items.value,
                groupedCategoryElements: groupedCategoryElements,
                operationalSegments: operationalSegments,
                progressStatus: utilities.changeState(state.progressStatus, 'categoryElements', 2),
                categoryElementsDataToggler: !state.categoryElementsDataToggler
            });
    }
    case GET_CATEGORY_ELEMENTS: {
        return Object.assign({},
            state,
            {
                categoryElementsToggler: buildTogglerState(state, action, 'categoryElements')
            });
    }
    case REQUEST_CATEGORIES: {
        return Object.assign({},
            state,
            {
                progressStatus: utilities.changeState(state.progressStatus, 'allCategories', 1)
            });
    }
    case RECEIVE_CATEGORIES: {
        return Object.assign({},
            state,
            {
                allCategories: action.items.value.filter(x => x.isHomologation === false),
                homologationCategories: action.items.value,
                progressStatus: utilities.changeState(state.progressStatus, 'allCategories', 2),
                categoriesDataToggler: !state.categoriesDataToggler
            });
    }
    case GET_CATEGORIES: {
        return Object.assign({},
            state,
            {
                allCategoriesToggler: buildTogglerState(state, action, 'allCategories')
            });
    }
    case REQUEST_SYSTEM_TYPES: {
        return Object.assign({},
            state,
            {
                progressStatus: utilities.changeState(state.progressStatus, 'systemTypes', 1)
            });
    }
    case RECEIVE_SYSTEM_TYPES: {
        const fileTypes = action.items.filter(x => x.isFileType);
        optionService.setFileTypes('fileTypes', fileTypes);
        return Object.assign({},
            state,
            {
                systemTypes: action.items,
                fileTypes,
                progressStatus: utilities.changeState(state.progressStatus, 'systemTypes', 2),
                systemTypesDataToggler: !state.systemTypesDataToggler
            });
    }
    case GET_SYSTEM_TYPES: {
        return Object.assign({},
            state,
            {
                systemTypesToggler: buildTogglerState(state, action, 'systemTypes')
            });
    }
    case REQUEST_LOGISTIC_CENTERS: {
        return Object.assign({},
            state,
            {
                progressStatus: utilities.changeState(state.progressStatus, 'logisticCenters', 1)
            });
    }
    case RECEIVE_LOGISTIC_CENTERS: {
        return Object.assign({},
            state,
            {
                logisticCenters: action.data,
                progressStatus: utilities.changeState(state.progressStatus, 'logisticCenters', 2),
                logisticCentersDataToggler: !state.logisticCentersDataToggler
            });
    }
    case GET_LOGISTIC_CENTERS: {
        return Object.assign({},
            state,
            {
                logisticCentersToggler: buildTogglerState(state, action, 'logisticCenters')
            });
    }
    case REQUEST_STORAGE_LOCATIONS: {
        return Object.assign({},
            state,
            {
                progressStatus: utilities.changeState(state.progressStatus, 'storageLocations', 1)
            });
    }
    case RECEIVE_STORAGE_LOCATIONS: {
        return Object.assign({},
            state,
            {
                storageLocations: action.data,
                progressStatus: utilities.changeState(state.progressStatus, 'storageLocations', 2),
                storageLocationsDataToggler: !state.logisticCentersDataToggler
            });
    }
    case GET_STORAGE_LOCATIONS: {
        return Object.assign({},
            state,
            {
                storageLocationsToggler: buildTogglerState(state, action, 'storageLocations')
            });
    }
    case REQUEST_VARIABLE_TYPES: {
        return Object.assign({},
            state,
            {
                progressStatus: utilities.changeState(state.progressStatus, 'variableTypes', 1)
            });
    }
    case RECEIVE_VARIABLE_TYPES: {
        return Object.assign({},
            state,
            {
                variableTypes: action.items,
                progressStatus: utilities.changeState(state.progressStatus, 'variableTypes', 2),
                variableTypesDataToggler: !state.variableTypesDataToggler
            });
    }
    case GET_VARIABLE_TYPES: {
        return Object.assign({},
            state,
            {
                variableTypesToggler: buildTogglerState(state, action, 'variableTypes')
            });
    }
    case REQUEST_ORIGIN_TYPES: {
        return Object.assign({},
            state,
            {
                progressStatus: utilities.changeState(state.progressStatus, 'originTypes', 1)
            });
    }
    case RECEIVE_ORIGIN_TYPES: {
        return Object.assign({},
            state,
            {
                originTypes: action.items,
                progressStatus: utilities.changeState(state.progressStatus, 'originTypes', 2),
                originTypesDataToggler: !state.originTypesDataToggler
            });
    }
    case GET_ORIGIN_TYPES: {
        return Object.assign({},
            state,
            {
                originTypesToggler: buildTogglerState(state, action, 'originTypes')
            });
    }
    case RECEIVE_USERS: {
        return Object.assign({},
            state,
            {
                users: action.users,
                usersDataToggler: !state.usersDataToggler
            });
    }
    case RECEIVE_FILE_READACCESSINFO: {
        return Object.assign({}, state, { readAccessInfo: action.accessInfo });
    }
    case RECEIVE_FILE_READACCESSINFO_BY_CONTAINER: {
        const readAccessInfoByContainer = Object.assign({}, state.readAccessInfoByContainer, { [action.container]: action.accessInfo });
        return Object.assign({}, state, { readAccessInfoByContainer: readAccessInfoByContainer });
    }
    default:
        return state;
    }
};

// Category element reducer
const categoryElementFilterInitialState = {
    defaultValues: {
        categoryElements: [
            {
                category: null, element: null
            }
        ]
    }
};

export const categoryElementFilter = (state = categoryElementFilterInitialState, action = {}) => {
    switch (action.type) {
    case INIT_CATEGORY_ELEMENT_FILTER: {
        const filterState = state[action.name];
        return Object.assign({},
            state,
            {
                [action.name]: utilities.isNullOrUndefined(filterState) ? {
                    selectedCategories: {},
                    values: state.defaultValues
                } : Object.assign({}, filterState)
            });
    }
    case CATEGORY_ELEMENTS_FILTER_RESET_FIELDS:
        return Object.assign({},
            state,
            {
                [action.name]: {
                    selectedCategories: [],
                    values: state.defaultValues
                }
            });
    case SAVE_CATEGORY_ELEMENT_FILTER:
    case SAVE_UPLOADFILE_FILTER: {
        const filterState = Object.assign({}, state[action.name], {
            values: Object.assign({}, action.values)
        });

        return Object.assign({},
            state,
            {
                [action.name]: filterState
            });
    }
    case CATEGORY_ELEMENT_FILTER_ON_SELECT_CATEGORY: {
        const filterState = Object.assign({}, state[action.name], {
            selectedCategories: Object.assign({}, state[action.name].selectedCategories, {
                [action.index]: action.item
            })
        });
        return Object.assign({},
            state,
            {
                [action.name]: filterState
            });
    }
    default:
        return state;
    }
};


// Wizard Reducer

export const wizard = (state = {}, action = {}) => {
    switch (action.type) {
    case INIT_WIZARD: {
        return Object.assign({},
            state,
            {
                [action.name]: {
                    activeStep: action.activeStep,
                    totalSteps: action.totalSteps
                }
            });
    }
    case WIZARD_SET_STEP: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    activeStep: action.activeStep
                })
            });
    }
    case WIZARD_PREV_STEP: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    activeStep: (state[action.name].activeStep <= 1) ? state[action.name].activeStep : (state[action.name].activeStep - 1)
                })
            });
    }
    case WIZARD_NEXT_STEP: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    activeStep: (state[action.name].activeStep >= state[action.name].totalSteps) ? state[action.name].activeStep : (state[action.name].activeStep + 1)
                })
            });
    }
    default:
        return state;
    }
};

// Notification
export const notification = (state = { isShow: false }, action = {}) => {
    switch (action.type) {
    case SHOW_NOTIFICATION: {
        return Object.assign({},
            state,
            {
                state: action.state,
                message: action.message,
                enableLink: action.enableLink,
                launchComponent: action.launchComponent,
                linkDescription: action.linkDescription,
                linkMessage: action.linkMessage,
                show: action.show,
                showOnModal: action.showOnModal,
                title: action.title,
                isButton: action.isButton,
                buttonText: action.buttonText,
                isCacheRefresh: action.isCacheRefresh,
                component: action.component
            });
    }
    case HIDE_NOTIFICATION: {
        return Object.assign({},
            state,
            {
                message: '',
                show: false
            });
    }

    default:
        return state;
    }
};

// Notification button
export const notificationButton = (state = {}, action = {}) => {
    if (action.type === INVOKE_NOTIFICATION_BUTTON_CLICK) {
        return Object.assign({}, state, { invokeNotificationButtonToggler: !action.invokeNotificationButtonToggler });
    }
    return state;
};

// Dual Select
function buildSource(all, target) {
    const source = [];
    all.filter(e => target.findIndex(t => t.id === e.id) < 0).forEach(s => {
        source.push(Object.assign({}, s));
    });

    return source;
}

function selectItem(items, id, ctrlKey) {
    const newItems = [];
    items.forEach(item => {
        const newItem = Object.assign({}, item);
        if (ctrlKey) {
            newItem.selected = (newItem.id === id || newItem.selected);
        } else if (newItem.selected) {
            newItem.selected = false;
        } else {
            newItem.selected = newItem.id === id ? true : false;
        }

        newItems.push(newItem);
    });

    return newItems;
}

function searchItems(items, name) {
    const newItems = [];
    items.filter(i => i.name.toLowerCase().includes(name.toLowerCase())).forEach(item => newItems.push(Object.assign({}, item)));

    return newItems;
}

function moveSelected(source, target, all) {
    const newItems = [];
    target.forEach(t => newItems.push(Object.assign({}, t)));

    const filtered = all === false ? source.filter(i => i.selected).map(s => Object.assign({}, s, { selected: false })) : source.map(s => Object.assign({}, s, { selected: false }));

    filtered.forEach(item => newItems.push(Object.assign({}, item, { selected: false })));
    return {
        source: all === false ? source.filter(i => i.selected === false).map(s => Object.assign({}, s, { selected: false })) : [],
        target: newItems.sort((a, b) => (a.name.toLowerCase() > b.name.toLowerCase()) ? 1 : -1)
    };
}

const dualSelectInitialState = {
    all: [],
    source: [],
    sourceSearch: [],
    sourceText: '',
    target: [],
    targetSearch: [],
    targetText: ''
};

export const dualSelect = (state = {}, action = {}) => {
    switch (action.type) {
    case CONFIGURE_DUAL_SELECT: {
        return Object.assign({}, state, {
            [action.name]: dualSelectInitialState
        });
    }
    case INIT_ALL_ITEMS: {
        const source = buildSource(action.all, state[action.name].target);
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    all: action.all,
                    source,
                    sourceSearch: source
                })
            });
    }
    case INIT_TARGET_ITEMS: {
        const source = buildSource(state[action.name].all, action.target);
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    target: action.target,
                    targetSearch: action.target,
                    source,
                    sourceSearch: source
                })
            });
    }
    case SELECT_SOURCE_ITEM: {
        const source = selectItem(state[action.name].source, action.id, action.ctrlKey);
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    source,
                    sourceSearch: searchItems(source, state[action.name].sourceText)
                })
            });
    }
    case SELECT_TARGET_ITEM: {
        const target = selectItem(state[action.name].target, action.id, action.ctrlKey);
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    target,
                    targetSearch: searchItems(target, state[action.name].targetText)
                })
            });
    }
    case SEARCH_SOURCE_ITEMS: {
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    sourceText: action.text,
                    sourceSearch: searchItems(state[action.name].source, action.text)
                })
            });
    }
    case SEARCH_TARGET_ITEMS: {
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    targetText: action.text,
                    targetSearch: searchItems(state[action.name].target, action.text)
                })
            });
    }
    case MOVE_SOURCE_ITEMS: {
        const movement = moveSelected(state[action.name].source, state[action.name].target, false);
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    source: movement.source,
                    sourceSearch: searchItems(movement.source, state[action.name].sourceText),
                    target: movement.target,
                    targetSearch: searchItems(movement.target, state[action.name].targetText)
                })
            });
    }
    case MOVE_ALL_SOURCE_ITEMS: {
        const movement = moveSelected(state[action.name].source, state[action.name].target, true);
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    source: movement.source,
                    sourceSearch: searchItems(movement.source, state[action.name].sourceText),
                    target: movement.target,
                    targetSearch: searchItems(movement.target, state[action.name].targetText)
                })
            });
    }
    case MOVE_TARGET_ITEMS: {
        const movement = moveSelected(state[action.name].target, state[action.name].source, false);
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    source: movement.target,
                    sourceSearch: searchItems(movement.target, state[action.name].sourceText),
                    target: movement.source,
                    targetSearch: searchItems(movement.source, state[action.name].targetText)
                })
            });
    }
    case MOVE_ALL_TARGET_ITEMS: {
        const movement = moveSelected(state[action.name].target, state[action.name].source, true);
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    source: movement.target,
                    sourceSearch: searchItems(movement.target, state[action.name].sourceText),
                    target: movement.source,
                    targetSearch: searchItems(movement.source, state[action.name].targetText)
                })
            });
    }
    case UPDATE_TARGET_ITEMS: {
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    target: action.target,
                    targetSearch: action.target
                })
            });
    }
    case CLEAR_DUAL_SELECT_SEARCH_TEXT: {
        return Object.assign({}, state, {
            [action.name]: Object.assign({}, state[action.name], {
                sourceText: '',
                targetText: ''
            })
        });
    }
    default:
        return state;
    }
};

const pageActionsInitialState = {
    disabledActions: [],
    hiddenActions: []
};

function buildActions(allActions, actionName, isAllowed) {
    const updatedActions = allActions.filter(x => x !== actionName);
    if (isAllowed) {
        updatedActions.push(actionName);
    }

    return updatedActions;
}

export const pageActions = function (state = pageActionsInitialState, action = {}) {
    switch (action.type) {
    case TOGGLE_PAGE_ACTIONS: {
        return Object.assign({},
            state,
            {
                enabled: action.enabled
            });
    }
    case DISABLE_PAGE_ACTIONS: {
        return Object.assign({},
            state,
            {
                disabledActions: action.disabledActions
            });
    }
    case ENABLE_DISABLE_PAGE_ACTION: {
        return Object.assign({},
            state,
            {
                disabledActions: buildActions(state.disabledActions, action.actionName, action.disabled)
            });
    }
    case HIDE_PAGE_ACTIONS: {
        return Object.assign({},
            state,
            {
                hiddenActions: action.hiddenActions
            });
    }
    case SHOW_HIDE_PAGE_ACTION: {
        return Object.assign({},
            state,
            {
                hiddenActions: buildActions(state.hiddenActions, action.actionName, action.hidden)
            });
    }
    case RESET_PAGE_ACTION: {
        return Object.assign({},
            state, {
                disabledActions: [],
                hiddenActions: [],
                enabled: false
            });
    }
    default:
        return state;
    }
};

// reporting
export const reports = function (state = {}, action = {}) {
    switch (action.type) {
    case RECEIVE_REPORT_CONFIG: {
        const refreshToggler = state[action.key] && state[action.key].refreshToggler;
        return Object.assign({}, state, { [action.key]: { config: action.data, refreshToggler: !refreshToggler } });
    }
    case REFRESH_ANY_REPORT_BY_KEY: {
        const refreshToggler = state[action.key] && state[action.key].refreshToggler;
        const data = state[action.key] && state[action.key].config;
        return Object.assign({},
            state,
            {
                [action.key]: {
                    config: data,
                    refreshToggler: !refreshToggler
                }
            });
    }
    default:
        return state;
    }
};


// Color Picker
export const colorPicker = (state = {}, action = {}) => {
    switch (action.type) {
    case TOGGLE_COLOR_PICKER: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    isOpen: action.isOpen
                })
            });
    }
    case SET_COLOR_PICKER: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    color: action.color
                })
            });
    }
    case RESET_COLOR_PICKER: {
        return {};
    }
    default:
        return state;
    }
};

// Icon Picker
export const iconPicker = (state = {}, action = {}) => {
    switch (action.type) {
    case SET_ICON_ID: {
        return Object.assign({},
            state,
            {
                id: action.id,
                name: action.name
            });
    }
    case RESET_ICON_PICKER: {
        return {};
    }
    default:
        return state;
    }
};


// power automate
export const powerAutomate = (state = {}, action = {}) => {
    if (action.type === RECEIVE_FLOW_CONFIG) {
        const refreshToggler = state[action.key] && state[action.key].refreshToggler;
        return Object.assign({}, state, { [action.key]: { config: action.data, refreshToggler: !refreshToggler } });
    }
    return state;
};

// node filter reducer
const nodeFilterInitialState = {
    selectedCategory: null,
    selectedElement: null,
    searchedNodes: [],
    dateRange: {},
    defaultYear: null,
    viewReportButtonStatusToggler: false
};

export const nodeFilter = (state = nodeFilterInitialState, action = {}) => {
    switch (action.type) {
    case NODE_FILTER_ON_SELECT_CATEGORY:
        return Object.assign({},
            state,
            {
                selectedCategory: action.selectedCategory
            });
    case NODE_FILTER_ON_SELECT_ELEMENT:
        return Object.assign({},
            state,
            {
                selectedElement: action.selectedElement
            });
    case NODE_FILTER_RECEIVE_SEARCH_NODES:
        return Object.assign({},
            state,
            {
                searchedNodes: action.nodes
            });
    case NODE_FILTER_CLEAR_SEARCH_NODES:
        return Object.assign({},
            state,
            {
                searchedNodes: []
            });
    case NODE_FILTER_RESET_FIELDS:
        return Object.assign({},
            state,
            {
                selectedCategory: [],
                selectedElement: [],
                searchedNodes: []
            });
    case REPORT_REQUEST_COMPLETED:
        return Object.assign({},
            state,
            {
                reportToggler: !state.reportToggler
            });
    case RECEIVE_DATE_RANGES:
        return Object.assign({},
            state,
            {
                dateRange: action.data.officialPeriods,
                defaultYear: action.data.defaultYear,
                dateRangeToggler: !state.dateRangeToggler
            });
    case RESET_DATE_RANGES:
        return Object.assign({},
            state,
            {
                dateRange: {},
                defaultYear: null,
                dateRangeToggler: null,
                viewReportButtonStatusToggler: false
            });
    case ENABLE_DISABLE_VIEW_REPORT_BUTTON:
        return Object.assign({},
            state,
            {
                viewReportButtonStatusToggler: action.buttonStatus
            });
    default:
        return state;
    }
};

// Ownership Rules
export const ownershipRules = (state = { rules: [] }, action = {}) => {
    switch (action.type) {
    case TRIGGER_BULK_UPDATE_POPUP: {
        const rules = [...new Set(action.selection.map(r => utilities.getValue(r, action.path)))].filter(e => e !== null);
        return Object.assign({}, state, {
            [action.name]: Object.assign({}, state[action.name], {
                items: action.selection,
                rules,
                variableTypes: !utilities.isNullOrUndefined(action.selection[0].nodeProductRule) ? action.selection[0].storageLocationProductVariables.map(item => {
                    return { variableTypeId: item.variableTypeId, name: item.variableType.name, ficoName: item.variableType.ficoName };
                }) : [],
                ownershipRules: action.isContractualStrategy ? action.selection[0].nodeProductRule : null,
                isContractualStrategy: action.isContractualStrategy,
                confirmToggler: rules.length > 1 ? !(state[action.name] && state[action.name].confirmToggler) : (state[action.name] && state[action.name].confirmToggler),
                updateToggler: rules.length > 1 ? (state[action.name] && state[action.name].updateToggler) : !(state[action.name] && state[action.name].updateToggler)
            })
        });
    }
    case RECEIVE_NODE_RULES:
    case RECEIVE_NODEPRODUCT_RULES:
    case RECEIVE_NODECONNECTIONPRODUCT_RULES: {
        return Object.assign({},
            state,
            {
                rules: action.rules
            });
    }
    case TOGGLE_BULK_UPDATE_POPUP: {
        return Object.assign({}, state, {
            [action.name]: Object.assign({}, state[action.name], {
                updateToggler: !(state[action.name] && state[action.name].updateToggler)
            })
        });
    }
    case RECEIVE_BULK_UPDATE_POPUP: {
        return Object.assign({}, state, {
            [action.name]: Object.assign({}, state[action.name], {
                receiveUpdatedToggler: !(state[action.name] && state[action.name].receiveUpdatedToggler)
            })
        });
    }
    case SET_SELECTED_NODE: {
        return Object.assign({},
            state,
            {
                selectedNodeId: action.nodeId
            });
    }
    case SET_VARIABLE_STATE: {
        return Object.assign({}, state, {
            [action.name]: Object.assign({}, state[action.name], {
                ownershipRules: action.ownership,
                variableTypes: !utilities.isNullOrUndefined(action.variables) ? action.variables : null
            })
        });
    }
    default:
        return state;
    }
};

// Rule Sync
function buildSyncState(status, triggered) {
    if (triggered === true && status === true) {
        return { triggered: true, state: constants.SyncStatus.Info };
    }

    if (triggered === true && status === false) {
        return { triggered: false, state: constants.SyncStatus.Success };
    }

    if (triggered === false && status === true) {
        return { triggered: true, state: constants.SyncStatus.Info };
    }

    return { triggered: false, state: constants.SyncStatus.NotReady };
}
export const ruleSynchronizer = (state = {}, action = {}) => {
    switch (action.type) {
    case INIT_SYNC_RULES: {
        return Object.assign({}, state, {
            [action.name]: {
                state: constants.SyncStatus.NotReady,
                enabled: false,
                triggered: false
            }
        });
    }
    case RECEIVE_RULES_SYNC_PROGRESS: {
        const syncState = buildSyncState(action.status, state[action.name].triggered);
        return Object.assign({}, state, {
            [action.name]: Object.assign({}, state[action.name], {
                state: syncState.state,
                enabled: !action.status,
                triggered: syncState.triggered,
                timerToggler: action.init ? !state[action.name].timerToggler : state[action.name].timerToggler
            })
        });
    }
    case REQUEST_SYNC_RULES: {
        return Object.assign({}, state, {
            [action.name]: Object.assign({}, state[action.name], {
                state: constants.SyncStatus.Info,
                enabled: false
            })
        });
    }
    case RECEIVE_SYNC_RULES: {
        return Object.assign({}, state, {
            [action.name]: Object.assign({}, state[action.name], {
                state: action.status === constants.Status.Processed ? constants.SyncStatus.Info : constants.SyncStatus.Failed,
                triggered: action.status === constants.Status.Processed,
                enabled: action.status === constants.Status.Failed,
                timerToggler: !state[action.name].timerToggler,
                errorToggler: action.status === 2 ? !state[action.name].errorToggler : state[action.name].errorToggler
            })
        });
    }
    default:
        return state;
    }
};
