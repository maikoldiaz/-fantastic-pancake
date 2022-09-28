import { bootstrapService } from '../../../common/services/bootstrapService';
import routerConfig from './routerConfig';
import modalConfig from './modalConfig';

// Register modules
bootstrapService.initModule('transportContracts', {
    routerConfig,
    modalConfig
});


