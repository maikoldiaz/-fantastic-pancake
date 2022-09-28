import { apiService } from './services/apiService.js';
import { constants } from './services/constants.js';
import { utilities } from './services/utilities.js';

export const BOOTSTRAP_APP = 'BOOTSTRAP_APP';
export const BOOTSTRAP_APP_SUCCESS = 'BOOTSTRAP_APP_SUCCESS';
export const BOOTSTRAP_APP_FAILED = 'BOOTSTRAP_APP_FAILED';

export const APP_READY = 'APP_READY';

export const APP_MENU_ITEM_TOGGLE = 'APP_MENU_ITEM_TOGGLE';
export const APP_MENU_TOGGLE = 'APP_MENU_TOGGLE';
export const ADD_COMMENT = 'ADD_COMMENT';
export const INIT_ADD_COMMENT = 'INIT_ADD_COMMENT';
export const APP_MENU_CURRENT_MODULE = 'APP_MENU_CURRENT_MODULE';


export const appReady = () => {
    return {
        type: APP_READY
    };
};

export const bootstrapSuccess = data => {
    return {
        type: BOOTSTRAP_APP_SUCCESS,
        data
    };
};

export const bootstrapFailure = () => {
    return {
        type: BOOTSTRAP_APP_FAILED
    };
};

export const bootstrapApp = () => {
    return {
        type: BOOTSTRAP_APP,
        fetchConfig: {
            path: apiService.bootstrap(),
            success: bootstrapSuccess,
            failure: bootstrapFailure
        }
    };
};

export const toggleMenu = () => {
    return {
        type: APP_MENU_TOGGLE
    };
};

export const toggleMenuItem = scenario => {
    return {
        type: APP_MENU_ITEM_TOGGLE,
        scenario
    };
};

export const setModuleName = moduleName => {
    return {
        type: APP_MENU_CURRENT_MODULE,
        moduleName
    };
};

// Loader
export const SHOW_LOADER = 'SHOW_LOADER';
export const HIDE_LOADER = 'HIDE_LOADER';

export const showLoader = function () {
    return {
        type: SHOW_LOADER
    };
};

export const hideLoader = function () {
    return { type: HIDE_LOADER };
};

// MODAL
export const OPEN_MODAL = 'OPEN_MODAL';
export const CLOSE_MODAL = 'CLOSE_MODAL';
export const RESET_MODAL = 'RESET_MODAL';
export const SET_MODAL_DIRTY = 'SET_MODAL_DIRTY';
export const HANDLE_CLOSE_MODAL = 'HANDLE_CLOSE_MODAL';
export const OPEN_MESSAGE_ONLY_MODAL = 'OPEN_MESSAGE_ONLY_MODAL';
export const OPEN_COMPONENT_MODAL = 'OPEN_COMPONENT_MODAL';

/**
 * Action to open a modal
 * @param {*} modalKey
 * @param {*} mode
 * @param {*} title
 * @param {*} className
 * @param  {...any} styleProps [0] -> title class name [1] bodyClassName
 * @returns
 */

export const openModal = (modalKey, mode = '', title = '', className = 'ep-modal', ...styleProps) => {
    const titleClassName = styleProps[0];
    const bodyClassName = styleProps[1] || 'ep-modal__body';
    return {
        type: OPEN_MODAL,
        modalKey,
        title,
        mode,
        className,
        titleClassName,
        bodyClassName
    };
};

export const openComponentModal = (component, mode = '', title = '', className = 'ep-modal', titleClassName = '') => {
    return {
        type: OPEN_COMPONENT_MODAL,
        component,
        title,
        mode,
        className,
        titleClassName
    };
};

export const openMessageModal = (message, options) => {
    return {
        type: OPEN_MESSAGE_ONLY_MODAL,
        message,
        options
    };
};

export const closeModal = (displayWarning = false) => {
    return { type: CLOSE_MODAL, displayWarning };
};

// Confirm

export const OPEN_CONFIRM = 'OPEN_CONFIRM';
export const CLOSE_CONFIRM = 'CLOSE_CONFIRM';

export const openConfirm = (message, title, data, cancelButtonText = '', shouldShowCancelButton = true) => {
    return {
        type: OPEN_CONFIRM,
        title,
        message,
        data,
        shouldShowCancelButton,
        cancelButtonText
    };
};

export const closeConfirm = () => {
    return { type: CLOSE_CONFIRM };
};


// add comment
export const addComment = (comment, name) => {
    return {
        type: ADD_COMMENT,
        comment,
        name
    };
};

