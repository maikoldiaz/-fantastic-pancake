import RouterConfigBase from '../../../common/router/routerConfig';
import NodeStatusReport from './components/filter.jsx';
import { constants } from '../../../common/services/constants';
import NodeStatus from './components/report.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                filter: {
                    routeKey: 'manage',
                    component: NodeStatusReport,
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query
                    ]
                },
                report: {
                    routeKey: 'view',
                    component: NodeStatus,
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query
                    ]
                }
            }
        };

        super('nodeStatusReport', routeConfig);
    }
}
