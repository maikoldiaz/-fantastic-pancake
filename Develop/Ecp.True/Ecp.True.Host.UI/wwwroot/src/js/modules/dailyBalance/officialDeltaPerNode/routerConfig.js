import RouterConfigBase from '../../../common/router/routerConfig.js';
import { constants } from '../../../common/services/constants';
import NodesGrid from './components/nodesGrid.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                officialdeltapernode: {
                    routeKey: 'manage',
                    component: NodesGrid,
                    details: {
                        navKey: 'ticketId',
                        component: NodesGrid,
                        actions: [
                            {
                                title: 'returnListing',
                                type: constants.RouterActions.Type.Link,
                                iconClass: 'fas fa-angle-left',
                                actionType: 'custom'
                            }
                        ]
                    },
                    roles: [
                        constants.Roles.Chain
                    ]
                }
            }
        };

        super('officialdeltapernode', routeConfig);
    }
}
