import * as actions from '../../../modules/transportBalance/logistics/actions';
import { logistics } from '../../../modules/transportBalance/logistics/reducers';
import { dateService } from '../../../common/services/dateService';

describe('Reducers for Logistics', () => {
    beforeAll(() => {
        dateService.isMinDate = jest.fn(key => key);
        dateService.format = jest.fn(key => key);
    });

    const initialState = {
        operational: {
            refreshToggler: false,
            status: false,
            refreshDateToggler: false,
            lastOwnershipDate: '29/10/1989',
            isInitialDateDisabled: true,
            isFinalDateDisabled: true,
            initialDate: null,
            searchNodesErrorToggler: false
        }
    };

    it('should handle action RECEIVE_ADD_LOGISTICS_TICKET', () => {
        const status = true;
        const action = {
            type: actions.RECEIVE_ADD_LOGISTICS_TICKET,
            status: true,
            name: 'operational'
        };

        const newState = Object.assign({}, initialState,
            {
                [action.name]: Object.assign({}, initialState[action.name], {
                    status: status, refreshToggler: !initialState[action.name].refreshToggler 
                })
            });
        expect(logistics(initialState, action)).toEqual(newState);
    });

    it('should handle action RECEIVE_DATE_FOR_SEGMENT', () => {
        const action = {
            type: actions.RECEIVE_DATE_FOR_SEGMENT,
            date: '29/10/1989',
            name: 'operational'
        };
       
        const newState = Object.assign({}, initialState,
            {
                [action.name]: Object.assign({}, initialState[action.name], {
                    refreshDateToggler: !initialState[action.name].refreshDateToggler,
                    lastOwnershipDate: action.date,
                    isInitialDateDisabled: initialState[action.name].isInitialDateDisabled,
                    isFinalDateDisabled: initialState[action.name].isFinalDateDisabled
                })
            });

        expect(logistics(initialState, action)).toEqual(newState);
    });

    it('should handle action SET_INITIAL_DATE', () => {
        const action = {
            type: actions.SET_INITIAL_DATE,
            date: '29/10/1989',
            name: 'operational'
        };

        const newState = Object.assign({}, initialState,
            {
                [action.name]: Object.assign({}, initialState[action.name], {
                    initialDate: action.date
                })
            });
            
        expect(logistics(initialState, action)).toEqual(newState);
    });

    it('should handle action CLEAR_LOGISTICS_OWNERSHIP_REQUEST_DATA', () => {
        const action = {
            type: actions.CLEAR_LOGISTICS_OWNERSHIP_REQUEST_DATA,
            lastOwnershipDate: null,
            isInitialDateDisabled: true,
            isFinalDateDisabled: true,
            initialDate: null,
            name: 'operational'
        };
        
        const newState = Object.assign({}, initialState,
            {
                [action.name]: Object.assign({}, initialState[action.name], {
                    lastOwnershipDate: action.lastOwnershipDate,
                    isInitialDateDisabled: action.isInitialDateDisabled,
                    isFinalDateDisabled: action.isFinalDateDisabled,
                    initialDate: action.initialDate
                })
            });
            
        expect(logistics(initialState, action)).toEqual(newState);
    });

    it('should handle action CLEAR_SEARCH_NODES', () => {
        const action = {
            type: actions.CLEAR_SEARCH_NODES,
            searchedNodes: [],
            name: 'operational'
        };

        const newState = Object.assign({}, initialState,
            {
                [action.name]: Object.assign({}, initialState[action.name], {
                    searchedNodes: action.searchedNodes
                })
            });

        expect(logistics(initialState, action)).toEqual(newState);
    });

    it('should handle action ON_SEGMENT_SELECT', () => {
        const action = {
            type: actions.ON_SEGMENT_SELECT,
            selectedSegment: {
                categoryElementId: 10,
                name: 'Test Segment'
            },
            name: 'operational'
        };

        const newState = Object.assign({}, initialState,
            {
                [action.name]: Object.assign({}, initialState[action.name], {
                    selectedSegment: action.selectedSegment
                })
            });
            

        expect(logistics(initialState, action)).toEqual(newState);
    });

    it('should handle action RECEIVE_SEARCH_NODES', () => {
        const action = {
            type: actions.RECEIVE_SEARCH_NODES,
            nodes: {
                categoryElementId: 10,
                name: 'Test Node'
            },
            name: 'operational'
        };
    
        const newState = Object.assign({}, initialState,
            {
                [action.name]: Object.assign({}, initialState[action.name], {
                    searchedNodes: action.nodes
                })
            });
            

        expect(logistics(initialState, action)).toEqual(newState);
    });

    it('should handle action SET_LOGISTICS_INFO', () => {
        const action = {
            type: actions.SET_LOGISTICS_INFO,
            logisticsInfo: {
                segmentId: 10,
                segmentName: 'Test Segment',
                owner: 'Ecopetrol',
                node: 'Todos'
            },
            name: 'operational'
        };

        const newState = Object.assign({}, initialState,
            {
                [action.name]: Object.assign({}, initialState[action.name], {
                    logisticsInfo: action.logisticsInfo
                })
            });
            

        expect(logistics(initialState, action)).toEqual(newState);
    });

    it('should handle action RECEIVE_LOGISTICS_VALIDATION_DATA', () => {
        const action = {
            type: actions.RECEIVE_LOGISTICS_VALIDATION_DATA,
            validationData: {
                nodeName: 'Test Node',
                operationDate: '01/02/2020',
                nodeStatus: 'FAILED'
            },
            validationDataToggler: false,
            name: 'operational'
        };

        const newState = Object.assign({}, initialState,
            {
                [action.name]: Object.assign({}, initialState[action.name], {
                    validationData: action.validationData,
                    validationDataToggler: true
                })
            });
            

        expect(logistics(initialState, action)).toEqual(newState);
    });
});
