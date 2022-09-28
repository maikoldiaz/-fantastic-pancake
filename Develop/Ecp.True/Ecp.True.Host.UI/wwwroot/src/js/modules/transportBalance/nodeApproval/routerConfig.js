import RouterConfigBase from '../../../common/router/routerConfig';
import NodeApproval from './components/nodeApproval.jsx';
import { constants } from '../../../common/services/constants.js';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                attributes: {
                    routeKey: 'manage',
                    component: NodeApproval,
                    roles: [
                        constants.Roles.Approver
                    ]
                }
            }
        };

        super('nodeapproval', routeConfig);
    }
}
