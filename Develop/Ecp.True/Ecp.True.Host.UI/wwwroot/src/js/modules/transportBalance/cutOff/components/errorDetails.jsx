import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { dateService } from '../../../../common/services/dateService';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { constants } from '../../../../common/services/constants';

export class ErrorDetails extends React.Component {
    render() {
        let title = '';
        let firstPropertyTitle = resourceProvider.read('category');
        let firstPropertyValue = this.props.ticket.categoryName;
        let secondPropertyTitle = resourceProvider.read('element');
        let secondPropertyValue = this.props.ticket.segment;

        if (this.props.ticket.ticketTypeId === 'Cutoff') {
            title = resourceProvider.read('ticketErrorDetailHeader');
        } else if (this.props.ticket.ticketTypeId === 'Logistics' || this.props.ticket.ticketTypeId === constants.CalculationType.OfficialLogistics) {
            title = resourceProvider.read('logisticsErrorDetailsTitle');
            firstPropertyTitle = resourceProvider.read('segment');
            firstPropertyValue = this.props.ticket.segment;
            secondPropertyTitle = resourceProvider.read('owner');
            secondPropertyValue = this.props.ticket.ownerName;
        } else if (this.props.ticket.ticketTypeId === constants.CalculationType.LogisticMovements) {
            title = resourceProvider.read('logisticMovementsErrorDetailsTitle');
            firstPropertyTitle = resourceProvider.read('stage');
            firstPropertyValue = this.props.ticket.scenarioName;
            secondPropertyTitle = resourceProvider.read('segment');
            secondPropertyValue = this.props.ticket.segment;
        } else {
            title = this.props.ticket.state === constants.StatusType.CONCILIATIONFAILED
                ? resourceProvider.read('conciliationErrorMessage')
                : resourceProvider.read('ownershipCalculationErrorDetailMessage');
        }
        return (
            <>
                <div className="ep-modal__content">
                    <p className="m-t-0">{title}:</p>
                    <div className="d-flex">
                        <label id="lbl_ErrorDetails_Category" className="ep-label d-inline-block m-r-1">{firstPropertyTitle}:</label>
                        <span id="lbl_ErrorDetails_categoryName" className="ep-data fw-bold">{firstPropertyValue}</span>
                    </div>
                    {this.props.ticket.ticketTypeId === constants.CalculationType.OfficialLogistics &&
                        <div className="d-flex">
                            <label id="lbl_ErrorDetails_Element" className="ep-label d-inline-block m-r-1">{resourceProvider.read('node')}:</label>
                            <span id="lbl_ErrorDetails_ElementName" className="ep-data fw-bold">{this.props.ticket.nodeName}</span>
                        </div>
                    }
                    <div className="d-flex">
                        <label id="lbl_ErrorDetails_Element" className="ep-label d-inline-block m-r-1">{secondPropertyTitle}:</label>
                        <span id="lbl_ErrorDetails_ElementName" className="ep-data fw-bold">{secondPropertyValue}</span>
                    </div>
                    <div className="d-flex">
                        <label id="lbl_ErrorDetails_Period" className="ep-label d-inline-block m-r-1">{resourceProvider.read('period')}:</label>
                        <span id="lbl_ErrorDetails_StartDate" className="ep-data fw-bold text-caps">{dateService.format(this.props.ticket.ticketStartDate)}</span>
                        <label id="lbl_ErrorDetails_To" className="fw-bold m-x-1">{resourceProvider.read('to')}</label>
                        <span id="lbl_ErrorDetails_EndDate" className="ep-data fw-bold text-caps">{dateService.format(this.props.ticket.ticketFinalDate)}</span>
                    </div>
                    {this.props.ticket.ticketTypeId === constants.CalculationType.LogisticMovements &&
                        <div className="d-flex">
                            <label id="lbl_ErrorDetails_Element" className="ep-label d-inline-block m-r-1">{resourceProvider.read('owner')}:</label>
                            <span id="lbl_ErrorDetails_ElementName" className="ep-data fw-bold">{this.props.ticket.ownerName}</span>
                        </div>
                    }
                    <div className="d-flex m-t-5">
                        <label id="lbl_ErrorDetails_Error" className="ep-label d-inline-block m-r-1">{resourceProvider.read('error')}:</label>
                        <span id="lbl_ErrorDetails_ErrorMessage" className="ep-data fw-bold">{this.props.ticket.errorMessage}</span>
                    </div>
                </div>
                <ModalFooter config={footerConfigService.getAcceptConfig('errorDetails', {
                    closeModal: true, acceptText: 'accept', acceptClassName: 'ep-btn ep-btn--sm ep-btn--primary'
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
export default connect(mapStateToProps)(ErrorDetails);
