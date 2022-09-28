import React from 'react';
import { connect } from 'react-redux';
import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import TicketsGrid from '../../cutOff/components/ticketsGrid.jsx';
import { openModal, openMessageModal } from '../../../../common/actions';
import { initializeTicketError } from '../../cutOff/actions';
import { requestConciliation, requestOwnershipNodeData, saveSelectedTicket, requestLastOperationalTicket } from '../actions';
import { refreshSilent } from '../../../../common/components/grid/actions';

export class OwnershipTickets extends React.Component {
    constructor(props) {
        super(props);

        this.enableExecute = this.enableExecute.bind(this);
        this.enableDetail = this.enableDetail.bind(this);
        this.onExecute = this.onExecute.bind(this);
        this.onDetail = this.onDetail.bind(this);
    }

    enableExecute(ticket) {
        return utilities.matchesAny(ticket.state, [
            constants.StatusType.FINALIZED, constants.StatusType.CONCILIATIONFAILED
        ]) && this.props.lastOperationalConciliationTicket.some(lastTicket => lastTicket.ticketId === ticket.ticketId);
    }

    enableDetail(ticket) {
        return ticket.state === constants.StatusType.CONCILIATIONFAILED;
    }

    onExecute(ticket) {
        this.props.requestOwnershipNode(ticket);
    }

    onDetail(ticket) {
        const title = resourceProvider.read('conciliationErrorTitle');
        this.props.showError(ticket, title, 'ep-modal--sm', 'text-unset');
    }

    getInvalidOwnershipNodes() {
        const invalidTransferPoint = this.props.ownershipNodesData
            .filter(ownershipNode => (
                ownershipNode.isTransferPoint) &&
                utilities.matchesAny(ownershipNode.state, [
                    constants.OwnershipNodeStatusType.APPROVED, constants.OwnershipNodeStatusType.SUBMITFORAPPROVAL
                ])
            );

        return invalidTransferPoint;
    }

    render() {
        return (
            <TicketsGrid {...this.props}
                execute
                detail
                onExecute={this.onExecute}
                onDetail={this.onDetail}
                enableExecute={this.enableExecute}
                enableDetail={this.enableDetail}
                executeTitle="manualConciliation"
                detailTitle="viewConciliationError"
                actionCellWith={180} />
        );
    }

    componentDidMount() {
        this.props.requestLastOperationalTicket();
    }

    componentDidUpdate(prevProps) {
        if ((prevProps.conciliationSuccessToggler !== this.props.conciliationSuccessToggler) && (!utilities.isNullOrUndefined(this.props.conciliationSuccessToggler))) {
            this.props.refreshGrid();
        }
        if ((prevProps.conciliationErrorToggler !== this.props.conciliationErrorToggler) && (!utilities.isNullOrUndefined(this.props.conciliationErrorToggler))) {
            this.props.confirmModal(resourceProvider.read('conciliationErrorResponse'), resourceProvider.read('error'), false);
        }
        if ((prevProps.refreshedGridData !== this.props.refreshedGridData) && (!utilities.isNullOrUndefined(this.props.refreshedGridData))) {
            this.props.requestLastOperationalTicket();
        }
        if ((prevProps.ownershipNodesSuccessToggler !== this.props.ownershipNodesSuccessToggler) && (!utilities.isNullOrUndefined(this.props.ownershipNodesSuccessToggler))) {
            const invalidOwnershipNodes = this.getInvalidOwnershipNodes();
            if (invalidOwnershipNodes.length === 0) {
                this.props.requestConciliation(this.props.selectedTicket);
            } else {
                this.props.confirmModal(resourceProvider.read('conciliationOwnershipNodeStatusError'), resourceProvider.read('error'), false);
            }
        }
        if ((prevProps.ownershipNodesErrorToggler !== this.props.ownershipNodesErrorToggler) && (!utilities.isNullOrUndefined(this.props.ownershipNodesErrorToggler))) {
            this.props.confirmModal(resourceProvider.read('systemErrorMessage'), resourceProvider.read('error'), false);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        conciliationSuccessToggler: state.ownership.conciliationSuccessToggler,
        conciliationErrorToggler: state.ownership.conciliationErrorToggler,
        ownershipNodesSuccessToggler: state.ownership.ownershipNodesSuccessToggler,
        ownershipNodesErrorToggler: state.ownership.ownershipNodesErrorToggler,
        refreshedGridData: state.ownership.refreshedGridData,
        ownershipNodesData: state.ownership.ownershipNodesData || [],
        selectedTicket: state.ownership.selectedTicket,
        lastOperationalConciliationTicket: state.ownership.lastOperationalConciliationTicket || []
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        showError: (ticket, title, className, titleClassName = '') => {
            dispatch(initializeTicketError(ticket));
            dispatch(openModal('showError', '', title, className, titleClassName));
        },
        refreshGrid: () => {
            dispatch(refreshSilent('tickets'));
        },
        confirmModal: (message, title, canCancel) => {
            dispatch(openMessageModal(message, {
                title: title,
                canCancel: canCancel
            }));
        },
        requestLastOperationalTicket: () => {
            dispatch(requestLastOperationalTicket(['PROCESSED', 'PROCESSING', 'CONCILIATIONFAILED']));
        },
        requestOwnershipNode: ticket => {
            dispatch(saveSelectedTicket(ticket));
            dispatch(requestOwnershipNodeData(ticket.ticketId));
        },
        requestConciliation: ticket => {
            const data = {
                ticketId: ticket.ticketId
            };
            dispatch(requestConciliation(data));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(OwnershipTickets);
