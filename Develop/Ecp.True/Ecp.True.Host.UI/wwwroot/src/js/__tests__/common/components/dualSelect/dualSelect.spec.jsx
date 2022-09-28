import React from 'react';
import { createStore, combineReducers } from 'redux';
import { mount } from 'enzyme';
import { Provider } from 'react-redux';
import setup from '../../../areas/setup';
import DualSelect from '../../../../common/components/dualSelect/dualSelect';

function mountWithRealStore() {
    const defaultProps = {
        history: 'history',
        location: 'location',
        match: {},
        dualSelect:
        {
            nodeProducts_owners: {
                sourceSearch: [{
                    id: 'someId',
                    ctrlKey: 'someKey',
                    name: 'someSourceName',
                    selected: true
                }],
                targetSearch: [{
                    id: 'someId',
                    ctrlKey: 'someKey',
                    name: 'someTargetName',
                    selected: true
                }]
            },
            connectionProducts_owners: {
                sourceSearch: [{
                    id: 'someId',
                    ctrlKey: 'someKey',
                    name: 'someSourceName',
                    selected: true
                }],
                targetSearch: [{
                    id: 'someId',
                    ctrlKey: 'someKey',
                    name: 'someTargetName',
                    selected: true
                }]
            },
            segments: {
                sourceSearch: [{
                    id: 'someId',
                    ctrlKey: 'someKey',
                    name: 'someSourceName',
                    selected: true
                }],
                targetSearch: [{
                    id: 'someId',
                    ctrlKey: 'someKey',
                    name: 'someTargetName',
                    selected: true
                }]
            }
        }
    };

    const reducers = {
        dualSelect: jest.fn(() => defaultProps.dualSelect)
    };
    const store = createStore(combineReducers(reducers));
    const props = {
        name: 'segments',
        sourceTitle: 'dualSelectLeftTtl',
        targetTitle: 'dualSelectRightTtl',
        selectSource: jest.fn(),
        searchSource: jest.fn(),
        moveAll: jest.fn(),
        moveBackAll: jest.fn(),
        searchTarget: jest.fn(),
        selectTarget: jest.fn(),
        move: jest.fn(),
        moveBack: jest.fn(),
        sourceSearch: defaultProps.dualSelect.connectionProducts_owners.sourceSearch
    };

    const enzymeWrapper = mount(<Provider store={store}><DualSelect {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('dual select', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call select source if condition is satisfied', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        enzymeWrapper.find(`#li_segments_source_someId`).equals(true);
        enzymeWrapper.find(`#li_segments_source_someId`).at(0).simulate('click');
        expect(props.selectSource.mock.calls).toHaveLength(1);
    });

    it('should call source search if condition is satisfied', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        enzymeWrapper.find(`#txt_segments_source_search`).equals(true);
        enzymeWrapper.find(`#txt_segments_source_search`).at(0).simulate('change');
        expect(props.searchSource.mock.calls).toHaveLength(1);
    });

    it('should call move all if condition is satisfied', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        enzymeWrapper.find(`#btn_segments_source_moveAll`).equals(true);
        enzymeWrapper.find(`#btn_segments_source_moveAll`).at(0).simulate('click');
        expect(props.moveAll.mock.calls).toHaveLength(1);
    });

    it('should call move if condition is satisfied', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        enzymeWrapper.find(`#btn_segments_source_move`).equals(true);
        enzymeWrapper.find(`#btn_segments_source_move`).at(0).simulate('click');
        expect(props.move.mock.calls).toHaveLength(1);
    });

    it('should call move back if condition is satisfied', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        enzymeWrapper.find(`#btn_segments_target_move`).equals(true);
        enzymeWrapper.find(`#btn_segments_target_move`).at(0).simulate('click');
        expect(props.moveBack.mock.calls).toHaveLength(1);
    });

    it('should call move back all if condition is satisfied', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        enzymeWrapper.find(`#btn_segments_target_moveAll`).equals(true);
        enzymeWrapper.find(`#btn_segments_target_moveAll`).at(0).simulate('click');
        expect(props.moveBackAll.mock.calls).toHaveLength(1);
    });

    it('should call target search if condition is satisfied', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        enzymeWrapper.find(`#txt_segments_target_search`).equals(true);
        enzymeWrapper.find(`#txt_segments_target_search`).at(0).simulate('change');
        expect(props.searchTarget.mock.calls).toHaveLength(1);
    });

    it('should call target search on link click', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        enzymeWrapper.find(`#li_segments_target_someId`).equals(true);
        enzymeWrapper.find(`#li_segments_target_someId`).at(0).simulate('click');
        expect(props.selectTarget.mock.calls).toHaveLength(1);
    });

    it('should show left title', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        enzymeWrapper.find(`#txt_segments_left_title`).equals(true);
    });

    it('should show right title', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        enzymeWrapper.find(`#txt_segments_right_title`).equals(true);
    });
});
