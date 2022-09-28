import * as actions from '../../../modules/transportBalance/logistics/actions';
import { apiService } from '../../../common/services/apiService';
import { constants } from '../../../common/services/constants';
import { systemConfigService } from '../../../common/services/systemConfigService';

describe('Actions for logistics', () => {
    it('should create the logistics ticket', () => {
        const operationalCutOff = {
            Ticket: {
                startDate: '',
                endDate: '',
                categoryElementId: 10,
                categoryElementName: 'SegmentOne',
                ticketTypeId: constants.TicketType.Logistics,
                ownerId: 30
            }
        };
        const REQUEST_ADD_LOGISTICS_TICKET = 'REQUEST_ADD_LOGISTICS_TICKET';
        const action = actions.createLogisticTicket(operationalCutOff);

        expect(action.type).toEqual(REQUEST_ADD_LOGISTICS_TICKET);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.operationalCutOff.saveOperationalCutOff());
        expect(action.fetchConfig.body).toEqual(operationalCutOff);

        expect(action.fetchConfig.success).toBeDefined();
        const receiveLogisticTicket = action.fetchConfig.success(true);

        const RECEIVE_ADD_LOGISTICS_TICKET = 'RECEIVE_ADD_LOGISTICS_TICKET';
        expect(receiveLogisticTicket.type).toEqual(RECEIVE_ADD_LOGISTICS_TICKET);
        expect(receiveLogisticTicket.status).toEqual(true);
    });

    it('should get last ownership performed date for the selected segment', () => {
        const selectedSegmentId = 10;
        const actionType = 'REQUEST_LAST_OWNERSHIP_PERFORMED_DATE';
        const action = actions.getLastOwnershipPerformedDateForSelectedSegment(selectedSegmentId);

        expect(action.type).toEqual(actionType);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownership.getLastOwnershipPerformedDateForSelectedSegment(selectedSegmentId));
        expect(action.fetchConfig.success).toBeDefined();

        const receivedDate = {};

        const receiveAction = action.fetchConfig.success(receivedDate);
        const RECEIVE_DATE_FOR_SEGMENT = 'RECEIVE_DATE_FOR_SEGMENT';
        expect(receiveAction.type).toEqual(RECEIVE_DATE_FOR_SEGMENT);
    });

    it('should set initial date', () => {
        const SET_INITIAL_DATE = 'SET_INITIAL_DATE';
        const date = '29/1/2010';
        const action = actions.setInitialDate(date);

        expect(action.type).toEqual(SET_INITIAL_DATE);
        expect(action.date).toEqual(date);
    });

    it('should clear logistics ownership create request data', () => {
        const CLEAR_LOGISTICS_OWNERSHIP_REQUEST_DATA = 'CLEAR_LOGISTICS_OWNERSHIP_REQUEST_DATA';
        const action = actions.clearLogisticsOwnershipRequestData();

        expect(action.type).toEqual(CLEAR_LOGISTICS_OWNERSHIP_REQUEST_DATA);
    });

    it('should clear search node data', () => {
        const CLEAR_SEARCH_NODES = 'CLEAR_SEARCH_NODES';
        const action = actions.clearSearchNodes();

        expect(action.type).toEqual(CLEAR_SEARCH_NODES);
    });

    it('should set selected segment', () => {
        const ON_SEGMENT_SELECT = 'ON_SEGMENT_SELECT';
        const selectedSegment = {
            categoryElementId: 10,
            name: 'Test Segment'
        };
        const action = actions.onSegmentSelect(selectedSegment);

        expect(action.type).toEqual(ON_SEGMENT_SELECT);
        expect(action.selectedSegment).toEqual(selectedSegment);
    });

    it('should request search nodes', () => {
        const searchedNodes = {
            categoryElementId: 10,
            name: 'Test Node'
        };
        const segmentId = 10;
        const searchText = 'Test Node';

        const REQUEST_SEARCH_NODES = 'REQUEST_SEARCH_NODES';
        systemConfigService.getAutocompleteItemsCount = jest.fn(() => 5);
        const action = actions.requestSearchNodes(segmentId, searchText);

        expect(action.type).toEqual(REQUEST_SEARCH_NODES);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.node.searchSendToSapNodes(segmentId, searchText));

        expect(action.fetchConfig.success).toBeDefined();
        const receiveSearchNodes = action.fetchConfig.success(searchedNodes);

        const RECEIVE_SEARCH_NODES = 'RECEIVE_SEARCH_NODES';
        expect(receiveSearchNodes.type).toEqual(RECEIVE_SEARCH_NODES);

        expect(action.fetchConfig.failure).toBeDefined();
        const receiveSearchNodesFailure = action.fetchConfig.failure(searchText);

        const CLEAR_SEARCH_NODES = 'CLEAR_SEARCH_NODES';
        expect(receiveSearchNodesFailure.type).toEqual(CLEAR_SEARCH_NODES);
    });

    it('should set logistics info', () => {
        const SET_LOGISTICS_INFO = 'SET_LOGISTICS_INFO';
        const logisticsInfo = {
            segmentId: 10,
            segmentName: 'Test Segment',
            owner: 'Ecopetrol',
            node: 'Todos'
        };
        const action = actions.setLogisticsInfo(logisticsInfo);

        expect(action.type).toEqual(SET_LOGISTICS_INFO);
        expect(action.logisticsInfo).toEqual(logisticsInfo);
    });

    it('should request logistics validation data', () => {
        const validationData = {
            nodeName: 'Test Node',
            operationDate: '01/02/2020',
            nodeStatus: 'FAILED'
        };
        const ticket = {
            categoryElementId: 10,
            NodeId: 15,
            startDate: '01/12/2020',
            endDate: '02/02/2020'
        };

        const REQUEST_LOGISTICS_VALIDATION_DATA = 'REQUEST_LOGISTICS_VALIDATION_DATA';
        const action = actions.getLogisticsValidationData(ticket);

        expect(action.type).toEqual(REQUEST_LOGISTICS_VALIDATION_DATA);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.ticket.getLogisticsValidationData());
        expect(action.fetchConfig.body).toEqual(ticket);

        expect(action.fetchConfig.success).toBeDefined();
        const receiveLogisticsValidationData = action.fetchConfig.success(validationData);

        const RECEIVE_LOGISTICS_VALIDATION_DATA = 'RECEIVE_LOGISTICS_VALIDATION_DATA';
        expect(receiveLogisticsValidationData.type).toEqual(RECEIVE_LOGISTICS_VALIDATION_DATA);
    });
});
