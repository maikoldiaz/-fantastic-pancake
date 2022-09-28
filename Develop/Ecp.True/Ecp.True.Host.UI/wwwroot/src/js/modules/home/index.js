import { bootstrapService } from '../../common/services/bootstrapService';
import homeRouterConfig from './home/routerConfig';

// Register modules
bootstrapService.initModule('home', {
    routerConfig: homeRouterConfig,
    modalConfig: {}
});
