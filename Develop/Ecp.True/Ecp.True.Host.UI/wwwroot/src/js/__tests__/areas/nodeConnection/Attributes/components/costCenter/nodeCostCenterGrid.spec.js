import setup from '../../../../setup';
import { createStore, combineReducers } from 'redux';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { Provider } from 'react-redux';
import { httpService } from '../../../../../../common/services/httpService';
import { resourceProvider } from '../../../../../../common/services/resourceProvider';
import Grid from '../../../../../../common/components/grid/grid.jsx';
import NodeCostCenterGrid, {NodeCostCenterGrid as NodeCostCenterGridComponent } from '../../../../../../modules/administration/nodeConnection/attributes/components/costCenter/nodeCostCenterGrid';

function mountWithRealStore() {


    const data = {
        pageActions: {
            hiddenActions: [],
            disabledActions: []
        },
        nodeConnection: {
            nodeCostCenters: {
                status: false
            }
        },

        grid: {
            nodeCostCenter: {
                status: false,
                config: {
                    apiUrl: '',
                    name: 'nodeCostCenter',
                    idField: 'nodeCostCenterId'
                },
                items: [
                    {
                        "nodeCostCenterId": 62,
                        "sourceNodeId": 91538,
                        "destinationNodeId": 91539,
                        "movementTypeId": 375191,
                        "costCenterId": 375374,
                        "isActive": true,
                        "isDeleted": false,
                        "rowVersion": "AAAAAAACWCs=",
                        "isAuditable": true,
                        "createdBy": "trueadmin",
                        "createdDate": "2021-03-31T08:17:51.327Z",
                        "lastModifiedBy": "trueadmin",
                        "lastModifiedDate": "2021-04-06T06:37:48.267Z",
                        "sourceNode": {
                            "name": "Nodo_A"
                        },
                        "destinationNode": {
                            "name": "Nodo_B"
                        },
                        "costCenterCategoryElement": {
                            "name": "IG Centro de costo - Test"
                        },
                        "movementTypeCategoryElement": {
                            "name": "Automation_5ddyv"
                        }
                    },
                    {
                        "nodeCostCenterId": 60,
                        "sourceNodeId": 91538,
                        "destinationNodeId": 91539,
                        "movementTypeId": 375240,
                        "costCenterId": 375374,
                        "isActive": true,
                        "isDeleted": false,
                        "rowVersion": "AAAAAAACVFY=",
                        "isAuditable": true,
                        "createdBy": "trueadmin",
                        "createdDate": "2021-03-31T08:12:03.16Z",
                        "lastModifiedBy": null,
                        "lastModifiedDate": null,
                        "sourceNode": {
                            "name": "Nodo_A"
                        },
                        "destinationNode": {
                            "name": "Nodo_B"
                        },
                        "costCenterCategoryElement": {
                            "name": "IG Centro de costo - Test"
                        },
                        "movementTypeCategoryElement": {
                            "name": "Automation_ai8jc"
                        }
                    }
                ],
                refreshToggler: true,
                receiveDataToggler: false,
                pageFilters: {}
            }
        }


    };

    const reducers = {
        pageActions: jest.fn(() => data.pageActions),
        grid: jest.fn(() => data.grid),
        nodeConnection: jest.fn(() => data.nodeConnection),
    };

    const initialProps = {};

    const store = createStore(combineReducers(reducers));

    const props = Object.assign({}, initialProps, { hideEdit: true, hideEdit: true });
    const enzymeWrapper = mount(<Provider store={store} >
        <NodeCostCenterGrid name="nodeCostCenter" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('nodeCostCenter', () => {
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
        expect(enzymeWrapper.find(Grid).at(0).at(0).prop('name')).toEqual('nodeCostCenter');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(6);
    });
    
    it('should do show error modal component did update', () => {
        const props = {
            openErrorModal: jest.fn(() => Promise.resolve()),
            clearStatus: jest.fn(() => Promise.resolve()),
            statusError: false
        };
    
        const wrapper = shallow(<NodeCostCenterGridComponent {...props} />);
        wrapper.setState({ isUpdated: true });
        expect(wrapper.instance().props.openErrorModal.mock.calls).toHaveLength(1);
    });
});
