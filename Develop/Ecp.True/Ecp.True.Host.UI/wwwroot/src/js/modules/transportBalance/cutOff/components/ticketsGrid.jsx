import React from 'react';
import { connect } from 'react-redux';
import Grid from './../../../../common/components/grid/grid.jsx';
import { gridUtils } from './../../../../common/components/grid/gridUtils';
import { dataGrid } from './../../../../common/components/grid/gridComponent';
import { apiService } from './../../../../common/services/apiService';
import { navigationService } from './../../../../common/services/navigationService';
import { ActionCell, DateCell, UploadStatusCell } from './../../../../common/components/grid/gridCells.jsx';
import { utilities } from '../../../../common/services/utilities';
import { openModal, requestFileReadAccessInfo, hidePageActions } from '../../../../common/actions';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dateService } from '../../../../common/services/dateService';
import { initializeTicketError } from '../../cutOff/actions';
import { saveCutOffReportFilter } from '../../../report/cutOff/actions';
import { constants } from '../../../../common/services/constants';
import blobService from '../../../../common/services/blobService';
import { optionService } from '../../../../common/services/optionService';
import { ticketService } from '../services/ticketService';

class TicketsGrid extends React.Component {
    constructor() {
        super();

        this.viewSummary = this.viewSummary.bind(this);
        this.navigateToPBIReport = this.navigateToPBIReport.bind(this);
        this.onDownload = this.onDownload.bind(this);
        this.getStatus = this.getStatus.bind(this);
        this.hasNoLocalize = this.hasNoLocalize.bind(this);
        this.shouldRefresh = this.shouldRefresh.bind(this);
    }

    viewSummary(ticket) {
        if (ticket.errorMessage) {
            const gridDetails = ticketService.getGridInfo(this.props);
            this.props.showError(ticket, gridDetails.title, gridDetails.popupName, 'ep-modal--sm', 'text-unset');
        } else if (utilities.matchesAny(ticket.state, [
            constants.StatusType.PROCESSED, constants.StatusType.FAILED, constants.StatusType.FINALIZED,
            constants.StatusType.DELTA, constants.StatusType.SENT, constants.StatusType.ERROR
        ])) {
            if (this.props.componentType === constants.TicketType.Ownership) {
                this.props.hidePageActions(['ownershipNodeActionBar']);
                navigationService.navigateToModule(`ownershipnodes/manage/${ticket.ticketId}_${constants.DetailsPageType.Grid}`, { ticketId: ticket.ticketId });
            } else if (this.props.componentType === constants.TicketType.Cutoff) {
                navigationService.navigateTo(`manage/${ticket.ticketId}`);
            } else if (this.props.componentType === constants.TicketType.Delta) {
                this.props.showError(ticket, resourceProvider.read('operationalDeltasBusinessErrorsTitle'),
                    resourceProvider.read('operationalDeltaBusinessErrorPopUp'), 'ep-modal--lg');
            } else if (this.props.componentType === constants.TicketType.OfficialDelta) {
                navigationService.navigateToModule(`officialdeltapernode/manage/${ticket.ticketId}`);
            } else if (this.props.componentType === constants.TicketType.LogisticMovements) {
                navigationService.navigateToModule(`senttosap/manage/${ticket.ticketId}`);
            }
        }
    }

    getStatus(row) {
        if (this.props.componentType === constants.TicketType.Ownership && row.state === constants.StatusType.CONCILIATIONFAILED) {
            return 'CONCILIATIONFAILED';
        }
        return row.state;
    }

    hasNoLocalize(row) {
        if (this.props.componentType === constants.TicketType.Ownership) {
            return false;
        }
        return row.state !== constants.StatusType.VISUALIZATION;
    }

    isLogistics() {
        return this.props.componentType === constants.TicketType.Logistics || this.props.componentType === constants.TicketType.OfficialLogistics;
    }

    isLogisticMovements() {
        return this.props.componentType === constants.TicketType.LogisticMovements;
    }

