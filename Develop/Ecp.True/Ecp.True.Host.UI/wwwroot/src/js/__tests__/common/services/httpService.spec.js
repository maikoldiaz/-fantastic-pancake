import { httpService } from '../../../common/services/httpService';

describe('http service',
    () => {
        it('should build api for read antiforgery token',
            () => {
                httpService.setAntiforgeryRequestToken('token');
                expect(httpService.readAntiforgeryToken()).toMatch('token');
            });
        it('should build api for getCurrentCulture',
            () => {
                expect(httpService.getCurrentCulture())
                    .toMatch('es');
            });
        it('should build api for getParamByName',
            () => {
                expect(httpService.getParamByName())
                    .toBeUndefined();
            });
        it('should build api for getModuleName',
            () => {
                expect(httpService.getModuleName())
                    .toMatch('');
            });
        it('should build api for getSubModuleName',
            () => {
                expect(httpService.getSubModuleName())
                    .toMatch('');
            });
        it('should build api for getDetailsModuleName',
            () => {
                expect(httpService.getDetailsModuleName())
                    .toBeNull();
            });
        it('should build api for hasHash',
            () => {
                expect(httpService.hasHash('nodeId'))
                    .toStrictEqual(false);
            });
        it('should build api for getCurrentPathName',
            () => {
                expect(httpService.getCurrentPathName(''))
                    .toMatch('/');
            });
        it('should build api for getQueryString',
            () => {
                expect(httpService.getQueryString('culture'))
                    .toBeNull();
            });
    });
