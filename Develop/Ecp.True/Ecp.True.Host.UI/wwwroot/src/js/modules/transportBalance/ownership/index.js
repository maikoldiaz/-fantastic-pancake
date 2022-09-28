import { bootstrapService } from '../../../common/services/bootstrapService';
import routerConfig from './routerConfig';
import modalConfig from './modalConfig';
import { ownership } from './reducers.js';

// Register modules
bootstrapService.initModule('ownership', {
    routerConfig,
    modalConfig
});

// Register reducer
bootstrapService.registerReducer({ ownership });
