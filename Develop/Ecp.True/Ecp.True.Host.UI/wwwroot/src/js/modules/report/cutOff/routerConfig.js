import RouterConfigBase from '../../../common/router/routerConfig';
import CutOffReport from './components/report.jsx';
import CutoffReportFilter from './components/filter.jsx';
import { constants } from '../../../common/services/constants';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                filter: {
                    routeKey: 'manage',
                    component: CutoffReportFilter,
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query,
                        constants.Roles.Programmer
                    ],
                    details: {
                        navKey: 'reportId',
                        component: CutOffReport
                    },
                    props: {
                        type: constants.ReportType.BeforeCutOff
                    }
                }
            }
        };

        super('cutoffReport', routeConfig);
    }
}
