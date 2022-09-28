import setup from '../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import { DeltaCalculationsTechnicalError } from '../../../../common/components/modal/deltaCalculationsTechnicalError.jsx';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';

const initialValues = {
    cutoff: {
        ticketInfo: {
            ticket: {
                segment: 'segmentName',
                errorMessage: 'error'

            }
        }
    }
};

function mountWithRealStore(initialValue) {
    const reducers = {
        cutoff: jest.fn(() => initialValue.cutoff)
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        closeModal: jest.fn(() => Promise.resolve),
        ticket: {
            segment: 'segmentName',
            errorMessage: 'error'

        }
    };


    const enzymeWrapper = mount(<Provider store={store}>
        <DeltaCalculationsTechnicalError {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Operational Delta errors', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore(initialValues);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('find controls', () => {
        const { enzymeWrapper } = mountWithRealStore(initialValues);
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#lbl_ErrorDetails_Category')).toBe(true);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_deltaCalculationsTechnicalError_submit').text()).toEqual('accept');
    });

    it('should close pop up when accept button is clicked ', () => {
        const { enzymeWrapper } = mountWithRealStore(initialValues);
        enzymeWrapper.find(ModalFooter).find('#btn_deltaCalculationsTechnicalError_submit').simulate('click');
    });
});
