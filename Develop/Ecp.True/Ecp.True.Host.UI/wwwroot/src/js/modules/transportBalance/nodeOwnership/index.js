import { combineReducers } from 'redux';
import { bootstrapService } from '../../../common/services/bootstrapService';
import routerConfig from './routerConfig';
import modalConfig from './modalConfig';
import { ownershipNode, ownershipNodeDetails } from './reducers.js';

// Register modules
bootstrapService.initModule('ownershipnodes', {
    routerConfig,
    modalConfig
});

// Register reducer
const nodeOwnership = combineReducers({ ownershipNode, ownershipNodeDetails });
bootstrapService.registerReducer({ nodeOwnership });
