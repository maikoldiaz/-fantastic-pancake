import * as React from 'react';
import { constants } from '../../services/constants';
import { connect } from 'react-redux';
import { updateNodeConnectionDetail, createConnectionToDelete, resetConnectionToDelete, confirmEnable } from '../../../modules/administration/nodeConnection/network/actions.js';
import { resourceProvider } from '../../services/resourceProvider';
import { openMessageModal } from '../../actions.js';
import { networkBuilderService } from '../../services/networkBuilderService.js';
import { svgPaths } from '../../services/svgPathService';

class EnableConfigurationButton extends React.Component {
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
        if (connections.length > 0 && connections[0].state === 'Active') {
            return;
        }

        const connectionToUpdate = {
            sourcePortId: nodes[0],
            targetPortId: nodes[1],
            sourceNodeName: this.props.nodeGraphicalNetwork[sourceNode].nodeName,
            destinationNodeName: this.props.nodeGraphicalNetwork[targetNode].nodeName,
            state: constants.NodeConnectionState.Active
        };

        this.props.createConnectionToDelete(connectionToUpdate, sourceNode, targetNode);
        this.props.openConfirmModal({ canCancel: true, acceptAction: confirmEnable(), cancelAction: resetConnectionToDelete(), closeAction: resetConnectionToDelete() });
    }

    render() {
        return (
            <g id={`btnEnableConnection_${this.props.linkId}`} className="ep-diagram__btn" transform="translate(18 30)" onClick={() => this.onLinkClick(this.props.linkId)}>
                <title>{resourceProvider.read('activate')}</title>
                <ellipse cx="13.413" cy="12.775" rx="13.413" ry="12.775" transform="translate(0 1.277)" fill="silver" />
                <g transform="translate(0 0)" fill={
                    networkBuilderService.getColor(this.props.linkId, constants.NodeConnectionState.Active, this.props.nodeGraphicalNetwork)} stroke="#dfdfdf" strokeWidth="1">
                    <circle cx="12.775" cy="12.775" r="12.775" stroke="none" />
                    <circle cx="12.775" cy="12.775" r="12.275" fill="none" />
                </g>
                <g transform="translate(5.749 5.749)">
                    <g transform="translate(0)" fill="#fff">
                        <path d={svgPaths.enableButton.innerCircleOutline} stroke="none" />
                        <path d={svgPaths.enableButton.innerCircle} stroke="none" fill={
                            networkBuilderService.getColor(this.props.linkId, constants.NodeConnectionState.Active, this.props.nodeGraphicalNetwork, '#cccccc', '#1592e6')} />
                    </g>
                    <rect width="3.832" height="3.832" transform="matrix(0.602, -0.799, 0.799, 0.602, 8.878, 3.317)" fill="#fff" />
                    <path d="M-16401.555,7285.56l2.514,2.543,6.615-8.69" transform="translate(16405.955 -7278.885)" fill="none" stroke="#2f353e" strokeLinecap="round" strokeWidth="1" />
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
            dispatch(openMessageModal(resourceProvider.read('activateNodeConnectionConfirmationMessage'), options));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(EnableConfigurationButton);
