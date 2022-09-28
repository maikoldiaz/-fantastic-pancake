import { bootstrapService } from '../../../common/services/bootstrapService';
import routerConfig from './routerConfig';
import modalConfig from './modalConfig';
import { controlException } from './reducers.js';
import { combineReducers } from 'redux';

// Register modules
bootstrapService.initModule('exceptions', {
    routerConfig,
    modalConfig
});

// Register reducer
const controlexceptionReducer = combineReducers({ controlException });
bootstrapService.registerReducer({ controlexception: controlexceptionReducer });
