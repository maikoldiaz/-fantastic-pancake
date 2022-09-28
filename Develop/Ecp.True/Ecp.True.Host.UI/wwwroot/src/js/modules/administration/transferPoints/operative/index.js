import { bootstrapService } from '../../../../common/services/bootstrapService';
import routerConfig from './routerConfig';
import modalConfig from './modalConfig';
import { transferPointsOperational } from './reducers';

// Register modules
bootstrapService.initModule('transferPointsOperational', {
    routerConfig,
    modalConfig
});

// Register reducer
bootstrapService.registerReducer({ transferPointsOperational });
