import * as nodeConfigurationFilterBuilder from '../../../../modules/report/nodeConfiguration/filterBuilder.js';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';

describe('Node Configuration Report Filter Builder Validate',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
        });
        it('should return filter for node configuration report',
            async () => {
                const values = {
                    elementName: 'Estación',
                    nodeName: 'SAN FERNANDO',
                    reportType: constants.Report.NodeConfigurationReport
                };
                const result = nodeConfigurationFilterBuilder.nodeConfigurationFilterBuilder.build(values);
                await expect(result.length).toBe(11);
                await expect(result[0].target.table).toBe('ReportHeaderDetails');
                await expect(result[0].target.column).toBe('Category');
                await expect(result[1].target.column).toBe('Element');
                await expect(result[2].target.table).toBe('NodeGeneralInfo');
                await expect(result[5].target.table).toBe('NodeProductInfo');
                await expect(result[8].target.table).toBe('NodeConnectionInfo');
            });
    });
