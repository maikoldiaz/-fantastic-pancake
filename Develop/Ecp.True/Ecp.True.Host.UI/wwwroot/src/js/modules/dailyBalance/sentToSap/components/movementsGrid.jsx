import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { apiService } from '../../../../common/services/apiService';
import { utilities } from '../../../../common/services/utilities';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { requestMovements, initMovementsConfirmation, initSentToSap, requestSendMovements, resetConfirmWizardData, requestCancelBatch } from '../actions';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { selectAllGridData } from '../../../../common/components/grid/actions';
import { openModal, openMessageModal } from '../../../../common/actions';
import { navigationService } from '../../../../common/services/navigationService';
import { DateCell } from '../../../../common/components/grid/gridCells.jsx';

export class MovementsGrid extends React.Component {
    constructor() {
        super();
        this.getMovements = this.getMovements.bind(this);
        this.cancelWizard = this.cancelWizard.bind(this);
    }

    cancelWizard() {
        navigationService.navigateToModule('senttosap/manage');
    }

    getColumns() {
        const columns = [];

        const dateCell = rowProps => <DateCell {...this.props} {...rowProps} ignoreMax={true} />;

        columns.push(gridUtils.buildTextColumn('segment', this.props, null, 'segment'));
        columns.push(gridUtils.buildTextColumn('gmCode', this.props, null, 'gmCode'));
        columns.push(gridUtils.buildTextColumn('movementType', this.props, null, 'movementType'));
        columns.push(gridUtils.buildTextColumn('sourceCenter', this.props, null, 'sourceCenter'));
        columns.push(gridUtils.buildTextColumn('sourceStorage', this.props, null, 'sourceStorage'));
        columns.push(gridUtils.buildTextColumn('sourceProduct', this.props, null, 'sourceProduct'));
        columns.push(gridUtils.buildTextColumn('destinationCenter', this.props, null, 'destinationCenter'));
        columns.push(gridUtils.buildTextColumn('destinationStorage', this.props, null, 'destinationStorage'));
        columns.push(gridUtils.buildTextColumn('costCenter', this.props, null, 'costCenter'));
        columns.push(gridUtils.buildTextColumn('destinationProduct', this.props, null, 'destinationProduct'));
        columns.push(gridUtils.buildTextColumn('documentNumber', this.props, null, 'documentNumber'));
        columns.push(gridUtils.buildTextColumn('position', this.props, null, 'position'));
        columns.push(gridUtils.buildDecimalColumn('ownershipVolume', this.props, null, 'ownershipVolume'));
        columns.push(gridUtils.buildTextColumn('units', this.props, null, 'units'));
        columns.push(gridUtils.buildTextColumn('order', this.props, null, 'order'));
        columns.push(gridUtils.buildDateColumn('operationalDate', this.props, dateCell, 'dateOperational'));
        columns.push(gridUtils.buildTextColumn('movementId', this.props, null, 'movementId'));
        columns.push(gridUtils.buildTextColumn('scenario', this.props, null, 'scenario'));
        columns.push(gridUtils.buildTextColumn('owner', this.props, null, 'owner'));

        return columns;
    }

    getMovements() {
        this.props.requestMovements(this.props.name, this.props.ticketId);
    }

    render() {
        if (!this.props.ticketId) {
            return null;
        }

        return (
            <div className="ep-section">
                <header className="ep-section__header">
                    <h1 className="ep-section__title m-t-3">
                        {
                            resourceProvider.read('subtitleSendToSapPreview')
                        }
                    </h1>
                    <p>
                        {
                            resourceProvider.read('movementCounterSendToSapPreview')
                                .replace('{0}', this.props.selection.length)
                                .replace('{1}', this.props.items.length)
                        }
                    </p>
                </header>
                <div className="ep-section__body ep-section__body--hf p-a-0">
                    <Grid name={this.props.name} columns={this.getColumns()} className="ep-table--row-sm ep-table--wizard" />
                </div>
                <SectionFooter floatRight={true} config={footerConfigService.getCommonConfig('movementsGrid',
                    {
                        onAccept: () => this.props.sendToSap('sapMovements', this.props.selection, this.props.items.length, resourceProvider.read('titleMovementsConfirmationModal')),
                        onCancel: this.cancelWizard,
                        cancelActions: [initSentToSap, selectAllGridData(false, 'sapMovements')],
                        disableAccept: (this.props.selection.length === 0),
                        acceptText: resourceProvider.read('sendToSAP')
                    })} />
            </div>
        );
    }

