import { telemetryMiddleware } from '../../../common/middlewares/telemetryMiddleware';
import { ai } from '../../../common/telemetry/telemetryService';

describe('telemetryMiddleware', () =>{
    it('should handle actions when ai is not initialized', () =>{
        const state = {};
        const next = function (action) {
            return action;
        };
        const action = {};

        const result = telemetryMiddleware(state)(next)(action);

        expect(result).toEqual(action);
    });

    it('should handle actions when ai is initialized', () =>{
        const state = {};
        const next = function (action) {
            return action;
        };
        const action = {};
        ai.isReady = jest.fn().mockImplementationOnce(() => true);
        state.getState = jest.fn().mockImplementationOnce(() => 'teststate');

        const result = telemetryMiddleware(state)(next)(action);

        expect(result).toEqual(action);
    });

    it('should handle actions when ai is initialized and exception', () =>{
        const state = {};
        const next = function (action) {
            throw 'test error';
        };
        const action = {};
        ai.isReady = jest.fn().mockImplementationOnce(() => true);
        ai.w3cId = jest.fn().mockImplementationOnce(() => 'testId');
        const trackTraceMock = ai.trackTrace = jest.fn();
        const trackExceptionMock = ai.trackException = jest.fn();
        state.getState = jest.fn().mockImplementation(() => 'teststate');

        let thrownError = '';
        try {
            telemetryMiddleware(state)(next)(action);
        } catch (error) {
            thrownError = error;
        }

        expect(thrownError).toBe('test error');
        expect(trackTraceMock).toHaveBeenCalledTimes(3);
        expect(trackExceptionMock).toHaveBeenCalledTimes(1);
    });
});
