import React from 'react';
import { connect } from 'react-redux';
import { apiService } from '../../../../common/services/apiService';
import Grid from '../../../../common/components/grid/grid.jsx';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { ActionCell, DateCell, UploadStatusCell } from '../../../../common/components/grid/gridCells.jsx';
import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { navigationService } from '../../../../common/services/navigationService';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { dateService } from '../../../../common/services/dateService';
import { openModal, setModuleName, hidePageActions, showPageAction, openMessageModal } from '../../../../common/actions';
import { initializeNodeErrorDetail, requestLastOperationalTicket, requestConciliationNode } from '../actions';
import { saveCutOffReportFilter } from '../../../report/cutOff/actions';
import { optionService } from '../../../../common/services/optionService';
import { refreshSilent } from '../../../../common/components/grid/actions';

export class OwnershipNodeGrid extends React.Component {
    constructor() {
        super();
        this.enableEdit = this.enableEdit.bind(this);
        this.enableExecute = this.enableExecute.bind(this);
    }

    getColumns() {
        const actionCell = rowProps => (<ActionCell {...this.props} {...rowProps}
            enableExecute={this.enableExecute} enableEdit={this.enableEdit} onDownload={row => this.props.saveCutOffReportFilter(row)} />);

        const date = rowProps => <DateCell {...this.props} {...rowProps} />;
        const dateWithTime = rowProps => <DateCell {...this.props} {...rowProps} dateWithTime={true} />;
        const ticketStatus = gridProps => <UploadStatusCell {...this.props} {...gridProps} noLocalize={true} />;
        const columns = [];
        columns.push(gridUtils.buildTextColumn('ticketId', this.props, r => <span className="float-r">{r.original.ticketId}</span>, 'ticket', { width: 100 }));
        columns.push(gridUtils.buildDateColumn('ticketStartDate', this.props, date, 'initialDateCapital'));
        columns.push(gridUtils.buildDateColumn('ticketFinalDate', this.props, date, 'finalDate'));
        columns.push(gridUtils.buildDateColumn('cutoffExecutionDate', this.props, dateWithTime, 'cutoffExecutionDate'));
        columns.push(gridUtils.buildTextColumn('createdBy', this.props));
        columns.push(gridUtils.buildTextColumn('nodeName', this.props, null, 'node'));
        columns.push(gridUtils.buildTextColumn('segment', this.props));
        columns.push(gridUtils.buildSelectColumn('state', this.props, ticketStatus, 'state', {
            width: 180,
            values: optionService.getOwnershipNodeStateTypes()
        }));
        columns.push(gridUtils.buildActionColumn(actionCell, '', 160));
        return columns;
    }

    enableEdit(row) {
        return this.props.lastTicketPerSegment.filter(x => x.ticketId === row.ticketId).length > 0 && this.props.enableDownload(row);
    }

