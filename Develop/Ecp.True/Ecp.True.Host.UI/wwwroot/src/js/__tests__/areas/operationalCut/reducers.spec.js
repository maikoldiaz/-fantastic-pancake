import * as actions from '../../../modules/transportBalance/cutOff/actions';
import { operationalCut, ticketInfo } from '../../../modules/transportBalance/cutOff/reducers';
import { dateService } from '../../../common/services/dateService';

describe('Reducer for operational cut', () => {
    const initialState = {
        ticket: {},
        pendingTransactionErrors: [],
        unbalances: [],
        officialMovements: [],
        segment: 1,
        sessionId: null
    };

    const initialTicketInfoState = {
        generated: [],
        processed: [],
        total: [],
        ticket: {}
    };

    it('should receive last ticket', () => {
        const action = {
            type: actions.SET_TICKET_INFO,
            ticket: {}
        };

        const newState = Object.assign({}, initialState);
        expect(operationalCut(initialState, action)).toEqual(newState);
    });

    it('should set ticket info.', () => {
        const action = {
            type: actions.SET_TICKET_INFO,
            ticket: {}
        };
        const newState = Object.assign({}, initialState);
        expect(operationalCut(initialState, action)).toEqual(newState);
    });

    it('should set pending transactions error', () => {
        const action = {
            type: actions.SET_PENDING_TRANSACTION_ERRORS,
            pendingTransactionErrors: []
        };
        const newState = Object.assign({}, initialState);
        expect(operationalCut(initialState, action)).toEqual(newState);
    });

    it('should set unbalances.', () => {
        const action = {
            type: actions.SET_UNBALANCES,
            pendingTransactionErrors: [],
            officialMovements: [],
            unbalances: []
        };
        const newState = Object.assign({}, initialState, { officialMovements: [...initialState.officialMovements, ...action.officialMovements] });
        expect(operationalCut(initialState, action)).toEqual(newState);
    });

    it('should set Transfer Point Movements.', () => {
        const action = {
            type: actions.SET_TRANSFER_POINT_MOVEMENTS,
            pendingTransactionErrors: [],
            officialMovements: [],
            unbalances: []
        };
        const newState = Object.assign({}, initialState);
        expect(operationalCut(initialState, action)).toEqual(newState);
    });

    it('should receive last ticket.', () => {
        const action = {
            type: actions.RECEIVE_LAST_TICKET,
            pendingTransactionErrors: [],
            unbalances: [],
            ticket: { value: [{ endDate: '11-Nov-19' }] }
        };
        const newState = Object.assign({}, initialState, { ready: true, lastTicket: { endDate: '11-Nov-19' }, ticket: { initialDate: '2019-11-12T00:00:00Z', segment: 1 } });
        expect(operationalCut(initialState, action)).toEqual(newState);
    });

    it('should set ticket info.', () => {
        const action = {
            type: actions.SET_TICKET_INFO,
            ticket: { value: [{ endDate: '12-Nov-19' }] }
        };
        const newState = Object.assign({}, initialState, { ticket: { value: [{ endDate: '12-Nov-19' }] } });
        expect(operationalCut(initialState, action)).toEqual(newState);
    });

    it('should complete cut off process.', () => {
        const action = {
            type: actions.COMPLETE_CUTOFF_PROCESS,
            receiveToggler: true,
            ticketId: 1
        };
        const newState = Object.assign({}, initialState, { receiveToggler: true, ticketId: 1 });
        expect(operationalCut(initialState, action)).toEqual(newState);
    });

    it('should init cutoff process state.', () => {
        const action = {
            type: actions.INIT_CUTOFF_PROCESS
        };
        const newState = Object.assign({}, initialState, {
            lastTicket: null,
            ticket: { initialDate: dateService.format(dateService.subtract(dateService.now(), 1, 'd')) },
            pendingTransactionErrors: [],
            unbalances: [],
            firstTimeNodes: [],
            initToggler: true,
            step: 0,
            ready: false,
            segmentId: 0,
            commentToggler: false,
            confirmCutoffToggler: false,
            disableConfirmCutoff: false
        });

        expect(operationalCut(initialState, action)).toEqual(newState);
    });

    it('should add cut off comment.', () => {
        const action = {
            type: actions.INIT_ADD_CUTOFF_COMMENT,
            preText: 'preText',
            postText: 'postText',
            count: 9
        };
        const newState = operationalCut(initialState, action);

        expect(newState).toBeDefined();
    });

    it('@@redux-form/CHANGE', () => {
        const action = {
            type: '@@redux-form/CHANGE', meta: { form: 'initTicket', field: 'segment' },
            payload: 1
        };
        const state = Object.assign({}, initialState, { ticket: { startDate: '22/10/2019' } });
        const newState = Object.assign({}, initialState, { ticket: { startDate: '22/10/2019' }, segment: 1, segmentChangeToggler: true });

        expect(operationalCut(state, action)).toEqual(newState);
    });

    it('@@redux-form/CHANGE when meta form is not initTicket', () => {
        const action = {
            type: '@@redux-form/CHANGE', meta: {},
            payload: 1
        };
        const state = Object.assign({}, initialState, { ticket: { startDate: '22/10/2019' } });
        expect(operationalCut(state, action)).toEqual(state);
    });


    it('should complete cutoff process', () => {
        const action = {
            type: 'COMPLETE_CUTOFF_PROCESS',
            ticketId: 1
        };
        const state = Object.assign({}, initialState, { receiveToggler: false, ticketId: action.ticketId });
        const newState = Object.assign({}, state, { receiveToggler: true });
        expect(operationalCut(state, action)).toEqual(newState);
    });

    it('should increment cutoff process', () => {
        const action = {
            type: 'CUTOFF_PROCESS_START'
        };
        const state = Object.assign({}, initialState, { step: 0 });
        const newState = Object.assign({}, state, { step: 1 });

        expect(operationalCut(state, action)).toEqual(newState);
    });

    it('should return default state', () => {
        const action = {
            type: 'CUTOFF_PROCESS_START1'
        };
        const state = Object.assign({}, initialState, { step: 0 });

        expect(operationalCut(state, action)).toEqual(state);
    });

    it('should confirm cut off', () => {
        const localInitialState = {
            ticket: {},
            pendingTransactionErrors: [],
            unbalances: [],
            segment: 1,
            confirmCutoffToggler: false
        };

        const finalState = {
            ticket: {},
            pendingTransactionErrors: [],
            unbalances: [],
            segment: 1,
            confirmCutoffToggler: true
        };

        const action = {
            type: 'CONFIRM_CUT_OFF'
        };
        const state = Object.assign({}, localInitialState);

        expect(operationalCut(state, action)).toEqual(finalState);
    });

    it('should reset ticket details', () => {
        const action = {
            type: 'RESET_TICKET_DETAILS',
            ticket: {}
        };
        const state = Object.assign({}, initialTicketInfoState, {});

        expect(ticketInfo(state, action)).toEqual(state);
    });

    it('should set ticket error', () => {
        const action = {
            type: 'SET_TICKET_ERROR',
            ticket: {}
        };
        const state = Object.assign({}, initialTicketInfoState, {});

        expect(ticketInfo(state, action)).toEqual(state);
    });

    it('should set official points error', () => {
        const action = {
            type: 'SET_OFFICIAL_POINT_ERROR',
            errors: [],
            row: { movementId: 1, movementTypeName: 'name', operationalDate: 'date', errorMessage: 'message' }
        };
        const state = Object.assign({}, initialTicketInfoState, { officialPoint: {
            errors: action.errors,
            movementId: action.row.movementId,
            movementTypeName: action.row.movementTypeName,
            operationalDate: action.row.operationalDate,
            errorMessage: action.row.errorMessage
        },
        officialPointErrorToggler: true });
        const newState = Object.assign({}, state, { officialPointErrorToggler: false });

        expect(ticketInfo(state, action)).toEqual(newState);
    });

    it('should set toggle batch toggler', () => {
        const action = {
            type: 'RECEIVE_UPDATE_CUTOFF_COMMENT'
        };
        const state = Object.assign({}, initialTicketInfoState, { batchToggler: !initialTicketInfoState.batchToggler });

        expect(ticketInfo(state, action)).toEqual(state);
    });

    it('should set toggle batch failure toggler', () => {
        const action = {
            type: 'RECEIVE_UPDATE_CUTOFF_COMMENT_FAILURE'
        };
        const state = Object.assign({}, initialTicketInfoState, { batchFailureToggler: !initialTicketInfoState.batchFailureToggler, disableConfirmCutoff: true });

        expect(ticketInfo(state, action)).toEqual(state);
    });

    it('should return default state', () => {
        const action = {
            type: 'SET_TICKET_ERROR1'
        };
        const state = Object.assign({}, initialState, {});
        expect(ticketInfo(state, action)).toEqual(state);
    });

    it('should set first time nodes', () => {
        const action = {
            type: actions.RECEIVE_FIRSTTIME_NODES,
            nodeIds: []
        };
        const newState = Object.assign({}, initialState, { firstTimeNodes: [...action.nodeIds] });
        expect(operationalCut(initialState, action)).toEqual(newState);
    });
});
