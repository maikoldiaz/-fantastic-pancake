import setup from '../../../setup.js';
import React from 'react';
import { shallow } from 'enzyme';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import { navigationService } from '../../../../../common/services/navigationService';
import { OfficialNodesStatusReportFilter as OfficialNodesStatusReportFilterComponent } from '../../../../../modules/report/officialNodesStatus/components/filter.jsx';

describe('OfficialNodesStatusReportFilter', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.navigateTo = jest.fn();
    });

    it('should mount successfully', () => {
        const props = {
            categoryElements: [{
                isActive: true,
                categoryId: 2
            }],
            segment: {},
            officialPeriods: [],
            handleSubmit: jest.fn(callback => callback),
            getCategoryElements: jest.fn(),
        };

        const wrapper = shallow(<OfficialNodesStatusReportFilterComponent {...props} />);
        expect(wrapper).toHaveLength(1);
    });

    it('should call saveOfficialNodeStatusReport when the form is submmited', () => {
        const props = {
            categoryElements: [{
                isActive: true,
                categoryId: 2
            }],
            segment: {},
            officialPeriods: [],
            handleSubmit: jest.fn(callback => callback({
                segment: {
                    categoryId: 2,
                    elementId: 12,
                    name: 'segment-name-1',
                    category: {
                        name: 'segment'
                    }
                },
                periods: {
                    start: new Date(),
                    end: new Date()
                }
            })),
            getCategoryElements: jest.fn(),
            requestOfficialNodeStatusReport: jest.fn(),
            saveOfficialNodeStatusFilter: jest.fn()
        };

        const wrapper = shallow(<OfficialNodesStatusReportFilterComponent {...props} />);
        wrapper.find('form').at(0).simulate('click');

        expect(props.requestOfficialNodeStatusReport.mock.calls).toHaveLength(1);
        expect(props.saveOfficialNodeStatusFilter.mock.calls).toHaveLength(1);
    });

    it('should not call requestOfficialPeriodRange - not exist segment', () => {
        const props = {
            categoryElements: [{
                isActive: true,
                categoryId: 2
            }],
            segment: {},
            officialPeriods: [],
            handleSubmit: jest.fn(callback => callback),
            getCategoryElements: jest.fn(),
            requestOfficialPeriodRange: jest.fn(),
            resetOfficialNodeStatusPeriods: jest.fn(),
            resetField: jest.fn()
        };

        const wrapper = shallow(<OfficialNodesStatusReportFilterComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, {
            segmentChangeToggler: true,
            segment: null
        }));
        expect(props.requestOfficialPeriodRange.mock.calls).toHaveLength(0);
    });

    it('should call requestOfficialPeriodRange - segment exist', () => {
        const props = {
            categoryElements: [{
                isActive: true,
                categoryId: 2
            }],
            segment: {},
            officialPeriods: [],
            handleSubmit: jest.fn(callback => callback),
            getCategoryElements: jest.fn(),
            requestOfficialPeriodRange: jest.fn(),
            resetOfficialNodeStatusPeriods: jest.fn(),
            resetField: jest.fn()
        };

        const wrapper = shallow(<OfficialNodesStatusReportFilterComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, {
            segmentChangeToggler: true,
            segment: { categoryId: 2, elementId: 12 }
        }));
        expect(props.requestOfficialPeriodRange.mock.calls).toHaveLength(1);
    });

    it('should redirect to view when the reportExecution was saved', () => {
        const props = {
            categoryElements: [{
                isActive: true,
                categoryId: 2
            }],
            segment: {},
            officialPeriods: [],
            handleSubmit: jest.fn(callback => callback),
            getCategoryElements: jest.fn(),
        };

        const wrapper = shallow(<OfficialNodesStatusReportFilterComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { receiveOfficialNodeStatusToggler: true }));
        expect(navigationService.navigateTo.mock.calls).toHaveLength(1);
    });

    it('should show an error modal when the save request has an error', () => {
        const props = {
            categoryElements: [{
                isActive: true,
                categoryId: 2
            }],
            segment: {},
            officialPeriods: [],
            handleSubmit: jest.fn(callback => callback),
            getCategoryElements: jest.fn(),
            showTechnicalError: jest.fn()
        };

        const wrapper = shallow(<OfficialNodesStatusReportFilterComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { failureOfficialNodeStatusToggler: true }));
        expect(props.showTechnicalError.mock.calls).toHaveLength(1);
    });
});
