import { networkBuilderService } from '../../../common/services/networkBuilderService.js';
import { LinkModel, PortModel } from '@projectstorm/react-diagrams';
import { dispatcher } from '../../../common/store/dispatcher.js';
import NetworkDiagramServiceProvider from '../../../common/components/networkDiagram/networkDiagramServiceProvider.js';
import { resourceProvider } from '../../../common/services/resourceProvider.js';

describe('Actions for NetworkBuilderService', () => {
    const nodeGraphicalNetwork = {
       1: {
          nodeName: 'Test 1',
          segment: 'Transporte',
          operator: 'Test 1',
          controlLimit: 0.35,
          acceptableBalancePercentage: 0.2,
          isActive: true,
          inputConnections: 1,
          outputConnections: 1,
          in_1: [{ sourceNodeId: 2, destinationNodeId: 1, state: 'Active' }]
       },
       2: {
        nodeName: 'Test 2',
        segment: 'Transporte',
        operator: 'Test 2',
        controlLimit: 0.35,
        acceptableBalancePercentage: 0.2,
        isActive: true,
        inputConnections: 0,
        outputConnections: 1,
        in_2: []
       }
    };

    const networkDiagramFactory = new NetworkDiagramServiceProvider();
    beforeAll(() => {
        networkDiagramFactory.getNodeModel = jest.fn(() => {
            return null;
        });
        networkDiagramFactory.registerNodeFactory = jest.fn(() => {
            return 1;
        });
        networkDiagramFactory.registerLinkFactory = jest.fn(() => {
            return 1;
        });
        networkBuilderService.initialize(networkDiagramFactory);
    })
    it('should build network', () => {
        dispatcher.dispatch = jest.fn(() => {
            return 1;
        });
        networkBuilderService.buildNetwork(nodeGraphicalNetwork, {}, {});
        expect(dispatcher.dispatch.mock.calls).toHaveLength(1);
    });
    it('should remove connection', () => {
        const unsavedConnection = {
            sourcePortId: 'out_2',
            targetPortId: 'in_1',            
        };
        const connections = {};
        connections['out_2-in_1'] = {};
        connections['out_2-in_1'].isExisting = true;
        connections['out_2-in_1'].link = new LinkModel();

        networkBuilderService.removeNodeConnection(unsavedConnection, connections);
        expect(connections['out_2-in_1'].isExisting).toEqual(false);
    });
    it('should update connection', () => {
        const unsavedConnection = {
            sourcePortId: 'out_2',
            targetPortId: 'in_1',            
        };
        const connections = {};
        connections['out_2-in_1'] = {};
        connections['out_2-in_1'].isExisting = true;
        connections['out_2-in_1'].link = new LinkModel();

        networkBuilderService.updateNodeConnection(unsavedConnection, connections);
        expect(connections['out_2-in_1'].isExisting).toEqual(true);
    });
    it('should add new connection on drag and drop', () => {
        const link = new LinkModel();
        link.setSourcePort(new PortModel({ id: 'out_2' }));
        link.setTargetPort(new PortModel({ id: 'in_1' }));

        networkDiagramFactory.getNewLink = jest.fn(() => {
            return link;
        });
        dispatcher.dispatch = jest.fn(() => {
            return 1;
        });

        networkBuilderService.onMouseUp(nodeGraphicalNetwork, {});
        expect(dispatcher.dispatch.mock.calls).toHaveLength(0);
    });
    it('should check if connection is existing', () => {
        const link = new LinkModel();
        link.setSourcePort(new PortModel({ id: 'out_2' }));
        link.setTargetPort(new PortModel({ id: 'in_1' }));

        const connections = {};
        connections['out_2-in_1'] = {};
        connections['out_2-in_1'].isExisting = true;
        connections['out_2-in_1'].link = link;

        networkBuilderService.isExistingConnection(link, connections);
        expect(connections['out_2-in_1'].isExisting).toEqual(true);
    });
    it('should check if connection is loose connection', () => {
        const link = new LinkModel();
        link.setSourcePort(new PortModel({ id: 'out_2' }));

        expect(networkBuilderService.isLooseConnection(link)).toEqual(true);
    });
    it('should check if connection is reverse connection', () => {
        const link = new LinkModel();
        link.setSourcePort(new PortModel({ id: 'in_1' }));
        link.setTargetPort(new PortModel({ id: 'out_2' }));

        expect(networkBuilderService.isReverseConnection(link)).toEqual(true);
    });
    it('should check if connection is invalid duplicate connection', () => {
        const link = new LinkModel();
        const connections = {};
        connections['out_2-in_1'] = {};
        connections['out_2-in_1'].isExisting = true;
        connections['out_2-in_1'].link = new LinkModel();
        link.setSourcePort(new PortModel({ id: 'out_2' }));
        link.setTargetPort(new PortModel({ id: 'in_1' }));

        expect(networkBuilderService.isInvalidConnection(link, nodeGraphicalNetwork, connections).isDuplicate).toEqual(true);
        expect(networkBuilderService.isInvalidConnection(link, nodeGraphicalNetwork, connections).reason).toEqual(resourceProvider.read('duplicateNodeConnectionErrorMessage'));
    });
    it('should check if connection is invalid connection for node status inactive', () => {
        const link = new LinkModel();
        const connections = {};
        const graphicalNetwork = Object.assign({}, nodeGraphicalNetwork);
        graphicalNetwork[1].isActive = false
        link.setSourcePort(new PortModel({ id: 'out_2' }));
        link.setTargetPort(new PortModel({ id: 'in_1' }));

        expect(networkBuilderService.isInvalidConnection(link, graphicalNetwork, connections).reason).toEqual(resourceProvider.read('nodeStatusInactiveErrorMessage'));
    });
    it('should remove duplicate connection', () => {
        const unsavedConnection = {
            sourcePortId: 'out_2',
            targetPortId: 'in_1',
            isDuplicate: true           
        };
        const connections = {};
        connections['out_2-in_1-duplicate'] = {};
        connections['out_2-in_1-duplicate'].isExisting = true;
        connections['out_2-in_1-duplicate'].link = new LinkModel();

        networkBuilderService.removeNodeConnection(unsavedConnection, connections);
        expect(connections['out_2-in_1-duplicate'].isExisting).toEqual(false);
    });
    it('should get node after delete', () => {
        const graphicalNetwork = {
            10: {
                in_10: [{ sourceNodeId: 10, destinationNodeId: 10, state: 'Active' }],
                inputConnections: 1,
                out_10: [{ sourceNodeId: 10, destinationNodeId: 10, state: 'Active' }],
                outputConnections: 1
            }
        };

        const nodeId = 10;

        const graphicalNetworkAfterDelete = {
                in_10: [],
                inputConnections: 0,
                out_10: [],
                outputConnections: 0
        };

        const result = networkBuilderService.getNodeAfterDelete(graphicalNetwork, nodeId);
        
        expect(result).toEqual(graphicalNetworkAfterDelete);
    });

    it('should get all graphical network source node details', () => {
        const sourceNodesDetailsgraphicalNetwork = {
            graphicalNodeConnections: [
                {
                    destinationNodeId: 848,
                    rowVersion: "AAAAAAACKo4=",
                    sourceNodeId: 848,
                    state: "Active"
                }
            ],
            graphicalNodes: [
                {
                    acceptableBalancePercentage: 50,
                    controlLimit: 15,
                    inputConnections: 2,
                    isActive: true,
                    nodeId: 848,
                    nodeName: "testnode1",
                    nodeType: "Oleoducto",
                    operator: "ECOPETROL",
                    order: 5,
                    outputConnections: 2,
                    segment: "Transporte por Oleoducto",
                    segmentColor: "#DF443D",
                }
            ]
        }

        const sourceNodeId = 848;
        const nodeGraphicalNetwork = {
            848: {
                acceptableBalancePercentage: 50,
                controlLimit: 15,
                in_848: [],
                inputConnections: 2,
                isActive: true,
                name: "testnode1",
                nodeId: 848,
                nodeName: "testnode1",
                nodeType: "Oleoducto",
                operator: "ECOPETROL",
                order: 5,
                out_848: [],
                outputConnections: 2,
                segment: "Transporte por Oleoducto",
                segmentColor: "#DF443D",
            }
        }
        const expectedNodeGraphicalNetwork = {
            848: {
                acceptableBalancePercentage: 50,
                controlLimit: 15,
                in_848: [
                    {
                        destinationNodeId: 848,
                        rowVersion: "AAAAAAACKo4=",
                        sourceNodeId: 848,
                        state: "Active",
                    }
                ],
                inputConnections: 2,
                isActive: true,
                nodeId: 848,
                nodeName: "testnode1",
                nodeType: "Oleoducto",
                operator: "ECOPETROL",
                order: 5,
                out_848: [
                    {
                        destinationNodeId: 848,
                        rowVersion: "AAAAAAACKo4=",
                        sourceNodeId: 848,
                        state: "Active",
                    }
                ],
                outputConnections: 2,
                segment: "Transporte por Oleoducto",
                segmentColor: "#DF443D",
            }
        }

        networkBuilderService.getAllGraphicalNetworkSourceNodeDetails(sourceNodesDetailsgraphicalNetwork, sourceNodeId, nodeGraphicalNetwork);
        expect(expectedNodeGraphicalNetwork).toEqual(nodeGraphicalNetwork);
    })

    it('should get all graphical network destination node details', () => {
        const sourceNodesDetailsgraphicalNetwork = {
            graphicalNodeConnections: [
                {
                    destinationNodeId: 848,
                    rowVersion: "AAAAAAACKo4=",
                    sourceNodeId: 848,
                    state: "Active"
                }
            ],
            graphicalNodes: [
                {
                    acceptableBalancePercentage: 50,
                    controlLimit: 15,
                    inputConnections: 2,
                    isActive: true,
                    nodeId: 848,
                    nodeName: "testnode1",
                    nodeType: "Oleoducto",
                    operator: "ECOPETROL",
                    order: 5,
                    outputConnections: 2,
                    segment: "Transporte por Oleoducto",
                    segmentColor: "#DF443D",
                }
            ]
        }

        const sourceNodeId = 848;
        const nodeGraphicalNetwork = {
            848: {
                acceptableBalancePercentage: 50,
                controlLimit: 15,
                in_848: [],
                inputConnections: 2,
                isActive: true,
                name: "testnode1",
                nodeId: 848,
                nodeName: "testnode1",
                nodeType: "Oleoducto",
                operator: "ECOPETROL",
                order: 5,
                out_848: [],
                outputConnections: 2,
                segment: "Transporte por Oleoducto",
                segmentColor: "#DF443D",
            }
        }
        const expectedNodeGraphicalNetwork = {
            848: {
                acceptableBalancePercentage: 50,
                controlLimit: 15,
                in_848: [
                    {
                        destinationNodeId: 848,
                        rowVersion: "AAAAAAACKo4=",
                        sourceNodeId: 848,
                        state: "Active",
                    }
                ],
                inputConnections: 2,
                isActive: true,
                nodeId: 848,
                nodeName: "testnode1",
                nodeType: "Oleoducto",
                operator: "ECOPETROL",
                order: 5,
                out_848: [
                    {
                        destinationNodeId: 848,
                        rowVersion: "AAAAAAACKo4=",
                        sourceNodeId: 848,
                        state: "Active",
                    }
                ],
                outputConnections: 2,
                segment: "Transporte por Oleoducto",
                segmentColor: "#DF443D",
            }
        }

        networkBuilderService.getAllGraphicalNetworkDestinationNodeDetails(sourceNodesDetailsgraphicalNetwork, sourceNodeId, nodeGraphicalNetwork);
        expect(expectedNodeGraphicalNetwork).toEqual(nodeGraphicalNetwork);
    })
});
