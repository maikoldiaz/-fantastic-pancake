import { bootstrapService } from '../../../common/services/bootstrapService';
import RegisterDailyBalanceContractRouterConfig from './routerConfig';
import fileUploadModalConfig from './modalConfig';

// Register modules
bootstrapService.initModule('dailycontracts', {
    routerConfig: RegisterDailyBalanceContractRouterConfig,
    modalConfig: fileUploadModalConfig
});


