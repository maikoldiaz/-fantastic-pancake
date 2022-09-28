import RouterConfigBase from '../../../common/router/routerConfig.js';
import { constants } from '../../../common/services/constants';
import OwnershipNodeGrid from './components/ownershipNodeGrid.jsx';
import OwnershipStatus from './components/ownershipStatus.jsx';
import OwnershipNodeNavigator from './components/ownershipNodeNavigator.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                ownershipnode: {
                    routeKey: 'manage',
                    component: OwnershipNodeGrid,
                    details: {
                        navKey: 'ticketNodeId',
                        component: OwnershipNodeNavigator,
                        actions: [
                            {
                                title: 'returnListing',
                                type: constants.RouterActions.Type.Link,
                                iconClass: 'fas fa-angle-left',
                                actionType: 'custom'
                            },
                            {
                                title: 'ownershipNodeActionBar',
                                type: constants.RouterActions.Type.Component,
                                component: OwnershipStatus
                            }
                        ]
                    },
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query,
                        constants.Roles.Programmer
                    ]
                }
            }
        };

        super('ownershipnode', routeConfig);
    }
}
