import { constants } from './../../../common/services/constants';
import { dateService } from './../../../common/services/dateService';
import {
    INIT_MOVEMENTS_CONFIRMATION,
    CONFIRM_SENT_SAP,
    INIT_SENTTOSAP_PROCESS,
    COMPLETE_SENTTOSAP_PROCESS,
    RECEIVE_FAILURE_SENTTOSAP_PROCESS,
    SAVE_CONFIRM_WIZARD_DATA,
    RESET_CONFIRM_WIZARD_DATA,
    RECEIVE_NODES_IN_TICKET,
    RECEIVE_FAILURE_NODES_IN_TICKET,
    RECEIVE_GET_TICKET,
    RECEIVE_FAILURE_GET_TICKET,
    CONFIRM_CANCEL_BATCH,
    COMPLETE_CANCEL_BATCH_PROCESS,
    RECEIVE_FAILURE_CANCEL_BATCH_PROCESS,
    RECEIVE_SAP_OFFICIAL_PERIODS,
    RESET_SAP_OFFICIAL_PERIODS,
    CLEAR_SAP_SEARCH_NODES,
    RECEIVE_SAP_SEARCH_NODES,
    RESET_SAP_FORM_DATA,
    SET_SAP_TICKET_INFO,
    RECEIVE_SAP_TICKET_VALIDATIONS,
    RESET_SAP_TICKET_VALIDATIONS,
    RECEIVE_SAP_TICKET,
    RECEIVE_SAP_TICKET_FAILURE,
    COMPLETE_FORWARD_PROCESS
} from './actions';

const sendToSapInitialState = {
    ticket: {
        initialDate: null
    },
    inventoriesValidations: { [constants.InventoriesValidations.InitialInventories]: [], [constants.InventoriesValidations.NewNodes]: [] },
    officialPeriods: {
        defaultYear: null,
        officialPeriods: []
    },
    searchedNodes: [],
    scenario: null,
    segment: null,
    validations: []
};

