import RouterConfigBase from '../../../../common/router/routerConfig.js';
import GraphicalNetworkFilter from './components/graphicalNetworkFilter.jsx';
import { constants } from '../../../../common/services/constants.js';
import GraphicalConnection from './components/graphicalConnection.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                graphicConfigurationNetwork: {
                    routeKey: 'filter',
                    component: GraphicalNetworkFilter
                },
                nodeGraphicalConnection: {
                    routeKey: 'manage',
                    component: GraphicalConnection,
                    mode: constants.Modes.Create,
                    actions: [
                        {
                            title: 'selectCriteria',
                            type: constants.RouterActions.Type.Link,
                            iconClass: 'fas fa-angle-left',
                            actionType: 'navigate',
                            route: 'graphicConfigurationNetwork/filter'
                        }
                    ]
                }
            }
        };

        super('graphicConfigurationNetwork', routeConfig);
    }
}
