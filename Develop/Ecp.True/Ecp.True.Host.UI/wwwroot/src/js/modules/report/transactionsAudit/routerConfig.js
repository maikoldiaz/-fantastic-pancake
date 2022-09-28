import RouterConfigBase from '../../../common/router/routerConfig';
import { constants } from '../../../common/services/constants';
import TransactionsAudit from './components/report.jsx';
import TransactionsAuditReport from './components/filter.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                filter: {
                    routeKey: 'manage',
                    component: TransactionsAuditReport,
                    roles: [
                        constants.Roles.Auditor
                    ]
                },
                report: {
                    routeKey: 'view',
                    component: TransactionsAudit,
                    actions: [
                        {
                            title: 'backToTransactionsAudit',
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

        super('transactionsAudit', routeConfig);
    }
}
