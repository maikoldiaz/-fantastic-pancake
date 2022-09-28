import setup from '../../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import * as tag from '../../../../../modules/administration/node/nodeTags/components/tagNodes.jsx';
import { TagNodes as TagNodesComponent } from '../../../../../modules/administration/node/nodeTags/components/tagNodes.jsx';
import configureStore from 'redux-mock-store';
import { dateService } from '../../../../../common/services/dateService';
import { ModalFooter } from '../../../../../common/components/footer/modalFooter.jsx';

const initialState = {
    categoryElementFilter: {
        nodeTags: {
            values: {
                categoryElements: [{
                    category: {
                        categoryId: 1
                    },
                    element: {
                        elementId: 1
                    }
                }]
            }
        }
    },
    shared: {
        categoryElements: [{
            category: {
                categoryId: 1
            },
            element: {
                elementId: 1
            }
        }]
    },
    node: {
        nodeTags: {
            defaultValues: {

            },
            refreshToggler: {

            }
        }
    },
    modal: {
        modalkey: 'key1'
    },
    grid: {
        nodeTags: {
            items: {

            },
            selection: {

            }
        }
    }
};

function mountWithMockStore(defaultState) {
    const mockStore = configureStore();
    const store = mockStore(defaultState);
    const props = {
        closeModal: jest.fn(() => Promise.resolve)
    };
    const enzymeWrapper = mount(<Provider store={store}><tag.tagNodes {...props}/></Provider>);
    return { store, enzymeWrapper };
}

describe('tagNodes', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithMockStore(initialState);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('find Control', () => {
        const { enzymeWrapper } = mountWithMockStore(initialState);
        expect(enzymeWrapper.exists('#p_nodeTag_select')).toBe(true);
    });

    it('should call cancel when clicked', () => {
        const { enzymeWrapper } = mountWithMockStore(initialState);
        enzymeWrapper.find('#btn_nodeTag_cancel').simulate('click');
    });

    it('should call save when clicked', () => {
        const { enzymeWrapper } = mountWithMockStore(initialState);
        enzymeWrapper.find(ModalFooter).find('#btn_nodeTag_submit').simulate('click');
    });
    it('should handle form submit', () => {
        const props = {
            handleSubmit: jest.fn(),
            mode: 'new',
            selectedItems: [],
            expire: {},
            groupNodeCategory: jest.fn(),
            selectedCategory: {
                name: 'name'
            },
            selectedElement: {
                name: 'name'
            },
            getCategoryElements: jest.fn(),
            hideError: jest.fn()
        };

        dateService.parseFieldToISOString = jest.fn(() => {
            return false;
        });
        const wrapper = shallow(<TagNodesComponent {...props} />);
        wrapper.find('form').simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(1);
    });
    it('should update on failureToggler change', () => {
        const props = {
            handleSubmit: jest.fn(),
            mode: 'new',
            selectedItems: [],
            expire: {},
            groupNodeCategory: jest.fn(),
            selectedCategory: {
                name: 'name'
            },
            selectedElement: {
                name: 'name'
            },
            getCategoryElements: jest.fn(),
            hideError: jest.fn(),
            failureToggler: true,
            showError: jest.fn(),
            closeModal: jest.fn()
        };

        dateService.parseFieldToISOString = jest.fn(() => {
            return false;
        });
        const wrapper = shallow(<TagNodesComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { failureToggler: false }));
        wrapper.find('form').simulate('submit');
        expect(props.showError.mock.calls).toHaveLength(1);
    });
    it('should update on refreshToggler change', () => {
        const props = {
            handleSubmit: jest.fn(),
            mode: 'new',
            selectedItems: [],
            expire: {},
            groupNodeCategory: jest.fn(),
            selectedCategory: {
                name: 'name'
            },
            selectedElement: {
                name: 'name'
            },
            getCategoryElements: jest.fn(),
            hideError: jest.fn(),
            refreshToggler: true,
            showError: jest.fn(),
            closeModal: jest.fn(),
            showSuccess: jest.fn(),
            refreshGrid: jest.fn()
        };

        dateService.parseFieldToISOString = jest.fn(() => {
            return false;
        });
        const wrapper = shallow(<TagNodesComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { refreshToggler: false }));
        wrapper.find('form').simulate('submit');
        expect(props.refreshGrid.mock.calls).toHaveLength(1);
        expect(props.closeModal.mock.calls).toHaveLength(1);
        expect(props.showSuccess.mock.calls).toHaveLength(1);
    });
});
