import React from 'react';
import Grid from '../../../../common/components/grid/grid.jsx';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../common/services/apiService';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { optionService } from '../../../../common/services/optionService';
import { UploadStatusCell, DateCell } from './../../../../common/components/grid/gridCells.jsx';
import { navigationService } from '../../../../common/services/navigationService.js';
import { dateService } from '../../../../common/services/dateService';
import { requestGetTicket, requestForwardToSap, requestMovements } from '../actions';
import { routerActions } from '../../../../common/router/routerActions';
import { enableDisablePageAction } from '../../../../common/actions';
import { constants } from '../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';
import { disableGridItems } from '../../../../common/components/grid/actions';

export class SendToSapDetail extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
        this.getMovements = this.getMovements.bind(this);
        this.transformData = this.transformData.bind(this);
        this.forwardToSap = this.forwardToSap.bind(this);
        this.onReceiveData = this.onReceiveData.bind(this);
        this.shouldRefresh = this.shouldRefresh.bind(this);
        routerActions.configure('forwardToSap', this.forwardToSap);
    }

    getColumns() {
        const columns = [];

        const Status = gridProps => <UploadStatusCell {...this.props} {...gridProps} getStatus={this.getStatus} noLocalize={true} />;
        const date = rowProps => <DateCell {...this.props} {...rowProps} />;

        columns.push(gridUtils.buildSelectColumn('state', this.props, Status, 'state', {
            width: 140,
            values: optionService.getLogisticsMovementStateTypes()
        }));
        columns.push(gridUtils.buildTextColumn('description', this.props, '', 'description', { width: 140 }));
        columns.push(gridUtils.buildTextColumn('movementType', this.props, '', 'movementType', { width: 160 }));
        columns.push(gridUtils.buildTextColumn('sourceCenter', this.props, '', 'sourceCenter', { width: 140 }));
        columns.push(gridUtils.buildTextColumn('sourceStorage', this.props, '', 'sourceStorage', { width: 150 }));
        columns.push(gridUtils.buildTextColumn('sourceProduct', this.props, '', 'sourceProduct', { width: 160 }));
        columns.push(gridUtils.buildTextColumn('destinationCenter', this.props, '', 'destinationCenter', { width: 140 }));
        columns.push(gridUtils.buildTextColumn('destinationStorage', this.props, '', 'destinationStorage', { width: 150 }));
        columns.push(gridUtils.buildTextColumn('destinationProduct', this.props, '', 'destinationProduct', { width: 160 }));
        columns.push(gridUtils.buildDecimalColumn('ownershipVolume', this.props, '', 'ownershipVolume', { width: 120 }));
        columns.push(gridUtils.buildTextColumn('units', this.props, '', 'units'));
        columns.push(gridUtils.buildDateColumn('operationalDate', this.props, date, 'dateOperational', { width: 140 }));
        columns.push(gridUtils.buildTextColumn('movementId', this.props, '', 'officialMovementId', { width: 140 }));
        columns.push(gridUtils.buildTextColumn('costCenter', this.props, '', 'costCenter', { width: 150 }));
        columns.push(gridUtils.buildTextColumn('gmCode', this.props, '', 'gmCode'));
        columns.push(gridUtils.buildTextColumn('documentNumber', this.props, '', 'documentNumber'));
        columns.push(gridUtils.buildTextColumn('position', this.props, '', 'position'));
        columns.push(gridUtils.buildTextColumn('order', this.props, '', 'order'));
        columns.push(gridUtils.buildDateColumn('accountingDate', this.props, date, 'dateAccounting', { width: 180 }));

        return columns;
    }

    getStatus(row) {
        return row.state;
    }

    shouldRefresh() {
        return true;
    }

    transformData(json) {
        if (utilities.isNullOrUndefined(json.value) || json.value.length === 0) {
            return json;
        }

        const transformedValue = json.value.map(item => ({
            ...item,
            disabled: item.state !== constants.StatusType.FAILED
        }));

        return {
            ...json,
            value: transformedValue
        };
    }

    getMovements() {
        const filter = `$filter=state ne 'Cancelado'`;
        this.props.getLogisticsMovements(this.props.name, navigationService.getParamByName('ticketId'), this.transformData, filter);
    }

    forwardToSap() {
        const logisticsMovements = {
            ticketId: Number(navigationService.getParamByName('ticketId')),
            movements: this.props.selection.map(movement => movement.movementTransactionId)
        };
        this.props.forwardToSap(logisticsMovements);
    }

    onReceiveData(items) {
        const disableIds = items.filter(x => x.state !== constants.StatusType.FAILED).map(x => x.movementTransactionId);
        if (disableIds.length > 0) {
            this.props.disableGridItems(disableIds);
        }
    }

    componentDidMount() {
        this.props.getTicket(navigationService.getParamByName('ticketId'));
        this.props.enableDisableForward(true);
    }

    componentDidUpdate(prevProps) {
        this.props.enableDisableForward(!this.props.selection.length > 0);

        if (prevProps.forwardToggler !== this.props.forwardToggler) {
            this.getMovements();
        }
    }

    render() {
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <div className="ep-section ep-section--panel">
                        <div className="ep-section__body ep-section__body--h">
                            <div className="row">
                                <div className="col-md-3">
                                    <div className="d-flex">
                                        <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('segment')}:</label>
                                        <span className="ep-data fw-bold">{this.props.segment}</span>
                                    </div>
                                </div>
                                <div className="col-md-3">
                                    <div className="d-flex">
                                        <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('period')}:</label>
                                        <span className="ep-data fw-bold">{
                                            dateService.format(this.props.ticketStartDate)
                                            + ' ' + resourceProvider.read('to') + ' ' +
                                            dateService.format(this.props.ticketFinalDate)}
                                        </span>
                                    </div>
                                </div>
                                <div className="col-md-3">
                                    <div className="d-flex">
                                        <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('owner')}:</label>
                                        <span className="ep-data fw-bold">{this.props.ownerName}</span>
                                    </div>
                                </div>
                                <div className="col-md-3">
                                    <div className="d-flex">
                                        <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('stage')}:</label>
                                        <span className="ep-data fw-bold">{this.props.scenarioName}</span>
                                    </div>
                                </div>
                            </div>
                            <Grid name={this.props.name} onReceiveData={this.onReceiveData} columns={this.getColumns()} shouldRefresh={this.shouldRefresh} />
                        </div>
                    </div>
                </div>
            </section>
        );
    }
}

