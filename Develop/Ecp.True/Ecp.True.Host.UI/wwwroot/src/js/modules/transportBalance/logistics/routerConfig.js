import RouterConfigBase from '../../../common/router/routerConfig.js';
import LogisticsGrid from './components/logisticsGrid.jsx';
import { constants } from './../../../common/services/constants';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                attributes: {
                    routeKey: 'manage',
                    title: 'manageElements',
                    component: LogisticsGrid,
                    actions: [
                        {
                            title: 'createLogistics',
                            iconClass: 'fas fa-plus-square',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'modal',
                            key: 'createLogistics'
                        }
                    ],
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query
                    ],
                    props: {
                        componentType: constants.TicketType.Logistics
                    }
                }
            }
        };

        super('logistics', routeConfig);
    }
}
