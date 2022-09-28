import { pbiFilterBuilder } from '../../../common/services/pbiFilterBuilder';
import { dateService } from '../../../common/services/dateService';
import { systemConfigService } from '../../../common/services/systemConfigService';

const nodeStatusFilterBuilder = (function () {
    const filters = [];

    function buildColumnFilter(tableName, columnName, value) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, columnName, 'In', null, [value]));
    }

    function buildAdvancedDateFilter(tableName, columnName, values) {
        filters.push(pbiFilterBuilder.buildAdvancedPbiFilter(tableName, columnName, 'and',
            [{
                operator: 'GreaterThanOrEqual',
                value: dateService.parseToPBIString(values.initialDate)
            },
            {
                operator: 'LessThanOrEqual',
                value: dateService.parseToPBIString(values.finalDate)
            }]));
    }

    function buildCategoryElementFilter(tableName, value) {
        buildColumnFilter(tableName, 'Element', value);
    }

    function buildCategoryFilter(tableName) {
        buildColumnFilter(tableName, 'Category', 'Segmento');
    }

    function buildExecutionIdFilter(tableName) {
        buildColumnFilter(tableName, 'ExecutionId', systemConfigService.getSessionId());
    }

    function buildCategoryAndElementFilter(tableName, values) {
        buildCategoryFilter(tableName);
        buildCategoryElementFilter(tableName, values.elementName);
    }
    return {
        build: function (values) {
            filters.length = 0;
            buildCategoryAndElementFilter('ReportHeaderDetails', values);
            buildAdvancedDateFilter('DimDate', 'Date', values);
            buildCategoryAndElementFilter('TicketNodeStatus', values);
            buildExecutionIdFilter('TicketNodeStatus');
            buildAdvancedDateFilter('TicketNodeStatus', 'Startdate', values);
            buildAdvancedDateFilter('TicketNodeStatus', 'Enddate', values);
            return filters;
        }
    };
}());

export { nodeStatusFilterBuilder };


