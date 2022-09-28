import { bootstrapService } from '../../../common/services/bootstrapService';
import routerConfig from './routerConfig';
import modalConfig from './modalConfig';
import { fileRegistration } from './reducers';
import { combineReducers } from 'redux';

// Register modules
bootstrapService.initModule('fileupload', {
    routerConfig,
    modalConfig
});

// Register reducer
const messages = combineReducers({ fileRegistration });
bootstrapService.registerReducer({ messages });
