import RouterConfigBase from '../../../common/router/routerConfig.js';
import OfficialCustodyTransferPointsReport from './components/report.jsx';
import { constants } from '../../../common/services/constants.js';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                officialcustodytransferpoints: {
                    preventNavigation: true,
                    routeKey: 'view',
                    component: OfficialCustodyTransferPointsReport,
                    roles: [
                        constants.Roles.Query
                    ]
                }
            }
        };

        super('officialcustodytransferpoints', routeConfig);
    }
}
