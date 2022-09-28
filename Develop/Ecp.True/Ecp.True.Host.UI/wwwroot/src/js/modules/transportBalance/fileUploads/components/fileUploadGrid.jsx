import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../common/services/apiService';
import { ActionCell, MessageCountCell, UploadStatusCell, DateCell } from '../../../../common/components/grid/gridCells.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { utilities } from '../../../../common/services/utilities';
import { openModal, openFlyout, setModuleName, requestFileReadAccessInfo, saveUploadFileFilter, getSystemTypes } from '../../../../common/actions';
import { onFileRegistrationReinject, requestFileRegistrationGridDetails, updateGridFilter, clearGridFilter, initializeFileUploadFilter } from '../actions';
import blobService from '../../../../common/services/blobService';
import FileUploadFilter from './fileUploadGridFilter.jsx';
import { fileUploadFilterBuilder } from '../fileUploadFilterBuilder';
import { constants } from '../../../../common/services/constants';
import { optionService } from '../../../../common/services/optionService';
import { dateService } from '../../../../common/services/dateService';
import { systemConfigService } from './../../../../common/services/systemConfigService';
import { navigationService } from '../../../../common/services/navigationService';
import { change } from 'redux-form';
import { applyFilter } from '../../../../common/components/grid/actions';

export class FileUploadGrid extends React.Component {
    constructor() {
        super();

        this.getColumns = this.getColumns.bind(this);
        this.onReInject = this.onReInject.bind(this);
        this.onDownload = this.onDownload.bind(this);
        this.isExcel = this.isExcel.bind(this);
        this.shouldRefresh = this.shouldRefresh.bind(this);
        this.onFiltered = this.onFiltered.bind(this);
        this.setDefaultFilter = this.setDefaultFilter.bind(this);
        this.viewErrorDetails = this.viewErrorDetails.bind(this);
    }

    isExcel() {
        return this.props.systemType === constants.SystemType.EXCEL;
    }

    isContractOrEvent() {
        return (this.props.systemType === constants.SystemType.CONTRACT || this.props.systemType === constants.SystemType.EVENTS);
    }

    onReInject(row) {
        this.props.openModal();
        this.props.onReInject(row);
    }

    onDownload(row) {
        if (!utilities.isNullOrUndefined(this.props.readAccessInfo)) {
            blobService.initialize(this.props.readAccessInfo.accountName);
            const fileName = utilities.toLowerCase(row.systemTypeId) + '/' + row.uploadId;
            const downLoadLink = blobService.getDownloadLink(this.props.readAccessInfo.sasToken, this.props.readAccessInfo.containerName, fileName);
            const link = document.createElement('a');
            if (utilities.isIE()) {
                window.open(downLoadLink);
            } else if (link.download === '') {
                link.setAttribute('href', downLoadLink);
                link.setAttribute('download', row.name);
                link.style.visibility = 'hidden';

                document.body.appendChild(link);
                link.click();

                document.body.removeChild(link);
            }
        }
    }

    isFinalized(row) {
        return row.status !== 'Procesando';
    }

    isEnabled(row) {
        return row.errorCount > 0;
    }

    shouldRefresh() {
        return !utilities.isNullOrUndefined(this.props.files.find(x => utilities.equalsIgnoreCase(x.status, constants.StatusType.PROCESSING)));
    }

    getProcessedRecord(row) {
        const fileRegistrationTransactions = row.original.fileRegistrationTransactions;
        const processedRecord = fileRegistrationTransactions && fileRegistrationTransactions.filter(x => x.statusTypeId === constants.StatusType.PROCESSED
            || x.statusTypeId === constants.StatusType.FAILED);
        return processedRecord && Array.isArray(processedRecord) && processedRecord.length > 0 ? processedRecord.length : '';
    }

    viewErrorDetails(error) {
        navigationService.navigateToModule(`exceptions/manage/${error.uploadId}`);
    }

