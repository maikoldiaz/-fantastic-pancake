import React from 'react';
import { connect } from 'react-redux';
import { submit, SubmissionError } from 'redux-form';
import TabPanels from '../../../../../common/components/tabPanels/tabPanels.jsx';
import CreateNode from './createNode.jsx';
import NodeStorageLocationsGrid from './nodeStorageLocationsGrid.jsx';
import { requestCreateUpdateNode, requestNodeStorageLocations, changeNodesFilterPersistance, updateSameOrderNode, setFailureState, isValidNode, resetReorder, requestUpdateNode } from '../actions';
import { enableDisablePageAction, openMessageModal, openModal, showError, getCategoryElements, getLogisticCenters, hideNotification } from '../../../../../common/actions';
import { routerActions } from '../../../../../common/router/routerActions';
import { navigationService } from '../../../../../common/services/navigationService';
import { constants } from '../../../../../common/services/constants';
import { serverValidator } from '../../../../../common/services/serverValidator';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { utilities } from '../../../../../common/services/utilities';
import NetworkDiagramServiceProvider from '../../../../../common/components/NetworkDiagram/networkDiagramServiceProvider';
import { networkBuilderService } from '../../../../../common/services/networkBuilderService';
import { resetState, removeUnsavedNode, getGraphicalNode, showNodePanel, getGraphicalNetwork } from '../../../nodeConnection/network/actions';

