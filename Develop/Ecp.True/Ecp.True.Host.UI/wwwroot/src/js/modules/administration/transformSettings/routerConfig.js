import TransformSettingsPanel from './components/transformSettingsPanel.jsx';
import RouterConfigBase from '../../../common/router/routerConfig';
import { constants } from '../../../common/services/constants';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                transformSettings: {
                    routeKey: 'manage',
                    component: TransformSettingsPanel,
                    actions: [
                        {
                            title: 'transformation',
                            iconClass: 'fas fa-plus-square',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'custom',
                            key: 'transformation'
                        }
                    ],
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances
                    ]
                }
            }
        };

        super('transformSettings', routeConfig);
    }
}
