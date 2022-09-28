import { bootstrapService } from '../../../common/services/bootstrapService';
import LogisticsRouterConfig from './routerConfig';
import LogisticsModalConfig from './modalConfig';
import { logistics } from './reducers.js';

// Register modules
bootstrapService.initModule('logistics', {
    routerConfig: LogisticsRouterConfig,
    modalConfig: LogisticsModalConfig
});

// Register reducer
bootstrapService.registerReducer({ logistics });