    getColumns() {
        const columns = [];

        const actionCell = rowProps => (<ActionCell {...this.props} {...rowProps}
            enableView={row => row.errorCount > 0} enableRefresh={row => row.errorCount > 0}
            download={row => row.isParsed === true}
            view={row => row.status !== 'Procesando'} onView={this.viewErrorDetails}
            refresh={row =>
                this.isExcel() ? row.status !== 'Procesando' : false
            }
            onRefresh={this.onReInject} onDownload={this.onDownload} />);
        const messageCount = gridProps => <MessageCountCell {...this.props} {...gridProps} getErrorCount={row => row.errorCount} />;
        const uploadStatus = gridProps =>
            <UploadStatusCell {...this.props} {...gridProps} getStatus={row => row.status} getErrorCount={row => row.errorCount} noLocalize={true} />;
        const startDate = rowProps => <DateCell {...this.props} {...rowProps} ignoreMax={true} />;
        const fileTypeCell = rowProps => {
            if (utilities.isNullOrUndefined(this.props.fileTypes) || this.props.fileTypes.length === 0) {
                return null;
            }
            const currentFileType = this.props.fileTypes.find(f => f.systemTypeId === constants.SystemType[rowProps.value]);
            if (utilities.isNullOrUndefined(currentFileType)) {
                return null;
            }
            return <span>{currentFileType.name}</span>;
        };

        if (!this.isContractOrEvent()) {
            columns.push(gridUtils.buildTextColumn('uploadId', this.props, null, 'uploadFileId', { type: 'text' }));
        }
        const dateColumnName = this.isContractOrEvent() ? 'startDate' : 'createdDate';
        columns.push(gridUtils.buildDateColumn('createdDate', this.props, startDate, dateColumnName, { width: 130, onFiltered: this.onFiltered }));

        if (this.isExcel()) {
            columns.push(gridUtils.buildTextColumn('segmentName', this.props, null, 'segment', { width: 120 }));
        }

        columns.push(gridUtils.buildTextColumn('name', this.props, null, 'fileName'));
        columns.push(gridUtils.buildSelectColumn('actionType', this.props, row => row.original.actionType, 'actionType', {
            width: 120,
            values: this.isExcel() ? optionService.getExcelActionTypes() : optionService.getActionTypes()
        }));
        columns.push(gridUtils.buildTextColumn('createdBy', this.props, null, 'createdBy', { type: 'text', width: 100 }));
        columns.push(gridUtils.buildSelectColumn('status', this.props, uploadStatus, 'fileUploadStatus', {
            width: 120,
            values: optionService.getCutoffStateTypes()
        }));

        if (this.isContractOrEvent()) {
            columns.push(gridUtils.buildSelectColumn('systemTypeId', this.props, fileTypeCell, 'type', {
                width: 300,
                values: optionService.getFileTypes()
            }));
        }

        columns.push(gridUtils.buildTextColumn('recordsProcessed', this.props, null, 'recordsProcessed', { type: 'number', width: 120 }));
        columns.push(gridUtils.buildTextColumn('errorCount', this.props, messageCount, this.isContractOrEvent() ? '' : 'fileRegistrationErrors', { sortable: false, filterable: false, width: 90 }));
        columns.push(gridUtils.buildActionColumn(actionCell, '', 150));

        return columns;
    }

    onFiltered(value) {
        const dateFilter = {};
        if (!utilities.isNullOrUndefined(value)) {
            const filterDate = dateService.formatFromDate(value);
            dateFilter.initialDate = `${filterDate}`;
            dateFilter.finalDate = `${filterDate}`;
            this.props.updateGridFilter();
        } else {
            const days = this.props.systemType === constants.SystemType.CONTRACT ? systemConfigService.getDefaultTransportFileUploadLastDays() - 1 : 1;
            dateFilter.initialDate = dateService.format(dateService.parseToFilterString(-1 * days));
            dateFilter.finalDate = dateService.format(dateService.parseToFilterString());
            this.props.clearGridFilter();
        }
        this.props.change('fileUploadGridFilter', 'initialDate', dateFilter.initialDate);
        this.props.change('fileUploadGridFilter', 'finalDate', dateFilter.finalDate);
        this.props.applyFilter(dateFilter);
    }

