import RouterConfigBase from '../../../common/router/routerConfig.js';
import { constants } from '../../../common/services/constants';
import NodeApproval from '../../transportBalance/nodeApproval/components/nodeApproval.jsx';

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

        super('officialnodeapproval', routeConfig);
    }
}
