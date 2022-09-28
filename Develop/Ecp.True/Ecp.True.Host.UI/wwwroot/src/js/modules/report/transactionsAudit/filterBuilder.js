import { pbiFilterBuilder } from '../../../common/services/pbiFilterBuilder';
import { dateService } from '../../../common/services/dateService';
import { constants } from '../../../common/services/constants';

const transactionsAuditReportFilterBuilder = (function () {
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

    function buildCategoryElementFilter(tableName, columnName, value) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, columnName, 'In', null, [value]));
    }

    function buildCategoryFilter(tableName) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, 'Category', 'In', null, ['Segmento']));
    }

    function buildWithMovementFilters(values) {
        buildCategoryFilter('ReportHeaderDetails');
        buildCategoryElementFilter('ReportHeaderDetails', 'Element', values.elementName);
        buildAdvancedDateFilter('DimDate', 'Date', values);
        buildCategoryElementFilter('Movement', 'SegmentNameFilter', values.elementName);
        return filters;
    }

    function buildWithInventoryFilters(values) {
        buildCategoryFilter('ReportHeaderDetails');
        buildCategoryElementFilter('ReportHeaderDetails', 'Element', values.elementName);
        buildAdvancedDateFilter('DimDate', 'Date', values);
        buildCategoryElementFilter('InventoryProduct', 'SegmentNameFilter', values.elementName);
        return filters;
    }

    return {
        build: function (values) {
            filters.length = 0;
            return values.reportType === constants.Report.MovementAuditReport ? buildWithMovementFilters(values) : buildWithInventoryFilters(values);
        }
    };
}());


export { transactionsAuditReportFilterBuilder };