export const intAddComment = (name, message, component = null, placeholder = null, required = null) => {
    return {
        type: INIT_ADD_COMMENT,
        name,
        message,
        component,
        placeholder,
        required
    };
};

// Tabs

export const INIT_TAB = 'INIT_TAB';
export const CHANGE_TAB = 'CHANGE_TAB';

export const initTab = (name, activeTab) => {
    return {
        type: INIT_TAB,
        name,
        activeTab
    };
};

export const changeTab = (name, activeTab) => {
    return {
        type: CHANGE_TAB,
        name,
        activeTab
    };
};

// Flyout

export const INIT_FLYOUT = 'INIT_FLYOUT';
export const OPEN_FLYOUT = 'OPEN_FLYOUT';
export const CLOSE_FLYOUT = 'CLOSE_FLYOUT';

export const initFlyout = (name, isOpen) => {
    return {
        type: INIT_FLYOUT,
        name,
        isOpen
    };
};

export const openFlyout = name => {
    return {
        type: OPEN_FLYOUT,
        name
    };
};

export const closeFlyout = name => {
    return {
        type: CLOSE_FLYOUT,
        name
    };
};

export const GET_CATEGORIES = 'GET_CATEGORIES';
export const REQUEST_CATEGORIES = 'REQUEST_CATEGORIES';
export const RECEIVE_CATEGORIES = 'RECEIVE_CATEGORIES';

export const receiveCategories = function (json) {
    return {
        type: RECEIVE_CATEGORIES,
        items: json
    };
};

export const requestCategories = function () {
    return {
        type: REQUEST_CATEGORIES,
        fetchConfig: {
            path: apiService.category.query(),
            success: json => receiveCategories(json)
        }
    };
};

export const getCategories = function (forceFetch = false) {
    return {
        type: GET_CATEGORIES,
        forceFetch
    };
};

export const GET_CATEGORY_ELEMENTS = 'GET_CATEGORY_ELEMENTS';
export const REQUEST_CATEGORY_ELEMENTS = 'REQUEST_CATEGORY_ELEMENTS';
export const RECEIVE_CATEGORY_ELEMENTS = 'RECEIVE_CATEGORY_ELEMENTS';

export const receiveCategoryElements = function (json) {
    return {
        type: RECEIVE_CATEGORY_ELEMENTS,
        items: json,
        isLoading: false
    };
};

export const requestCategoryElements = function () {
    return {
        type: REQUEST_CATEGORY_ELEMENTS,
        fetchConfig: {
            path: apiService.getCategoryElements(),
            method: 'GET',
            success: json => receiveCategoryElements(json)
        }
    };
};

export const getCategoryElements = function (forceFetch = false) {
    return {
        type: GET_CATEGORY_ELEMENTS,
        forceFetch
    };
};

export const CATEGORY_ELEMENTS_FILTER_RESET_FIELDS = 'CATEGORY_ELEMENTS_FILTER_RESET_FIELDS';
export const categoryElementFilterResetFields = name => {
    return {
        type: CATEGORY_ELEMENTS_FILTER_RESET_FIELDS,
        name
    };
};

export const REQUEST_SYSTEM_TYPES = 'REQUEST_SYSTEM_TYPES';
export const RECEIVE_SYSTEM_TYPES = 'RECEIVE_SYSTEM_TYPES';
export const GET_SYSTEM_TYPES = 'GET_SYSTEM_TYPES';

export const receiveSystemTypes = function (json) {
    return {
        type: RECEIVE_SYSTEM_TYPES,
        items: json.value,
        isLoading: false
    };
};

export const requestSystemTypes = function () {
    return {
        type: REQUEST_SYSTEM_TYPES,
        fetchConfig: {
            path: apiService.getSystemTypes(),
            method: 'GET',
            success: json => receiveSystemTypes(json)
        }
    };
};

export const getSystemTypes = function (forceFetch = false) {
    return {
        type: GET_SYSTEM_TYPES,
        forceFetch
    };
};


export const REQUEST_VARIABLE_TYPES = 'REQUEST_VARIABLE_TYPES';
export const RECEIVE_VARIABLE_TYPES = 'RECEIVE_VARIABLE_TYPES';
export const GET_VARIABLE_TYPES = 'GET_VARIABLE_TYPES';

export const receiveVariableTypes = function (json) {
    return {
        type: RECEIVE_VARIABLE_TYPES,
        items: json,
        isLoading: false
    };
};

export const requestVariableTypes = function () {
    return {
        type: REQUEST_VARIABLE_TYPES,
        fetchConfig: {
            path: apiService.getVariableTypes(),
            success: receiveVariableTypes
        }
    };
};

