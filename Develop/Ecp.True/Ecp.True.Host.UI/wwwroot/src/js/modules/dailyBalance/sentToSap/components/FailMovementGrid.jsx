import * as React from 'react';
import { connect } from 'react-redux';
import { closeModal, openMessageModal } from '../../../../common/actions.js';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import Grid from '../../../../common/components/grid/grid.jsx';
import { DateCell, UploadStatusCell } from '../../../../common/components/grid/gridCells.jsx';
import { dataGrid } from '../../../../common/components/grid/gridComponent.js';
import { gridUtils } from '../../../../common/components/grid/gridUtils.js';
import { apiService } from '../../../../common/services/apiService.js';
import { footerConfigService } from '../../../../common/services/footerConfigService.js';
import { navigationService } from '../../../../common/services/navigationService.js';
import { resourceProvider } from '../../../../common/services/resourceProvider.js';
import { requestSaveTicket } from '../actions.js';


class FailMovementGrid extends React.Component {
    constructor() {
        super();
        this.navigateToFirstStep = this.navigateToFirstStep.bind(this);
        this.createBatch = this.createBatch.bind(this);
        this.handleNextStep = this.handleNextStep.bind(this);
        this.getStatus = this.getStatus.bind(this);
    }


    getStatus(row) {
        return row.state;
    }

    getColumns() {
        const columns = [];
        const ticketStatus = gridProps => <UploadStatusCell {...this.props} {...gridProps} getStatus={this.getStatus} noLocalize={this.hasStatusLocalize} />;

        const dateCell = rowProps => <DateCell {...this.props} {...rowProps} ignoreMax={true} />;

        columns.push(gridUtils.buildTextColumn('state', this.props, ticketStatus, 'state'));
        columns.push(gridUtils.buildTextColumn('description', this.props, null, 'description'));
        columns.push(gridUtils.buildTextColumn('movementType', this.props, null, 'operationType'));
        columns.push(gridUtils.buildTextColumn('sourceCenter', this.props, null, 'sourceCenter'));
        columns.push(gridUtils.buildTextColumn('sourceStorage', this.props, null, 'sourceStorageLocation'));
        columns.push(gridUtils.buildTextColumn('sourceProduct', this.props, null, 'sourceProduct'));
        columns.push(gridUtils.buildTextColumn('destinationCenter', this.props, null, 'destinationCenter'));
        columns.push(gridUtils.buildTextColumn('destinationStorage', this.props, null, 'destinationStorageLocation'));
        columns.push(gridUtils.buildTextColumn('destinationProduct', this.props, null, 'destinationProduct'));
        columns.push(gridUtils.buildDecimalColumn('ownershipVolume', this.props, null, 'propertyVolume'));
        columns.push(gridUtils.buildTextColumn('units', this.props, null, 'unit'));
        columns.push(gridUtils.buildDateColumn('operationalDate', this.props, dateCell, 'operationalDate'));
        columns.push(gridUtils.buildTextColumn('movementId', this.props, null, 'movementId'));
        columns.push(gridUtils.buildTextColumn('costCenter', this.props, null, 'costCenter'));
        columns.push(gridUtils.buildTextColumn('gmCode', this.props, null, 'sapTransactionCode'));
        columns.push(gridUtils.buildTextColumn('documentNumber', this.props, null, 'documentNumber'));
        columns.push(gridUtils.buildNumberColumn('position', this.props, null, 'position'));
        columns.push(gridUtils.buildNumberColumn('order', this.props, null, 'order'));
        columns.push(gridUtils.buildDateColumn('accountingDate', this.props, dateCell, 'dateAccounting'));

        return columns;
    }

    handleNextStep() {
        const { failMovementSelected = [], gridItemsLength = 0, openModal } = this.props;
        const failMovementNotSelected = failMovementSelected.length === 0 && gridItemsLength > 0;
        const movementStepMessage = failMovementNotSelected ? 'failMovementsMessage' : 'transformMovementStepMessage';
        openModal(
            resourceProvider.read(movementStepMessage),
            {
                acceptAction: () => {
                    this.createBatch();
                    this.props.closeModal();
                },
                canCancel: failMovementNotSelected
            }
        );
    }

    createBatch() {
        const { ticket, failMovementSelected = [] } = this.props;

        const failLogisticMovementIds = failMovementSelected.map(l => l.logisticMovementId);

        const ticketBody = {
            Ticket: ticket,
            FailedLogisticsMovements: failLogisticMovementIds
        };

        this.props.saveTicket(ticketBody);
        this.props.onNext(this.props.config.wizardName);
    }

    navigateToFirstStep() {
        navigationService.navigateTo('create');
    }

    render() {
        return (
            <>
                <div className="ep-section__body ep-section__body--hf p-a-0">
                    <Grid name={this.props.name} columns={this.getColumns()} className="ep-table--row-sm ep-table--wizard" />
                </div>
                <SectionFooter floatRight={true} config={footerConfigService.getCommonConfig('sapProcess', {
                    onCancel: this.props.cancelFailMovements,
                    onAccept: this.handleNextStep,
                    acceptText: 'next',
                    acceptClassName: 'ep-btn'
                })} />
            </>
        );
    }
}


const FailMovementGridFunc = props => {
    const parseDate = date => {
        const month = date.getMonth() + 1;
        return `${month > 9 ? month : `0${month}`}/${date.getDate()}/${date.getFullYear()}`;
    };

    const queryParams = {
        categoryElementId: props.ticket.categoryElementId,
        startDate: parseDate(props.ticket.startDate),
        endDate: parseDate(props.ticket.endDate),
        scenarioTypeId: props.ticket.scenarioTypeId,
        nodes: props.ticket.ticketNodes,
        ownerId: props.ticket.ownerId
    };

    return {
        name: 'failSapMovements',
        idField: 'logisticMovementId',
        selectable: true,
        apiUrl: apiService.logistic.getFailMovements(queryParams),
        resume: false,
        sortable: {
            defaultSort: 'logisticMovementId desc'
        }
    };
};

/* istanbul ignore next */
const mapStateToProps = state => {
    const { failSapMovements = { selection: [], items: [] } } = state.grid;
    return {
        ticket: state.sendToSap.ticket,
        failMovementSelected: failSapMovements.selection,
        gridItemsLength: failSapMovements.items.length
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        cancelFailMovements: () => {
            ownProps.goToStep('sendToSapWizard', 1);
        },
        saveTicket: ticket => {
            dispatch(requestSaveTicket(ticket));
        },
        openModal: (message, options) => {
            dispatch(openMessageModal(message, options));
        },
        closeModal: () => {
            dispatch(closeModal());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(FailMovementGrid, FailMovementGridFunc));