    shouldRefresh() {
        return !utilities.isNullOrUndefined(this.props.tickets.find(x => utilities.equalsIgnoreCase(x.state, constants.StatusType.PROCESSING) ||
            utilities.equalsIgnoreCase(x.state, constants.OwnershipNodeStatusType.SENT)));
    }

    navigateToPBIReport(row) {
        const data = {
            categoryName: row.categoryName,
            elementName: row.segment,
            nodeName: 'Todos',
            initialDate: dateService.parse(row.ticketStartDate),
            finalDate: dateService.parse(row.ticketFinalDate),
            reportType: this.props.componentType === constants.TicketType.Ownership ? constants.Report.BalanceOperativeWithPropertyReport : constants.Report.WithoutOwner
        };

        this.props.saveCutOffReportFilter(data);
        navigationService.navigateToModule(`cutoffreport/manage/view`);
    }

    onDownload(row) {
        if (!utilities.isNullOrUndefined(this.props.readAccessInfo)) {
            blobService.initialize(this.props.readAccessInfo.accountName);
            const fileName = 'logistics/' + row.blobPath;
            blobService.downloadFile(this.props.readAccessInfo.sasToken, this.props.readAccessInfo.containerName, fileName, row.segment);
        }
    }

    getSelectValues() {
        if (this.props.componentType === constants.TicketType.Cutoff) {
            return optionService.getCutoffStateTypes();
        } else if (this.props.componentType === constants.TicketType.Ownership) {
            return optionService.getOwnershipStateTypes();
        } else if (this.props.componentType === constants.TicketType.OfficialDelta || this.props.componentType === constants.TicketType.Delta) {
            return optionService.getOfficialDeltaStateTypes();
        } else if (this.props.componentType === constants.TicketType.Delta) {
            return optionService.getDeltaStateTypes();
        } else if (this.props.componentType === constants.TicketType.OfficialLogistics) {
            return optionService.getOfficialLogisticsStateTypes();
        } else if (this.props.componentType === constants.TicketType.LogisticMovements) {
            return optionService.getLogisticMovementsStateTypes();
        }
        return optionService.getLogisticsStateTypes();
    }