export const getVariableTypes = function (forceFetch = false) {
    return {
        type: GET_VARIABLE_TYPES,
        forceFetch
    };
};

export const SAVE_CATEGORY_ELEMENT_FILTER = 'SAVE_CATEGORY_ELEMENT_FILTER';
export const INIT_CATEGORY_ELEMENT_FILTER = 'INIT_CATEGORY_ELEMENT_FILTER';
export const CATEGORY_ELEMENT_FILTER_ON_SELECT_CATEGORY = 'CATEGORY_ELEMENT_FILTER_ON_SELECT_CATEGORY';
export const SAVE_UPLOADFILE_FILTER = 'SAVE_UPLOADFILE_FILTER';

export const saveCategoryElementFilter = function (name, values) {
    return {
        type: SAVE_CATEGORY_ELEMENT_FILTER,
        name,
        values
    };
};

export const saveUploadFileFilter = function (name, values) {
    return {
        type: SAVE_UPLOADFILE_FILTER,
        name,
        values
    };
};

export const initCategoryElementFilter = function (name) {
    return {
        type: INIT_CATEGORY_ELEMENT_FILTER,
        name
    };
};

export const onCategorySelection = function (item, index, name) {
    return {
        type: CATEGORY_ELEMENT_FILTER_ON_SELECT_CATEGORY,
        item,
        index,
        name
    };
};

// Wizard

export const INIT_WIZARD = 'INIT_WIZARD';
export const WIZARD_SET_STEP = 'WIZARD_SET_STEP';
export const WIZARD_NEXT_STEP = 'WIZARD_NEXT_STEP';
export const WIZARD_PREV_STEP = 'WIZARD_PREV_STEP';

export const initWizard = function (name, activeStep, totalSteps) {
    return {
        type: INIT_WIZARD,
        name,
        activeStep,
        totalSteps
    };
};

export const wizardSetStep = function (name, activeStep) {
    return {
        type: WIZARD_SET_STEP,
        name,
        activeStep
    };
};

export const wizardNextStep = function (name) {
    return {
        type: WIZARD_NEXT_STEP,
        name
    };
};

export const wizardPrevStep = function (name) {
    return {
        type: WIZARD_PREV_STEP,
        name
    };
};

// Notification

export const SHOW_NOTIFICATION = 'SHOW_NOTIFICATION';
export const HIDE_NOTIFICATION = 'HIDE_NOTIFICATION';

export const showNotification = function (state, message, showOnModal = false, title = '') {
    return {
        type: SHOW_NOTIFICATION,
        state,
        message,
        enableLink: false,
        launchComponent: null,
        linkDescription: null,
        linkMessage: null,
        show: true,
        showOnModal,
        title,
        component: null
    };
};

export const hideNotification = function () {
    return {
        type: HIDE_NOTIFICATION,
        enableLink: false,
        isButton: false,
        launchComponent: null,
        linkDescription: null,
        linkMessage: null,
        show: false
    };
};

export const showError = function (message, showOnModal = false, title = '') {
    return {
        type: SHOW_NOTIFICATION,
        state: constants.NotificationType.Error,
        message,
        title,
        enableLink: false,
        launchComponent: null,
        linkDescription: null,
        linkMessage: null,
        show: true,
        showOnModal,
        component: null
    };
};

export const showErrorComponent = function (component, showOnModal = false, title = '') {
    return {
        type: SHOW_NOTIFICATION,
        state: constants.NotificationType.Error,
        component,
        title,
        enableLink: false,
        launchComponent: null,
        linkDescription: null,
        linkMessage: null,
        show: true,
        showOnModal
    };
};

export const showLinkError = function (message, launchComponent, linkDescription, linkMessage) {
    return {
        type: SHOW_NOTIFICATION,
        state: constants.NotificationType.Error,
        message,
        enableLink: true,
        launchComponent,
        linkDescription,
        linkMessage,
        show: true,
        showOnModal: false,
        component: null
    };
};

export const showInfoWithButton = function (message, showOnModal = false, title = '', buttonText = '') {
    return {
        type: SHOW_NOTIFICATION,
        state: constants.NotificationType.Info,
        message,
        title,
        isButton: !utilities.isNullOrWhitespace(buttonText),
        buttonText,
        enableLink: false,
        launchComponent: null,
        linkDescription: null,
        linkMessage: null,
        show: true,
        showOnModal,
        component: null
    };
};

