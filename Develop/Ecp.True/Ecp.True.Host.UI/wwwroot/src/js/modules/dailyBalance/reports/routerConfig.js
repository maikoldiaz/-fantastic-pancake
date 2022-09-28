import OfficialReportsGrid from './components/officialReportsGrid.jsx';
import RouterConfigBase from '../../../common/router/routerConfig.js';
import { constants } from '../../../common/services/constants';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                attributes: {
                    routeKey: 'manage',
                    component: OfficialReportsGrid,
                    props: {
                        scenarioType: constants.ScenarioType.Official,
                        generalReportTypes: [
                            constants.ReportTypeName.SapBalance,
                            constants.ReportTypeName.UserRolesAndPermissions
                        ]
                    },
                    roles: [
                        constants.Roles.Chain,
                        constants.Roles.Query,
                        constants.Roles.Auditor,
                        constants.Roles.Programmer
                    ]
                }
            }
        };

        super('generatedSupplyChainReport', routeConfig);
    }
}
