import { combineReducers } from 'redux';
import { bootstrapService } from '../../../common/services/bootstrapService';
import manageSegmentsRouterConfig from './routerConfig';
import { segments } from './reducers';

// Register modules
bootstrapService.initModule('operationalSegments', {
    routerConfig: manageSegmentsRouterConfig
});

// Register reducer
const manageSegment = combineReducers({ segments });
bootstrapService.registerReducer({ manageSegment });