export const showWarning = function (message, showOnModal = false, title = '') {
    return {
        type: SHOW_NOTIFICATION,
        state: constants.NotificationType.Warning,
        message,
        title,
        enableLink: false,
        launchComponent: null,
        linkDescription: null,
        linkMessage: null,
        show: true,
        showOnModal,
        component: null
    };
};

export const showInfo = function (message, showOnModal = false, title = '') {
    return {
        type: SHOW_NOTIFICATION,
        state: constants.NotificationType.Info,
        message,
        title,
        enableLink: false,
        launchComponent: null,
        linkDescription: null,
        linkMessage: null,
        show: true,
        showOnModal,
        component: null
    };
};

export const showSuccess = function (message, showOnModal = false, title = '') {
    return {
        type: SHOW_NOTIFICATION,
        state: constants.NotificationType.Success,
        message,
        title,
        enableLink: false,
        launchComponent: null,
        linkDescription: null,
        linkMessage: null,
        show: true,
        showOnModal,
        component: null
    };
};

// Notification button

export const INVOKE_NOTIFICATION_BUTTON_CLICK = 'INVOKE_NOTIFICATION_BUTTON_CLICK';

export const invokeButton = function (invokeNotificationButtonToggler) {
    return {
        type: INVOKE_NOTIFICATION_BUTTON_CLICK,
        invokeNotificationButtonToggler
    };
};

// Dual Select
export const CONFIGURE_DUAL_SELECT = 'CONFIGURE_DUAL_SELECT';
export const INIT_ALL_ITEMS = 'INIT_ALL_ITEMS';
export const INIT_TARGET_ITEMS = 'INIT_TARGET_ITEMS';
export const SELECT_SOURCE_ITEM = 'SELECT_SOURCE_ITEM';
export const SELECT_TARGET_ITEM = 'SELECT_TARGET_ITEM';
export const SEARCH_SOURCE_ITEMS = 'SEARCH_SOURCE_ITEMS';
export const SEARCH_TARGET_ITEMS = 'SEARCH_TARGET_ITEMS';
export const MOVE_SOURCE_ITEMS = 'MOVE_SOURCE_ITEM';
export const MOVE_ALL_SOURCE_ITEMS = 'MOVE_ALL_SOURCE_ITEMS';
export const MOVE_TARGET_ITEMS = 'MOVE_TARGET_ITEMS';
export const MOVE_ALL_TARGET_ITEMS = 'MOVE_ALL_TARGET_ITEMS';
export const UPDATE_TARGET_ITEMS = 'UPDATE_TARGET_ITEMS';
export const CLEAR_DUAL_SELECT_SEARCH_TEXT = 'CLEAR_DUAL_SELECT_SEARCH_TEXT';

export const configureDualSelect = name => {
    return {
        type: CONFIGURE_DUAL_SELECT,
        name
    };
};

export const initAllItems = (all, name) => {
    return {
        type: INIT_ALL_ITEMS,
        all,
        name
    };
};

export const initTargetItems = (target, name) => {
    return {
        type: INIT_TARGET_ITEMS,
        target,
        name
    };
};

export const selectSource = (id, ctrlKey, name) => {
    return {
        type: SELECT_SOURCE_ITEM,
        id,
        ctrlKey,
        name
    };
};

export const selectTarget = (id, ctrlKey, name) => {
    return {
        type: SELECT_TARGET_ITEM,
        id,
        ctrlKey,
        name
    };
};

export const searchSource = (text, name) => {
    return {
        type: SEARCH_SOURCE_ITEMS,
        text,
        name
    };
};

export const searchTarget = (text, name) => {
    return {
        type: SEARCH_TARGET_ITEMS,
        text,
        name
    };
};

export const move = name => {
    return {
        type: MOVE_SOURCE_ITEMS,
        name
    };
};

export const moveAll = name => {
    return {
        type: MOVE_ALL_SOURCE_ITEMS,
        name
    };
};

export const moveBack = name => {
    return {
        type: MOVE_TARGET_ITEMS,
        name
    };
};

export const moveBackAll = name => {
    return {
        type: MOVE_ALL_TARGET_ITEMS,
        name
    };
};

export const updateTargetItems = (target, name) => {
    return {
        type: UPDATE_TARGET_ITEMS,
        target,
        name
    };
};

export const clearSearchText = name => {
    return {
        type: CLEAR_DUAL_SELECT_SEARCH_TEXT,
        name
    };
};

export const TOGGLE_PAGE_ACTIONS = 'TOGGLE_PAGE_ACTIONS';
export const DISABLE_PAGE_ACTIONS = 'DISABLE_PAGE_ACTIONS';
export const RESET_PAGE_ACTION = 'RESET_PAGE_ACTION';
export const togglePageActions = function (enabled) {
    return {
        type: TOGGLE_PAGE_ACTIONS,
        enabled
    };
};
export const disablePageActions = function (disabledActions) {
    return {
        type: DISABLE_PAGE_ACTIONS,
        disabledActions
    };
};

