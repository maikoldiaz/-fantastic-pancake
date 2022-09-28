import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../../common/services/apiService';
import { StatusCell, ActionCell, DateCell } from '../../../../../common/components/grid/gridCells.jsx';
import { openModal } from '../../../../../common/actions';
import { initializeEdit } from './../actions';
import { constants } from '../../../../../common/services/constants';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { optionService } from '../../../../../common/services/optionService';

class CategoriesGrid extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
    }

    getColumns() {
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} />;
        const status = rowProps => <StatusCell {...this.props} {...rowProps} />;
        const date = rowProps => <DateCell {...this.props} {...rowProps} />;

        const columns = [];

        columns.push(gridUtils.buildTextColumn('name', this.props));
        columns.push(gridUtils.buildTextColumn('description', this.props));

        columns.push(gridUtils.buildDateColumn('createdDate', this.props, date));

        columns.push(gridUtils.buildSelectColumn('isActive', this.props, status, 'state', {
            values: optionService.getGridStatusTypes()
        }));
        columns.push(gridUtils.buildSelectColumn('isGrouper', this.props, row => resourceProvider.read(row.original.isGrouper ? 'yes' : 'no'), 'isGrouper', {
            values: [{ label: 'yes', value: true }, { label: 'no', value: false }]
        }));

        columns.push(gridUtils.buildActionColumn(actions, null, 100));

        return columns;
    }

    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} onNoData={this.props.openCreateCategoryModal} />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = () => {
    return {
        edit: true,
        trueClass: 'ep-pill m-r-1 ep-pill--active',
        trueKey: 'active',
        falseClass: 'ep-pill m-r-1 ep-pill--inactive',
        falseKey: 'inActive'
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onEdit: category => {
            dispatch(initializeEdit(category));
            dispatch(openModal('createCategory', constants.Modes.Update));
        },
        openCreateCategoryModal: () => {
            dispatch(openModal('createCategory', constants.Modes.Create));
        }
    };
};

const categoryGridConfig = () => {
    return {
        name: 'categories',
        idField: 'categoryId',
        apiUrl: apiService.category.query(),
        dataLink: false,
        filterable: {
            defaultFilter: 'isHomologation eq false'
        },
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(CategoriesGrid, categoryGridConfig));
