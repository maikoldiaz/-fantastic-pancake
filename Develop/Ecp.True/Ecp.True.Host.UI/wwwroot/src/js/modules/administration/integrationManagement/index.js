import { bootstrapService } from '../../../common/services/bootstrapService';
import integrationManagementRouterConfig from './routerConfig';
import integrationManagementModalConfig from './modalConfig';

// Register modules
bootstrapService.initModule('integrationmanagement', {
    routerConfig: integrationManagementRouterConfig,
    modalConfig: integrationManagementModalConfig
});
