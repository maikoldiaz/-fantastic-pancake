import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dateService } from '../../../../common/services/dateService';
import Grid from './../../../../common/components/grid/grid.jsx';
import { DateCell, UploadStatusCell } from './../../../../common/components/grid/gridCells.jsx';
import { gridUtils } from './../../../../common/components/grid/gridUtils';
import { dataGrid } from './../../../../common/components/grid/gridComponent';
import { optionService } from '../../../../common/services/optionService';
import { constants } from './../../../../common/services/constants';
import { createLogisticTicket } from '../actions';
import { refreshGrid, receiveGridData } from './../../../../common/components/grid/actions';
import { openMessageModal } from './../../../../common/actions';
import { utilities } from '../../../../common/services/utilities';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

class LogisticsValidation extends React.Component {
    constructor() {
        super();
        this.saveLogisticTicket = this.saveLogisticTicket.bind(this);
        this.getStatus = this.getStatus.bind(this);
    }
    getColumns() {
        const date = rowProps => <DateCell {...this.props} {...rowProps} />;
        const nodeStatus = gridProps => <UploadStatusCell {...this.props} {...gridProps} getStatus={this.getStatus} noLocalize={true} />;

        const columns = [];
        if (this.props.componentType === constants.TicketType.OfficialLogistics || (this.props.componentType === constants.TicketType.Logistics && this.props.logisticsInfo.node.name === 'Todos')) {
            columns.push(gridUtils.buildTextColumn('nodeName', this.props));
        }
        if (this.props.componentType === constants.TicketType.Logistics) {
            columns.push(gridUtils.buildDateColumn('operationDate', this.props, date, 'date'));
        }

        columns.push(gridUtils.buildSelectColumn('nodeStatus', this.props, nodeStatus, 'state', {
            width: 150,
            values: this.getSelectValues()
        }));
        return columns;
    }

    getSelectValues() {
        return optionService.getOwnershipNodeStateTypes();
    }

    getStatus(row) {
        return row.nodeStatus;
    }

    saveLogisticTicket() {
        const ticket = {
            Ticket: {
                startDate: this.props.logisticsInfo.startDate,
                endDate: this.props.logisticsInfo.endDate,
                categoryElementId: this.props.logisticsInfo.segmentId,
                nodeId: this.props.logisticsInfo.node.nodeId,
                categoryElementName: this.props.logisticsInfo.segmentName,
                ticketTypeId: this.props.componentType,
                ownerId: this.props.logisticsInfo.owner[0].elementId
            }
        };

        this.props.createLogisticTicket(ticket);
    }

