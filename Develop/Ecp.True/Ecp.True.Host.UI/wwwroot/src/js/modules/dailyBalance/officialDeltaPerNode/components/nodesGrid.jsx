import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { ActionCell, DateCell, UploadStatusCell } from '../../../../common/components/grid/gridCells.jsx';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { dateService } from '../../../../common/services/dateService';
import { navigationService } from '../../../../common/services/navigationService';
import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';
import { optionService } from '../../../../common/services/optionService';
import { apiService } from '../../../../common/services/apiService.js';
import { routerActions } from '../../../../common/router/routerActions';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { openModal } from '../../../../common/actions';
import { initializeTicketError } from '../../../transportBalance/cutOff/actions';
import { setGridValues, setDeltaNodeSource } from '../../officialDeltaPerNode/actions';

export class NodesGrid extends React.Component {
    constructor() {
        super();
        this.viewSummary = this.viewSummary.bind(this);
        this.onDownload = this.onDownload.bind(this);
        routerActions.configure('returnListing', () => navigationService.goBack());
    }

    viewSummary(nodeData) {
        if (nodeData.status === constants.StatusType.FAILED) {
            this.props.showError(nodeData, resourceProvider.read('officialDeltasBusinessErrorsTitle'), 'showDeltaBusinessError', 'ep-modal--lg');
        }
    }

    getColumns() {
        const actionCell = rowProps => (<ActionCell {...this.props} {...rowProps} onView={this.viewSummary} onDownload={this.onDownload} />);

        const date = rowProps => <DateCell {...this.props} {...rowProps} />;
        const dateWithTime = rowProps => <DateCell {...this.props} {...rowProps} dateWithTime={true} />;
        const ticketStatus = gridProps => <UploadStatusCell {...this.props} {...gridProps} noLocalize={true} />;
        const columns = [];
        columns.push(gridUtils.buildTextColumn('ticketId', this.props, r => <span className="float-r">{r.original.ticketId}</span>, 'ticket', { width: 100 }));
        columns.push(gridUtils.buildDateColumn('startDate', this.props, date, 'initialDateCapital'));
        columns.push(gridUtils.buildDateColumn('endDate', this.props, date, 'finalDate'));
        columns.push(gridUtils.buildDateColumn('executionDate', this.props, dateWithTime, 'cutoffExecutionDate'));
        columns.push(gridUtils.buildTextColumn('createdBy', this.props, null, 'createdBy'));
        columns.push(gridUtils.buildTextColumn('nodeName', this.props, null, 'node'));
        columns.push(gridUtils.buildTextColumn('segment', this.props, null, 'segment'));
        columns.push(gridUtils.buildSelectColumn('status', this.props, ticketStatus, 'state', {
            width: 200,
            values: optionService.officialDeltaPerNodeStateTypes()
        }));
        columns.push(gridUtils.buildActionColumn(actionCell, '', 140));
        return columns;
    }

    onDownload(row) {
        this.props.setSource('grid');
        navigationService.navigateToModule('officialdeltanode/manage/' + row.deltaNodeId);
    }

    shouldRefresh() {
        return true;
    }

    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} shouldRefresh={this.shouldRefresh} />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        nodeGridToggler: state.officialDeltaPerNode.nodeGridToggler,
        nodeGridValues: state.officialDeltaPerNode.nodeGridValues,
        view: true,
        viewTitle: 'viewErrorCaps',
        download: true,
        downloadTitle: 'viewReport'
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        showError: (nodeData, title, popUpName, className) => {
            dispatch(initializeTicketError(nodeData));
            dispatch(openModal(popUpName, '', title, className));
        },
        enableView: row => {
            return row.status === constants.OwnershipNodeStatusType.FAILED;
        },
        enableDownload: row => {
            return !(row.status === constants.OwnershipNodeStatusType.FAILED || row.status === constants.StatusType.PROCESSING);
        },
        getStatus: row => {
            return row.status;
        },
        setGridValues: data => {
            dispatch(setGridValues(data));
        },
        setSource: source => {
            dispatch(setDeltaNodeSource(source));
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
    const date = dateService.subtract(dateService.now(), systemConfigService.getDefaultOfficialDeltaPerNodeLastDays(), 'd');
    return `executionDate gt ${date.toISOString()}${ticketFilter}`;
};

/* istanbul ignore next */
const gridConfig = props => {
    return {
        name: 'officialDeltaNodesGrid',
        apiUrl: apiService.officialDelta.getOfficialDeltaPerNode(),
        sortable: {
            defaultSort: 'ticketId desc, nodeName asc'
        },
        filterable: {
            defaultFilter: defaultFilter(props),
            override: false
        },
        refreshable: {
            interval: constants.Timeouts.Tickets
        },
        defaultPageSize: 50,
        section: true
    };
};


/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(dataGrid(NodesGrid, gridConfig));
