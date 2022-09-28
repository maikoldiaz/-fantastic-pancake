import RouterConfigBase from '../../../common/router/routerConfig.js';
import { constants } from '../../../common/services/constants';
import OwnershipTickets from './components/ownershipTickets.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                ownership: {
                    routeKey: 'manage',
                    component: OwnershipTickets,
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query
                    ],
                    props: {
                        componentType: constants.TicketType.Ownership
                    },
                    actions: [
                        {
                            title: 'newBalance',
                            iconClass: 'fa fa-plus-square',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'modal',
                            key: 'ownershipCalculation'
                        }
                    ]
                }
            }
        };

        super('ownership', routeConfig);
    }
}
