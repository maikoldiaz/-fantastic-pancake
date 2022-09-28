import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../common/services/apiService';
import { StatusCell, ActionCell } from '../../../../common/components/grid/gridCells.jsx';
import { openModal, getCategoryElements, getOriginTypes } from '../../../../common/actions';
import { initAnnulation } from '../actions';
import { constants } from '../../../../common/services/constants';
import { optionService } from '../../../../common/services/optionService';
import { dataService } from '../dataService';

class AnnulationsGrid extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
    }

    getColumns() {
        const status = rowProps => <StatusCell {...this.props} {...rowProps} />;
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} />;

        const options = {
            width: 150,
            values: optionService.getOriginTypes(this.props.originTypes)
        };

        const columns = [];

        columns.push(gridUtils.buildTextColumn('sourceCategoryElement.name', this.props, '', 'sourceCategoryElement'));
        columns.push(gridUtils.buildTextColumn('annulationCategoryElement.name', this.props, '', 'annulationCategoryElement'));
        columns.push(gridUtils.buildSelectColumn('sourceNodeOriginType.name', this.props, '', 'origin', options));
        columns.push(gridUtils.buildSelectColumn('destinationNodeOriginType.name', this.props, '', 'destination', options));
        columns.push(gridUtils.buildSelectColumn('sourceProductOriginType.name', this.props, '', 'prodOrigen', options));
        columns.push(gridUtils.buildSelectColumn('destinationProductOriginType.name', this.props, '', 'prodDestino', options));
        columns.push(gridUtils.buildSelectColumn('isActive', this.props, status, 'state', {
            values: optionService.getGridStatusTypes()
        }));

        columns.push(gridUtils.buildTextColumn('sapTransactionCode.name', this.props, '', 'sapTransactionCode'));

        columns.push(gridUtils.buildActionColumn(actions, null, 100));

        return columns;
    }

    componentDidMount() {
        this.props.getCategoryElements();
        this.props.getOriginTypes();
    }

    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} onNoData={this.props.openCreateRelationModal} />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        edit: true,
        originTypes: state.shared.originTypes,
        trueClass: 'ep-pill m-r-1 ep-pill--active',
        trueKey: 'active',
        falseClass: 'ep-pill m-r-1 ep-pill--inactive',
        falseKey: 'inActive'
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        openCreateRelationModal: () => {
            dispatch(openModal('createRelation', constants.Modes.Create, 'Annulation'));
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        getOriginTypes: () => {
            dispatch(getOriginTypes());
        },
        onEdit: row => {
            dispatch(initAnnulation(dataService.buildInitialValues(row)));
            dispatch(openModal('createRelation', constants.Modes.Update, 'Annulation'));
        }
    };
};

const annulationsGridConfig = () => {
    return {
        name: 'annulations',
        idField: 'annulationId',
        apiUrl: apiService.annulation.query(),
        sortable: {
            defaultSort: 'createdDate desc'
        },
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(AnnulationsGrid, annulationsGridConfig));
