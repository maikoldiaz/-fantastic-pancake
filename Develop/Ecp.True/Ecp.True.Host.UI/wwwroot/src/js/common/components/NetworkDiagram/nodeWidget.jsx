import React from 'react';
import { connect } from 'react-redux';
import { PortWidget } from '@projectstorm/react-diagrams';
import { resourceProvider } from './../../services/resourceProvider.js';
import classNames from 'classnames/bind';
import ReactHtmlParser from 'react-html-parser';
import { networkBuilderService } from '../../services/networkBuilderService.js';
import { constants } from '../../services/constants.js';
import {
    showAllSourceNodesDetails,
    hideAllSourceNodeDetails,
    showAllDestinationNodesDetails,
    hideAllDestinationNodeDetails,
    resetState,
    updateNetworkModel,
    selectNode,
    updateModelConnections,
    onInputAndOutputClicked,
    showNodePanel,
    clearErrorMessage
} from './../../../modules/administration/nodeConnection/network/actions.js';
import { initGraphicalUpdateNode, requestUpdateNode, requestNodeStorageLocations } from '../../../modules/administration/node/manageNode/actions.js';
import { openMessageModal } from '../../../common/actions';
import { utilities } from '../../services/utilities.js';
import Tooltip from '../tooltip/tooltip.jsx';

export class NetworkDiagramNodeWidget extends React.Component {
    constructor() {
        super();
        this.onMouseOver = this.onMouseOver.bind(this);
        this.onMouseOut = this.onMouseOut.bind(this);
        this.onShowAndHideAllSourceNodeDetails = this.onShowAndHideAllSourceNodeDetails.bind(this);
        this.onShowAndHideAllDestinationNodeDetails = this.onShowAndHideAllDestinationNodeDetails.bind(this);
        this.onRepaintAndUpdateNetworkModel = this.onRepaintAndUpdateNetworkModel.bind(this);
    }

    onMouseOver(e) {
        const engine = networkBuilderService.getEngine();
        const currentState = engine.getStateMachine().currentState;
        const unsavedLink = currentState.link;
        const invalidLink = networkBuilderService.getInvalidLink();
        if (unsavedLink) {
            const inPort = this.props.node.getPort('in');
            if (invalidLink && invalidLink.getTargetPort().getID() !== inPort.getID()) {
                return;
            }
            unsavedLink.setTargetPort(inPort);
            const isInvalidReason = networkBuilderService.isInvalidConnection(unsavedLink, this.props.nodeGraphicalNetwork, this.props.modelConnections);
            if (isInvalidReason) {
                networkBuilderService.constructAndAddLinkClone(unsavedLink, constants.NodeConnectionState.Invalid, isInvalidReason);
                e.currentTarget.classList.add('cursor-restrict');
            }
        }
    }

    onMouseOut(e) {
        e.currentTarget.classList.remove('cursor-restrict');
    }

    onRepaintAndUpdateNetworkModel() {
        networkBuilderService.onRepaintCanvas();
        setTimeout(async () => {
            // update the network model
            this.props.updateNetworkModel(this.props.cloneGraphicalNetwork);
            this.props.onInputAndOutputClicked();
        }, constants.Timeouts.Graphical);
    }

    onShowAndHideAllSourceNodeDetails(e) {
        // If there are connections then show and hide should perform
        if (this.props.nodeDetails.inputConnections > 0) {
            const nodeGraphicalNetwork = this.props.nodeGraphicalNetwork;
            if (this.props.nodeDetails.inputConnections > this.props.nodeDetails['in_' + this.props.nodeDetails.nodeId].length) {
                // show the network
                this.props.showAllSourceNodeDetails(this.props.nodeDetails.nodeId, nodeGraphicalNetwork);
            } else {
                networkBuilderService.onRepaintCanvas();
                setTimeout(async () => {
                    // hide the network
                    this.props.hideAllSourceNodeDetails(this.props.nodeDetails.nodeId, nodeGraphicalNetwork);
                    this.props.onInputAndOutputClicked();
                }, constants.Timeouts.Graphical);
            }
        }
        e.stopPropagation();
    }

    onShowAndHideAllDestinationNodeDetails(e) {
        // If there are connections then show and hide should perform
        if (this.props.nodeDetails.outputConnections > 0) {
            const nodeGraphicalNetwork = this.props.nodeGraphicalNetwork;
            if (this.props.nodeDetails.outputConnections > this.props.nodeDetails['out_' + this.props.nodeDetails.nodeId].length) {
                // show the network
                this.props.showAllDestinationNodeDetails(this.props.nodeDetails.nodeId, nodeGraphicalNetwork);
            } else {
                networkBuilderService.onRepaintCanvas();
                setTimeout(async () => {
                    // hide the network
                    this.props.hideAllDestinationNodeDetails(this.props.nodeDetails.nodeId, nodeGraphicalNetwork);
                    this.props.onInputAndOutputClicked();
                }, constants.Timeouts.Graphical);
            }
        }
        e.stopPropagation();
    }

