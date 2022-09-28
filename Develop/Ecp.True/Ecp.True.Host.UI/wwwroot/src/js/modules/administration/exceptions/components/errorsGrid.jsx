import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { ActionCell, DateCell } from '../../../../common/components/grid/gridCells.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../common/services/apiService';
import { utilities } from '../../../../common/services/utilities';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { dateService } from '../../../../common/services/dateService';
import { navigationService } from '../../../../common/services/navigationService';
import { constants } from '../../../../common/services/constants';
import { routerActions } from '../../../../common/router/routerActions';
import { applyPageFilter, refreshGrid, fetchGridData } from '../../../../common/components/grid/actions';
import { openModal, enableDisablePageAction, showError } from '../../../../common/actions';
import { initErrorAddComment, setSelectedData, requestRetryRecord, savePageFilters, resetGridFilter } from '../actions';

class ErrorsGrid extends React.Component {
    constructor() {
        super();

        this.viewErrorDetails = this.viewErrorDetails.bind(this);
        this.onDiscardException = this.onDiscardException.bind(this);
        this.onRetryRecord = this.onRetryRecord.bind(this);
        this.onRetryRecords = this.onRetryRecords.bind(this);
        this.requestGridData = this.requestGridData.bind(this);

        routerActions.configure('discardException', this.onDiscardException);
        routerActions.configure('retryRecord', this.onRetryRecords);

        this.details = false;
    }

    onDiscardException() {
        const count = this.props.selection.length;
        const txt = count > 1 ? 'Plural' : 'Singular';
        this.props.onDiscardException(resourceProvider.read(`addCommentPreText${txt}`), resourceProvider.read(`addCommentPostText${txt}`), count, this.props.selection);
    }

    viewErrorDetails(error) {
        this.details = true;
        navigationService.navigateTo(`manage/${error.messageId}_${error.isRetry === true ? 'F' : 'P'}`);
    }

    onRetryRecords() {
        if (this.props.selection.findIndex(a => a.isRetry === false) >= 0) {
            this.props.showError();
        } else {
            this.props.onRetry(this.props.selection.map(record => record.fileRegistrationTransactionId));
        }
    }

    onRetryRecord(row) {
        this.props.onRetry([row.fileRegistrationTransactionId]);
    }

    getColumns() {
        const columns = [];
        const actionCell = rowProps => (<ActionCell {...this.props} {...rowProps} onInfo={this.viewErrorDetails}
            onRefresh={this.onRetryRecord} refresh={true} enableRefresh={row => row.isRetry === true} refreshTitle={'retryRecord'} />);

        const date = rowProps => <DateCell {...this.props} {...rowProps} dateWithTime={true} />;

        columns.push(gridUtils.buildTextColumn('messageId', null, null, 'message', { type: 'text', defaultValue: this.props.savedFilters.messageId }));
        columns.push(gridUtils.buildTextColumn('systemName', null, null, 'systemType', { type: 'text', defaultValue: this.props.savedFilters.systemName }));
        columns.push(gridUtils.buildSelectColumn('process', null,
            r => r.original.process ? resourceProvider.read(`${utilities.toLowerCase(r.original.process)}Register`) : '', 'process', {
                width: 250,
                defaultValue: this.props.savedFilters.process,
                values:
                    [
                        { label: 'inventoryRegister', value: constants.ProcessType.Inventory },
                        { label: 'movementRegister', value: constants.ProcessType.Movement },
                        { label: 'eventsRegister', value: constants.ProcessType.Events },
                        { label: 'contractRegister', value: constants.ProcessType.Contract }
                    ]
            }));

        columns.push(gridUtils.buildTextColumn('fileName', null, null, 'fileName', { type: 'text', defaultValue: this.props.savedFilters.fileName }));
        columns.push(gridUtils.buildDateColumn('creationDate', null, date, 'createdDate', { defaultValue: this.props.savedFilters.creationDate }));
        columns.push(gridUtils.buildActionColumn(actionCell, null, 120));
        return columns;
    }

