import RouterConfigBase from '../../../common/router/routerConfig';
import TicketsGrid from './components/ticketsGrid.jsx';
import CutOffWizard from './components/cutOffWizard.jsx';
import { constants } from './../../../common/services/constants';
import TicketDetail from './components/ticketDetail.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                ticket: {
                    routeKey: 'manage',
                    component: TicketsGrid,
                    actions: [
                        {
                            title: 'newCut',
                            iconClass: 'fa fa-plus-square',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'navigate',
                            route: 'cutoff/create'
                        }
                    ],
                    details: {
                        navKey: 'ticketId',
                        component: TicketDetail,
                        actions: [
                            {
                                title: 'returnListing',
                                type: constants.RouterActions.Type.Link,
                                iconClass: 'fas fa-angle-left',
                                actionType: 'navigate',
                                route: 'cutOff/manage'
                            }
                        ]
                    },
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query
                    ],
                    props: {
                        componentType: constants.TicketType.Cutoff
                    }
                },
                cutoff: {
                    routeKey: 'create',
                    component: CutOffWizard,
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances
                    ],
                    actions: [
                        {
                            title: 'returnListing',
                            type: constants.RouterActions.Type.Link,
                            iconClass: 'fas fa-angle-left',
                            actionType: 'navigate',
                            route: 'cutOff/manage'
                        }
                    ]
                }
            }
        };

        super('cutoff', routeConfig);
    }
}
