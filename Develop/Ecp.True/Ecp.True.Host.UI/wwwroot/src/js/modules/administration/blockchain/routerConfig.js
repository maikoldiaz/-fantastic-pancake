import RouterConfigBase from '../../../common/router/routerConfig.js';
import { constants } from './../../../common/services/constants';
import BlockRangeWizard from './components/blockRangeWizard.jsx';
import TransactionsGrid from './components/transactionsGrid.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                transactions: {
                    routeKey: 'manage',
                    component: TransactionsGrid,
                    actions: [
                        {
                            title: 'search',
                            type: constants.RouterActions.Type.Dropdown,
                            name: 'blockchainSearch',
                            options: [{
                                title: 'transactions',
                                iconClass: 'fas fa-search',
                                type: constants.RouterActions.Type.Button,
                                actionType: 'modal',
                                key: 'blockWizard'
                            },
                            {
                                title: 'events',
                                iconClass: 'fas fa-search',
                                type: constants.RouterActions.Type.Button,
                                actionType: 'navigate',
                                key: 'blockRangeWizard',
                                route: 'blockchain/events'
                            }]
                        }
                    ],
                    roles: [
                        constants.Roles.Auditor
                    ]
                },
                nodeRules: {
                    routeKey: 'events',
                    bcrumbsKey: 'events',
                    component: BlockRangeWizard,
                    actions: [
                        {
                            title: 'returnListing',
                            type: constants.RouterActions.Type.Link,
                            iconClass: 'fas fa-angle-left',
                            actionType: 'navigate',
                            route: 'blockchain/manage'
                        }
                    ]
                }
            }
        };

        super('blockchain', routeConfig);
    }
}