const mapStateToProps = state => {
    return {
        segment: state.sendToSap.ticket.segment,
        ticketStartDate: state.sendToSap.ticket.ticketStartDate,
        ticketFinalDate: state.sendToSap.ticket.ticketFinalDate,
        ownerName: state.sendToSap.ticket.ownerName,
        scenarioName: state.sendToSap.ticket.scenarioName,
        selection: state.grid.movements ? state.grid.movements.selection : [],
        forwardToggler: state.sendToSap.forwardToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getTicket: ticketId => {
            dispatch(requestGetTicket(ticketId));
        },
        getLogisticsMovements: (name, ticketId, transformData, queryString) => {
            dispatch(requestMovements(name, ticketId, transformData, queryString));
        },
        forwardToSap: logisticsMovements => {
            dispatch(requestForwardToSap(logisticsMovements));
        },
        enableDisableForward: disabled => {
            dispatch(enableDisablePageAction('forwardToSap', disabled));
        },
        disableGridItems: keyValues => {
            dispatch(disableGridItems('movements', 'movementTransactionId', keyValues));
        }
    };
};

/* istanbul ignore next */
const detailGridConfig = () => {
    return {
        name: 'movements',
        idField: 'movementTransactionId',
        selectable: true,
        apiUrl: apiService.logistic.getMovementsDetail(navigationService.getParamByName('ticketId')),
        section: true,
        sortable: {
            defaultSort: 'movementId desc'
        },
        refreshable: {
            interval: constants.Timeouts.Sap
        }
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(SendToSapDetail, detailGridConfig));
