import RouterConfigBase from '../../../common/router/routerConfig';
import CutOffReport from './../../report/cutOff/components/report.jsx';
import Filter from './components/filter.jsx';
import { constants } from '../../../common/services/constants';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                filter: {
                    routeKey: 'manage',
                    component: Filter,
                    roles: [
                        constants.Roles.Chain,
                        constants.Roles.Programmer,
                        constants.Roles.Query
                    ],
                    props: {
                        type: constants.ReportType.OperativeBalance
                    },
                    details: {
                        navKey: 'reportId',
                        component: CutOffReport
                    }
                }
            }
        };

        super('cutoffReport', routeConfig);
    }
}
