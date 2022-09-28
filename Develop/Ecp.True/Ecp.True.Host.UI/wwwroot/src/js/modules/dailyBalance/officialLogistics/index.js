import { bootstrapService } from '../../../common/services/bootstrapService';
import OfficialLogisticsRouterConfig from './routerConfig';
import OfficialLogisticsModalConfig from './modalConfig';

// Register modules
bootstrapService.initModule('officialLogistics', {
    routerConfig: OfficialLogisticsRouterConfig,
    modalConfig: OfficialLogisticsModalConfig
});
