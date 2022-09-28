import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../../../common/services/apiService';
import { ActionCell, StatusCell } from '../../../../../../common/components/grid/gridCells.jsx';
import { openModal, openMessageModal, closeModal } from '../../../../../../common/actions';
import { constants } from '../../../../../../common/services/constants';
import { refreshGrid } from '../../../../../../common/components/grid/actions';
import PageActions from '../../../../../../common/router/pageActions.jsx';
import { optionService } from '../../../../../../common/services/optionService';
import { dataService } from '../../dataService';
import { initNodeCostCenter, deleteNodeCostCenter, clearStatusNodeCostCenter } from '../../actions';
import { resourceProvider } from '../../../../../../common/services/resourceProvider';


export class NodeCostCenterGrid extends React.Component {
    constructor() {
        super();
        this.getStatus = this.getStatus.bind(this);
        this.getColumns = this.getColumns.bind(this);
    }

    getStatus(row) {
        return row.state;
    }

    getColumns() {
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} onDelete={v => this.onDelete(v)} />;
        const status = rowProps => <StatusCell {...this.props} {...rowProps} />;

        const columns = [];

        columns.push(gridUtils.buildTextColumn('sourceNode.name', this.props, '', 'sourceNode'));
        columns.push(gridUtils.buildTextColumn('destinationNode.name', this.props, '', 'destinationNode'));
        columns.push(gridUtils.buildTextColumn('movementTypeCategoryElement.name', this.props, '', 'movementType'));
        columns.push(gridUtils.buildTextColumn('costCenterCategoryElement.name', this.props, '', 'costCenter'));
        columns.push(gridUtils.buildSelectColumn('isActive', this.props, status, 'state', {
            width: 150,
            values: optionService.getGridStatusTypes()
        }));

        columns.push(gridUtils.buildActionColumn(actions, null, 100));

        return columns;
    }

    onDelete(data) {
        const opts = {
            title: resourceProvider.read('deleteNodeCosCenter'), canCancel: true, acceptAction: () => {
                this.props.deleteNodeCostCenter(data);
            }
        };
        this.props.openConfirmModal(opts);
    }

    componentDidUpdate() {
        if (this.props.statusError !== undefined) {
            if (!this.props.statusError) {
                const optsError = {
                    title: resourceProvider.read('validateLogisticMovements'), acceptAction: () => {
                        this.props.clearStatus();
                    }
                };
                this.props.openErrorModal(optsError);
            }
        }
    }

    render() {
        return (
            <>
                <section className="ep-content">
                    <header className="d-block ep-content__header ep-content__header--h71 p-t-0">
                        <span className="float-r">
                            <PageActions
                                actions={
                                    [
                                        {
                                            title: 'assignCostCenter',
                                            type: constants.RouterActions.Type.Button,
                                            actionType: 'navigate',
                                            route: 'connectionAttributes/assigncostcenter'
                                        }
                                    ]
                                }
                            />
                        </span>
                    </header>
                    <Grid name={this.props.name} columns={this.getColumns()} />
                </section>

            </>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        edit: true,
        delete: true,
        trueClass: 'ep-pill m-r-1 ep-pill--active',
        trueKey: 'active',
        falseClass: 'ep-pill m-r-1 ep-pill--inactive',
        falseKey: 'inActive',
        statusError: state.nodeConnection.nodeCostCenters.status
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        refreshGrid: name => {
            dispatch(refreshGrid(name));
        },
        onEdit: row => {
            dispatch(initNodeCostCenter(dataService.buildInitialValues(row)));
            dispatch(openModal('editCostCenter', constants.Modes.Update));
        },
        openConfirmModal: options => {
            dispatch(openMessageModal(resourceProvider.read('confirmNodeDeleteCostCenter'), options));
        },
        openErrorModal: options => {
            dispatch(openMessageModal(resourceProvider.read('nodeCostCenterhashlogisticMovement'), options));
        },
        deleteNodeCostCenter: row => {
            dispatch(deleteNodeCostCenter(row.nodeCostCenterId));
            dispatch(refreshGrid('nodeCostCenter'));
            dispatch(closeModal());
        },
        clearStatus: () => {
            dispatch(clearStatusNodeCostCenter());
            dispatch(closeModal());
        }
    };
};

const gridConfig = () => {
    return {
        name: 'nodeCostCenter',
        apiUrl: apiService.nodeConnection.getAllNodeCostCenter(),
        idField: 'nodeCostCenterId',
        sortable: {
            defaultSort: 'createdDate desc'
        },
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(NodeCostCenterGrid, gridConfig));
