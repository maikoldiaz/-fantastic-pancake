import React from 'react';
import { connect } from 'react-redux';
import Grid from './../../../../common/components/grid/grid.jsx';
import { gridUtils } from './../../../../common/components/grid/gridUtils';
import { dataGrid } from './../../../../common/components/grid/gridComponent';
import { apiService } from './../../../../common/services/apiService';
import { ActionCell } from './../../../../common/components/grid/gridCells.jsx';
import { openModal, closeModal, showError, openMessageModal } from './../../../../common/actions';
import { deleteHomologationGroup, initHomologationGroup } from './../actions';
import { navigationService } from './../../../../common/services/navigationService';
import { constants } from './../../../../common/services/constants';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { utilities } from '../../../../common/services/utilities';
import { refreshGrid } from '../../../../common/components/grid/actions';

export class HomologationsGrid extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
    }

    getColumns() {
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} onDelete={v => this.onDelete(v)} />;

        const columns = [];

        columns.push(gridUtils.buildTextColumn('homologation.sourceSystem.name', this.props, '', 'sourceSystem'));
        columns.push(gridUtils.buildTextColumn('homologation.destinationSystem.name', this.props, '', 'destinationSystem'));
        columns.push(gridUtils.buildTextColumn('group.name', this.props, '', 'groups'));

        columns.push(gridUtils.buildActionColumn(actions, null, 100));

        return columns;
    }

    onDelete(data) {
        const opts = { canCancel: true, acceptAction: () => this.props.deleteHomologationGroup(data) };
        this.props.openConfirmModal(data, opts);
    }

    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} onNoData={this.props.openCreateHomologationModal} />
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.refreshToggler !== this.props.refreshToggler) {
            this.props.refreshGrid();
            this.props.closeModal();
        }

        if (prevProps.conflictToggler !== this.props.conflictToggler) {
            this.props.showConflict();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        edit: true,
        delete: true,
        refreshToggler: state.homologations.refreshToggler,
        conflictToggler: state.homologations.conflictToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onEdit: homologationGroup => {
            dispatch(initHomologationGroup(constants.Modes.Update));
            navigationService.navigateTo(`manage/${homologationGroup.homologationGroupId}`);
        },
        openConfirmModal: (data, options) => {
            const dataMappingsCount = data.homologationDataMapping.length;
            dispatch(openMessageModal(resourceProvider.readFormat('deleteHomologationGroupConfirmMessage', [dataMappingsCount]), options));
        },
        deleteHomologationGroup: group => {
            dispatch(deleteHomologationGroup(group));
        },
        openCreateHomologationModal: () => {
            dispatch(openModal('createHomologation'));
        },
        refreshGrid: () => {
            dispatch(refreshGrid('homologations'));
        },
        showConflict: () => {
            dispatch(showError(resourceProvider.read('conflictError')));
        },
        closeModal: () => {
            dispatch(closeModal());
        }
    };
};

const homologationsGridConfig = () => {
    return {
        name: 'homologations',
        idField: 'homologationGroupId',
        apiUrl: apiService.homologation.getHomologationGroups(),
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(dataGrid(HomologationsGrid, homologationsGridConfig));
