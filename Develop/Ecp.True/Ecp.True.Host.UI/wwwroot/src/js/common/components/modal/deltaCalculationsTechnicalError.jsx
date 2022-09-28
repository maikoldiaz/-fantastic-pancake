import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../services/resourceProvider';
import { dateService } from '../../services/dateService';
import ModalFooter from '../footer/modalFooter.jsx';
import { footerConfigService } from '../../services/footerConfigService';

export class DeltaCalculationsTechnicalError extends React.Component {
    render() {
        return (
            <>
                <div className="ep-modal__content">
                    <p className="m-t-0 m-b-1">{this.props.ticket.errorMessage}</p>
                    <div className="d-flex m-t-5">
                        <label id="lbl_ErrorDetails_Category" className="ep-label d-inline-block m-r-1">{resourceProvider.read('segment')}:</label>
                        <span id="lbl_ErrorDetails_categoryName" className="fw-bold">{this.props.ticket.segment}</span>
                    </div>
                    <div className="d-flex">
                        <label id="lbl_ErrorDetails_Period" className="ep-label d-inline-block m-r-1">{resourceProvider.read('period')}:</label>
                        <span id="lbl_ErrorDetails_StartDate" className="ep-data fw-bold text-caps">{dateService.format(this.props.ticket.ticketStartDate)}</span>
                        <label id="lbl_ErrorDetails_To" className="fw-bold m-x-1">{resourceProvider.read('to')}</label>
                        <span id="lbl_ErrorDetails_EndDate" className="ep-data fw-bold text-caps">{dateService.format(this.props.ticket.ticketFinalDate)}</span>
                    </div>
                </div>
                <ModalFooter config={footerConfigService.getAcceptConfig('deltaCalculationsTechnicalError',
                    { closeModal: true, acceptText: 'accept', acceptClassName: 'ep-btn ep-btn--sm ep-btn--primary' })} />
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
export default connect(mapStateToProps)(DeltaCalculationsTechnicalError);
