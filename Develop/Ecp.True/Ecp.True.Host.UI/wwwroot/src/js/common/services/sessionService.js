import { authService } from './authService';
import { navigationService } from './navigationService';
import * as store from 'store';

/* istanbul ignore next */
const sessionService = (function () {
    let maxSessionDurationInMilliseconds;
    let interval = null;
    const storeKey = 'lastAction';

    const getlastAction = () => {
        return parseInt(store.get(storeKey), 10);
    };

    const setlastAction = value => {
        store.set(storeKey, value);
    };

    const clear = () => {
        if (interval) {
            clearInterval(interval);
        }
    };

    const check = () => {
        const now = Date.now();
        const timeleft = getlastAction() + maxSessionDurationInMilliseconds;
        const diff = timeleft - now;
        const expired = diff < 0;

        if (expired && authService.isAuthorized) {
            clear();
            navigationService.signOut();
        }
    };

    const initInterval = () => {
        interval = setInterval(() => {
            check();
        }, maxSessionDurationInMilliseconds);
    };

    const reset = () => {
        setlastAction(Date.now());
        clear();
        initInterval();
    };

    const initializeSession = maxDurationInMins => {
        maxSessionDurationInMilliseconds = maxDurationInMins * 60 * 1000;
        document.body.addEventListener('click', reset);
        reset();
    };

    return {
        initialize: initializeSession
    };
}());

export { sessionService };
