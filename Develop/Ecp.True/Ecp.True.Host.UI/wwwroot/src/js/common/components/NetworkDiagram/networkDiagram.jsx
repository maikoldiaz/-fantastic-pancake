import React from 'react';
import { connect } from 'react-redux';
import { CanvasWidget } from '@projectstorm/react-canvas-core';
import {
    clearUnsavedConnection,
    updateModelConnections,
    resetState,
    removeDeletedConnection,
    updateNodeConnectionDetail,
    deleteNodeConnection,
    resetConnectionToDelete,
    updateConnectionState,
    clearErrorMessage,
    getNodeConnectionByNodeIds,
    updateRowVersionForConnectionUpdate
}
    from '../../../modules/administration/nodeConnection/network/actions.js';
import { networkBuilderService } from '../../services/networkBuilderService.js';
import { navigationService } from '../../services/navigationService';
import NetworkDiagramServiceProvider from './networkDiagramServiceProvider.js';
import { utilities } from '../../services/utilities.js';
import { constants } from '../../services/constants.js';
import { resourceProvider } from '../../services/resourceProvider.js';
import { getConnection, setControlLimitSource, getAlgorithmList } from '../../../modules/administration/nodeConnection/attributes/actions.js';
import { openMessageModal, openModal, closeModal } from '../../actions.js';

class NetworkDiagram extends React.Component {
    constructor() {
        super();
        networkBuilderService.initialize(new NetworkDiagramServiceProvider());
    }

    render() {
        if (this.props.nodeGraphicalNetwork) {
            networkBuilderService.buildNetwork(this.props.nodeGraphicalNetwork, this.props.modelConnections, this.props.modelNodes);
        }

        return (
            <div className="full-height" onMouseUp={e => networkBuilderService.onMouseUp(this.props.nodeGraphicalNetwork, this.props.modelConnections, e)}>
                {networkBuilderService.getEngine() && networkBuilderService.getModel() &&
                    <CanvasWidget maxNumberPointsPerLink="0" smartRouting={true} className="ep-diagram__canvas" engine={networkBuilderService.getEngine()} />}
            </div>
        );
    }

    componentDidMount() {
        if (!this.props.requested) {
            navigationService.navigateTo('filter');
        }
    }

