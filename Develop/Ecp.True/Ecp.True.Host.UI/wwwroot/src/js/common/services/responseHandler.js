import { navigationService } from './navigationService';
import { constants } from './constants';
import { dispatcher } from '../store/dispatcher';
import { httpService } from './httpService';
import { showError } from '../actions';
import { resourceProvider } from './resourceProvider';
import { modalService } from './modalService';

const responseHandler = (function () {
    function handleForbidden(failureHandler, response) {
        if (response.headers.has('RedirectPathOnAuthFailure')) {
            const redirectPath = response.headers.get('RedirectPathOnAuthFailure') + '?returnPath=' + window.location.pathname + window.location.search;
            window.location = encodeURI(redirectPath);
        } else if (failureHandler) {
            dispatcher.dispatch(failureHandler(response.body));
        } else {
            navigationService.handleError(constants.Errors.Forbidden);
        }
    }

    function handleError(failureHandler, response) {
        if (response.status === 401) {
            handleForbidden(failureHandler, response);
        } else if ((response.status === 400 || response.status === 403 || response.status === 404 || response.status === 500) && !failureHandler) {
            navigationService.handleError(response.status);
        } else if (response.status === 409) {
            dispatcher.dispatch(showError(resourceProvider.read('conflictError'), modalService.isOpen()));
        }
        if (response.headers.has('XSRF-TOKEN')) {
            httpService.setAntiforgeryRequestToken(response.headers.get('XSRF-TOKEN'));
        }
    }

    function handleResponse(successHandler, failureHandler, response, notFound) {
        if (response.status === 409 || response.status === 401) {
            return;
        }
        if (notFound === true && response.body.count === 0) {
            navigationService.handleError(constants.Errors.NotFound);
        }
        if (response.status !== 200 && failureHandler) {
            dispatcher.dispatch(failureHandler(response.body, response.status));
        } else if (successHandler) {
            if (Array.isArray(successHandler)) {
                successHandler.map(x => dispatcher.dispatch(x(response.body)));
            } else {
                dispatcher.dispatch(successHandler(response.body));
            }
        }
    }

    return {
        handleError: (failureHandler, response) => {
            handleError(failureHandler, response);
        },
        handleResponse: (successHandler, failureHandler, response, notFound = false) => {
            handleResponse(successHandler, failureHandler, response, notFound);
        }
    };
}());

export { responseHandler };
