import { combineReducers } from 'redux';
import { bootstrapService } from '../../../common/services/bootstrapService';
import nodesRouterConfig from './manageNode/routerConfig';
import nodesModalConfig from './manageNode/modalConfig';
import attributesRouterConfig from './attributes/routerConfig';
import attributesModalConfig from './attributes/modalConfig';
import routerConfig from './nodeTags/routerConfig';
import modalConfig from './nodeTags/modalConfig';
import { attributes } from './attributes/reducers';
import { nodeTags } from './nodeTags/reducers';
import { manageNode } from './manageNode/reducers';

// Register modules

bootstrapService.initModule('nodes', {
    routerConfig: nodesRouterConfig,
    modalConfig: nodesModalConfig
});

bootstrapService.initModule('nodeAttributes', {
    routerConfig: attributesRouterConfig,
    modalConfig: attributesModalConfig
});

bootstrapService.initModule('nodeTags', {
    routerConfig,
    modalConfig
});

bootstrapService.initModule('nodeOwnershipRules', {
    routerConfig,
    modalConfig
});


// Register reducer
const node = combineReducers({ attributes, nodeTags, manageNode });
bootstrapService.registerReducer({ node });
