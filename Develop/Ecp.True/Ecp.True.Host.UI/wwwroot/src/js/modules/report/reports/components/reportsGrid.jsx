import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../common/services/apiService';
import { ActionCell, UploadStatusCell, DateCell } from '../../../../common/components/grid/gridCells.jsx';
import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';
import { optionService } from '../../../../common/services/optionService';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { openMessageModal } from '../../../../common/actions';
import { navigationService } from '../../../../common/services/navigationService';
import { scenarioService } from '../../../../common/services/scenarioService.js';

export class ReportsGrid extends React.Component {
    constructor() {
        super();

        this.getColumns = this.getColumns.bind(this);
        this.onDownload = this.onDownload.bind(this);
    }

    onDownload(row) {
        if (utilities.equalsIgnoreCase(row.reportType, constants.ReportTypeName.BeforeCutOff)) {
            navigationService.navigateToModule(`cutoffReport/manage/view?ex=${row.executionId}`);
        }

        if (utilities.equalsIgnoreCase(row.reportType, constants.ReportTypeName.SapBalance)) {
            navigationService.navigateToModule(`senttosapreport/manage/${row.executionId}`);
        }

        if (utilities.equalsIgnoreCase(row.reportType, constants.ReportTypeName.OperativeBalance)) {
            navigationService.navigateToModule(`operativebalancereport/manage/view?ex=${row.executionId}`);
        }

        if (utilities.isEqual(row.reportType, constants.ReportTypeName.OfficialInitialBalance)) {
            navigationService.navigateToModule(`officialbalanceloaded/manage/${row.executionId}`);
        }

        if (utilities.isEqual(row.reportType, constants.ReportTypeName.UserRolesAndPermissions)) {
            navigationService.navigateToModule(`userrolesandpermissions/manage/${row.executionId}`);
        }
    }

    getSegmentName(row) {
        if (row.original.categoryId !== 2) {
            return '';
        }

        return row.original.elementName;
    }

    getSystemName(row) {
        if (row.original.categoryId !== 8) {
            return '';
        }

        return row.original.elementName;
    }

    getColumns() {
        const columns = [];
        const actionCell = rowProps => <ActionCell {...this.props} {...rowProps} onDownload={this.onDownload} />;
        const reportStatus = gridProps => <UploadStatusCell {...this.props} {...gridProps} noLocalize />;
        const dateCell = rowProps => <DateCell {...this.props} {...rowProps} ignoreMax={true} />;
        const dateWithTime = rowProps => <DateCell {...this.props} {...rowProps} dateWithTime={true} />;

        columns.push(gridUtils.buildTextColumn('reportName', this.props));
        columns.push(gridUtils.buildTextColumn('segment', this.props));
        columns.push(gridUtils.buildTextColumn('system', this.props));
        columns.push(gridUtils.buildTextColumn('nodeName', this.props));
        columns.push(gridUtils.buildDateColumn('startDate', this.props, dateCell, 'initialDate', { width: 150 }));
        columns.push(gridUtils.buildDateColumn('endDate', this.props, dateCell, 'finalDate', { width: 150 }));
        columns.push(gridUtils.buildDateColumn('createdDate', this.props, dateWithTime, 'executionDateOC', { width: 150 }));
        columns.push(gridUtils.buildSelectColumn('statusType', this.props, reportStatus, 'fileUploadStatus', { width: 150, values: optionService.getExecutionStatusTypes() }));

        columns.push(gridUtils.buildActionColumn(actionCell, '', 100));

        return columns;
    }

    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        reports: state.grid.reportExecutions ? state.grid.reportExecutions.items : [],
        download: true,
        view: true,
        viewTitle: 'viewError'
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        enableView: row => {
            return row.statusType === constants.StatusType.FAILED;
        },
        enableDownload: row => {
            return row.statusType === constants.StatusType.FINALIZED;
        },
        getStatus: row => {
            return row.statusType;
        },
        onView: () => {
            const options = { canCancel: false, title: resourceProvider.read('error') };
            dispatch(openMessageModal(resourceProvider.read('reportsError'), options));
        }
    };
};

const defaultFilter = props => {
    const reportTypes = scenarioService.getReportTypes();


    const reportFilterAll = !utilities.isNullOrUndefined(props.exclusionReportTypes)
        ? reportTypes.filter(reportType => !props.exclusionReportTypes.some(report => report === reportType))
        : reportTypes;

    const typeNames = reportFilterAll.map(r => `'${r}'`).join(', ');
    const reportTypesFilter = `reportType in (${typeNames})`;

    let generalReportFilter = '';
    const reportFilter = !utilities.isNullOrUndefined(props.generalReportTypes)
        ? reportFilterAll.filter(reportType => props.generalReportTypes.some(report => report === reportType))
        : [];
    if (reportFilter.length > 0) {
        generalReportFilter = `reportType in (${reportFilter.map(r => `'${r}'`).join(', ')}) or `;
    }

    return `(${generalReportFilter}scenarioType eq '${props.scenarioType}' and ${reportTypesFilter})`;
};

/* istanbul ignore next */
const gridConfig = props => {
    return {
        name: 'reportExecutions',
        apiUrl: apiService.reports.query(),
        idField: 'executionId',
        filterable: {
            defaultFilter: defaultFilter(props)
        },
        sortable: {
            defaultSort: 'createdDate desc'
        },
        refreshable: {
            interval: constants.Timeouts.Reports
        },
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(dataGrid(ReportsGrid, gridConfig));
