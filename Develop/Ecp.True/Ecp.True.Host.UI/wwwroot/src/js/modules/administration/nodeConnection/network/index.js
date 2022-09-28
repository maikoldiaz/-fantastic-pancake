import { bootstrapService } from '../../../../common/services/bootstrapService';
import routerConfig from './routerConfig';
import modalConfig from './modalConfig';
import { nodeGraphicalConnection } from './reducers';

// Register modules
bootstrapService.initModule('graphicConfigurationNetwork', {
    routerConfig,
    modalConfig
});

// Register reducer
bootstrapService.registerReducer({ nodeGraphicalConnection });
