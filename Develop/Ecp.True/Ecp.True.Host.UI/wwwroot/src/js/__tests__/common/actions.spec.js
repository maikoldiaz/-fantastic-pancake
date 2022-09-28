import * as actions from '../../common/actions';
import * as gridActions from '../../common/components/grid/actions';
import { apiService } from '../../common/services/apiService';
import { systemConfigService } from '../../common/services/systemConfigService';

it('should init Category ElementFilter', () => {
    const returnedData = actions.initCategoryElementFilter('initCategoryElementFilter');
    expect(returnedData.type).toEqual('INIT_CATEGORY_ELEMENT_FILTER');
    expect(returnedData.name).toEqual('initCategoryElementFilter');
});

it('should request/response category elements', () => {
    const returnedData = actions.requestCategoryElements('initCategoryElementFilter');

    expect(returnedData.type).toEqual('REQUEST_CATEGORY_ELEMENTS');
    expect(returnedData.fetchConfig).toBeDefined();
    expect(returnedData.fetchConfig.path).toEqual(apiService.getCategoryElements());
    expect(returnedData.fetchConfig.method).toEqual('GET');
    expect(returnedData.fetchConfig.success).toBeDefined();

    const responseJsonObj = {
        count: 1, value: [{
            elementId: 2, name: 'Area Sur', description: 'Area Sur', isActive: true, categoryId: 1, isAuditable: true,
            createdBy: 'System', createdDate: '2019-09-24T16:29:19.443Z', lastModifiedBy: null, lastModifiedDate: null, category: {
                categoryId: 1, name: 'Segment5454545 ededd', description: 'Segment',
                isActive: true, isGrouper: false, isAuditable: true, createdBy: 'System', createdDate: '2019-09-24T15:01:50.773Z',
                lastModifiedBy: 'System', lastModifiedDate: '2019-09-26T17:54:19.727Z'
            }
        }]
    };

    const receiveAction = returnedData.fetchConfig.success(responseJsonObj);

    expect(receiveAction.type).toEqual('RECEIVE_CATEGORY_ELEMENTS');
    expect(receiveAction.items).toEqual(responseJsonObj);
});


it('should get category elemnets of select category', () => {
    const item = {
        categoryId: 3, name: 'NodeType', description: 'NodeType', isActive: true, isGrouper: false,
        isAuditable: true, createdBy: 'System', createdDate: '2019-09-24T15:01:50.79ZY', lastModifiedBy: null, lastModifiedDate: null
    };
    const returnedData = actions.onCategorySelection(item, 0, 'categoryElementFilter');
    expect(returnedData.type).toEqual('CATEGORY_ELEMENT_FILTER_ON_SELECT_CATEGORY');
    expect(returnedData.item).toEqual(item);
    expect(returnedData.index).toEqual(0);
    expect(returnedData.name).toEqual('categoryElementFilter');
});

it('should save category elemnets filter', () => {
    const valuesJson = {
        categoryElements:
            [{
                category: {
                    categoryId: 2, name: 'Operator11111', description: 'Operatoraaa',
                    isActive: true, isGrouper: true, isAuditable: true, createdBy: 'System', createdDate: '2019-09-24T15:01:50.78Z',
                    lastModifiedBy: 'System', lastModifiedDate: '2019-09-26T19:05:11.683Z'
                },
                element: {
                    elementId: 7, name: 'ElementName_7', description: 'Element description test', isActive: true,
                    categoryId: 2, isAuditable: true, createdBy: 'System', createdDate: '2019-09-25T04:58:12.453Z',
                    lastModifiedBy: null, lastModifiedDate: null,
                    category: {
                        categoryId: 2, name: 'Operator11111', description: 'Operatoraaa',
                        isActive: true, isGrouper: true, isAuditable: true, createdBy: 'System', createdDate: '2019-09-24T15:01:50.78Z',
                        lastModifiedBy: 'System', lastModifiedDate: '2019-09-26T19:05:11.683Z'
                    }
                }
            }]
    };
    const returnedData = actions.saveCategoryElementFilter('saveCategoryElementFilter', valuesJson);
    expect(returnedData.type).toEqual('SAVE_CATEGORY_ELEMENT_FILTER');
    expect(returnedData.name).toEqual('saveCategoryElementFilter');
    expect(returnedData.values).toEqual(valuesJson);
});

