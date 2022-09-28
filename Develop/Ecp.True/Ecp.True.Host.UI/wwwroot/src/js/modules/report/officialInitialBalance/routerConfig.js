import RouterConfigBase from '../../../common/router/routerConfig';
import { constants } from '../../../common/services/constants';
import OfficialInitialBalanceReport from './components/report.jsx';
import OfficialInitialBalanceFilter from './components/filter.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                filter: {
                    routeKey: 'manage',
                    component: OfficialInitialBalanceFilter,
                    roles: [
                        constants.Roles.Query,
                        constants.Roles.Chain
                    ],
                    details: {
                        navKey: 'executionId',
                        component: OfficialInitialBalanceReport
                    }
                }
            }
        };

        super('officialbalanceloaded', routeConfig);
    }
}
