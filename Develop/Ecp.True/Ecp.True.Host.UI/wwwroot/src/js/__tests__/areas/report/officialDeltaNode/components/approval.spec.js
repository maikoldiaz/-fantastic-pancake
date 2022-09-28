import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { reducer as formReducer } from 'redux-form';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import Approval, { Approval as ApprovalComponent } from '../../../../../modules/report/officialDeltaNode/components/approval.jsx';
import { navigationService } from '../../../../../common/services/navigationService';


const formValues = {
    element: {
        elementId: 0,
        name: 'name element'
    },
    node: { 
        nodeId: 0, 
        name: 'name node' 
    },
    periods: { start: '2020-08-01', end: '2020-08-31', officialPeriods: [] }
};

const initialValuesReport = {
    officialDeltaNode: {
        filterSettings: {},
        filters: null,
        manualToggler: false,
        formValues: formValues
    }
};

const sharedInitialState = {
    categoryElements: [{categoryId:1,isActive:false},{category:2,isActive:true}],
    allCategories: [],
    progressStatus: {},
    selectedCategory: [],
    selectedElement: [],
    searchedNodes: [],
    categoryElementsToggler: false
};

const nodeFilterInitialState = {
    dateRange: {},
    defaultYear: null,
    dateRangeToggler: null,
    viewReportButtonStatusToggler: false
};

const loaderInitialState = {
    counter: 0
};

const configState = {

}

function mountWithRealStore() {
    const reducers = {
        formValues: jest.fn(() => formValues),
        form: formReducer,
        nodeFilter: jest.fn(() => nodeFilterInitialState),
        loader: jest.fn(() => loaderInitialState),
        shared: jest.fn(() => sharedInitialState),
        report: jest.fn(() => initialValuesReport)
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        onSelectedElement: jest.fn(),
        resetFilter: jest.fn(),
        resetField: jest.fn(),
        resetDateRange: jest.fn(),
        handleSubmit: callback => callback(formValues)
    };

    const enzymeWrapper = mount(<Provider store={store} >
        <Approval {...props} /></Provider>);

    return { store, enzymeWrapper, props };
}


describe('Report OfficialDeltaNode Approval', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.navigateTo = jest.fn();
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });
    
    it('should call componentDidUpdate and execute showReopenModel', () => {
        const props = {
            approveToggler: true,
            reopenToggler: false,
            showReopenModel: jest.fn()
        }
        const enzymeWrapper = shallow(<ApprovalComponent {...props} />)
        enzymeWrapper.setProps(Object.assign({}, props, { reopenToggler: true }));
        expect(props.showReopenModel.mock.calls).toHaveLength(1);
    });
    
    it('should call componentDidUpdate and go to \'officialdeltapernode/manage\'', () => {
        const props = {
            approveToggler: false,
            reopenToggler: false,
            sendForApprovalResponse: {
                isValidOfficialDeltaNode: true
            }
        }
        navigationService.navigateToModule = jest.fn();
        const enzymeWrapper = shallow(<ApprovalComponent {...props} />)
        enzymeWrapper.setProps(Object.assign({}, props, { approveToggler: true }));
        expect(navigationService.navigateToModule.mock.calls).toHaveLength(1);
    });
    
    it('should call componentDidUpdate and throw error', () => {
        const props = {
            approveToggler: false,
            reopenToggler: false,
            sendForApprovalResponse: {
                isValidOfficialDeltaNode: false,
                isApproverExist: false
            },
            showError: jest.fn()
        }
        navigationService.navigateToModule = jest.fn();
        const enzymeWrapper = shallow(<ApprovalComponent {...props} />)
        enzymeWrapper.setProps(Object.assign({}, props, { approveToggler: true }));
        expect(props.showError.mock.calls).toHaveLength(1);
    });
});

