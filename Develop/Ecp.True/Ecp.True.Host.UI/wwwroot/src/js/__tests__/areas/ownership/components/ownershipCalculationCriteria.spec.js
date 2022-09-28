import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import OwnershipCalculationCriteria, { OwnershipCalculationCriteria as OwnershipCalculationCriteriaComponent } from '../../../../modules/transportBalance/ownership/components/ownershipCalculationCriteria';
import { constants } from '../../../../common/services/constants';
import { dateService } from '../../../../common/services/dateService';

const initialValue = {
    shared: {
        groupedCategoryElements: {}
    },
    ownership: {
        ticket: {
            categoryElementId: 1,
            startDate: '',
            endDate: '',
            ticketTypeId: constants.TicketType.Ownership
        },
        initialDate: '12/09/2019',
        lastCutoffDate: '12/09/2019'
    },
    refreshToggler: false
};

function mountWithRealStore() {
    const reducers = {
        ownership: jest.fn(() => initialValue.ownership),
        refreshToggler: false,
        shared: jest.fn(() => initialValue.shared)
    };

    const props = {
        closeModal: jest.fn(() => Promise.resolve),
        refreshGrid: jest.fn(()=> Promise.resolve),
        getCategoryElements: jest.fn(()=> Promise.resolve),
        clearSegmentAndDate: jest.fn(()=> Promise.resolve),
        refreshToggler: false,
        getInitialDate: jest.fn(),
        hideError: jest.fn(),
        setOwnershipCalculationInfo: jest.fn(),
        onNext: jest.fn(),
        handleSubmit: jest.fn()
    };

    const store = createStore(combineReducers(reducers));
    const enzymeWrapper = mount(<Provider store={store} >
        <OwnershipCalculationCriteria {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Ownership Calculation Criteria', () => {
    beforeAll(() => {

    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should handle form submit', () => {
        const props = {
            refreshDateToggler: true,
            hideError: jest.fn(),
            initialValues: {
                initialDate: '2014-02-02'
            },
            showError: jest.fn(),
            groupedCategoryElements: [
                {
                    id: '1'
                },
                {
                    id: '2'
                }
            ],
            handleSubmit: jest.fn(),
            getCategoryElements: jest.fn(),
            clearSegmentAndDate: jest.fn()
        };

        dateService.isValidDate = jest.fn(() => {
            return false;
        });
        const wrapper = shallow(<OwnershipCalculationCriteriaComponent {...props} />);
        wrapper.find('form').simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(1);
    });

    it('should handle selected segment change', () => {
        const props = {
            refreshDateToggler: true,
            hideError: jest.fn(),
            initialValues: {
                initialDate: '2014-02-02'
            },
            showError: jest.fn(),
            groupedCategoryElements: [
                {
                    id: '1'
                },
                {
                    id: '2'
                }
            ],
            handleSubmit: jest.fn(),
            getCategoryElements: jest.fn(),
            clearSegmentAndDate: jest.fn()
        };

        dateService.isValidDate = jest.fn(() => {
            return false;
        });
        const wrapper = shallow(<OwnershipCalculationCriteriaComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { refreshDateToggler: false }));
        expect(props.showError.mock.calls).toHaveLength(1);
        expect(props.hideError.mock.calls).toHaveLength(1);
    });
});
