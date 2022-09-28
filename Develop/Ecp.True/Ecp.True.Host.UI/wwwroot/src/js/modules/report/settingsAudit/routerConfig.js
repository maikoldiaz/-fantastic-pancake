import RouterConfigBase from '../../../common/router/routerConfig';
import SettingsAuditReport from './components/filter.jsx';
import { constants } from '../../../common/services/constants';
import SettingsAudit from './components/report.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                filter: {
                    routeKey: 'manage',
                    component: SettingsAuditReport,
                    roles: [
                        constants.Roles.Auditor
                    ]
                },
                report: {
                    routeKey: 'view',
                    component: SettingsAudit,
                    actions: [
                        {
                            title: 'backToSettingsAudit',
                            type: constants.RouterActions.Type.Link,
                            iconClass: 'fas fa-angle-left',
                            actionType: 'custom'
                        }
                    ],
                    roles: [
                        constants.Roles.Auditor
                    ]
                }
            }
        };

        super('settingsAudit', routeConfig);
    }
}
