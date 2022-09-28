import { bootstrapService } from '../../../common/services/bootstrapService';
import { httpService } from '../../../common/services/httpService';
import { dateService } from '../../../common/services/dateService';
import { sessionService } from '../../../common/services/sessionService';
import { systemConfigService } from '../../../common/services/systemConfigService';
import { reducer } from 'redux-form';

describe('bootstrap Service',
    () => {
        beforeAll(() => {
            bootstrapService.initModule('testModule', {
                routerConfig: {
                    testRouter: 'testRouter'
                },
                modalConfig: {
                    testModal: 'testModal'
                }
            });
        });
        it('should bootstrap',
            () => {
                const dateServiceMock = dateService.initialize = jest.fn();
                const sessionServiceMock = sessionService.initialize = jest.fn();
                const systemConfigServiceMock = systemConfigService.getMaxSessionDuration = jest.fn();
                bootstrapService.bootstrap();
                expect(dateServiceMock.mock.calls.length).toBe(1);
                expect(systemConfigServiceMock.mock.calls.length).toBe(1);
                expect(sessionServiceMock.mock.calls.length).toBe(1);
            });

        it('should registerAppBootstrapper and bootstrapApp',
            () => {
                const mock = jest.fn();
                bootstrapService.registerAppBootstrapper(mock);
                bootstrapService.bootstrapApp();
                expect(mock.mock.calls.length).toBe(1);
            });

        it('should getAllReducers',
            () => {
                const result = bootstrapService.getAllReducers();
                expect(Object.keys(result)).toEqual(['root',
                    'modal', 'form', 'loader', 'tabs', 'grid', 'flyout', 'shared', 'categoryElementFilter',
                    'wizard', 'notification', 'dualSelect', 'pageActions', 'addComment', 'confirmModal', 'reports',
                    'notificationButton', 'nodeFilter', 'colorPicker', 'iconPicker', 'powerAutomate',
                    'ownershipRules', 'ruleSynchronizer']);
            });

        it('should getRouterConfig',
            () => {
                httpService.getModuleName = jest.fn(() => 'testModule');
                const result = bootstrapService.getRouterConfig();
                expect(result).toEqual({ testRouter: 'testRouter' });
            });

        it('should getDefaultRoute',
            () => {
                const result = bootstrapService.getDefaultRoute();
                expect(result).toEqual('home/index');
            });

        it('should getModalConfig',
            () => {
                httpService.getModuleName = jest.fn(() => 'testModule');
                const result = bootstrapService.getModalConfig();
                expect(result).toEqual({ testModal: 'testModal' });
            });

        it('should registerReducer',
            () => {
                const result = bootstrapService.registerReducer(reducer);
                expect(result).toBeUndefined();
            });
    });
