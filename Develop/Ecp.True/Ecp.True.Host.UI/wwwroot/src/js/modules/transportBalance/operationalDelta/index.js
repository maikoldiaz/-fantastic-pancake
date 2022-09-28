import { bootstrapService } from '../../../common/services/bootstrapService';
import OperationalDeltaRouterConfig from './routerConfig';
import OperationalDeltaModalConfig from './modalConfig';
import { operationalDelta } from './reducers.js';

// Register modules
bootstrapService.initModule('deltaCalculation', {
    routerConfig: OperationalDeltaRouterConfig,
    modalConfig: OperationalDeltaModalConfig
});

// Register reducer
bootstrapService.registerReducer({ operationalDelta });

