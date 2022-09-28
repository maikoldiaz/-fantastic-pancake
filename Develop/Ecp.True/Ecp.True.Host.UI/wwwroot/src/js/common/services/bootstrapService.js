import { httpService } from './httpService.js';
import {
    root, modal, loader, tabs, flyout, shared, categoryElementFilter, wizard, notification,
    dualSelect, pageActions, addComment, confirmModal, reports, notificationButton,
    nodeFilter, colorPicker, iconPicker, powerAutomate, ownershipRules, ruleSynchronizer
} from '../reducers.js';
import { grid } from '../components/grid/reducers';
import { reducer as form } from 'redux-form';
import { utilities } from './utilities.js';
import { dateService } from './dateService.js';
import { authService } from './authService.js';
import { sessionService } from './sessionService';
import { systemConfigService } from './systemConfigService';

const bootstrapService = (function () {
    let appBootstrapFn = null;
    const registrations = [];
    const reducers = [];

    return {
        initModule: (module, config) => {
            registrations.push({ module, config });
        },
        registerReducer: reducer => {
            reducers.push(reducer);
        },
        registerAppBootstrapper: bootstrapFn => {
            appBootstrapFn = bootstrapFn;
        },
        bootstrap: context => {
            dateService.initialize();
            authService.initialize(context);
            sessionService.initialize(systemConfigService.getMaxSessionDuration());
        },
        bootstrapApp: () => {
            if (appBootstrapFn) {
                appBootstrapFn();
            }
        },
        getAllReducers: () => {
            const commonReducers = {
                root, modal, form, loader, tabs, grid, flyout, shared, categoryElementFilter,
                wizard, notification, dualSelect, pageActions, addComment, confirmModal,
                reports, notificationButton, nodeFilter, colorPicker, iconPicker,
                powerAutomate, ownershipRules, ruleSynchronizer
            };

            let appReducers = {};
            reducers.forEach(reducer => {
                appReducers = Object.assign({}, appReducers, reducer);
            });

            return Object.assign({}, commonReducers, appReducers);
        },
        getRouterConfig: () => {
            const index = registrations.findIndex(x => x.module !== '' && x.module.toLowerCase() === httpService.getModuleName().toLowerCase());
            if (index >= 0) {
                return registrations[index].config.routerConfig;
            }
            return null;
        },
        getRoute: moduleName => {
            let module = moduleName;
            const index = registrations.findIndex(x => x.module !== '' && utilities.equalsIgnoreCase(x.module, module));
            if (index >= 0) {
                const RouterConfig = registrations[index].config.routerConfig;
                const config = new RouterConfig();
                const routes = config.getRoutes();
                const routesArr = Object.values(routes);

                module = routesArr.length > 0 ? `${moduleName}/${routesArr[0].routeKey}` : moduleName;
            }

            return module.toLowerCase();
        },
        getDefaultRoute: () => {
            return 'home/index';
        },
        getModalConfig: () => {
            const index = registrations.findIndex(x => utilities.equalsIgnoreCase(x.module, httpService.getModuleName()));
            if (index >= 0) {
                return registrations[index].config.modalConfig;
            }
            return null;
        },
        isActive: (match, location, moduleName) => {
            if (match) {
                return true;
            }

            let isUriValid = false;
            const module = moduleName;
            const index = registrations.findIndex(x => x.module !== '' && utilities.equalsIgnoreCase(x.module, module));
            if (index >= 0) {
                const RouterConfig = registrations[index].config.routerConfig;
                const config = new RouterConfig();
                const routes = config.getRoutes();
                Object.values(routes).forEach(function (key) {
                    if (location.pathname && location.pathname.includes(utilities.toLowerCase(`${moduleName}/${key.routeKey}`))) {
                        isUriValid = true;
                    }
                });
            }

            return isUriValid;
        }
    };
}());

export { bootstrapService };
