import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { ActionCell, CheckBoxCell, StatusCell } from '../../../../../common/components/grid/gridCells.jsx';
import PageActions from '../../../../../common/router/pageActions.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils.js';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../../common/services/apiService';
import { utilities } from '../../../../../common/services/utilities';
import { navigationService } from '../../../../../common/services/navigationService';
import { CategoryElementFilterComponent } from '../../../../../common/components/filters/categoryElementFilter.jsx';
import { connectionAttributesFilterBuilder } from '../connectionAttributesFilterBuilder.js';
import { openFlyout, openMessageModal, closeModal } from '../../../../../common/actions';
import { constants } from '../../../../../common/services/constants.js';
import { optionService } from '../../../../../common/services/optionService';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { deleteConnectionAttributes, clearStatusConnectionAttributes } from '../actions';
import { refreshGrid } from '../../../../../common/components/grid/actions';

export class ConnectionAttributes extends React.Component {
    constructor() {
        super();
        this.getStatus = this.getStatus.bind(this);
        this.getColumns = this.getColumns.bind(this);
        this.buildControlLimit = this.buildControlLimit.bind(this);
    }

    buildControlLimit(controlLimit) {
        return !utilities.isNullOrUndefined(controlLimit)
            ? `${constants.Prefix}${controlLimit.toString()}`
            : '';
    }
    getStatus(row) {
        return row.state;
    }

    onDelete(data) {
        const opts = {
            title: resourceProvider.read('deleteConnectionAttributes'), canCancel: true, acceptAction: () => {
                this.props.deleteConnectionAttributes(data);
            }
        };
        this.props.openConfirmModal(opts);
    }

    componentDidUpdate() {
        if (this.props.statusError !== undefined) {
            if (!this.props.statusError) {
                const optsError = {
                    title: resourceProvider.read('validateConnectionAttributesMovements'), acceptAction: () => {
                        this.props.clearStatus();
                    }
                };
                this.props.openErrorModal(optsError);
            }
        }
    }

    getColumns() {
        const actions = rowProps => (
            <ActionCell {...this.props} {...rowProps} edit={true} delete={true} onDelete={v => this.onDelete(v)} />
        );
        const status = rowProps => <StatusCell {...this.props} {...rowProps} />;
        const checkbox = rowProps => (<span className="float-r"><CheckBoxCell {...this.props} {...rowProps} /></span>);
        const columns = [];

        columns.push(
            gridUtils.buildTextColumn(
                'sourceNode.name',
                this.props,
                row => utilities.toUpperCase(row.original.sourceNode.name),
                'sourceNode'
            )
        );
        columns.push(
            gridUtils.buildTextColumn(
                'destinationNode.name',
                this.props,
                row => utilities.toUpperCase(row.original.destinationNode.name),
                'destinationNode'
            )
        );
        columns.push(
            gridUtils.buildDecimalColumn(
                'controlLimit',
                this.props,
                'controlLimit',
                { prefix: constants.Prefix }
            )
        );
        columns.push(
            gridUtils.buildSelectColumn(
                'isTransfer',
                this.props,
                checkbox,
                'isTransfer',
                {
                    values: [
                        { label: 'transfer', value: true },
                        { label: 'notTransfer', value: false }
                    ]
                }
            )
        );
        columns.push(
            gridUtils.buildTextColumn(
                'algorithm.modelName',
                this.props,
                row =>
                    row.original.algorithmId === null
                        ? ''
                        : utilities.toUpperCase(row.original.algorithm.modelName),
                'analyticalModelTitle'
            )
        );
        columns.push(gridUtils.buildSelectColumn('isActive', this.props, status, 'state', {
            width: 150,
            values: optionService.getGridStatusTypes()
        }));

        columns.push(gridUtils.buildActionColumn(actions, null, 100));

        return columns;
    }

    render() {
        return (<>
            <section className="ep-content">
                <header className="d-block ep-content__header ep-content__header--h71 p-t-0">
                    <span className="float-r">
                        <PageActions
                            actions={
                                [
                                    {
                                        title: 'createConnection',
                                        type: constants.RouterActions.Type.Button,
                                        actionType: 'navigate',
                                        route: 'connectionAttributes/createattributesnodes'
                                    },
                                    {
                                        title: 'massiveConnectionProduct',
                                        iconClass: 'fas fa-project-diagram',
                                        type: constants.RouterActions.Type.Button,
                                        actionType: 'navigate',
                                        route: 'connectionattributes/bulkUpdate'
                                    },
                                    {
                                        title: 'search',
                                        iconClass: 'fas fa-search',
                                        type: constants.RouterActions.Type.Button,
                                        actionType: 'flyout',
                                        key: 'connAttributes'
                                    }
                                ]
                            }
                        />
                    </span>
                </header>

                <Grid name={this.props.name} columns={this.getColumns()} onNoData={this.props.openConnAttributesFilterFlyout} />
                <CategoryElementFilterComponent name={this.props.name} showTrash={true} />
            </section>
        </>
        );
    }
}


/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        trueClass: 'ep-pill m-r-1 ep-pill--active',
        trueKey: 'active',
        falseClass: 'ep-pill m-r-1 ep-pill--inactive',
        falseKey: 'inActive',
        statusError: state.nodeConnection.attributes.statusDelete
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onEdit: data => {
            navigationService.navigateTo(`manage/${data.nodeConnectionId}`);
        },
        openConnAttributesFilterFlyout: () => {
            dispatch(openFlyout('connAttributes'));
        },
        openConfirmModal: options => {
            dispatch(openMessageModal(resourceProvider.read('confirmDeleteConnectionAttributes'), options));
        },
        openErrorModal: options => {
            dispatch(openMessageModal(resourceProvider.read('ConnectionAttributesMovement'), options));
        },
        deleteConnectionAttributes: row => {
            dispatch(deleteConnectionAttributes(row.sourceNodeId, row.destinationNodeId));
            dispatch(closeModal());
            dispatch(refreshGrid('connAttributes'));
        },
        refreshGrid: name => {
            dispatch(refreshGrid(name));
        },
        clearStatus: () => {
            dispatch(clearStatusConnectionAttributes());
            dispatch(closeModal());
        }
    };
};

const gridConfig = () => {
    return {
        name: 'connAttributes',
        apiUrl: apiService.nodeConnection.query(),
        idField: 'nodeConnectionId',
        filterbale: {
            filterBuilder: connectionAttributesFilterBuilder.build
        },
        sortable: {
            defaultSort: 'nodeConnectionId desc'
        },
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(ConnectionAttributes, gridConfig));