    getColumns() {
        const actionCell = rowProps => (<ActionCell {...this.props} {...rowProps}
            onView={this.viewSummary}
            onDownload={this.props.componentType === constants.TicketType.Logistics
                || this.props.componentType === constants.TicketType.OfficialLogistics ? this.onDownload : this.navigateToPBIReport} />);

        const date = rowProps => <DateCell {...this.props} {...rowProps} />;
        const dateWithTime = rowProps => <DateCell {...this.props} {...rowProps} dateWithTime={true} />;
        const ticketStatus = gridProps => <UploadStatusCell {...this.props} {...gridProps} getStatus={this.getStatus} noLocalize={this.hasNoLocalize} />;

        const columns = [];
        if (!this.isLogistics()) {
            const ticketColumnName = this.isLogisticMovements() ? 'batch' : 'ticket';
            columns.push(gridUtils.buildTextColumn('ticketId', this.props, r => <span className="float-r">{r.original.ticketId}</span>, ticketColumnName, { width: 100 }));
        }
        columns.push(gridUtils.buildTextColumn('segment', this.props));
        if (this.isLogistics()) {
            columns.push(gridUtils.buildTextColumn('nodeName', this.props, null, 'node'));
            columns.push(gridUtils.buildTextColumn('ownerName', this.props, r => utilities.toSentenceCase(r.original.ownerName), 'owner'));
        }
        if (this.isLogisticMovements()) {
            columns.push(gridUtils.buildTextColumn('scenarioName', this.props, null, 'stage'));
        }
        columns.push(gridUtils.buildDateColumn('ticketStartDate', this.props, date));
        columns.push(gridUtils.buildDateColumn('ticketFinalDate', this.props, date));
        columns.push(gridUtils.buildDateColumn('cutoffExecutionDate', this.props, dateWithTime));
        columns.push(gridUtils.buildTextColumn('createdBy', this.props));
        columns.push(gridUtils.buildSelectColumn('state', this.props, ticketStatus, 'state', {
            width: 150,
            values: this.getSelectValues()
        }));
        columns.push(gridUtils.buildActionColumn(actionCell, '', this.props.actionCellWith || 140));
        return columns;
    }

    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} shouldRefresh={this.shouldRefresh} />
        );
    }

    componentDidMount() {
        if (this.props.download) {
            this.props.requestFileReadAccess();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    const hasDownload = (ownProps.componentType === constants.TicketType.Delta ||
        ownProps.componentType === constants.TicketType.OfficialDelta ||
        ownProps.componentType === constants.TicketType.LogisticMovements
    ) ? false : true;

    return {
        view: true,
        download: hasDownload,
        trueKey: 'finalized',
        falseKey: 'processing',
        tickets: state.grid.tickets ? state.grid.tickets.items : [],
        readAccessInfo: state.shared.readAccessInfo
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        saveCutOffReportFilter: data => {
            dispatch(saveCutOffReportFilter(data));
        },
        showError: (ticket, title, popUpName, className, titleClassName = '') => {
            dispatch(initializeTicketError(ticket));
            dispatch(openModal(popUpName, '', title, className, titleClassName));
        },
        enableView: row => {
            if (ownProps.componentType === constants.TicketType.Logistics) {
                return !utilities.isNullOrUndefined(row.errorMessage) &&
                (row.state === constants.StatusType.FAILED || row.state === constants.StatusType.ERROR);
            }
            if (ownProps.componentType === constants.TicketType.Delta) {
                return row.state === constants.StatusType.FAILED;
            }
            if (ownProps.componentType === constants.TicketType.OfficialDelta) {
                return row.state !== constants.StatusType.PROCESSING;
            }
            if (ownProps.componentType === constants.TicketType.OfficialLogistics) {
                return row.state === constants.StatusType.ERROR;
            }
            if (ownProps.componentType === constants.TicketType.LogisticMovements) {
                return row.state === constants.StatusType.SENT || row.state === constants.StatusType.FINALIZED || row.state === constants.StatusType.FAILED || row.state === constants.StatusType.ERROR;
            }
            return row.state === constants.StatusType.PROCESSED || row.state === constants.StatusType.FAILED || row.state === constants.StatusType.FINALIZED;
        },
        viewTitle: ticket => {
            if (!utilities.isNullOrUndefined(ownProps.viewTitle)) {
                return ownProps.viewTitle;
            }
            return ticket.errorMessage ? 'viewError' : 'viewSummary';
        },
        enableDownload: row => {
            return (row.state === constants.StatusType.PROCESSED
                || row.state === constants.StatusType.FINALIZED
                || row.state === constants.StatusType.CONCILIATIONFAILED)
                && utilities.isNullOrWhitespace(row.errorMessage);
        },
        requestFileReadAccess: () => {
            dispatch(requestFileReadAccessInfo());
        },
        hidePageActions: hiddenActions => {
            dispatch(hidePageActions(hiddenActions));
        }
    };
};

/* istanbul ignore next */
const defaultFilter = props => {
    const gridDetails = ticketService.getGridInfo(props);
    const date = dateService.subtract(dateService.now(), gridDetails.days, 'd');
    return `cutoffExecutionDate gt ${date.toISOString()} and ${gridDetails.filter}`;
};

/* istanbul ignore next */
const gridConfig = props => {
    return {
        name: 'tickets',
        idField: 'ticketId',
        apiUrl: apiService.ticket.getTickets(),
        sortable: {
            defaultSort: 'cutoffExecutionDate desc'
        },
        filterable: {
            defaultFilter: defaultFilter(props),
            override: false
        },
        refreshable: {
            interval: constants.Timeouts.Tickets
        },
        defaultPageSize: props.componentType === constants.TicketType.OfficialDelta || props.componentType === constants.TicketType.OfficialLogistics ? 50 : 10,
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(TicketsGrid, gridConfig));

