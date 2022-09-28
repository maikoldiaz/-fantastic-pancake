import fetchMiddleware from '../../../common/middlewares/fetchMiddleware';

describe('fetchMiddleware', () =>{
    it('should handle actions without having fetchConfig', () =>{
        const state = {};
        const next = function (action) {
            return action;
        };
        const action = {};

        const result = fetchMiddleware(state)(next)(action);

        expect(result).toEqual(action);
    });

    it('should handle actions without having show progress property', () =>{
        const state = {
            dispatch: function () {}
        };
        const next = function (action) {
            return action;
        };
        const action = {
            fetchConfig: {}
        };

        const mockSuccessResponse = {};
        const mockJsonPromise = Promise.resolve(mockSuccessResponse);
        const mockFetchPromise = Promise.resolve({
            json: () => mockJsonPromise
        });

        jest.spyOn(global, 'fetch').mockImplementation(() => mockFetchPromise);

        fetchMiddleware(state)(next)(action);

        expect(global.fetch).toHaveBeenCalledTimes(1);
    });

    it('should handle actions without header information', () =>{
        const state = {
            dispatch: function () {}
        };
        const next = function (action) {
            return action;
        };
        const action = {
            fetchConfig: { headers: { } }
        };

        const mockSuccessResponse = {};
        const mockJsonPromise = Promise.resolve(mockSuccessResponse);
        const mockFetchPromise = Promise.resolve({
            json: () => mockJsonPromise
        });

        jest.spyOn(global, 'fetch').mockImplementation(() => mockFetchPromise);

        fetchMiddleware(state)(next)(action);

        expect(global.fetch).toHaveBeenCalledTimes(2);
    });

    it('should handle actions with header information', () =>{
        const state = {
            dispatch: function () {}
        };
        const next = function (action) {
            return action;
        };
        const action = {
            fetchConfig: { headers: { 'Content-Type': 'application/json' } }
        };

        const mockSuccessResponse = {};
        const mockJsonPromise = Promise.resolve(mockSuccessResponse);
        const mockFetchPromise = Promise.resolve({
            json: () => mockJsonPromise
        });

        jest.spyOn(global, 'fetch').mockImplementation(() => mockFetchPromise);

        fetchMiddleware(state)(next)(action);

        expect(global.fetch).toHaveBeenCalledTimes(3);
    });

    it('should handle actions with capability information', () =>{
        const state = {
            dispatch: function () {}
        };
        const next = function (action) {
            return action;
        };
        const action = {
            fetchConfig: { capability: 'capabilityValue' }
        };

        const mockSuccessResponse = {};
        const mockJsonPromise = Promise.resolve(mockSuccessResponse);
        const mockFetchPromise = Promise.resolve({
            json: () => mockJsonPromise
        });

        jest.spyOn(global, 'fetch').mockImplementation(() => mockFetchPromise);

        fetchMiddleware(state)(next)(action);

        expect(global.fetch).toHaveBeenCalledTimes(4);
    });

    it('should handle actions with method other than GET', () =>{
        const state = {
            dispatch: function () {}
        };
        const next = function (action) {
            return action;
        };
        const action = {
            fetchConfig: { method: 'POST' }
        };

        const mockSuccessResponse = {};
        const mockJsonPromise = Promise.resolve(mockSuccessResponse);
        const mockFetchPromise = Promise.resolve({
            json: () => mockJsonPromise
        });

        jest.spyOn(global, 'fetch').mockImplementation(() => mockFetchPromise);

        fetchMiddleware(state)(next)(action);

        expect(global.fetch).toHaveBeenCalledTimes(5);
    });
});
