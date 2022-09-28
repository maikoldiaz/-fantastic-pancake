import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { ActionCell, DateCell } from '../../../../common/components/grid/gridCells.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { constants } from '../../../../common/services/constants';
import { dataService } from '../services/dataService';
import { calculationService } from '../services/calculationService';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { setMovementInventoryOwnershipData, initOwnershipDetails } from '../actions';
import { receiveGridData } from '../../../../common/components/grid/actions';
import { openModal } from '../../../../common/actions';
import { utilities } from '../../../../common/services/utilities';
import NumberFormatter from '../../../../common/components/formControl/numberFormatter.jsx';

export class OwnershipNodeDataGrid extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
        this.onRowActionClick = this.onRowActionClick.bind(this);
        this.enableRowActionBuilder = this.enableRowActionBuilder.bind(this);
        this.actionEnabled = this.actionEnabled.bind(this);
        this.executeCalculation = this.executeCalculation.bind(this);
    }

    executeCalculation() {
        const nodeId = this.props.ownershipNodeDetails.nodeId;
        const items = [...this.props.nodeMovementInventoryData];
        const summaryData = [...this.props.ownershipNodeBalance];

        if (items.filter(item => item.status === constants.Modes.Create || item.status === constants.Modes.Update || item.status === constants.Modes.Delete).length > 0) {
            calculationService.calculate(nodeId, items.filter(item => item.status !== constants.Modes.Delete), summaryData);
        }

        summaryData.forEach(item => {
            item.inputs = item.inputs ? parseFloat(item.inputs) : 0;
            item.volume = parseFloat(item.volume);
            item.control = !item.inputs || item.inputs === 0 ?
                <span className="fc-error float-r">{resourceProvider.read('error')}</span> :
                (<NumberFormatter
                    className="float-r fw-600"
                    value={Math.abs(parseFloat(item.volume / item.inputs)).toFixed(2) === '0.00' ? '0.00' : parseFloat(item.volume / item.inputs).toFixed(2)}
                    displayType="text"
                    suffix={constants.Suffix}
                    isNumericString={true} />);
        });

        this.props.updateOwnershipNodeBalance(summaryData);
    }

    filterMovementInventoryData() {
        const filters = this.props.movementInventoryfilters;
        if (filters.product && filters.variableType && filters.owner) {
            const filteredMovementInventoryData = this.props.nodeMovementInventoryData.filter(v => {
                if (v.status && v.status === constants.Modes.Delete) {
                    return false;
                }

                // Scenario - Entrada
                if (filters.variableType.variableTypeId === constants.VariableType.Input) {
                    return (v.destinationProductId === filters.product.productId) && (v.ownerId === filters.owner.elementId) &&
                        (v.destinationNodeId === this.props.ownershipNodeDetails.nodeId) &&
                        (v.variableTypeId === constants.VariableType.Input || v.variableTypeId === constants.VariableType.Output);
                }

                // Scenario - Salida
                if (filters.variableType.variableTypeId === constants.VariableType.Output) {
                    return (v.sourceProductId === filters.product.productId) && (v.ownerId === filters.owner.elementId) &&
                        (v.sourceNodeId === this.props.ownershipNodeDetails.nodeId) &&
                        (v.variableTypeId === constants.VariableType.Input || v.variableTypeId === constants.VariableType.Output);
                }

                return (v.sourceProductId === filters.product.productId || v.destinationProductId === filters.product.productId) && (v.ownerId === filters.owner.elementId) &&
                    (filters.variableType.variableTypeId === v.variableTypeId);
            });
            this.props.loadMovementInventoryGrid(filteredMovementInventoryData);
        }
    }

    onRowActionClick(data, mode) {
        const movementInventoryOwnershipData = this.props.nodeMovementInventoryData.filter(v => v.transactionId === data.transactionId);
        const isMovement = movementInventoryOwnershipData[0].isMovement;
        const type = isMovement ? 'movement' : 'inventory';
        if (mode === constants.Modes.Read) {
            this.props.onInfo(movementInventoryOwnershipData, type);
        } else if (mode === constants.Modes.Delete) {
            this.props.onDelete(movementInventoryOwnershipData, type);
        } else {
            this.props.onEdit(movementInventoryOwnershipData, type);
        }
    }

    enableRowActionBuilder() {
        const enableRowActionFactory = { info: true, edit: false, delete: false };

        const nodeOwnershipStatus = this.props.ownershipNodeDetails && this.props.ownershipNodeDetails.ownershipStatus;
        const variableType = this.props.movementInventoryfilters && this.props.movementInventoryfilters.variableType && this.props.movementInventoryfilters.variableType.variableTypeId;
        const nodeEditorName = this.props.ownershipNodeDetails && this.props.ownershipNodeDetails.editor;

        const editDeleteStatusArray = [constants.OwnershipNodeStatus.OWNERSHIP, constants.OwnershipNodeStatus.UNLOCKED, constants.OwnershipNodeStatus.PUBLISHED,
            constants.OwnershipNodeStatus.REJECTED, constants.OwnershipNodeStatus.REOPENED,
            constants.OwnershipNodeStatus.CONCILIATIONFAILED, constants.OwnershipNodeStatus.RECONCILED, constants.OwnershipNodeStatus.NOTRECONCILED];
        const infoStatusArray = [constants.OwnershipNodeStatus.SENT, constants.OwnershipNodeStatus.FAILED, constants.OwnershipNodeStatus.PUBLISHING];

        if (dataService.compareStatus(nodeOwnershipStatus, infoStatusArray, 'eq', 'or')) {
            enableRowActionFactory.info = false;
        } else if ((nodeOwnershipStatus === constants.OwnershipNodeStatus.LOCKED && nodeEditorName === this.props.currentUser)) {
            enableRowActionFactory.info = true;
        }

        if (variableType === constants.VariableType.InitialInventory) {
            enableRowActionFactory.edit = false;
        } else if ((nodeOwnershipStatus === constants.OwnershipNodeStatus.LOCKED && nodeEditorName === this.props.currentUser) ||
            (dataService.compareStatus(nodeOwnershipStatus, editDeleteStatusArray, 'eq', 'or'))) {
            enableRowActionFactory.edit = true;
        }
        if (variableType === constants.VariableType.InitialInventory || variableType === constants.VariableType.FinalInventory) {
            enableRowActionFactory.delete = false;
        } else if ((nodeOwnershipStatus === constants.OwnershipNodeStatus.LOCKED && nodeEditorName === this.props.currentUser)) {
            enableRowActionFactory.delete = true;
        } else if (dataService.compareStatus(nodeOwnershipStatus, editDeleteStatusArray, 'eq', 'or')) {
            enableRowActionFactory.delete = true;
        }
        return enableRowActionFactory;
    }

    actionEnabled(key) {
        const enableRowActionFactory = this.enableRowActionBuilder();
        return utilities.getValue(enableRowActionFactory, key);
    }

    getColumns() {
        const variableType = this.props.movementInventoryfilters.variableType && this.props.movementInventoryfilters.variableType.variableTypeId;
        const date = rowProps => <DateCell {...this.props} {...rowProps} />;
        const actionCell = rowProps => (<ActionCell {...this.props} {...rowProps} enableInfo={() => this.actionEnabled('info')}
            enableEdit={() => this.actionEnabled('edit')} enableDelete={() => this.actionEnabled('delete')}
            onInfo={v => this.onRowActionClick(v, constants.Modes.Read)} onEdit={v => this.onRowActionClick(v, constants.Modes.Update)}
            onDelete={v => this.onRowActionClick(v, constants.Modes.Delete)} />);
        let columns = [];
        if (!utilities.isNullOrUndefined(variableType)) {
            columns = dataService.buildVariableTypeColumns(variableType, date, this.props);
            columns.push(gridUtils.buildActionColumn(actionCell, '', 140));
        }
        return columns;
    }

    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} wrapperClassName="ep-table-wrap--mh200" />
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.nodeMovementInventoryDataToggler !== this.props.nodeMovementInventoryDataToggler) {
            this.filterMovementInventoryData();
            this.executeCalculation();
        }

        if (prevProps.movementInventoryfilterToggler !== this.props.movementInventoryfilterToggler) {
            this.filterMovementInventoryData();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        edit: true,
        delete: true,
        info: true,
        ownershipNodeDetails: state.nodeOwnership.ownershipNode.nodeDetails,
        ownershipNodeBalance: state.grid.ownershipNodeBalance && state.grid.ownershipNodeBalance.items,
        nodeMovementInventoryData: state.nodeOwnership.ownershipNode.nodeMovementInventoryData,
        nodeMovementInventoryDataToggler: state.nodeOwnership.ownershipNode.nodeMovementInventoryDataToggler,
        movementInventoryfilters: state.nodeOwnership.ownershipNodeDetails.movementInventoryfilters,
        movementInventoryfilterToggler: state.nodeOwnership.ownershipNodeDetails.movementInventoryfilterToggler,
        currentUser: state.root.context.userId
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        loadMovementInventoryGrid: ownershipNodeData => {
            dispatch(receiveGridData(ownershipNodeData, 'ownershipNodeData'));
        },
        onEdit: (ownership, type) => {
            dispatch(setMovementInventoryOwnershipData(ownership));
            dispatch(initOwnershipDetails());
            dispatch(openModal(`${type}Ownership`, constants.Modes.Update, type === 'movement' ? type : 'finalInventory', 'ep-modal--md'));
        },
        onInfo: (ownership, type) => {
            dispatch(setMovementInventoryOwnershipData(ownership));
            dispatch(initOwnershipDetails());
            dispatch(openModal(`${type}Ownership`, constants.Modes.Read, `${type}Information`, 'ep-modal--md'));
        },
        onDelete: (ownership, type) => {
            dispatch(setMovementInventoryOwnershipData(ownership));
            dispatch(initOwnershipDetails());
            dispatch(openModal(`${type}Ownership`, constants.Modes.Delete, `${type}Information`, 'ep-modal--md'));
        },
        updateOwnershipNodeBalance: summaryData => {
            dispatch(receiveGridData(summaryData, 'ownershipNodeBalance'));
        }
    };
};

/* istanbul ignore next */
const gridConfig = () => {
    return {
        name: 'ownershipNodeData',
        odata: false,
        startEmpty: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(OwnershipNodeDataGrid, gridConfig));
