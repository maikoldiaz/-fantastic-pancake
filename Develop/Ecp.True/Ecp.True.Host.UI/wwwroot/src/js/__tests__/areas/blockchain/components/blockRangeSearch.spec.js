import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { reduxForm } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import BlockRangeSearch, { BlockRangeSearch as BlockRangeSearchComponent } from '../../../../modules/administration/blockchain/components/blockRangeSearch.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { blockValidator } from '../../../../modules/administration/blockchain/blockValidationService';

function mountWithRealStore() {
    const reducers = {
        saveBlockRange: jest.fn(() => Promise.resolve)
    };
    const props = {
        closeModal: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn()
    };
    const store = createStore(combineReducers(reducers));

    const BlockRangeSearchForm = reduxForm({
        form: 'blockRangeSearch'
    })(BlockRangeSearch);

    const enzymeWrapper = mount(<Provider store={store} >
        <BlockRangeSearchForm {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('BlockRangeSearchForm', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should be able to add head block', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#txt_blockchainRangeSearch_headBlockNumber')).toBe(true);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should be able to add tail block', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#txt_blockchainRangeSearch_tailBlockNumber')).toBe(true);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should be able to select event', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_blockchainRangeSearch_type')).toBe(true);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should handle submit', () => {
        const props = { saveBlockRange: jest.fn(), handleSubmit: jest.fn(), resetForm: jest.fn() };
        blockValidator.validateBlockRange = jest.fn(() => Promise.resolve);
        const wrapper = shallow(<BlockRangeSearchComponent {...props} />);
        wrapper.instance().onSubmit({ headBlockNumber: '1', event: 0, tailBlockNumber: '2' });
        expect(props.saveBlockRange.mock.calls).toHaveLength(0);
    });
});
