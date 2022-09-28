import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import OwnershipNodeGrid, { OwnershipNodeGrid as OwnershipNodeGridComponent } from './../../../../modules/transportBalance/nodeOwnership/components/ownershipNodeGrid.jsx';
import { navigationService } from './../../../../common/services/navigationService';
import { httpService } from './../../../../common/services/httpService';
import { constants } from './../../../../common/services/constants';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import Grid from './../../../../common/components/grid/grid.jsx';
import { optionService } from '../../../../common/services/optionService';
import { systemConfigService } from '../../../../common/services/systemConfigService';

function mountWithRealStore() {
    const dataGrid = {
        ownershipNodes: {
            config: {
                apiUrl: ''
            },
            items: [
                {
                    ticketId: 1,
                    ticketStartDate: '01/02/2020',
                    ticketFinalDate: '01/02/2020',
                    cutoffExecutionDate: '01/02/2020',
                    createdBy: 'test',
                    nodeName: 'testNode',
                    segment: 'testSegment',
                    state: constants.OwnershipNodeStatusType.NOTCONCILIATED
                },
                {
                    ticketId: 2,
                    ticketStartDate: '01/02/2020',
                    ticketFinalDate: '01/02/2020',
                    cutoffExecutionDate: '01/02/2020',
                    createdBy: 'test',
                    nodeName: 'testNode',
                    segment: 'testSegment',
                    state: constants.OwnershipNodeStatusType.NOTCONCILIATED
                }
            ],
            refreshToggler: true,
            receiveDataToggler: false,
            pageFilters: {}
        }
    };

    const nodeOwnership = {
        ownershipNode: {
            lastTicketPerSegment: [
                { ticketId: 1 }
            ]
        }
    };

    const grid = { ownershipNodes: dataGrid.ownershipNodes };

    const props = {
        onEdit: jest.fn(() => Promise.resolve()),
        onView: jest.fn(() => Promise.resolve()),
        onDownload: jest.fn(() => Promise.resolve()),
        saveCutOffReportFilter: jest.fn(() => Promise.resolve())
    };

    const reducers = {
        grid: jest.fn(() => grid),
        nodeOwnership: jest.fn(() => nodeOwnership)
    };

    const store = createStore(combineReducers(reducers));
    const dispatchSpy = jest.spyOn(store, 'dispatch');

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <OwnershipNodeGrid name="ownershipNodes" />
    </Provider>);
    return { store, enzymeWrapper, props, dispatchSpy };
}

describe('ownershipNodes Grid', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.getParamByName = jest.fn();
        navigationService.navigateToModule = jest.fn(() => Promise.resolve());
        systemConfigService.getDefaultOwnershipNodeLastDays = jest.fn(() => {
            return 10;
        });
        systemConfigService.getDefaultTransportFileUploadLastDays = jest.fn(() => {
            return 10;
        });
        optionService.getOwnershipNodeStateTypes = jest.fn(() => [{ label: 'ownershipoption', value: 'Propiedad' }]);
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('ownershipNodes');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(9);
    });
    it('should call edit actions on click of buttons', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('[id^="lnk_ownershipNodes_editOwnership"]').at(0).simulate('click');
        expect(props.onEdit.mock.calls).toHaveLength(0);
        enzymeWrapper.find('[id^="lnk_ownershipNodes_viewReport"]').at(0).simulate('click');
        expect(props.saveCutOffReportFilter.mock.calls).toHaveLength(0);
        enzymeWrapper.find('[id^="lnk_ownershipNodes_viewError"]').at(0).simulate('click');
        expect(props.onView.mock.calls).toHaveLength(0);
    });
    it('should call REQUEST_CONCILIATION_NODE action when execute manual conciliation', () => {
        const { enzymeWrapper, dispatchSpy } = mountWithRealStore();
        enzymeWrapper.find('[id^="lnk_ownershipNodes_manualConciliation_0"]').at(0).simulate('click');

        expect(dispatchSpy).toHaveBeenCalledWith(expect.objectContaining({ type: 'REQUEST_CONCILIATION_NODE' }));
    });
    it('should update the data grid when the conciliation is successful', () => {
        const props = {
            refreshGrid: jest.fn(),
            requestLastOperationalTicket: jest.fn()
        };

        const enzymeWrapper = shallow(<OwnershipNodeGridComponent name="ownershipNodes" {...props} />);
        enzymeWrapper.setProps(Object.assign({}, props, { conciliationSuccessToggler: true }));

        expect(props.refreshGrid.mock.calls).toHaveLength(1);
    });
    it('should display a error modal when the manual conciliation is failure', () => {
        const props = {
            confirmModal: jest.fn(),
            requestLastOperationalTicket: jest.fn()
        };

        const enzymeWrapper = shallow(<OwnershipNodeGridComponent name="ownershipNodes" {...props} />);
        enzymeWrapper.setProps(Object.assign({}, props, { conciliationErrorToggler: true }));

        expect(props.confirmModal.mock.calls).toHaveLength(1);
    });
});
