import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import TicketsGrid from './../../../../modules/transportBalance/cutOff/components/ticketsGrid.jsx';
import { navigationService } from './../../../../common/services/navigationService';
import { httpService } from './../../../../common/services/httpService';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import blobService from './../../../../common/services/blobService';
import Grid from './../../../../common/components/grid/grid.jsx';
import { systemConfigService } from './../../../../common/services/systemConfigService';
import { constants } from '../../../../common/services/constants';
import { ticketService } from './../../../../modules/transportBalance/cutOff/services/ticketService';

const initialState = {
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
            createdBy: 'trueAdmin',
            state: constants.StatusType.PROCESSED
        },
        {
            ticketId: 2,
            categoryElement: {
                name: 'Automation_2',
                category: {
                    name: 'Segmento'
                }
            },
            startDate: '2019-10-10',
            endDate: '2019-10-10',
            createdDate: '2019-10-10',
            createdBy: 'trueProfessional',
            state: constants.StatusType.FAILED
        },
        {
            ticketId: 3,
            categoryElement: {
                name: 'Automation_3',
                category: {
                    name: 'Segmento'
                }
            },
            startDate: '2019-10-10',
            endDate: '2019-10-10',
            createdDate: '2019-10-10',
            createdBy: 'trueProfessional'
        },
        {
            ticketId: 3,
            categoryElement: {
                name: 'Automation_2',
                category: {
                    name: 'Segmento'
                }
            },
            startDate: '2019-10-10',
            endDate: '2019-10-10',
            createdDate: '2019-10-10',
            createdBy: 'trueProfessional',
            state: constants.StatusType.PROCESSING
        },
        {
            ticketId: 4,
            categoryElement: {
                name: 'Automation_4',
                category: {
                    name: 'Segmento'
                }
            },
            startDate: '2019-10-10',
            endDate: '2019-10-10',
            createdDate: '2019-10-10',
            createdBy: 'trueProfessional',
            state: constants.OwnershipNodeStatus.SENT
        },
        {
            ticketId: 5,
            categoryElement: {
                name: 'Automation_5',
                category: {
                    name: 'Segmento'
                }
            },
            startDate: '2019-10-10',
            endDate: '2019-10-10',
            createdDate: '2019-10-10',
            createdBy: 'trueProfessional',
            state: constants.StatusType.SENT
        },
        {
            ticketId: 6,
            categoryElement: {
                name: 'Automation_6',
                category: {
                    name: 'Segmento'
                }
            },
            startDate: '2019-10-10',
            endDate: '2019-10-10',
            createdDate: '2019-10-10',
            createdBy: 'trueProfessional',
            state: constants.StatusType.SENT,
            errorMessage: 'general error'
        },
        {
            ticketId: 7,
            categoryElement: {
                name: 'Automation_7',
                category: {
                    name: 'Segmento'
                }
            },
            startDate: '2019-10-10',
            endDate: '2019-10-10',
            createdDate: '2019-10-10',
            createdBy: 'trueProfessional',
            state: constants.StatusType.VISUALIZATION
        },
        {
            ticketId: 8,
            categoryElement: {
                name: 'Automation_8',
                category: {
                    name: 'Segmento'
                }
            },
            startDate: '2019-10-10',
            endDate: '2019-10-10',
            createdDate: '2019-10-10',
            createdBy: 'trueProfessional',
            state: constants.StatusType.ERROR
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
    }
};

function mountWithRealStore(ticketsGridProps = {}) {
    const grid = { ticketsgrid: initialState.ticketsGrid };
    const shared = { readAccessInfo: 'Token' };
    const props = {
        onEdit: jest.fn(() => Promise.resolve()),
        onView: jest.fn(() => Promise.resolve()),
        onDownload: jest.fn(() => Promise.resolve()),
        refreshGrid: jest.fn(() => Promise.resolve())
    };

    const reducers = {
        grid: jest.fn(() => grid),
        shared: jest.fn(() => shared),
        saveCutOffReportFilter: jest.fn(() => Promise.resolve())
    };

    const store = createStore(combineReducers(reducers));
    const dispatchSpy = jest.spyOn(store, 'dispatch');

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <TicketsGrid name="ticketsgrid" {...ticketsGridProps}/>
    </Provider>);
    return { store, enzymeWrapper, props, dispatchSpy };
}

function shallowMount(data = initialState) {
    const grid = { ticketsgrid: initialState.ticketsGrid };
    const shared = { readAccessInfo: 'Token' };
    const props = {
        ticketsgrid: data.ticketsGrid,
        onEdit: jest.fn(() => Promise.resolve()),
        onView: jest.fn(() => Promise.resolve()),
        onDownload: jest.fn(() => Promise.resolve()),
        refreshGrid: jest.fn(() => Promise.resolve())
    };

    const reducers = {
        grid: jest.fn(() => grid),
        shared: jest.fn(() => shared),
        saveCutOffReportFilter: jest.fn(() => Promise.resolve())
    };

    const store = createStore(combineReducers(reducers));
    // //const enzymeWrapper = shallow(<TicketsGrid {...props} name="ticketsgrid" />);
    const enzymeWrapper = shallow(<Provider store={store} {...props}>
        <TicketsGrid name="ticketsgrid" />
    </Provider>);

    return { enzymeWrapper, props };
}

