import {
    shared, root, modal, confirmModal, loader, tabs, notification, notificationButton, pageActions, addComment, flyout, wizard,
    dualSelect, nodeFilter, ownershipRules, categoryElementFilter
} from '../../common/reducers';
import { grid } from '../../common/components/grid/reducers';
import { utilities } from '../../common/services/utilities';

function buildTogglerState(state, action, property) {
    return state.progressStatus[property] === 0 || action.forceFetch ? !state[`${property}Toggler`] : state[`${property}Toggler`];
}

describe('Reducers for shared Component',
    function () {
        it('should handle action INIT_CATEGORY_ELEMENT_FILTER',
            function () {
                const initialState = {
                    defaultValues: {
                        categoryElements: [
                            {
                                category: null, element: null
                            }
                        ]
                    }
                };
                const actionName = 'testCategoryFilter';
                const action = {
                    type: 'INIT_CATEGORY_ELEMENT_FILTER',
                    name: actionName
                };
                const expectedState = Object.assign(initialState, { testCategoryFilter: { selectedCategories: {}, values: initialState.defaultValues } });
                const returnedState = categoryElementFilter(initialState, action);
                expect(returnedState).toEqual(expectedState);
            });
        it('should handle action CATEGORY_ELEMENTS_FILTER_RESET_FIELDS',
            function () {
                const initialState = {
                    defaultValues: {
                        categoryElements: [
                            {
                                category: null, element: null
                            }
                        ]
                    }
                };
                const actionName = 'testCategoryFilter';
                const action = {
                    type: 'CATEGORY_ELEMENTS_FILTER_RESET_FIELDS',
                    name: actionName
                };
                const expectedState = Object.assign(initialState, { testCategoryFilter: { selectedCategories: [], values: initialState.defaultValues } });
                const returnedState = categoryElementFilter(initialState, action);
                expect(returnedState).toEqual(expectedState);
            });
        it('should handle action REQUEST_CATEGORY_ELEMENTS',
            function () {
                const sharedInitialState = {
                    categories: [],
                    categoryElements: [],
                    progressStatus: {
                        categoryElements: 0
                    }
                };
                const actionName = 'testCategoryFilter';
                const action = {
                    type: 'REQUEST_CATEGORY_ELEMENTS',
                    name: actionName
                };
                const expectedState = Object.assign(sharedInitialState, { progressStatus: utilities.changeState(sharedInitialState.progressStatus, 'categoryElements', 1) });
                const returnedState = shared(sharedInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });
        it('should handle action RECEIVE_CATEGORY_ELEMENTS',
            function () {
                const initialState = {
                    categories: [],
                    categoryElements: [],
                    groupedCategoryElements: [],
                    progressStatus: {
                        categoryElements: 0
                    }
                };
                const categories = [{ categoryId: 1, name: 'Segment5454545 ededd' }];
                const progStatus = { categoryElements: 0 };
                const progressStatus = utilities.changeState(progStatus, 'categoryElements', 2);
                const categoryElements = [
                    {
                        elementId: 2,
                        name: 'Area Sur',
                        categoryId: 1,
                        category: {
                            categoryId: 1,
                            name: 'Segment5454545 ededd'
                        }
                    },
                    {
                        elementId: 3,
                        name: 'ElementName_3',
                        categoryId: 2,
                        category: {
                            categoryId: 1,
                            name: 'Segment5454545 ededd'
                        }
                    }
                ];
                const grouped = utilities.normalizedGroupBy(categoryElements, 'categoryId');
                const action = {
                    type: 'RECEIVE_CATEGORY_ELEMENTS',
                    items: {
                        value: categoryElements
                    }
                };
                const expectedState = Object.assign({}, initialState, {
                    categories, categoryElements, progressStatus,
                    groupedCategoryElements: grouped, operationalSegments: [], categoryElementsDataToggler: true
                });
                const processedState = shared(initialState, action);
                expect(processedState).toEqual(expectedState);
            });
        it('should receive logistic centers', () => {
            const initialState = {
                progressStatus: null
            };
            const action = {
                type: 'RECEIVE_LOGISTIC_CENTERS',
                data: { someData: 'testData' }
            };
            const newState = Object.assign({},
                initialState,
                {
                    logisticCenters: action.data,
                    logisticCentersDataToggler: true,
                    progressStatus: { logisticCenters: 2 }
                });
            expect(shared(initialState, action)).toEqual(newState);
        });
        it('should receive storage locations', () => {
            const initialState = {
                storageLocations: null
            };
            const action = {
                type: 'RECEIVE_STORAGE_LOCATIONS',
                data: { someData: 'testData' }
            };
            const newState = Object.assign({},
                initialState,
                {
                    storageLocations: action.data,
                    storageLocationsDataToggler: true,
                    progressStatus: { storageLocations: 2 }
                });
            expect(shared(initialState, action)).toEqual(newState);
        });
        it('should get storage locations', () => {
            const initialState = {
                progressStatus: { storageLocations: 'testLocation' }
            };
            const action = {
                type: 'GET_STORAGE_LOCATIONS',
                storageLocations: 1,
                storageLocationsToggler: false
            };
            const newState = Object.assign({},
                initialState,
                {
                    storageLocationsToggler: buildTogglerState(initialState, action, 'storageLocations')
                });
            expect(shared(initialState, action)).toEqual(newState);
        });
        it('should handle action CATEGORY_ELEMENT_FILTER_ON_SELECT_CATEGORY',
            function () {
                const categoryElementFilterInitialState = {
                    defaultValues: {
                        categoryElements: [{ category: null, element: null }]
                    }, testCategoryFilter: { selectedCategories: {}, values: { categoryElements: [{ category: null, element: null }] } }
                };
                const actionName = 'testCategoryFilter';
                const action = {
                    type: 'CATEGORY_ELEMENT_FILTER_ON_SELECT_CATEGORY',
                    name: actionName,
                    index: 0,
                    item: { categoryId: 1, name: 'Segment5454545 ededd' }
                };
                const filterState = Object.assign(categoryElementFilterInitialState[action.name], {
                    selectedCategories: Object.assign({}, categoryElementFilterInitialState[action.name].selectedCategories, {
                        [action.index]: action.item
                    })
                });
                const expectedState = {
                    defaultValues: {
                        categoryElements: [{ category: null, element: null }]
                    },
                    testCategoryFilter: filterState
                };

                const returnedState = categoryElementFilter(categoryElementFilterInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });
        it('should handle action SAVE_CATEGORY_ELEMENT_FILTER',
            function () {
                const categoryElementFilterInitialState = {
                    defaultValues: {
                        categoryElements: [
                            {
                                category: null, element: null
                            }
                        ]
                    }
                };
                const actionName = 'saveCategoryFilter';
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
                const action = {
                    type: 'SAVE_CATEGORY_ELEMENT_FILTER',
                    name: actionName,
                    values: valuesJson
                };
                const filterState = Object.assign({}, categoryElementFilterInitialState[action.name], {
                    values: Object.assign({}, action.values)
                });
                const expectedState = Object.assign({}, categoryElementFilterInitialState, { saveCategoryFilter: filterState });
                const returnedState = categoryElementFilter(categoryElementFilterInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });
        it('should handle action SAVE_UPLOADFILE_FILTER',
            function () {
                const categoryElementFilterInitialState = {
                    defaultValues: {
                        categoryElements: [
                            {
                                category: null, element: null
                            }
                        ]
                    }
                };
                const actionName = 'saveCategoryFilter';
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
                const action = {
                    type: 'SAVE_UPLOADFILE_FILTER',
                    name: actionName,
                    values: valuesJson
                };
                const filterState = Object.assign({}, categoryElementFilterInitialState[action.name], {
                    values: Object.assign({}, action.values)
                });
                const expectedState = Object.assign({}, categoryElementFilterInitialState, { saveCategoryFilter: filterState });
                const returnedState = categoryElementFilter(categoryElementFilterInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });
        it('should handle action BOOTSTRAP_APP_SUCCESS',
            function () {
                const sharedInitialState = { context: {}, scenarios: {} };
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
                                    description: 'categoría',
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
                        name: 'xyz',
                        image: null
                    },
                    systemConfig: {
                        controlLimit: 1,
                        standardUncertaintyPercentage: 40,
                        acceptableBalancePercentage: 2
                    },
                    instrumentationKey: '5c64a112-1b57-402f-9d37-474f0000a192'
                };
                const action = {
                    type: 'BOOTSTRAP_APP_SUCCESS',
                    data: responseJsonObj
                };
                const expectedState = {
                    context: action.data.context,
                    scenarios: {
                        administration:
                        {
                            scenario: 'administration',
                            features: [{
                                name: 'category',
                                description: 'categoría'
                            }],
                            isCollapsible: true,
                            isCollapsed: true
                        }
                    },
                    appToggler: true,
                    systemConfig: action.data.systemConfig
                };
                const returnedState = root(sharedInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });
        it('should set bootstrap failed.', () => {
            const initialState = {
                error: false
            };
            const action = {
                type: 'BOOTSTRAP_APP_FAILED'
            };
            const expectedState = Object.assign({}, initialState, { error: true });
            const newState = root(initialState, action);
            expect(expectedState).toEqual(newState);
        });
        it('should handle action APP_READY',
            function () {
                const sharedInitialState = { context: {}, scenarios: {}, appReady: true, appReadyToggler: true };
                const action = {
                    type: 'APP_READY'
                };
                const expectedState = { context: {}, scenarios: {}, appReady: true, appReadyToggler: false };
                const returnedState = root(sharedInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });
        it('should handle action APP_MENU_ITEM_TOGGLE',
            function () {
                const sharedInitialState = { context: {}, scenarios: { scenario: { isCollapsed: false } } };
                const action = {
                    type: 'APP_MENU_ITEM_TOGGLE',
                    scenario: 'scenario'
                };
                const returnedState = root(sharedInitialState, action);

                const expectedState = Object.assign(sharedInitialState, {
                    scenarios: { scenario: { isCollapsed: true } }, isOpen: true
                });

                expect(returnedState).toEqual(expectedState);
            });
        it('should handle action APP_MENU_TOGGLE',
            function () {
                const currentScenario = 'scenario';
                const sharedInitialState = { context: {}, scenarios: { scenario: { isCollapsed: false } } };
                const action = {
                    type: 'APP_MENU_TOGGLE',
                    currentScenario
                };
                const returnedState = root(sharedInitialState, action);
                const expectedState = Object.assign(sharedInitialState, {
                    scenarios: { scenario: { isCollapsed: true } }, isOpen: true
                });
                expect(returnedState).toEqual(expectedState);
            });
        it('should set current context.', () => {
            const initialState = {
                currentModule: null
            };
            const action = {
                type: 'APP_MENU_CURRENT_MODULE',
                moduleName: 'CurrentModule'
            };
            const expectedState = Object.assign({}, initialState, { currentModule: action.moduleName });
            const newState = root(initialState, action);
            expect(expectedState).toEqual(newState);
        });
        it('should handle Default action for Root reducer',
            function () {
                const action = {
                    type: 'Default'
                };

                const sharedInitialState = { context: {}, scenarios: {}, isOpen: true };
                const returnedState = root(sharedInitialState, action);
                expect(returnedState).toEqual(sharedInitialState);
            });

        it('should handle action CLOSE_MODAL',
            function () {
                const initialState = {};
                const action = {
                    type: 'CLOSE_MODAL'
                };
                const expectedState = Object.assign(initialState, {
                    isOpen: false
                });

                const returnedState = modal(initialState, action);
                expect(returnedState).toEqual(expectedState);
            });

        it('should handle action OPEN_MODAL',
            function () {
                const initialState = {};
                const action = {
                    type: 'OPEN_MODAL',
                    modalKey: '1',
                    className: 'className',
                    title: 'open modal',
                    mode: 'open'
                };
                const expectedState = Object.assign(initialState, {
                    isOpen: true,
                    modalKey: '1',
                    title: 'open modal',
                    messageOnly: false,
                    className: 'className',
                    mode: 'open'
                });

                const returnedState = modal(initialState, action);
                expect(returnedState).toEqual(expectedState);
            });

        it('should handle default action for modal reducer',
            function () {
                const initialState = {};
                const action = {
                    type: 'default'
                };

                const returnedState = modal(initialState, action);
                expect(returnedState).toEqual(initialState);
            });

        it('should handle default action for message modal reducer',
            function () {
                const initialState = {
                    messageOnly: true,
                    isOpen: true
                };
                const action = {
                    type: 'OPEN_MESSAGE_ONLY_MODAL',
                    message: 'test message',
                    options: {
                        title: null,
                        className: null,
                        bodyClassName: null,
                        acceptActionTitle: 'accept',
                        canCancel: true,
                        cancelAction: null,
                        acceptAction: null,
                        acceptActionAndClose: null,
                        cancelActionTitle: 'cancel',
                        closeAction: null,
                        titleClassName: undefined
                    }
                };
                const expectedState = Object.assign({},
                    initialState,
                    {
                        message: 'test message',
                        title: 'confirmation',
                        className: 'ep-modal ep-modal--confirm',
                        bodyClassName: 'ep-modal__body',
                        acceptActionTitle: 'accept',
                        canCancel: true,
                        cancelAction: null,
                        acceptAction: null,
                        closeAction: null,
                        acceptActionAndClose: null,
                        cancelActionTitle: 'cancel'
                    });
                const newState = modal(initialState, action);
                expect(expectedState).toEqual(newState);
            });

        it('should handle action OPEN_CONFIRM',
            function () {
                const confirmModalInitialState = {
                    title: 'confirmation',
                    isOpen: false,
                    message: null,
                    data: null
                };

                const action = {
                    type: 'OPEN_CONFIRM',
                    message: 'message',
                    title: 'title',
                    data: null
                };
                const expectedState = Object.assign(confirmModalInitialState, {
                    isOpen: true,
                    message: action.message,
                    title: action.title,
                    data: null
                });

                const returnedState = confirmModal(confirmModalInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });

        it('should handle action CLOSE_CONFIRM',
            function () {
                const confirmModalInitialState = {
                    title: 'confirmation',
                    isOpen: false,
                    message: null,
                    data: null
                };
                const action = {
                    type: 'CLOSE_CONFIRM'
                };
                const expectedState = confirmModalInitialState;
                const returnedState = confirmModal(confirmModalInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });

        it('should handle default action for confirm modal.',
            function () {
                const confirmModalInitialState = {
                    title: 'confirmation',
                    isOpen: false,
                    message: null,
                    data: null
                };
                const action = {
                    type: 'DEFAULT'
                };
                const returnedState = confirmModal(confirmModalInitialState, action);
                expect(returnedState).toEqual(confirmModalInitialState);
            });

        it('should handle action SHOW_LOADER',
            function () {
                const loaderInitialState = {
                    counter: 0
                };
                const action = {
                    type: 'SHOW_LOADER'
                };
                const expectedState = Object.assign({}, loaderInitialState, {
                    counter: 1
                });
                const returnedState = loader(loaderInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });

        it('should handle action HIDE_LOADER',
            function () {
                const loaderInitialState = {
                    counter: 1
                };
                const action = {
                    type: 'HIDE_LOADER'
                };
                const expectedState = Object.assign({}, loaderInitialState, {
                    counter: 0
                });
                const returnedState = loader(loaderInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });

        it('should handle default action for Loader',
            function () {
                const loaderInitialState = {
                    counter: 0
                };
                const action = {
                    type: 'DEFAULT'
                };
                const expectedState = Object.assign(loaderInitialState, {
                    counter: 0
                });
                const returnedState = loader(loaderInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });

        it('should handle action INIT_TAB',
            function () {
                const action = {
                    type: 'INIT_TAB',
                    name: 'test',
                    activeTab: 2
                };
                const expectedState = { test: { activeTab: 2 } };
                const returnedState = tabs({ test: {} }, action);
                expect(returnedState).toEqual(expectedState);
            });

        it('should handle action CHANGE_TAB',
            function () {
                const action = {
                    type: 'CHANGE_TAB',
                    name: 'test',
                    activeTab: 2
                };
                const expectedState = { test: { activeTab: 2 } };
                const returnedState = tabs({ test: {} }, action);
                expect(returnedState).toEqual(expectedState);
            });

        it('should handle default action for tab',
            function () {
                const action = {
                    type: 'DEFAULT'
                };
                const returnedState = tabs({}, action);
                expect(returnedState).toEqual({});
            });

        it('should handle action SHOW_NOTIFICATION',
            function () {
                const notificationInitialState = { isShow: false };
                const action = {
                    type: 'SHOW_NOTIFICATION',
                    state: 'show',
                    message: 'abc',
                    show: true
                };
                const expectedState = Object.assign(notificationInitialState, {
                    state: 'show',
                    message: 'abc',
                    show: true
                });
                const returnedState = notification(notificationInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });


        it('should handle action HIDE_NOTIFICATION',
            function () {
                const notificationInitialState = { isShow: false };
                const action = {
                    type: 'HIDE_NOTIFICATION'
                };
                const expectedState = Object.assign(notificationInitialState, {
                    message: '',
                    show: false
                });
                const returnedState = notification(notificationInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });

        it('should handle INVOKE_NOTIFICATION_BUTTON_CLICK',
            function () {
                const notificationInitialState = { invokeNotificationButtonToggler: false };
                const action = {
                    type: 'INVOKE_NOTIFICATION_BUTTON_CLICK',
                    invokeNotificationButtonToggler: false
                };
                const expectedState = Object.assign({},
                    notificationInitialState,
                    {
                        invokeNotificationButtonToggler: true
                    });
                const returnedState = notificationButton(notificationInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });

        it('should handle action TOGGLE_PAGE_ACTIONS',
            function () {
                const pageActionsInitialState = {
                    disabledActions: []
                };
                const action = {
                    type: 'TOGGLE_PAGE_ACTIONS',
                    enabled: true
                };
                const expectedState = Object.assign(pageActionsInitialState, {
                    enabled: true
                });
                const returnedState = pageActions(pageActionsInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });

        it('should handle action DISABLE_PAGE_ACTIONS',
            function () {
                const pageActionsInitialState = {
                    disabledActions: []
                };
                const action = {
                    type: 'DISABLE_PAGE_ACTIONS',
                    disabledActions: []
                };
                const expectedState = pageActionsInitialState;
                const returnedState = pageActions(pageActionsInitialState, action);
                expect(returnedState).toEqual(expectedState);
            });

        it('addComment- should handle add comment action for addComment reducer', () => {
            const state = { name: 'actionName', actionName: { comment: 'comment', commentToggler: true } };

            const action = {
                type: 'ADD_COMMENT',
                name: 'actionName',
                comment: 'comment',
                commentToggler: false
            };
            const returnedState = addComment(state, action);
            const expectedState = Object.assign({}, state, { actionName: { comment: 'comment', commentToggler: false } });
            expect(returnedState).toEqual(expectedState);
        });


        it('addComment- should handle initialize add comment action for addComment reducer', () => {
            const state = { name: 'stateName', stateName: { comment: '', message: 'message', commentToggler: false } };
            const action = {
                type: 'INIT_ADD_COMMENT',
                name: 'stateName',
                message: 'message'
            };
            const returnedState = addComment(state, action);
            expect(returnedState).toEqual(state);
        });

        it('addComment- should handle default action for addComment reducer', () => {
            const confirmModalInitialState = {};
            const action = {
                type: 'DEFAULT'
            };
            const returnedState = addComment(confirmModalInitialState, action);
            expect(returnedState).toEqual(confirmModalInitialState);
        });

        it('INIT_GRID- Should handle grid initialization', () => {
            const state = {};
            const action = {
                type: 'INIT_GRID', config: { name: 'configName', resume: false, filterable: { initialValues: {} } }
            };
            const returnedState = grid(state, action);
            const expectedState = {
                configName: {
                    config: { name: 'configName', resume: false, filterable: { initialValues: {} } },
                    filterValues: {}, items: [], pageItems: [], selectAll: false, selection: [], totalItems: 0, pageFilters: {}
                }
            };
            expect(returnedState).toEqual(expectedState);
        });

        it('INIT_GRID- Should handle grid initialization for resumed state', () => {
            const state = { configName: {} };
            const action = {
                type: 'INIT_GRID', config: { name: 'configName', resume: true }
            };
            const returnedState = grid(state, action);

            expect(returnedState).toEqual(state);
        });

        it('UPDATE_GRID_PAGE_ITEMS- Should update grid page items', () => {
            const initialState = { refresh: { timeOut: 50 } };
            const action = {
                type: 'UPDATE_GRID_PAGE_ITEMS',
                name: 'refresh',
                pageItems: 100
            };
            const expectedState = { refresh: { timeOut: 50, pageItems: 100 } };
            const returnedState = grid(initialState, action);
            expect(returnedState).toEqual(expectedState);
        });

        it('RECEIVE_GRID_DATA', () => {
            const state = { actionName: { selectAll: false, receiveDataToggler: true }, configName: {} };
            const action = {
                type: 'RECEIVE_GRID_DATA',
                name: 'actionName',
                totalItems: 0,
                items: [],
                config: { name: 'configName', resume: true }
            };
            const returnedState = grid(state, action);
            const expectedState = {
                actionName: {
                    receiveDataToggler: false,
                    items: [],
                    selectAll: false,
                    selection: [],
                    totalItems: 0
                },
                configName: {}
            };
            expect(returnedState).toEqual(expectedState);
        });

        it('REFRESH_GRID_DATA', () => {
            const state = { actionName: { selectAll: false, refreshToggler: true }, configName: {} };
            const action = {
                type: 'REFRESH_GRID_DATA',
                name: 'actionName',
                totalItems: 0,
                items: [],
                config: { name: 'configName', resume: true }
            };
            const returnedState = grid(state, action);
            const expectedState = {
                actionName: { selectAll: false, refreshToggler: false },
                configName: {}
            };
            expect(returnedState).toEqual(expectedState);
        });

        it('REFRESH_SILENT_GRID_DATA', () => {
            const state = { actionName: { selectAll: false, refreshSilentToggler: true }, configName: {} };
            const action = {
                type: 'REFRESH_SILENT_GRID_DATA',
                name: 'actionName',
                totalItems: 0,
                items: [],
                config: { name: 'configName', resume: true }
            };
            const returnedState = grid(state, action);
            const expectedState = {
                actionName: { selectAll: false, refreshSilentToggler: false },
                configName: {}
            };
            expect(returnedState).toEqual(expectedState);
        });

        it('CLEAR_GRID_SELECTION- Should clear grid selection', () => {
            const initialState = { testAction: { testActionToggler: false } };
            const action = {
                type: 'CLEAR_GRID_SELECTION',
                name: 'testAction',
                toggler: 'testAction'
            };
            const expectedState = { testAction: { selection: [], selectAll: false, testActionToggler: true } };
            const returnedState = grid(initialState, action);
            expect(returnedState).toEqual(expectedState);
        });

        it('APPLY_GRID_PAGE_FILTER- Should apply grid page filter', () => {
            const initialState = { testAction: { pageFilters: ['currentFilter1', 'currentFilter2'], refreshToggler: false } };
            const action = {
                type: 'APPLY_GRID_PAGE_FILTER',
                name: 'testAction',
                pageFilters: ['newFilter1', 'newFilter2']
            };
            const expectedState = { testAction: { pageFilters: { 0: 'newFilter1', 1: 'newFilter2' }, refreshToggler: true } };
            const returnedState = grid(initialState, action);
            expect(returnedState).toEqual(expectedState);
        });

        it('SELECT_GRID_DATA', () => {
            const state = {
                name: 'stateName', stateName: {
                    items: [{ id: '1' }, { id: '2' }],
                    selection: [],
                    selectAll: false,
                    config: { idField: 'id' }
                }, configName: {}
            };

            const action = {
                type: 'SELECT_GRID_DATA',
                name: 'stateName',
                key: '1'
            };
            const returnedState = grid(state, action);
            const expectedState = { ...state, stateName: { ...state.stateName, selection: [{ id: '1' }] } };
            expect(returnedState).toEqual(expectedState);
        });

        it('SELECT_GRID_DATA- handle selection', () => {
            const state = {
                name: 'stateName', stateName: {
                    items: [{ id: '1' }, { id: '2' }],
                    selection: [{ id: '1' }],
                    selectAll: false,
                    config: { idField: 'id' }
                }, configName: {}
            };

            const action = {
                type: 'SELECT_GRID_DATA',
                name: 'stateName',
                key: '1'
            };
            const returnedState = grid(state, action);
            const expectedState = { ...state, stateName: { ...state.stateName, selection: [] } };
            expect(returnedState).toEqual(expectedState);
        });

        it('SELECT_ALL_GRID_DATA', () => {
            const state = {
                name: 'stateName', stateName: {
                    items: [{ id: '1' }, { id: '2' }],
                    selectAll: true,
                    config: {}
                }, config: {}
            };

            const action = {
                type: 'SELECT_ALL_GRID_DATA',
                name: 'stateName',
                selectAll: true
            };
            const returnedState = grid(state, action);
            const expectedState = { ...state, stateName: { ...state.stateName, selection: [{ id: '1' }, { id: '2' }] } };
            expect(returnedState).toEqual(expectedState);
        });

        it('CLEAR_GRID_DATA', () => {
            const state = {
                name: 'stateName',
                stateName: {
                    items: [{ id: '1' }, { id: '2' }],
                    selectAll: true
                },
                configName: {}
            };

            const action = {
                type: 'CLEAR_GRID_DATA',
                name: 'stateName',
                selectAll: true
            };
            const returnedState = grid(state, action);
            const expectedState = { ...state, stateName: { ...state.stateName, selection: [], selectAll: false, items: [], totalItems: 0 } };
            expect(returnedState).toEqual(expectedState);
        });

        it('APPLY_FILTER', () => {
            const state = {
                name: 'stateName',
                stateName: {
                    items: [{ id: '1' }, { id: '2' }],
                    selectAll: true
                },
                configName: {}
            };

            const action = {
                type: 'APPLY_FILTER',
                name: 'stateName',
                selectAll: true,
                filterValues: {}
            };
            const returnedState = grid(state, action);
            const expectedState = { ...state, stateName: { ...state.stateName, filterValues: {}, selectAll: true, refreshToggler: true } };
            expect(returnedState).toEqual(expectedState);
        });

        it('RESET_FILTER', () => {
            const state = {
                name: 'stateName',
                stateName: {
                    items: [{ id: '1' }, { id: '2' }],
                    selectAll: true
                },
                configName: {}
            };

            const action = {
                type: 'RESET_FILTER',
                name: 'stateName',
                selectAll: true,
                filterValues: []
            };
            const returnedState = grid(state, action);
            const expectedState = { ...state, stateName: { ...state.stateName, filterValues: {}, selectAll: true, resetToggler: true } };
            expect(returnedState).toEqual(expectedState);
        });

        it('DISABLE_ITEMS_GRID', () => {
            const state = {
                name: 'stateName',
                stateName: {
                    items: [{ id: '1' }, { id: '2' }],
                    selectAll: true
                },
                configName: {}
            };

            const action = {
                type: 'DISABLE_ITEMS_GRID',
                name: 'stateName',
                keyField: [],
                keyValues: []
            };
            const returnedState = grid(state, action);
            const expectedState = { ...state, stateName: { ...state.stateName, items: [{ id: '1', disabled: false }, { id: '2', disabled: false }], selectAll: true } };
            expect(returnedState).toEqual(expectedState);
        });

        it('REMOVE_GRID_DATA', () => {
            const state = {
                name: 'stateName',
                stateName: {
                    items: [{ id: '1' }, { id: '2' }],
                    selectAll: true, config: { idField: 'id' },
                    selection: []
                }

            };

            const action = {
                type: 'REMOVE_GRID_DATA',
                name: 'stateName',
                items: [{ id: '1' }, { id: '2' }],
                selectAll: true,
                filterValues: []
            };
            const returnedState = grid(state, action);
            const expectedState = { ...state, stateName: { ...state.stateName, items: [], totalItems: 0, selectAll: true } };
            expect(returnedState).toEqual(expectedState);
        });

        it('grid - Handle default action', () => {
            const state = { name: 'stateName' };

            const action = {
                type: 'DEFAULT'
            };
            const returnedState = grid(state, action);
            expect(returnedState).toEqual(state);
        });


        it('INIT_FLYOUT', () => {
            const state = { name: 'stateNames', stateNames: {} };

            const action = {
                type: 'INIT_FLYOUT',
                name: 'stateNames',
                isOpen: true
            };
            const returnedState = flyout(state, action);
            const expectedState = { ...state, stateNames: { ...state.stateNames, isOpen: true } };

            expect(returnedState).toEqual(expectedState);
        });

        it('OPEN_FLYOUT', () => {
            const state = { name: 'stateNames', stateNames: {} };

            const action = {
                type: 'OPEN_FLYOUT',
                name: 'stateNames',
                isOpen: true
            };
            const returnedState = flyout(state, action);
            const expectedState = { ...state, stateNames: { ...state.stateNames, isOpen: true } };

            expect(returnedState).toEqual(expectedState);
        });

        it('CLOSE_FLYOUT', () => {
            const state = { ownership: {} };

            const action = {
                type: 'CLOSE_FLYOUT',
                name: 'ownership'
            };
            const returnedState = flyout(state, action);
            const expectedState = { ownership: { isOpen: false } };

            expect(returnedState).toEqual(expectedState);
        });
        it('FLYOUT- Should handle default action', () => {
            const state = { name: 'stateNames', stateNames: {} };

            const action = {
                type: 'DEFAULT',
                name: 'stateNames',
                isOpen: true
            };
            const returnedState = flyout(state, action);

            expect(returnedState).toEqual(state);
        });

        it('GET_CATEGORY_ELEMENTS', () => {
            const state = { name: 'stateNames', stateNames: {}, progressStatus: { categoryElements: 'categoryElements', categoryElementsToggler: true } };

            const action = {
                type: 'GET_CATEGORY_ELEMENTS',
                name: 'stateNames',
                isOpen: true
            };
            const returnedState = shared(state, action);

            expect(returnedState).toEqual(state);
        });

        it('GET_PROPERTY_RULES', () => {
            const state = { name: 'stateNames', stateNames: {}, progressStatus: { rules: 'rules', rulesToggler: true } };
            const action = {
                type: 'GET_PROPERTY_RULES',
                name: 'stateNames',
                isOpen: true
            };
            const returnedState = shared(state, action);

            expect(returnedState).toEqual(state);
        });

        it('Shared - Should handle default action.', () => {
            const state = { name: 'stateNames', stateNames: {} };

            const action = {
                type: 'DEFAULT',
                rules: []
            };
            const returnedState = shared(state, action);

            expect(returnedState).toEqual(state);
        });

        it('INIT_WIZARD', () => {
            const state = { actionName: {} };

            const action = {
                type: 'INIT_WIZARD',
                name: 'actionName',
                activeStep: 'activeStep',
                totalSteps: 4,
                rules: []
            };
            const returnedState = wizard(state, action);
            const expectedState = { ...state, actionName: { activeStep: 'activeStep', totalSteps: 4 } };

            expect(returnedState).toEqual(expectedState);
        });

        it('WIZARD_SET_STEP', () => {
            const state = { actionName: {} };

            const action = {
                type: 'WIZARD_SET_STEP',
                name: 'actionName',
                activeStep: 'activeStep',
                rules: []
            };
            const returnedState = wizard(state, action);
            const expectedState = { ...state, actionName: { activeStep: 'activeStep' } };
            expect(returnedState).toEqual(expectedState);
        });

        it('WIZARD_PREV_STEP', () => {
            const state = { actionName: { activeStep: 1 } };

            const action = {
                type: 'WIZARD_PREV_STEP',
                name: 'actionName',
                activeStep: 'activeStep',
                rules: []
            };
            const returnedState = wizard(state, action);
            const expectedState = { ...state, actionName: { activeStep: 1 } };
            expect(returnedState).toEqual(expectedState);
        });

        it('WIZARD_NEXT_STEP', () => {
            const state = { actionName: { activeStep: 1 } };

            const action = {
                type: 'WIZARD_NEXT_STEP',
                name: 'actionName',
                activeStep: 'activeStep',
                rules: []
            };
            const returnedState = wizard(state, action);
            const expectedState = { ...state, actionName: { activeStep: 2 } };
            expect(returnedState).toEqual(expectedState);
        });

        it('WIZARD- Should handle default action', () => {
            const state = { actionName: { activeStep: 1 } };

            const action = {
                type: 'DEFAULT',
                name: 'actionName',
                activeStep: 'activeStep',
                rules: []
            };
            const returnedState = wizard(state, action);
            expect(returnedState).toEqual(state);
        });

        it('Notification - Should handle default action', () => {
            const action = {
                type: 'Default'
            };

            expect(notification({}, action)).toEqual({});
        });

        it('dualSelect -INIT_ALL_ITEMS', () => {
            const state = { nodeProducts_owners: { all: [], source: [], sourceSearch: [], sourceText: '', target: [], targetSearch: [], targetText: '' } };

            const action = {
                type: 'INIT_ALL_ITEMS',
                all: [],
                name: 'nodeProducts_owners'
            };

            const returnedState = dualSelect(state, action);
            expect(returnedState).toEqual(state);
        });

        it('dualSelect -INIT_TARGET_ITEMS', () => {
            const state = { segments: { all: [], source: [], sourceSearch: [], sourceText: '', target: [], targetSearch: [], targetText: '' } };

            const action = {
                type: 'INIT_TARGET_ITEMS',
                all: [],
                target: [],
                name: 'segments'
            };

            const returnedState = dualSelect(state, action);
            expect(returnedState).toEqual(state);
        });

        it('dualSelect -SELECT_SOURCE_ITEM:', () => {
            const state = { nodeProducts_owners: { all: [], source: [], sourceSearch: [], sourceText: '', target: [], targetSearch: [], targetText: '' } };

            const action = {
                type: 'SELECT_SOURCE_ITEM',
                all: [],
                target: [],
                name: 'nodeProducts_owners'
            };

            const returnedState = dualSelect(state, action);
            expect(returnedState).toEqual(state);
        });

        it('dualSelect -SELECT_TARGET_ITEM:', () => {
            const state = { nodeProducts_owners: { all: [], source: [], sourceSearch: [], sourceText: '', target: [], targetSearch: [], targetText: '' } };

            const action = {
                type: 'SELECT_TARGET_ITEM',
                all: [],
                target: [],
                ctrlKey: 'x',
                name: 'nodeProducts_owners'
            };

            const returnedState = dualSelect(state, action);
            expect(returnedState).toEqual(state);
        });

        it('dualSelect -SEARCH_SOURCE_ITEMS:', () => {
            const state = { connectionProducts_owners: { source: [], sourceSearch: [], sourceText: 'text' } };

            const action = {
                type: 'SEARCH_SOURCE_ITEMS',
                text: 'text',
                name: 'connectionProducts_owners'
            };

            const returnedState = dualSelect(state, action);
            expect(returnedState).toEqual(state);
        });

        it('dualSelect -SEARCH_TARGET_ITEMS:', () => {
            const state = { connectionProducts_owners: { all: [], source: [], sourceSearch: [], sourceText: '', target: [], targetSearch: [], targetText: '' } };

            const action = {
                type: 'SEARCH_TARGET_ITEMS',
                all: [],
                target: [],
                text: 'text',
                name: 'connectionProducts_owners'
            };

            const returnedState = dualSelect(state, action);
            const expectedState = {
                ...state,
                connectionProducts_owners: {
                    ...state[action.name],
                    targetText: 'text'
                }
            };

            expect(returnedState).toEqual(expectedState);
        });

        it('dualSelect -MOVE_SOURCE_ITEMS:', () => {
            const state = { all: [], source: [] };

            const action = {
                type: 'MOVE_SOURCE_ITEMS',
                all: [],
                target: [],
                text: 'text'
            };

            const returnedState = dualSelect(state, action);
            expect(returnedState).toEqual(state);
        });

        it('dualSelect -MOVE_ALL_SOURCE_ITEMS:', () => {
            const state = { segments: { all: [], source: [{ name: 'name' }], target: [{ name: 'name' }], sourceText: '', targetText: '' } };

            const action = {
                type: 'MOVE_ALL_SOURCE_ITEMS',
                all: [],
                target: [{ kye: 'val' }],
                text: 'text',
                name: 'segments'
            };

            const returnedState = dualSelect(state, action);
            const expectedState = {
                ...state,
                segments: {
                    ...state[action.name],
                    source: [],
                    target: [{ name: 'name', selected: false }, { name: 'name' }],
                    sourceSearch: [],
                    targetSearch: [{ name: 'name', selected: false }, { name: 'name' }]
                }
            };

            expect(returnedState).toEqual(expectedState);
        });

        it('dualSelect -MOVE_TARGET_ITEMS:', () => {
            const state = { connectionProducts_owners: { all: [], source: [{ name: 'name' }], target: [{ name: 'name' }], sourceText: '', targetText: '' } };

            const action = {
                type: 'MOVE_TARGET_ITEMS',
                all: [],
                target: [{ kye: 'val' }],
                text: 'text',
                name: 'connectionProducts_owners'
            };

            const returnedState = dualSelect(state, action);
            const expectedState = {
                ...state,
                connectionProducts_owners: {
                    ...state[action.name],
                    source: [{ name: 'name' }],
                    target: [],
                    sourceSearch: [{ name: 'name' }],
                    targetSearch: []
                }
            };

            expect(returnedState).toEqual(expectedState);
        });

        it('dualSelect - MOVE_ALL_TARGET_ITEMS:', () => {
            const state = {
                segments: { all: [], source: [{ name: 'name' }], target: [{ name: 'name' }], sourceText: '', targetText: '' }
            };

            const action = {
                type: 'MOVE_ALL_TARGET_ITEMS',
                all: [],
                target: [{ kye: 'val' }],
                text: 'text',
                name: 'segments'
            };

            const returnedState = dualSelect(state, action);
            const expectedState = {
                ...state,
                segments: {
                    ...state[action.name],
                    source: [{ name: 'name', selected: false }, { name: 'name' }],
                    target: [],
                    sourceSearch: [{ name: 'name', selected: false }, { name: 'name' }],
                    targetSearch: []
                }
            };
            expect(returnedState).toEqual(expectedState);
        });

        it('dualSelect - should handle default action', () => {
            const state = { all: [], source: [{ name: 'name' }], target: [{ name: 'name' }], sourceText: '', targetText: '' };

            const action = {
                type: 'DEFAULT',
                all: [],
                target: [{ kye: 'val' }],
                text: 'text'
            };

            const returnedState = dualSelect(state, action);
            expect(returnedState).toEqual(state);
        });

        it('pageActions- Should handle default action', () => {
            const state = {};

            const action = {
                type: 'DEFAULT'
            };

            const returnedState = pageActions(state, action);
            expect(returnedState).toEqual(state);
        });

        it('should handle action NODE_FILTER_ON_SELECT_CATEGORY',
            function () {
                const initialState = {
                    selectedCategory: 'y',
                    selectedElement: 'y',
                    searchedNodes: ['node'],
                    filters: {}
                };
                const action = {
                    type: 'NODE_FILTER_ON_SELECT_CATEGORY',
                    selectedCategory: 'x'
                };
                const newState = Object.assign({}, initialState, { selectedCategory: 'x' });
                expect(nodeFilter(initialState, action)).toEqual(newState);
            });

        it('should handle action NODE_FILTER_ON_SELECT_ELEMENT',
            function () {
                const initialState = {
                    selectedCategory: 'y',
                    selectedElement: 'y',
                    searchedNodes: ['node'],
                    filters: {}
                };
                const action = {
                    type: 'NODE_FILTER_ON_SELECT_ELEMENT',
                    selectedElement: 'x'
                };
                const newState = Object.assign({}, initialState, { selectedElement: 'x' });
                expect(nodeFilter(initialState, action)).toEqual(newState);
            });

        it('should handle action NODE_FILTER_RECEIVE_SEARCH_NODES',
            function () {
                const initialState = {
                    selectedCategory: 'y',
                    selectedElement: 'y',
                    searchedNodes: ['node'],
                    filters: {}
                };
                const action = {
                    type: 'NODE_FILTER_RECEIVE_SEARCH_NODES',
                    nodes: ['newNode']
                };
                const newState = Object.assign({}, initialState, { searchedNodes: ['newNode'] });
                expect(nodeFilter(initialState, action)).toEqual(newState);
            });

        it('should handle action NODE_FILTER_CLEAR_SEARCH_NODES', () => {
            const initialState = {
                selectedCategory: 'y',
                selectedElement: 'y',
                searchedNodes: ['node'],
                filters: {}
            };
            const action = {
                type: 'NODE_FILTER_CLEAR_SEARCH_NODES',
                searchedNodes: []
            };
            const newState = Object.assign({},
                initialState,
                {
                    searchedNodes: action.searchedNodes
                });

            expect(nodeFilter(initialState, action)).toEqual(newState);
        });
        it('should set selected node id ', () => {
            const initialState = {
                selectedNodeId: null
            };
            const action = {
                type: 'SET_SELECTED_NODE',
                nodeId: 1
            };
            const newState = Object.assign({}, initialState, { selectedNodeId: action.nodeId });
            expect(ownershipRules(initialState, action)).toEqual(newState);
        });
        it('should update the receiveUpdatedToggler state to true', () => {
            const initialState = {
                ownershipRules: {
                    receiveUpdatedToggler: null
                }
            };
            const action = {
                type: 'RECEIVE_BULK_UPDATE_POPUP',
                name: 'ownershipRules'
            };
            const newState = Object.assign({}, initialState, { ownershipRules: { receiveUpdatedToggler: true } });
            expect(ownershipRules(initialState, action)).toEqual(newState);
        });

        it('should handle action NODE_FILTER_RESET_FIELDS', () => {
            const initialState = {
                searchedNodes: [],
                selectedCategory: [],
                selectedElement: []
            };
            const action = {
                type: 'NODE_FILTER_RESET_FIELDS'
            };
            const newState = Object.assign({}, initialState, { selectedCategory: [], selectedElement: [], searchedNodes: [] });
            expect(nodeFilter(initialState, action)).toEqual(newState);
        });

        it('should handle action REPORT_REQUEST_COMPLETED', () => {
            const initialState = {
                reportToggler: false
            };
            const action = {
                type: 'REPORT_REQUEST_COMPLETED'
            };
            const newState = Object.assign({}, initialState, { reportToggler: true });
            expect(nodeFilter(initialState, action)).toEqual(newState);
        });

        it('should handle action RECEIVE_DATE_RANGES', () => {
            const initialState = {
                dateRange: {},
                defaultYear: '2020',
                dateRangeToggler: false
            };
            const action = {
                type: 'RECEIVE_DATE_RANGES',
                data: {
                    officialPeriods: {},
                    defaultYear: '2020'
                }
            };
            const newState = Object.assign({}, initialState, { dateRange: {}, defaultYear: '2020', dateRangeToggler: true });
            expect(nodeFilter(initialState, action)).toEqual(newState);
        });

        it('should handle action RESET_DATE_RANGES', () => {
            const initialState = {
                dateRange: {},
                defaultYear: '2020',
                dateRangeToggler: false,
                viewReportButtonStatusToggler: false
            };
            const action = {
                type: 'RESET_DATE_RANGES'
            };
            const newState = Object.assign({}, initialState, { dateRange: {}, defaultYear: null, dateRangeToggler: null, viewReportButtonStatusToggler: false });
            expect(nodeFilter(initialState, action)).toEqual(newState);
        });

        it('should handle action ENABLE_DISABLE_VIEW_REPORT_BUTTON', () => {
            const initialState = {
                viewReportButtonStatusToggler: false
            };
            const action = {
                type: 'ENABLE_DISABLE_VIEW_REPORT_BUTTON',
                buttonStatus: true
            };
            const newState = Object.assign({}, initialState, { viewReportButtonStatusToggler: true });
            expect(nodeFilter(initialState, action)).toEqual(newState);
        });

        it('RESET_PAGE_INDEX', () => {
            const state = { actionName: { selectAll: false, refreshToggler: true }, configName: {} };
            const action = {
                type: 'RESET_PAGE_INDEX',
                name: 'actionName'
            };
            const returnedState = grid(state, action);

            const expectedState = {
                actionName: {
                    selectAll: false,
                    refreshToggler: true,
                    resetPageIndexToggler: true
                },
                configName: {}
            };
            expect(returnedState).toEqual(expectedState);
        });
    });