    render() {
        const logisticsInfo = this.props.logisticsInfo;
        const isErrorExist = this.props.validationData && this.props.validationData.length > 0;
        const errorCount = this.props.validationData && this.props.validationData.length > 0 ? this.props.validationData.length : 0;
        return (
            <>
                <section className="ep-modal__content">
                    {!isErrorExist && this.props.componentType === constants.TicketType.Logistics &&
                        <h1 className="fs-18 fw-bold m-t-0 m-b-6">{resourceProvider.read('logisticsValidationTitle')}</h1>
                    }

                    {!isErrorExist && this.props.componentType === constants.TicketType.OfficialLogistics &&
                        <h1 className="fs-14 fw-bold m-t-0 m-b-6">
                            <label className="ep-label d-inline-block m-r-2">{resourceProvider.read('validation')}:</label>
                            {this.props.logisticsInfo.node.name === 'Todos' ?
                                resourceProvider.read('officialBalApprovedAllNodesValSuccess') :
                                resourceProvider.read('officialBalApprovedSingleNodeValSuccess')}
                        </h1>
                    }

                    {isErrorExist && this.props.componentType === constants.TicketType.OfficialLogistics &&
                        <h1 className="fs-14 fw-bold m-t-0 m-b-6">
                            <label className="ep-label d-inline-block m-r-2">{resourceProvider.read('error')}:</label>
                            {this.props.logisticsInfo.node.name === 'Todos' ?
                                resourceProvider.read('officialBalApprovedAllNodesValFailure') :
                                resourceProvider.read('officialBalApprovedSingleNodeValFailure')}
                        </h1>
                    }
                    <div className="ep-label-wrap">
                        <label className="ep-label">{resourceProvider.read('segment')}:</label>
                        <span className="ep-data m-l-2 fw-bold" id="span_logisticsValidation_segment">{logisticsInfo.segmentName}</span>
                    </div>
                    <div className="ep-label-wrap">
                        <label className="ep-label">{resourceProvider.read('node')}:</label>
                        <span className="ep-data m-l-2 fw-bold" id="span_logisticsValidation_node">{logisticsInfo.node.name}</span>
                    </div>
                    <div className="ep-label-wrap">
                        <label className="ep-label">{resourceProvider.read('owner')}:</label>
                        <span className="ep-data m-l-2 fw-bold" id="span_logisticsValidation_owner">{utilities.toSentenceCase(logisticsInfo.owner[0].name)}</span>
                    </div>
                    <div className="ep-label-wrap">
                        <label className="ep-label">{resourceProvider.read('period')}:</label>
                        <span className="ep-data m-l-2 fw-bold text-caps" id="span_logisticsValidation_periodFrom">
                            {dateService.format(logisticsInfo.startDate)}</span>
                        <span className="ep-data m-l-1" id="span_logisticsValidation_period">
                            {resourceProvider.read('to')}</span>
                        <span className="ep-data m-l-1 fw-bold text-caps" id="span_logisticsValidation_periodTo">
                            {dateService.format(logisticsInfo.endDate)}</span>
                    </div>
                    {isErrorExist &&
                        <>
                            {this.props.componentType === constants.TicketType.Logistics &&
                                <div className="ep-label-wrap m-t-6">
                                    <label className="ep-label">{resourceProvider.read('error')}:</label>
                                    <span className="ep-data m-l-2 fw-bold" id="span_logisticsValidation_error">{resourceProvider.read('nodesNotApproved')}</span>
                                </div>
                            }
                            <div className="ep-label-wrap m-b-3 m-t-6">
                                <label className="ep-label">{resourceProvider.read(this.props.componentType === constants.TicketType.Logistics ? 'errorRecordCount' : 'totalNodes')}:</label>
                                <span className="ep-data m-l-2 fw-bold fas--success" id="span_logisticsValidation_error">{errorCount}</span>
                            </div>
                            <Grid name={this.props.name} columns={this.getColumns()} className="ep-table--pivotal ep-table--pivotal-alt" />
                        </>
                    }
                </section>
                {!isErrorExist &&
                    <ModalFooter config={footerConfigService.getCommonConfig('logisticsValidation',
                        { onAccept: this.saveLogisticTicket, acceptType: 'button', acceptText: 'createlogisticReport' })} />}
                {isErrorExist && <ModalFooter config={footerConfigService.getAcceptConfig('logisticsValidation', { closeModal: true, acceptText: 'accept' })} />}
            </>
        );
    }

    componentDidMount() {
        this.props.loadGridData(this.props.validationData);
    }
    componentDidUpdate(prevProps) {
        if (prevProps.refreshToggler !== this.props.refreshToggler) {
            this.props.refreshGrid('tickets');
            this.props.closeModal();
        }

        if ((prevProps.failureToggler !== this.props.failureToggler) && (!utilities.isNullOrUndefined(this.props.failureToggler))) {
            this.props.closeModal();
            this.props.showTechnicalError(resourceProvider.read('systemErrorMessage'), resourceProvider.read('error'), false);
        }
    }
}

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    const name = constants.LogisticsType[ownProps.componentType];
    return {
        createLogisticTicket: logisticTicket => {
            dispatch(createLogisticTicket(logisticTicket, name));
        },
        refreshGrid: gridName => {
            dispatch(refreshGrid(gridName));
        },
        loadGridData: validationData => {
            dispatch(receiveGridData(validationData, 'logisticsValidationGrid'));
        },
        showTechnicalError: (message, title, canCancel) => {
            dispatch(openMessageModal(message, { title: title, canCancel: canCancel }));
        }
    };
};

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    const name = constants.LogisticsType[ownProps.componentType];
    return {
        logisticsInfo: state.logistics[name].logisticsInfo,
        refreshToggler: state.logistics[name].refreshToggler,
        validationData: state.logistics[name].validationData,
        failureToggler: state.logistics[name].failureToggler
    };
};

/* istanbul ignore next */
const gridConfig = () => {
    return {
        name: 'logisticsValidationGrid',
        idField: 'nodeId',
        odata: false,
        showPagination: false,
        startEmpty: true,
        defaultPageSize: 999999,
        sortable: false,
        filterable: false
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(LogisticsValidation, gridConfig));
