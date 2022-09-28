import { bootstrapService } from '../../../common/services/bootstrapService';
import productConfigurationRouterConfig from './routerConfig';
import productConfigurationModalConfig from './modalConfig';
import { products } from './reducers';

// Register modules
bootstrapService.initModule('productConfiguration', {
    routerConfig: productConfigurationRouterConfig,
    modalConfig: productConfigurationModalConfig
});

// Register reducer
bootstrapService.registerReducer({ products });
