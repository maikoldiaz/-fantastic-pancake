import { pbiFilterBuilder } from '../../../common/services/pbiFilterBuilder';
import { dateService } from '../../../common/services/dateService';
import { utilities } from '../../../common/services/utilities';
import { systemConfigService } from '../../../common/services/systemConfigService';

const eventContractFilterBuilder = (function () {
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

    function buildNodeNameInFilter(tableName, columnName, value) {
        if (!utilities.isNullOrWhitespace(value) && value !== 'Todos') {
            filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, columnName, 'In', null, [value]));
        }
    }

    function buildCategoryElementFilter(tableName, columnName, value) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, columnName, 'In', null, [value]));
    }

    function buildCategoryFilter(tableName) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, 'Category', 'In', null, ['Segmento']));
    }

    function buildExecutionIdFilter(tableName) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, 'ExecutionId', 'In', null, [systemConfigService.getSessionId()]));
    }

    function buildViewFilters(tableName, values) {
        buildCategoryElementFilter(tableName, 'InputElement', values.elementName);
        buildNodeNameInFilter(tableName, 'InputNodeName', values.nodeName);
        buildExecutionIdFilter(tableName);
    }

    return {
        build: function (values) {
            filters.length = 0;
            buildCategoryFilter('ReportHeaderDetails');
            buildCategoryElementFilter('ReportHeaderDetails', 'Element', values.elementName);
            buildNodeNameInFilter('ReportHeaderDetails', 'NodeName', values.nodeName);
            buildAdvancedDateFilter('DimDate', 'Date', values);
            buildViewFilters('EventInformation', values);
            buildViewFilters('ContractInformation', values);
            return filters;
        }
    };
}());

export { eventContractFilterBuilder };


