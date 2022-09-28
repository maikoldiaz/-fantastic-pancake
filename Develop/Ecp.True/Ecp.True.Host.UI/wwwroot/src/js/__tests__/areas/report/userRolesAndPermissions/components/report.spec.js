import setup from '../../../setup.js';
import React from 'react';
import { shallow } from 'enzyme';
import { UserRolesAndPermissionsReport as UserRolesAndPermissionsReportComponent } from '../../../../../modules/report/userRolesAndPermissions/components/report.jsx';
import { dateService } from '../../../../../common/services/dateService';
import { navigationService } from '../../../../../common/services/navigationService';

const initialValues = {
  filters: {
    executionId: 10,
    initialDate: dateService.parse('12/09/2019'),
    finalDate: dateService.parse('18/09/2019')
  }
};

describe('UserRolesAndPermissionsReport', () => {
  beforeAll(() => {
    navigationService.navigateTo = jest.fn();
    navigationService.getParamByName = jest.fn(() => 'executionId1');
  });

  it('should mount component is successful', () => {
    const props = {
      getReportDetails: jest.fn()
    };

    const enzymeWrapper = shallow(<UserRolesAndPermissionsReportComponent {...props} />);
    expect(enzymeWrapper).toHaveLength(1);
  });

  it('should redirect to manage - executionId not exists', () => {
    navigationService.getParamByName = jest.fn();
    const props = {
      filters: Object.assign({}, initialValues.filters),
      getReportDetails: jest.fn()
    };

    shallow(<UserRolesAndPermissionsReportComponent {...props} />);
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

    const enzymeWrapper = shallow(<UserRolesAndPermissionsReportComponent {...props} />);
    enzymeWrapper.setProps(Object.assign({}, props, {
      filters: Object.assign({}, initialValues.filters),
      reportToggler: true,
    }));
    expect(props.buildReportFilters.mock.calls).toHaveLength(1);
  });
});