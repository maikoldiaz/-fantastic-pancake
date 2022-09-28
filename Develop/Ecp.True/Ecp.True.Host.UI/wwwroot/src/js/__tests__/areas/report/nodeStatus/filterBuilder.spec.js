import * as nodeStatusFilterBuilder from '../../../../modules/report/nodeStatus/filterBuilder.js';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';

describe('Node Status Report Filter Builder Validate',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
        });
        it('should return filter for node status report',
            async () => {
                const values = {
                    elementName: 'Estación',
                    initialDate: '12/09/2019',
                    finalDate: '18/09/2019',
                    reportType: constants.Report.NodeStatusReport
                };
                const result = nodeStatusFilterBuilder.nodeStatusFilterBuilder.build(values);
                await expect(result.length).toBe(8);
                await expect(result[0].target.table).toBe('ReportHeaderDetails');
                await expect(result[0].target.column).toBe('Category');
                await expect(result[1].target.column).toBe('Element');
                await expect(result[2].target.table).toBe('DimDate');
                await expect(result[3].target.table).toBe('TicketNodeStatus');
            });
    });
