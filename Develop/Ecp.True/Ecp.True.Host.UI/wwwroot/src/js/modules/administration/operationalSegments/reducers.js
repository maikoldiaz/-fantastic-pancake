import { UPDATE_SON_SEGMENTS_SUCCESS,
    UPDATE_SON_SEGMENTS_FAILURE,
    UPDATE_SEGMENTS } from './actions';

const sonSegmentsInitialState = {
    updateSonSegmentsFailureToggler: false,
    updateSonSegmentsSuccessToggler: false,
    operational: [],
    others: [],
    updateSegmentsToggler: false
};
export const segments = (state = sonSegmentsInitialState, action = {}) => {
    switch (action.type) {
    case UPDATE_SON_SEGMENTS_SUCCESS: {
        return Object.assign({}, state,
            {
                updateSonSegmentsSuccessToggler: !state.updateSonSegmentsSuccessToggler
            });
    }
    case UPDATE_SON_SEGMENTS_FAILURE: {
        return Object.assign({}, state,
            {
                updateSonSegmentsFailureToggler: !state.updateSonSegmentsFailureToggler
            });
    }
    case UPDATE_SEGMENTS: {
        const sonItems = [];
        const nonSonItems = [];
        action.operational.forEach(e => sonItems.push({
            id: e.elementId,
            name: e.name,
            value: 0,
            selected: false,
            rowVersion: e.rowVersion
        }));
        action.others.forEach(e => nonSonItems.push({
            id: e.elementId,
            name: e.name,
            value: 0,
            selected: false,
            rowVersion: e.rowVersion
        }));
        return Object.assign({}, state, {
            operational: sonItems,
            others: nonSonItems,
            updateSegmentsToggler: !state.updateSegmentsToggler
        });
    }
    default:
        return state;
    }
};
