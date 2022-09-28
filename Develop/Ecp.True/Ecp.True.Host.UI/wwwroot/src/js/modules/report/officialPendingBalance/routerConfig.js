import RouterConfigBase from '../../../common/router/routerConfig';
import { constants } from '../../../common/services/constants';
import OfficialPendingBalanceReport from './components/report.jsx';
import OfficialPendingBalanceFilter from './components/filter.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                filter: {
                    routeKey: 'manage',
                    component: OfficialPendingBalanceFilter,
                    roles: [
                        constants.Roles.Query,
                        constants.Roles.Chain
                    ]
                },
                report: {
                    routeKey: 'view',
                    component: OfficialPendingBalanceReport,
                    roles: [
                        constants.Roles.Query,
                        constants.Roles.Chain
                    ]
                }

            }
        };

        super('previousperiodpendingofficial', routeConfig);
    }
}
