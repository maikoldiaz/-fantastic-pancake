import { bootstrapService } from '../../../common/services/bootstrapService';
import annulationsRouterConfig from './routerConfig';
import annulationsModalConfig from './modalConfig';
import { annulations } from './reducers';

// Register modules
bootstrapService.initModule('annulations', {
    routerConfig: annulationsRouterConfig,
    modalConfig: annulationsModalConfig
});

// Register reducer
bootstrapService.registerReducer({ annulations });
