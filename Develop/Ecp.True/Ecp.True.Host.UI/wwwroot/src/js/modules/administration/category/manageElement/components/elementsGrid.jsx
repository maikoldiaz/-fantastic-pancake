import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../../common/services/apiService';
import { StatusCell, ActionCell, DateCell } from '../../../../../common/components/grid/gridCells.jsx';
import { openModal } from '../../../../../common/actions';
import { constants } from '../../../../../common/services/constants';
import { initializeEdit, initializeValues } from './../actions';
import { optionService } from '../../../../../common/services/optionService';

class ElementsGrid extends React.Component {
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
        columns.push(gridUtils.buildTextColumn('category.name', this.props, { cell: row => row.original.category.name }, 'category'));
        columns.push(gridUtils.buildSelectColumn('isActive', this.props, status, 'state', {
            values: optionService.getGridStatusTypes()
        }));
        columns.push(gridUtils.buildActionColumn(actions, null, 100));

        return columns;
    }
    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} onNoData={this.props.openCreateElementModal} />
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
        onEdit: categoryElement => {
            dispatch(initializeEdit(categoryElement));
            dispatch(initializeValues(categoryElement));
            dispatch(openModal('createElement', constants.Modes.Update));
        },
        openCreateElementModal: () => {
            dispatch(openModal('createElement', constants.Modes.Create));
        }
    };
};

const gridConfig = () => {
    return {
        name: 'elements',
        idField: 'elementId',
        apiUrl: apiService.category.queryElements(),
        section: true
    };
};

const elementsGridWrapper = dataGrid(ElementsGrid, gridConfig);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(elementsGridWrapper);
