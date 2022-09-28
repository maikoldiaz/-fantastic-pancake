/* eslint-disable no-prototype-builtins */
import 'isomorphic-fetch';
import { httpService } from '../services/httpService';
import { showLoader, hideLoader } from '../actions';
import { constants } from '../services/constants';
import { responseHandler } from '../services/responseHandler';

/* istanbul ignore next */
const fetchMiddleware = store => next => action => {
    if (!action || !action.fetchConfig) {
        return next(action);
    }

    const dispatch = store.dispatch;
    const config = action.fetchConfig;

    // Show progress indicator by default for all fetch calls
    if (!config.hasOwnProperty('showProgress')) {
        config.showProgress = true;
    }

    // copy everything except fetchConfig
    const clonedAction = JSON.parse(JSON.stringify(action));
    delete clonedAction.fetchConfig;

    dispatch(clonedAction);

    // Show progress
    if (config.showProgress) {
        dispatch(showLoader());
    }

    const path = config.path;
    const method = config.method || (config.body ? 'POST' : 'GET');

    // Create request headers
    let headers;
    if (!config.headers) {
        headers = new Headers({ 'Content-Type': 'application/json' });
    } else if (!config.headers['Content-Type']) {
        headers = new Headers(Object.assign({}, config.headers, { 'Content-Type': 'application/json' }));
    } else {
        headers = new Headers(config.headers);
    }

    if (config.capability) {
        headers.append('capability', config.capability);
    }
    const body = config.body;
    const successHandler = config.success;
    const failureHandler = config.failure;
    const notFound = config.notFound;
    let status = 0;

    if (method === 'GET') {
        headers.append('Pragma', 'no-cache');
    } else {
        headers.append(constants.ForgeryTokenName, httpService.readAntiforgeryToken());
    }

    headers.append('True-Origin', 'true');

    return fetch(path,
        {
            method,
            headers,
            credentials: 'same-origin',
            body: JSON.stringify(body),
            cache: 'no-cache'
        })
        .then(response => {
            status = response.status;
            responseHandler.handleError(failureHandler, response);
            return response.json().then(data => ({ status: response.status, body: data }));
        })
        .then(response => {
            responseHandler.handleResponse(successHandler, failureHandler, response, notFound);
            if (config.showProgress) {
                dispatch(hideLoader());
            }
        })
        .catch(error => {
            if (failureHandler && status !== 401) {
                dispatch(failureHandler(error));
            }

            if (config.showProgress) {
                dispatch(hideLoader());
            }
        });
};

export default fetchMiddleware;
