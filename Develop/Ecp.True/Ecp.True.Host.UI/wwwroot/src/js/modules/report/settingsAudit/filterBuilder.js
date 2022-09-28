import { pbiFilterBuilder } from '../../../common/services/pbiFilterBuilder';
import { dateService } from '../../../common/services/dateService';

const settingsAuditReportFilterBuilder = (function () {
    const filters = [];

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

    function buildWithDateFilters(values) {
        buildAdvancedDateFilter('DimDate', 'Date', values);
    }

    return {
        build: function (values) {
            filters.length = 0;
            buildWithDateFilters(values);
            return filters;
        }
    };
}());


export { settingsAuditReportFilterBuilder };
