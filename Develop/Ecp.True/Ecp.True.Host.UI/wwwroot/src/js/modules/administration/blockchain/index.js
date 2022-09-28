import { bootstrapService } from '../../../common/services/bootstrapService';
import routerConfig from './routerConfig';
import modalConfig from './modalConfig';
import { blockchain } from './reducers';

// Register modules
bootstrapService.initModule('blockchain', {
    routerConfig,
    modalConfig
});

// Register reducer
bootstrapService.registerReducer({ blockchain });
