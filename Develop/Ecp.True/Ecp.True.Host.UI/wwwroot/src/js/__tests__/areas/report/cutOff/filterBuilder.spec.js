import * as cutoffReportFilterBuilder from '../../../../modules/report/cutOff/filterBuilder.js';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';

describe('CutOff Report Filter Builder Validate',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
        });
        it('should return filter for no ownership calculation',
            async () => {
                const values = {
                    categoryName: 'Segmento',
                    elementName: 'Estación',
                    nodeName: 'SAN FERNANDO',
                    initialDate: '12/09/2019',
                    finalDate: '18/09/2019',
                    reportType: constants.Report.WithoutOwner
                };
                const result = cutoffReportFilterBuilder.cutoffReportFilterBuilder.build(values);
                await expect(result.length).toBe(39);
                await expect(result[0].target.table).toBe('KPIDataByCategoryElementNode');
                await expect(result[0].target.column).toBe('FilterType');
                await expect(result[1].target.column).toBe('NodeName');
                await expect(result[2].target.column).toBe('Category');
                await expect(result[3].target.column).toBe('Element');
                await expect(result[4].target.column).toBe('CalculationDate');
                await expect(result[1].values[0]).toBe(values.nodeName);
                await expect(result[2].values[0]).toBe(values.categoryName);
                await expect(result[3].values[0]).toBe(values.elementName);
                await expect(result[5].target.table).toBe('KPIPreviousDateDataByCategoryElementNode');
                await expect(result[10].target.table).toBe('MovementDetailsWithoutOwner');
                await expect(result[35].target.table).toBe('QualityDetailsWithoutOwner');
            });
        it('should return filter for ownership calculation',
            async () => {
                const values = {
                    categoryName: 'Sistema',
                    elementName: 'Estación',
                    nodeName: 'SAN FERNANDO',
                    initialDate: '12/09/2019',
                    finalDate: '18/09/2019',
                    reportType: constants.Report.WithOwner
                };
                const result = cutoffReportFilterBuilder.cutoffReportFilterBuilder.build(values);
                await expect(result.length).toBe(47);
                await expect(result[0].target.table).toBe('KPIDataByCategoryElementNodeWithOwnership');
                await expect(result[0].target.column).toBe('FilterType');
                await expect(result[1].target.column).toBe('NodeName');
                await expect(result[2].target.column).toBe('Category');
                await expect(result[3].target.column).toBe('Element');
                await expect(result[4].target.column).toBe('CalculationDate');
                await expect(result[1].values[0]).toBe(values.nodeName);
                await expect(result[2].values[0]).toBe(values.categoryName);
                await expect(result[3].values[0]).toBe(values.elementName);
                await expect(result[5].target.table).toBe('KPIPreviousDateDataByCategoryElementNodeWithOwner');
                await expect(result[0].target.column).toBe('FilterType');
                await expect(result[39].target.table).toBe('BackupMovementDetailsWithOwner');
            });
        it('should return filter for without cutoff',
            async () => {
                const values = {
                    categoryName: 'Sistema',
                    elementName: 'Estación',
                    nodeName: 'SAN FERNANDO',
                    initialDate: '12/09/2019',
                    finalDate: '18/09/2019',
                    reportType: constants.Report.WithoutCutoff,
                    executionId: '59cc2fda-41a9-4e21-b239-a605270f0bac'
                };
                const result = cutoffReportFilterBuilder.cutoffReportFilterBuilder.build(values);
                await expect(result.length).toBe(11);
                await expect(result[0].target.table).toBe('DimDate');
                await expect(result[0].target.column).toBe('Date');
                await expect(result[1].target.table).toBe('ReportHeaderDetails');
                await expect(result[1].target.column).toBe('NodeName');
                await expect(result[2].target.column).toBe('Category');
                await expect(result[3].target.column).toBe('Element');
                await expect(result[4].target.table).toBe('Operational');
                await expect(result[4].target.column).toBe('ExecutionId');
            });

        it('should return filter for non son with owner report',
            async () => {
                const values = {
                    categoryName: 'Sistema',
                    elementName: 'Estación',
                    nodeName: 'SAN FERNANDO',
                    initialDate: '12/09/2019',
                    finalDate: '18/09/2019',
                    reportType: constants.Report.NonSonWithOwnerReport,
                    executionId: '59cc2fda-41a9-4e21-b239-a605270f0bae'
                };
                const result = cutoffReportFilterBuilder.cutoffReportFilterBuilder.build(values);
                await expect(result.length).toBe(9);
                await expect(result[0].target.table).toBe('ReportHeaderDetails');
                await expect(result[0].target.column).toBe('NodeName');
                await expect(result[1].target.table).toBe('ReportHeaderDetails');
                await expect(result[1].target.column).toBe('Category');
                await expect(result[2].target.table).toBe('ReportHeaderDetails');
                await expect(result[2].target.column).toBe('Element');
                await expect(result[3].target.table).toBe('DimDate');
                await expect(result[3].target.column).toBe('Date');
                await expect(result[4].target.table).toBe('OperationalInventoryOwnerNonSon');
                await expect(result[4].target.column).toBe('ExecutionId');
            });
    });
