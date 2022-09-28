import { httpService } from './httpService.js';

const navigationService = (function () {
    const props = {};

    const getBasePath = () => {
        return '';
    };

    const getModulePath = () => {
        return `${httpService.getModuleName().toLowerCase()}`;
    };

    const handleError = errorCode => {
        switch (errorCode) {
        case 401:
            if (props.history) {
                props.history.push({ pathname: `${getBasePath()}/error/forbidden` });
            } else {
                window.location = '/error/forbidden';
            }
            break;
        case 403:
            if (props.history) {
                props.history.push({ pathname: `${getBasePath()}/error/noaccess` });
            } else {
                window.location = '/error/noaccess';
            }
            break;
        case 500:
        case 400:
            if (props.history) {
                props.history.push({ pathname: `${getBasePath()}/error/unknown` });
            } else {
                window.location = '/error/unknown';
            }
            break;
        case 504:
            window.location = '/error/forcedlogout';
            break;
        default:
            props.history.push({ pathname: `${getBasePath()}/error/notfound` });
        }
    };

    const getParamByName = id => {
        let param = props.match.params[id];
        if (!param) {
            param = httpService.getParamByName(id.toLowerCase());
        }
        return param;
    };

    const getBreadCrumbs = (routerConfig, location) => {
        const data = routerConfig.getRoutes();
        const routePath = location.pathname.split('/');
        const pagePath = Object.keys(data).find(x => x.toLowerCase() === routePath[5].toLowerCase());
        const currentData = data[pagePath];

        let pageName;
        if (currentData.routeKey) {
            pageName = currentData.routeKey;
        } else if (location.state && location.state.pageName) {
            pageName = location.state.pageName;
        } else {
            pageName = currentData.details.pageName ? currentData.details.pageName : pagePath;
        }

        const routeBack = `${getModulePath()}/${location.state && location.state.routeBackKey ? location.state.routeBackKey : pagePath}`;
        const subPage = location.state ? location.state.subPage : null;
        return { pageName, routeBack, subPage };
    };

    const goBack = () => {
        props.history.goBack();
    };

    return {
        initialize: (history, location, match) => {
            props.history = history;
            props.location = location;
            props.match = match;
        },
        getModulePath: () => {
            return getModulePath();
        },
        getBasePath: () => {
            return getBasePath();
        },
        navigateTo: (path, state = null, search = null) => {
            props.history.push({
                pathname: `${path ? `/${getModulePath()}/${path}` : getModulePath()}`,
                state,
                search: search ? search.toLowerCase() : null
            });
        },
        navigateToModule: (path, state = null, search = null) => {
            props.history.push({
                pathname: `${path ? `${getBasePath()}/${path.toLowerCase()}` : getModulePath()}`,
                state,
                search: search ? search.toLowerCase() : null
            });
        },
        signOut: () => {
            window.location.replace(window.location.origin + '/Account/Signout?returnPath=' + window.location.pathname);
        },
        getParamByName: id => {
            return getParamByName(id);
        },
        isDetail: () => {
            return props.location.pathname.split('/').length > 6;
        },
        handleError: errorCode => {
            return handleError(errorCode);
        },
        getCurrentFeatureName: () => {
            return props.location.pathname.split('/')[1];
        },
        goBack: () => {
            return goBack();
        },
        getBreadCrumbs: (routerConfig, location) => {
            return getBreadCrumbs(routerConfig, location);
        },
        isErrorModule: () => {
            return getModulePath() === 'error';
        },
        isNotAuthorized: () => {
            return getModulePath() === 'error' && window.location.pathname.includes('unauthorized');
        }
    };
}());

export { navigationService };
