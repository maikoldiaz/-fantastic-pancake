import RouterConfigBase from '../../../common/router/routerConfig';
import EventContractReport from './components/filter.jsx';
import { constants } from '../../../common/services/constants';
import EventContract from './components/report.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                filter: {
                    routeKey: 'manage',
                    component: EventContractReport,
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query,
                        constants.Roles.Programmer
                    ]
                },
                report: {
                    routeKey: 'view',
                    component: EventContract,
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query,
                        constants.Roles.Programmer
                    ]
                }
            }
        };

        super('eventContractReport', routeConfig);
    }
}
