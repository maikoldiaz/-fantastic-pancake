import RouterConfigBase from '../../../common/router/routerConfig.js';
import AnalyticalModel from './components/analyticalModel.jsx';
import { constants } from '../../../common/services/constants.js';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                analyticalModel: {
                    preventNavigation: true,
                    routeKey: 'view',
                    component: AnalyticalModel,
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Query
                    ]
                }
            }
        };

        super('analyticalModel', routeConfig);
    }
}
