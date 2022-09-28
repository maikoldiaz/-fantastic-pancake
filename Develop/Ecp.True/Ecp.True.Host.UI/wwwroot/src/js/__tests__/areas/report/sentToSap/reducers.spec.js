import * as actions from '../../../../modules/report/sentToSap/actions';
import { sentToSapReport } from '../../../../modules/report/sentToSap/reducers';
import { constants } from '../../../../common/services/constants';
import { dateService } from '../../../../common/services/dateService';

describe('Reducers for SentToSap report', () => {
    const initialState = {
        filters: null
    };

    it('should return status unchanged', () => {
        expect(sentToSapReport(initialState)).toEqual(initialState);
    });

    it('should handle action CUTOFF_REPORT_FILTER_ON_SELECT_CATEGORY', () => {
        const action = {
            type: actions.BUILD_SEND_TO_SAP_REPORT_EXECUTION_FILTERS,
            execution: {
                executionId: 10,
                segment: 'SegmentName',
                nodeName: 'NodeName',
                startDate: '2021-01-14T00:00:00Z',
                endDate: '2021-01-14T00:00:00Z',
                name: 'ReportName'
            }
        };
        const newState = Object.assign({}, initialState, {
            filters: {
                executionId: 10,
                categoryName: 'Segmento',
                elementName: 'SegmentName',
                nodeName: 'NodeName',
                initialDate: dateService.parse('2021-01-14T00:00:00Z'),
                finalDate: dateService.parse('2021-01-14T00:00:00Z'),
                reportType: constants.Report.SendToSapStatesReport
            }
        });
        expect(sentToSapReport(initialState, action)).toEqual(newState);
    });
});

