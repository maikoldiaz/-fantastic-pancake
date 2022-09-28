import * as reportsFilterBuilder from '../../../../common/components/filterBuilder/reportsFilterBuilder';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import { dateService } from '../../../../common/services/dateService';

describe('Official Balance Filter',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
            dateService.format = jest.fn();
            dateService.parseToPBIString = jest.fn();
            dateService.subtract = jest.fn();
        });

        it('should return filter for official balance per node report',
            async () => {
                const values = {
                    element: { elementName: 'element', elementId: 1 },
                    elementName: 'element',
                    nodeName: 'node',
                    nodeId: 123,
                    executionId: 123,
                    initialDate: dateService.today(),
                    finalDate: dateService.today(),
                    reportType: constants.Report.OfficialBalancePerNodeReport
                };
                const result = reportsFilterBuilder.reportsFilterBuilder.build(values);
                await expect(result.length).toBe(15);
                await expect(result[0].target.table).toBe('ReportHeaderDetails');
                await expect(result[0].target.column).toBe('Category');
                await expect(result[1].target.table).toBe('ReportHeaderDetails');
                await expect(result[1].target.column).toBe('Element');
                await expect(result[2].target.table).toBe('ReportHeaderDetails');
                await expect(result[2].target.column).toBe('NodeName');
                await expect(result[3].target.table).toBe('DimDate');
                await expect(result[3].target.column).toBe('Date');
            });

        it('should return filter for official initial balance report',
            async () => {
                const values = {
                    element: {},
                    elementName: 'element',
                    nodeName: 'node',
                    nodeId: 123,
                    reportType: constants.Report.OfficialInitialBalanceReport,
                    executionId: 123
                };
                const result = reportsFilterBuilder.reportsFilterBuilder.build(values);
                await expect(result.length).toBe(10);
                await expect(result[0].target.table).toBe('ReportHeaderDetails');
                await expect(result[0].target.column).toBe('Category');
                await expect(result[1].target.table).toBe('ReportHeaderDetails');
                await expect(result[1].target.column).toBe('Element');
                await expect(result[2].target.table).toBe('ReportHeaderDetails');
                await expect(result[2].target.column).toBe('NodeName');
                await expect(result[3].target.table).toBe('DimDate');
                await expect(result[3].target.column).toBe('Date');
            });

        it('should return filter for official pending balance report',
            async () => {
                const values = {
                    name: 'name',
                    element: {},
                    executionId: 123
                };
                const result = reportsFilterBuilder.reportsFilterBuilder.build(values);
                await expect(result.length).toBe(5);
                await expect(result[0].target.table).toBe('ReportHeaderDetails');
                await expect(result[0].target.column).toBe('Category');
                await expect(result[1].target.table).toBe('ReportHeaderDetails');
                await expect(result[1].target.column).toBe('Element');
                await expect(result[2].target.table).toBe('MovementGroup');
                await expect(result[3].target.table).toBe('DimDate');
                await expect(result[4].target.table).toBe('MovementGroup');
            });
    });