export const ENABLE_DISABLE_PAGE_ACTION = 'ENABLE_DISABLE_PAGE_ACTION';
export const enableDisablePageAction = function (actionName, disabled) {
    return {
        type: ENABLE_DISABLE_PAGE_ACTION,
        actionName,
        disabled
    };
};

export const HIDE_PAGE_ACTIONS = 'HIDE_PAGE_ACTIONS';
export const hidePageActions = function (hiddenActions) {
    return {
        type: HIDE_PAGE_ACTIONS,
        hiddenActions
    };
};

export const SHOW_HIDE_PAGE_ACTION = 'SHOW_HIDE_PAGE_ACTION';
export const showPageAction = function (actionName) {
    return {
        type: SHOW_HIDE_PAGE_ACTION,
        actionName,
        hidden: false
    };
};

export const hidePageAction = function (actionName) {
    return {
        type: SHOW_HIDE_PAGE_ACTION,
        actionName,
        hidden: true
    };
};

export const resetPageAction = () => {
    return {
        type: RESET_PAGE_ACTION
    };
};

export const REQUEST_LOGISTIC_CENTERS = 'REQUEST_LOGISTIC_CENTERS';
export const RECEIVE_LOGISTIC_CENTERS = 'RECEIVE_LOGISTIC_CENTERS';
export const GET_LOGISTIC_CENTERS = 'GET_LOGISTIC_CENTERS';

export const receiveLogisticCentersSuccess = data => {
    return {
        type: RECEIVE_LOGISTIC_CENTERS,
        data
    };
};

export const requestLogisticCenters = () => {
    return {
        type: REQUEST_LOGISTIC_CENTERS,
        fetchConfig: {
            path: apiService.getLogisticCenters(),
            success: receiveLogisticCentersSuccess
        }
    };
};

export const getLogisticCenters = function (forceFetch = false) {
    return {
        type: GET_LOGISTIC_CENTERS,
        forceFetch
    };
};

export const REQUEST_STORAGE_LOCATIONS = 'REQUEST_STORAGE_LOCATIONS';
export const RECEIVE_STORAGE_LOCATIONS = 'RECEIVE_STORAGE_LOCATIONS';
export const GET_STORAGE_LOCATIONS = 'GET_STORAGE_LOCATIONS';

export const receiveStorageLocations = data => {
    return {
        type: RECEIVE_STORAGE_LOCATIONS,
        data
    };
};

export const requestStorageLocations = () => {
    return {
        type: REQUEST_STORAGE_LOCATIONS,
        fetchConfig: {
            path: apiService.getStorageLocations(),
            success: json => receiveStorageLocations(json.value)
        }
    };
};

export const getStorageLocations = function (forceFetch = false) {
    return {
        type: GET_STORAGE_LOCATIONS,
        forceFetch
    };
};


export const GET_ORIGIN_TYPES = 'GET_ORIGIN_TYPES';
export const REQUEST_ORIGIN_TYPES = 'REQUEST_ORIGIN_TYPES';
export const RECEIVE_ORIGIN_TYPES = 'RECEIVE_ORIGIN_TYPES';

export const receiveOriginTypes = function (json) {
    return {
        type: RECEIVE_ORIGIN_TYPES,
        items: json,
        isLoading: false
    };
};

export const requestOriginTypes = function () {
    return {
        type: REQUEST_ORIGIN_TYPES,
        fetchConfig: {
            path: apiService.getOriginTypes(),
            success: receiveOriginTypes
        }
    };
};

export const getOriginTypes = function (forceFetch = false) {
    return {
        type: GET_ORIGIN_TYPES,
        forceFetch
    };
};

export const REQUEST_USERS = 'REQUEST_USERS';
export const RECEIVE_USERS = 'RECEIVE_USERS';

export const receiveUsers = users => {
    return {
        type: RECEIVE_USERS,
        users
    };
};

export const requestUsers = () => {
    return {
        type: REQUEST_USERS,
        fetchConfig: {
            path: apiService.getUsers(),
            success: receiveUsers
        }
    };
};

export const REQUEST_NODE_RULES = 'REQUEST_NODE_RULES';
export const RECEIVE_NODE_RULES = 'RECEIVE__NODE_RULES';

export const receiveNodeRules = rules => {
    return {
        type: RECEIVE_NODE_RULES,
        rules
    };
};

