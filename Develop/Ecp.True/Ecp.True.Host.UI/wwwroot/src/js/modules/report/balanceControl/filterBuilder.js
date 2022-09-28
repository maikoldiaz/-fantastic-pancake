import { pbiFilterBuilder } from '../../../common/services/pbiFilterBuilder';
import { dateService } from '../../../common/services/dateService';
import { utilities } from '../../../common/services/utilities';

const balanceControlChartFilterBuilder = (function () {
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

    function buildNodeNameInFilter(tableName, value) {
        if (!utilities.isNullOrWhitespace(value) && value !== 'Todos') {
            filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, 'NodeName', 'In', null, [value]));
        }
    }

    function buildCategoryElementFilter(tableName, value) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, 'Element', 'In', null, [value]));
    }

    function buildCategoryFilter(tableName) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, 'Category', 'In', null, ['Segmento']));
    }

    function buildAllFilters(tableName, values) {
        buildCategoryElementFilter(tableName, values.elementName);
        buildNodeNameInFilter(tableName, values.nodeName);
        buildAdvancedDateFilter(tableName, 'CalculationDate', values);
        buildCategoryFilter(tableName);
    }

    return {
        build: function (values) {
            filters.length = 0;
            buildCategoryFilter('ReportHeaderDetails');
            buildCategoryElementFilter('ReportHeaderDetails', values.elementName);
            buildNodeNameInFilter('ReportHeaderDetails', values.nodeName);
            buildAllFilters('BalanceControl', values);
            buildAdvancedDateFilter('DimDate', 'Date', values);
            return filters;
        }
    };
}());

export { balanceControlChartFilterBuilder };