export const sendToSap = (state = sendToSapInitialState, action = {}) => {
    switch (action.type) {
    case SAVE_CONFIRM_WIZARD_DATA: {
        return Object.assign({}, state, { confirmWizard: action.data });
    }
    case RESET_CONFIRM_WIZARD_DATA: {
        return Object.assign({}, state, { confirmWizard: null });
    }
    case RECEIVE_NODES_IN_TICKET: {
        const ticket = action.tickets.value[0];

        return Object.assign({}, state, {
            nodesInTicketToggler: !state.nodesInTicketToggler,
            nodesInTicket: ticket.ticketNodes.map(ticketNode => ticketNode.node)
        });
    }
    case RECEIVE_FAILURE_NODES_IN_TICKET: {
        return Object.assign({}, state, { failureNodesInTicketToggler: !state.failureNodesInTicketToggler });
    }
    case INIT_SENTTOSAP_PROCESS:
        return Object.assign({}, state, {
            confirmSentToSapToggler: false,
            confirmCancelBatchToggler: false
        });
    case INIT_MOVEMENTS_CONFIRMATION:
        return Object.assign({},
            state,
            {
                name: action.name,
                [action.name]: Object.assign({}, state[action.name] ? state[action.name] : {}, {
                    selectedMovements: action.selectedMovements,
                    countTotalMovements: action.countTotalMovements
                })
            });
    case CONFIRM_SENT_SAP:
        return Object.assign({}, state, { confirmSentToSapToggler: !state.confirmSentToSapToggler });
    case COMPLETE_SENTTOSAP_PROCESS:
        return Object.assign({}, state, { receiveToggler: !state.receiveToggler });
    case RECEIVE_FAILURE_SENTTOSAP_PROCESS: {
        return Object.assign({},
            state,
            {
                saveSentToSapFailureToggler: !state.saveSentToSapFailureToggler,
                saveSentToSapFailedErrorMessage: action.errorMessage,
                saveSentToSapFailed: true
            });
    }
    case RECEIVE_GET_TICKET: {
        const ticket = action.ticket.value[0];
        return Object.assign({},
            state,
            {
                ticket: Object.assign({}, ticket, {
                    segment: ticket.segment,
                    ticketStartDate: ticket.ticketStartDate,
                    ticketFinalDate: ticket.ticketFinalDate,
                    ownerName: ticket.ownerName,
                    scenarioName: ticket.scenarioName
                })
            });
    }
    case RECEIVE_FAILURE_GET_TICKET: {
        return Object.assign({},
            state,
            {
                getTicketFailure: !state.getTicketFailure,
                getTicketFailed: true
            });
    }
    case CONFIRM_CANCEL_BATCH: {
        return Object.assign({},
            state,
            {
                confirmCancelBatchToggler: !state.confirmCancelBatchToggler
            });
    }
    case COMPLETE_CANCEL_BATCH_PROCESS:
        return Object.assign({}, state, { receiveCancelBatchToggler: !state.receiveCancelBatchToggler });
    case RECEIVE_FAILURE_CANCEL_BATCH_PROCESS: {
        return Object.assign({},
            state,
            {
                saveCancelBatchFailureToggler: !state.saveCancelBatchFailureToggler,
                saveCancelBatchFailedErrorMessage: action.errorMessage,
                saveCancelBatchFailed: true
            });
    }
    case '@@redux-form/CHANGE': {
        if (action.meta.form === 'initSapFormTicket') {
            if (action.meta.field === 'scenario') {
                return Object.assign({}, state, { scenario: action.payload, scenarioChangeToggler: !state.scenarioChangeToggler });
            }
            if (action.meta.field === 'segment') {
                return Object.assign({}, state, { segment: action.payload, segmentChangeToggler: !state.segmentChangeToggler });
            }
        }
        return state;
    }
    case RECEIVE_SAP_OFFICIAL_PERIODS: {
        const limitDate = dateService.subtract(dateService.now(), 1, 'y').toDate();

        const periods = Object
            .keys(action.periods.officialPeriods)
            .reduce((prev, year) => {
                if (+year > limitDate.getFullYear()) {
                    prev[year] = action.periods.officialPeriods[year];
                } else if (+year === limitDate.getFullYear()) {
                    prev[year] = action.periods.officialPeriods[year]
                        .filter(period => period.month >= limitDate.getMonth() + 1);
                }

                return prev;
            }, {});

        return Object.assign({}, state, { officialPeriods: {
            ...action.periods,
            officialPeriods: periods
        } });
    }
    case RESET_SAP_OFFICIAL_PERIODS: {
        return Object.assign({}, state, {
            officialPeriods: {
                defaultYear: null,
                officialPeriods: []
            }
        });
    }
    case CLEAR_SAP_SEARCH_NODES: {
        return Object.assign({}, state, { searchedNodes: [] });
    }
    case RECEIVE_SAP_SEARCH_NODES: {
        return Object.assign({}, state, { searchedNodes: action.nodes });
    }
    case RESET_SAP_FORM_DATA: {
        return Object.assign({}, state, {
            officialPeriods: {
                defaultYear: null,
                officialPeriods: []
            },
            searchedNodes: [],
            scenario: null,
            segment: null,
            ticket: {},
            validations: []
        });
    }
    case SET_SAP_TICKET_INFO: {
        return Object.assign({}, state, { ticket: action.ticket });
    }
    case RECEIVE_SAP_TICKET_VALIDATIONS: {
        return Object.assign({}, state, { validations: action.validations });
    }
    case RESET_SAP_TICKET_VALIDATIONS: {
        return Object.assign({}, state, { validations: [] });
    }
    case RECEIVE_SAP_TICKET:
        return Object.assign({}, state, { receiveToggler: !state.receiveToggler });
    case RECEIVE_SAP_TICKET_FAILURE:
        return Object.assign({}, state, { failureToggler: !state.failureToggler });
    case COMPLETE_FORWARD_PROCESS:
        return Object.assign({}, state, { forwardToggler: !state.forwardToggler });
    default:
        return state;
    }
};
