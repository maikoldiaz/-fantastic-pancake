import React from 'react';
import setup from '../../setup';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import OfficialPointsGrid, { OfficialPointsGrid as OfficialPointsGridComponent } from '../../../../modules/transportBalance/cutOff/components/officialPointsGrid.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';

function mountWithRealStore() {
    const grid = {
        addComment: {
            comments: []
        },
        cutoff: {
            operationalCut: {
                ticket: {}
            },
            ticketInfo: {
                officialPointErrorToggler: false
            }
        },
        officialPoints: {
            selection: [],
            config: { name: 'officialPoints', idField: 'movementId', selectable: true, odata: false },
            refreshToggler: true,
            items: [{
                destinationNodeName: 'Automation_kcn07',
                destinationProductName: 'CRUDO CAMPO MAMBO',
                errorMessage: null,
                measurementUnit: 'Bbl',
                movementId: '1',
                movementTypeName: 'Automation_8jsy2',
                netStandardVolume: '2345.82',
                operationalDate: '2020-05-25T00:00:00',
                sapTrackingId: 8,
                sourceNodeName: 'Automation_wd1lm',
                sourceProductName: 'CRUDO CAMPO CUSUCO'
            }]
        },
        config: { wizardName: 'name' }
    };

    const initialProps = {
        ticket: jest.fn(() => Promise.resolve()),
        selection: false,
        comments: []
    };

    const reducers = {
        cutoff: jest.fn(() => grid.cutoff),
        grid: jest.fn(() => grid),
        addComment: jest.fn(() => grid.addComment)
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true });

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <OfficialPointsGrid config={{ wizardName: 'name' }} name="officialPoints" />
    </Provider>);
    return { store, enzymeWrapper, props };
}


describe('officialPoints', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('officialPoints');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(10);
    });

    it('should verify column names for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(1).find('span').at(0).text()).toEqual('officialMovementId');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(2).find('span').at(0).text()).toEqual('movementTypeTransferOperational');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(3).find('span').at(0).text()).toEqual('sourceNodeTransferOperational');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(4).find('span').at(0).text()).toEqual('destinationNodeTransferOperational');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(5).find('span').at(0).text()).toEqual('sourceProductTransferOperational');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(6).find('span').at(0).text()).toEqual('destinationProductTransferPoints');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(7).find('span').at(0).text()).toEqual('netQuantity');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(8).find('span').at(0).text()).toEqual('units');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(9).find('span').at(0).text()).toEqual('dateOperational');
    });


    it('should read all Column', () => {
        gridUtils.buildTextColumn = jest.fn();
        gridUtils.buildDecimalColumn = jest.fn();
        gridUtils.buildDateColumn = jest.fn();
        const props = {
            selection: [],
            comment: 'comment',
            items: [{
                destinationNodeName: 'Automation_kcn07',
                destinationProductName: 'CRUDO CAMPO MAMBO',
                errorMessage: null,
                measurementUnit: 'Bbl',
                movementId: '1',
                movementTypeName: 'Automation_8jsy2',
                netStandardVolume: '2345.82',
                operationalDate: '2020-05-25T00:00:00',
                sapTrackingId: 8,
                sourceNodeName: 'Automation_wd1lm',
                sourceProductName: 'CRUDO CAMPO CUSUCO'
            }],
            operationalCut: {
                ticket: {
                    startDate: '2019-10-14T11:57:14.74',
                    endDate: '2019-10-14T11:57:14.74',
                    categoryElementName: 'categoryname'
                }
            },
            step: 1,
            getMovements: jest.fn(),
            incrementCutOff: jest.fn(),
            config: { wizardName: 'name' }
        };

        const wrapper = shallow(<OfficialPointsGridComponent {...props} />);
        expect(wrapper.instance().getColumns()).toHaveLength(10);

        wrapper.instance().getMovements('url');

        expect(props.getMovements.mock.calls).toHaveLength(1);
        expect(props.incrementCutOff.mock.calls).toHaveLength(1);
    });
});


