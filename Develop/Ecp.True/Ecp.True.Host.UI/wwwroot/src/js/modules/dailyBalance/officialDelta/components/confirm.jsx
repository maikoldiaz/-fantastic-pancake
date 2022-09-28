import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { saveOfficialDelta, setIsValid } from '../actions.js';
import { dateService } from '../../../../common/services/dateService';
import { ticketValidator } from '../ticketValidationService';
import { constants } from '../../../../common/services/constants';
import { hideNotification, showLoader, hideLoader, showError, showErrorComponent, openMessageModal } from '../../../../common/actions';
import { navigationService } from '../../../../common/services/navigationService';
import { utilities } from '../../../../common/services/utilities';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';

export class ConfirmOfficialDelta extends React.Component {
    constructor() {
        super();
        this.confirm = this.confirm.bind(this);
    }

    async confirm() {
        await ticketValidator.validateOfficialDeltaInProgress(this.props, this.props.ticket.categoryElementId, true);
        const officialDeltaTicket = {
            ticket: {
                startDate: this.props.ticket.startDate,
                endDate: this.props.ticket.endDate,
                categoryElementId: this.props.ticket.categoryElementId,
                ticketTypeId: constants.TicketType.OfficialDelta
            }
        };
        await ticketValidator.validatePreviousOfficialPeriod(this.props, officialDeltaTicket.ticket, true);
        this.props.save(officialDeltaTicket);
    }

    render() {
        const ticket = this.props.ticket;
        return (
            <>
                <div className="ep-modal__content">
                    <p><span className="fw-bold">{resourceProvider.read('officialDeltaConfirm')}</span></p>
                    <div className="d-flex">
                        <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('segment')}:</label>
                        <span className="fw-bold">{ticket.name}</span>
                    </div>
                    <div className="d-flex">
                        <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('period')}:</label>
                        <span className="fw-bold m-r-1">{dateService.capitalize(ticket.startDate)}</span>
                        <span className="fw-sb fc-label m-r-1">{resourceProvider.read('to')}</span>
                        <span className="fw-bold">{dateService.capitalize(ticket.endDate)}</span>
                    </div>
                </div>
                <ModalFooter config={footerConfigService.getCommonConfig('confirmOfficialDeltaTicket',
                    { onAccept: this.confirm, acceptText: 'accept' })} />
            </>
        );
    }

    componentDidUpdate(prevProps) {
        if (this.props.receiveToggler !== prevProps.receiveToggler) {
            this.props.closeModal();
            navigationService.navigateToModule('officialdelta/manage');
        }

        if ((prevProps.failureToggler !== this.props.failureToggler) && (!utilities.isNullOrUndefined(this.props.failureToggler))) {
            this.props.showTechnicalError(resourceProvider.read('systemErrorMessage'), resourceProvider.read('error'), false);
            navigationService.navigateToModule('officialdelta/manage');
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        ticket: state.officialDelta.ticket,
        receiveToggler: state.officialDelta.receiveToggler,
        failureToggler: state.officialDelta.failureToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        save: ticket => {
            dispatch(saveOfficialDelta(ticket));
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
        setIsValid: valid => {
            dispatch(setIsValid(valid));
        },
        showError: (message, title) => {
            dispatch(showError(message, false, title));
        },
        showErrorComponent: (component, title) => {
            dispatch(showErrorComponent(component, false, title));
        },
        showTechnicalError: (message, title, canCancel) => {
            dispatch(openMessageModal(message, { title: title, canCancel: canCancel }));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(ConfirmOfficialDelta);
