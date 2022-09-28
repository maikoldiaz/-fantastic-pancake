import ReportsGrid from './components/reportsGrid.jsx';
import { constants } from '../../../common/services/constants';
import RouterConfigBase from '../../../common/router/routerConfig';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                reports: {
                    routeKey: 'manage',
                    component: ReportsGrid,
                    props: {
                        scenarioType: constants.ScenarioType.Operational,
                        exclusionReportTypes: [
                            constants.ReportTypeName.SapBalance,
                            constants.ReportTypeName.UserRolesAndPermissions
                        ]
                    },
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query,
                        constants.Roles.Auditor,
                        constants.Roles.Programmer
                    ]
                }
            }
        };

        super('generatedreport', routeConfig);
    }
}
