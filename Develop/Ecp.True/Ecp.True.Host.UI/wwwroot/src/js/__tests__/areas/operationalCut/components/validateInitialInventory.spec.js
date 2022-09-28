import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import ValidateInitialInventory from '../../../../modules/transportBalance/cutOff/components/validateInitialInventory.jsx';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';

const initialState = {
    cutoff: {
        operationalCut: {
            ticket: {
                startDate: '2019-01-01',
                endDate: '2019-11-12',
                segment: {
                    name: 'Segmento',
                    category: {
                        name: 'Automation_first'
                    }
                }
            },
            inventoriesValidations: { 1: [], 2: [] }
        }
    },
    config: { wizardName: 'name' }
};

function mountWithRealStore(newState = {}) {
    const reducers = {
        cutoff: jest.fn(() => Object.assign({}, initialState.cutoff, newState))
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        getInventoriesValidations: jest.fn(() => Promise.resolve),
        cancelCutOffValidation: jest.fn(),
        showWarning: jest.fn(),
        hideWarning: jest.fn()
    };

    const enzymeWrapper = mount(<Provider store={store}>
        <ValidateInitialInventory {...props} config={initialState.config} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('validateInitialInventory', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('find controls', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_validateInitialInventory_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find(ModalFooter).find('#btn_validateInitialInventory_submit').text()).toEqual('next');
    });

    it('should validate when nodes with Initial Inventories without Ownership, nodes without Initial Inventories', () => {
        const newProps = {
            operationalCut: {
                ticket: {
                    startDate: '2019-01-01',
                    endDate: '2019-11-12',
                    segment: {
                        name: 'Segmento',
                        category: {
                            name: 'Automation_first'
                        }
                    }
                },
                inventoriesValidations: {
                    1: [{ nodeName: 'Automation_fnn6d', inventoryDate: '2020-05-17T00:00:00', type: 1 }],
                    2: [{ nodeName: 'Automation_0n4ew', inventoryDate: '2020-05-17T00:00:00', type: 2 }]
                }
            }
        };
        const { enzymeWrapper } = mountWithRealStore(newProps);
        expect(enzymeWrapper.find(SectionFooter).find('#btn_validateInitialInventory_submit').prop('disabled')).toEqual(true);
        expect(enzymeWrapper.find('#tbl_validate_nodesWithoutOwnership')).toHaveLength(1);
        expect(enzymeWrapper.find('#tbl_validate_nodesWithoutInitialInventory')).toHaveLength(1);
    });

    it('should validate when nodes with Initial Inventories with Ownership, nodes with Initial Inventories', () => {
        const newProps = {
            operationalCut: {
                ticket: {
                    startDate: '2019-01-01',
                    endDate: '2019-11-12',
                    segment: {
                        name: 'Segmento',
                        category: {
                            name: 'Automation_first'
                        }
                    }
                },
                inventoriesValidations: { 1: [], 2: [] }
            }
        };
        const { enzymeWrapper } = mountWithRealStore(newProps);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_validateInitialInventory_submit').prop('disabled')).toEqual(false);
        expect(enzymeWrapper.find('#tbl_validate_nodesWithoutOwnership')).toHaveLength(0);
        expect(enzymeWrapper.find('#tbl_validate_nodesWithoutInitialInventory')).toHaveLength(0);
    });

    it('should validate when only nodes with Initial Inventories without Ownership', () => {
        const newProps = {
            operationalCut: {
                ticket: {
                    startDate: '2019-01-01',
                    endDate: '2019-11-12',
                    segment: {
                        name: 'Segmento',
                        category: {
                            name: 'Automation_first'
                        }
                    }
                },
                inventoriesValidations: { 1: [{ nodeName: 'Automation_fnn6d', inventoryDate: '2020-05-17T00:00:00', type: 1 }], 2: [] }
            }
        };
        const { enzymeWrapper } = mountWithRealStore(newProps);
        expect(enzymeWrapper.find(SectionFooter).find('#btn_validateInitialInventory_submit').prop('disabled')).toEqual(true);
        expect(enzymeWrapper.find('#tbl_validate_nodesWithoutOwnership')).toHaveLength(1);
        expect(enzymeWrapper.find('#tbl_validate_nodesWithoutInitialInventory')).toHaveLength(0);
    });

    it('should validate when only nodes without Initial Inventories', () => {
        const newProps = {
            operationalCut: {
                ticket: {
                    startDate: '2019-01-01',
                    endDate: '2019-11-12',
                    segment: {
                        name: 'Segmento',
                        category: {
                            name: 'Automation_first'
                        }
                    }
                },
                inventoriesValidations: { 1: [], 2: [{ nodeName: 'Automation_fnn6d', inventoryDate: '2020-05-17T00:00:00', type: 1 }] }
            }
        };
        const { enzymeWrapper } = mountWithRealStore(newProps);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_validateInitialInventory_submit').prop('disabled')).toEqual(false);
        expect(enzymeWrapper.find('#tbl_validate_nodesWithoutOwnership')).toHaveLength(0);
        expect(enzymeWrapper.find('#tbl_validate_nodesWithoutInitialInventory')).toHaveLength(1);
    });
});
