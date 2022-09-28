import React from 'react';
import { connect } from 'react-redux';
import { SubmissionError } from 'redux-form';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { executeOwnershipCalculation } from './../actions';
import { showLoader, hideLoader, showError, hideNotification, openMessageModal } from './../../../../common/actions';
import { dateService } from '../../../../common/services/dateService';
import { constants } from '../../../../common/services/constants';
import { serverValidator } from './../../../../common/services/serverValidator';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { refreshGrid } from '../../../../common/components/grid/actions';
import { utilities } from '../../../../common/services/utilities';

class OwnershipCalculationConfirmation extends React.Component {
    constructor() {
        super();
        this.executeOwnershipCalculation = this.executeOwnershipCalculation.bind(this);
    }

    async executeOwnershipCalculation() {
        const ticket = this.props.ticket;
        const ticketInfo = Object.assign({}, {
            ticket: {
                categoryElementId: ticket.categoryElementId,
                startDate: ticket.startDate,
                endDate: ticket.endDate,
                ticketTypeId: constants.TicketType.Ownership
            }
        });

        await this.validate(ticket.categoryElementId, ticket.startDate, ticket.endDate);

        this.props.executeOwnershipCalculation(ticketInfo);
    }

    async validate(categoryElementId, startDate, endDate) {
        const ownership = { categoryElementId, startDate, endDate };

        this.props.showLoader();
        const data = await serverValidator.validateOwnership(ownership);
        this.props.hideLoader();
        if (!data.body) {
            const error = resourceProvider.read('volumetricTicketInprocess');
            const errorTitle = resourceProvider.read('outstandingBalance');
            this.props.showError(error, true, errorTitle);
            throw new SubmissionError({
                _error: error
            });
        }
    }

    render() {
        const ticket = this.props.ticket;
        return (
            <>
                <section className="ep-modal__content">
                    <h1 className="fs-18 fw-bold m-t-0 m-b-8">{resourceProvider.read('confirmExecuteVolumetricBalance')}</h1>
                    <div className="ep-label-wrap">
                        <label className="ep-label">{resourceProvider.read('segment')}</label>
                        <span className="ep-data m-l-2 fw-600" id="span_ownershipCalConfirmation_segment">{ticket.categoryElementName}</span>
                    </div>
                    <div className="ep-label-wrap">
                        <label className="ep-label">{resourceProvider.read('period')}</label>
                        <span className="ep-data m-l-2 fw-600" id="span_ownershipCalConfirmation_period">
                            {`${dateService.capitalize(this.props.ticket.startDate)}
                                ${resourceProvider.read('to')} ${dateService.capitalize(this.props.ticket.endDate)}`}</span>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('ownershipCalculationConfirmation',
                    { onAccept: this.executeOwnershipCalculation, acceptText: 'execute' })} />
            </>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.refreshToggler !== this.props.refreshToggler) {
            this.props.refreshGrid();
            this.props.closeModal();
        }

        if ((prevProps.failureToggler !== this.props.failureToggler) && (!utilities.isNullOrUndefined(this.props.failureToggler))) {
            this.props.closeModal();
            this.props.showTechnicalError(resourceProvider.read('systemErrorMessage'), resourceProvider.read('error'), false);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        ticket: state.ownership.ticket,
        refreshToggler: state.ownership.refreshToggler,
        failureToggler: state.ownership.failureToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        executeOwnershipCalculation: ticketInfo => {
            dispatch(executeOwnershipCalculation(ticketInfo));
        },
        refreshGrid: () => {
            dispatch(refreshGrid('tickets'));
        },
        showLoader: () => {
            dispatch(showLoader());
        },
        hideLoader: () => {
            dispatch(hideLoader());
        },
        showError: (message, showOnModal, title) => {
            dispatch(showError(message, showOnModal, title));
        },
        hideError: () => {
            dispatch(hideNotification());
        },
        showTechnicalError: (message, title, canCancel) => {
            dispatch(openMessageModal(message, { title: title, canCancel: canCancel }));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(OwnershipCalculationConfirmation);
