import { bootstrapService } from '../../../common/services/bootstrapService';

import operativeBalanceReportRouterConfig from './routerConfig';
import { cutOffReport } from './../../report/cutOff/reducers';

bootstrapService.initModule('operativeBalanceReport', {
    routerConfig: operativeBalanceReportRouterConfig,
    modalConfig: {}
});

// Register reducer
bootstrapService.registerReducer({ report: cutOffReport });
