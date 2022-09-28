import RouterConfigBase from '../../../../common/router/routerConfig.js';
import TransferPointsLogisticGrid from './components/grid.jsx';
import { constants } from '../../../../common/services/constants.js';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                transferPointsLogistics: {
                    routeKey: 'manage',
                    component: TransferPointsLogisticGrid,
                    actions: [
                        {
                            title: 'createTransferPoint',
                            iconClass: 'fas fa-plus-square',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'modal',
                            key: 'create'
                        }
                    ],
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances
                    ]
                }
            }
        };

        super('transferPointsLogistics', routeConfig);
    }
}
