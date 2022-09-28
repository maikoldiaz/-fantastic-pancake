import React from 'react';
import setup from '../../setup';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import UnbalancesGrid, { UnbalancesGrid as UnbalancesGridComponent } from '../../../../modules/transportBalance/cutOff/components/unbalancesGrid.jsx';
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
                ticket: {
                    startDate: '2019-10-14T11:57:14.74',
                    endDate: '2019-10-14T11:57:14.74',
                    categoryElementName: 'categoryname'
                }
            }
        },
        unbalances: {
            selection: [],
            config: { name: 'unbalances', idField: 'unbalanceId', selectable: true, odata: false },
            refreshToggler: true,
            items: [{
                nodeName: 'Automation_kcn07',
                productName: 'CRUDO CAMPO MAMBO',
                unitName: 'Bbl',
                acceptableBalance: '1',
                unbalance: '2345.82',
                unbalancePercentageText: 'text'
            }]
        }

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
        <UnbalancesGrid name="unbalances" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('unbalances', () => {
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
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('unbalances');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(6);
    });

    it('should verify column names for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(1).find('span').at(0).text()).toEqual('node');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(2).find('span').at(0).text()).toEqual('product');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(3).find('span').at(0).text()).toEqual('unbalance');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(4).find('span').at(0).text()).toEqual('units');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(5).find('span').at(0).text()).toEqual('unbalancePercentage');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(6).find('span').at(0).text()).toEqual('acceptableControl');
    });

    it('should read all Column', () => {
        gridUtils.buildTextColumn = jest.fn();
        gridUtils.buildDecimalColumn = jest.fn();
        gridUtils.buildDateColumn = jest.fn();
        const props = {
            selection: [],
            config: { name: 'unbalances', idField: 'unbalanceId', selectable: true, odata: false },
            refreshToggler: true,
            items: [{
                nodeName: 'Automation_kcn07',
                productName: 'CRUDO CAMPO MAMBO',
                unitName: 'Bbl',
                acceptableBalance: '1',
                unbalance: '2345.82',
                unbalancePercentageText: 'text'
            }],
            operationalCut: {
                ticket: {
                    startDate: '2019-10-14T11:57:14.74',
                    endDate: '2019-10-14T11:57:14.74',
                    categoryElementName: 'categoryname'
                }
            },
            step: 2,
            requestUnbalances: jest.fn(),
            incrementCutOff: jest.fn()
        };

        const wrapper = shallow(<UnbalancesGridComponent {...props} />);
        expect(wrapper.instance().getColumns()).toHaveLength(6);

        wrapper.instance().getUnbalances('url');

        expect(props.requestUnbalances.mock.calls).toHaveLength(1);
        expect(props.incrementCutOff.mock.calls).toHaveLength(1);
    });

    it('should submit on toggle of commentToggler', () => {
        gridUtils.buildTextColumn = jest.fn();
        gridUtils.buildDecimalColumn = jest.fn();
        gridUtils.buildDateColumn = jest.fn();
        const props = {
            selection: [],
            config: { name: 'unbalances', idField: 'unbalanceId', selectable: true, odata: false },
            refreshToggler: true,
            items: [{
                nodeName: 'Automation_kcn07',
                productName: 'CRUDO CAMPO MAMBO',
                unitName: 'Bbl',
                acceptableBalance: '1',
                unbalance: '2345.82',
                unbalancePercentageText: 'text'
            }],
            operationalCut: {
                ticket: {
                    startDate: '2019-10-14T11:57:14.74',
                    endDate: '2019-10-14T11:57:14.74',
                    categoryElementName: 'categoryname'
                }
            },
            step: 2,
            requestUnbalances: jest.fn(),
            incrementCutOff: jest.fn(),
            onSubmit: jest.fn(),
            clearSelection: jest.fn()
        };

        const wrapper = shallow(<UnbalancesGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, {
            commentToggler: true
        }));

        expect(props.onSubmit.mock.calls).toHaveLength(1);
        expect(props.clearSelection.mock.calls).toHaveLength(1);
    });

    it('should show error on toggle of batchFailureToggler', () => {
        gridUtils.buildTextColumn = jest.fn();
        gridUtils.buildDecimalColumn = jest.fn();
        gridUtils.buildDateColumn = jest.fn();
        const props = {
            selection: [],
            config: { name: 'unbalances', idField: 'unbalanceId', selectable: true, odata: false },
            refreshToggler: true,
            items: [{
                nodeName: 'Automation_kcn07',
                productName: 'CRUDO CAMPO MAMBO',
                unitName: 'Bbl',
                acceptableBalance: '1',
                unbalance: '2345.82',
                unbalancePercentageText: 'text'
            }],
            operationalCut: {
                ticket: {
                    startDate: '2019-10-14T11:57:14.74',
                    endDate: '2019-10-14T11:57:14.74',
                    categoryElementName: 'categoryname'
                }
            },
            step: 2,
            requestUnbalances: jest.fn(),
            incrementCutOff: jest.fn(),
            onSubmit: jest.fn(),
            saveOperationalCutOff: jest.fn(),
            showError: jest.fn()
        };

        const wrapper = shallow(<UnbalancesGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, {
            operationalCut: Object.assign({}, props.operationalCut, { batchFailureToggler: true })
        }));

        expect(props.showError.mock.calls).toHaveLength(1);
    });
});
