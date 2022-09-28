import { createStore, applyMiddleware, compose } from 'redux';
import { createLogger } from 'redux-logger';
import { constants } from './../services/constants';
import { telemetryMiddleware } from './../middlewares/telemetryMiddleware';

const configureStore = function (reducer, preloadedState, middlewares) {
    // Enable logging for non-production environment
    if (process.env.NODE_ENV === constants.Config_Constants.PROD) {
        return createStore(
            reducer,
            preloadedState,
            applyMiddleware(...middlewares, telemetryMiddleware)
        );
    }
    const logger = createLogger({
    });

    return createStore(
        reducer,
        preloadedState,
        compose(
            applyMiddleware(...middlewares, telemetryMiddleware, logger)
        ));
};

export default configureStore;
