import {
    RECEIVE_LAST_TICKET,
    SET_TICKET_INFO,
    SET_PENDING_TRANSACTION_ERRORS,
    SET_TRANSFER_POINT_MOVEMENTS,
    SET_UNBALANCES,
    COMPLETE_CUTOFF_PROCESS,
    INIT_CUTOFF_PROCESS,
    INCREMENT_CUTOFF_PROCESS,
    ADD_CUTOFF_COMMENT,
    INIT_ADD_CUTOFF_COMMENT,
    CONFIRM_CUT_OFF,
    RECEIVE_TICKET_DETAILS,
    RESET_TICKET_DETAILS,
    SET_TICKET_ERROR,
    INIT_INVENTORIES_VALIDATIONS,
    RECEIVE_INVENTORIES_VALIDATIONS,
    RECEIVE_FAILURE_SAVE_OPERATIONAL_CUTOFF,
    SET_OFFICIAL_POINT_ERROR,
    RECEIVE_UPDATE_CUTOFF_COMMENT,
    RECEIVE_UPDATE_CUTOFF_COMMENT_FAILURE,
    RECEIVE_FIRSTTIME_NODES
} from './actions';

import { dateService } from '../../../common/services/dateService';
import { resourceProvider } from './../../../common/services/resourceProvider';
import { utilities } from './../../../common/services/utilities';
import { constants } from './../../../common/services/constants';

const buildTickets = value => {
    let lastTicket = null;
    let initialDate = dateService.format(dateService.subtract(dateService.now(), 1, 'd'));
    if (value.length > 0) {
        lastTicket = value[0];
        initialDate = dateService.format(dateService.parse(lastTicket.endDate).add(1, 'days'));
    }

    return { lastTicket, ticket: { initialDate } };
};

function doBuildChartData(keys, values, alphabet, category) {
    const data = [];
    const categoryText = category ? `${resourceProvider.read(category)} ` : '';
    let color = alphabet;
    keys.map((key, index) => {
        color = String.fromCharCode(color.charCodeAt(0) + 1);
        return (
            data.push({
                name: `${categoryText}${resourceProvider.read(key)}`, value: values[index], color: color
            })
        );
    });
    return data;
}

function buildChartData(data) {
    const arrSum = arr => arr.reduce((a, b) => a + b, 0);

    // extract Keys and values
    const movementKeys = Object.keys(data.movements);
    const movementValues = Object.values(data.movements);
    const inventoryKeys = Object.keys(data.inventories);
    const inventoryValues = Object.values(data.inventories);
    const generatedMovementsKeys = Object.keys(data.generatedMovements);
    const generatedMovementsValues = Object.values(data.generatedMovements);

    // get name value pair
    const movementDetailChartData = doBuildChartData(movementKeys, movementValues, 'a', 'movements');
    const inventoryDetailChartData = doBuildChartData(inventoryKeys, inventoryValues, 'f', 'inventory');
    const processed = [...movementDetailChartData, ...inventoryDetailChartData];
    const generated = doBuildChartData(generatedMovementsKeys, generatedMovementsValues, 'j');

    // Manipulate total Records Chart
    const totalChartsKey = ['processed', 'created'];
    const totalChartsValue = [arrSum(movementValues) + arrSum(inventoryValues), arrSum(generatedMovementsValues)];
    const total = doBuildChartData(totalChartsKey, totalChartsValue, 'm');

    const chartData = {
        total,
        processed,
        generated
    };
    return chartData;
}

function buildInventoriesValidations(data) {
    const validations = utilities.normalizedGroupBy(data, 'type');
    return validations;
}

const ticketInfoInitialState = {
    ticket: {},
    total: [],
    processed: [],
    generated: []
};

const initialState = {
    ticket: {
        initialDate: null
    },
    inventoriesValidations: { [constants.InventoriesValidations.InitialInventories]: [], [constants.InventoriesValidations.NewNodes]: [] }
};