export class NodePanel extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
        this.onSave = this.onSave.bind(this);
        this.onBack = this.onBack.bind(this);
        this.onCancel = this.onCancel.bind(this);
        this.initOnUpdate = this.initOnUpdate.bind(this);
        routerActions.configure('submit', this.onSubmit);
        routerActions.configure('backToNode', this.onBack);
    }
    async onSubmit() {
        this.props.resetReorder();
        if (typeof this.props.node.nodeId === 'undefined') {
            this.props.node.nodeId = 0;
        }

        if (this.props.node.order > constants.DecimalRange.MaxIntValue) {
            this.processSubmit();
            return;
        }

        const node = await serverValidator.validateNodeOrder(this.props.node.nodeId, this.props.node.segment.elementId, this.props.node.order);
        if (node) {
            if (node.nodeId === 0) {
                this.processSubmit();
                return;
            }

            if (parseInt(this.props.node.order, 10) === constants.DecimalRange.MaxIntValue) {
                this.props.showError(resourceProvider.readFormat('orderMaxLimitReached', [node.name]));
                return;
            }

            this.props.confirmReOrder(node);
        }
    }

    onBack() {
        this.props.persist();
    }

    processSubmit() {
        if (this.props.tabs && this.props.tabs.activeTab === 'nodeGeneralInfo') {
            this.props.submit();
        } else if (this.props.tabs && this.props.tabs.activeTab === 'warehouse') {
            this.onSave(this.props.node);
        }
    }

    onSave(nodeValues) {
        const node = Object.assign({}, nodeValues, {
            name: nodeValues.name.trim(),
            nodeTypeId: nodeValues.nodeType.elementId,
            segmentId: nodeValues.segment.elementId,
            order: nodeValues.order,
            operatorId: nodeValues.operator.elementId,
            logisticCenterId: nodeValues.logisticCenter ? nodeValues.logisticCenter.logisticCenterId : null,
            nodeType: null,
            segment: null,
            operator: null,
            unit: null,
            logisticCenter: null,
            sendToSap: nodeValues.sendToSap ? nodeValues.sendToSap : false,
            autoOrder: this.props.autoOrder,
            capacity: nodeValues.capacity ? nodeValues.capacity : null,
            unitId: nodeValues.unit ? nodeValues.unit.elementId : null
        });

        if (nodeValues.capacity && (nodeValues.capacity < constants.DecimalRange.Min || nodeValues.capacity > constants.DecimalRange.Max)) {
            throw new SubmissionError({
                capacity: resourceProvider.read('invalidRange')
            });
        }

        if (node.capacity && !node.unitId) {
            throw new SubmissionError({
                unit: resourceProvider.read('required')
            });
        }

        if (!node.capacity && node.unitId) {
            throw new SubmissionError({
                capacity: resourceProvider.read('required')
            });
        }

        if (nodeValues.capacity >= constants.DecimalRange.Min) {
            const newCapacityValue = nodeValues.capacity;
            const length = newCapacityValue.toString().includes('.') ? newCapacityValue.toString().split('.')[1].length : 0;
            if (length > 2) {
                throw new SubmissionError({
                    capacity: resourceProvider.read('invalidRange')
                });
            }
        }

        if (nodeValues.order > constants.DecimalRange.MaxIntValue) {
            throw new SubmissionError({
                order: resourceProvider.read('invalidRange')
            });
        }

        const nodeStorageLocations = this.props.nodeStorageLocations.map(x =>
            Object.assign({}, x, {
                storageLocation: null,
                storageLocationType: null,
                nodeStorageLocationId: Number(x.nodeStorageLocationId) > 9999000000 ? 0 : x.nodeStorageLocationId,
                sendToSap: node.sendToSap,
                storageLocationId: node.sendToSap === true ? x.storageLocationId : null
            }));

        nodeStorageLocations.forEach(x => {
            x.products.forEach(y => {
                y.product = null;
            });
        });

        node.nodeStorageLocations = nodeStorageLocations;
        const isCreate = this.props.showCreateNodePanel ? this.props.mode === constants.Modes.Create : this.props.route.mode === constants.Modes.Create;
        this.props.requestCreateUpdateNode(node, isCreate, this.props.showCreateNodePanel);
    }

    onCancel() {
        if (this.props.mode === constants.Modes.Create) {
            this.props.setFailureState(false);
            this.props.resetState();
            networkBuilderService.initialize(new NetworkDiagramServiceProvider());
            setTimeout(async () => {
                // hide the network
                this.props.removeUnsavedNode();
            }, 100);
        } else {
            this.props.showNodePanel(false);
        }
    }

    initOnUpdate(disabled, node, mode) {
        if (mode === constants.Modes.Update) {
            this.props.validateNode(disabled, node);
        }
        this.props.getLogisticCenters();
        this.props.getCategoryElements();
    }

    render() {
        return (
            <>
                {this.props.showCreateNodePanel &&
                    <div className="ep-pane ep-pane--wht m-t-6">
                        <h3 className="m-t-2">{this.props.mode === constants.Modes.Create ? resourceProvider.read('createNode') : resourceProvider.read('editNode')}</h3>
                        <ul className="ep-pane__actions">
                            <li><button id="btn_cancel" className="ep-btn ep-btn--link ep-btn--sm" onClick={this.onCancel}>
                                <span className="ep-btn__txt">{resourceProvider.read('cancel')}</span></button></li>
                            <li><button id="btn_submit" className="ep-btn m-l-4" onClick={this.onSubmit} disabled={!this.props.isValid} >
                                <span className="ep-btn__txt">{resourceProvider.read('submit')}</span></button></li>
                        </ul>
                    </div>}
                <section className="ep-content">
                    <div className="ep-content__body">
                        <TabPanels name="nodePanel" defaultTab ="nodeGeneralInfo">
                            <div title="nodeGeneralInfo"><CreateNode mode={this.props.mode} onSubmit={this.onSave} /></div>
                            <div title="warehouse"><NodeStorageLocationsGrid mode={this.props.route.mode} /></div>
                        </TabPanels>
                    </div>
                </section>
            </>
        );
    }

    componentDidMount() {
        if (utilities.isNullOrUndefined(this.props.node.name) && utilities.isNullOrUndefined(this.props.filterValues) && utilities.isNumber(navigationService.getParamByName('nodeId'))) {
            this.props.getNode(navigationService.getParamByName('nodeId'));
        } else {
            this.initOnUpdate(true, this.props.node, this.props.mode);
        }
    }

    componentDidUpdate(prevProps) {
        if (prevProps.isValid !== this.props.isValid) {
            this.props.enableDisableSubmit(!this.props.isValid);
        }

        if (prevProps.refreshToggler !== this.props.refreshToggler) {
            navigationService.navigateTo('manage');
        }

        if (prevProps.autoOrder !== this.props.autoOrder && this.props.autoOrder === true) {
            this.processSubmit();
        }

        if ((prevProps.failureToggler !== this.props.failureToggler) && (!utilities.isNullOrUndefined(this.props.failureToggler))) {
            this.props.confirm(resourceProvider.read('nodoCreationFailed'), resourceProvider.read('error'), false);
            this.onCancel();
        }

        if ((prevProps.nodeSavedToggler !== this.props.nodeSavedToggler) && (!utilities.isNullOrUndefined(this.props.node.segment))) {
            this.props.resetState();
            networkBuilderService.initialize(new NetworkDiagramServiceProvider());
            if (this.props.mode === constants.Modes.Update || this.props.autoOrderNode) {
                this.props.getGraphicalNetwork(this.props.graphicalNodeFilters.elementId, this.props.graphicalNodeFilters.nodeId);
            } else {
                this.props.getGraphicalNode(this.props.node.segment.elementId, this.props.savedNodeId);
            }
        }

        if (this.props.mode !== constants.Modes.Create && prevProps.validNodeToggler !== this.props.validNodeToggler) {
            if (this.props.validNode) {
                this.props.hideError();
            } else {
                this.props.showError(resourceProvider.read('validNode'));
            }
        }

        if (prevProps.updateToggler !== this.props.updateToggler) {
            this.props.requestNodeStorageLocations(this.props.node.nodeId);
            this.initOnUpdate(true, this.props.node, this.props.mode);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        tabs: state.tabs.nodePanel,
        node: state.node.manageNode.node,
        mode: state.node.manageNode.mode,
        nodeStorageLocations: state.node.manageNode.nodeStorageLocations,
        isValid: state.node.manageNode.isValid && state.node.manageNode.isAsyncValid && state.node.manageNode.node && state.node.manageNode.node.order >= 1,
        refreshToggler: state.node.manageNode.refreshToggler,
        autoOrder: state.node.manageNode.autoOrder ? state.node.manageNode.autoOrder : false,
        autoOrderNode: state.node.manageNode.autoOrderNode,
        failureToggler: utilities.isNullOrUndefined(state.node.manageNode.saveNodeFailureToggler) ? null : state.node.manageNode.saveNodeFailureToggler,
        saveNodeFailed: utilities.isNullOrUndefined(state.node.manageNode.saveNodeFailed) ? false : state.node.manageNode.saveNodeFailed,
        showCreateNodePanel: utilities.isNullOrUndefined(state.nodeGraphicalConnection.showCreateNodePanel) ? false : state.nodeGraphicalConnection.showCreateNodePanel,
        nodeSavedToggler: utilities.isNullOrUndefined(state.node.manageNode.nodeSavedToggler) ? null : state.node.manageNode.nodeSavedToggler,
        graphicalNodeFilters: state.nodeGraphicalConnection.filters,
        validNode: state.node.manageNode.validNode,
        filterValues: state.node.manageNode.filterValues,
        updateToggler: state.node.manageNode.updateToggler,
        validNodeToggler: state.node.manageNode.validNodeToggler,
        graphicalNodeUpdate: state.node.manageNode.graphicalNodeUpdate
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        submit: () => {
            dispatch(submit('createNode'));
        },
        resetReorder: () => {
            dispatch(resetReorder());
        },
        confirmReOrder: node => {
            dispatch(updateSameOrderNode(node));
            dispatch(openModal('autoOrderNodes'));
        },
        enableDisableSubmit: disabled => {
            dispatch(enableDisablePageAction('submit', disabled));
        },
        requestCreateUpdateNode: (node, isCreate, isGraphical) => {
            dispatch(requestCreateUpdateNode(node, isCreate, isGraphical));
        },
        requestNodeStorageLocations: nodeId => {
            dispatch(requestNodeStorageLocations(nodeId));
        },
        persist: () => {
            dispatch(changeNodesFilterPersistance(true));
        },
        confirm: (message, title, canCancel) => {
            dispatch(openMessageModal(message, {
                title: title,
                canCancel: canCancel
            }));
        },
        resetState: () => {
            dispatch(resetState());
        },
        removeUnsavedNode: () => {
            dispatch(removeUnsavedNode());
        },
        setFailureState: status => {
            dispatch(setFailureState(status));
        },
        getGraphicalNode: (elementId, nodeId) => {
            dispatch(getGraphicalNode(elementId, nodeId));
        },
        getGraphicalNetwork: (elementId, nodeId) => {
            setTimeout(async () => {
                // hide the network
                dispatch(getGraphicalNetwork(elementId, nodeId));
            }, 100);
        },
        showNodePanel: show => {
            dispatch(showNodePanel(show));
        },
        showError: message => {
            dispatch(showError(message, false));
        },
        getNode: nodeId => {
            dispatch(requestUpdateNode(nodeId));
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        getLogisticCenters: () => {
            dispatch(getLogisticCenters());
        },
        validateNode: (disabled, node) => {
            dispatch(enableDisablePageAction('submit', disabled));
            dispatch(isValidNode(node));
        },
        hideError: () => {
            dispatch(hideNotification());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(NodePanel);
