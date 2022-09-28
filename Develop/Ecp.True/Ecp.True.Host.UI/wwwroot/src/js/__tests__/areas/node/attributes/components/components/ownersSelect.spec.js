import setup from '../../../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../../common/services/httpService';
import OwnersSelect, { OwnersSelect as OwnersSelectComponent } from '../../../../../../modules/administration/node/attributes/components/ownersSelect.jsx';
import { ModalFooter } from '../../../../../../common/components/footer/modalFooter.jsx';

function mountWithRealStore() {
    const data = {
        nodeConnection: {
            attributes: {
                owners: [{
                    category: null,
                    categoryId: 7,
                    createdBy: 'System',
                    createdDate: '2019-10-03T17:36:04.367',
                    description: 'Reficar',
                    elementId: 27,
                    isActive: true,
                    isAuditable: true,
                    name: 'Reficar',
                    selected: true
                },
                {
                    category: null,
                    categoryId: 8,
                    createdBy: 'System',
                    createdDate: '2019-10-03T17:36:04.367',
                    description: 'Reficar',
                    elementId: 27,
                    isActive: true,
                    isAuditable: true,
                    name: 'Reficar',
                    selected: true
                }],
                ownersToggler: false
            }
        },
        dualSelect: {
            nodeProducts_owners: {
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
        }
    };

    const reducers = {
        nodeConnection: jest.fn(() => data.nodeConnection),
        dualSelect: jest.fn(() => data.dualSelect),
        onClose: jest.fn(() => Promise.resolve())
    };

    const props = {
        owners: [{
            category: null,
            categoryId: 7,
            createdBy: 'System',
            createdDate: '2019-10-03T17:36:04.367',
            description: 'Reficar',
            elementId: 27,
            isActive: true,
            isAuditable: true,
            name: 'Reficar',
            selected: true
        },
        {
            category: null,
            categoryId: 8,
            createdBy: 'System',
            createdDate: '2019-10-03T17:36:04.367',
            description: 'Reficar',
            elementId: 27,
            isActive: true,
            isAuditable: true,
            name: 'Reficar',
            selected: true
        }],
        closeModal: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn(),
        initAllItems: jest.fn()
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <OwnersSelect {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('OwnersSelect', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should have cancel and next button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_nodeProducts_ownersselect_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find(ModalFooter).find('#btn_nodeProducts_ownersselect_submit').text()).toEqual('next');
    });

    it('should call onClose on click of cancel button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find(ModalFooter).find('footer').find('#btn_nodeProducts_ownersselect_cancel').at(0).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('ownersToggler update should toggle the property', () => {
        const { props } = mountWithRealStore();
        props.ownersToggler = false;
        expect(props.owners[0].selected).toEqual(true);
    });

    it('ownersToggler update should call component did update on prop change', () => {
        const props = {
            closeModal: jest.fn(() => Promise.resolve),
            handleSubmit: jest.fn(),
            initAllItems: jest.fn(),
            target: [{ name: 'someName' }],
            owners: [{
                category: null,
                categoryId: 7,
                createdBy: 'System',
                createdDate: '2019-10-03T17:36:04.367',
                description: 'Reficar',
                elementId: 27,
                isActive: true,
                isAuditable: true,
                name: 'Reficar',
                selected: true
            },
            {
                category: null,
                categoryId: 8,
                createdBy: 'System',
                createdDate: '2019-10-03T17:36:04.367',
                description: 'Reficar',
                elementId: 27,
                isActive: true,
                isAuditable: true,
                name: 'Reficar',
                selected: true
            }]
        };
        const wrapper = shallow(<OwnersSelectComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { ownersToggler: true }));
        expect(props.initAllItems.mock.calls).toHaveLength(1);
    });

    it('should call get owners if owners not present', () => {
        const props = {
            closeModal: jest.fn(() => Promise.resolve),
            handleSubmit: jest.fn(),
            initAllItems: jest.fn(),
            target: [{ name: 'someName' }],
            getOwners: jest.fn()
        };
        const wrapper = shallow(<OwnersSelectComponent {...props} />);
        expect(props.getOwners.mock.calls).toHaveLength(1);
    });
});
