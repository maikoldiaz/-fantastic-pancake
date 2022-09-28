import { pbiFilterBuilder } from '../../services/pbiFilterBuilder';
import { constants } from '../../services/constants';
import { dateService } from '../../services/dateService';

const reportsFilterBuilder = (function () {
    const filters = [];

    function buildBasicFilter(tableName, columnName, value) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, columnName, 'In', null, [value]));
    }

    function buildCategoryElementFilter(tableName, columnName, value) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, columnName, 'In', null, [value]));
    }

    function buildCategoryFilter(tableName) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, 'Category', 'In', null, ['Segmento']));
    }

    function buildNodeNameInFilter(tableName, columnName, value) {
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

    function buildExecutionIdFilter(tableName, executionId) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, 'ExecutionId', 'In', null, [executionId]));
    }

    function buildPendingBalanceFilters(values) {
        buildCategoryFilter('ReportHeaderDetails');
        buildCategoryElementFilter('ReportHeaderDetails', 'Element', values.element.name);
        buildCategoryElementFilter('MovementGroup', 'SegmentId', values.element.elementId);
        buildAdvancedDateFilter('DimDate', 'Date', values);
        buildAdvancedDateFilter('MovementGroup', 'StartDate', values);
        return filters;
    }

    function buildInitialExecution(values) {
        buildExecutionIdFilter('SysVersion', values.executionId);
        buildExecutionIdFilter('OfficialMonthlyBalance', values.executionId);
        buildExecutionIdFilter('OfficialMonthlyMovementDetails', values.executionId);
        buildExecutionIdFilter('OfficialMonthlyMovementQualityDetails', values.executionId);
        buildExecutionIdFilter('OfficialMonthlyInventoryDetails', values.executionId);
        buildExecutionIdFilter('OfficialMonthlyInventoryQualityDetails', values.executionId);
    }

    function buildOfficialBalancePerNodeFilters(values) {
        buildCategoryFilter('ReportHeaderDetails');
        buildCategoryElementFilter('ReportHeaderDetails', 'Element', values.elementName);
        buildNodeNameInFilter('ReportHeaderDetails', 'NodeName', values.nodeName);
        buildAdvancedDateFilter('DimDate', 'Date', values);
        buildBasicFilter('OfficialDeltaBalance', 'NodeId', values.nodeId);
        buildBasicFilter('OfficialDeltaBalance', 'SegmentId', values.elementId);
        buildBasicFilter('OfficialDeltaBalance', 'StartDate', dateService.parseToPBIString(values.initialDate));
        buildBasicFilter('OfficialDeltaBalance', 'EndDate', dateService.parseToPBIString(values.finalDate));
        buildBasicFilter('OfficialDeltaMovements', 'NodeId', values.nodeId);
        buildBasicFilter('OfficialDeltaMovements', 'SegmentId', values.elementId);
        buildBasicFilter('OfficialDeltaMovements', 'StartDate', dateService.parseToPBIString(values.initialDate));
        buildBasicFilter('OfficialDeltaMovements', 'EndDate', dateService.parseToPBIString(values.finalDate));
        buildBasicFilter('OfficialDeltaInventory', 'NodeId', values.nodeId);
        buildBasicFilter('OfficialDeltaInventory', 'SegmentId', values.elementId);
        values.initialDate = dateService.subtract(dateService.parse(values.initialDate), 1, 'd');
        buildAdvancedDateFilter('OfficialDeltaInventory', 'Date', values);

        return filters;
    }

    function buildInitialBalanceFilters(values) {
        buildCategoryFilter('ReportHeaderDetails');
        buildCategoryElementFilter('ReportHeaderDetails', 'Element', values.elementName);
        buildNodeNameInFilter('ReportHeaderDetails', 'NodeName', values.nodeName);
        buildAdvancedDateFilter('DimDate', 'Date', values);
        buildInitialExecution(values);
        return filters;
    }

    return {
        build: function (values) {
            filters.length = 0;
            if (values.reportType === constants.Report.OfficialBalancePerNodeReport) {
                return buildOfficialBalancePerNodeFilters(values);
            } else if (values.reportType === constants.Report.OfficialInitialBalanceReport) {
                return buildInitialBalanceFilters(values);
            }
            return buildPendingBalanceFilters(values);
        }
    };
}());


export { reportsFilterBuilder };
