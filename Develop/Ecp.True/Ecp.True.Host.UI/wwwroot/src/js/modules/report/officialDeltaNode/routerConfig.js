import RouterConfigBase from '../../../common/router/routerConfig';
import { constants } from '../../../common/services/constants';
import OfficialDeltaReport from './components/report.jsx';
import OfficialDeltaFilter from './components/filter.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                filter: {
                    routeKey: 'manage',
                    component: OfficialDeltaFilter,
                    roles: [
                        constants.Roles.Query,
                        constants.Roles.Chain
                    ],
                    details: {
                        navKey: 'deltaNodeId',
                        component: OfficialDeltaReport,
                        actions: [
                            {
                                title: 'returnDeltaNodeListing',
                                type: constants.RouterActions.Type.Link,
                                iconClass: 'fas fa-angle-left',
                                actionType: 'custom',
                                key: 'returnDeltaNodeListing',
                                hide: true
                            },
                            {
                                title: 'addManualMovementsDeltaNode',
                                iconClass: '',
                                classButton: 'ep-btn--sec',
                                type: constants.RouterActions.Type.Button,
                                actionType: 'custom',
                                key: 'addManualMovementsDeltaNode',
                                hide: true
                            },
                            {
                                title: 'submitForApproval',
                                iconClass: 'fas fa-check',
                                type: constants.RouterActions.Type.Button,
                                actionType: 'custom',
                                key: 'submitForApproval',
                                hide: true
                            },
                            {
                                title: 'submitForReopen',
                                iconClass: 'fas fa-check',
                                type: constants.RouterActions.Type.Button,
                                actionType: 'custom',
                                key: 'submitForReopen',
                                hide: true
                            }
                        ],
                        roles: [
                            constants.Roles.Query,
                            constants.Roles.Chain
                        ]
                    }
                }
            }
        };

        super('officialdeltanode', routeConfig);
    }
}
