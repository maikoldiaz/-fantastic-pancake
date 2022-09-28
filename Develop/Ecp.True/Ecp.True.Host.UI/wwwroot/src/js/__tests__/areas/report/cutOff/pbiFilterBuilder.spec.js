import * as pbiFilterBuilder from '../../../../common/services/pbiFilterBuilder';
import { resourceProvider } from '../../../../common/services/resourceProvider';

describe('pbi Filter Builder Validate',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
        });
        it('should build filter',
            async () => {
                const table = 'View_RelationShipView';
                const column = 'Category';
                const operator = 'In';
                const value = null;
                const values = ['Tipo de Nodo'];
                const basicFilter = {
                    $schema: 'http://powerbi.com/product/schema#basic',
                    target: {
                        table: table,
                        column: column
                    },
                    operator: operator,
                    filterType: 1
                };
                if (value) {
                    basicFilter.value = value;
                } else {
                    basicFilter.values = values;
                }
                const advancedFilter = {
                    $schema: 'http://powerbi.com/product/schema#advanced',
                    target: {
                        table: table,
                        column: column
                    },
                    logicalOperator: null,
                    conditions: null,
                    filterType: 0
                };

                const basicFilterResult = pbiFilterBuilder.pbiFilterBuilder.buildBasicPbiFilter(table, column, operator, value, values);
                await expect(basicFilterResult).toStrictEqual(basicFilter);

                const advancedFilterResult = pbiFilterBuilder.pbiFilterBuilder.buildAdvancedPbiFilter(table, column, null, null);
                await expect(advancedFilterResult).toStrictEqual(advancedFilter);
            });
    });
