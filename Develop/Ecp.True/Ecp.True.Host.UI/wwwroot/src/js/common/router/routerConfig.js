import { resourceProvider } from './../../common/services/resourceProvider';
import { httpService } from '../services/httpService';
import { utilities } from '../services/utilities';

export default class RouterConfig {
    constructor(pageKey, config) {
        this.pageKey = pageKey;
        this.config = config;
    }

    getRoutes() {
        const routes = this.config.routes;
        return routes;
    }

    isNoRoute() {
        return this.config.noRoute;
    }

    getPageTitle() {
        return resourceProvider.read(this.pageKey);
    }

    getPageNames() {
        const names = [];
        Object.keys(this.getRoutes()).map(m => names.push({ name: m, key: this.config.routes[m].routeKey }));

        return names;
    }

    getPageRoute(pageName) {
        const allRoutes = this.getRoutes();
        return allRoutes[Object.keys(allRoutes).find(x => x.toLowerCase() === (pageName ? pageName.toLowerCase() : pageName))];
    }

    getPageDetails(routeKey) {
        const routes = Object.values(this.getRoutes());
        const route = routes.filter(r => r.routeKey === routeKey);
        return route.length > 0 ? route[0].details : null;
    }

    getPageRoles() {
        const routes = Object.values(this.getRoutes());
        const route = routes.find(r => utilities.equalsIgnoreCase(r.routeKey, httpService.getSubModuleName()));

        const roles = route && route.roles ? route.roles : [];

        // Administrator is implicit for all modules
        roles.push('Administrator');
        return roles;
    }

    buildNavPath(routeKey) {
        return this.getPageDetails(routeKey) ? `:${this.getPageDetails(routeKey).navKey}` : '';
    }
}
