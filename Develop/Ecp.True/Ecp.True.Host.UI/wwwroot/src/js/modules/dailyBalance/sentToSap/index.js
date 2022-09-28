import { bootstrapService } from '../../../common/services/bootstrapService';
import { sendToSap } from './reducers';
import RouterConfig from './routerConfig';
import ModalConfig from './modalConfig';

// Register modules
bootstrapService.initModule('SentToSap', {
    routerConfig: RouterConfig,
    modalConfig: ModalConfig
});

// Register reducer
bootstrapService.registerReducer({ sendToSap });
