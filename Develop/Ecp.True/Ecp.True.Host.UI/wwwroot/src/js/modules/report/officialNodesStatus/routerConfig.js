import RouterConfigBase from '../../../common/router/routerConfig';
import { constants } from '../../../common/services/constants';
import OfficialNodesStatusReportFilter from './components/filter.jsx';
import OfficialNodesStatusReport from './components/report.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                filter: {
                    routeKey: 'manage',
                    component: OfficialNodesStatusReportFilter,
                    bcrumbsKey: '',
                    roles: [
                        constants.Roles.Chain,
                        constants.Roles.Query
                    ],
                    props: {
                        type: constants.ReportType.officialNodeStatusReport,
                        officialPeriodsYearsRange: 5
                    }
                },
                report: {
                    routeKey: 'view',
                    component: OfficialNodesStatusReport,
                    bcrumbsKey: '',
                    roles: [
                        constants.Roles.Chain,
                        constants.Roles.Query
                    ]
                }
            }
        };

        super('OfficialNodeStatusReport', routeConfig);
    }
}
