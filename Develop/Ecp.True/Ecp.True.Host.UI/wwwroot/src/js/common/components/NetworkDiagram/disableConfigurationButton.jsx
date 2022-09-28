import React from 'react';
import { constants } from '../../services/constants';
import { updateNodeConnectionDetail, createConnectionToDelete, resetConnectionToDelete, confirmDisable } from '../../../modules/administration/nodeConnection/network/actions.js';
import { connect } from 'react-redux';
import { resourceProvider } from '../../services/resourceProvider';
import { openMessageModal } from '../../actions.js';
import { networkBuilderService } from '../../services/networkBuilderService.js';
import { svgPaths } from '../../services/svgPathService';

class DisableConfigurationButton extends React.Component {
    constructor() {
        super();
        this.onLinkClick = this.onLinkClick.bind(this);
    }

    onLinkClick(linkId) {
        const nodes = linkId.split('-');
        const sourceNode = nodes[0].substr(nodes[0].indexOf('_') + 1);
        const targetNode = nodes[1].substr(nodes[1].indexOf('_') + 1);
        const out_node = `out_${sourceNode}`;
        const connections = this.props.nodeGraphicalNetwork[sourceNode][out_node].filter(x => x.destinationNodeId === Number(targetNode));
        if (connections.length > 0 && connections[0].state === 'Inactive') {
            return;
        }

        const connectionToUpdate = {
            sourcePortId: nodes[0],
            targetPortId: nodes[1],
            sourceNodeName: this.props.nodeGraphicalNetwork[sourceNode].nodeName,
            destinationNodeName: this.props.nodeGraphicalNetwork[targetNode].nodeName,
            state: constants.NodeConnectionState.Inactive
        };

        this.props.createConnectionToDelete(connectionToUpdate, sourceNode, targetNode);
        this.props.openConfirmModal({ canCancel: true, acceptAction: confirmDisable(), cancelAction: resetConnectionToDelete(), closeAction: resetConnectionToDelete() });
    }

    render() {
        return (
            <g id={`btnDisableConnection_${this.props.linkId}`} className="ep-diagram__btn" transform="translate(-18 30)" onClick={() => this.onLinkClick(this.props.linkId)}>
                <title>{resourceProvider.read('Inactivate')} </title>
                <ellipse cx="13.413" cy="12.775" rx="13.413" ry="12.775" transform="translate(0 1.277)" fill="silver" />
                <g transform="translate(0 0)" fill={
                    networkBuilderService.getColor(this.props.linkId, constants.NodeConnectionState.Inactive, this.props.nodeGraphicalNetwork)}>
                    <path d={svgPaths.disableButton.circularSpace} stroke="none" />
                    <path d={svgPaths.disableButton.outerCircle} stroke="none" fill="#dfdfdf" />
                </g>
                <g transform="translate(5.749 5.181)">
                    <g transform="translate(0 0.567)" fill="#fff">
                        <path d={svgPaths.disableButton.innerCircleOutline} stroke="none" />
                        <path d={svgPaths.disableButton.innerCircle} stroke="none"
                            fill={networkBuilderService.getColor(this.props.linkId, constants.NodeConnectionState.Inactive, this.props.nodeGraphicalNetwork, '#cccccc', '#1592e6')} />
                    </g>
                    <rect width="3.832" height="3.832" transform="matrix(0.656, -0.755, 0.755, 0.656, 8.686, 2.892)" fill="#fff" />
                    <path d="M-16401.8,7289.317l8.488-9.763" transform="translate(16405.887 -7278.279)" fill="none" stroke="#3e3e3e" strokeLinecap="round" strokeWidth="1" />
                    <path d="M590.646,546.692l6.018,6.85" transform="translate(-586.304 -542.686)" fill="none" stroke="#3e3e3e" strokeLinecap="round" strokeWidth="1" />
                </g>
            </g>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        nodeGraphicalNetwork: state.nodeGraphicalConnection.graphicalNetwork
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        updateNodeConnectionDetail: connection => {
            dispatch(updateNodeConnectionDetail(connection));
        },
        createConnectionToDelete: (connectionToDelete, sourceNode, targetNode) => {
            dispatch(createConnectionToDelete(connectionToDelete, sourceNode, targetNode));
        },
        openConfirmModal: options => {
            dispatch(openMessageModal(resourceProvider.read('deactivateNodeConnectionConfirmationMessage'), options));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(DisableConfigurationButton);
