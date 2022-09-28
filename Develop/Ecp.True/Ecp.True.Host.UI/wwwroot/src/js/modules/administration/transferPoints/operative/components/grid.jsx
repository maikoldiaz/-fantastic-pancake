import React from 'react';
import { connect } from 'react-redux';
import { apiService } from '../../../../../common/services/apiService';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { gridUtils } from '../../../../../common/components/grid/gridUtils';
import { ActionCell } from '../../../../../common/components/grid/gridCells.jsx';
import { initTransferPointRow } from '../actions';
import { openModal, getCategories, getCategoryElements } from '../../../../../common/actions';
import { constants } from '../../../../../common/services/constants';

class TransferPointsOperationalGrid extends React.Component {
    getColumns() {
        const actionCell = rowProps => (<ActionCell {...this.props} {...rowProps} enableEdit={true} />);
        const columns = [];

        columns.push(gridUtils.buildTextColumn('transferPoint', this.props));
        columns.push(gridUtils.buildTextColumn('movementType', this.props, null, 'movementTypeTransferOperational'));
        columns.push(gridUtils.buildTextColumn('sourceNode', this.props, null, 'sourceNodeTransferOperational'));
        columns.push(gridUtils.buildTextColumn('sourceNodeType', this.props));
        columns.push(gridUtils.buildTextColumn('destinationNode', this.props, null, 'destinationNodeTransferOperational'));
        columns.push(gridUtils.buildTextColumn('destinationNodeType', this.props));
        columns.push(gridUtils.buildTextColumn('sourceProduct', this.props, null, 'sourceProductTransferOperational'));
        columns.push(gridUtils.buildTextColumn('sourceProductType', this.props));
        columns.push(gridUtils.buildActionColumn(actionCell, '', 100));
        return columns;
    }

    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} onNoData={this.props.createTransferPoint} />
        );
    }

    componentDidMount() {
        this.props.getCategories();
        this.props.getCategoryElements();
    }
}

/* istanbul ignore next */
const mapStateToProps = () => {
    return {
        edit: true,
        delete: true,
        editTitle: 'updateTransferPoint',
        deleteTitle: 'deleteTransferPoint'
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getCategories: () => {
            dispatch(getCategories());
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        getStatus: row => {
            return row.state;
        },
        onDelete: row => {
            dispatch(initTransferPointRow(row, false));
            dispatch(openModal('deleteTransferPointOperational', constants.Modes.Delete));
        },
        onEdit: row => {
            dispatch(initTransferPointRow(row, true));
            dispatch(openModal('updateTransferPointOperational', constants.Modes.Update));
        },
        createTransferPoint: () => {
            dispatch(openModal('createTransferPointOperational', constants.Modes.Create));
        }
    };
};

/* istanbul ignore next */
const gridConfig = () => {
    return {
        name: 'transferPointsOperational',
        apiUrl: apiService.nodeRelationship.query(constants.NodeRelationship.Operative),
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(TransferPointsOperationalGrid, gridConfig));