export const operationalCut = function (state = initialState, action = {}) {
    switch (action.type) {
    case INIT_CUTOFF_PROCESS:
        return Object.assign({}, state, {
            lastTicket: null,
            ticket: { initialDate: dateService.format(dateService.subtract(dateService.now(), 1, 'd')) },
            pendingTransactionErrors: [],
            unbalances: [],
            firstTimeNodes: [],
            officialMovements: [],
            sessionId: null,
            initToggler: !state.initToggler,
            step: 0,
            ready: false,
            segmentId: 0,
            commentToggler: false,
            confirmCutoffToggler: false,
            disableConfirmCutoff: false
        });
    case '@@redux-form/CHANGE': {
        if (action.meta.form === 'initTicket') {
            if (action.meta.field === 'segment') {
                return Object.assign({}, state, { segment: action.payload, segmentChangeToggler: !state.segmentChangeToggler });
            }
            if (action.meta.field === 'initialDate' || action.meta.field === 'finalDate') {
                return Object.assign({}, state, { dateChangedToggler: !state.dateChangedToggler });
            }
        }
        return state;
    }
    case RECEIVE_LAST_TICKET: {
        const tickets = buildTickets(action.ticket.value);
        tickets.ticket.segment = state.segment;
        return Object.assign({}, state, { ticket: tickets.ticket, lastTicket: tickets.lastTicket, ready: true });
    }
    case SET_TICKET_INFO:
        return Object.assign({}, state, { ticket: action.ticket });
    case SET_PENDING_TRANSACTION_ERRORS:
        return Object.assign({}, state, { pendingTransactionErrors: [...state.pendingTransactionErrors, ...action.pendingTransactionErrors] });
    case SET_UNBALANCES:
        return Object.assign({}, state, { unbalances: [...state.unbalances, ...action.unbalances] });
    case SET_TRANSFER_POINT_MOVEMENTS:
        return Object.assign({}, state, { officialMovements: [...state.officialMovements, ...action.officialMovements] });
    case COMPLETE_CUTOFF_PROCESS:
        return Object.assign({}, state, { receiveToggler: !state.receiveToggler, ticketId: action.ticketId });
    case INCREMENT_CUTOFF_PROCESS:
        return Object.assign({}, state, { step: state.step + 1 });
    case RECEIVE_UPDATE_CUTOFF_COMMENT:
        return Object.assign({}, state, { batchToggler: !state.batchToggler });
    case RECEIVE_UPDATE_CUTOFF_COMMENT_FAILURE:
        return Object.assign({}, state, { batchFailureToggler: !state.batchFailureToggler, disableConfirmCutoff: true });
    case ADD_CUTOFF_COMMENT: {
        return Object.assign({},
            state,
            {
                [state.name]: Object.assign({}, state[state.name], {
                    comment: action.comment,
                    commentToggler: !state[state.name].commentToggler
                })
            });
    }
    case INIT_ADD_CUTOFF_COMMENT:
        return Object.assign({},
            state,
            {
                name: action.name,
                [action.name]: Object.assign({}, state[action.name] ? state[action.name] : {}, {
                    comment: '',
                    preText: action.preText,
                    postText: action.postText,
                    count: action.count
                })
            });
    case CONFIRM_CUT_OFF:
        return Object.assign({}, state, { confirmCutoffToggler: !state.confirmCutoffToggler });

    case INIT_INVENTORIES_VALIDATIONS: {
        return Object.assign({}, state, { inventoriesValidations: initialState.inventoriesValidations });
    }
    case RECEIVE_INVENTORIES_VALIDATIONS: {
        return Object.assign({}, state, { inventoriesValidations: Object.assign({}, initialState.inventoriesValidations, buildInventoriesValidations(action.validations)) });
    }
    case RECEIVE_FAILURE_SAVE_OPERATIONAL_CUTOFF: {
        return Object.assign({},
            state,
            {
                saveCutOffFailureToggler: !state.saveCutOffFailureToggler,
                saveCutOffFailedErrorMessage: action.errorMessage,
                saveCutOffFailed: true
            });
    }
    case RECEIVE_FIRSTTIME_NODES:
        return Object.assign({}, state, { firstTimeNodes: [...action.nodeIds] });
    default:
        return state;
    }
};

export const ticketInfo = (state = ticketInfoInitialState, action = {}) => {
    switch (action.type) {
    case RECEIVE_TICKET_DETAILS: {
        const chartData = buildChartData(action.ticket);
        return Object.assign({}, state, {
            ticket: action.ticket.ticket,
            total: chartData.total,
            processed: chartData.processed,
            generated: chartData.generated,
            dataToggler: !state.dataToggler
        });
    }
    case RESET_TICKET_DETAILS: {
        return Object.assign({}, ticketInfoInitialState);
    }
    case SET_TICKET_ERROR: {
        return Object.assign({}, state, { ticket: action.ticket });
    }
    case SET_OFFICIAL_POINT_ERROR: {
        return Object.assign({}, state, {
            officialPoint: {
                errors: action.errors,
                movementId: action.row.movementId,
                movementTypeName: action.row.movementTypeName,
                operationalDate: action.row.operationalDate,
                errorMessage: action.row.errorMessage
            },
            officialPointErrorToggler: !state.officialPointErrorToggler
        });
    }

    default:
        return state;
    }
};
