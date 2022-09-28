import createEngine, { DiagramModel, DefaultDiagramState, DagreEngine, PathFindingLinkFactory } from '@projectstorm/react-diagrams';
import { dispatcher } from '../store/dispatcher';
import { constants } from './constants.js';
import { createUnsavedConnection, updateModelConnections, updateModelNodes, setErrorMessage, resetState } from './../../modules/administration/nodeConnection/network/actions.js';
import { resourceProvider } from './resourceProvider.js';
import { utilities } from '../../common/services/utilities';
import NetworkDiagramServiceProvider from '../../common/components/NetworkDiagram/networkDiagramServiceProvider';

const networkBuilderService = (function () {
    let engine = null;
    let dagreEngine = null;
    let model = null;
    let networkDiagramFactory = null;
    let isNewLink = false;
    let invalidLink = null;

    const getNodeId = portId => {
        return parseInt(portId.split('_')[1], 10);
    };

    const addToModelConnections = (connections, key, link) => {
        connections[key] = connections[key] ? connections[key] : {};
        connections[key].isExisting = true;
        connections[key].link = link;
    };

    const getInvalidLink = () => {
        return invalidLink;
    };

    const isLooseConnection = link => {
        return !link.sourcePort || !link.targetPort;
    };

    const isReverseConnection = link => {
        return !(link.targetPort.options.id.startsWith('in') && link.sourcePort.options.id.startsWith('out'));
    };

    const isSelfConnection = link => {
        if (link.targetPort && link.sourcePort) {
            return getNodeId(link.targetPort.options.id) === getNodeId(link.sourcePort.options.id);
        }
        return false;
    };

    const isExistingConnection = (link, modelConnections) => {
        if (isLooseConnection(link)) {
            return false;
        }
        return modelConnections[`${link.sourcePort.options.id}-${link.targetPort.options.id}`]
            ? modelConnections[`${link.sourcePort.options.id}-${link.targetPort.options.id}`].isExisting : false;
    };

    const linksUpdated = (e, modelConnections) => {
        if (e.link && e.link.isInvalid && !e.link.isRemoved) {
            invalidLink = e.link;
        }
        if (e.link && !isExistingConnection(e.link, modelConnections) && !e.link.isRemoved && !e.link.isCloned) {
            isNewLink = true;
            e.link.options.isNew = true;
        }
    };

    const registerListener = modelConnections => {
        model.registerListener({ linksUpdated: e => linksUpdated(e, modelConnections) });
    };

    const isInvalidConnection = (link, nodeGraphicalNetwork, modelConnections) => {
        if (isExistingConnection(link, modelConnections)) {
            return { reason: resourceProvider.read('duplicateNodeConnectionErrorMessage'), isDuplicate: true };
        }
        if (link.sourcePort.options.id.startsWith('out')
            && (!nodeGraphicalNetwork[getNodeId(link.sourcePort.options.id)].isActive || !nodeGraphicalNetwork[getNodeId(link.targetPort.options.id)].isActive)) {
            return { reason: resourceProvider.read('nodeStatusInactiveErrorMessage') };
        }
        return null;
    };

    const generateNode = (nodeGraphicalNetwork, modelNodes) => {
        let row = 0;
        let col = 0;
        const offset = { x: 50, y: 50 };
        const nodeWidth = 265;
        const NodeHeight = 146;
        const containerWidth = document.querySelector('.ep-content__body') && document.querySelector('.ep-content__body').offsetWidth;
        let isModelNodesUpdated = false;
        Object.keys(nodeGraphicalNetwork).forEach(key => {
            const nodeExists = modelNodes[key] && modelNodes[key].isExisting;
            if (!nodeExists) {
                const node = networkDiagramFactory.getNodeModel({ id: key });

                const xPos = offset.x + ((offset.x + nodeWidth) * row);
                const yPos = offset.y + ((offset.x + NodeHeight) * col);
                if (node) {
                    node.setPosition(xPos, yPos);
                    model.addAll(node);
                }
                modelNodes[key] = modelNodes[key] ? modelNodes[key] : {};
                modelNodes[key].isExisting = true;
                isModelNodesUpdated = true;

                row++;
                if (xPos > containerWidth - xPos) {
                    col++;
                    row = 0;
                }
            }
        });
        if (isModelNodesUpdated) {
            dispatcher.dispatch(updateModelNodes(modelNodes));
        }
    };

    const generateNodeLinks = (nodeGraphicalNetwork, modelConnections) => {
        // rendering node connections
        let isModelConnectionsUpdated = false;
        if (model.layers.length === 2) {
            Object.keys(model.layers[1].models).forEach(key => {
                const nodeFromObject = nodeGraphicalNetwork[key];
                // Destination node : in port
                const inPort = model.layers[1].models[key].ports.in;
                if (nodeFromObject && nodeFromObject[constants.NodeSection.In + key]) {
                    nodeFromObject[constants.NodeSection.In + key].forEach(v => {
                        // Source node : out port
                        const outPort = model.layers[1].models[v.sourceNodeId].ports.out;
                        const isConnectionExists = modelConnections[`${outPort.options.id}-in_${key}`] ? modelConnections[`${outPort.options.id}-in_${key}`].isExisting : false;

                        if (!isConnectionExists) {
                            // State like - Active , Inactive , Transfer Point
                            // outPort.options.state = v.state;
                            modelConnections[`${outPort.options.id}-in_${key}`] = modelConnections[`${outPort.options.id}-in_${key}`] ? modelConnections[`${outPort.options.id}-in_${key}`] : {};
                            modelConnections[`${outPort.options.id}-in_${key}`].isExisting = true;
                            const link = outPort.link(inPort);
                            link.options.state = v.state;
                            modelConnections[`${outPort.options.id}-in_${key}`].link = link;
                            isModelConnectionsUpdated = true;
                            model.addAll(link);
                        }
                    });
                }
            });
            if (isModelConnectionsUpdated) {
                dispatcher.dispatch(updateModelConnections(modelConnections));
            }
        }
    };

    const constructAndAddLinkClone = (unsavedLink, state, isInvalidReason = null) => {
        if (isInvalidReason && invalidLink) {
            return;
        }
        const newLinkClone = unsavedLink.clone();
        const outPort = unsavedLink.getSourcePort();
        const inPort = unsavedLink.getTargetPort();

        // remove unsaved link from diagram model
        unsavedLink.options.isNew = false;
        unsavedLink.isRemoved = true;
        unsavedLink.remove();

        // add new active link in diagram model
        // outPort.options.state = constants.NodeConnectionState.Active;
        newLinkClone.options.state = state;
        newLinkClone.options.isNew = false;
        newLinkClone.isCloned = true;
        newLinkClone.setSourcePort(outPort);
        newLinkClone.setTargetPort(inPort);
        if (isInvalidReason) {
            newLinkClone.isInvalid = true;
            newLinkClone.reason = isInvalidReason.reason;
        }
        model.addAll(newLinkClone);
        // eslint-disable-next-line consistent-return
        return newLinkClone;
    };

    const updateNodeConnection = (unsavedConnection, modelConnections) => {
        const connections = Object.assign({}, modelConnections);
        if (unsavedConnection && connections[`${unsavedConnection.sourcePortId}-${unsavedConnection.targetPortId}`].isExisting) {
            const unsavedLink = connections[`${unsavedConnection.sourcePortId}-${unsavedConnection.targetPortId}`].link;
            const newLinkClone = constructAndAddLinkClone(unsavedLink, constants.NodeConnectionState.Active);
            connections[`${unsavedConnection.sourcePortId}-${unsavedConnection.targetPortId}`].link = newLinkClone;
        }

        isNewLink = false;
        return connections;
    };

    const removeNodeConnection = (unsavedConnection, modelConnections) => {
        const connections = Object.assign({}, modelConnections);
        if (unsavedConnection) {
            let key = `${unsavedConnection.sourcePortId}-${unsavedConnection.targetPortId}`;
            if (unsavedConnection.isDuplicate) {
                key = `${key}-duplicate`;
            }
            if (connections[key] && connections[key].isExisting) {
                // remove link from diagram model
                const link = connections[key].link;
                link.isRemoved = true;
                link.remove();

                // remove from list of existing connections.
                connections[key].isExisting = false;
            }
        }
        isNewLink = false;
        return connections;
    };

    const removeNodeConnectionToDelete = (connectionToDelete, modelConnections) => {
        const connections = Object.assign({}, modelConnections);
        if (connectionToDelete) {
            const key = `${connectionToDelete.sourcePortId}-${connectionToDelete.targetPortId}`;
            if (connections[key] && connections[key].isExisting) {
                // remove link from diagram model
                const link = connections[key].link;
                model.removeLink(link);
                link.isRemoved = true;
                link.remove();

                // remove from list of existing connections.
                connections[key].isExisting = false;
            }
        }
        isNewLink = false;
        return connections;
    };

    const updateNodeConnectionToInactivate = (newNodeDiagram, connectionToDelete, modelConnections, connectionState) => {
        const connections = Object.assign({}, modelConnections);
        if (newNodeDiagram && connectionToDelete) {
            const key = `${connectionToDelete.sourcePortId}-${connectionToDelete.targetPortId}`;
            if (connections[key] && connections[key].isExisting) {
                // remove link from diagram model
                const link = connections[key].link;
                model.removeLink(link);
                link.isRemoved = true;
                link.remove();
                const outPort = newNodeDiagram.getSourcePort();
                const inPort = newNodeDiagram.getTargetPort();

                const newLinkClone = newNodeDiagram.clone();
                newLinkClone.options.state = connectionState;
                newLinkClone.options.isNew = false;
                newLinkClone.isCloned = true;
                newLinkClone.setSourcePort(outPort);
                newLinkClone.setTargetPort(inPort);

                connections[key].link = newLinkClone;

                model.addAll(newLinkClone);
            }
        }
        isNewLink = false;
        return connections;
    };

    const onMouseUp = (nodeGraphicalNetwork, modelConnections) => {
        const connections = Object.assign({}, modelConnections);
        if (invalidLink && !invalidLink.isRemoved) {
            invalidLink.isRemoved = true;
            invalidLink.remove();
            if (invalidLink.reason) {
                dispatcher.dispatch(setErrorMessage(invalidLink.reason));
            }
            invalidLink = null;
            isNewLink = false;
            return;
        }
        if (isNewLink) {
            isNewLink = false;
            const link = networkDiagramFactory.getNewLink(engine);
            if (link && !isLooseConnection(link)) {
                const sourceTargetPort =
                {
                    sourcePortId: link.sourcePort.options.id,
                    targetPortId: link.targetPort.options.id,
                    sourceNodeName: nodeGraphicalNetwork[getNodeId(link.sourcePort.options.id)].nodeName,
                    destinationNodeName: nodeGraphicalNetwork[getNodeId(link.targetPort.options.id)].nodeName,
                    state: constants.NodeConnectionState.Unsaved
                };
                let key = `${link.sourcePort.options.id}-${link.targetPort.options.id}`;
                const isInvalidReason = isInvalidConnection(link, nodeGraphicalNetwork, modelConnections);
                if (isInvalidReason || isReverseConnection(link)) {
                    if (isInvalidReason && isInvalidReason.isDuplicate) {
                        key = `${key}-duplicate`;
                        sourceTargetPort.isDuplicate = true;
                    }
                    addToModelConnections(connections, key, link);
                    removeNodeConnection(sourceTargetPort, connections);
                    return;
                }
                addToModelConnections(connections, key, link);
                dispatcher.dispatch(updateModelConnections(connections));
                dispatcher.dispatch(createUnsavedConnection(sourceTargetPort));
            }
        }
    };

    const onRepaintCanvas = () => {
        dispatcher.dispatch(resetState());
        networkBuilderService.initialize(new NetworkDiagramServiceProvider());
    };

    const isSelfConnectionExist = (selectedNodeObject, selectedNodeId) => {
        return selectedNodeObject ?
            selectedNodeObject.filter(a => a.sourceNodeId === selectedNodeId && a.destinationNodeId === selectedNodeId) : [];
    };

    const isDataExist = (nodeObject, key, connectionObject) => {
        return nodeObject[key].filter(a => a.sourceNodeId === connectionObject[0].sourceNodeId
            && a.destinationNodeId === connectionObject[0].destinationNodeId);
    };

    const getAllGraphicalNetworkSourceNodeDetails = (sourceNodesDetailsgraphicalNetwork, destinationNodeId, nodeGraphicalNetwork) => {
        const graphicalSourceNodesDetailsNetwork = utilities.normalize(sourceNodesDetailsgraphicalNetwork.graphicalNodes, 'nodeId');
        let normalizedSourceNodeConnections = {};
        let normalizedDestinationNodeConnections = {};
        if (sourceNodesDetailsgraphicalNetwork.graphicalNodeConnections) {
            normalizedSourceNodeConnections = utilities.normalizedGroupBy(sourceNodesDetailsgraphicalNetwork.graphicalNodeConnections, 'destinationNodeId');
            normalizedDestinationNodeConnections = utilities.normalizedGroupBy(sourceNodesDetailsgraphicalNetwork.graphicalNodeConnections, 'sourceNodeId');

            sourceNodesDetailsgraphicalNetwork.graphicalNodes.forEach(v => {
                const node = graphicalSourceNodesDetailsNetwork[v.nodeId];
                if (v.nodeId !== destinationNodeId) {
                    if (nodeGraphicalNetwork[v.nodeId]) {
                        // In the latest nodes collections,if the node already exist in canvas , then updating the out (destination nodes collections)
                        if (normalizedDestinationNodeConnections[v.nodeId]) {
                            normalizedDestinationNodeConnections[v.nodeId].forEach(item => {
                                // Checking if array contains the connection , not adding again
                                const isConnectionExist = nodeGraphicalNetwork[v.nodeId][constants.NodeSection.Out + v.nodeId].
                                    filter(a => a.sourceNodeId === item.sourceNodeId && a.destinationNodeId === item.destinationNodeId);
                                if (isConnectionExist.length === 0) {
                                    nodeGraphicalNetwork[v.nodeId][constants.NodeSection.Out + v.nodeId].push({
                                        sourceNodeId: item.sourceNodeId,
                                        destinationNodeId: item.destinationNodeId, state: item.state,
                                        rowVersion: item.rowVersion
                                    });
                                }
                            });
                        }

                        node[constants.NodeSection.Out + v.nodeId] = nodeGraphicalNetwork[v.nodeId][constants.NodeSection.Out + v.nodeId];
                        // Assign the old values for in (source nodes collections)
                        node[constants.NodeSection.In + v.nodeId] = nodeGraphicalNetwork[v.nodeId][constants.NodeSection.In + v.nodeId];
                    } else {
                        // In the latest nodes collection , if the node is new
                        node[constants.NodeSection.In + v.nodeId] = normalizedSourceNodeConnections[v.nodeId] ? normalizedSourceNodeConnections[v.nodeId] : [];
                        node[constants.NodeSection.Out + v.nodeId] = normalizedDestinationNodeConnections[v.nodeId] ? normalizedDestinationNodeConnections[v.nodeId] : [];
                    }
                } else {
                    // In the latest nodes collections, if the node and current node both are same , then only update the in (source nodes collections)
                    node[constants.NodeSection.In + v.nodeId] = normalizedSourceNodeConnections[v.nodeId] ? normalizedSourceNodeConnections[v.nodeId] : [];
                    // Assign the old values for out (destination nodes collections)
                    node[constants.NodeSection.Out + v.nodeId] = nodeGraphicalNetwork[v.nodeId][constants.NodeSection.Out + v.nodeId];
                    node.outputConnections = nodeGraphicalNetwork[v.nodeId].outputConnections;

                    // Checking the self connection exists in the normalized connection collection
                    const isConnectionExist = isSelfConnectionExist(normalizedDestinationNodeConnections[v.nodeId], destinationNodeId);
                    if (isConnectionExist.length === 1) {
                        // Checking if the connection already exist in the out (destination nodes collection)
                        const key = constants.NodeSection.Out + v.nodeId;
                        const isExist = isDataExist(node, key, isConnectionExist);
                        if (isExist.length === 0) {
                            // Pushing to the out (destination nodes collection)
                            node[constants.NodeSection.Out + v.nodeId].push({
                                sourceNodeId: isConnectionExist[0].sourceNodeId,
                                destinationNodeId: isConnectionExist[0].destinationNodeId, state: isConnectionExist[0].state,
                                rowVersion: isConnectionExist[0].rowVersion
                            });
                        }
                    }
                }
            });

            Object.keys(graphicalSourceNodesDetailsNetwork).forEach(item => {
                nodeGraphicalNetwork[item] = graphicalSourceNodesDetailsNetwork[item];
            });
        }
    };

    const getAllGraphicalNetworkDestinationNodeDetails = (destinationNodesDetailsgraphicalNetwork, sourceNodeId, nodeGraphicalNetwork) => {
        const graphicalDestinationNodesDetailsNetwork = utilities.normalize(destinationNodesDetailsgraphicalNetwork.graphicalNodes, 'nodeId');
        let normalizedSourceNodeConnections = {};
        let normalizedDestinationNodeConnections = {};
        if (destinationNodesDetailsgraphicalNetwork.graphicalNodeConnections) {
            normalizedSourceNodeConnections = utilities.normalizedGroupBy(destinationNodesDetailsgraphicalNetwork.graphicalNodeConnections, 'destinationNodeId');
            normalizedDestinationNodeConnections = utilities.normalizedGroupBy(destinationNodesDetailsgraphicalNetwork.graphicalNodeConnections, 'sourceNodeId');

            destinationNodesDetailsgraphicalNetwork.graphicalNodes.forEach(v => {
                const node = graphicalDestinationNodesDetailsNetwork[v.nodeId];
                if (v.nodeId !== sourceNodeId) {
                    if (nodeGraphicalNetwork[v.nodeId]) {
                        // In the latest nodes collections,if the node already exist in canvas , then updating the in (source nodes collections)
                        if (normalizedSourceNodeConnections[v.nodeId]) {
                            normalizedSourceNodeConnections[v.nodeId].forEach(item => {
                                // Checking if array contains the connection , not adding again
                                const isConnectionExist = nodeGraphicalNetwork[v.nodeId][constants.NodeSection.In + v.nodeId].
                                    filter(a => a.sourceNodeId === item.sourceNodeId && a.destinationNodeId === item.destinationNodeId);
                                if (isConnectionExist.length === 0) {
                                    nodeGraphicalNetwork[v.nodeId][constants.NodeSection.In + v.nodeId].push({
                                        sourceNodeId: item.sourceNodeId,
                                        destinationNodeId: item.destinationNodeId, state: item.state,
                                        rowVersion: item.rowVersion
                                    });
                                }
                            });
                        }

                        node[constants.NodeSection.In + v.nodeId] = nodeGraphicalNetwork[v.nodeId][constants.NodeSection.In + v.nodeId];
                        // Assign the old values for out (destination nodes collections)
                        node[constants.NodeSection.Out + v.nodeId] = nodeGraphicalNetwork[v.nodeId][constants.NodeSection.Out + v.nodeId];
                    } else {
                        // In the latest nodes collection , if the node is new
                        node[constants.NodeSection.In + v.nodeId] = normalizedSourceNodeConnections[v.nodeId] ? normalizedSourceNodeConnections[v.nodeId] : [];
                        node[constants.NodeSection.Out + v.nodeId] = normalizedDestinationNodeConnections[v.nodeId] ? normalizedDestinationNodeConnections[v.nodeId] : [];
                    }
                } else {
                    // In the latest nodes collections, if the node and current node both are same , then only update the out (destination nodes collections)
                    node[constants.NodeSection.Out + v.nodeId] = normalizedDestinationNodeConnections[v.nodeId] ? normalizedDestinationNodeConnections[v.nodeId] : [];
                    // Assign the old values for in (source nodes collections)
                    node[constants.NodeSection.In + v.nodeId] = nodeGraphicalNetwork[v.nodeId][constants.NodeSection.In + v.nodeId];
                    node.inputConnections = nodeGraphicalNetwork[v.nodeId].inputConnections;

                    // Checking the self connection exists in the normalized connection collection
                    const isConnectionExist = isSelfConnectionExist(normalizedSourceNodeConnections[v.nodeId], sourceNodeId);
                    if (isConnectionExist.length === 1) {
                        // Checking if the connection already exist in the in (source nodes collection)
                        const key = constants.NodeSection.In + v.nodeId;
                        const isExist = isDataExist(node, key, isConnectionExist);
                        if (isExist.length === 0) {
                            // Pushing to the in (source nodes collection)
                            node[constants.NodeSection.In + v.nodeId].push({
                                sourceNodeId: isConnectionExist[0].sourceNodeId,
                                destinationNodeId: isConnectionExist[0].destinationNodeId, state: isConnectionExist[0].state,
                                rowVersion: isConnectionExist[0].rowVersion
                            });
                        }
                    }
                }
            });

            Object.keys(graphicalDestinationNodesDetailsNetwork).forEach(item => {
                nodeGraphicalNetwork[item] = graphicalDestinationNodesDetailsNetwork[item];
            });
        }
    };


    const getAllNodes = (nodeID, nodeGraphicalNetwork) => {
        const nodes = [];
        const inCollection = nodeGraphicalNetwork[nodeID][constants.NodeSection.In + nodeID];
        inCollection.forEach(x => {
            nodes.push(x.sourceNodeId);
            nodes.push(x.destinationNodeId);
        });

        const outCollection = nodeGraphicalNetwork[nodeID][constants.NodeSection.Out + nodeID];
        outCollection.forEach(x => {
            nodes.push(x.sourceNodeId);
            nodes.push(x.destinationNodeId);
        });

        return nodes;
    };

    const getNodesToRemove = (nodesToRemove, parentNodeID, nodeGraphicalNetwork, nodeID) => {
        // Inserting the node id
        nodesToRemove.push(nodeID);
        // Getting the all in and out nodes
        const nodesCollection = getAllNodes(nodeID, nodeGraphicalNetwork);
        // filtering the collection
        const uniqueNodesCollection = nodesCollection.filter((v, i, a) => a.indexOf(v) === i);
        uniqueNodesCollection.forEach(x => {
            // Checking if node already exists then not calling the recursive function.
            // Node should be equal to the current node
            if (!nodesToRemove.includes(x) && x !== parentNodeID) {
                // Recursively calling the function
                getNodesToRemove(nodesToRemove, parentNodeID, nodeGraphicalNetwork, x);
            }
        });
    };

    const deleteNodesAndCollections = (nodesToRemove, nodeID, nodeGraphicalNetwork) => {
        // Deleting all the nodes except the self node
        nodesToRemove.forEach(x => x !== nodeID ? delete nodeGraphicalNetwork[x] : nodeGraphicalNetwork);

        // Deleting all the in nodes collections
        const in_filteredCollection = nodeGraphicalNetwork[nodeID][constants.NodeSection.In + nodeID].filter(x => !nodesToRemove.includes(x.sourceNodeId));
        nodeGraphicalNetwork[nodeID][constants.NodeSection.In + nodeID] = in_filteredCollection;

        // Deleting all the out nodes collections
        const out_filteredCollection = nodeGraphicalNetwork[nodeID][constants.NodeSection.Out + nodeID].filter(x => !nodesToRemove.includes(x.destinationNodeId));
        nodeGraphicalNetwork[nodeID][constants.NodeSection.Out + nodeID] = out_filteredCollection;
    };

    const updateConnectionDetail = (connectionToDelete, nodeGraphicalNetwork, isActive) => {
        const sourceNode = getNodeId(connectionToDelete.sourcePortId);
        const targetNode = getNodeId(connectionToDelete.targetPortId);
        const out_node = `out_${sourceNode}`;
        const rowVersion = nodeGraphicalNetwork[sourceNode][out_node].filter(x => x.destinationNodeId === targetNode)[0].rowVersion;
        const connectionDetail = {
            sourceNodeId: sourceNode,
            destinationNodeId: targetNode,
            isActive,
            rowVersion
        };
        return connectionDetail;
    };

    const getColor = (linkId, state, nodeGraphicalNetwork, disableColor = '#f5f5f5', enableColor = '#fff') => {
        const nodes = linkId.split('-');
        const sourceNode = nodes[0].substr(nodes[0].indexOf('_') + 1);
        const targetNode = nodes[1].substr(nodes[1].indexOf('_') + 1);
        const out_node = `out_${sourceNode}`;
        const connectionDetail = nodeGraphicalNetwork[sourceNode][out_node].filter(x => x.destinationNodeId === Number(targetNode));
        if (connectionDetail.length > 0) {
            return connectionDetail[0].state === state ? disableColor : enableColor;
        }
        return enableColor;
    };

    const updateSelectConnection = (connections, key, isSelected) => {
        connections[key].link.options.isSelected = isSelected;
        const link = connections[key].link;
        const outPort = link.getSourcePort();
        const inPort = link.getTargetPort();

        const newLink = link.clone();
        model.removeLink(link);
        link.isRemoved = true;
        link.remove();

        newLink.options.isNew = false;
        newLink.isCloned = true;
        newLink.setSourcePort(outPort);
        newLink.setTargetPort(inPort);

        connections[key].link = newLink;

        model.addAll(newLink);
    };

    const selectNodeAndConnection = (nodeId, modelConnections, nodeGraphicalNetwork) => {
        const connections = Object.assign({}, modelConnections);

        const selectedNode = nodeGraphicalNetwork[nodeId];

        const inputLinks = selectedNode[`in_${nodeId}`];
        const outputLinks = selectedNode[`out_${nodeId}`];
        const allLinks = inputLinks.concat(outputLinks);
        const activeLinks = allLinks.filter(v => (v.state === constants.NodeConnectionState.Active || v.state === constants.NodeConnectionState.TransferPoint));

        Object.keys(connections).forEach(key => {
            if (connections[key].link.options.isSelected) {
                updateSelectConnection(connections, key, false);
            }
        });

        activeLinks.forEach(v => {
            updateSelectConnection(connections, `out_${v.sourceNodeId}-in_${v.destinationNodeId}`, true);
        });

        isNewLink = false;
        return connections;
    };

    const getupdatedDestinationConnection = (graphicalNetwork, sourceNodeId, destinationNodeId, newState) => {
        const in_destinationNode = `in_${destinationNodeId}`;
        const sourceNodes = [...graphicalNetwork[destinationNodeId][in_destinationNode]];
        const index = sourceNodes.findIndex(x => x.sourceNodeId === Number(sourceNodeId));
        sourceNodes[index].state = newState;
        const destinationNetwork = Object.assign({}, graphicalNetwork[destinationNodeId]);
        destinationNetwork[in_destinationNode] = sourceNodes;

        return destinationNetwork;
    };

    const getupdatedSourceConnection = (graphicalNetwork, sourceNodeId, destinationNodeId, newState) => {
        const out_sourceNode = `out_${sourceNodeId}`;
        const destinationNodes = [...graphicalNetwork[sourceNodeId][out_sourceNode]];
        const index = destinationNodes.findIndex(x => x.destinationNodeId === Number(destinationNodeId));
        destinationNodes[index].state = newState;
        const sourceNetwork = Object.assign({}, graphicalNetwork[sourceNodeId]);
        sourceNetwork[out_sourceNode] = destinationNodes;

        return sourceNetwork;
    };

    const getSourceNodeAfterDelete = (graphicalNetwork, sourceNodeId, destinationNodeId) => {
        const out_sourceNode = `out_${sourceNodeId}`;
        const destinationNodes = [...graphicalNetwork[sourceNodeId][out_sourceNode]];

        const index = destinationNodes.findIndex(x => x.destinationNodeId === Number(destinationNodeId));
        destinationNodes.splice(index, 1);

        const sourceNetwork = Object.assign({}, graphicalNetwork[sourceNodeId]);

        sourceNetwork[out_sourceNode] = destinationNodes;
        sourceNetwork.outputConnections = sourceNetwork.outputConnections - 1;

        return sourceNetwork;
    };

    const getDestinationNodeAfterDelete = (graphicalNetwork, sourceNodeId, destinationNodeId) => {
        const in_destinationNode = `in_${destinationNodeId}`;
        const sourceNodes = [...graphicalNetwork[destinationNodeId][in_destinationNode]];
        const index = sourceNodes.findIndex(x => x.sourceNodeId === Number(sourceNodeId));
        sourceNodes.splice(index, 1);

        const destinationNetwork = Object.assign({}, graphicalNetwork[destinationNodeId]);

        destinationNetwork[in_destinationNode] = sourceNodes;
        destinationNetwork.inputConnections = destinationNetwork.inputConnections - 1;

        return destinationNetwork;
    };

    const getNodeAfterDelete = (graphicalNetwork, nodeId) => {
        const in_destinationNode = `in_${nodeId}`;
        const out_sourceNode = `out_${nodeId}`;
        const sourceNodes = [...graphicalNetwork[nodeId][in_destinationNode]];
        const destinationNodes = [...graphicalNetwork[nodeId][out_sourceNode]];
        let index = sourceNodes.findIndex(x => x.sourceNodeId === Number(nodeId));
        sourceNodes.splice(index, 1);
        index = destinationNodes.findIndex(x => x.destinationNodeId === Number(nodeId));
        destinationNodes.splice(index, 1);

        const nodeNetwork = Object.assign({}, graphicalNetwork[nodeId]);

        nodeNetwork[in_destinationNode] = sourceNodes;
        nodeNetwork[out_sourceNode] = destinationNodes;
        nodeNetwork.inputConnections = nodeNetwork.inputConnections - 1;
        nodeNetwork.outputConnections = nodeNetwork.outputConnections - 1;
        return nodeNetwork;
    };

    return {
        initialize: networkFactory => {
            dagreEngine = new DagreEngine();
            engine = createEngine({ registerDefaultDeleteItemsAction: false });
            model = new DiagramModel();
            isNewLink = false;
            const state = engine.getStateMachine().getCurrentState();
            if (state instanceof DefaultDiagramState) {
                state.dragNewLink.config.allowLooseLinks = false;
            }
            networkDiagramFactory = networkFactory;
            networkDiagramFactory.registerNodeFactory(engine);
            networkDiagramFactory.registerLinkFactory(engine);
        },
        getEngine: () => {
            return engine;
        },
        repaint: () => {
            engine.repaintCanvas();
        },
        zoomIn: () => {
            engine.getModel().setZoomLevel(engine.getModel().getZoomLevel() + 10);
            engine.repaintCanvas();
        },
        zoomOut: () => {
            engine.getModel().setZoomLevel(engine.getModel().getZoomLevel() - 10);
            engine.repaintCanvas();
        },
        zoomReset: () => {
            engine.getModel().setZoomLevel(100);
            engine.repaintCanvas();
        },
        organize() {
            dagreEngine.redistribute(this.getModel());
            this.reroute();
            engine.repaintCanvas();
        },
        reroute() {
            engine.getLinkFactories()
                .getFactory(PathFindingLinkFactory.NAME)
                .calculateRoutingMatrix();
        },
        getModel: () => {
            return engine.model;
        },
        getNodeId: id => {
            return getNodeId(id);
        },
        isNewLink: () => {
            return isNewLink;
        },
        isInvalidConnection: (link, nodeGraphicalNetwork, modelConnections) => {
            return isInvalidConnection(link, nodeGraphicalNetwork, modelConnections);
        },
        getInvalidLink: () => {
            return getInvalidLink();
        },
        addToModelConnections: (connections, key, link) => {
            addToModelConnections(connections, key, link);
        },
        isReverseConnection: link => {
            return isReverseConnection(link);
        },
        isLooseConnection: link => {
            return isLooseConnection(link);
        },
        isExistingConnection: (link, modelConnections) => {
            return isExistingConnection(link, modelConnections);
        },
        isValidConnection: (link, modelConnections) => {
            return !isLooseConnection(link) && !isExistingConnection(link, modelConnections) && !isReverseConnection(link);
        },
        buildNetwork: (data, modelConnections, modelNodes) => {
            const nodeGraphicalNetwork = data;
            const connections = Object.assign({}, modelConnections);
            const nodes = Object.assign({}, modelNodes);
            if (nodeGraphicalNetwork) {
                generateNode(nodeGraphicalNetwork, nodes);
                generateNodeLinks(nodeGraphicalNetwork, connections);
                registerListener(connections);
                engine.setModel(model);
            }
        },
        constructAndAddLinkClone: (unsavedLink, state, isInvalidReason = null) => {
            constructAndAddLinkClone(unsavedLink, state, isInvalidReason);
        },
        updateNodeConnection: (unsavedConnection, modelConnections) => {
            return updateNodeConnection(unsavedConnection, modelConnections);
        },
        removeNodeConnection: (unsavedConnection, modelConnections) => {
            return removeNodeConnection(unsavedConnection, modelConnections);
        },
        updateNodeConnectionToInactivate: (newNodeDiagram, connectionToDelete, modelConnections, connectionState) => {
            return updateNodeConnectionToInactivate(newNodeDiagram, connectionToDelete, modelConnections, connectionState);
        },
        removeNodeConnectionToDelete: (connectionToDelete, modelConnections) => {
            return removeNodeConnectionToDelete(connectionToDelete, modelConnections);
        },
        onMouseUp: (nodeGraphicalNetwork, modelConnections) => {
            onMouseUp(nodeGraphicalNetwork, modelConnections);
        },
        getAllGraphicalNetworkSourceNodeDetails: (sourceNodesDetailsgraphicalNetwork, destinationNodeId, nodeGraphicalNetwork) => {
            getAllGraphicalNetworkSourceNodeDetails(sourceNodesDetailsgraphicalNetwork, destinationNodeId, nodeGraphicalNetwork);
        },
        getAllGraphicalNetworkDestinationNodeDetails: (destinationNodesDetailsgraphicalNetwork, sourceNodeId, nodeGraphicalNetwork) => {
            getAllGraphicalNetworkDestinationNodeDetails(destinationNodesDetailsgraphicalNetwork, sourceNodeId, nodeGraphicalNetwork);
        },
        getNodesToRemove: (nodesToRemove, parentNodeID, nodeGraphicalNetwork, nodeID) => {
            getNodesToRemove(nodesToRemove, parentNodeID, nodeGraphicalNetwork, nodeID);
        },
        deleteNodesAndCollections: (nodesToRemove, nodeID, nodeGraphicalNetwork) => {
            deleteNodesAndCollections(nodesToRemove, nodeID, nodeGraphicalNetwork);
        },
        onRepaintCanvas: () => {
            onRepaintCanvas();
        },
        updateConnectionDetail: (connectionToDelete, nodeGraphicalNetwork, isActive) => {
            return updateConnectionDetail(connectionToDelete, nodeGraphicalNetwork, isActive);
        },
        getColor: (linkId, state, nodeGraphicalNetwork, enableColor, disableColor) => {
            return getColor(linkId, state, nodeGraphicalNetwork, enableColor, disableColor);
        },
        selectNodeAndConnection: (nodeId, modelConnections, nodeGraphicalNetwork) => {
            return selectNodeAndConnection(nodeId, modelConnections, nodeGraphicalNetwork);
        },
        getupdatedDestinationConnection: (graphicalNetwork, sourceNodeId, destinationNodeId, newState) => {
            return getupdatedDestinationConnection(graphicalNetwork, sourceNodeId, destinationNodeId, newState);
        },
        getupdatedSourceConnection: (graphicalNetwork, sourceNodeId, destinationNodeId, newState) => {
            return getupdatedSourceConnection(graphicalNetwork, sourceNodeId, destinationNodeId, newState);
        },
        isSelfConnection: link => {
            return isSelfConnection(link);
        },
        getSourceNodeAfterDelete: (graphicalNetwork, sourceNodeId, destinationNodeId) => {
            return getSourceNodeAfterDelete(graphicalNetwork, sourceNodeId, destinationNodeId);
        },
        getDestinationNodeAfterDelete: (graphicalNetwork, sourceNodeId, destinationNodeId) => {
            return getDestinationNodeAfterDelete(graphicalNetwork, sourceNodeId, destinationNodeId);
        },
        getNodeAfterDelete: (graphicalNetwork, nodeId) => {
            return getNodeAfterDelete(graphicalNetwork, nodeId);
        }
    };
}());

export { networkBuilderService };
