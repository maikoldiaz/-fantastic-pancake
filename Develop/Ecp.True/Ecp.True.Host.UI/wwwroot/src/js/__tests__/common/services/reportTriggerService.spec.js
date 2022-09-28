import { reportTriggerService } from '../../../common/services/reportTriggerService';
import { dateService } from '../../../common/services/dateService';
import { constants } from '../../../common/services/constants';
describe('report trigger service',
    () => {
        it('should get invoke build request data', () =>{
            const reportRequest = {
                segment: 'segmentName',
                nodeName: 'nodeName',
                nodeId: 1,
                segmentId: 1,
                startDate: new Date(),
                endDate: new Date(),
                reportType: constants.Report.OfficialNodeBalanceReport,
                nodeStatus: 'processing',
                ticketStatus: 'delta',
                deltaNodeId: 1,
                fromList: true,
                executionId: 1
            };
            const reportResponse = reportTriggerService.buildRequestData(constants.Report.OfficialNodeBalanceReport, reportRequest, true);
            expect(reportResponse).toBeDefined();
        });
    });

