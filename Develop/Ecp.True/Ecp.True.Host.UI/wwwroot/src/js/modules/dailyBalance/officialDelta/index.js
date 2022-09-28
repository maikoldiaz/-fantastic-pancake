import { bootstrapService } from '../../../common/services/bootstrapService';
import RouterConfig from './routerConfig';
import ModalConfig from './modalConfig';
import { officialDelta } from './reducers';

// Register modules
bootstrapService.initModule('officialDelta', {
    routerConfig: RouterConfig,
    modalConfig: ModalConfig
});

// Register reducer
bootstrapService.registerReducer({ officialDelta });