    componentDidUpdate(prevProps) {
        if (!utilities.isNullOrWhitespace(this.props.updateConnectionToggler) && prevProps.updateConnectionToggler !== this.props.updateConnectionToggler) {
            const modelConnections = networkBuilderService.updateNodeConnection(this.props.unsavedConnection, this.props.modelConnections);
            this.props.updateModelConnections(modelConnections);
            this.props.clearUnsavedConnection();
        }

        if (!utilities.isNullOrWhitespace(this.props.removeConnectionToggler) && prevProps.removeConnectionToggler !== this.props.removeConnectionToggler) {
            const modelConnections = networkBuilderService.removeNodeConnection(this.props.unsavedConnection, this.props.modelConnections);
            this.props.updateModelConnections(modelConnections);
            this.props.clearUnsavedConnection();
        }

        if (!utilities.isNullOrWhitespace(this.props.hasConfirmedDeleteToggler) && prevProps.hasConfirmedDeleteToggler !== this.props.hasConfirmedDeleteToggler) {
            const sourceNode = networkBuilderService.getNodeId(this.props.connectionToDelete.sourcePortId);
            const targetNode = networkBuilderService.getNodeId(this.props.connectionToDelete.targetPortId);
            this.props.deleteNodeConnection(sourceNode, targetNode);
        }

        if (!utilities.isNullOrWhitespace(this.props.hasConfirmedEnableToggler) && prevProps.hasConfirmedEnableToggler !== this.props.hasConfirmedEnableToggler) {
            const connectionDetail = networkBuilderService.updateConnectionDetail(this.props.connectionToDelete, this.props.nodeGraphicalNetwork, 1);
            this.props.updateNodeConnectionDetail(connectionDetail);
        }

        if (!utilities.isNullOrWhitespace(this.props.hasConfirmedDisableToggler) && prevProps.hasConfirmedDisableToggler !== this.props.hasConfirmedDisableToggler) {
            const connectionDetail = networkBuilderService.updateConnectionDetail(this.props.connectionToDelete, this.props.nodeGraphicalNetwork, 0);
            this.props.updateNodeConnectionDetail(connectionDetail);
        }

        if (!utilities.isNullOrWhitespace(this.props.removeConnectionDetailToggler) && prevProps.removeConnectionDetailToggler !== this.props.removeConnectionDetailToggler) {
            this.props.closeModal();
            if (utilities.isNullOrUndefined(this.props.errorMessage)) {
                const modelConnections = networkBuilderService.removeNodeConnectionToDelete(this.props.connectionToDelete, this.props.modelConnections);
                this.props.updateModelConnections(modelConnections);
                this.props.removeDeletedConnection();

                this.props.resetConnectionToDelete();
            } else {
                this.props.showError(this.props.errorMessage);
                this.props.resetConnectionToDelete();
                this.props.clearErrorMessage();
            }
        }

        if (!utilities.isNullOrWhitespace(this.props.updateConnectionDetailToggler) && prevProps.updateConnectionDetailToggler !== this.props.updateConnectionDetailToggler) {
            this.props.closeModal();
            if (utilities.isNullOrUndefined(this.props.errorMessage)) {
                const modelConnectionName = `${this.props.connectionToDelete.sourcePortId}-${this.props.connectionToDelete.targetPortId}`;
                const newNodeDiagram = this.props.modelConnections[modelConnectionName].link;
                const connectionState = this.props.connectionToDelete.state === constants.NodeConnectionState.Active ?
                    constants.NodeConnectionState.Active : constants.NodeConnectionState.Inactive;

                const modelConnections = networkBuilderService.updateNodeConnectionToInactivate(newNodeDiagram, this.props.connectionToDelete, this.props.modelConnections, connectionState);
                this.props.getNodeConnectionByNodeIds(this.props.sourceNodeId, this.props.destinationNodeId);
                this.props.updateModelConnections(modelConnections);
                this.props.updateConnectionState(connectionState);
            } else {
                this.props.showError(this.props.errorMessage);
                this.props.resetConnectionToDelete();
                this.props.clearErrorMessage();
            }
        }

        if (prevProps.connectionToggler !== this.props.connectionToggler) {
            this.props.closeModal();
            const modelConnectionName = `${this.props.connectionToDelete.sourcePortId}-${this.props.connectionToDelete.targetPortId}`;
            const newNodeDiagram = this.props.modelConnections[modelConnectionName].link;

            const modelConnections = networkBuilderService.updateNodeConnectionToInactivate(
                newNodeDiagram,
                this.props.connectionToDelete,
                this.props.modelConnections,
                this.props.connectionToDelete.state);
            this.props.getNodeConnectionByNodeIds(this.props.sourceNodeId, this.props.destinationNodeId);
            this.props.updateModelConnections(modelConnections);
            this.props.updateConnectionState(this.props.connectionToDelete.state);
        }

        if (!utilities.isNullOrWhitespace(this.props.receiveConnectionAfterUpdateToggler) && prevProps.receiveConnectionAfterUpdateToggler !== this.props.receiveConnectionAfterUpdateToggler) {
            this.props.updateRowVersionForConnectionUpdate(this.props.connectionAfterUpdate);
            this.props.resetConnectionToDelete();
        }

        if (prevProps.receiveConnectionByNodeIDToggler !== this.props.receiveConnectionByNodeIDToggler) {
            this.props.getConnection(this.props.nodeConnectionId);
            this.props.setControlLimitSource('graphicconfigurationnetwork');
        }

        if (prevProps.receiveConnectionToggler !== this.props.receiveConnectionToggler) {
            this.props.openModal();
        }
    }

