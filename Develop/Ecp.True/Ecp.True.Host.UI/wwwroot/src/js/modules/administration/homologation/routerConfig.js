import HomologationsGrid from './components/homologationsGrid.jsx';
import HomologationDetails from './components/homologationDetails.jsx';
import { constants } from './../../../common/services/constants.js';
import RouterConfigBase from './../../../common/router/routerConfig.js';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                homologations: {
                    routeKey: 'manage',
                    component: HomologationsGrid,
                    actions: [
                        {
                            title: 'createHomologation',
                            iconClass: 'fas fa-plus-square',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'modal',
                            key: 'createHomologation'
                        }
                    ],
                    details: {
                        navKey: 'homologationGroupId',
                        component: HomologationDetails,
                        actions: [
                            {
                                title: 'returnListing',
                                type: constants.RouterActions.Type.Link,
                                iconClass: 'fas fa-angle-left',
                                actionType: 'navigate',
                                route: 'homologations/manage'
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
                    component: HomologationDetails,
                    bcrumbsKey: 'createHomologation'
                }
            }
        };

        super('homologations', routeConfig);
    }
}
