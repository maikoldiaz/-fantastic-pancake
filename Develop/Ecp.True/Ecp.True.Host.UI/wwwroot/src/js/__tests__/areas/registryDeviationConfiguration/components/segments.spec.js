import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import Segments, { Segments as SegmentsComponent }
  from '../../../../modules/administration/registryDeviationConfiguration/components/segments';

const initialValues = {
  registryDeviation: {
    categoryElements: [
      {
        elementId: 1,
        name: 'segment-1',
        deviationPercentage: 0.00
      },
      {
        elementId: 2,
        name: 'segment-2',
        deviationPercentage: 1.00
      },
      {
        elementId: 3,
        name: 'segment-3',
        deviationPercentage: 2.00
      }
    ],
    updatedDeviationToggler: false
  },
  root: {
    systemConfig: {
      maxDeviationPercentage: 3.00
    }
  }
};

function mountWithRealStore(ownProps = {}) {
  const reducers = {
    registryDeviation: jest.fn(() => initialValues.registryDeviation),
    root: jest.fn(() => initialValues.root)
  };

  const props = {
    handleSubmit: (callback) => callback({ segments: [] }),
    ...ownProps
  };

  const store = createStore(combineReducers(reducers));
  const dispatchSpy = jest.spyOn(store, 'dispatch');
  const enzymeWrapper = mount(<Provider store={store}>
    <Segments {...props} />
  </Provider>);
  return { store, enzymeWrapper, dispatchSpy };
}

describe('Segments - registry deviation configuration', () => {
  it('should mount successfully', () => {
    const { enzymeWrapper } = mountWithRealStore();
    expect(enzymeWrapper).toHaveLength(1);
  });

  it('should get filtered categories when search', () => {
    const { enzymeWrapper, dispatchSpy } = mountWithRealStore();
    enzymeWrapper.find('#filter-text-input').simulate('keydown', { keyCode: 13 });
    expect(dispatchSpy).toHaveBeenCalledWith(expect.objectContaining({ type: 'FILTER_CATEGORY_ELEMENTS' }));
  });

  it('should avoid submitting the form when user hit enter', () => {
    const { enzymeWrapper } = mountWithRealStore();
    const keyEvent = { keyCode: 13, preventDefault: jest.fn() };
    enzymeWrapper.find('form').simulate('keydown', keyEvent);
    expect(keyEvent.preventDefault).toHaveBeenCalled();
  });

  it('should submit a request when the form is submitted', () => {
    const componentProps = {
      handleSubmit: callback => callback({
        segments: [
          {
            elementId: 1,
            name: 'segment-1',
            deviationPercentage: 1.00
          },
          {
            elementId: 2,
            name: 'segment-2',
            deviationPercentage: 1.00
          },
          {
            elementId: 3,
            name: 'segment-3',
            deviationPercentage: 2.00
          }
        ]
      })
    };
    const { enzymeWrapper, dispatchSpy } = mountWithRealStore(componentProps);

    enzymeWrapper.find('form').simulate('submit');
    expect(dispatchSpy).toHaveBeenCalledWith(expect.objectContaining({ type: 'REQUEST_UPDATE_DEVIATION' }));
  });

  it('should not submit a request when the form has not changed values', () => {
    const componentProps = {
      handleSubmit: callback => callback({
        segments: [
          {
            elementId: 1,
            name: 'segment-1',
            deviationPercentage: 0.00
          },
          {
            elementId: 2,
            name: 'segment-2',
            deviationPercentage: 1.00
          },
          {
            elementId: 3,
            name: 'segment-3',
            deviationPercentage: 2.00
          }
        ]
      })
    };
    const { enzymeWrapper, dispatchSpy } = mountWithRealStore(componentProps);

    enzymeWrapper.find('form').simulate('submit');
    expect(dispatchSpy).not.toHaveBeenCalledWith(expect.objectContaining({ type: 'REQUEST_UPDATE_DEVIATION' }));
  });

  it('should submit a request with deviation and show a success alert', () => {
    const props = {
      initialValues: initialValues.registryDeviation.categoryElements,
      segments: initialValues.registryDeviation.categoryElements,
      updatedDeviationToggler: initialValues.registryDeviation.updatedDeviationToggler,
      maxDeviationPercentage: initialValues.root.systemConfig.maxDeviationPercentage,
      updateSegments: jest.fn(),
      updateDeviation: jest.fn(),
      showSuccess: jest.fn(),
      handleSubmit: jest.fn()
    };

    const wrapper = shallow(<SegmentsComponent {...props} />);
    wrapper.setProps(Object.assign({}, props, { updatedDeviationToggler: !props.updatedDeviationToggler }));
    expect(props.updateSegments.mock.calls).toHaveLength(2);
    expect(props.showSuccess.mock.calls).toHaveLength(1);
  });
});
