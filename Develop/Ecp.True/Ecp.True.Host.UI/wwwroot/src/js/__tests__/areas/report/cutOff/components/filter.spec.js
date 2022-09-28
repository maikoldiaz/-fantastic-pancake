import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import { navigationService } from '../../../../../common/services/navigationService';
import CutoffReportFilter, { CutoffReportFilter as CutoffReportFilterComponent } from '../../../../../modules/report/cutOff/components/filter.jsx';

const initialValues = {
    cutOffReport: {
        selectedCategory: {},
        selectedElement: {},
        searchedNodes: []
    }
};

const sharedInitialState = {
    categoryElements: [],
    allCategories: []
};

function mountWithRealStore() {
    const reducers = {
        formValues: jest.fn(() => initialValues.cutOffReport),
        saveCutOffReportFilter: jest.fn(() => 'filters'),
        form: formReducer,
        shared: jest.fn(() => sharedInitialState),
        report: jest.fn(() => initialValues)
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        showError: jest.fn(),
        handleSubmit: jest.fn(),
        type: 'cutOffReport'
    };

    const enzymeWrapper = mount(<Provider store={store} >
        <CutoffReportFilter {...props} />
    </Provider>);

    return { store, enzymeWrapper, props };
}

describe('CutoffReportFilter', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('radio-button-group should exists', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#r_reportFilter_type')).toBe(true);
    });

    it('should process form submit', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('form').simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(2);
    });

    it('should call showModal on reportToggler', () => {
        const props = {
            showError: jest.fn(),
            handleSubmit: jest.fn(),
            reportToggler: true,
            type: 'cutOffReport',
            showModal: jest.fn(),
            allCategories: [],
            initialValues: initialValues,
            showWarning: jest.fn(),
            getCategories: jest.fn(),
            getCategoryElements: jest.fn(),
            cutOffReportResetFields: jest.fn()
        };

        const wrapper = shallow(<CutoffReportFilterComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { reportToggler: false }));
        expect(props.showModal.mock.calls).toHaveLength(1);
    });

    it('should call showModal on errorSaveCutOffToggler', () => {
        const props = {
            showError: jest.fn(),
            handleSubmit: jest.fn(),
            errorSaveCutOffToggler: true,
            type: 'cutOffReport',
            showModal: jest.fn(),
            allCategories: [],
            initialValues: initialValues,
            showWarning: jest.fn(),
            getCategories: jest.fn(),
            getCategoryElements: jest.fn(),
            cutOffReportResetFields: jest.fn()
        };

        const wrapper = shallow(<CutoffReportFilterComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { errorSaveCutOffToggler: false }));
        expect(props.showModal.mock.calls).toHaveLength(1);
    });

    it('should call navigateToModule on navigateToggler', () => {
        navigationService.navigateToModule = jest.fn();
        const props = {
            showError: jest.fn(),
            handleSubmit: jest.fn(),
            navigateToggler: true,
            type: 'cutOffReport',
            showModal: jest.fn(),
            allCategories: [],
            initialValues: initialValues,
            showWarning: jest.fn(),
            getCategories: jest.fn(),
            getCategoryElements: jest.fn(),
            cutOffReportResetFields: jest.fn()
        };

        const wrapper = shallow(<CutoffReportFilterComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { navigateToggler: false }));
        expect(navigationService.navigateToModule.mock.calls).toHaveLength(1);
    });

    it('should call resetClear on clearSelectedNodeToggler', () => {
        navigationService.navigateToModule = jest.fn();
        const props = {
            showError: jest.fn(),
            handleSubmit: jest.fn(),
            clearSelectedNodeToggler: true,
            type: 'cutOffReport',
            showModal: jest.fn(),
            allCategories: [],
            initialValues: initialValues,
            showWarning: jest.fn(),
            getCategories: jest.fn(),
            getCategoryElements: jest.fn(),
            cutOffReportResetFields: jest.fn(),
            resetClear: jest.fn()
        };

        const wrapper = shallow(<CutoffReportFilterComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { clearSelectedNodeToggler: false }));
        expect(props.resetClear.mock.calls).toHaveLength(1);
    });

    it('should clear status and hide errors when component will unmount', () => {
        navigationService.navigateToModule = jest.fn();
        const props = {
            showError: jest.fn(),
            handleSubmit: jest.fn(),
            clearSelectedNodeToggler: true,
            type: 'cutOffReport',
            showModal: jest.fn(),
            allCategories: [],
            initialValues: initialValues,
            showWarning: jest.fn(),
            getCategories: jest.fn(),
            getCategoryElements: jest.fn(),
            cutOffReportResetFields: jest.fn(),
            resetClear: jest.fn(),
            clearStatus: jest.fn(),
            hideError: jest.fn()
        };

        const wrapper = shallow(<CutoffReportFilterComponent {...props} />);
        wrapper.unmount()
        expect(props.clearStatus.mock.calls).toHaveLength(1);
        expect(props.hideError.mock.calls).toHaveLength(1);
    });
});
