import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../common/services/apiService';
import { ActionCell } from '../../../../common/components/grid/gridCells.jsx';
import { deleteTransformation, initializeEdit } from '../actions';
import { openModal } from '../../../../common/actions';
import { constants } from '../../../../common/services/constants';
import { refreshGrid } from '../../../../common/components/grid/actions';

class TransformationGrid extends React.Component {
    constructor() {
        super();

        this.buildOriginColumns = this.buildOriginColumns.bind(this);
        this.buildDestinationColumns = this.buildDestinationColumns.bind(this);
        this.isMovements = this.isMovements.bind(this);
        this.getColumns = this.getColumns.bind(this);
    }

    isMovements() {
        return this.props.name === 'movements';
    }

    buildOriginColumns() {
        const originColumns = [gridUtils.buildTextColumn('originSourceNode.name', this.props, null, 'transformationSourceNode')];
        if (this.isMovements()) {
            originColumns.push(gridUtils.buildTextColumn('originDestinationNode.name', this.props, null, 'transformationDestinationNode'));
            originColumns.push(gridUtils.buildTextColumn('originSourceProduct.name', this.props, null, 'transformationSourceProduct'));
            originColumns.push(gridUtils.buildTextColumn('originDestinationProduct.name', this.props, null, 'transformationDestinationProduct'));
        } else {
            originColumns.push(gridUtils.buildTextColumn('originSourceProduct.name', this.props, null, 'transformationProduct'));
            originColumns.push(gridUtils.buildTextColumn('originMeasurement.name', this.props, null, 'transformationMeasurement'));
        }

        return originColumns;
    }

    buildDestinationColumns() {
        const destinationColumns = [gridUtils.buildTextColumn('destinationSourceNode.name', this.props, null, 'transformationSourceNode')];
        if (this.isMovements()) {
            destinationColumns.push(gridUtils.buildTextColumn('destinationDestinationNode.name', this.props, null, 'transformationDestinationNode'));
            destinationColumns.push(gridUtils.buildTextColumn('destinationSourceProduct.name', this.props, null, 'transformationSourceProduct'));
            destinationColumns.push(gridUtils.buildTextColumn('destinationDestinationProduct.name', this.props, null, 'transformationDestinationProduct'));
        } else {
            destinationColumns.push(gridUtils.buildTextColumn('destinationSourceProduct.name', this.props, null, 'transformationProduct'));
            destinationColumns.push(gridUtils.buildTextColumn('destinationMeasurement.name', this.props, null, 'transformationMeasurement'));
        }

        return destinationColumns;
    }

    getColumns() {
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} />;

        const columns = [
            gridUtils.buildHeaderColumn('origin', ...this.buildOriginColumns()),
            gridUtils.buildHeaderColumn('destination', ...this.buildDestinationColumns()),
            gridUtils.buildActionColumn(actions, null, 100)
        ];

        return columns;
    }

    render() {
        return (
            <Grid className="ep-table--headergroups" name={this.props.name} columns={this.getColumns()} />
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.deleteToggler !== this.props.deleteToggler) {
            this.props.refreshGrid(this.props.name);
        }

        if (prevProps.editToggler !== this.props.editToggler) {
            this.props.openModal(this.isMovements());
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        edit: true,
        delete: true,
        deleteToggler: state.transformSettings.deleteToggler,
        editToggler: state.transformSettings.editToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        onEdit: transformation => {
            dispatch(initializeEdit(transformation));
        },
        onDelete: transformation => {
            dispatch(deleteTransformation(transformation));
        },
        refreshGrid: name => {
            dispatch(refreshGrid(name));
        },
        openModal: () => {
            dispatch(openModal('transformation', constants.Modes.Update, `${ownProps.name}transformation`));
        }
    };
};

/* istanbul ignore next */
const gridConfig = props => {
    return {
        name: props.name,
        idField: 'transformationId',
        apiUrl: apiService.transformation.query(),
        filterable: {
            defaultFilter: `messageTypeId eq ${props.name === 'movements' ? 1 : 4} and IsDeleted eq false`,
            override: false
        },
        section: true
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(TransformationGrid, gridConfig));
