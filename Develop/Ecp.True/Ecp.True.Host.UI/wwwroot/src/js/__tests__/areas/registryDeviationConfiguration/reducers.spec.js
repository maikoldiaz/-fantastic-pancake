import * as actions from '../../../modules/administration/registryDeviationConfiguration/actions';
import { registryDeviation } from '../../../modules/administration/registryDeviationConfiguration/reducers';

describe('Reducer for registryDeviationConfiguration', () => {
    const initialState = {
        categoryElements: {},
        updatedDeviationToggler: false
    };

    it('should receive segment deviation and set valid deviationPercentage when value is null', () => {
        const action = {
            type: actions.RECEIVE_FILTER_CATEGORY_ELEMENTS,
            items: {
                value: [{
                    elementId: 1,
                    name: 'segment-1',
                    deviationPercentage: null
                }]
            }
        };
        const state = Object.assign({}, initialState, { categoryElements: [] });
        const newState = Object.assign({}, initialState, {
            categoryElements: [{
                elementId: 1,
                name: 'segment-1',
                deviationPercentage: '0.00'
            }]
        });

        expect(registryDeviation(state, action)).toEqual(newState);
    });

    it('should receive segment deviation and set deviationPercentage - value is right', () => {
        const action = {
            type: actions.RECEIVE_FILTER_CATEGORY_ELEMENTS,
            items: {
                value: [{
                    elementId: 1,
                    name: 'segment-1',
                    deviationPercentage: '2.00'
                }]
            }
        };
        const state = Object.assign({}, initialState, { categoryElements: [] });
        const newState = Object.assign({}, initialState, {
            categoryElements: [{
                elementId: 1,
                name: 'segment-1',
                deviationPercentage: '2.00'
            }]
        });

        expect(registryDeviation(state, action)).toEqual(newState);
    });

    it('should receive the status of the request and exchange the variable updatedDeviationToggler', () => {
        const action = {
            type: actions.RECEIVE_UPDATE_DEVIATION,
            status: 'successful'
        };
        const state = Object.assign({}, initialState, { updatedDeviationToggler: false });
        const newState = Object.assign({}, initialState, {
            status: 'successful',
            updatedDeviationToggler: true
        });

        expect(registryDeviation(state, action)).toEqual(newState);
    });

    it('should return the same state when the action type is not include', () => {
        const action = {
            type: 'ACTION_NOT_INCLUDED'
        };
        const state = Object.assign({}, initialState);

        expect(registryDeviation(state, action)).toEqual(state)
    });
});
