import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { httpService } from '../../../../common/services/httpService';
import TransactionDetails, { TransactionDetails as TransactionDetailsComponent } from '../../../../modules/administration/blockchain/components/transactionDetails.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';

const initialState = {
    blockchain: {
        transaction: {
            blockNumber: 168784,
            data: '',
            id: 'HIIO0m1WcSiw9QOcBMd4dN5qgI8ZJCeJX8lNNpv29v4=',
            timestamp: 1595350195,
            transactionHash: '0x872d7e88be51276d665a20ca054f630280ffda5452fa07d74e9d8472a263c0b7',
            transactionTime: '2020-07-21T11:49:55',
            type: 8,
            version: 2
        },
        transactionDetails: {
            blockNumber: 168784,
            gasUsed: 107812,
            blockHash: '0xb7350a75cbe975194f28218262c669f43e72ebc7c84c0eb8f01f5def499631e3',
            gasLimit: '700000000',
            parentHash: '0x7a901e2fd0c0a85786b4a53bbcf77ad2946316e6d95d1ccdc7b94cbafd829f9b',
            transactionTime: '0001-01-01T00:00:00',
            transactionHash: '0x872d7e88be51276d665a20ca054f630280ffda5452fa07d74e9d8472a263c0b7',
            type: 8,
            id: 'HIIO0m1WcSiw9QOcBMd4dN5qgI8ZJCeJX8lNNpv29v4=',
            content: ''
        }
    }
};

function mountWithRealStore() {
    const reducers = {
        blockchain: jest.fn(() => initialState.blockchain)
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        closeModal: jest.fn(() => Promise.resolve)
    };
    const enzymeWrapper = mount(<Provider store={store}><TransactionDetails {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('View Transaction Details', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call api on mount', () => {
        const props = {
            transaction: {
                blockNumber: 168784,
                data: '',
                id: 'HIIO0m1WcSiw9QOcBMd4dN5qgI8ZJCeJX8lNNpv29v4=',
                timestamp: 1595350195,
                transactionHash: '0x872d7e88be51276d665a20ca054f630280ffda5452fa07d74e9d8472a263c0b7',
                transactionTime: '2020-07-21T11:49:55',
                type: 8,
                version: 2
            },
            transactionDetails: {
                blockNumber: 168784,
                gasUsed: 107812,
                blockHash: '0xb7350a75cbe975194f28218262c669f43e72ebc7c84c0eb8f01f5def499631e3',
                gasLimit: '700000000',
                parentHash: '0x7a901e2fd0c0a85786b4a53bbcf77ad2946316e6d95d1ccdc7b94cbafd829f9b',
                transactionTime: '0001-01-01T00:00:00',
                transactionHash: '0x872d7e88be51276d665a20ca054f630280ffda5452fa07d74e9d8472a263c0b7',
                type: 8,
                id: 'HIIO0m1WcSiw9QOcBMd4dN5qgI8ZJCeJX8lNNpv29v4=',
                content: ''
            },
            getTransaction: jest.fn()
        };
        const wrapper = shallow(<TransactionDetailsComponent {...props} />);
        expect(props.getTransaction.mock.calls).toHaveLength(1);
    });

    it('should cancel add comment when cancel button is clicked ', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find(ModalFooter).find('#btn_transaction_submit').simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should reset props on unmount', () => {
        const props = {
            transaction: {
                blockNumber: 168784,
                data: '',
                id: 'HIIO0m1WcSiw9QOcBMd4dN5qgI8ZJCeJX8lNNpv29v4=',
                timestamp: 1595350195,
                transactionHash: '0x872d7e88be51276d665a20ca054f630280ffda5452fa07d74e9d8472a263c0b7',
                transactionTime: '2020-07-21T11:49:55',
                type: 8,
                version: 2
            },
            transactionDetails: {
                blockNumber: 168784,
                gasUsed: 107812,
                blockHash: '0xb7350a75cbe975194f28218262c669f43e72ebc7c84c0eb8f01f5def499631e3',
                gasLimit: '700000000',
                parentHash: '0x7a901e2fd0c0a85786b4a53bbcf77ad2946316e6d95d1ccdc7b94cbafd829f9b',
                transactionTime: '0001-01-01T00:00:00',
                transactionHash: '0x872d7e88be51276d665a20ca054f630280ffda5452fa07d74e9d8472a263c0b7',
                type: 8,
                id: 'HIIO0m1WcSiw9QOcBMd4dN5qgI8ZJCeJX8lNNpv29v4=',
                content: ''
            },
            getTransaction: jest.fn(),
            wizardSetStep: jest.fn(),
            resetTransaction: jest.fn(),
            config: { wizardName: 'name' }
        };
        const wrapper = shallow(<TransactionDetailsComponent {...props} />);
        wrapper.unmount();
        expect(props.wizardSetStep.mock.calls).toHaveLength(1);
        expect(props.resetTransaction.mock.calls).toHaveLength(1);
    });
});
