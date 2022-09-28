import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dateService } from '../../../../common/services/dateService';
import { saveDeltaCalculation } from './../actions';
import { closeModal, showError, hideNotification, showLoader, hideLoader, openMessageModal } from '../../../../common/actions';
import { constants } from '../../../../common/services/constants';
import { navigationService } from '../../../../common/services/navigationService';
import { ticketValidator } from '../ticketValidationService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { utilities } from '../../../../common/services/utilities';

class ConfirmDeltaCalculation extends React.Component {
    constructor() {
        super();
        this.saveDeltaCalculation = this.saveDeltaCalculation.bind(this);
    }

    async saveDeltaCalculation() {
        await ticketValidator.validateCutoffDeltaCalculation(this.props, this.props.deltaTicket.segment.elementId, true);
        const deltaTicket = {
            ticket: {
                startDate: this.props.deltaTicket.startDate,
                endDate: this.props.deltaTicket.endDate,
                categoryElementId: this.props.deltaTicket.segment.elementId,
                ticketTypeId: constants.TicketType.Delta
            }
        };
        this.props.saveDeltaCalculation(deltaTicket);
    }

    render() {
        const deltaTicket = this.props.deltaTicket;
        return (
            <>
                <div className="ep-modal__content">
                    <p>
                        <span className="fw-bold">
                            {resourceProvider.read('deltaCalculationConfirmationMessage')}
                        </span>
                    </p>
                    <div className="d-flex m-t-5">
                        <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('segment')}:</label>
                        <span className="fw-bold">{deltaTicket.segment.name}</span>
                    </div>
                    <div className="d-flex">
                        <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('period')}:</label>
                        <span className="fw-bold m-r-1">
                            {dateService.capitalize(deltaTicket.startDate)}</span>
                        <span className="ep-label m-r-1">
                            {resourceProvider.read('to')}</span>
                        <span className="fw-bold">
                            {dateService.capitalize(deltaTicket.endDate)}</span>
                    </div>
                </div>
                <ModalFooter config={footerConfigService.getCommonConfig('confirmDeltaCalculation',
                    { onAccept: this.saveDeltaCalculation, acceptText: 'accept' })} />
            </>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.refreshDeltaCalculationGridToggler !== this.props.refreshDeltaCalculationGridToggler) {
            this.props.closeModal();
            navigationService.navigateTo(`manage`);
        }

        if ((prevProps.failureToggler !== this.props.failureToggler) && (!utilities.isNullOrUndefined(this.props.failureToggler))) {
            this.props.showTechnicalError(resourceProvider.read('systemErrorMessage'), resourceProvider.read('error'), false);
            navigationService.navigateTo(`manage`);
        }
    }
}


/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        deltaTicket: state.operationalDelta.deltaTicket,
        refreshDeltaCalculationGridToggler: state.operationalDelta.refreshDeltaCalculationGridToggler,
        failureToggler: state.operationalDelta.failureToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        saveDeltaCalculation: deltaTicket => {
            dispatch(saveDeltaCalculation(deltaTicket));
        },
        closeModal: () => {
            dispatch(closeModal());
        },
        showError: (message, title) => {
            dispatch(showError(message, false, title));
        },
        hideError: () => {
            dispatch(hideNotification());
        },
        showLoader: () => {
            dispatch(showLoader());
        },
        hideLoader: () => {
            dispatch(hideLoader());
        },
        showTechnicalError: (message, title, canCancel) => {
            dispatch(openMessageModal(message, { title: title, canCancel: canCancel }));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(ConfirmDeltaCalculation);
