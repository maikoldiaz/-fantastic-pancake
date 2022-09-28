import * as actions from '../../../modules/dailyBalance/sentToSap/actions';
import { sendToSap } from '../../../modules/dailyBalance/sentToSap/reducers';
import { dateService } from '../../../common/services/dateService';

describe('Reducer for sentToSap', () => {
    const sendToSapInitialState = {
        ticket: {
            initialDate: null
        },
        scenarioName: null
    };

    it('should receive ticket', () => {
        const action = {
            type: actions.RECEIVE_GET_TICKET,
            ticket: {
                value: [{
                    segment: 'Transporte',
                    ticketStartDate: '2021-04-27',
                    ticketFinalDate: '2021-04-28',
                    ownerName: 'Ecopetrol',
                    scenarioName: 'Operativo'
                }]
            }
        };

        const state = Object.assign({}, sendToSapInitialState, { ticket: {} });
        const newState = Object.assign({}, sendToSapInitialState, {
            ticket: {
                segment: 'Transporte',
                ticketStartDate: '2021-04-27',
                ticketFinalDate: '2021-04-28',
                ownerName: 'Ecopetrol',
                scenarioName: 'Operativo'
            }
        });

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should forward to sap', () => {
        const action = {
            type: actions.COMPLETE_FORWARD_PROCESS
        };

        const state = Object.assign({}, sendToSapInitialState, { forwardToggler: true });
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                forwardToggler: false
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should save confirm wizard data', () => {
        const action = {
            type: actions.SAVE_CONFIRM_WIZARD_DATA,
            data: {}
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                confirmWizard: {}
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should reset confirm wizard data', () => {
        const action = {
            type: actions.RESET_CONFIRM_WIZARD_DATA
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                confirmWizard: null
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should receive nodes in ticket', () => {
        const action = {
            type: actions.RECEIVE_NODES_IN_TICKET,
            tickets: {
                value: [{
                    ticketNodes: [{ node: {} }]
                }]
            }
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                nodesInTicketToggler: true,
                nodesInTicket: [{}]
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should receive failure nodes in ticket', () => {
        const action = {
            type: actions.RECEIVE_FAILURE_NODES_IN_TICKET
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                failureNodesInTicketToggler: true
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should init sent to sap process', () => {
        const action = {
            type: actions.INIT_SENTTOSAP_PROCESS
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                confirmSentToSapToggler: false,
                confirmCancelBatchToggler: false
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should init movements confirmation', () => {
        const action = {
            type: actions.INIT_MOVEMENTS_CONFIRMATION,
            name: 'name',
            selectedMovements: [],
            countTotalMovements: 0
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                name: 'name',
                [action.name]: Object.assign({}, state[action.name] ? state[action.name] : {}, {
                    selectedMovements: [],
                    countTotalMovements: 0
                })
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should confirm sent to sap', () => {
        const action = {
            type: actions.CONFIRM_SENT_SAP
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                confirmSentToSapToggler: true
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should complete sent to sap process', () => {
        const action = {
            type: actions.COMPLETE_SENTTOSAP_PROCESS
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                receiveToggler: true
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should receive failure sent to sap process', () => {
        const action = {
            type: actions.RECEIVE_FAILURE_SENTTOSAP_PROCESS,
            errorMessage: 'error'
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                saveSentToSapFailureToggler: true,
                saveSentToSapFailedErrorMessage: 'error',
                saveSentToSapFailed: true
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should receive failure get ticket', () => {
        const action = {
            type: actions.RECEIVE_FAILURE_GET_TICKET
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                getTicketFailure: true,
                getTicketFailed: true
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should confirm cancel batch', () => {
        const action = {
            type: actions.CONFIRM_CANCEL_BATCH
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                confirmCancelBatchToggler: true
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should complete cancel batch process', () => {
        const action = {
            type: actions.COMPLETE_CANCEL_BATCH_PROCESS
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                receiveCancelBatchToggler: true
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should receive failure cancel batch process', () => {
        const action = {
            type: actions.RECEIVE_FAILURE_CANCEL_BATCH_PROCESS,
            errorMessage: 'error'
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                saveCancelBatchFailureToggler: true,
                saveCancelBatchFailedErrorMessage: 'error',
                saveCancelBatchFailed: true
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should reset sap official periods', () => {
        const action = {
            type: actions.RESET_SAP_OFFICIAL_PERIODS
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                officialPeriods: {
                    defaultYear: null,
                    officialPeriods: []
                }
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should clear sap search nodes', () => {
        const action = {
            type: actions.CLEAR_SAP_SEARCH_NODES
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                searchedNodes: []
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });
    
    it('should receive sap search nodes', () => {
        const action = {
            type: actions.RECEIVE_SAP_SEARCH_NODES,
            nodes: []
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                searchedNodes: []
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });
    
    it('should reset sap form data', () => {
        const action = {
            type: actions.RESET_SAP_FORM_DATA
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                officialPeriods: {
                    defaultYear: null,
                    officialPeriods: []
                },
                searchedNodes: [],
                scenario: null,
                segment: null,
                ticket: {},
                validations: []
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });
    
    it('should set sap ticket info', () => {
        const action = {
            type: actions.SET_SAP_TICKET_INFO,
            ticket: {}
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                ticket: {}
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });
    
    it('should receive sap ticket validations', () => {
        const action = {
            type: actions.RECEIVE_SAP_TICKET_VALIDATIONS,
            validations: []
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                validations: []
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });
    
    it('should reset sap ticket validations', () => {
        const action = {
            type: actions.RESET_SAP_TICKET_VALIDATIONS,
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                validations: []
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });
    
    it('should receive sap ticket', () => {
        const action = {
            type: actions.RECEIVE_SAP_TICKET,
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                receiveToggler: true
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });
    
    it('should receive sap ticket failure', () => {
        const action = {
            type: actions.RECEIVE_SAP_TICKET_FAILURE,
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                failureToggler: true
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should enter in default', () => {
        const action = {
            type: 'DEFAULT',
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({}, sendToSapInitialState, {});

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should redux form change with scenario', () => {
        const action = {
            type: '@@redux-form/CHANGE',
            payload: 'payload',
            meta: {
                form: 'initSapFormTicket',
                field: 'scenario'
            }
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                scenario: 'payload', 
                scenarioChangeToggler: true 
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should redux form change with segment', () => {
        const action = {
            type: '@@redux-form/CHANGE',
            payload: 'payload',
            meta: {
                form: 'initSapFormTicket',
                field: 'segment'
            }
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                segment: 'payload', 
                segmentChangeToggler: true 
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should redux form change without init sap form ticket', () => {
        const action = {
            type: '@@redux-form/CHANGE',
            payload: 'payload',
            meta: {
                form: 'form',
                field: 'segment'
            }
        };

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({}, sendToSapInitialState, {});

        expect(sendToSap(state, action)).toEqual(newState);
    });

    it('should receive sap official periods', () => {
        const action = {
            type: actions.RECEIVE_SAP_OFFICIAL_PERIODS,
            periods: { officialPeriods: {} }
        };
        dateService.initialize();

        const state = Object.assign({}, sendToSapInitialState, {});
        const newState = Object.assign({},
            sendToSapInitialState,
            {
                officialPeriods:{
                    officialPeriods: {}
                }                
            }
        );

        expect(sendToSap(state, action)).toEqual(newState);
    });
});
