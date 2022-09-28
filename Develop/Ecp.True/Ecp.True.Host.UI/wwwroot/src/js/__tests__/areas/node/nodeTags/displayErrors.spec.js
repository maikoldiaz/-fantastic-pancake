import setup from '../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import DisplayErrors from '../../../../modules/administration/node/nodeTags/components/tagErrorsGrid.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import Grid from '../../../../common/components/grid/grid.jsx';

function mountWithRealStore() {
    const grid = {
        nodeTags: {
            errorNodes: [{
                categoryElement: {
                    category: {
                        categoryId: 1,
                        createdBy: 'System',
                        createdDate: '2019-10-03T17:36:03.723Z',
                        description: 'Tipo de Nodo',
                        isActive: true,
                        isAuditable: true,
                        isGrouper: false,
                        isReadOnly: true,
                        lastModifiedBy: null,
                        lastModifiedDate: null,
                        name: 'Tipo de Nodo',
                        createdBy: 'System',
                        createdDate: '2019-10-03T17:36:03.88Z',
                        description: 'Facilidad',
                        elementId: 1,
                        isActive: true,
                        isAuditable: true,
                        lastModifiedBy: null,
                        lastModifiedDate: null,
                        name: 'Facilidad'
                    },
                    categoryId: 1
                },
                createdBy: 'System',
                createdDate: '2019-10-09T03:18:20.95Z',
                elementId: 1,
                endDate: '2987-10-10T00:00:00Z',
                lastModifiedBy: 'System',
                lastModifiedDate: '2019-10-09T03:31:26.91Z',
                node: {
                    acceptableBalancePercentage: 12,
                    controlLimit: 34,
                    createdBy: 'System',
                    createdDate: '2019-10-07T19:23:02.967Z',
                    description: 'Oleoducto',
                    isActive: true,
                    isAuditable: true,
                    lastModifiedBy: 'System',
                    lastModifiedDate: '2019-10-11T09:51:12.107Z',
                    logisticCenterId: '4107',
                    name: 'SAN FERNANDO-APIAY L30',
                    nodeId: 82,
                    nodeType: null,
                    operator: null,
                    segment: null,
                    sendToSap: true
                },
                nodeId: 82,
                nodeTagId: 264,
                startDate: '2019-10-24T00:00:00Z'
            }]
        },
        nodeTagErrors: {
            config: { name: 'nodeTagErrors', odata: false }
        }
    };

    const initialProps = {
        selection: false
    };

    const reducers = {
        grid: jest.fn(() => grid),
        node: jest.fn(() => grid)
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true });

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <DisplayErrors name="nodeTagErrors" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('displayErrors', () => {
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
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('nodeTagErrors');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(3);
    });
});
