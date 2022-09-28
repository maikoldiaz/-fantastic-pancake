import { navigationService } from '../../../common/services/navigationService';
import { httpService } from '../../../common/services/httpService';
import { RouterConfig } from '../../../common/router/routerConfig';

const history = {
    push: jest.fn()
};
describe('navigation Service',

    () => {
        const { location } = window;

        beforeAll(() => {
            delete window.location;
            window.location = {};

            const location = { pathname: '/true/category/manageCategory/test' };
            const match = {
                params: {
                    id: null
                }
            };
            navigationService.initialize(history, location, match);
            httpService.getModuleName = jest.fn(() => 'moduleTest');
        });

        afterAll(() => {
            window.location = location;
        });

        it('should return modulePath',
            () => {
                const result = navigationService.getModulePath();
                expect(result).toMatch('moduletest');
            });

        it('should return basePath',
            () => {
                const result = navigationService.getBasePath();
                expect(result).toMatch('');
            });

        it('should navigateTo',
            () => {
                navigationService.navigateTo();
                expect(history.push.mock.calls.length).toBe(1);
                expect(history.push.mock.calls[0][0]).toEqual({
                    pathname: 'moduletest',
                    search: null,
                    state: null
                });
                navigationService.navigateTo('test', 'testState', 'testSearch');
                expect(history.push.mock.calls.length).toBe(2);
                expect(history.push.mock.calls[1][0]).toEqual({
                    pathname: '/moduletest/test',
                    search: 'testsearch',
                    state: 'testState'
                });
            });

        it('should navigateToModule',
            () => {
                navigationService.navigateToModule();
                expect(history.push.mock.calls.length).toBe(3);
                expect(history.push.mock.calls[0][0]).toEqual({
                    pathname: 'moduletest',
                    search: null,
                    state: null
                });
                navigationService.navigateToModule('test', 'testState', 'testSearch');
                expect(history.push.mock.calls.length).toBe(4);
                expect(history.push.mock.calls[1][0]).toEqual({
                    pathname: '/moduletest/test',
                    search: 'testsearch',
                    state: 'testState'
                });
            });

        it('should return paramByName',
            () => {
                const mock = httpService.getParamByName = jest.fn(() => '1');
                const result = navigationService.getParamByName('Id');
                expect(mock.mock.calls.length).toBe(1);
                expect(mock.mock.calls[0][0]).toEqual('id');
                expect(result).toMatch('1');
            });

        it('should return isDetail',
            () => {
                const result = navigationService.isDetail();
                expect(result).toBe(false);
            });

        it('should handleError',
            () => {
                navigationService.handleError(401);
                expect(history.push.mock.calls.length).toBe(5);
                expect(history.push.mock.calls[4][0]).toEqual({
                    pathname: '/error/forbidden'
                });
                navigationService.handleError(403);
                expect(history.push.mock.calls.length).toBe(6);
                expect(history.push.mock.calls[5][0]).toEqual({
                    pathname: '/error/noaccess'
                });
                navigationService.handleError();
                expect(history.push.mock.calls.length).toBe(7);
                expect(history.push.mock.calls[6][0]).toEqual({
                    pathname: '/error/notfound'
                });
            });

        it('should return getCurrentFeatureName',
            () => {
                const result = navigationService.getCurrentFeatureName();
                expect(result).toBe('true');
            });

        it('should return false for non error module',
            () => {
                const result = navigationService.isErrorModule();
                expect(result).toBe(false);
            });

        it('should return true for error module',
            () => {
                httpService.getModuleName = jest.fn(() => 'error');
                const result = navigationService.isErrorModule();
                expect(result).toBe(true);
                httpService.getModuleName = jest.fn(() => 'moduleTest');
            });

        it('should return false for non error module',
            () => {
                httpService.getModuleName = jest.fn(() => 'category');
                const result = navigationService.isNotAuthorized();
                expect(result).toBe(false);
                httpService.getModuleName = jest.fn(() => 'moduleTest');
            });

        it('should return false for error module which is not unauthorized',
            () => {
                httpService.getModuleName = jest.fn(() => 'error');
                window.location.pathname = '/error/noaccess';
                const result = navigationService.isNotAuthorized();
                expect(result).toBe(false);
                httpService.getModuleName = jest.fn(() => 'moduleTest');
            });

        it('should return true for error unauthorized module',
            () => {
                httpService.getModuleName = jest.fn(() => 'error');
                window.location.pathname = '/error/unauthorized';
                const result = navigationService.isNotAuthorized();
                expect(result).toBe(true);
                httpService.getModuleName = jest.fn(() => 'moduleTest');
            });
    });
