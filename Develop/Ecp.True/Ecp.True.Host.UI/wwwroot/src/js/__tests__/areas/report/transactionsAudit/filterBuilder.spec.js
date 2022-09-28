import * as transactionsAuditReportFilterBuilder from '../../../../modules/report/transactionsAudit/filterBuilder';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import { dateService } from '../../../../common/services/dateService';

describe('TransactionsAudit Report Filter Builder Validate',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
        });

        it('should return filter for transactions Audit Movements report',
            async () => {
                const values = {
                    elementId: 10,
                    initialDate: dateService.now(),
                    finalDate: dateService.now(),
                    reportType: constants.Report.MovementAuditReport
                };
                const result = transactionsAuditReportFilterBuilder.transactionsAuditReportFilterBuilder.build(values);
                await expect(result.length).toBe(4);
                await expect(result[0].target.table).toBe('ReportHeaderDetails');
                await expect(result[0].target.column).toBe('Category');
                await expect(result[1].target.table).toBe('ReportHeaderDetails');
                await expect(result[1].target.column).toBe('Element');
                await expect(result[2].target.table).toBe('DimDate');
                await expect(result[2].target.column).toBe('Date');
                await expect(result[3].target.table).toBe('Movement');
                await expect(result[3].target.column).toBe('SegmentNameFilter');
            });
        it('should return filter for transactions Audit Inventory report',
            async () => {
                const values = {
                    elementId: 10,
                    initialDate: dateService.now(),
                    finalDate: dateService.now(),
                    reportType: constants.Report.InventoryAuditReport
                };
                const result = transactionsAuditReportFilterBuilder.transactionsAuditReportFilterBuilder.build(values);
                await expect(result.length).toBe(4);
                await expect(result[0].target.table).toBe('ReportHeaderDetails');
                await expect(result[0].target.column).toBe('Category');
                await expect(result[1].target.table).toBe('ReportHeaderDetails');
                await expect(result[1].target.column).toBe('Element');
                await expect(result[2].target.table).toBe('DimDate');
                await expect(result[2].target.column).toBe('Date');
                await expect(result[3].target.table).toBe('InventoryProduct');
                await expect(result[3].target.column).toBe('SegmentNameFilter');
            });
    });
