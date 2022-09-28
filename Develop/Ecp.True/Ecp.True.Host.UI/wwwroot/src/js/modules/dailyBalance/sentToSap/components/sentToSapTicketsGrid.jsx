import React from 'react';
import { connect } from 'react-redux';
import { utilities } from '../../../../common/services/utilities';
import { navigationService } from './../../../../common/services/navigationService';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { openMessageModal, openModal } from '../../../../common/actions';
import { constants } from '../../../../common/services/constants';
import TicketsGrid from '../../../transportBalance/cutOff/components/ticketsGrid.jsx';
import { saveConfirmWizardData, requestNodesInTicket } from '../actions';

class SentToSapTicketsGrid extends React.Component {
    constructor() {
        super();

        this.enableContinue = this.enableContinue.bind(this);
        this.onInfo = this.onInfo.bind(this);
        this.onContinue = this.onContinue.bind(this);
    }

    enableContinue(row) {
        return row.state === constants.StatusType.VISUALIZATION;
    }

    onInfo(ticket) {
        this.props.getNodesInTicket(ticket.ticketId);
    }

    onContinue(ticket) {
        const confirmData = {
            ticketId: ticket.ticketId
        };
        this.props.saveConfirmWizardData(confirmData);
        navigationService.navigateToModule('senttosap/create');
    }

    render() {
        return (
            <>
                <TicketsGrid {...this.props}
                    info
                    continue
                    enableInfo
                    enableContinue={this.enableContinue}
                    onInfo={this.onInfo}
                    onContinue={this.onContinue}
                    continueTitle="continueWithWizard"
                    infoTitle="nodes"
                    viewTitle="viewDetail" />
            </>
        );
    }

    componentDidUpdate(prevProps) {
        if (this.props.receiveToggler !== prevProps.receiveToggler) {
            this.props.showModal(resourceProvider.read('nodes'), 'nodesInTicket', 'ep-modal--sm');
        }

        if ((prevProps.failureToggler !== this.props.failureToggler) && (!utilities.isNullOrUndefined(this.props.failureToggler))) {
            this.props.showTechnicalError(resourceProvider.read('systemErrorMessage'), resourceProvider.read('error'), false);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        receiveToggler: state.sendToSap.nodesInTicketToggler,
        failureToggler: state.sendToSap.failureNodesInTicketToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        saveConfirmWizardData: data => {
            dispatch(saveConfirmWizardData(data));
        },
        getNodesInTicket: ticketId => {
            dispatch(requestNodesInTicket(ticketId));
        },
        showTechnicalError: (message, title, canCancel) => {
            dispatch(openMessageModal(message, { title: title, canCancel }));
        },
        showModal: (title, popUpName, className) => {
            dispatch(openModal(popUpName, '', title, className));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(SentToSapTicketsGrid);
