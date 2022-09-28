import RouterConfigBase from '../../../common/router/routerConfig';
import UserRolesAndPermissionsReport from './components/report.jsx';
import UserRolesAndPermissionsFilter from './components/filter.jsx';
import { constants } from '../../../common/services/constants';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                filter: {
                    routeKey: 'manage',
                    component: UserRolesAndPermissionsFilter,
                    roles: [
                        constants.Roles.Auditor
                    ],
                    details: {
                        navKey: 'executionId',
                        component: UserRolesAndPermissionsReport
                    },
                    props: {
                        type: constants.ReportType.UserRolesAndPermissions
                    }
                }
            }
        };

        super('userRolesAndPermissions', routeConfig);
    }
}
