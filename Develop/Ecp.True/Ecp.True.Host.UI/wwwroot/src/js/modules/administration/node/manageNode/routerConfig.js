import NodesGrid from './components/nodesGrid.jsx';
import NodePanel from './components/nodePanel.jsx';
import RouterConfigBase from '../../../../common/router/routerConfig.js';
import { constants } from '../../../../common/services/constants.js';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                nodes: {
                    routeKey: 'manage',
                    bcrumbsKey: '',
                    component: NodesGrid,
                    actions: [
                        {
                            title: 'search',
                            iconClass: 'fas fa-search',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'flyout',
                            key: 'nodesGridFilter'
                        },
                        {
                            title: 'createNode',
                            iconClass: 'fas fa-plus-square',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'navigate',
                            route: 'nodes/create'
                        }
                    ],
                    details: {
                        navKey: 'nodeId',
                        bcrumbsKey: 'editNode',
                        component: NodePanel,
                        mode: constants.Modes.Update,
                        actions: [
                            {
                                title: 'backToNode',
                                type: constants.RouterActions.Type.Link,
                                iconClass: 'fas fa-angle-left',
                                actionType: 'navigate',
                                route: 'nodes/manage'
                            },
                            {
                                title: 'submit',
                                type: constants.RouterActions.Type.Button,
                                actionType: 'custom'
                            }
                        ]
                    }
                },
                create: {
                    routeKey: 'create',
                    component: NodePanel,
                    bcrumbsKey: 'createNode',
                    mode: constants.Modes.Create,
                    actions: [
                        {
                            title: 'backToNode',
                            type: constants.RouterActions.Type.Link,
                            iconClass: 'fas fa-angle-left',
                            actionType: 'navigate',
                            route: 'nodes/manage'
                        },
                        {
                            title: 'submit',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'custom'
                        }
                    ],
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances
                    ]
                }
            }
        };

        super('nodes', routeConfig);
    }
}
