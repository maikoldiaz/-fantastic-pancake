import RouterConfigBase from '../../../common/router/routerConfig';
import { constants } from '../../../common/services/constants';
import SendToSapWizard from './components/sendToSapWizard.jsx';
import SendToSapDetail from './components/sendToSapDetail.jsx';
import SentToSapTicketsGrid from './components/sentToSapTicketsGrid.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                sentToSap: {
                    routeKey: 'manage',
                    component: SentToSapTicketsGrid,
                    bcrumbsKey: '',
                    actions: [
                        {
                            title: 'newSendToSap',
                            iconClass: '',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'navigate',
                            route: 'senttosap/create'
                        }
                    ],
                    details: {
                        navKey: 'ticketId',
                        bcrumbsKey: '',
                        component: SendToSapDetail,
                        mode: constants.Modes.Update,
                        actions: [
                            {
                                title: 'returnListing',
                                type: constants.RouterActions.Type.Link,
                                iconClass: 'fas fa-angle-left',
                                actionType: 'navigate',
                                route: 'senttosap/manage'
                            },
                            {
                                title: 'forwardToSap',
                                class: 'ep-btn__txt--tt',
                                type: constants.RouterActions.Type.Button,
                                actionType: 'custom'
                            }
                        ]
                    },
                    roles: [
                        constants.Roles.Administrator,
                        constants.Roles.Chain
                    ],
                    props: {
                        componentType: constants.TicketType.LogisticMovements
                    }
                },
                sentToSapCreate: {
                    routeKey: 'create',
                    component: SendToSapWizard,
                    bcrumbsKey: 'createSendToSap',
                    roles: [
                        constants.Roles.Administrator,
                        constants.Roles.Chain
                    ],
                    props: {
                        initSapFormTicket: {
                            validateRangeDate: true,
                            dateRange: 60,
                            officialPeriodsYearsRange: 2
                        }
                    }
                }
            }
        };

        super('sentToSap', routeConfig);
    }
}
