import * as actions from '../../../../modules/administration/nodeConnection/network/actions.js';
import { nodeGraphicalConnection } from '../../../../modules/administration/nodeConnection/network/reducers.js';
import { constants } from '../../../../common/services/constants.js';
import { resourceProvider } from '../../../../common/services/resourceProvider.js';
import { utilities } from '../../../../common/services/utilities.js';

describe('Reducer for graphical configuration network', () => {
    const initialState = {
        graphicalNetworkBgEnabled: true,
        graphicalNetwork: null,
        errorMessage: null,
        unsavedConnection: null,
        modelConnections: {},
        modelNodes: {},
        showCreateNodePanel: false,
        receiveGraphicalNetworkToggler: false,
        createUnsavedConnectionToggler: false,
        getErrorNodeDetailsToggler: false,
        receiveEnableButtonToggler: false,
        connectionAfterUpdate: null,
        connectionToDelete: null
    };

    it('should request graphical network', () => {
        const action = {
            type: actions.REQUEST_GET_GRAPHICALNETWORK
        };
        const newState = Object.assign({}, initialState, { requested: true });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });

    it('should create unsaved connection', () => {
        const action = {
            type: actions.CREATE_UNSAVED_CONNECTION,
            unsavedConnection: {
                sourcePortId: 'out_2',
                targetPortId: 'in_1'
            },
            createUnsavedConnectionToggler: true
        };
        const newState = Object.assign({}, initialState, { unsavedConnection: action.unsavedConnection, createUnsavedConnectionToggler: action.createUnsavedConnectionToggler });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should add the new connection on successful creation', () => {
        const action = {
            type: actions.RECEIVE_CREATE_CONNECTION
        };
        const unsavedConnection = {
            sourcePortId: 'out_2',
            targetPortId: 'in_1'
        };
        const graphicalNetwork = {
            1: {
                nodeName: 'Test 1',
                segment: 'Transporte',
                operator: 'Test 1',
                controlLimit: 0.35,
                acceptableBalancePercentage: 0.2,
                inputConnections: 0,
                outputConnections: 0,
                in_1: [],
                out_1: []
            },
            2: {
                nodeName: 'Test 2',
                segment: 'Transporte',
                operator: 'Test 2',
                controlLimit: 0.35,
                acceptableBalancePercentage: 0.2,
                inputConnections: 0,
                outputConnections: 0,
                in_2: [],
                out_2: []
            }
        };

        const initialState = Object.assign({}, initialState, { createdConnection: { rowVersion: 'AAABBBCCC' } });

        const state = Object.assign({}, initialState, { unsavedConnection, graphicalNetwork });
        const newState = Object.assign({}, state, {
            createConnectionToggler: !state.createConnectionToggler,
            graphicalNetwork: {
                1: {
                    nodeName: 'Test 1',
                    segment: 'Transporte',
                    operator: 'Test 1',
                    controlLimit: 0.35,
                    acceptableBalancePercentage: 0.2,
                    inputConnections: 1,
                    outputConnections: 0,
                    in_1: [{ sourceNodeId: 2, destinationNodeId: 1, state: constants.NodeConnectionState.Active, rowVersion: 'AAABBBCCC' }],
                    out_1: []
                },
                2: {
                    nodeName: 'Test 2',
                    segment: 'Transporte',
                    operator: 'Test 2',
                    controlLimit: 0.35,
                    acceptableBalancePercentage: 0.2,
                    inputConnections: 0,
                    outputConnections: 1,
                    in_2: [],
                    out_2: [{ sourceNodeId: 2, destinationNodeId: 1, state: constants.NodeConnectionState.Active, rowVersion: 'AAABBBCCC' }]
                }
            },
            createdConnection: { rowVersion: 'AAABBBCCC' }
        });
        expect(nodeGraphicalConnection(state, action)).toEqual(newState);
    });
    it('should set error message on failed create connection', () => {
        const action = {
            type: actions.FAIL_CREATE_CONNECTION,
            errorMessage: resourceProvider.read('systemErrorMessage')
        };
        const newState = Object.assign({}, initialState,
            {
                createConnectionToggler: !initialState.createConnectionToggler,
                errorMessage: action.errorMessage
            });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should toggle remove connection toggler on remove connection', () => {
        const action = {
            type: 'REMOVE_CONNECTION'
        };
        const newState = Object.assign({}, initialState, { removeConnectionToggler: !initialState.removeConnectionToggler });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should toggle update connection toggler on update connection', () => {
        const action = {
            type: 'UPDATE_NODE_CONNECTION'
        };
        const newState = Object.assign({}, initialState, { updateConnectionToggler: !initialState.updateConnectionToggler });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should update model connections', () => {
        const action = {
            type: 'UPDATE_MODEL_CONNECTIONS',
            modelConnections: [{
                sourcePortId: 'out_2',
                targetPortId: 'in_1'
            }]
        };
        const newState = Object.assign({}, initialState, { modelConnections: action.modelConnections });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should update model nodes', () => {
        const action = {
            type: 'UPDATE_MODEL_NODES',
            modelNodes: [{
                nodeId: 1,
                nodeName: 'Test Node'
            }]
        };
        const newState = Object.assign({}, initialState, { modelNodes: action.modelNodes });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should clear error message', () => {
        const action = {
            type: 'CLEAR_ERROR_MESSAGE'
        };
        const newState = Object.assign({}, initialState, { errorMessage: null });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should clear unsaved connection', () => {
        const action = {
            type: 'CLEAR_UNSAVED_CONNECTION'
        };
        const newState = Object.assign({}, initialState, { unsavedConnection: null });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should reset state', () => {
        const action = {
            type: 'RESET_STATE'
        };
        expect(nodeGraphicalConnection(initialState, action)).toEqual(initialState);
    });
    it('should persist current graphical network', () => {
        const initialState = {
            errorMessage: null,
            unsavedConnection: null,
            graphicalNetwork: {
                graphicalNode: {
                    1: {
                        nodeName: 'Test 2'
                    }
                }
            }
        };
        const action = {
            type: 'PERSIST_CURRENT_GRAPHICAL_NETWORK'
        };
        const newState = Object.assign({}, initialState, { unsavedgraphicalNetwork: initialState.graphicalNetwork });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should set unsaved node', () => {
        const initialState = {
            unsavedgraphicalNetwork: {
                graphicalNode: {
                    1: {
                        nodeName: 'Test 2'
                    }
                }
            }
        };
        const action = {
            type: 'SET_UNSAVED_NODE',
            unsavedGraphicalNode: {
                nodeId: 0,
                nodeName: 'Nodo',
                acceptableBalancePercentage: '####',
                controlLimit: '####',
                segment: 'XXXX',
                operator: 'XXXX',
                nodeType: null,
                segmentColor: '#66666B',
                nodeTypeIcon: null,
                isActive: true,
                isUnsaved: true,
                inputConnections: '###',
                outputConnections: '###',
                createdBy: null,
                createdDate: null
            }
        };
        const newState = Object.assign({}, initialState,
            {
                graphicalNetwork: { 0: action.unsavedGraphicalNode, ...initialState.unsavedgraphicalNetwork },
                showCreateNodePanel: true
            });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should removed unsaved node', () => {
        const initialState = {
            unsavedgraphicalNetwork: {
                graphicalNode: {
                    1: {
                        nodeName: 'Test 2'
                    }
                }
            }
        };
        const action = {
            type: 'REMOVE_UNSAVED_NODE'
        };
        const newState = Object.assign({}, initialState,
            {
                graphicalNetwork: initialState.unsavedgraphicalNetwork,
                showCreateNodePanel: false
            });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should request graphical node', () => {
        const action = {
            type: 'REQUEST_GET_GRAPHICALNODE'
        };
        const newState = Object.assign({}, initialState, { requested: true });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should receive graphical node', () => {
        const initialState = {
            unsavedgraphicalNetwork: {
                graphicalNode: {
                    1: {
                        nodeName: 'Test 2'
                    }
                }
            }
        };
        const action = {
            type: 'RECEIVE_GET_GRAPHICALNODE',
            graphicalNetwork: {
                graphicalNodes: [
                    {
                        nodeId: 0,
                        nodeName: 'Test 0'
                    },
                    {
                        nodeId: 1,
                        nodeName: 'Test 1'
                    }
                ]
            }
        };
        const savedGraphicalNetwork = utilities.normalize(action.graphicalNetwork.graphicalNodes, 'nodeId');
        const newState = Object.assign({},
            initialState,
            {
                graphicalNetwork: { ...savedGraphicalNetwork, ...initialState.unsavedgraphicalNetwork }
            });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('Hide all destination node details', () => {
        const initialState = {
            nodeGraphicalNetwork: {
                6399: {
                    nodeName: 'Test 1',
                    in_6399: [],
                    out_6399: []
                }
            }
        };
        const action = {
            type: 'HIDE_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK',
            sourceNodeId: 6399,
            nodeGraphicalNetwork: {
                6399: {
                    nodeName: 'Test 1',
                    in_6399: [],
                    out_6399: [{ sourceNodeId: 6399, destinationNodeId: 6400, state: 'Active' },
                        { sourceNodeId: 6399, destinationNodeId: 6401, state: 'Active' }]
                },
                6400: {
                    nodeName: 'Test 6400',
                    in_6400: [{ sourceNodeId: 6399, destinationNodeId: 6400, state: 'Active' }],
                    out_6400: [{ sourceNodeId: 6400, destinationNodeId: 6401, state: 'Active' }]
                },
                6401: {
                    nodeName: 'Test 6401',
                    in_6401: [{ sourceNodeId: 6399, destinationNodeId: 6401, state: 'Active' },
                        { sourceNodeId: 6400, destinationNodeId: 6401, state: 'Active' }],
                    out_6401: []
                }
            },
            receiveGraphicalNetworkToggler: false
        };
        const newState = Object.assign({},
            initialState,
            {
                graphicalNetwork: { ...initialState.nodeGraphicalNetwork },
                receiveGraphicalNetworkToggler: true,
                selectedNodeId: action.sourceNodeId
            });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('Hide all source node details', () => {
        const initialState = {
            nodeGraphicalNetwork: {
                6399: {
                    nodeName: 'Test 1',
                    in_6399: [],
                    out_6399: []
                },
                6407: {
                    nodeName: 'Test 6407',
                    in_6407: [],
                    out_6407: []
                }
            }
        };
        const action = {
            type: 'HIDE_ALLSOURCENODESDETAILS_GRAPHICALNETWORK',
            destinationNodeId: 6399,
            nodeGraphicalNetwork: {
                6399: {
                    nodeName: 'Test 1',
                    in_6399: [{ sourceNodeId: 6400, destinationNodeId: 6399, state: 'Active' },
                        { sourceNodeId: 6403, destinationNodeId: 6399, state: 'Active' }],
                    out_6399: []
                },
                6400: {
                    nodeName: 'Test 6400',
                    in_6400: [{ sourceNodeId: 6403, destinationNodeId: 6400, state: 'Active' }],
                    out_6400: [{ sourceNodeId: 6400, destinationNodeId: 6399, state: 'Active' }]
                },
                6403: {
                    nodeName: 'Test 6403',
                    in_6403: [],
                    out_6403: [{ sourceNodeId: 6403, destinationNodeId: 6399, state: 'Active' },
                        { sourceNodeId: 6403, destinationNodeId: 6400, state: 'Active' }]
                },
                6407: {
                    nodeName: 'Test 6407',
                    in_6407: [],
                    out_6407: []
                }
            },
            receiveGraphicalNetworkToggler: false
        };
        const newState = Object.assign({},
            initialState,
            {
                graphicalNetwork: { ...initialState.nodeGraphicalNetwork },
                receiveGraphicalNetworkToggler: true,
                selectedNodeId: action.destinationNodeId
            });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('confirm enable', () => {
        const initialState = {
            hasConfirmedEnableToggler: false
        };

        const action = {
            type: actions.CONFIRM_ENABLE
        };

        const newState = Object.assign({}, initialState, { hasConfirmedEnableToggler: !initialState.hasConfirmedEnableToggler });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('confirm disable', () => {
        const initialState = {
            hasConfirmedDisableToggler: false
        };

        const action = {
            type: actions.CONFIRM_DISABLE
        };

        const newState = Object.assign({}, initialState, { hasConfirmedDisableToggler: !initialState.hasConfirmedDisableToggler });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('confirm delete', () => {
        const initialState = {
            hasConfirmedDeleteToggler: false
        };

        const action = {
            type: actions.CONFIRM_DELETE
        };

        const newState = Object.assign({}, initialState, { hasConfirmedDeleteToggler: !initialState.hasConfirmedDeleteToggler });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('receive connection after update', () => {
        const initialState = {
            receiveConnectionAfterUpdateToggler: false
        };
        const action = {
            type: actions.RECEIVE_CONNECTION_AFTER_UPDATE,
            connection: { connectionId: 1 }
        };
        const newState = Object.assign({}, initialState,
            {
                connectionAfterUpdate: action.connection,
                receiveConnectionAfterUpdateToggler: !initialState.receiveConnectionAfterUpdateToggler
            });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('enable connection', () => {
        const initialState = {
            receiveEnableButtonToggler: false
        };
        const action = {
            type: actions.RECEIVE_ENABLE_CONNECTION
        };
        const newState = Object.assign({},
            initialState,
            {
                receiveEnableButtonToggler: !initialState.receiveEnableButtonToggler
            });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('fail delete connection', () => {
        const initialState = {
            removeConnectionDetailToggler: false
        };
        const action = {
            type: actions.FAIL_DELETE_CONNECTION,
            errorMessage: 'error'
        };
        const newState = Object.assign({}, initialState, {
            removeConnectionDetailToggler: !initialState.removeConnectionDetailToggler,
            errorMessage: action.errorMessage
        });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('update connection state', () => {
        const initialState = {
            destinationNodeId: 10,
            sourceNodeId: 11,
            graphicalNetwork: {
                10: {
                    in_10: [{ sourceNodeId: 11, destinationNodeId: 10, state: 'InActive' }]
                },
                11: {
                    out_11: [{ sourceNodeId: 11, destinationNodeId: 10, state: 'InActive' }]
                }
            }
        };

        const action = {
            type: actions.UPDATE_CONNECTION_STATE,
            state: 'Active'
        };
        const updatedDestinationConnection = initialState.graphicalNetwork[10].in_10[0];
        updatedDestinationConnection.state = action.state;
        const updatedSourceConnection = initialState.graphicalNetwork[11].out_11[0];
        updatedSourceConnection.state = action.state;
        const newState = Object.assign({}, initialState, {
            graphicalNetwork: {
                10: {
                    in_10: [updatedDestinationConnection]
                },
                11: {
                    out_11: [updatedSourceConnection]
                }
            }
        });

        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('reset connection to delete', () => {
        const initialState = {
            sourceNodeId: 10,
            destinationNodeId: 20,
            connectionToDelete: { connectionId: 10 }
        };
        const action = { type: actions.RESET_CONNECTION_TO_DELETE };
        const newState = Object.assign({}, initialState, {
            sourceNodeId: null,
            destinationNodeId: null,
            connectionToDelete: null
        });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('remove deleted connection', () => {
        const initialState = {
            destinationNodeId: 10,
            sourceNodeId: 11,
            graphicalNetwork: {
                10: {
                    in_10: [{ sourceNodeId: 11, destinationNodeId: 10, state: 'Active' },
                        { sourceNodeId: 12, destinationNodeId: 10, state: 'Active' }],
                    inputConnections: 2
                },
                11: {
                    out_11: [{ sourceNodeId: 11, destinationNodeId: 10, state: 'Active' }],
                    outputConnections: 1
                }
            },
            modelConnections: {}
        };

        const action = {
            type: actions.REMOVE_DELETED_CONNECTION,
            state: 'Active'
        };
        const sourceConnection = {
            in_10: [{ sourceNodeId: 12, destinationNodeId: 10, state: action.state }]
        };
        const destinationConnection = {
            out_11: []
        };


        const newState = Object.assign({}, initialState, {
            graphicalNetwork: {
                10: {
                    in_10: sourceConnection.in_10,
                    inputConnections: 1
                },
                11: {
                    out_11: destinationConnection.out_11,
                    outputConnections: 0
                }
            },
            modelConnections: {}
        });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });

    it('should remove self connection', () => {
        const initialState = {
            destinationNodeId: 10,
            sourceNodeId: 10,
            graphicalNetwork: {
                10: {
                    in_10: [{ sourceNodeId: 10, destinationNodeId: 10, state: 'Active' }],
                    inputConnections: 1,
                    out_10: [{ sourceNodeId: 10, destinationNodeId: 10, state: 'Active' }],
                    outputConnections: 1
                }
            },
            modelConnections: {}
        };

        const action = {
            type: actions.REMOVE_DELETED_CONNECTION,
            state: 'Active'
        };
        const sourceConnection = {
            in_10: []
        };
        const destinationConnection = {
            out_10: []
        };


        const newState = Object.assign({}, initialState, {
            graphicalNetwork: {
                10: {
                    in_10: sourceConnection.in_10,
                    out_10: destinationConnection.out_10,
                    inputConnections: 0,
                    outputConnections: 0
                }
            },
            modelConnections: {}
        });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });

    it('clear connection to delete', () => {
        const initialState = {
            connectionToDelete: { connectionId: 1 }
        };
        const action = {
            type: actions.CLEAR_CONNECTION_TO_DELETE
        };
        const newState = Object.assign({}, initialState, { connectionToDelete: null });

        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('create connection to delete', () => {
        const initialState = {
            connectionToDelete: null
        };
        const action = {
            type: actions.CREATE_CONNECTION_TO_DELETE,
            connectionToDelete: { connectionId: 1 },
            sourceNode: 1,
            targetNode: 2
        };

        const newState = Object.assign({}, initialState, {
            connectionToDelete: action.connectionToDelete,
            sourceNodeId: action.sourceNode,
            destinationNodeId: action.targetNode,
            errorMessage: null
        });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should set Node Connection', () => {
        const initialState = {
            connectionToDelete: null
        };
        const action = {
            type: actions.SET_NODE_CONNECTION,
            connectionToActive: { connectionId: 1 }
        };
        const newState = Object.assign({}, initialState, { connectionToActive: action.connectionToActive });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should update fail connection detail', () => {
        const initialState = {
            connectionToDelete: null,
            updateConnectionDetailToggler: false
        };
        const action = {
            type: actions.FAIL_UPDATE_CONNECTION_DETAIL,
            errorMessage: 'error'
        };
        const newState = Object.assign({}, initialState,
            {
                updateConnectionDetailToggler: !initialState.updateConnectionDetailToggler,
                errorMessage: action.errorMessage
            });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should update toggler for update connection detail', () => {
        const initialState = {
            updateConnectionDetailToggler: false
        };
        const action = {
            type: actions.RECEIVE_UPDATE_CONNECTION_DETAIL
        };
        const newState = Object.assign({}, initialState, { updateConnectionDetailToggler: !initialState.updateConnectionDetailToggler });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should update toggler for remove connection detail', () => {
        const initialState = {
            removeConnectionDetailToggler: false
        };
        const action = {
            type: actions.RECEIVE_DELETE_NODECONNECTION
        };
        const newState = Object.assign({}, initialState, { removeConnectionDetailToggler: !initialState.removeConnectionDetailToggler });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should update network graphical model', () => {
        const initialState = {
            receiveGraphicalNetworkToggler: false,
            cloneGraphicalNetwork: { networkId: 1 },
            graphicalNetwork: null
        };
        const action = {
            nodeGraphicalNetwork: { networkId: 1 },
            type: actions.UPDATE_NETWORK_MODEL
        };
        const newState = Object.assign({}, initialState, {
            graphicalNetwork: action.nodeGraphicalNetwork,
            receiveGraphicalNetworkToggler: !initialState.receiveGraphicalNetworkToggler,
            cloneGraphicalNetwork: null
        });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });

    it('should update row version for connection', () => {
        const initialState = {
            destinationNodeId: 10,
            sourceNodeId: 11,
            graphicalNetwork: {
                10: {
                    in_10: [{ sourceNodeId: 11, destinationNodeId: 10, state: 'Active', rowVersion: 'ABCABC' }]
                },
                11: {
                    out_11: [{ sourceNodeId: 11, destinationNodeId: 10, state: 'Active', rowVersion: 'ABCABC' }]
                }
            },
            connectionAfterUpdate: { rowVersion: 'AAABBBCCC' }
        };
        const action = {
            type: actions.UPDATE_ROW_VERSION_FOR_CONNECTION,
            connectionAfterUpdate: { rowVersion: 'AAABBBCCC' }
        };

        const destinationNetwork = initialState.graphicalNetwork[10].in_10[0];
        const sourceNetwork = initialState.graphicalNetwork[11].out_11[0];
        destinationNetwork.rowVersion = action.connectionAfterUpdate.rowVersion;
        sourceNetwork.rowVersion = action.connectionAfterUpdate.rowVersion;

        const newState = Object.assign({}, initialState, {
            graphicalNetwork: Object.assign({}, initialState.graphicalNetwork, {
                10: {
                    in_10: [destinationNetwork]
                },
                11: {
                    out_11: [sourceNetwork]
                }
            })
        });

        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
    it('should set graphical filter', () => {
        const initialState = {
            filters: null
        };
        const action = {
            type: actions.SAVE_GRAPHICAL_FILTER,
            filters: { }
        };
        const newState = Object.assign({}, initialState, { filters: action.filters });
        expect(nodeGraphicalConnection(initialState, action)).toEqual(newState);
    });
});
