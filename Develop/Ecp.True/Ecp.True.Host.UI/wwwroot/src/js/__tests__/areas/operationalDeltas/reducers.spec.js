import * as actions from '../../../modules/transportBalance/operationalDelta/actions';
import { operationalDelta } from '../../../modules/transportBalance/operationalDelta/reducers';
import { dateService } from '../../../common/services/dateService';

describe('Reducer for Operational Delta', () => {
    const initialState = {
        deltaTicket: {
            segment: null,
            startDate: null,
            endDate: null
        },
        isFinalDateReceivedToggler: true
    };

    it('should Init Delta Ticket', () => {
        const action = {
            type: actions.INIT_DELTA_TICKET
        };

        const newState = Object.assign({}, initialState);
        expect(operationalDelta(initialState, action)).toEqual(newState);
    });

    it('should Init Delta Segment', () => {
        const action = {
            type: actions.INIT_DELTA_SEGMENT,
            segment: {}
        };

        const newState = Object.assign({}, initialState, { deltaTicket: Object.assign({}, initialState.deltaTicket, { segment: {} }) });
        expect(operationalDelta(initialState, action)).toEqual(newState);
    });

    it('should Init Delta Start Date', () => {
        const action = {
            type: actions.INIT_DELTA_START_DATE,
            startDate: '04-06-2020'
        };

        const newState = Object.assign({}, initialState, { deltaTicket: Object.assign({}, initialState.deltaTicket, { startDate: '04-06-2020' }) });
        expect(operationalDelta(initialState, action)).toEqual(newState);
    });

    it('should Set Delta Ticket', () => {
        const action = {
            type: actions.SET_DELTA_TICKET,
            deltaTicket: {
                startDate: '04-06-2020',
                endDate: '06-07-2020',
                segment: {}
            }
        };

        const newState = Object.assign({}, initialState, { deltaTicket: { startDate: '04-06-2020', endDate: '06-07-2020', segment: {} } });
        expect(operationalDelta(initialState, action)).toEqual(newState);
    });

    it('should receive Delta Start Date', () => {
        const action = {
            type: actions.RECEIVE_END_DATE,
            date: '2020-04-06T00:00:00Z'
        };

        const newState = Object.assign({}, initialState, { deltaTicket: Object.assign({}, initialState.deltaTicket, { endDate: '2020-04-06T00:00:00Z' }), isFinalDateReceivedToggler: !initialState.isFinalDateReceivedToggler });
        expect(operationalDelta(initialState, action)).toEqual(newState);
    });
});
