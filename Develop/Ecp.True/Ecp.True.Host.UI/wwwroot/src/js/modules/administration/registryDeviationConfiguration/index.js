import { bootstrapService } from '../../../common/services/bootstrapService';
import { registryDeviation } from './reducers';
import RouterConfig from './routerConfig';

// Register modules
bootstrapService.initModule('registrydeviationconfiguration', {
    routerConfig: RouterConfig
});

// Register reducer
bootstrapService.registerReducer({ registryDeviation });