it('should request/receive App Info', () => {
    const returnedData = actions.bootstrapApp();
    expect(returnedData.type).toEqual('BOOTSTRAP_APP');
    expect(returnedData.fetchConfig).toBeDefined();
    expect(returnedData.fetchConfig.path).toEqual(apiService.bootstrap());
    expect(returnedData.fetchConfig.success).toBeDefined();

    const responseJsonObj = {
        scenarios: [
            {
                scenarioId: 1,
                name: 'administration',
                sequence: 10,
                features: [
                    {
                        featureId: 1,
                        name: 'category',
                        sequence: 1010,
                        scenarioId: 1,
                        scenario: null,
                        createdBy: 'System',
                        createdDate: '2019-10-17T15:45:19.977'
                    }
                ],
                createdBy: 'System',
                createdDate: '2019-10-17T15:45:19.937'
            }
        ],
        context: {
            userId: 'xyz@ecopetrol.com.co',
            name: 'xyz'
        },
        image: null,
        systemConfig: {
            controlLimit: 1,
            standardUncertaintyPercentage: 40,
            acceptableBalancePercentage: 2
        }
    };

    const receiveAction = returnedData.fetchConfig.success(responseJsonObj);
    expect(receiveAction.type).toEqual('BOOTSTRAP_APP_SUCCESS');
    expect(receiveAction.data).toEqual(responseJsonObj);
});

it('should app ready', () => {
    const returnedData = actions.appReady();
    expect(returnedData.type).toEqual('APP_READY');
});

it('should toggle menu item', () => {
    const scenario = 'toggle';
    const returnedData = actions.toggleMenuItem(scenario);
    expect(returnedData.type).toEqual('APP_MENU_ITEM_TOGGLE');
    expect(returnedData.scenario).toEqual(scenario);
});

it('should toggle menu', () => {
    const returnedData = actions.toggleMenu();
    expect(returnedData.type).toEqual('APP_MENU_TOGGLE');
});

it('should add comment', () => {
    const comment = 'comment';
    const returnedData = actions.addComment(comment);
    expect(returnedData.type).toEqual('ADD_COMMENT');
    expect(returnedData.comment).toEqual(comment);
});

it('should init add comment', () => {
    const name = 'nodecomments';
    const message = 'est comment';
    const returnedData = actions.intAddComment(name, message);
    expect(returnedData.type).toEqual('INIT_ADD_COMMENT');
    expect(returnedData.name).toEqual(name);
    expect(returnedData.message).toEqual(message);
});

it('should show Loader', () => {
    const returnedData = actions.showLoader();
    expect(returnedData.type).toEqual('SHOW_LOADER');
});

it('should hide Loader', () => {
    const returnedData = actions.hideLoader();
    expect(returnedData.type).toEqual('HIDE_LOADER');
});

it('should open Modal', () => {
    const modalKey = 'modalKey';
    const mode = 'mode';
    const title = 'title';
    const returnedData = actions.openModal(modalKey, mode, title);
    expect(returnedData.type).toEqual('OPEN_MODAL');
    expect(returnedData.modalKey).toEqual(modalKey);
    expect(returnedData.title).toEqual(title);
    expect(returnedData.mode).toEqual(mode);
});

it('should close Modal', () => {
    const returnedData = actions.closeModal();
    expect(returnedData.type).toEqual('CLOSE_MODAL');
    expect(returnedData.displayWarning).toEqual(false);
});

it('should open Confirm', () => {
    const title = 'title';
    const message = 'est comment';
    const returnedData = actions.openConfirm(message, title);
    expect(returnedData.type).toEqual('OPEN_CONFIRM');
    expect(returnedData.title).toEqual(title);
    expect(returnedData.message).toEqual(message);
});

it('should close Confirm', () => {
    const returnedData = actions.closeConfirm();
    expect(returnedData.type).toEqual('CLOSE_CONFIRM');
});

it('should open Component Modal', () => {
    const component = null;
    const title = 'title';
    const mode = 'mode';
    const className = 'xyz';
    const titleClassName = 'abc';

    const returnedData = actions.openComponentModal(component, mode, title, className, titleClassName);
    expect(returnedData.type).toEqual('OPEN_COMPONENT_MODAL');
    expect(returnedData.component).toEqual(component);
    expect(returnedData.title).toEqual(title);
    expect(returnedData.mode).toEqual(mode);
    expect(returnedData.className).toEqual(className);
    expect(returnedData.titleClassName).toEqual(titleClassName);
});

