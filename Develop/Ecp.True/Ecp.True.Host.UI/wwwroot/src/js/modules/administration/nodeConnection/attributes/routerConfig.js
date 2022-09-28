import ConnectionDetails from './components/connectionDetails.jsx';
import { constants } from '../../../../common/services/constants.js';
import RouterConfigBase from '../../../../common/router/routerConfig.js';
import connectionProductsRules from './components/connectionProductRules.jsx';
import AssignmentCostCenter from './components/costCenter/assignmentCostCenter.jsx';
import connectionAttributesPanel from './components/connectionAttributesPanel.jsx';
import ConnectionAttributesNodes from './components/connectionAttributesNodes.jsx';


export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                attributes: {
                    routeKey: 'manage',
                    component: connectionAttributesPanel,
                    actions: [
                    ],
                    details: {
                        navKey: 'nodeConnectionId',
                        component: ConnectionDetails,
                        actions: [
                            {
                                title: 'returnListing',
                                type: constants.RouterActions.Type.Link,
                                iconClass: 'fas fa-angle-left',
                                actionType: 'navigate',
                                route: 'connectionAttributes/manage'
                            }
                        ]
                    }
                },
                products: {
                    routeKey: 'bulkUpdate',
                    bcrumbsKey: 'nodeRules',
                    component: connectionProductsRules,
                    actions: [
                        {
                            title: 'changeStrategy',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'custom',
                            key: 'changeStrategy'
                        }
                    ]
                },
                costcenterassignment: {
                    routeKey: 'assigncostcenter',
                    bcrumbsKey: 'assignCostCenter',
                    actionType: 'navigate',
                    component: AssignmentCostCenter
                },
                createAttributesNodes: {
                    routeKey: 'createattributesnodes',
                    actionType: 'navigate',
                    component: ConnectionAttributesNodes
                }
            }
        };

        super('connectionattributes', routeConfig);
    }
}
