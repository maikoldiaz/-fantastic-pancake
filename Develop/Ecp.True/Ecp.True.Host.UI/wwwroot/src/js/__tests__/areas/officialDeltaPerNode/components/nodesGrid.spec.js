import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import NodesGrid, { NodesGrid as NodesGridComponent } from '../../../../modules/dailyBalance/officialDeltaPerNode/components/nodesGrid.jsx';
import Grid from '../../../../common/components/grid/grid.jsx';
import { constants } from '../../../../common/services/constants';
import { navigationService } from '../../../../common/services/navigationService';
import { systemConfigService } from '../../../../common/services/systemConfigService';

function mountWithRealStore() {
    const dataGrid = {
        nodeGridData: {
            nodesGridValues: {},
            nodeGridToggler: false,
            balancePerNode: 100,
            nodeFilter: {
                element: {
                    name: 'test'
                },
                node: {
                    name: 'nodeName',
                    nodeId: 123
                }
            }
        },
        nodesGrid: {
            config: {
                apiUrl: ''
            },
            items: [
                {
                    ticketId: 1,
                    startDate: '2019-10-10',
                    endDate: '2019-10-10',
                    executionDate: '2019-10-10',
                    createdBy: 'trueProfessional',
                    status: constants.OwnershipNodeStatus.SENT,
                    segment: 'segment',
                    nodeName: 'node'
                }
            ],
            pageFilters: {}
        }
    };
    const grid = { officialDeltaNodesGrid: dataGrid.nodesGrid };

    const props = {
        ticketId: '123',
        enableView: jest.fn(() => Promise.resolve()),
        enableDownload: jest.fn(() => Promise.resolve()),
        getStatus: jest.fn(() => Promise.resolve()),
        saveOfficialDeltaNodes: jest.fn(() => Promise.resolve())
    };

    const reducers = {
        officialDeltaNodesGrid: jest.fn(() => dataGrid.nodesGrid),
        grid: jest.fn(() => grid),
        officialDeltaPerNode: jest.fn(() => dataGrid.nodeGridData),
        report: jest.fn(() => dataGrid.nodeGridData),
        form: jest.fn(() => dataGrid.nodeGridData)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(
        <Provider store={store} {...props}>
            <NodesGrid />
        </Provider>
    );
    return { store, enzymeWrapper, props };
}

describe('Official DeltaNodes Grid', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        navigationService.getParamByName = jest.fn();
        navigationService.navigateToModule = jest.fn();
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        systemConfigService.getDefaultOfficialDeltaPerNodeLastDays = jest.fn(() => {
            return 10;
        });
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });
    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual(
            'officialDeltaNodesGrid'
        );
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(9);
    });

    it('should set report toggler and grid values on download', () => {
        const props = {
            setSource: jest.fn()
        };
        const wrapper = shallow(<NodesGridComponent {...props} />);
        wrapper.instance().onDownload({});
        expect(navigationService.navigateToModule.mock.calls).toHaveLength(1);
    });

    it('should show error when status is failed', () => {
        const props = {
            setReportToggler: jest.fn(),
            setGridValues: jest.fn(),
            showError: jest.fn()
        };
        const wrapper = shallow(<NodesGridComponent {...props} />);
        wrapper.instance().viewSummary({ status: constants.StatusType.FAILED });
        expect(props.showError.mock.calls).toHaveLength(1);
    });

    it('should not show error when status is not failed', () => {
        const props = {
            setReportToggler: jest.fn(),
            setGridValues: jest.fn(),
            showError: jest.fn()
        };
        const wrapper = shallow(<NodesGridComponent {...props} />);
        wrapper.instance().viewSummary({ status: constants.StatusType.DELTA });
        expect(props.showError.mock.calls).toHaveLength(0);
    });
});


