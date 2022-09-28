import HomePage from './components/homePage.jsx';
import RouterConfigBase from './../../../common/router/routerConfig';
import { constants } from '../../../common/services/constants.js';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                home: {
                    routeKey: 'index',
                    component: HomePage,
                    navigation: false,
                    scenario: 'home',
                    roles: [constants.Roles.Anonymous]
                }
            }
        };

        super('home', routeConfig);
    }
}
