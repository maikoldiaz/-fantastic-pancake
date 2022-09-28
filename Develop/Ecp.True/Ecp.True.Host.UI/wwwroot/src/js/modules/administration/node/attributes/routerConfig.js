import AttributesGrid from './components/nodeAttributes.jsx';
import NodeDetails from './components/nodeDetails.jsx';
import { constants } from '../../../../common/services/constants.js';
import RouterConfigBase from '../../../../common/router/routerConfig.js';
import nodeOwnershipRules from './components/nodeOwnershipRules.jsx';
import nodeProductRules from './components/nodeProductRules.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                attributes: {
                    routeKey: 'manage',
                    component: AttributesGrid,
                    actions: [
                        {
                            title: 'changeOwnershipRules',
                            type: constants.RouterActions.Type.Dropdown,
                            name: 'rules',
                            options: [{
                                title: 'massiveByNode',
                                iconClass: 'fas fa-project-diagram',
                                actionType: 'navigate',
                                key: 'massiveByNode',
                                route: 'nodeAttributes/bulkUpdate'
                            },
                            {
                                title: 'massiveByNodeProducts',
                                iconClass: 'fas fa-network-wired',
                                actionType: 'navigate',
                                key: 'massiveByNodeProducts',
                                route: 'nodeAttributes/productsBulkUpdate'
                            }]
                        },
                        {
                            title: 'search',
                            iconClass: 'fas fa-search',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'flyout',
                            key: 'nodeAttributes'
                        }
                    ],
                    details: {
                        navKey: 'nodeId',
                        component: NodeDetails,
                        actions: [
                            {
                                title: 'returnListing',
                                type: constants.RouterActions.Type.Link,
                                iconClass: 'fas fa-angle-left',
                                actionType: 'navigate',
                                route: 'nodeAttributes/manage'
                            }
                        ]
                    }
                },
                nodeRules: {
                    routeKey: 'bulkUpdate',
                    bcrumbsKey: 'nodeRules',
                    component: nodeOwnershipRules,
                    actions: [
                        {
                            title: 'changeStrategy',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'custom',
                            key: 'changeStrategy'
                        }
                    ]
                },
                productRules: {
                    routeKey: 'productsBulkUpdate',
                    bcrumbsKey: 'productRules',
                    component: nodeProductRules,
                    actions: [
                        {
                            title: 'changeStrategy',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'custom',
                            key: 'changeStrategy'
                        }
                    ]
                }
            }
        };

        super('nodeattributes', routeConfig);
    }
}