it('should open Message Modal', () => {
    const message = 'message';
    const options = {};
    const returnedData = actions.openMessageModal(message, options);
    expect(returnedData.type).toEqual('OPEN_MESSAGE_ONLY_MODAL');
    expect(returnedData.message).toEqual(message);
    expect(returnedData.options).toEqual(options);
});

it('should init Flyout', () => {
    const name = 'search';
    const isOpen = false;
    const returnedData = actions.initFlyout(name, isOpen);
    expect(returnedData.type).toEqual('INIT_FLYOUT');
    expect(returnedData.name).toEqual(name);
    expect(returnedData.isOpen).toEqual(isOpen);
});

it('should open Flyout', () => {
    const name = 'search';
    const returnedData = actions.openFlyout(name);
    expect(returnedData.type).toEqual('OPEN_FLYOUT');
    expect(returnedData.name).toEqual(name);
});

it('should close Flyout', () => {
    const name = 'search';
    const returnedData = actions.closeFlyout(name);
    expect(returnedData.type).toEqual('CLOSE_FLYOUT');
    expect(returnedData.name).toEqual(name);
});


it('should Apply Filters', () => {
    const name = 'filter';
    const filterValues = {};
    const returnedData = gridActions.applyFilter(name, filterValues);
    expect(returnedData.type).toEqual('APPLY_FILTER');
    expect(returnedData.name).toEqual(name);
    expect(returnedData.filterValues).toEqual(filterValues);
});

it('should Reset Filters', () => {
    const name = 'filter';
    const value = '';
    const returnedData = gridActions.resetFilter(name, value);
    expect(returnedData.type).toEqual('RESET_FILTER');
    expect(returnedData.name).toEqual(name);
    expect(returnedData.value).toEqual(value);
});


it('should change Tab', () => {
    const returnedData = actions.changeTab('test', 1);
    expect(returnedData.type).toEqual('CHANGE_TAB');
    expect(returnedData.name).toEqual('test');
    expect(returnedData.activeTab).toEqual(1);
});

it('should configure Grid', () => {
    const config = {};
    const returnedData = gridActions.configureGrid(config);
    expect(returnedData.type).toEqual('INIT_GRID');
    expect(returnedData.config).toEqual(config);
});

it('should get selected category for node filter', () => {
    const NODE_FILTER_ON_SELECT_CATEGORY = 'NODE_FILTER_ON_SELECT_CATEGORY';
    const selectedCategory = 'category1';
    const action = actions.nodeFilterOnSelectCategory(selectedCategory);

    expect(action.type).toEqual(NODE_FILTER_ON_SELECT_CATEGORY);
    expect(action.selectedCategory).toEqual(selectedCategory);
});

it('should get selected element for node filter', () => {
    const NODE_FILTER_ON_SELECT_ELEMENT = 'NODE_FILTER_ON_SELECT_ELEMENT';
    const selectedElement = 'Campo';
    const action = actions.nodeFilterOnSelectElement(selectedElement);

    expect(action.type).toEqual(NODE_FILTER_ON_SELECT_ELEMENT);
    expect(action.selectedElement).toEqual(selectedElement);
});

it('should request search nodes for node filter', () => {
    const elementId = 1;
    const searchText = 'CASTILLA';
    const NODE_FILTER_REQUEST_SEARCH_NODES = 'NODE_FILTER_REQUEST_SEARCH_NODES';
    systemConfigService.getAutocompleteItemsCount = jest.fn(() => 5);
    const action = actions.nodeFilterRequestSearchNodes(elementId, searchText);

    expect(action.type).toEqual(NODE_FILTER_REQUEST_SEARCH_NODES);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.node.searchNodeTags(elementId, searchText));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const NODE_FILTER_RECEIVE_SEARCH_NODES = 'NODE_FILTER_RECEIVE_SEARCH_NODES';
    expect(receiveAction.type).toEqual(NODE_FILTER_RECEIVE_SEARCH_NODES);

    expect(action.fetchConfig.failure).toBeDefined();
    const receiveFailureAction = action.fetchConfig.failure(true);

    const NODE_FILTER_CLEAR_SEARCH_NODES = 'NODE_FILTER_CLEAR_SEARCH_NODES';
    expect(receiveFailureAction.type).toEqual(NODE_FILTER_CLEAR_SEARCH_NODES);
});

