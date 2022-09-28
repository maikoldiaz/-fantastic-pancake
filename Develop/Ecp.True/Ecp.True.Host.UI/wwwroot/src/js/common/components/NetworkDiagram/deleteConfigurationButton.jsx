import * as React from 'react';
import { constants } from '../../services/constants';
import { deleteNodeConnection, createConnectionToDelete, confirmDelete, resetConnectionToDelete } from '../../../modules/administration/nodeConnection/network/actions.js';
import { connect } from 'react-redux';
import { resourceProvider } from '../../services/resourceProvider';
import { openMessageModal } from '../../actions.js';
import { svgPaths } from '../../services/svgPathService';

const DeleteConfigurationButton = props => {
    const onLinkClick = linkId => {
        const nodes = linkId.split('-');
        const sourceNode = nodes[0].substr(nodes[0].indexOf('_') + 1);
        const targetNode = nodes[1].substr(nodes[1].indexOf('_') + 1);
        const connectionToDelete = {
            sourcePortId: nodes[0],
            targetPortId: nodes[1],
            sourceNodeName: props.nodeGraphicalNetwork[sourceNode].nodeName,
            destinationNodeName: props.nodeGraphicalNetwork[targetNode].nodeName,
            state: constants.NodeConnectionState.Active
        };
        props.createConnectionToDelete(connectionToDelete, sourceNode, targetNode);
        props.openConfirmModal({ canCancel: true, acceptAction: confirmDelete(), cancelAction: resetConnectionToDelete(), closeAction: resetConnectionToDelete() });
    };

    return (
        <g id={`btnDeleteConnection_${props.linkId}`} className="ep-diagram__btn" transform="translate(40 10)" onClick={() =>onLinkClick(props.linkId)}>
            <title>{resourceProvider.read('delete')}</title>
            <g>
                <ellipse cx="13.413" cy="12.775" rx="13.413" ry="12.775" transform="translate(0 1.714)" fill="silver"/>
                <g transform="translate(0 -0.086)" fill="#fff" stroke="#dfdfdf" strokeWidth="1">
                    <ellipse cx="13.413" cy="12.775" rx="13.413" ry="12.775" stroke="none"/>
                    <ellipse cx="13.413" cy="12.775" rx="13.413" ry="12.775" fill="none"/>
                </g>
            </g>
            <g transform="translate(4.749 3.749)">
                <g>
                    <g transform="translate(-0.207 0.203)" fill="#fff">
                        <path d={svgPaths.deleteButton.innerCircleOutline} stroke="none"/>
                        <path d={svgPaths.deleteButton.innerCircle} stroke="none" fill="#1592e6"/>
                    </g>
                    <g transform="translate(-0.022 0.124)">
                        <path d="M1389.845,285.989l1.245,8.812a1.471,1.471,0,0,0,1.485.91h3.688a1.089,1.089,0,0,0,.814-.91c.048-.718,1.053-8.812,1.053-8.812Z"
                            transform="translate(-1384.881 -280.361)" fill="none" stroke="#2f353e" strokeLinecap="round" strokeWidth="0.5"/>
                        <path d="M1388.988,281.582h8.586" transform="translate(-1384.172 -276.798)" fill="none" stroke="#2f353e" strokeLinecap="round" strokeWidth="0.5"/>
                        <path d="M1401.332,279.848a1.668,1.668,0,0,1,2.729,0" transform="translate(-1393.526 -275.064)" fill="none" stroke="#2f353e" strokeLinecap="round" strokeWidth="0.5"/>
                        <g transform="translate(6.842 6.928)">
                            <path d="M1397.236,290.275l.451,5.517" transform="translate(-1396.262 -289.296)" fill="none" stroke="#2f353e" strokeLinecap="round" strokeWidth="0.5"/>
                            <path d="M.661,0,0,5.517" transform="matrix(0.999, -0.035, 0.035, 0.999, 2.877, 0.993)" fill="none" stroke="#2f353e" strokeLinecap="round" strokeWidth="0.5"/>
                        </g>
                    </g>
                </g>
            </g>
        </g>
    );
};

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        nodeGraphicalNetwork: state.nodeGraphicalConnection.graphicalNetwork
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        deleteNodeConnection: (sourceNode, targetNode) => {
            dispatch(deleteNodeConnection(sourceNode, targetNode));
        },
        createConnectionToDelete: (connectionToDelete, sourceNode, targetNode) => {
            dispatch(createConnectionToDelete(connectionToDelete, sourceNode, targetNode));
        },
        openConfirmModal: options => {
            dispatch(openMessageModal(resourceProvider.read('eliminateNodeConnectionConfirmationMessage'), options));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(DeleteConfigurationButton);