    setDefaultFilter() {
        const days = this.props.systemType === constants.SystemType.CONTRACT ? systemConfigService.getDefaultTransportFileUploadLastDays() - 1 : 1;
        const initialValues = {
            initialDate: dateService.format(dateService.parseToFilterString(-1 * days)),
            finalDate: dateService.format(dateService.parseToFilterString()),
            username: null,
            state: null,
            action: null
        };
        this.props.initialize(initialValues);
    }

    render() {
        return (
            <>
                <Grid name={this.props.name} columns={this.getColumns()} shouldRefresh={this.shouldRefresh} onNoData={this.props.openModal} />
                <FileUploadFilter name="fileUploadFilter" systemType={this.props.systemType} />
            </>
        );
    }

    componentDidMount() {
        this.props.requestFileRegistrationReadAccess();
        this.props.getSystemTypes();
        this.setDefaultFilter();
        if (!this.isExcel()) {
            this.props.setModuleName('transportContracts');
        }
    }

    componentWillUnmount() {
        this.props.setModuleName(null);
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        download: true,
        viewTitle: 'viewError',
        refreshTitle: resourceProvider.read('reInject'),
        files: state.grid.fileUploads ? state.grid.fileUploads.items : [],
        readAccessInfo: state.shared.readAccessInfo,
        fileTypes: state.shared.fileTypes
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        requestFileRegistrationReadAccess: () => {
            dispatch(requestFileReadAccessInfo());
        },
        openModal: () => {
            dispatch(openModal('uploadFileModal'));
        },
        onReInject: fileInfo => {
            dispatch(onFileRegistrationReinject(fileInfo));
        },
        openFileUploadFilterFlyout: () => {
            dispatch(openFlyout('fileUploadFilter'));
        },
        requestFileRegistrationGridDetails: (apiUrl, name) => {
            dispatch(requestFileRegistrationGridDetails(apiUrl, name));
        },
        setModuleName: moduleName => {
            dispatch(setModuleName(moduleName));
        },
        applyFilter: filterValues => {
            dispatch(saveUploadFileFilter('fileUploads', filterValues));
            dispatch(applyFilter('fileUploads', filterValues));
        },
        updateGridFilter: () => {
            dispatch(updateGridFilter());
        },
        clearGridFilter: () => {
            dispatch(clearGridFilter());
        },
        change: (form, field, value) => {
            dispatch(change(form, field, value));
        },
        getSystemTypes: () => {
            dispatch(getSystemTypes());
        },
        initialize: initialValues => {
            dispatch(initializeFileUploadFilter(initialValues));
        }
    };
};

/* istanbul ignore next */
const defaultFilter = props => {
    let fileRegistrationTypeFilter = '';
    if (props.systemType === constants.SystemType.CONTRACT) {
        fileRegistrationTypeFilter =
            `(SystemTypeId eq 'CONTRACT' or SystemTypeId eq 'EVENTS')`;
    } else {
        fileRegistrationTypeFilter = `SystemTypeId eq 'EXCEL'`;
    }

    return fileRegistrationTypeFilter;
};

/* istanbul ignore next */
const defaultDateFilter = props => {
    const days = props.systemType === constants.SystemType.CONTRACT ? systemConfigService.getDefaultTransportFileUploadLastDays() - 1 : 1;

    return {
        initialDate: dateService.format(dateService.parseToFilterString(-1 * days)),
        finalDate: dateService.format(dateService.parseToFilterString())
    };
};

/* istanbul ignore next */
const gridConfig = props => {
    return {
        name: 'fileUploads',
        apiUrl: apiService.fileUpload.query(),
        idField: 'uploadId',
        filterable: {
            defaultFilter: defaultFilter(props),
            filterBuilder: fileUploadFilterBuilder.build
        },
        sortable: {
            defaultSort: 'createdDate desc'
        },
        refreshable: {
            interval: constants.Timeouts.FileUploads
        },
        defaultFilter: defaultDateFilter(props),
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(dataGrid(FileUploadGrid, gridConfig));