export const requestNodeRules = () => {
    return {
        type: REQUEST_NODE_RULES,
        fetchConfig: {
            path: apiService.node.getRules(),
            success: receiveNodeRules
        }
    };
};

export const REQUEST_NODEPRODUCT_RULES = 'REQUEST_NODEPRODUCT_RULES';
export const RECEIVE_NODEPRODUCT_RULES = 'RECEIVE_NODEPRODUCT_RULES';

export const receiveNodeProductRules = rules => {
    return {
        type: RECEIVE_NODEPRODUCT_RULES,
        rules
    };
};

export const requestNodeProductRules = () => {
    return {
        type: REQUEST_NODEPRODUCT_RULES,
        fetchConfig: {
            path: apiService.node.getProductRules(),
            success: receiveNodeProductRules
        }
    };
};

export const REQUEST_NODECONNECTIONPRODUCT_RULES = 'REQUEST_NODECONNECTIONPRODUCT_RULES';
export const RECEIVE_NODECONNECTIONPRODUCT_RULES = 'RECEIVE_NODECONNECTIONPRODUCT_RULES';

export const receiveNodeConnectionProductRules = rules => {
    return {
        type: RECEIVE_NODECONNECTIONPRODUCT_RULES,
        rules
    };
};

export const requestNodeConnectionProductRules = () => {
    return {
        type: REQUEST_NODECONNECTIONPRODUCT_RULES,
        fetchConfig: {
            path: apiService.nodeConnection.getProductRules(),
            success: receiveNodeConnectionProductRules
        }
    };
};

// reporting
const REQUEST_REPORT_CONFIG = 'REQUEST_REPORT_CONFIG';
export const RECEIVE_REPORT_CONFIG = 'RECEIVE_REPORT_CONFIG';

const receiveReportConfig = (key, data) => {
    return {
        type: RECEIVE_REPORT_CONFIG,
        key,
        data
    };
};

export const requestReportConfig = key => {
    return {
        type: REQUEST_REPORT_CONFIG,
        fetchConfig: {
            path: apiService.getReportConfig(key),
            success: json => receiveReportConfig(key, json)
        }
    };
};

const REQUEST_FILE_READACCESSINFO = 'REQUEST_FILE_READACCESSINFO';
export const RECEIVE_FILE_READACCESSINFO = 'RECEIVE_FILE_READACCESSINFO';

export const receiveFileReadAccessInfo = accessInfo => {
    return {
        type: RECEIVE_FILE_READACCESSINFO,
        accessInfo
    };
};

export const requestFileReadAccessInfo = () => {
    return {
        type: REQUEST_FILE_READACCESSINFO,
        fetchConfig: {
            path: apiService.fileUpload.getReadAccessInfo(),
            success: receiveFileReadAccessInfo
        }
    };
};

const REQUEST_FILE_READACCESSINFO_BY_CONTAINER = 'REQUEST_FILE_READACCESSINFO_BY_CONTAINER';
export const RECEIVE_FILE_READACCESSINFO_BY_CONTAINER = 'RECEIVE_FILE_READACCESSINFO_BY_CONTAINER';

export const receiveFileReadAccessInfoByContainer = (accessInfo, container) => {
    return {
        type: RECEIVE_FILE_READACCESSINFO_BY_CONTAINER,
        accessInfo,
        container
    };
};

export const requestFileReadAccessInfoByContainer = container => {
    return {
        type: REQUEST_FILE_READACCESSINFO_BY_CONTAINER,
        fetchConfig: {
            path: apiService.fileUpload.getReadAccessInfoByContainer(container),
            success: data => receiveFileReadAccessInfoByContainer(data, container)
        }
    };
};

export const REFRESH_ANY_REPORT_BY_KEY = 'REFRESH_ANY_REPORT_BY_KEY';
export const refreshAnyReport = reportKey => {
    return {
        type: REFRESH_ANY_REPORT_BY_KEY,
        key: reportKey
    };
};

// nodefilter

export const NODE_FILTER_ON_SELECT_CATEGORY = 'NODE_FILTER_ON_SELECT_CATEGORY';
export const nodeFilterOnSelectCategory = selectedCategory => {
    return {
        type: NODE_FILTER_ON_SELECT_CATEGORY,
        selectedCategory
    };
};

export const NODE_FILTER_ON_SELECT_ELEMENT = 'NODE_FILTER_ON_SELECT_ELEMENT';
export const nodeFilterOnSelectElement = selectedElement => {
    return {
        type: NODE_FILTER_ON_SELECT_ELEMENT,
        selectedElement
    };
};