    requestGridData(apiUrl) {
        if (!this.props.loadGridEmpty) {
            this.props.fetchGridData(apiUrl, this.props.name);
        }
    }

    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} requestGridData={this.requestGridData} />
        );
    }

    componentDidMount() {
        this.props.enableDisableDiscard(true);
        this.props.enableDisableRetry(true);

        if (this.props.loadGridEmpty) {
            this.props.resetGridFilter();
        }
    }

    componentDidUpdate(prevProps) {
        this.props.enableDisableDiscard(!this.props.selection.length > 0);
        this.props.enableDisableRetry(!this.props.selection.length > 0);

        if (prevProps.reloadGridToggler !== this.props.reloadGridToggler) {
            this.props.applyPageFilters(this.props.savedFilters);
        }

        if (prevProps.retryToggler !== this.props.retryToggler) {
            this.props.refreshGrid();
        }
    }

    componentWillUnmount() {
        this.props.savePageFilters(this.details ? this.props.pageFilters : {});
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        info: true,
        delete: true,
        deleteTitle: 'discardException',
        updateGrid: true,
        selection: state.grid.pendingTransactionErrors ? state.grid.pendingTransactionErrors.selection : [],
        pageFilters: state.grid.pendingTransactionErrors ? state.grid.pendingTransactionErrors.pageFilters : {},
        savedFilters: state.controlexception.controlException.pageFilters,
        retryToggler: state.controlexception.controlException.retryToggler,
        loadGridEmpty: state.controlexception.controlException.loadGridEmpty,
        reloadGridToggler: state.controlexception.controlException.reloadGridToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onDelete: error => {
            dispatch(setSelectedData([error]));
            dispatch(openModal('addComments'));
            dispatch(initErrorAddComment('pendingTransactionErrors',
                resourceProvider.read('addCommentPreTextSingular'), resourceProvider.read('addCommentPostTextSingular'),
                1));
        },
        onDiscardException: (preText, postText, count, errors) => {
            dispatch(setSelectedData(errors));
            dispatch(openModal('addComments'));
            dispatch(initErrorAddComment('pendingTransactionErrors', preText, postText, count));
        },
        enableDisableDiscard: disabled => {
            dispatch(enableDisablePageAction('discardException', disabled));
        },
        enableDisableRetry: disabled => {
            dispatch(enableDisablePageAction('retryRecord', disabled));
        },
        onRetry: pendingTransactionErrors => {
            dispatch(requestRetryRecord(pendingTransactionErrors));
        },
        showError: () => {
            dispatch(showError(resourceProvider.read('retryRecordsFailed')));
        },
        savePageFilters: pageFilters => {
            dispatch(savePageFilters(pageFilters));
        },
        applyPageFilters: pageFilters => {
            dispatch(applyPageFilter('pendingTransactionErrors', pageFilters));
        },
        refreshGrid: () => {
            dispatch(refreshGrid('pendingTransactionErrors', true));
        },
        resetGridFilter: () => {
            dispatch(resetGridFilter());
        },
        fetchGridData: (apiUrl, name) => {
            dispatch(fetchGridData(apiUrl, name));
        }
    };
};

/* istanbul ignore next */
const defaultFilter = props => {
    let errorFilter = '';
    if (props.errorId) {
        errorFilter = ` and uploadId eq '${props.errorId}'`;
    }

    let defaultLastDays = 0;
    defaultLastDays = systemConfigService.getDefaultErrorLastDays();

    const date = dateService.subtract(dateService.now(), defaultLastDays, 'd');
    return `creationDate gt ${date.toISOString()}${errorFilter}`;
};

/* istanbul ignore next */
const gridConfig = props => {
    return {
        name: 'pendingTransactionErrors',
        idField: 'id',
        selectable: true,
        apiUrl: apiService.error.getErrors(),
        filterable: {
            defaultFilter: defaultFilter(props),
            override: false
        },
        sortable: {
            defaultSort: 'creationDate desc'
        },
        section: true
    };
};

/* istanbul ignore next */
export default dataGrid(connect(mapStateToProps, mapDispatchToProps)(ErrorsGrid), gridConfig);
