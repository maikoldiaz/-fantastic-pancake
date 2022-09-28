import { constants } from '../../../common/services/constants.js';
import RouterConfigBase from '../../../common/router/routerConfig.js';
import ContractsGrid from './components/contractsGrid.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                attributes: {
                    routeKey: 'manage',
                    component: ContractsGrid,
                    actions: [
                        {
                            title: 'loadNew',
                            iconClass: 'fas fa-file-upload',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'modal',
                            key: 'uploadFileModal'
                        }
                    ],
                    props: {
                        systemType: constants.SystemType.CONTRACT
                    },
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances
                    ]
                }
            }
        };

        super('registerContracts', routeConfig);
    }
}
