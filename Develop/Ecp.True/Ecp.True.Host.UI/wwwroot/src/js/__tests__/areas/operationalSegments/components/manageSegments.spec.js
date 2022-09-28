import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import ManageSegments, { ManageSegments as ManageSegmentsComponent } from '../../../../modules/administration/operationalSegments/components/manageSegments.jsx';

function mountWithRealStore() {
    const data = {
        shared: {
            groupedCategoryElements: [],
            categoryElementsDataToggler: false
        },
        dualSelect: {
            segments: {
                target: {},
                sourceSearch: [{
                    id: 27,
                    name: 'Reficar',
                    value: 0,
                    selected: true
                },
                {
                    id: 28,
                    name: 'Cepcolsa',
                    value: 0,
                    selected: false
                },
                {
                    id: 29,
                    name: 'EQUION',
                    value: 0,
                    selected: false
                },
                {
                    id: 30,
                    name: 'ECOPETROL',
                    value: 0,
                    selected: false
                },
                {
                    id: 31,
                    name: 'CENIT',
                    value: 0,
                    selected: false
                }],
                targetSearch: []
            }
        },
        manageSegments: {
            segments: {
                updateSonSegmentsFailureToggler: false,
                updateSonSegmentsSuccessToggler: false
            }
        }
    };

    const reducers = {
        shared: jest.fn(() => data.shared),
        dualSelect: jest.fn(() => data.dualSelect),
        manageSegment: jest.fn(() => data.manageSegments)
    };

    const props = {
        groupedCategoryElements: [{
            category: { categoryId: 1, name: 'Tipo de Nodo', description: 'Tipo de Nodo', isActive: true, isGrouper: true },
            categoryId: 1,
            color: null,
            createdBy: 'System',
            createdDate: '2020-03-30T01:07:35.67Z',
            description: 'Facilidad',
            elementId: 1,
            iconId: 1,
            isActive: true,
            isAuditable: true,
            isOperationalSegment: null,
            lastModifiedBy: 'trueadmin',
            lastModifiedDate: '2020-04-13T05:39:03.53Z',
            name: 'Facilidad',
            rowVersion: 'AAAAAAAACtU='
        }]
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <ManageSegments {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('ManageSegments', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should have save button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find('#btn_segments_submit').text()).toEqual('submit');
    });

    it('should configure dual select on mount', () => {
        const props = {
            configureDualSelect: jest.fn(),
            getCategoryElements: jest.fn(),
            name: 'name',
            target: []
        };
        const wrapper = shallow(<ManageSegmentsComponent {...props} />);
        expect(props.configureDualSelect.mock.calls).toHaveLength(1);
        expect(props.getCategoryElements.mock.calls).toHaveLength(1);
    });

    it('should build segments on category element update', () => {
        const props = {
            clearSearchText: jest.fn(),
            updateSegments: jest.fn(),
            configureDualSelect: jest.fn(),
            getCategoryElements: jest.fn(),
            name: 'name',
            groupedCategoryElements: { 2: [{ name: 'name' }, { name: 'name1' }] },
            operationalSegments: [{ name: 'name' }, { name: 'name1' }],
            target: []
        };
        const wrapper = shallow(<ManageSegmentsComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { categoryElementsDataToggler: true }));
        expect(props.clearSearchText.mock.calls).toHaveLength(1);
        expect(props.updateSegments.mock.calls).toHaveLength(1);
    });

    it('should open modal on son failure toggler', () => {
        const props = {
            clearSearchText: jest.fn(),
            updateSegments: jest.fn(),
            configureDualSelect: jest.fn(),
            getCategoryElements: jest.fn(),
            name: 'name',
            groupedCategoryElements: { 2: [{ name: 'name' }, { name: 'name1' }] },
            operationalSegments: [{ name: 'name' }, { name: 'name1' }],
            openModal: jest.fn(),
            target: []
        };
        const wrapper = shallow(<ManageSegmentsComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { updateSonSegmentsFailureToggler: true }));
        expect(props.openModal.mock.calls).toHaveLength(1);
    });

    it('should get category elements on son update success', () => {
        const props = {
            clearSearchText: jest.fn(),
            updateSegments: jest.fn(),
            configureDualSelect: jest.fn(),
            getCategoryElements: jest.fn(),
            name: 'name',
            groupedCategoryElements: { 2: [{ name: 'name' }, { name: 'name1' }] },
            operationalSegments: [{ name: 'name' }, { name: 'name1' }],
            openModal: jest.fn(),
            target: []
        };
        const wrapper = shallow(<ManageSegmentsComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { updateSonSegmentsSuccessToggler: true }));
        expect(props.getCategoryElements.mock.calls).toHaveLength(2);
    });

    it('should initialize items on update', () => {
        const props = {
            clearSearchText: jest.fn(),
            updateSegments: jest.fn(),
            configureDualSelect: jest.fn(),
            getCategoryElements: jest.fn(),
            initAllItems: jest.fn(),
            updateTargetItems: jest.fn(),
            name: 'name',
            groupedCategoryElements: { 2: [{ name: 'name' }, { name: 'name1' }] },
            operationalSegments: [{ name: 'name' }, { name: 'name1' }],
            openModal: jest.fn(),
            target: []
        };
        const wrapper = shallow(<ManageSegmentsComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { updateSegmentsToggler: true }));
        expect(props.initAllItems.mock.calls).toHaveLength(1);
        expect(props.updateTargetItems.mock.calls).toHaveLength(1);
    });

    it('should disable submit button on no items in target', () => {
        const props = {
            clearSearchText: jest.fn(),
            updateSegments: jest.fn(),
            configureDualSelect: jest.fn(),
            getCategoryElements: jest.fn(),
            initAllItems: jest.fn(),
            updateTargetItems: jest.fn(),
            name: 'name',
            groupedCategoryElements: { 2: [{ name: 'name' }, { name: 'name1' }] },
            operationalSegments: [{ name: 'name' }, { name: 'name1' }],
            openModal: jest.fn(),
            target: []
        };
        const wrapper = shallow(<ManageSegmentsComponent {...props} />);
        expect(wrapper.find('#btn_segments_submit').props()).toHaveProperty('disabled', true);
    });

    it('should save segments on submit', () => {
        const props = {
            clearSearchText: jest.fn(),
            updateSegments: jest.fn(),
            saveSegments: jest.fn(),
            configureDualSelect: jest.fn(),
            getCategoryElements: jest.fn(),
            initAllItems: jest.fn(),
            updateTargetItems: jest.fn(),
            name: 'name',
            groupedCategoryElements: { 2: [{ name: 'name' }, { name: 'name1' }] },
            operationalSegments: [{ name: 'name' }, { name: 'name1' }],
            openModal: jest.fn(),
            target: [{ id: 1, rowVersion: 'version' }]
        };
        const wrapper = shallow(<ManageSegmentsComponent {...props} />);
        wrapper.find('#btn_segments_submit').simulate('click');
        expect(props.saveSegments.mock.calls).toHaveLength(1);
    });
});
