import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { ActionCell } from '../../../../common/components/grid/gridCells.jsx';
import { receiveGridData, removeGridData } from '../../../../common/components/grid/actions';
import { openModal } from '../../../../common/actions.js';
import { constants } from '../../../../common/services/constants';
import { initHomologationGroupData } from './../actions';
import { utilities } from '../../../../common/services/utilities.js';

export class HomologationsDataGrid extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
    }

    getColumns() {
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} />;

        const columns = [];

        if (this.isSourceSystemTrue()) {
            columns.push(gridUtils.buildTextColumn('value', this.props, null, 'originData'));
            columns.push(gridUtils.buildTextColumn('destinationValue', this.props, null, 'destinationData'));
        } else {
            columns.push(gridUtils.buildTextColumn('sourceValue', this.props, null, 'originData'));
            columns.push(gridUtils.buildTextColumn('value', this.props, null, 'destinationData'));
        }
        columns.push(gridUtils.buildActionColumn(actions, null, 100));

        return columns;
    }

    isSourceSystemTrue() {
        const homologationGroup = this.props.homologationGroup;
        return homologationGroup.sourceSystem.systemTypeId === constants.SystemType.TRUE;
    }

    render() {
        return (
            <Grid className="ep-table--brless" wrapperClassName="ep-table-wrap--hn150"
                name={this.props.name} columns={this.getColumns()} onNoData={this.props.openAddHomologationDataModal} />
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.dataMappingsToggler !== this.props.dataMappingsToggler) {
            this.props.loadDataMappings(this.props.homologationGroupDataMappings);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        edit: true,
        delete: true,
        mode: state.homologations.mode,
        dataMappingsToggler: state.homologations.dataMappingsToggler,
        homologationGroup: state.homologations.homologationGroup,
        homologationGroupDataMappings: state.homologations.homologationGroup ? state.homologations.homologationGroup.homologationDataMappings : []
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        loadDataMappings: data => {
            dispatch(receiveGridData(data, 'homologationGroupData'));
        },
        onDelete: data => {
            dispatch(removeGridData('homologationGroupData', [data]));
        },
        onEdit: data => {
            dispatch(openModal('addHomologationData', constants.Modes.Update));
            dispatch(initHomologationGroupData(data));
        },
        openAddHomologationDataModal: () => {
            dispatch(openModal('addHomologationData', constants.Modes.Create));
        }
    };
};

const homologationsGridConfig = () => {
    return {
        name: 'homologationGroupData',
        idField: 'tempId',
        odata: false
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(dataGrid(HomologationsDataGrid, homologationsGridConfig));
