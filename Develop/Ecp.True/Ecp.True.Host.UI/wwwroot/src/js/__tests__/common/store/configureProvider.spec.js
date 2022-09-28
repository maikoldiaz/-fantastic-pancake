import provider from '../../../common/store/ConfigureProvider';
import thunk from 'redux-thunk';
import { combineReducers } from 'redux';
import fetchMiddleware from '../../../common/middlewares/fetchMiddleware';
import { bootstrapService } from '../../../common/services/bootstrapService';
import AppComponent from '../../../common/layouts/app';

it('should return the store', () => {
    const { store } = provider(<AppComponent />, combineReducers(bootstrapService.getAllReducers()), {}, thunk, fetchMiddleware);
    expect(store).toBeDefined();
});