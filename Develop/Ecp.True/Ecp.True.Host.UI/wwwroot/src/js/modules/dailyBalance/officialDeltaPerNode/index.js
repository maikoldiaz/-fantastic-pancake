import { bootstrapService } from '../../../common/services/bootstrapService';
import OfficialDeltaPerNodeRouterConfig from './routerConfig';
import OfficialDeltaPerNodeModalConfig from './modalConfig';
import { officialDeltaPerNode } from './reducers';

// Register modules
bootstrapService.initModule('officialdeltapernode', {
    routerConfig: OfficialDeltaPerNodeRouterConfig,
    modalConfig: OfficialDeltaPerNodeModalConfig
});

bootstrapService.registerReducer({ officialDeltaPerNode });