it('should clear search node for node filter', () => {
    const NODE_FILTER_CLEAR_SEARCH_NODES = 'NODE_FILTER_CLEAR_SEARCH_NODES';
    const action = actions.nodeFilterClearSearchNodes();

    expect(action.type).toEqual(NODE_FILTER_CLEAR_SEARCH_NODES);
});

it('should request Date Ranges', () => {
    const REQUEST_DATE_RANGES = 'REQUEST_DATE_RANGES';
    const action = actions.requestDateRanges(123, 23);

    expect(action.type).toEqual(REQUEST_DATE_RANGES);
});

it('should reset Date Ranges', () => {
    const RESET_DATE_RANGES = 'RESET_DATE_RANGES';
    const action = actions.resetDateRange();

    expect(action.type).toEqual(RESET_DATE_RANGES);
});

it('should toggle ViewReport button', () => {
    const ENABLE_DISABLE_VIEW_REPORT_BUTTON = 'ENABLE_DISABLE_VIEW_REPORT_BUTTON';
    const action = actions.toggleViewReportButton(true);

    expect(action.type).toEqual(ENABLE_DISABLE_VIEW_REPORT_BUTTON);
});


it('it should receiveSyncprogress', () => {
    const INIT_SYNC_RULES = 'INIT_SYNC_RULES';
    const action = actions.initSyncRules();
    expect(action.type).toEqual(INIT_SYNC_RULES);
});

it('it should request sync progress', () => {
    const REQUEST_RULES_SYNC_PROGRESS = 'REQUEST_RULES_SYNC_PROGRESS';
    const action = actions.requestSyncProgress();
    expect(action.type).toEqual(REQUEST_RULES_SYNC_PROGRESS);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.ownership.getSyncProgress());

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const RECEIVE_RULES_SYNC_PROGRESS = 'RECEIVE_RULES_SYNC_PROGRESS';
    expect(receiveAction.type).toEqual(RECEIVE_RULES_SYNC_PROGRESS);
});

it('it should receive Sync rules', () => {
    const RECEIVE_SYNC_RULES = 'RECEIVE_SYNC_RULES';
    const action = actions.receiveSyncRules();
    expect(action.type).toEqual(RECEIVE_SYNC_RULES);
});

it('it should request sync rules', () => {
    const REQUEST_SYNC_RULES = 'REQUEST_SYNC_RULES';
    const action = actions.syncRules();
    expect(action.type).toEqual(REQUEST_SYNC_RULES);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.ownership.syncRules());
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const RECEIVE_SYNC_RULES = 'RECEIVE_SYNC_RULES';
    expect(receiveAction.type).toEqual(RECEIVE_SYNC_RULES);
});

it('it should request ticket node status', () => {
    const REQUEST_TICKET_NODE_STATUS = 'REQUEST_TICKET_NODE_STATUS';
    const action = actions.requestTicketNodeStatus();
    expect(action.type).toEqual(REQUEST_TICKET_NODE_STATUS);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.ticket.postTicketNodeStatus());
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const REPORT_REQUEST_COMPLETED = 'REPORT_REQUEST_COMPLETED';
    expect(receiveAction.type).toEqual(REPORT_REQUEST_COMPLETED);
});

it('it should request event contract reports', () => {
    const REQUEST_EVENT_CONTRACT_REPORT = 'REQUEST_EVENT_CONTRACT_REPORT';
    const action = actions.requestEventContractReport();
    expect(action.type).toEqual(REQUEST_EVENT_CONTRACT_REPORT);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.ticket.postEventContractReportRequest());
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const REPORT_REQUEST_COMPLETED = 'REPORT_REQUEST_COMPLETED';
    expect(receiveAction.type).toEqual(REPORT_REQUEST_COMPLETED);
});

it('it should request bulk update rules', () => {
    const BULK_UPDATE_RULES = 'BULK_UPDATE_RULES';
    const action = actions.bulkUpdateRules();
    expect(action.type).toEqual(BULK_UPDATE_RULES);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.node.bulkUpdateRules());
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const RECEIVE_BULK_UPDATE_POPUP = 'RECEIVE_BULK_UPDATE_POPUP';
    expect(receiveAction.type).toEqual(RECEIVE_BULK_UPDATE_POPUP);
});

