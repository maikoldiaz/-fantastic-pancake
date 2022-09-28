import RouterConfigBase from '../../../common/router/routerConfig.js';
import ErrorsGrid from './components/errorsGrid.jsx';
import ErrorNavigator from './components/errorNavigator.jsx';
import { constants } from './../../../common/services/constants';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                exceptions: {
                    routeKey: 'manage',
                    component: ErrorsGrid,
                    actions: [
                        {
                            title: 'retryRecord',
                            iconClass: 'fas fa-redo',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'custom'
                        },
                        {
                            title: 'discardException',
                            iconClass: 'fas fa-trash',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'custom'
                        }
                    ],
                    details: {
                        navKey: 'errorId',
                        component: ErrorNavigator,
                        actions: [
                            {
                                title: 'returnListing',
                                type: constants.RouterActions.Type.Link,
                                iconClass: 'fas fa-angle-left',
                                actionType: 'custom'
                            },
                            {
                                title: 'retryRecord',
                                iconClass: 'fas fa-redo',
                                type: constants.RouterActions.Type.Button,
                                actionType: 'custom'
                            },
                            {
                                title: 'discardException',
                                iconClass: 'fas fa-trash',
                                type: constants.RouterActions.Type.Button,
                                actionType: 'custom'
                            }
                        ]
                    },
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances
                    ]
                }
            }
        };

        super('exceptions', routeConfig);
    }
}
