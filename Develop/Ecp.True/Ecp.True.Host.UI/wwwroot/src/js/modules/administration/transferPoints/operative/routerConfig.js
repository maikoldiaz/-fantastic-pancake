import RouterConfigBase from '../../../../common/router/routerConfig';
import TransferPointsOperationalGrid from './components/grid.jsx';
import { constants } from '../../../../common/services/constants';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                transferPointsOperational: {
                    routeKey: 'manage',
                    component: TransferPointsOperationalGrid,
                    actions: [
                        {
                            title: 'createTransferPoint',
                            iconClass: 'fas fa-plus-square',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'modal',
                            key: 'createTransferPointOperational',
                            mode: constants.Modes.Create
                        }
                    ],
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances
                    ]
                }
            }
        };

        super('transferPointsOperational', routeConfig);
    }
}
