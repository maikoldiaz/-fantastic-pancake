import React from 'react';
import { connect } from 'react-redux';
import NetworkDiagram from '../../../../../common/components/NetworkDiagram/networkDiagram.jsx';
import NodeConnectionModal from './nodeConnectionModal.jsx';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import NodePanel from '../../../node/manageNode/components/nodePanel.jsx';
import { persistCurrentGraphicalNetwork, resetState, getGraphicalNode, setUnsavedNode, toggleBG } from '../actions';
import { networkBuilderService } from '../../../../../common/services/networkBuilderService';
import NetworkDiagramServiceProvider from '../../../../../common/components/NetworkDiagram/networkDiagramServiceProvider';
import { utilities } from '../../../../../common/services/utilities';
import { initCreateNode } from '../../../node/manageNode/actions';
import classNames from 'classnames/bind';
import Tooltip from '../../../../../common/components/tooltip/tooltip.jsx';

class GraphicalConnection extends React.Component {
    constructor() {
        super();
        this.onCreateNode = this.onCreateNode.bind(this);
    }

    zoomIn() {
        networkBuilderService.zoomIn();
    }
    zoomOut() {
        networkBuilderService.zoomOut();
    }
    zoomReset() {
        networkBuilderService.zoomReset();
    }
    organize() {
        networkBuilderService.organize();
    }
    onCreateNode() {
        this.props.onCreateNode();
    }

    componentDidUpdate() {
        if (this.props.showCreateNodePanel) {
            document.querySelector('.ep-body__content').scrollTo(0, document.querySelector('.ep-content--half').clientHeight);
        }
    }

    render() {
        return (
            <>
                <section className={classNames('ep-content', { ['ep-content--half']: this.props.showCreateNodePanel })}>
                    <div className="ep-content__body">
                        <section className="ep-diagram">
                            <header className="ep-diagram__header">
                                <div className="ep-diagram__actionbar">
                                    <ul className="ep-diagram__actions">
                                        <Tooltip body={resourceProvider.read('zoomIn')}>
                                            <li id="lnk_diagram_zoomin" className="ep-diagram__action" onClick={() => this.zoomIn()}><i className="fas fa-search-plus m-r-1" /></li>
                                        </Tooltip>
                                        <Tooltip body={resourceProvider.read('zoomOut')}>
                                            <li id="lnk_diagram_zoomout" className="ep-diagram__action" onClick={() => this.zoomOut()}><i className="fas fa-search-minus m-r-1" /></li>
                                        </Tooltip>
                                        <Tooltip body={resourceProvider.read('resetZoom')}>
                                            <li className="ep-diagram__action" onClick={() => this.zoomReset()}><i className="fas fa-undo m-r-1" /><span className="fa-undo--addon" /></li>
                                        </Tooltip>
                                        <Tooltip body={resourceProvider.read('toggleBackground')}>
                                            <li id="lnk_diagram_toggleBg" className="ep-diagram__action ep-diagram__action--dvdr"
                                                onClick={() => this.props.toggleBG()}><i className="fas fa-th-large m-r-1" /></li>
                                        </Tooltip>
                                    </ul>
                                    <button id="btn_newNode" className="ep-btn ep-btn--sec ep-btn--sm" onClick={this.onCreateNode} disabled={this.props.showCreateNodePanel}>
                                        <i className="fas fa-plus-square m-r-1" /><span className="ep-btn__txt">{resourceProvider.read('newNode')}</span></button>
                                </div>
                                <div className="ep-diagram__pane">
                                    <button id="btn_diagram_organize" className="ep-btn ep-btn--sec ep-btn--sm" onClick={() => this.organize()}>{resourceProvider.read('organise')}</button>
                                </div>
                            </header>
                            <div className={classNames('ep-diagram__body', { ['ep-diagram__body--bg']: this.props.graphicalNetworkBgEnabled })}>
                                <NetworkDiagram />
                                <NodeConnectionModal />
                            </div>
                            {this.props.showCreateNodePanel &&
                                <div className="ep-diagram__overlay">
                                    <p className="ep-diagram__overlay-txt">{resourceProvider.read('nodeEditScreenDesc')}</p>
                                </div>
                            }
                        </section>
                    </div>
                </section>
                {this.props.showCreateNodePanel && <NodePanel {...this.props} />}
            </>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        showCreateNodePanel: state.nodeGraphicalConnection.showCreateNodePanel,
        selectedElementId: utilities.isNullOrUndefined(state.nodeFilter.selectedElement) ? 0 : state.nodeFilter.selectedElement.elementId,
        selectedNodeId: utilities.isNullOrUndefined(state.nodeFilter.selectedNodeId) ? 0 : state.nodeFilter.selectedNodeId,
        node: utilities.isNullOrUndefined(state.node.manageNode.node) ? null : state.node.manageNode.node,
        nodeStorageLocations: utilities.isNullOrUndefined(state.node.manageNode.nodeStorageLocations) ? null : state.node.manageNode.nodeStorageLocations,
        isValid: state.node.manageNode.isValid && state.node.manageNode.isAsyncValid && state.node.manageNode.node && state.node.manageNode.node.order >= 1,
        savedNodeId: utilities.isNullOrUndefined(state.node.manageNode.savedNodeId) ? null : state.node.manageNode.savedNodeId,
        graphicalNetworkBgEnabled: state.nodeGraphicalConnection.graphicalNetworkBgEnabled
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onCreateNode: () => {
            dispatch(initCreateNode());
            dispatch(persistCurrentGraphicalNetwork());
            dispatch(resetState());
            networkBuilderService.initialize(new NetworkDiagramServiceProvider());
            setTimeout(async () => {
                // hide the network
                dispatch(setUnsavedNode());
            }, 100);
        },
        resetState: () => {
            dispatch(resetState());
        },
        getGraphicalNode: (elementId, nodeId) => {
            dispatch(getGraphicalNode(elementId, nodeId));
        },
        setUnsavedNodeAndShowNodePanel: () => {
            dispatch(setUnsavedNode());
        },
        toggleBG: () => {
            dispatch(toggleBG());
        }
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(GraphicalConnection);
