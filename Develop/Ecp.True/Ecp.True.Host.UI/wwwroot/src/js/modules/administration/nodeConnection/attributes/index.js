import { combineReducers } from 'redux';
import { bootstrapService } from '../../../../common/services/bootstrapService';
import attributesRouterConfig from './routerConfig';
import attributesModalConfig from './modalConfig';
import { attributes, nodeCostCenters } from './reducers';

// Register modules
bootstrapService.initModule('connectionAttributes', {
    routerConfig: attributesRouterConfig,
    modalConfig: attributesModalConfig
});

// Register reducer
const nodeConnection = combineReducers({ attributes, nodeCostCenters });
bootstrapService.registerReducer({ nodeConnection });
