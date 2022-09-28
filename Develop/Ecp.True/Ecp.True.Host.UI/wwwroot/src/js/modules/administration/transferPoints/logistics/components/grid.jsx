import React from 'react';
import { connect } from 'react-redux';
import { apiService } from '../../../../../common/services/apiService';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { gridUtils } from '../../../../../common/components/grid/gridUtils';
import { ActionCell } from '../../../../../common/components/grid/gridCells.jsx';
import { initTransferPointRow } from '../actions';
import { getCategories, getCategoryElements, openModal } from '../../../../../common/actions';
import { constants } from '../../../../../common/services/constants';

class TransferPointsLogisticGrid extends React.Component {
    getColumns() {
        const actionCell = rowProps => (<ActionCell {...this.props} {...rowProps} />);
        const columns = [];

        columns.push(gridUtils.buildTextColumn('transferPoint', this.props));
        columns.push(gridUtils.buildTextColumn('logisticSourceCenter', this.props));
        columns.push(gridUtils.buildTextColumn('sourceProduct', this.props, null, 'sourceProductTransferOperational'));
        columns.push(gridUtils.buildTextColumn('logisticDestinationCenter', this.props));
        columns.push(gridUtils.buildTextColumn('destinationProduct', this.props, null, 'destinationProductTransferPoints'));
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
        delete: true,
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
            dispatch(initTransferPointRow(row));
            dispatch(openModal('delete'));
        },
        createTransferPoint: () => {
            dispatch(openModal('create'));
        }
    };
};

/* istanbul ignore next */
const gridConfig = () => {
    return {
        name: 'transferPointsLogistics',
        apiUrl: apiService.nodeRelationship.query(constants.NodeRelationship.Logistics),
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(TransferPointsLogisticGrid, gridConfig));
