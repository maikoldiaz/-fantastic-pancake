import * as settingsAuditReportFilterBuilder from '../../../../modules/report/settingsAudit/filterBuilder';
import { resourceProvider } from '../../../../common/services/resourceProvider';

describe('Settings Audit Report Filter Builder Validate',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
        });
        it('should return filter for settings Audit Configurations report',
            async () => {
                const values = {
                    initialDate: '12/09/2019',
                    finalDate: '18/09/2019'
                };
                const result = settingsAuditReportFilterBuilder.settingsAuditReportFilterBuilder.build(values);
                await expect(result.length).toBe(1);
                await expect(result[0].target.table).toBe('DimDate');
            });
    });
