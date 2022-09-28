import * as actions from '../../../modules/administration/operationalSegments/actions';
import { segments } from '../../../modules/administration/operationalSegments/reducers';

describe('Reducers for operational segment', () => {
    const initialState = {
        updateSonSegmentsFailureToggler: false,
        updateSonSegmentsSuccessToggler: false,
        operational: [],
        others: [],
        updateSegmentsToggler: false
    };

    it('should update toggler on son segments success', () => {
        const action = {
            type: actions.UPDATE_SON_SEGMENTS_SUCCESS
        };
        const newState = Object.assign({}, initialState, {
            updateSonSegmentsSuccessToggler: true
        });
        expect(segments(initialState, action)).toEqual(newState);
    });

    it('should update toggler on son segments failure', () => {
        const action = {
            type: actions.UPDATE_SON_SEGMENTS_FAILURE
        };
        const newState = Object.assign({}, initialState, {
            updateSonSegmentsFailureToggler: true
        });
        expect(segments(initialState, action)).toEqual(newState);
    });

    it('should update operational and other segments', () => {
        const operational = [{
            elementId: 1,
            name: 'test',
            rowVersion: 'testRowVersion'
        }];
        const others = [{
            elementId: 2,
            name: 'test',
            rowVersion: 'testRowVersion'
        }];
        const action = {
            type: actions.UPDATE_SEGMENTS,
            operational,
            others
        };
        const newState = Object.assign({}, initialState, {
            operational: [{
                id: 1,
                name: 'test',
                value: 0,
                selected: false,
                rowVersion: 'testRowVersion'
            }],
            others: [{
                id: 2,
                name: 'test',
                value: 0,
                selected: false,
                rowVersion: 'testRowVersion'
            }],
            updateSegmentsToggler: true
        });
        expect(segments(initialState, action)).toEqual(newState);
    });

    it('should return default state', () => {
        const action = {
            type: 'test'
        };
        expect(segments(initialState, action)).toEqual(initialState);
    });
});
