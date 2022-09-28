import RouterConfigBase from '../../../common/router/routerConfig';
import BalanceControlChart from './components/filter.jsx';
import { constants } from '../../../common/services/constants';
import BalanceControl from './components/report.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                filter: {
                    routeKey: 'manage',
                    component: BalanceControlChart,
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query
                    ]
                },
                report: {
                    routeKey: 'view',
                    component: BalanceControl,
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query
                    ]
                }
            }
        };

        super('balanceControlChart', routeConfig);
    }
}
