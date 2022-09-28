import RouterConfigBase from '../../../common/router/routerConfig.js';
import { constants } from '../../../common/services/constants';
import OfficialLogisticsGrid from '../officialLogistics/components/officialLogisticsGrid.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                officialLogistics: {
                    routeKey: 'manage',
                    title: 'manageElements',
                    component: OfficialLogisticsGrid,
                    actions: [
                        {
                            title: 'createOfficialLogistics',
                            iconClass: 'fas fa-plus-square',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'modal',
                            key: 'createOfficialLogistics'
                        }
                    ],
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Chain
                    ],
                    props: {
                        componentType: constants.TicketType.OfficialLogistics
                    }
                }
            }
        };

        super('officiallogistics', routeConfig);
    }
}
