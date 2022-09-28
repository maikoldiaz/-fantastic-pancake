import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { createNodeConnection, removeConnection, clearErrorMessage, updateNodeConnection, getNewConnectionDetail, receiveCreateConnection } from './../../network/actions.js';
import { openMessageModal, closeModal } from '../../../../../common/actions.js';
import { utilities } from '../../../../../common/services/utilities';

export class NodeConnectionModal extends React.Component {
    constructor() {
        super();
        this.onCancel = this.onCancel.bind(this);
    }

    getNodeId(portId) {
        return parseInt(portId.split('_')[1], 10);
    }

    onCancel() {
        this.props.removeConnection(this.props.unsavedConnection);
    }

    getNodeConnection(sourceTargetPort) {
        return {
            sourceNodeId: this.getNodeId(sourceTargetPort.sourcePortId),
            destinationNodeId: this.getNodeId(sourceTargetPort.targetPortId),
            description: `${sourceTargetPort.sourceNodeName}-${sourceTargetPort.destinationNodeName}`,
            isActive: true
        };
    }

    render() {
        return (
            <>
            </>
        );
    }

    componentDidUpdate(prevProps) {
        if (!utilities.isNullOrWhitespace(this.props.createConnectionToggler) && prevProps.createConnectionToggler !== this.props.createConnectionToggler) {
            if (this.props.errorMessage) {
                this.props.removeConnection(this.props.unsavedConnection);
                const opts = { canCancel: true, title: resourceProvider.read('error'), acceptActionAndClose: () => clearErrorMessage(), cancelAction: () => clearErrorMessage() };
                this.props.confirm(this.props.errorMessage, opts);
            } else {
                this.props.updateNodeConnection(this.props.unsavedConnection);
            }
        }
        if (!utilities.isNullOrWhitespace(this.props.connectionCreatedToggler) && prevProps.connectionCreatedToggler !== this.props.connectionCreatedToggler) {
            this.props.onClose();
            this.props.getNewConnectionDetail(this.props.unsavedConnection.sourcePortId, this.props.unsavedConnection.targetPortId);
        }
        if (!utilities.isNullOrWhitespace(this.props.receiveNewConnectionToggler) && prevProps.receiveNewConnectionToggler !== this.props.receiveNewConnectionToggler) {
            this.props.receiveCreateConnection();
        }

        if (prevProps.createUnsavedConnectionToggler !== this.props.createUnsavedConnectionToggler) {
            const opts = { canCancel: true, acceptAction: createNodeConnection(this.getNodeConnection(this.props.unsavedConnection)), cancelAction: () => this.onCancel() };
            this.props.showCreateConnectionConfirm(resourceProvider.read('createNodeConnectionConfirmationMessage'), opts);
        }
    }
}

const mapStateToProps = state => {
    return {
        unsavedConnection: state.nodeGraphicalConnection.unsavedConnection,
        createConnectionToggler: state.nodeGraphicalConnection.createConnectionToggler,
        errorMessage: state.nodeGraphicalConnection.errorMessage,
        connectionCreatedToggler: state.nodeGraphicalConnection.connectionCreatedToggler,
        receiveNewConnectionToggler: state.nodeGraphicalConnection.receiveNewConnectionToggler,
        createUnsavedConnectionToggler: state.nodeGraphicalConnection.createUnsavedConnectionToggler
    };
};

const mapDispatchToProps = dispatch => {
    return {
        removeConnection: connection => {
            dispatch(removeConnection(connection));
        },
        onClose: () => {
            dispatch(closeModal());
        },
        confirm: (message, opts) => {
            dispatch(openMessageModal(message, opts));
        },
        showCreateConnectionConfirm: (message, opts) => {
            dispatch(openMessageModal(message, opts));
        },
        updateNodeConnection: nodeConnection => {
            dispatch(updateNodeConnection(nodeConnection));
        },
        getNewConnectionDetail: (sourceNodeId, destinationNodeId) => {
            dispatch(getNewConnectionDetail(sourceNodeId, destinationNodeId));
        },
        receiveCreateConnection: () => {
            dispatch(receiveCreateConnection());
        }
    };
};


export default connect(mapStateToProps, mapDispatchToProps)(NodeConnectionModal);
