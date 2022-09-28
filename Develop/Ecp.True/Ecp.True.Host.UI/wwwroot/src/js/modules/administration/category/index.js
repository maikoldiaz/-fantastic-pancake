import { bootstrapService } from '../../../common/services/bootstrapService';
import categoryRouterConfig from './manageCategory/routerConfig';
import categoryModalConfig from './manageCategory/modalConfig';
import elementRouterConfig from './manageElement/routerConfig';
import elementModalConfig from './manageElement/modalConfig';
import { manageCategory } from './manageCategory/reducers';
import { manageElement } from './manageElement/reducers';
import { combineReducers } from 'redux';

// Register modules
bootstrapService.initModule('category', {
    routerConfig: categoryRouterConfig,
    modalConfig: categoryModalConfig
});

bootstrapService.initModule('categoryElements', {
    routerConfig: elementRouterConfig,
    modalConfig: elementModalConfig
});

// Register reducer
const category = combineReducers({ manageCategory, manageElement });
bootstrapService.registerReducer({ category });