it('it should update the updateToggler TOGGLE_BULK_UPDATE_POPUP', () => {
    const TOGGLE_BULK_UPDATE_POPUP = 'TOGGLE_BULK_UPDATE_POPUP';
    const action = actions.toggleUpdatePopUp();
    expect(action.type).toEqual(TOGGLE_BULK_UPDATE_POPUP);
});

it('it should set variable state', () => {
    const SET_VARIABLE_STATE = 'SET_VARIABLE_STATE';
    const action = actions.setVariableState();
    expect(action.type).toEqual(SET_VARIABLE_STATE);
});

it('it should trigger popup', () => {
    const TRIGGER_BULK_UPDATE_POPUP = 'TRIGGER_BULK_UPDATE_POPUP';
    const action = actions.triggerPopup();
    expect(action.type).toEqual(TRIGGER_BULK_UPDATE_POPUP);
});

it('it should request flow config', () => {
    const REQUEST_FLOW_CONFIG = 'REQUEST_FLOW_CONFIG';
    const action = actions.requestFlowConfig();
    expect(action.type).toEqual(REQUEST_FLOW_CONFIG);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.getFlowConfig());
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const RECEIVE_FLOW_CONFIG = 'RECEIVE_FLOW_CONFIG';
    expect(receiveAction.type).toEqual(RECEIVE_FLOW_CONFIG);
});

it('it should reset icon picker', () => {
    const RESET_ICON_PICKER = 'RESET_ICON_PICKER';
    const action = actions.resetIconPicker();
    expect(action.type).toEqual(RESET_ICON_PICKER);
});

it('it should set icon id', () => {
    const SET_ICON_ID = 'SET_ICON_ID';
    const action = actions.setIconId();
    expect(action.type).toEqual(SET_ICON_ID);
});

it('it should reset color picker', () => {
    const RESET_COLOR_PICKER = 'RESET_COLOR_PICKER';
    const action = actions.resetColorPicker();
    expect(action.type).toEqual(RESET_COLOR_PICKER);
});

it('it should set color picker', () => {
    const SET_COLOR_PICKER = 'SET_COLOR_PICKER';
    const action = actions.setColorPicker();
    expect(action.type).toEqual(SET_COLOR_PICKER);
});

it('it should toggle color picker', () => {
    const TOGGLE_COLOR_PICKER = 'TOGGLE_COLOR_PICKER';
    const action = actions.toggleColorPicker();
    expect(action.type).toEqual(TOGGLE_COLOR_PICKER);
});

it('it should request users', () => {
    const REQUEST_USERS = 'REQUEST_USERS';
    const action = actions.requestUsers();
    expect(action.type).toEqual(REQUEST_USERS);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.getUsers());
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const RECEIVE_USERS = 'RECEIVE_USERS';
    expect(receiveAction.type).toEqual(RECEIVE_USERS);
});

it('it should request storage locations', () => {
    const REQUEST_STORAGE_LOCATIONS = 'REQUEST_STORAGE_LOCATIONS';
    const action = actions.requestStorageLocations();
    expect(action.type).toEqual(REQUEST_STORAGE_LOCATIONS);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.getStorageLocations());
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const RECEIVE_STORAGE_LOCATIONS = 'RECEIVE_STORAGE_LOCATIONS';
    expect(receiveAction.type).toEqual(RECEIVE_STORAGE_LOCATIONS);
});

it('it should request logistic centers', () => {
    const REQUEST_LOGISTIC_CENTERS = 'REQUEST_LOGISTIC_CENTERS';
    const action = actions.requestLogisticCenters();
    expect(action.type).toEqual(REQUEST_LOGISTIC_CENTERS);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.getLogisticCenters());
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const RECEIVE_LOGISTIC_CENTERS = 'RECEIVE_LOGISTIC_CENTERS';
    expect(receiveAction.type).toEqual(RECEIVE_LOGISTIC_CENTERS);
});

it('it should get logistic centers', () => {
    const GET_LOGISTIC_CENTERS = 'GET_LOGISTIC_CENTERS';
    const action = actions.getLogisticCenters();
    expect(action.type).toEqual(GET_LOGISTIC_CENTERS);
});

it('it should show hide page action', () => {
    const SHOW_HIDE_PAGE_ACTION = 'SHOW_HIDE_PAGE_ACTION';
    const action = actions.hidePageAction();
    expect(action.type).toEqual(SHOW_HIDE_PAGE_ACTION);
});