export const NODE_FILTER_REQUEST_SEARCH_NODES = 'NODE_FILTER_REQUEST_SEARCH_NODES';
export const NODE_FILTER_RECEIVE_SEARCH_NODES = 'NODE_FILTER_RECEIVE_SEARCH_NODES';
export const NODE_FILTER_CLEAR_SEARCH_NODES = 'NODE_FILTER_CLEAR_SEARCH_NODES';

export const nodeFilterClearSearchNodes = () => {
    return {
        type: NODE_FILTER_CLEAR_SEARCH_NODES
    };
};

export const RECEIVE_DATE_RANGES = 'RECEIVE_DATE_RANGES';
const receiveDateRanges = data => {
    return {
        type: RECEIVE_DATE_RANGES,
        data
    };
};

export const REQUEST_DATE_RANGES = 'REQUEST_DATE_RANGES';
export const requestDateRanges = (elementId, years, isPerNodeReport) => {
    return {
        type: REQUEST_DATE_RANGES,
        fetchConfig: {
            path: apiService.ticket.requestDateRanges(elementId, years, isPerNodeReport),
            success: data => receiveDateRanges(data)
        }
    };
};

export const RESET_DATE_RANGES = 'RESET_DATE_RANGES';
export const resetDateRange = () => {
    return {
        type: RESET_DATE_RANGES
    };
};

export const ENABLE_DISABLE_VIEW_REPORT_BUTTON = 'ENABLE_DISABLE_VIEW_REPORT_BUTTON';
export const toggleViewReportButton = buttonStatus => {
    return {
        type: ENABLE_DISABLE_VIEW_REPORT_BUTTON,
        buttonStatus
    };
};

const nodeFilterReceiveSearchNodes = json => {
    const nodes = json.value ? json.value.map(x => x.node) : [];
    return {
        type: NODE_FILTER_RECEIVE_SEARCH_NODES,
        nodes
    };
};

export const nodeFilterRequestSearchNodes = (elementId, searchText) => {
    return {
        type: NODE_FILTER_REQUEST_SEARCH_NODES,
        fetchConfig: {
            path: apiService.node.searchNodeTags(elementId, searchText),
            success: nodeFilterReceiveSearchNodes,
            failure: nodeFilterClearSearchNodes
        }
    };
};

export const NODE_FILTER_RESET_FIELDS = 'NODE_FILTER_RESET_FIELDS';
export const nodeFilterResetFields = () => {
    return {
        type: NODE_FILTER_RESET_FIELDS
    };
};

export const SET_SELECTED_NODE = 'SET_SELECTED_NODE';
export const setSelectedNode = node => {
    let nodeId = 0;
    if (!utilities.isNullOrUndefined(node.nodeId)) {
        nodeId = node.nodeId;
    }
    return {
        type: SET_SELECTED_NODE,
        nodeId
    };
};

export const TOGGLE_COLOR_PICKER = 'TOGGLE_COLOR_PICKER';
export const SET_COLOR_PICKER = 'SET_COLOR_PICKER';
export const RESET_COLOR_PICKER = 'RESET_COLOR_PICKER';

export const toggleColorPicker = (name, isOpen) => {
    return {
        type: TOGGLE_COLOR_PICKER,
        name,
        isOpen
    };
};

export const setColorPicker = (name, color) => {
    return {
        type: SET_COLOR_PICKER,
        name,
        color
    };
};

export const resetColorPicker = name => {
    return {
        type: RESET_COLOR_PICKER,
        name
    };
};

export const SET_ICON_ID = 'SET_ICON_ID';
export const RESET_ICON_PICKER = 'RESET_ICON_PICKER';

export const setIconId = (id, name) => {
    return {
        type: SET_ICON_ID,
        id,
        name
    };
};

export const resetIconPicker = () => {
    return {
        type: RESET_ICON_PICKER
    };
};

// power automate
const REQUEST_FLOW_CONFIG = 'REQUEST_FLOW_CONFIG';
export const RECEIVE_FLOW_CONFIG = 'RECEIVE_FLOW_CONFIG';

export const receiveFlowConfig = (key, data) => {
    return {
        type: RECEIVE_FLOW_CONFIG,
        key,
        data
    };
};

export const requestFlowConfig = key => {
    return {
        type: REQUEST_FLOW_CONFIG,
        fetchConfig: {
            path: apiService.getFlowConfig(key),
            method: 'GET',
            success: json => receiveFlowConfig(key, json)
        }
    };
};

// ruleSynchronizer
export const INIT_SYNC_RULES = 'INIT_SYNC_RULES';

