import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { DateCell } from '../../../../common/components/grid/gridCells.jsx';
import { apiService } from '../../../../common/services/apiService';
import { dateService } from '../../../../common/services/dateService';
import { showWarning, openModal, hideNotification } from './../../../../common/actions';
import { requestPendingMovements } from './../actions';
import { utilities } from '../../../../common/services/utilities';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

export class PendingMovementsGrid extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
        this.getPendingMovements = this.getPendingMovements.bind(this);
    }

    getColumns() {
        const columns = [];
        const date = rowProps => <DateCell {...this.props} {...rowProps} />;
        columns.push(gridUtils.buildTextColumn('movementId', this.props, null, 'movementId', { type: 'text' }));
        columns.push(gridUtils.buildTextColumn('movementType', this.props));
        columns.push(gridUtils.buildTextColumn('sourceNode', this.props));
        columns.push(gridUtils.buildTextColumn('destinationNode', this.props));
        columns.push(gridUtils.buildTextColumn('sourceProduct', this.props, null, 'prodOrigen'));
        columns.push(gridUtils.buildTextColumn('destinationProduct', this.props, null, 'prodDestino'));
        columns.push(gridUtils.buildDecimalColumn('amount', this.props, 'netQuantity', { type: 'decimal' }));
        columns.push(gridUtils.buildTextColumn('unit', this.props));
        columns.push(gridUtils.buildDateColumn('operationalDate', this.props, date, 'dateOperational'));
        columns.push(gridUtils.buildTextColumn('action', this.props));
        return columns;
    }

    getPendingMovements(apiUrl) {
        const ticket = {
            startDate: this.props.deltaTicket.startDate,
            endDate: this.props.deltaTicket.endDate,
            categoryElementId: this.props.deltaTicket.segment.elementId
        };
        this.props.getPendingMovements(apiUrl, ticket, this.props.name);
    }

    hasMovementsAndInventories() {
        return (this.props.inventories.length > 0 || this.props.movements.length > 0);
    }

    showWarning() {
        this.props.showWarning(resourceProvider.read('noInventoriesMovementsForDeltaValMsgDesc'), resourceProvider.read('noInventoriesMovementsForDeltaValMsgTitle'));
    }

    render() {
        return (
            <div className="ep-section">
                <header className="ep-section__header">
                    <h1 className="ep-section__title m-y-3">
                        {resourceProvider.readFormat('pendingMovementsGridMessage', [
                            dateService.capitalize(this.props.deltaTicket.startDate),
                            dateService.capitalize(this.props.deltaTicket.endDate),
                            this.props.deltaTicket.segment.name
                        ])}
                    </h1>
                </header>
                <div className="ep-section__body ep-section__body--hf p-a-0">
                    <Grid name={this.props.name} columns={this.getColumns()} requestGridData={this.getPendingMovements} className="ep-table--row-sm ep-table--wizard" />
                </div>
                <SectionFooter floatRight={true} config={footerConfigService.getCommonConfig('pendingMovementsGrid',
                    {
                        onAccept: this.props.openDeltaCalculationConfirmModal, onCancel: this.props.cancelDeltaCalculation,
                        acceptText: 'next', acceptClassName: 'ep-btn', acceptType: 'button', disableAccept: !this.hasMovementsAndInventories()
                    })} />
            </div>
        );
    }

    componentWillUnmount() {
        this.props.hideWarning();
    }

    componentDidUpdate() {
        if (!this.hasMovementsAndInventories()) {
            this.showWarning();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        deltaTicket: state.operationalDelta.deltaTicket,
        inventories: state.grid.pendingInventories ? state.grid.pendingInventories.items : [],
        movements: state.grid.pendingMovements ? state.grid.pendingMovements.items : []
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        showWarning: (message, title) => {
            dispatch(showWarning(message, false, title));
        },
        hideWarning: () => {
            dispatch(hideNotification());
        },
        cancelDeltaCalculation: () => {
            ownProps.goToStep('operationalDelta', 1);
        },
        getPendingMovements: (apiUrl, deltaTicket, name) => {
            dispatch(requestPendingMovements(apiUrl, deltaTicket, name));
        },
        openDeltaCalculationConfirmModal: () => {
            dispatch(openModal('confirmDeltaCalculation', '', 'confirmation'));
        }
    };
};

/* istanbul ignore next */
const gridConfig = () => {
    return {
        name: 'pendingMovements',
        odata: false,
        apiUrl: apiService.operationalDelta.getPendingMovements()
    };
};

/* istanbul ignore next */
export default dataGrid(connect(mapStateToProps, mapDispatchToProps, utilities.merge)(PendingMovementsGrid), gridConfig);