    enableExecute(row) {
        return this.props.enableExecute(row, this.props);
    }

    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} />
        );
    }

    componentDidMount() {
        this.props.requestLastOperationalTicket();
        if (!utilities.isNullOrUndefined(this.props.hideAction)) {
            this.props.hidePageActions([this.props.hideAction]);
        }
    }

    componentWillUnmount() {
        if (!utilities.isNullOrUndefined(this.props.hideAction)) {
            this.props.showPageAction(this.props.hideAction);
        }
    }

    componentDidUpdate(prevProps) {
        if ((prevProps.conciliationSuccessToggler !== this.props.conciliationSuccessToggler) && (!utilities.isNullOrUndefined(this.props.conciliationSuccessToggler))) {
            this.props.refreshGrid();
        }

        if ((prevProps.conciliationErrorToggler !== this.props.conciliationErrorToggler) && (!utilities.isNullOrUndefined(this.props.conciliationErrorToggler))) {
            this.props.confirmModal(resourceProvider.read('conciliationErrorResponse'), resourceProvider.read('error'), false);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        view: true,
        edit: true,
        download: true,
        execute: true,
        viewTitle: 'viewError',
        editTitle: 'editOwnership',
        downloadTitle: 'viewReport',
        executeTitle: 'manualConciliation',
        lastTicketPerSegment: state.nodeOwnership.ownershipNode.lastTicketPerSegment,
        conciliationSuccessToggler: state.nodeOwnership.ownershipNode.conciliationSuccessToggler,
        conciliationErrorToggler: state.nodeOwnership.ownershipNode.conciliationErrorToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onView: node => {
            dispatch(initializeNodeErrorDetail(node));
            dispatch(openModal('showError', '', resourceProvider.read('detailNodeProcessingError'), 'ep-modal--lg'));
        },
        onEdit: node => {
            navigationService.navigateToModule(`ownershipnodes/manage/${node.ownershipNodeId}_${constants.DetailsPageType.Details}`);
        },
        onExecute: node => {
            const data = {
                ticketId: node.ticketId,
                nodeId: node.nodeId
            };
            dispatch(requestConciliationNode(data));
        },
        requestLastOperationalTicket: () => {
            dispatch(requestLastOperationalTicket());
        },
        setModuleName: moduleName => {
            dispatch(setModuleName(moduleName));
        },
        enableView: row => {
            return utilities.matchesAny(row.state, [
                constants.OwnershipNodeStatusType.FAILED, constants.OwnershipNodeStatusType.NOTCONCILIATED, constants.OwnershipNodeStatusType.CONCILIATIONFAILED
            ]);
        },
        enableDownload: row => {
            return !utilities.matchesAny(row.state, [
                constants.OwnershipNodeStatusType.PROCESSING, constants.OwnershipNodeStatusType.FAILED, constants.OwnershipNodeStatusType.PUBLISHING
            ]);
        },
        enableExecute: (row, props) => {
            if (props.lastTicketPerSegment.filter(x => x.ticketId === row.ticketId).length > 0 && props.enableDownload(row)) {
                if (utilities.matchesAny(row.state, [
                    constants.OwnershipNodeStatusType.UNLOCKED, constants.OwnershipNodeStatusType.PUBLISHED, constants.OwnershipNodeStatusType.REJECTED, constants.OwnershipNodeStatusType.REOPENED
                ])) {
                    return row.isTransferPoint;
                }
                return utilities.matchesAny(row.state, [
                    constants.OwnershipNodeStatusType.CONCILIATED, constants.OwnershipNodeStatusType.NOTCONCILIATED, constants.OwnershipNodeStatusType.CONCILIATIONFAILED
                ]);
            }
            return false;
        },
        getStatus: row => {
            return row.state;
        },
        getErrorCount: row => {
            let errorCount = 0;
            if (!utilities.isNullOrUndefined(row.status) && row.state === constants.OwnershipNodeStatusType.FAILED) {
                errorCount = 1;
            }

            return errorCount;
        },
        saveCutOffReportFilter: row => {
            const initialDate = dateService.convertToColombian(dateService.parseToDate(dateService.format(row.ticketStartDate)));
            const finalDate = dateService.convertToColombian(dateService.parseToDate(dateService.format(row.ticketFinalDate)));
            const data = {
                categoryName: row.categoryName,
                elementName: row.segment,
                nodeName: row.nodeName,
                initialDate,
                finalDate,
                reportType: constants.Report.WithOwner
            };
            dispatch(saveCutOffReportFilter(data));
            navigationService.navigateToModule(`cutoffreport/manage/view`);
        },
        hidePageActions: hiddenActions => {
            dispatch(hidePageActions(hiddenActions));
        },
        showPageAction: actionName => {
            dispatch(showPageAction(actionName));
        },
        refreshGrid: () => {
            dispatch(refreshSilent('ownershipNodes'));
        },
        confirmModal: (message, title, canCancel) => {
            dispatch(openMessageModal(message, {
                title: title,
                canCancel: canCancel
            }));
        }
    };
};

/* istanbul ignore next */
const defaultFilter = props => {
    let ticketFilter = '';
    const ticketId = props.ticketId || navigationService.getParamByName('ticketId');
    if (utilities.checkIfNumber(ticketId)) {
        ticketFilter = ` and ticketId eq ${ticketId}`;
    }
    const date = dateService.subtract(dateService.now(), systemConfigService.getDefaultOwnershipNodeLastDays(), 'd');
    return `cutoffExecutionDate gt ${date.toISOString()}${ticketFilter}`;
};

/* istanbul ignore next */
const gridConfig = props => {
    return {
        name: 'ownershipNodes',
        apiUrl: apiService.ownershipNode.query(),
        filterable: {
            defaultFilter: defaultFilter(props),
            override: false
        },
        sortable: {
            defaultSort: 'ticketId desc'
        },
        refreshable: {
            interval: constants.Timeouts.OwnershipNodes
        },
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(dataGrid(OwnershipNodeGrid, gridConfig));