it('it should show page action', () => {
    const SHOW_HIDE_PAGE_ACTION = 'SHOW_HIDE_PAGE_ACTION';
    const action = actions.showPageAction();
    expect(action.type).toEqual(SHOW_HIDE_PAGE_ACTION);
});

it('it should hide page action', () => {
    const HIDE_PAGE_ACTIONS = 'HIDE_PAGE_ACTIONS';
    const action = actions.hidePageActions();
    expect(action.type).toEqual(HIDE_PAGE_ACTIONS);
});

it('it should enable disable page action', () => {
    const ENABLE_DISABLE_PAGE_ACTION = 'ENABLE_DISABLE_PAGE_ACTION';
    const action = actions.enableDisablePageAction();
    expect(action.type).toEqual(ENABLE_DISABLE_PAGE_ACTION);
});

it('it should disable page action', () => {
    const DISABLE_PAGE_ACTIONS = 'DISABLE_PAGE_ACTIONS';
    const action = actions.disablePageActions();
    expect(action.type).toEqual(DISABLE_PAGE_ACTIONS);
});

it('it should toggle page action', () => {
    const TOGGLE_PAGE_ACTIONS = 'TOGGLE_PAGE_ACTIONS';
    const action = actions.togglePageActions();
    expect(action.type).toEqual(TOGGLE_PAGE_ACTIONS);
});


it('should request/receive Variable Types', () => {
    const returnedData = actions.requestVariableTypes();
    expect(returnedData.type).toEqual('REQUEST_VARIABLE_TYPES');
    expect(returnedData.fetchConfig).toBeDefined();
    expect(returnedData.fetchConfig.path).toEqual(apiService.getVariableTypes());
    expect(returnedData.fetchConfig.success).toBeDefined();

    const responseJsonObj = {

    };

    const receiveAction = returnedData.fetchConfig.success(responseJsonObj);
    expect(receiveAction.type).toEqual('RECEIVE_VARIABLE_TYPES');
    expect(receiveAction.items).toEqual(responseJsonObj);
});

it('should request/receive System Types', () => {
    const returnedData = actions.requestSystemTypes();
    expect(returnedData.type).toEqual('REQUEST_SYSTEM_TYPES');
    expect(returnedData.fetchConfig).toBeDefined();
    expect(returnedData.fetchConfig.path).toEqual(apiService.getSystemTypes());
    expect(returnedData.fetchConfig.success).toBeDefined();

    const responseJsonObj = {
        value: {}
    };

    const receiveAction = returnedData.fetchConfig.success(responseJsonObj);
    expect(receiveAction.type).toEqual('RECEIVE_SYSTEM_TYPES');
    expect(receiveAction.items).toEqual(responseJsonObj.value);
});

it('should init Wizard', () => {
    const name = 'wizard';
    const activeStep = 1;
    const totalSteps = 3;
    const returnedData = actions.initWizard(name, activeStep, totalSteps);
    expect(returnedData.type).toEqual('INIT_WIZARD');
    expect(returnedData.name).toEqual(name);
    expect(returnedData.activeStep).toEqual(activeStep);
    expect(returnedData.totalSteps).toEqual(totalSteps);
});

it('should set Wizard Step', () => {
    const name = 'wizard';
    const activeStep = 1;
    const returnedData = actions.wizardSetStep(name, activeStep);
    expect(returnedData.type).toEqual('WIZARD_SET_STEP');
    expect(returnedData.name).toEqual(name);
    expect(returnedData.activeStep).toEqual(activeStep);
});

it('should set Wizard Next Step', () => {
    const name = 'wizard';
    const returnedData = actions.wizardNextStep(name);
    expect(returnedData.type).toEqual('WIZARD_NEXT_STEP');
    expect(returnedData.name).toEqual(name);
});

it('should set Wizard Prev Step', () => {
    const name = 'wizard';
    const returnedData = actions.wizardPrevStep(name);
    expect(returnedData.type).toEqual('WIZARD_PREV_STEP');
    expect(returnedData.name).toEqual(name);
});

it('should reset page index', () => {
    const returnedData = gridActions.resetPageIndex('gridName');
    expect(returnedData.type).toEqual('RESET_PAGE_INDEX');
    expect(returnedData.name).toEqual('gridName');
});
