import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import { navigationService } from '../../../../../common/services/navigationService';
import { SentToSapReport as SentToSapReportComponent } from '../../../../../modules/report/sentToSap/components/report.jsx';
import { constants } from '../../../../../common/services/constants';
import { dateService } from '../../../../../common/services/dateService.js';

const initialValues = {
    filters: {
        executionId: 10,
        categoryName: 'Segmento',
        elementName: 'SegmentName',
        nodeName: 'Todos',
        initialDate: dateService.parse('12/09/2019'),
        finalDate: dateService.parse('18/09/2019')
    },
    execution: {
        categoryName: 'Segmento',
        elementName: 'Estación',
        nodeName: 'Todos',
        initialDate: '12/09/2019',
        finalDate: '18/09/2019',
        reportType: constants.Report.SendToSapStatesReport
    },
    reportToggler: false
};

describe('SentToSapReport', () => {
    beforeEach(() => {
        navigationService.navigateTo = jest.fn();
        navigationService.getParamByName = jest.fn(key => key);
    });

    it('should mount component is successful', () => {
        const props = {
            filters: Object.assign({}, initialValues.filters),
            execution: Object.assign({}, initialValues.execution),
            reportToggler: initialValues.reportToggler,
            getReportDetails: jest.fn(),
            buildReportFilters: jest.fn()
        };

        const enzymeWrapper = shallow(<SentToSapReportComponent {...props} />);
        expect(enzymeWrapper).toHaveLength(1);
    });
    it('should redirect to manage - data is invalid', () => {
        navigationService.getParamByName = jest.fn(() => undefined);

        const props = {
            filters: Object.assign({}, initialValues.filters),
            execution: Object.assign({}, initialValues.execution),
            reportToggler: initialValues.reportToggler,
            getReportDetails: jest.fn(),
            buildReportFilters: jest.fn()
        };

        const enzymeWrapper = shallow(<SentToSapReportComponent {...props} />);
        expect(enzymeWrapper).toHaveLength(1);
        expect(navigationService.navigateTo.mock.calls).toHaveLength(1);
    });
    it('should call buildReportFilters when receive execution data', () => {
        const props = {
            filters: null,
            execution: null,
            reportToggler: false,
            getReportDetails: jest.fn(),
            buildReportFilters: jest.fn()
        };

        const enzymeWrapper = shallow(<SentToSapReportComponent {...props} />);
        enzymeWrapper.setProps(Object.assign({}, props, {
            filters: Object.assign({}, initialValues.filters),
            execution: Object.assign({}, initialValues.execution),
            reportToggler: true,
        }));
        expect(props.buildReportFilters.mock.calls).toHaveLength(1);
    });
    it('should not call buildReportFilters when not receiving execution data', () => {
        const props = {
            filters: Object.assign({}, initialValues.filters),
            execution: Object.assign({}, initialValues.execution),
            reportToggler: false,
            getReportDetails: jest.fn(),
            buildReportFilters: jest.fn()
        };

        const enzymeWrapper = shallow(<SentToSapReportComponent {...props} />);
        enzymeWrapper.setProps(Object.assign({}, props));
        expect(props.buildReportFilters.mock.calls).toHaveLength(0);
    });
});