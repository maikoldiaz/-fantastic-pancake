import setup from '../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import ErrorDetail from '../../../../modules/administration/exceptions/components/errorDetail.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { navigationService } from './../../../../common/services/navigationService';

const initialValue = {
    controlexception: {
        controlException: {
            errorDetail: [{
                errorId: 1,
                errorMessage: 'Sample error text'
            }]
        }
    },
    selectedData: [1]
};

function mountWithRealStore() {
    const reducers = {
        controlexception: jest.fn(() => initialValue.controlexception),
        setSelectedData: jest.fn(() => initialValue.selectedData)
    };

    const props = {
        getErrorDetail: jest.fn(() => Promise.resolve()),
        enableDisableDiscard: jest.fn(() => Promise.resolve()),
        onDiscardException: jest.fn(() => Promise.resolve()),
        setModuleName: jest.fn(() => Promise.resolve())
    };

    const store = createStore(combineReducers(reducers));
    const enzymeWrapper = mount(<Provider store={store} >
        <ErrorDetail {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Transformation Section', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        navigationService.getParamByName = jest.fn(() => {
            return '1_P';
        });
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should unmount component', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.unmount();
        expect(props.setModuleName.mock.calls).toHaveLength(0);
    });
});
