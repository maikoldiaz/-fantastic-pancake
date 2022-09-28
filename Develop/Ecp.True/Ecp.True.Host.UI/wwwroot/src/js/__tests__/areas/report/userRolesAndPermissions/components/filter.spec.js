import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { optionService } from '../../../../../common/services/optionService';
import { navigationService } from '../../../../../common/services/navigationService';
import UserRolesAndPermissionsFilter, { UserRolesAndPermissionsFilter as UserRolesAndPermissionsFilterComponent } from '../../../../../modules/report/userRolesAndPermissions/components/filter.jsx';

const initialValues = {
  report: {
    userRolesAndPermissions: {
      receiveReportToggler: false,
      failureReportToggler: false
    }
  },
  form: {
    reportType: {
      name: 'report type 1',
      value: 'report name 1'
    }
  }
};

function mountWithRealStore() {
  const reducers = {
    report: jest.fn(() => initialValues.report)
  };

  const props = {
    handleSubmit: jest.fn(callback => callback(initialValues.form))
  }

  const store = createStore(combineReducers(reducers));
  const dispatchSpy = jest.spyOn(store, 'dispatch');

  const enzymeWrapper = mount(<Provider store={store} >
    <UserRolesAndPermissionsFilter {...props} />
  </Provider>);

  return { store, enzymeWrapper, dispatchSpy };
}

describe('UserRolesAndPermissionsFilter', () => {
  beforeAll(() => {
    optionService.getUserRolesAndPermissionsReportTypes = jest.fn(() => ([
      { label: 'report type 1', value: 'report name 1' },
      { label: 'report type 2', value: 'report name 2' }
    ]));
    navigationService.navigateToModule = jest.fn();
  });

  it('should mount successfully', () => {
    const { enzymeWrapper } = mountWithRealStore();
    expect(enzymeWrapper).toHaveLength(1);
  });

  it('should process form submit', () => {
    const { enzymeWrapper, dispatchSpy } = mountWithRealStore();
    enzymeWrapper.find('form').simulate('submit');
    expect(dispatchSpy).toHaveBeenCalledWith(expect.objectContaining({ type: 'REQUEST_USER_ROLE_PERMISSION_REPORT' }));
  });

  it('should redirect to view when receive receiveReportToggler', () => {
    const props = {
      form: 'UserRolesAndPermissions',
      receiveReportToggler: false,
      handleSubmit: jest.fn(),
      openConfirmModal: jest.fn()
    };

    const wrapper = shallow(<UserRolesAndPermissionsFilterComponent {...props} />);
    wrapper.setProps(Object.assign({}, props, { receiveReportToggler: true }));
    expect(props.openConfirmModal.mock.calls).toHaveLength(1);
  });

  it('should call error modal when receive failureReportToggler', () => {
    const props = {
      form: 'UserRolesAndPermissions',
      failureReportToggler: false,
      confirm: jest.fn(),
      handleSubmit: jest.fn()
    };

    const wrapper = shallow(<UserRolesAndPermissionsFilterComponent {...props} />);
    wrapper.setProps(Object.assign({}, props, { failureReportToggler: true }));
    expect(props.confirm.mock.calls).toHaveLength(1);
  });
});
