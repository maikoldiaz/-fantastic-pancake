import RouterConfigBase from '../../../common/router/routerConfig';
import SentToSapReport from './components/report.jsx';
import SentToSapReportFilter from './components/filter.jsx';
import { constants } from '../../../common/services/constants';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                filter: {
                    routeKey: 'manage',
                    component: SentToSapReportFilter,
                    roles: [
                        constants.Roles.Chain,
                        constants.Roles.Query
                    ],
                    details: {
                        navKey: 'executionId',
                        component: SentToSapReport
                    },
                    props: {
                        type: constants.ReportType.SapBalance,
                        officialPeriodsYearsRange: 5
                    }
                }
            }
        };

        super('sentToSapReport', routeConfig);
    }
}
