import RouterConfigBase from '../../../common/router/routerConfig';
import NodeConfigurationReport from './components/filter.jsx';
import { constants } from '../../../common/services/constants';
import NodeConfiguration from './components/report.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                filter: {
                    routeKey: 'manage',
                    component: NodeConfigurationReport,
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query
                    ]
                },
                report: {
                    routeKey: 'view',
                    component: NodeConfiguration,
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query
                    ]
                }
            }
        };

        super('nodeConfigurationReport', routeConfig);
    }
}
