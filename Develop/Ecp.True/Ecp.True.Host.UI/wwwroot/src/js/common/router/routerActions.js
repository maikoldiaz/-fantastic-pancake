import { utilities } from '../services/utilities';

const routerActions = (function () {
    const actionCallbacks = {};

    return {
        configure: (actionName, action) =>{
            actionCallbacks[actionName] = action;
        },
        fireAction: actionName => {
            if (utilities.hasProperty(actionCallbacks, actionName)) {
                actionCallbacks[actionName]();
            }
        }
    };
}());

export { routerActions };
