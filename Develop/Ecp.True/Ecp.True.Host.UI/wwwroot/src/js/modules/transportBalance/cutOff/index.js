import { bootstrapService } from '../../../common/services/bootstrapService';
import CutOffModalConfig from './routerConfig';
import CutOffRouterConfig from './modalConfig';
import { operationalCut, ticketInfo } from './reducers';
import { combineReducers } from 'redux';

// Register modules
bootstrapService.initModule('cutoff', {
    routerConfig: CutOffModalConfig,
    modalConfig: CutOffRouterConfig
});

// Register reducer
const cutoffReducer = combineReducers({ operationalCut, ticketInfo });
bootstrapService.registerReducer({ cutoff: cutoffReducer });

