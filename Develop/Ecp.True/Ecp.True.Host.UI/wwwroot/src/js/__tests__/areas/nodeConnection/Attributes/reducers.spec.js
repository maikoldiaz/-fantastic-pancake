import * as actions from '../../../../modules/administration/nodeConnection/attributes/actions';
import { attributes } from '../../../../modules/administration/nodeConnection/attributes/reducers';

describe('reducer test for node connection', () => {
    it('should handle Receive Get Connection Action', () => {
        const initialState = 'Test State';
        const action = {
            type: actions.RECEIVE_GET_CONNECTION,
            connection: [{ name: 'Test Connection 1', isTransfer: false }, { name: 'Test Connection 2', isTransfer: true }]
        };
        const newState = Object.assign({}, initialState, {
            connection: action.connection[0],
            isTransfer: action.connection[0].isTransfer,
            receiveConnectionToggler: true
        });
        expect(attributes(initialState, action)).toEqual(newState);
    });

    it('should handle Receive Update Connection Action', () => {
        const initialState = {
            connectionToggler: true,
            status: undefined
        };
        const action = {
            type: actions.RECEIVE_UPDATE_CONNECTION
        };
        const newState = Object.assign({}, initialState, { connectionToggler: !initialState.connectionToggler, status: initialState.status });
        expect(attributes(initialState, action)).toEqual(newState);
    });

    it('should handle Init Product Action', () => {
        const initialState = 'Test State';
        const action = {
            type: actions.INIT_PRODUCT,
            connectionProduct: 'Test Connection Product'
        };
        const newState = Object.assign({}, initialState, { connectionProduct: action.connectionProduct });
        expect(attributes(initialState, action)).toEqual(newState);
    });

    it('should handle Receive Update Product Action', () => {
        const initialState = {
            productToggler: true
        };
        const action = {
            type: actions.RECEIVE_UPDATE_PRODUCT
        };
        const newState = Object.assign({}, initialState, { productToggler: !initialState.productToggler });
        expect(attributes(initialState, action)).toEqual(newState);
    });

    it('should handle Receive Get Owners Action', () => {
        const initialState = {
            ownersToggler: true
        };
        const action = {
            type: actions.RECEIVE_GET_OWNERS,
            owners: 'Test Owners'
        };
        const newState = Object.assign({}, initialState, { owners: action.owners, ownersToggler: !initialState.ownersToggler });
        expect(attributes(initialState, action)).toEqual(newState);
    });

    it('should handle Receive Update Node Connection Owners Action', () => {
        const initialState = {
            ownersUpdateToggler: true
        };
        const action = {
            type: actions.RECEIVE_UPDATE_NODE_CONNECTION_OWNERS
        };
        const newState = Object.assign({}, initialState, { ownersUpdateToggler: !initialState.ownersUpdateToggler });
        expect(attributes(initialState, action)).toEqual(newState);
    });
    it('should set control limit source', () => {
        const initialState = {
            ownersToggler: true
        };
        const action = {
            type: actions.SET_CONTROL_LIMIT_SOURCE,
            controlLimitSource: 'graphicalNodeConnection'
        };
        const newState = Object.assign({}, initialState, { controlLimitSource: action.controlLimitSource });
        expect(attributes(initialState, action)).toEqual(newState);
    });
    it('should receive connection by Node Ids', () => {
        const initialState = {
            receiveConnectionByNodeIDToggler: false
        };
        const action = {
            type: actions.RECEIVE_CONNECTION_BY_NODES,
            connection: { nodeConnectionId: 1 }
        };

        const newState = Object.assign({}, initialState,
            {
                nodeConnectionId: action.connection.nodeConnectionId,
                receiveConnectionByNodeIDToggler: !initialState.receiveConnectionByNodeIDToggler
            });
        expect(attributes(initialState, action)).toEqual(newState);
    });

    it('should receive nodes', () => {
        const initialState = {
            nodes: []
        };

        const action = {
            type: actions.RECEIVE_GET_NODES,
            nodes: [
                {
                    "name": "Test",
                    "nodeId": 1,
                    "description": "Test",
                    "isActive": true,
                    "autoOrder": false,
                    "order": 1,
                    "sendToSap": false,
                    "logisticCenterId": null,
                    "controlLimit": "0.20",
                    "acceptableBalancePercentage": null,
                    "unitId": null,
                    "capacity": null,
                    "nodeOwnershipRuleId": null,
                    "isExportation": false,
                    "rowVersion": "AAAAAAACRIU=",
                    "isAuditable": true,
                    "createdBy": "Data Generator",
                    "createdDate": "2020-10-22T02:56:39.77Z",
                    "lastModifiedBy": "System",
                    "lastModifiedDate": null
                }
            ]
        };

        const newState = Object.assign({}, initialState, {
            nodes: action.nodes
        });

        expect(attributes(initialState, action)).toEqual(newState);



    });

    it('should receive destination nodes', () => {
        const initialState = {};

        const action = {
            type: actions.RECEIVE_GET_DESTINATION_NODES,
            nodes: [
                {
                    destinationNode: {
                        name: "Test",
                        nodeId: 1
                    }
                }
            ],
            nodeIdSelected: 1
        };

        const newState = Object.assign({}, initialState, {
            destinationNodes: {
                nodes: [{ name: "Test", nodeId: 1 }],
                nodeIdSelected: action.nodeIdSelected
            }
        });

        expect(attributes(initialState, action)).toEqual(newState);

    });

    it('should blur node cost center', () => {
        const initialState = {
            destinationNode: {
                nodes: [{ name: "Test", nodeId: 1 }],
                nodeIdSelected: 1
            }
        };

        const action = {
            type: actions.BLUR_NODE_COST_CENTER
        };

        const newState = Object.assign({}, initialState, {
            destinationNodes: {
                nodes: [],
                nodeIdSelected: undefined
            }
        });

        expect(attributes(initialState, action)).toEqual(newState);
    });

    it('should handle Receive Delete Connection Action', () => {
        const initialState = {
            statusDelete: true
        };
        const action = {
            type: actions.RECEIVE_DELETE_CONNECTION_ATTRIBUTES,
            statusDelete: true
        };
        const newState = Object.assign({}, initialState, { statusDelete: initialState.statusDelete });
        expect(attributes(initialState, action)).toEqual(newState);
    });

    it('should clear status delete Connection Action', () => {
        const initialState = {
            statusDelete: true
        };
        const action = {
            type: actions.CLEAR_STATUS_CONNECTION_ATTRIBUTES,
            statusDelete: true
        };
        const newState = Object.assign({}, initialState, { statusDelete: initialState.statusDelete });
        expect(attributes(initialState, action)).toEqual(newState);
    });

    it('should change status Connection Action', () => {
        const initialState = {
            status: true
        };
        const action = {
            type: actions.CHANGE_STATUS_NODE_CONNECTION_ATTRIBUTES,
            status: true
        };
        const newState = Object.assign({}, initialState, { status: initialState.status });
        expect(attributes(initialState, action)).toEqual(newState);
    });

    it('should clear status node cost center Action', () => {
        const initialState = {
            status: true
        };
        const action = {
            type: actions.CLEAR_STATUS_NODE_COST_CENTER,
            status: true
        };
        const newState = Object.assign({}, initialState, { status: initialState.status });
        expect(attributes(initialState, action)).toEqual(newState);
    });

    it('should receive node connections for source segment', () => {
        const initialState = {
            newConnections: {}
        };

        const action = {
            type: actions.RECEIVE_NODES_BY_SEGMENT,
            isSource: true,
            nodes: [],
            segmentIdSelected: 1,
            position: 0
        };

        const newState = Object.assign({}, initialState, {
            newConnections: {
                0: {
                    segmentIdSelected: 1,
                    sourceNodes: []
                }
            }
        });

        expect(attributes(initialState, action)).toEqual(newState);

    });

    it('should receive node connections for destination segment', () => {
        const initialState = {
            newConnections: {}
        };

        const action = {
            type: actions.RECEIVE_NODES_BY_SEGMENT,
            isSource: false,
            nodes: [],
            segmentIdSelected: 1,
            position: 0
        };

        const newState = Object.assign({}, initialState, {
            newConnections: {
                0: {
                    segmentIdSelected: 1,
                    destinationNodes: []
                }
            }
        });

        expect(attributes(initialState, action)).toEqual(newState);

    });
});
