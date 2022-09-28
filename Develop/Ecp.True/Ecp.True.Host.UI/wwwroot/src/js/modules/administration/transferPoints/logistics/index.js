import { bootstrapService } from '../../../../common/services/bootstrapService';
import routerConfig from './routerConfig';
import modalConfig from './modalConfig';
import { transferPointsLogistics } from './reducers';

// Register modules
bootstrapService.initModule('transferPointsLogistics', {
    routerConfig,
    modalConfig
});

// Register reducer
bootstrapService.registerReducer({ transferPointsLogistics });

