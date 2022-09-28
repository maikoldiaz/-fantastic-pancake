import { bootstrapService } from '../../../common/services/bootstrapService';
import transformSettingsRouterConfig from './routerConfig';
import transformationSettingsModalConfig from './modalConfig';
import { transformSettings } from './reducers';

// Register modules
bootstrapService.initModule('transformSettings', {
    routerConfig: transformSettingsRouterConfig,
    modalConfig: transformationSettingsModalConfig
});


// Register reducer
bootstrapService.registerReducer({ transformSettings });
