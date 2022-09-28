import * as React from 'react';
import { constants } from '../../services/constants';
import { connect } from 'react-redux';
import { queryConnectionBySourceAndDestinationNodeId } from '../../../modules/administration/nodeConnection/attributes/actions.js';
import { createConnectionToDelete } from '../../../modules/administration/nodeConnection/network/actions.js';
import { resourceProvider } from '../../services/resourceProvider';
import { networkBuilderService } from '../../services/networkBuilderService.js';
import { svgPaths } from '../../services/svgPathService';

const TransferPointConfigurationButton = props => {
    const onLinkClick = linkId => {
        const nodes = linkId.split('-');
        const sourceNode = nodes[0].substr(nodes[0].indexOf('_') + 1);
        const targetNode = nodes[1].substr(nodes[1].indexOf('_') + 1);

        const out_node = `out_${sourceNode}`;
        const connections = props.nodeGraphicalNetwork[sourceNode][out_node].filter(x => x.destinationNodeId === Number(targetNode));
        if (connections.length > 0 && connections[0].state === constants.NodeConnectionState.TransferPoint) {
            return;
        }

        const connectionToUpdate = {
            sourcePortId: nodes[0],
            targetPortId: nodes[1],
            sourceNodeName: props.nodeGraphicalNetwork[sourceNode].nodeName,
            destinationNodeName: props.nodeGraphicalNetwork[targetNode].nodeName,
            state: constants.NodeConnectionState.TransferPoint
        };

        props.createConnectionToDelete(connectionToUpdate, sourceNode, targetNode);
        props.queryConnectionBySourceAndDestinationNodeId(sourceNode, targetNode);
    };

    return (
        <g id={`btnTransferConnection_${props.linkId}`} className="ep-diagram__btn" transform="translate(-40 10)" onClick={() =>onLinkClick(props.linkId)}>
            <title>{resourceProvider.read('transferPoint')}</title>
            <ellipse cx="13.413" cy="12.775" rx="13.413" ry="12.775" transform="translate(0 1.277)" fill="silver"/>
            <g transform="translate(0 0)" fill={
                networkBuilderService.getColor(props.linkId, constants.NodeConnectionState.TransferPoint, props.nodeGraphicalNetwork)} stroke="#dfdfdf" strokeWidth="1">
                <circle cx="12.775" cy="12.775" r="12.775" stroke="none" />
                <circle cx="12.775" cy="12.775" r="12.275" fill="none"/>
            </g>
            <g transform="translate(9.449 16.524) rotate(-90)">
                <path d="M6.487,1.5H0L1.631,0" transform="translate(0 0)" fill="none" stroke="#2f353e" strokeLinecap="round" strokeLinejoin="round" strokeWidth="1"/>
                <path d="M0,0,1.306,1.484" transform="translate(0 1.505)" fill="none" stroke="#2f353e" strokeLinecap="round" strokeWidth="1"/>
            </g>
            <g transform="translate(13.281 17.072) rotate(-90)">
                <path d="M7.463,1.5H0L1.554,0" transform="translate(7.463 2.988) rotate(180)" fill="none" stroke="#2f353e" strokeLinecap="round"
                    strokeLinejoin="round" strokeWidth="1"/>
                <path d="M0,0,1.306,1.484" transform="translate(7.463 1.484) rotate(180)" fill="none" stroke="#2f353e" strokeLinecap="round" strokeWidth="1"/>
            </g>
            <g transform="translate(5.749 5.749)" fill="none">
                <path d="M7.026,0A7.026,7.026,0,1,1,0,7.026,7.026,7.026,0,0,1,7.026,0Z" stroke="none"/>
                <path d={svgPaths.transferPointButton.innerCircle} stroke="none"
                    fill={networkBuilderService.getColor(props.linkId, constants.NodeConnectionState.Inactive, props.nodeGraphicalNetwork, '#cccccc', '#1592e6')}/>
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
        queryConnectionBySourceAndDestinationNodeId: (sourceNode, targetNode) => {
            dispatch(queryConnectionBySourceAndDestinationNodeId(sourceNode, targetNode));
        },
        createConnectionToDelete: (connectionToDelete, sourceNode, targetNode) => {
            dispatch(createConnectionToDelete(connectionToDelete, sourceNode, targetNode));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(TransferPointConfigurationButton);