export const RECEIVE_RULES_SYNC_PROGRESS = 'RECEIVE_RULES_SYNC_PROGRESS';
export const REQUEST_RULES_SYNC_PROGRESS = 'REQUEST_RULES_SYNC_PROGRESS';

export const RECEIVE_SYNC_RULES = 'RECEIVE_SYNC_RULES';
export const REQUEST_SYNC_RULES = 'REQUEST_SYNC_RULES';

export const receiveSyncProgress = (name, status, init) => {
    return {
        type: RECEIVE_RULES_SYNC_PROGRESS,
        name,
        status,
        init
    };
};

export const initSyncRules = name => {
    return {
        type: INIT_SYNC_RULES,
        name,
        fetchConfig: {
            path: apiService.ownership.getSyncProgress(),
            success: status => receiveSyncProgress(name, status, true),
            showProgress: false
        }
    };
};

export const requestSyncProgress = name => {
    return {
        type: REQUEST_RULES_SYNC_PROGRESS,
        name,
        fetchConfig: {
            path: apiService.ownership.getSyncProgress(),
            success: status => receiveSyncProgress(name, status, false),
            showProgress: false
        }
    };
};

export const receiveSyncRules = (name, status) => {
    return {
        type: RECEIVE_SYNC_RULES,
        name,
        status
    };
};

export const syncRules = name => {
    return {
        type: REQUEST_SYNC_RULES,
        name,
        fetchConfig: {
            path: apiService.ownership.syncRules(),
            success: status => receiveSyncRules(name, status),
            showProgress: false
        }
    };
};

// Ownership Rules
const REQUEST_BULKUPDATE_RULES = 'BULK_UPDATE_RULES';
export const TOGGLE_BULK_UPDATE_POPUP = 'TOGGLE_BULK_UPDATE_POPUP';
export const RECEIVE_BULK_UPDATE_POPUP = 'RECEIVE_BULK_UPDATE_POPUP';

const receiveBulkUpdatePopUp = name => {
    return {
        type: RECEIVE_BULK_UPDATE_POPUP,
        name
    };
};
export const toggleUpdatePopUp = name => {
    return {
        type: TOGGLE_BULK_UPDATE_POPUP,
        name
    };
};
export const bulkUpdateRules = (rules, name) => {
    return {
        type: REQUEST_BULKUPDATE_RULES,
        fetchConfig: {
            path: apiService.node.bulkUpdateRules(),
            method: 'PUT',
            body: rules,
            success: () => receiveBulkUpdatePopUp(name)
        }
    };
};

export const TRIGGER_BULK_UPDATE_POPUP = 'TRIGGER_BULK_UPDATE_POPUP';
export const triggerPopup = (selection, name, path = 'ruleName', isContractualStrategy = false) => {
    return {
        type: TRIGGER_BULK_UPDATE_POPUP,
        selection,
        name,
        path,
        isContractualStrategy
    };
};

export const SET_VARIABLE_STATE = 'SET_VARIABLE_STATE';
export const setVariableState = (ownership, variables, name) => {
    return {
        type: SET_VARIABLE_STATE,
        ownership,
        variables,
        name
    };
};

export const REQUEST_TICKET_NODE_STATUS = 'REQUEST_TICKET_NODE_STATUS';
export const REPORT_REQUEST_COMPLETED = 'REPORT_REQUEST_COMPLETED';

const reportRequestCompleted = () => {
    return {
        type: REPORT_REQUEST_COMPLETED
    };
};

export const requestTicketNodeStatus = body => {
    return {
        type: REQUEST_TICKET_NODE_STATUS,
        fetchConfig: {
            path: apiService.ticket.postTicketNodeStatus(),
            body,
            success: reportRequestCompleted
        }
    };
};

export const REQUEST_EVENT_CONTRACT_REPORT = 'REQUEST_EVENT_CONTRACT_REPORT';

export const requestEventContractReport = body => {
    return {
        type: REQUEST_EVENT_CONTRACT_REPORT,
        fetchConfig: {
            path: apiService.ticket.postEventContractReportRequest(),
            body,
            success: reportRequestCompleted
        }
    };
};

export const REQUEST_NODE_CONFIGURATION_REPORT = 'REQUEST_NODE_CONFIGURATION_REPORT';

export const requestNodeConfigurationReport = body => {
    return {
        type: REQUEST_NODE_CONFIGURATION_REPORT,
        fetchConfig: {
            path: apiService.ticket.postNodeConfigurationReportRequest(),
            body,
            success: reportRequestCompleted
        }
    };
};
