import { sentToSapReportFilterBuilder } from '../../../../modules/report/sentToSap/filterBuilder';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';

describe('SentToSap Report Filter Builder Validate', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
    });
    it('should return filter for sentToSap report', async () => {
        const values = {
            categoryName: 'Segmento',
            elementName: 'Estación',
            nodeName: 'SAN FERNANDO',
            initialDate: '12/09/2019',
            finalDate: '18/09/2019',
            reportType: constants.Report.SendToSapStatesReport
        };
        
        const result = sentToSapReportFilterBuilder.build(values);
        await expect(result.length).toBe(5);
        await expect(result[0].target.table).toBe('DimDate');
        await expect(result[0].target.column).toBe('Date');
        await expect(result[1].target.column).toBe('Category');
        await expect(result[2].target.column).toBe('Element');
        await expect(result[3].target.table).toBe('MovementSendSapInformation');
        await expect(result[4].target.table).toBe('ReportHeaderDetailsExecution');
        await expect(result[3].target.column).toBe('ExecutionId');
        await expect(result[4].target.column).toBe('ExecutionId');
        
    });
});