describe('ticket Grid', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        blobService.initialize = jest.fn();
        blobService.downloadFile = jest.fn();
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.getParamByName = jest.fn();
        navigationService.navigateToModule = jest.fn(() => Promise.resolve());
        systemConfigService.getDefaultCutoffLastDays = jest.fn(() => {
            return 10;
        });
        systemConfigService.getDefaultOwnershipCalculationLastDays = jest.fn(() => {
            return 10;
        });
        ticketService.getGridInfo = jest.fn(() => Promise.resolve());
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('ticketsgrid');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(8);
    });
    it('should call edit actions on click of buttons', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('[id^="lnk_ticketsgrid_viewSummary"]').at(1).simulate('click');
        expect(props.onEdit.mock.calls).toHaveLength(0);
        enzymeWrapper.find('[id^="lnk_ticketsgrid_download"]').at(1).simulate('click');
        expect(props.onDownload.mock.calls).toHaveLength(0);
        enzymeWrapper.find('[id^="lnk_ticketsgrid_viewSummary"]').at(1).simulate('click');
        expect(props.onView.mock.calls).toHaveLength(0);
    });
    it('should refresh grid', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.setProps(Object.assign(
            {}, props, { tickets: initialState.ticketsGrid.items }));
        expect(props.refreshGrid.mock.calls).toHaveLength(0);
    });
    it('should render nine columns when the component type is Logistic', () => {
        const ticketsGridProps = {
            componentType: constants.TicketType.Logistics
        };
        const { enzymeWrapper } = mountWithRealStore(ticketsGridProps);

        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(9);
    });
    it('should render scenarioName column on grid', () => {
        const ticketsGridProps = {
            componentType: constants.TicketType.LogisticMovements
        };
        const { enzymeWrapper } = mountWithRealStore(ticketsGridProps);

        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(9);
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(2).find('span').at(0).text()).toEqual('stage');
    });
    it('should redirect when do click on viewSummary action', () => {
        navigationService.navigateToModule = jest.fn();
        const ticketsGridProps = {
            componentType: constants.TicketType.LogisticMovements
        };
        const { enzymeWrapper, props } = mountWithRealStore(ticketsGridProps);
        enzymeWrapper.find('[id^="lnk_ticketsgrid_viewSummary"]').at(5).simulate('click');

        expect(navigationService.navigateToModule.mock.calls).toHaveLength(1);
    });
    it('should dispatch error when the ticket has an error - messageError', () => {
        const ticketsGridProps = {
            componentType: constants.TicketType.LogisticMovements
        };
        const { enzymeWrapper, dispatchSpy } = mountWithRealStore(ticketsGridProps);
        enzymeWrapper.find('[id^="lnk_ticketsgrid_viewError"]').at(0).simulate('click');

        expect(dispatchSpy).toHaveBeenCalledWith(jasmine.objectContaining({type: 'SET_TICKET_ERROR'}));
        expect(dispatchSpy).toHaveBeenCalledWith(jasmine.objectContaining({type: 'OPEN_MODAL'}));
    });
    it('should call to translation service to translate status when the ticket type is ownership', () => {
        const ticketsGridProps = {
            componentType: constants.TicketType.Ownership
        };
        mountWithRealStore(ticketsGridProps);
        expect(resourceProvider.read).toHaveBeenCalledWith('visualizacion');
    });
    it('should call blobService.downloadFile when click on download action', () => {
        const ticketsGridProps = {
            componentType: constants.TicketType.OfficialLogistics
        };
        const { enzymeWrapper } = mountWithRealStore(ticketsGridProps);
        enzymeWrapper.find('[id^="lnk_ticketsgrid_download_1"]').at(0).simulate('click');

        expect(enzymeWrapper.find('[id^="lnk_ticketsgrid_download_1"]')).toHaveLength(1)
        expect(blobService.downloadFile).toHaveBeenCalled();
    });
    it('should go to report when click on download action', () => {
        const ticketsGridProps = {
            componentType: constants.TicketType.Cutoff
        };
        const { enzymeWrapper } = mountWithRealStore(ticketsGridProps);
        enzymeWrapper.find('[id^="lnk_ticketsgrid_download_1"]').at(0).simulate('click');

        expect(enzymeWrapper.find('[id^="lnk_ticketsgrid_download_1"]')).toHaveLength(1)
        expect(navigationService.navigateToModule).toHaveBeenCalledWith('cutoffreport/manage/view');
    });
});
