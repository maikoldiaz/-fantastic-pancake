import * as balanceControlChartFilterBuilder from '../../../../modules/report/balanceControl/filterBuilder.js';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';

describe('CutOff Report Filter Builder Validate',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
        });
        it('should return filter for balance control chart report',
            async () => {
                const values = {
                    elementName: 'Estación',
                    nodeName: 'SAN FERNANDO',
                    initialDate: '12/09/2019',
                    finalDate: '18/09/2019',
                    reportType: constants.Report.BalanceControlChart
                };
                const result = balanceControlChartFilterBuilder.balanceControlChartFilterBuilder.build(values);
                await expect(result.length).toBe(8);
                await expect(result[0].target.table).toBe('ReportHeaderDetails');
                await expect(result[0].target.column).toBe('Category');
                await expect(result[1].target.column).toBe('Element');
                await expect(result[2].values[0]).toBe(values.nodeName);
                await expect(result[3].target.table).toBe('BalanceControl');
                await expect(result[7].target.table).toBe('DimDate');
            });
    });
