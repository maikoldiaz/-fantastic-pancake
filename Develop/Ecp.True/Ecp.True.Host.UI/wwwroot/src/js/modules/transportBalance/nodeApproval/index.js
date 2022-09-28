import { bootstrapService } from '../../../common/services/bootstrapService';
import routerConfig from './routerConfig';

// Register modules
bootstrapService.initModule('nodeapproval', {
    routerConfig
});
