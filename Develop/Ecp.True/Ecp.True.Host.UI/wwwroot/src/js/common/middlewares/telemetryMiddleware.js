import { ai } from './../telemetry/telemetryService';
import { utilities } from './../services/utilities';

export const telemetryMiddleware = store => next => action => {
    if (ai.isReady()) {
        const prevState = utilities.stringifyJson(store.getState());
        try {
            return next(action);
        } catch (error) {
            const traceId = ai.w3cId();
            ai.trackTrace('Previous state: ' + prevState, traceId);
            ai.trackTrace('Dispatching action: ' + action, traceId);
            ai.trackException(error, traceId);
            ai.trackTrace('Next state: ' + utilities.stringifyJson(store.getState()), traceId);
            throw error;
        }
    }

    return next(action);
};
