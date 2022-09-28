import setup from '../../setup';
import React from 'react';
import { mount } from 'enzyme';
import configureStore from 'redux-mock-store';
import { Provider } from 'react-redux';
import TicketDetail from './../../../../modules/transportBalance/cutOff/components/ticketDetail.jsx';
import { navigationService } from './../../../../common/services/navigationService';
import { httpService } from './../../../../common/services/httpService';
import { resourceProvider } from './../../../../common/services/resourceProvider';
const initialState = {
    cutoff: {
        ticketInfo: {
            ticket: {
                ticketId: 23678,
                startDate: '2019-10-01T00:00:00',
                endDate: '2019-10-14T00:00:00',
                status: 'PROCESSED',
                categoryElement: {
                    name: 'Transporte'
                }
            },
            total: [{
                name: 'category 1',
                value: 'value 1'
            }],
            generated: [{
                name: 'category 1',
                value: 'value 1'
            }],
            processed: [{
                name: 'category 1',
                value: 'value 1'
            }]
        }
    }
};

function mountWithMockStore(defaultState) {
    const mockStore = configureStore();
    const store = mockStore(defaultState);
    const enzymeWrapper = mount(<Provider store={store}><TicketDetail /></Provider>);
    return { store, enzymeWrapper };
}

describe('ticket Detail Chart', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.getParamByName = jest.fn();
    });
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithMockStore(initialState);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should mount successfully and find Control', () => {
        const { enzymeWrapper } = mountWithMockStore(initialState);
        expect(enzymeWrapper.exists('#heading_chart_detail')).toBe(true);

        expect(enzymeWrapper.exists('#lbl_ticketDetail_identifier')).toBe(true);
        expect(enzymeWrapper.find('#lbl_ticketDetail_identifier').text()).toEqual('identifier');
        expect(enzymeWrapper.exists('#lbl_ticketDetail_ticketId')).toBe(true);

        expect(enzymeWrapper.exists('#h1_ticketDetail_records_total')).toBe(true);
        expect(enzymeWrapper.exists('#h1_ticketDetail_records_processed')).toBe(true);
        expect(enzymeWrapper.exists('#h1_ticketDetail_records_created')).toBe(true);

        expect(enzymeWrapper.find('#h1_ticketDetail_records_total').text()).toEqual('totalRecords');
        expect(enzymeWrapper.find('#h1_ticketDetail_records_processed').text()).toEqual('processedRecords');
        expect(enzymeWrapper.find('#h1_ticketDetail_records_created').text()).toEqual('recordsCreated');
        expect(enzymeWrapper.exists('#cont_ticketDetail_records_total')).toBe(true);
        expect(enzymeWrapper.exists('#cont_ticketDetail_records_processed')).toBe(true);
        expect(enzymeWrapper.exists('#cont_ticketDetail_records_new')).toBe(true);


        expect(enzymeWrapper.exists('#img_ticketDetail_records_new_pending')).toBe(false);
        expect(enzymeWrapper.exists('#img_ticketDetail_records_total_pending')).toBe(false);
        expect(enzymeWrapper.exists('#img_ticketDetail_records_processed_pending')).toBe(false);
    });

    it('should mount successfully and find Control when status in progress', () => {
        const { enzymeWrapper } = mountWithMockStore(Object.assign({}, initialState,
            {
                cutoff: {
                    ticketInfo: {
                        ticket: { ...initialState.cutoff.ticketInfo.ticket, status: false },
                        totalRecordsChartData: { ...initialState.cutoff.ticketInfo.totalRecordsChartData },
                        generatedmovementsDetail: { ...initialState.cutoff.ticketInfo.generatedmovementsDetail },
                        processedRecordsChartData: { ...initialState.cutoff.ticketInfo.processedRecordsChartData }
                    }
                }
            }));
        expect(enzymeWrapper.exists('#img_ticketDetail_records_new_pending')).toBe(true);
        expect(enzymeWrapper.exists('#img_ticketDetail_records_total_pending')).toBe(true);
        expect(enzymeWrapper.exists('#img_ticketDetail_records_processed_pending')).toBe(true);

        expect(enzymeWrapper.exists('#ticketDetail_records_created_chart')).toBe(false);
        expect(enzymeWrapper.exists('#ticketDetail_records_processed_chart')).toBe(false);
        expect(enzymeWrapper.exists('#ticketDetail_total_records_chart')).toBe(false);
    });
});
