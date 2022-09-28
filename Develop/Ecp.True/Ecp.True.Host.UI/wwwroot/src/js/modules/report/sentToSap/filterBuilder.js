import { pbiFilterBuilder } from '../../../common/services/pbiFilterBuilder';
import { dateService } from '../../../common/services/dateService';

const sentToSapReportFilterBuilder = (function () {
    const filters = [];

    function buildExecutionIdFilter(tableName, executionId) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, 'ExecutionId', 'In', null, [executionId]));
    }

    function buildCalculationDateFilter(tableName, calcDateName, values) {
        filters.push(pbiFilterBuilder.buildAdvancedPbiFilter(tableName, calcDateName, 'and',
            [{
                operator: 'GreaterThanOrEqual',
                value: dateService.parseToPBIString(values.initialDate)
            },
            {
                operator: 'LessThanOrEqual',
                value: dateService.parseToPBIString(values.finalDate)
            }]));
    }

    function buildCategoryElementFilter(tableName, categoryName, elementName, values) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, categoryName, 'In', null, [values.categoryName]));
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, elementName, 'In', null, [values.elementName]));
    }

    return {
        build: function (values) {
            filters.length = 0;

            buildCalculationDateFilter('DimDate', 'Date', values);
            buildCategoryElementFilter('ReportHeaderDetails', 'Category', 'Element', values);
            buildExecutionIdFilter('MovementSendSapInformation', values.executionId);
            buildExecutionIdFilter('ReportHeaderDetailsExecution', values.executionId);
            return filters;
        }
    };
}());

export { sentToSapReportFilterBuilder };
