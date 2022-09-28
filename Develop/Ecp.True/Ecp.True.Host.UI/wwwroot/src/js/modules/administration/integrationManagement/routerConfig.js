import RouterConfigBase from './../../../common/router/routerConfig.js';
import IntegrationManagementGrid from './components/integrationManagementGrid.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                manage: {
                    routeKey: 'manage',
                    component: IntegrationManagementGrid
                }
            }
        };

        super('integrationmanagement', routeConfig);
    }
}