    componentWillUnmount() {
        this.props.resetState();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        modelConnections: state.nodeGraphicalConnection.modelConnections,
        modelNodes: state.nodeGraphicalConnection.modelNodes,
        nodeGraphicalNetwork: state.nodeGraphicalConnection.graphicalNetwork,
        updateConnectionToggler: state.nodeGraphicalConnection.updateConnectionToggler,
        removeConnectionToggler: state.nodeGraphicalConnection.removeConnectionToggler,
        unsavedConnection: state.nodeGraphicalConnection.unsavedConnection,
        requested: state.nodeGraphicalConnection.requested,
        connectionToDelete: state.nodeGraphicalConnection.connectionToDelete,
        errorMessage: state.nodeGraphicalConnection.errorMessage,
        removeConnectionDetailToggler: state.nodeGraphicalConnection.removeConnectionDetailToggler,
        updateConnectionDetailToggler: state.nodeGraphicalConnection.updateConnectionDetailToggler,
        receiveConnectionByNodeIDToggler: state.nodeConnection.attributes.receiveConnectionByNodeIDToggler,
        receiveConnectionToggler: state.nodeConnection.attributes.receiveConnectionToggler,
        nodeConnectionId: state.nodeConnection.attributes.nodeConnectionId,
        connectionToggler: state.nodeConnection.attributes.connectionToggler,
        connectionAfterUpdate: state.nodeGraphicalConnection.connectionAfterUpdate,
        sourceNodeId: state.nodeGraphicalConnection.sourceNodeId,
        destinationNodeId: state.nodeGraphicalConnection.destinationNodeId,
        receiveConnectionAfterUpdateToggler: state.nodeGraphicalConnection.receiveConnectionAfterUpdateToggler,
        hasConfirmedDeleteToggler: state.nodeGraphicalConnection.hasConfirmedDeleteToggler,
        hasConfirmedEnableToggler: state.nodeGraphicalConnection.hasConfirmedEnableToggler,
        hasConfirmedDisableToggler: state.nodeGraphicalConnection.hasConfirmedDisableToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        updateModelConnections: modelConnections => {
            dispatch(updateModelConnections(modelConnections));
        },
        clearUnsavedConnection: () => {
            dispatch(clearUnsavedConnection());
        },
        resetState: () => {
            dispatch(resetState());
        },
        resetConnectionToDelete: () => {
            dispatch(resetConnectionToDelete());
        },
        removeDeletedConnection: () => {
            dispatch(removeDeletedConnection());
        },
        updateConnectionState: state => {
            dispatch(updateConnectionState(state));
        },
        getConnection: connectionId => {
            dispatch(getConnection(connectionId));
        },
        setControlLimitSource: controlLimitSource => {
            dispatch(setControlLimitSource(controlLimitSource));
        },
        openModal: () => {
            dispatch(getAlgorithmList());
            dispatch(openModal('editConnControlLimit'));
        },
        getNodeConnectionByNodeIds: (sourceNodeId, destinationNodeId) => {
            dispatch(getNodeConnectionByNodeIds(sourceNodeId, destinationNodeId));
        },
        updateRowVersionForConnectionUpdate: connection => {
            dispatch(updateRowVersionForConnectionUpdate(connection));
        },
        closeModal: () => {
            dispatch(closeModal());
        },
        showError: errorMessage => {
            dispatch(openMessageModal(resourceProvider.read(errorMessage), {
                title: 'error',
                canCancel: false
            }));
        },
        clearErrorMessage: () => {
            dispatch(clearErrorMessage());
        },
        updateNodeConnectionDetail: connection => {
            dispatch(updateNodeConnectionDetail(connection));
        },
        deleteNodeConnection: (sourceNode, targetNode) => {
            dispatch(deleteNodeConnection(sourceNode, targetNode));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(NetworkDiagram);