    selectNodeAndConnections(nodeId) {
        this.props.selectNode(nodeId);
        if (this.props.modelConnections && Object.keys(this.props.modelConnections).length > 0) {
            const modelConnections = networkBuilderService.selectNodeAndConnection(nodeId, this.props.modelConnections, this.props.nodeGraphicalNetwork);
            this.props.updateModelConnections(modelConnections);
        }
    }

    render() {
        return (
            <>
                {this.props.nodeDetails &&
                    <div className={classNames('ep-diagram__node ' + 'ep-diagram__node--' + this.props.nodeDetails.segmentColor,
                        {
                            ['ep-diagram__node--inactive']: !this.props.nodeDetails.isActive, ['ep-diagram__node--selected']: this.props.isSelected === this.props.nodeDetails.nodeId ? true : false,
                            ['ep-diagram__node--unsaved']: this.props.nodeDetails.isUnsaved
                        })}
                    style={{ borderTopColor: this.props.nodeDetails.segmentColor }}
                    onClick={() => this.selectNodeAndConnections(this.props.nodeDetails.nodeId)}>
                        <div className="ep-diagram__node-body">
                            <div className="ep-diagram__node-icndrg"><i className="fas fa-arrows-alt" /></div>
                            {!utilities.isNullOrUndefined(this.props.nodeDetails.isUnsaved) ? null :
                                <div className="ep-diagram__node-icn" style={{ backgroundColor: this.props.nodeDetails.isActive && this.props.nodeDetails.segmentColor }}>
                                    {this.props.nodeDetails.isActive && ReactHtmlParser(this.props.nodeDetails.nodeTypeIcon)}
                                    {!this.props.nodeDetails.isActive && <span className="ep-diagram__node-icnlock"><i className="fas fa-lock" /></span>}
                                </div>}
                            <div className="ep-diagram__node-info">
                                <div className="m-b-2">
                                    <Tooltip body={this.props.nodeDetails.nodeName}>
                                        <div className="ep-data fs-12 fw-sb">{this.props.nodeDetails.nodeName}</div>
                                    </Tooltip>
                                </div>
                                <div className="row m-b-1">
                                    <div className="col-md-4">
                                        <label className="ep-label fs-8 m-b-0">{resourceProvider.read('segment')}</label>
                                        <Tooltip body={this.props.nodeDetails.segment}>
                                            <div className="ep-data fs-12 fw-sb">{this.props.nodeDetails.segment}</div>
                                        </Tooltip>
                                    </div>
                                    <div className="col-md-5">
                                        <label className="ep-label fs-8 m-b-0">{resourceProvider.read('operator')}</label>
                                        <Tooltip body={this.props.nodeDetails.operator}>
                                            <div className="ep-data fs-12 fw-sb">{this.props.nodeDetails.operator}</div>
                                        </Tooltip>
                                    </div>
                                    <div className="col-md-3">
                                        <label className="ep-label fs-8 m-b-0">{resourceProvider.read('order')}</label>
                                        <Tooltip body={this.props.nodeDetails.order}>
                                            <div className="ep-data fs-12 fw-sb">{this.props.nodeDetails.order}</div>
                                        </Tooltip>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-4">
                                        <label className="ep-label fs-8 m-b-0">{resourceProvider.read('controllimit')}</label>
                                        <div className="ep-data fs-12 fw-sb">{this.props.nodeDetails.controlLimit && constants.Prefix + this.props.nodeDetails.controlLimit}</div>
                                    </div>
                                    <div className="col-md-5">
                                        <label className="ep-label fs-8 m-b-0">{resourceProvider.read('acceptablebalance')}</label>
                                        <div className="ep-data fs-12 fw-sb">
                                            {this.props.nodeDetails.acceptableBalancePercentage && '' + this.props.nodeDetails.acceptableBalancePercentage + constants.Suffix}
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="custom-node-color" style={{ backgroundColor: this.props.node.color }} />
                        </div>
                        <footer className="ep-diagram__node-footer">
                            <span className="ep-diagram__node-port-in">
                                <PortWidget engine={this.props.engine} port={this.props.node.getPort('in')}>
                                    <span className="fas fa-plus" onMouseOver={e => this.onMouseOver(e)} onMouseOut={e => this.onMouseOut(e)} />
                                </PortWidget>
                            </span>
                            <span className="ep-diagram__node-port-out">
                                <PortWidget engine={this.props.engine} port={this.props.node.getPort('out')}>
                                    <span className="fas fa-plus" />
                                </PortWidget>
                            </span>
                            <div id="ep-diagram__node-port" className="ep-diagram__node-port ep-diagram__node-port--in" onClick={e => this.onShowAndHideAllSourceNodeDetails(e)}>
                                <div className="ep-diagram__node-port-icn"><i className="fas fa-sign-in-alt m-r-1" /></div>
                                <div className="ep-diagram__node-port-data text-right">
                                    <div className="ep-diagram__node-port-txt fw-sb">{resourceProvider.read('input')}</div>
                                    <div className="ep-diagram__node-port-val">{this.props.nodeDetails.inputConnections}</div>
                                </div>
                            </div>
                            {utilities.isNullOrUndefined(this.props.nodeDetails.isUnsaved) ?
                                <button className="ep-diagram__node-edit" onClick={() => this.props.onEdit(this.props.nodeDetails)}
                                    style={{ backgroundColor: this.props.nodeDetails.segmentColor }}><i className="far fa-edit m-r-1" />{resourceProvider.read('edit')}</button> :
                                <button className="ep-diagram__node-edit ep-diagram__node-edit--lt"
                                    style={{ backgroundColor: 'transparent' }}><i className="far fa-edit m-r-1" />{resourceProvider.read('edit')}</button>}
                            <div className="ep-diagram__node-port ep-diagram__node-port--out" onClick={e => this.onShowAndHideAllDestinationNodeDetails(e)}>
                                <div className="ep-diagram__node-port-data text-left">
                                    <div className="ep-diagram__node-port-txt fw-sb">{resourceProvider.read('output')}</div>
                                    <div className="ep-diagram__node-port-val">{this.props.nodeDetails.outputConnections}</div>
                                </div>
                                <div className="ep-diagram__node-port-icn"><i className="fas fa-sign-out-alt m-l-1" /></div>
                            </div>
                        </footer>
                        {utilities.isNullOrUndefined(this.props.nodeDetails.isUnsaved) ? null : <div className="ep-diagram__node-unsaved" />}
                    </div>
                } </>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.getErrorNodeDetailsToggler !== this.props.getErrorNodeDetailsToggler) {
            const opts = { canCancel: false, title: resourceProvider.read('error'), acceptActionAndClose: clearErrorMessage() };
            this.props.confirm(resourceProvider.read('getNodesDetailsError'), opts);
        }

        if (prevProps.receiveGraphicalNetworkDataToggler !== this.props.receiveGraphicalNetworkDataToggler && this.props.nodeDetails.nodeId === this.props.selectedNodeId) {
            this.onRepaintAndUpdateNetworkModel();
        }

        if (prevProps.inputOutputClickedToggler !== this.props.inputOutputClickedToggler && this.props.nodeDetails.nodeId === this.props.selectedNodeId) {
            this.selectNodeAndConnections(this.props.selectedNodeId);
        }

        if (prevProps.updateToggler !== this.props.updateToggler) {
            this.props.showNodePanel();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        nodeDetails: state.nodeGraphicalConnection.graphicalNetwork[ownProps.node.id],
        nodeGraphicalNetwork: state.nodeGraphicalConnection.graphicalNetwork,
        modelConnections: state.nodeGraphicalConnection.modelConnections,
        getErrorNodeDetailsToggler: state.nodeGraphicalConnection.getErrorNodeDetailsToggler,
        receiveGraphicalNetworkDataToggler: state.nodeGraphicalConnection.receiveGraphicalNetworkDataToggler,
        cloneGraphicalNetwork: state.nodeGraphicalConnection.cloneGraphicalNetwork,
        selectedNodeId: state.nodeGraphicalConnection.selectedNodeId,
        isSelected: state.nodeGraphicalConnection.isSelected,
        inputOutputClickedToggler: state.nodeGraphicalConnection.inputOutputClickedToggler,
        updateToggler: state.node.manageNode.updateToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        resetState: () => {
            dispatch(resetState());
        },
        showAllSourceNodeDetails: (destinationNodeId, nodeGraphicalNetwork) => {
            dispatch(showAllSourceNodesDetails(destinationNodeId, nodeGraphicalNetwork));
        },
        hideAllSourceNodeDetails: (destinationNodeId, nodeGraphicalNetwork) => {
            dispatch(hideAllSourceNodeDetails(destinationNodeId, nodeGraphicalNetwork));
        },
        showAllDestinationNodeDetails: (sourceNodeId, nodeGraphicalNetwork) => {
            dispatch(showAllDestinationNodesDetails(sourceNodeId, nodeGraphicalNetwork));
        },
        hideAllDestinationNodeDetails: (sourceNodeId, nodeGraphicalNetwork) => {
            dispatch(hideAllDestinationNodeDetails(sourceNodeId, nodeGraphicalNetwork));
        },
        confirm: (message, opts) => {
            dispatch(openMessageModal(message, opts));
        },
        updateNetworkModel: nodeGraphicalNetwork => {
            dispatch(updateNetworkModel(nodeGraphicalNetwork));
        },
        selectNode: nodeId => {
            dispatch(selectNode(nodeId));
        },
        updateModelConnections: modelConnections => {
            dispatch(updateModelConnections(modelConnections));
        },
        onInputAndOutputClicked: () => {
            dispatch(onInputAndOutputClicked());
        },
        onEdit: node => {
            dispatch(requestUpdateNode(node.nodeId));
            dispatch(initGraphicalUpdateNode(node));
            dispatch(requestNodeStorageLocations(node.nodeId));
        },
        showNodePanel: () => {
            dispatch(showNodePanel(true));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(NetworkDiagramNodeWidget);