    componentDidUpdate(prevProps) {
        if (this.props.operationalSentToSap.confirmSentToSapToggler !== prevProps.operationalSentToSap.confirmSentToSapToggler) {
            const ticket = {
                ticketId: this.props.ticketId,
                movements: this.props.operationalSentToSap.sapMovements.selectedMovements.map(movement => movement.movementTransactionId)
            };
            this.props.sendMovements(ticket);
        }

        if ((prevProps.failureToggler !== this.props.failureToggler) && (!utilities.isNullOrUndefined(this.props.failureToggler))) {
            this.props.confirm(this.props.saveSentToSapFailedErrorMessage, resourceProvider.read('error'), false);
        }

        if (this.props.operationalSentToSap.receiveToggler !== prevProps.operationalSentToSap.receiveToggler) {
            this.props.close();
            navigationService.navigateToModule('senttosap/manage');
        }

        if (this.props.operationalSentToSap.confirmCancelBatchToggler !== prevProps.operationalSentToSap.confirmCancelBatchToggler) {
            const ticketId = this.props.ticketId;
            this.props.cancelBatch(ticketId);
        }

        if ((prevProps.failureCancelBatchToggler !== this.props.failureCancelBatchToggler) && (!utilities.isNullOrUndefined(this.props.failureCancelBatchToggler))) {
            this.props.confirm(this.props.saveCancelBatchFailedErrorMessage, resourceProvider.read('error'), false);
        }

        if (this.props.operationalSentToSap.receiveCancelBatchToggler !== prevProps.operationalSentToSap.receiveCancelBatchToggler) {
            this.props.close();
            navigationService.navigateToModule('senttosap/manage');
        }
    }

    componentWillUnmount() {
        this.props.cleanConfirmWizard();
    }
}

const mapStateToProps = state => {
    const props = {
        selection: state.grid.sapMovements ? state.grid.sapMovements.selection : [],
        items: state.grid.sapMovements ? state.grid.sapMovements.items : [],
        operationalSentToSap: state.sendToSap,
        failureToggler: utilities.isNullOrUndefined(state.sendToSap.saveSentToSapFailureToggler) ? null : state.sendToSap.saveSentToSapFailureToggler,
        saveSentToSapFailedErrorMessage: state.sendToSap.saveSentToSapFailedErrorMessage,
        saveSentToSapFailed: utilities.isNullOrUndefined(state.sendToSap.saveSentToSapFailed) ? false : state.sendToSap.saveSentToSapFailed,
        ticketId: state.sendToSap.confirmWizard ? state.sendToSap.confirmWizard.ticketId : '',
        failureCancelBatchToggler: utilities.isNullOrUndefined(state.sendToSap.saveCancelBatchFailureToggler) ? null : state.sendToSap.saveCancelBatchFailureToggler,
        saveCancelBatchFailedErrorMessage: state.sendToSap.saveCancelBatchFailedErrorMessage,
        saveCancelBatchFailed: utilities.isNullOrUndefined(state.sendToSap.saveCancelBatchFailed) ? false : state.sendToSap.saveCancelBatchFailed
    };

    return props;
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        requestMovements: (name, ticketId) => {
            dispatch(requestMovements(name, ticketId));
        },
        sendToSap: (name, selectedMovements, countTotalMovements, title) => {
            dispatch(openModal('movementsConfirmation', '', title));
            dispatch(initMovementsConfirmation(name, selectedMovements, countTotalMovements));
        },
        sendMovements: ticket => {
            dispatch(requestSendMovements(ticket));
        },
        confirm: (message, title, canCancel) => {
            dispatch(openMessageModal(message, {
                title: title,
                canCancel: canCancel
            }));
        },
        cleanConfirmWizard: () => {
            dispatch(resetConfirmWizardData());
        },
        close: () => {
            dispatch(initSentToSap());
            dispatch(selectAllGridData(false, 'sapMovements'));
        },
        cancelBatch: ticketId => {
            dispatch(requestCancelBatch(ticketId));
        }
    };
};

const consistencyCheckGridConfig = props => {
    return {
        name: 'sapMovements',
        idField: 'movementId',
        selectable: true,
        apiUrl: apiService.logistic.getMovementsDetail(props.ticketId),
        resume: false,
        defaultPageSize: 100,
        pageSizes: [20, 40, 60, 80, 100, 120, 140, 180, 200],
        sortable: {
            defaultSort: 'movementId desc'
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(dataGrid(MovementsGrid, consistencyCheckGridConfig));
