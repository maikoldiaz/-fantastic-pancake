import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import OwnershipTickets, { OwnershipTickets as OwnershipTicketsComponent } from '../../../../modules/transportBalance/ownership/components/ownershipTickets.jsx';
import { constants } from '../../../../common/services/constants';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { ticketService } from './../../../../modules/transportBalance/cutOff/services/ticketService';

const initialValue = {
  ticketsGrid: {
    config: {
      apiUrl: ''
    },
    items: [{
      ticketId: 1,
      categoryElement: {
        name: 'Automation_1',
        category: {
          name: 'Segmento'
        }
      },
      startDate: '2019-10-10',
      endDate: '2019-10-10',
      createdDate: '2019-10-10',
      createdBy: 'trueProfessional',
      state: constants.StatusType.CONCILIATIONFAILED
    }],
    refreshToggler: true,
    receiveDataToggler: false,
    pageFilters: {},
    gridInfo:
    {
      filter: `ticketTypeId eq 'Cutoff'`,
      title: 'ticketErrorDetailTitle',
      popupName: 'showErrorPopUp',
      days: 10
    }
  },
  ownership: {
    conciliationErrorToggler: undefined,
    conciliationSuccessToggler: undefined,
    lastOperationalConciliationTicket: [{
      ticketId: 1
    }]
  },
  shared: { readAccessInfo: 'Token' }
};

function mountWithRealStore() {
  const reducers = {
    grid: jest.fn(() => ({ ownershipTicketsgrid: initialValue.ticketsGrid })),
    shared: jest.fn(() => initialValue.shared),
    ownership: jest.fn(() => initialValue.ownership)
  };

  const store = createStore(combineReducers(reducers));
  const dispatchSpy = jest.spyOn(store, 'dispatch');
  const enzymeWrapper = mount(<Provider store={store} >
    <OwnershipTickets name="ownershipTicketsgrid" />
  </Provider>);
  return { store, enzymeWrapper, dispatchSpy };
}

describe('<OwnershipTickets />', () => {
  beforeAll(() => {
    systemConfigService.getDefaultCutoffLastDays = jest.fn(() => 10);
    systemConfigService.getDefaultDeltaTicketLastDays = jest.fn(() => 10);
    systemConfigService.getDefaultLogisticsTicketLastDays = jest.fn(() => 10);
    systemConfigService.getDefaultOfficialDeltaTicketLastDays = jest.fn(() => 10);
    systemConfigService.getDefaultOwnershipCalculationLastDays = jest.fn(() => 10);
    ticketService.getGridInfo = jest.fn(() => Promise.resolve());
  });

  it('should call SAVE_SELECTED_OWNERSHIP_TICKET action when execute manual conciliation', () => {
    const { enzymeWrapper, dispatchSpy } = mountWithRealStore();

    enzymeWrapper.find('[id^="lnk_ownershipTicketsgrid_manualConciliation_1"]').at(0).simulate('click');

    expect(dispatchSpy).toHaveBeenCalledWith(expect.objectContaining({ type: 'SAVE_SELECTED_OWNERSHIP_TICKET' }));
  });
  it('should display a modal when execute conciliation detail', () => {
    const { enzymeWrapper, dispatchSpy } = mountWithRealStore();

    enzymeWrapper.find('[id^="lnk_ownershipTicketsgrid_viewConciliationError_1"]').at(0).simulate('click');

    expect(dispatchSpy).toHaveBeenCalledWith(expect.objectContaining({ type: 'OPEN_MODAL' }));
  });
  it('should update the data grid when the conciliation is successful', () => {
    const props = {
      refreshGrid: jest.fn(),
      requestLastOperationalTicket: jest.fn()
    };

    const enzymeWrapper = shallow(<OwnershipTicketsComponent name="ownershipTicketsgrid" {...props} />);
    enzymeWrapper.setProps(Object.assign({}, props, { conciliationSuccessToggler: true }));

    expect(props.refreshGrid.mock.calls).toHaveLength(1);
  });
  it('should display a error modal when the manual conciliation is failure', () => {
    const props = {
      confirmModal: jest.fn(),
      requestLastOperationalTicket: jest.fn()
    };

    const enzymeWrapper = shallow(<OwnershipTicketsComponent name="ownershipTicketsgrid" {...props} />);
    enzymeWrapper.setProps(Object.assign({}, props, { conciliationErrorToggler: true }));

    expect(props.confirmModal.mock.calls).toHaveLength(1);
  });
  it('should display a error modal when ownershipNode request is failure', () => {
    const props = {
      confirmModal: jest.fn(),
      requestLastOperationalTicket: jest.fn()
    };

    const enzymeWrapper = shallow(<OwnershipTicketsComponent name="ownershipTicketsgrid" {...props} />);
    enzymeWrapper.setProps(Object.assign({}, props, {
      ownershipNodesErrorToggler: true
    }));

    expect(props.confirmModal.mock.calls).toHaveLength(1);
  });
  it('should display a error modal when some ownershipNode is transferPoint and state is APPROVED', () => {
    const props = {
      confirmModal: jest.fn(),
      requestLastOperationalTicket: jest.fn()
    };

    const enzymeWrapper = shallow(<OwnershipTicketsComponent name="ownershipTicketsgrid" {...props} />);
    enzymeWrapper.setProps(Object.assign({}, props, {
      ownershipNodesSuccessToggler: true,
      ownershipNodesData: [
        {
          isTransferPoint: false,
          state: constants.OwnershipNodeStatusType.CONCILIATED
        },
        {
          isTransferPoint: true,
          state: constants.OwnershipNodeStatusType.APPROVED
        }
      ]
    }));

    expect(props.confirmModal.mock.calls).toHaveLength(1);
  });
  it('should send the concilation request when some ownershipNode is transferPoint and state is REOPENED', () => {
    const props = {
      confirmModal: jest.fn(),
      requestLastOperationalTicket: jest.fn(),
      requestConciliation: jest.fn()
    };

    const enzymeWrapper = shallow(<OwnershipTicketsComponent name="ownershipTicketsgrid" {...props} />);
    enzymeWrapper.setProps(Object.assign({}, props, {
      ownershipNodesSuccessToggler: true,
      ownershipNodesData: [
        {
          isTransferPoint: false,
          state: constants.OwnershipNodeStatusType.CONCILIATED
        },
        {
          isTransferPoint: true,
          state: constants.OwnershipNodeStatusType.REOPENED
        }
      ]
    }));

    expect(props.requestConciliation.mock.calls).toHaveLength(1);
  });
});
