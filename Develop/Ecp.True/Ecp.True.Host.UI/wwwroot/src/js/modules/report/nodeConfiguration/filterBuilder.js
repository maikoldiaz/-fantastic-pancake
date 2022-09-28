import { pbiFilterBuilder } from '../../../common/services/pbiFilterBuilder';
import { systemConfigService } from '../../../common/services/systemConfigService';

const nodeConfigurationFilterBuilder = (function () {
    const filters = [];

    function buildCategoryElementFilter(tableName, value) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, 'Element', 'In', null, [value]));
    }

    function buildCategoryFilter(tableName, value) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, 'Category', 'In', null, [value]));
    }

    function buildExecutionIdFilter(tableName) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, 'ExecutionId', 'In', null, [systemConfigService.getSessionId()]));
    }

    function buildFilters(tableName, values) {
        buildCategoryFilter(tableName, values.categoryName);
        buildCategoryElementFilter(tableName, values.elementName);
        buildExecutionIdFilter(tableName);
    }

    return {
        build: function (values) {
            filters.length = 0;
            buildCategoryFilter('ReportHeaderDetails', values.categoryName);
            buildCategoryElementFilter('ReportHeaderDetails', values.elementName);
            buildFilters('NodeGeneralInfo', values);
            buildFilters('NodeProductInfo', values);
            buildFilters('NodeConnectionInfo', values);
            return filters;
        }
    };
}());

export { nodeConfigurationFilterBuilder };


