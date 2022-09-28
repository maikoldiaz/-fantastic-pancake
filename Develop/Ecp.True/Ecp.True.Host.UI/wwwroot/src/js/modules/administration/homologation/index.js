import { bootstrapService } from '../../../common/services/bootstrapService';
import homologationsRouterConfig from './routerConfig';
import homologationsModalConfig from './modalConfig';
import { homologations } from './reducers';

// Register modules

bootstrapService.initModule('homologations', {
    routerConfig: homologationsRouterConfig,
    modalConfig: homologationsModalConfig
});

// Register reducer
bootstrapService.registerReducer({ homologations });
