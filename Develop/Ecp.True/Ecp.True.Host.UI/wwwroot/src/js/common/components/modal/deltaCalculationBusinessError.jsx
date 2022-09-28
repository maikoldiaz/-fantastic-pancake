import React from 'react';
import { connect } from 'react-redux';
import Grid from '../grid/grid.jsx';
import { gridUtils } from '../grid/gridUtils';
import { dataGrid } from '../grid/gridComponent';
import { resourceProvider } from '../../services/resourceProvider';
import { DateCell } from '../grid/gridCells.jsx';
import { apiService } from '../../services/apiService';
import ErrorGridComponent from '../../../modules/transportBalance/operationalDelta/components/errorGrid.jsx';
import { constants } from '../../services/constants';
import { dateService } from '../../services/dateService';
import ModalFooter from '../footer/modalFooter.jsx';
import { footerConfigService } from '../../services/footerConfigService';


export class DeltaCalculationBusinessError extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
    }

    getColumns() {
        const columns = [];
        const date = rowProps => <DateCell {...this.props} {...rowProps} />;
        if (this.props.ticket.ticketTypeId === constants.TicketTypeName.Delta) {
            columns.push(gridUtils.buildTextColumn('identifier', this.props));
        }
        columns.push(gridUtils.buildTextColumn('type', this.props));
        columns.push(gridUtils.buildTextColumn('sourceNode', this.props));
        columns.push(gridUtils.buildTextColumn('destinationNode', this.props));
        columns.push(gridUtils.buildTextColumn('sourceProduct', this.props));
        columns.push(gridUtils.buildTextColumn('destinationProduct', this.props));
        columns.push(gridUtils.buildDecimalColumn('quantity', this.props, 'netQuantity', { type: 'number' }));
        columns.push(gridUtils.buildTextColumn('unit', this.props));
        if (this.props.ticket.ticketTypeId === constants.TicketTypeName.Delta) {
            columns.push(gridUtils.buildDateColumn('date', this.props, date, 'date'));
        }
        return columns;
    }


    render() {
        return (
            <>
                <div className="ep-modal__content p-x-6">
                    {this.props.ticket.ticketTypeId === constants.TicketTypeName.Delta &&
                        <div className="d-flex">
                            <label id="lbl_ErrorDetails_Category" className="ep-label d-inline-block m-r-1">{resourceProvider.read('segment')}:</label>
                            <span id="lbl_ErrorDetails_categoryName" className="fw-bold">{this.props.ticket.segment}</span>
                        </div>
                    }
                    {this.props.ticket.ticketTypeId === constants.TicketTypeName.OfficialDelta &&
                        <div>
                            <div className="d-flex">
                                <label id="lbl_ErrorDetails_Category" className="ep-label d-inline-block m-r-1">{resourceProvider.read('node')}:</label>
                                <span id="lbl_ErrorDetails_categoryName" className="fw-bold">{this.props.ticket.nodeName}</span>
                            </div>
                            <div className="d-flex">
                                <label id="lbl_delta_ErrorDetails_Category" className="ep-label d-inline-block m-r-1">{resourceProvider.read('nodeExecutionDate')}:</label>
                                <span id="lbl_delta_ErrorDetails_CategoryName" className="fw-bold">{dateService.capitalize(this.props.ticket.executionDate)}</span>
                            </div>
                        </div>
                    }
                    <div className="ep-section__body ep-section__body--hf p-a-0">
                        <Grid name={this.props.name} columns={this.getColumns()} className="ep-table--row-sm ep-table--brless ep-table--expandable ep-table--mh350" />
                    </div>
                </div>
                <ModalFooter config={footerConfigService.getAcceptConfig('deltaCalculationBusinessError',
                    {
                        closeModal: true, acceptText: 'accept', acceptType: 'button',
                        acceptClassName: 'ep-btn ep-btn--sm ep-btn--primary'
                    })} />
            </>
        );
    }
}


/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        ticket: state.cutoff.ticketInfo.ticket
    };
};

/* istanbul ignore next */
const mapDispatchToProps = () => {
    return {
    };
};


/* istanbul ignore next */
const gridConfig = props => {
    return {
        name: 'operationalDeltaBusinessError',
        idField: '',
        apiUrl: apiService.ticket.getDeltaExceptionsDetails(props.ticket.ticketTypeId === constants.TicketTypeName.Delta
            ? props.ticket.ticketId : props.ticket.deltaNodeId, props.ticket.ticketTypeId),
        showPagination: false,
        filterable: false,
        sortable: false,
        expandable: {
            is: true,
            component: ErrorGridComponent
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(DeltaCalculationBusinessError, gridConfig));

