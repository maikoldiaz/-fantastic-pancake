import { pbiFilterBuilder } from '../../../common/services/pbiFilterBuilder';
import { dateService } from '../../../common/services/dateService';
import { utilities } from '../../../common/services/utilities';
import { constants } from '../../../common/services/constants';

const cutoffReportFilterBuilder = (function () {
    const filters = [];

    function buildFilterTypeFilter(tableName, filterTypeValue) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, 'FilterType', 'In', null, [filterTypeValue]));
    }

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

    function buildNodeNameInFilter(tableName, nodeName, values) {
        if (!utilities.isNullOrWhitespace(values.nodeName) && values.nodeName !== 'Todos') {
            filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, nodeName, 'In', null, [values.nodeName]));
        }
    }

    function buildCategoryElementCalcDateFilter(tableName, categoryName, elementName, calcDateName, values) {
        buildCategoryElementFilter(tableName, categoryName, elementName, values);
        buildCalculationDateFilter(tableName, calcDateName, values);
    }


    function buildInCalcDateFilter(tableName, values) {
        buildNodeNameInFilter(tableName, 'NodeName', values);
        buildCategoryElementCalcDateFilter(tableName, 'Category', 'Element', 'CalculationDate', values);
    }

    function buildInCalcDatePrevFilter(tableName, values) {
        buildNodeNameInFilter(tableName, 'NodeNamePrev', values);
        buildCategoryElementCalcDateFilter(tableName, 'CategoryPrev', 'ElementPrev', 'CalculationDatePrev', values);
    }

    function buildContainsFilter(tableName, values, movement = false) {
        if (!utilities.isNullOrWhitespace(values.nodeName) && (movement ? movement : values.nodeName !== 'Todos')) {
            filters.push(pbiFilterBuilder.buildAdvancedPbiFilter(tableName, 'NodeName', 'and',
                [{
                    operator: 'Contains',
                    value: ''.concat('-_-', values.nodeName, '-_-')
                }]));
        }
        buildCategoryElementCalcDateFilter(tableName, 'Category', 'Element', 'CalculationDate', values);
    }

    function buildFilterType(filterTypeViewName, values, suffix) {
        if (values.categoryName === 'Segmento') {
            if (values.nodeName === 'Todos') {
                buildFilterTypeFilter(filterTypeViewName, 'Segment' + suffix);
            } else {
                buildFilterTypeFilter(filterTypeViewName, suffix);
            }
        } else if (values.categoryName === 'Sistema') {
            if (values.nodeName === 'Todos') {
                buildFilterTypeFilter(filterTypeViewName, 'System' + suffix);
            } else {
                buildFilterTypeFilter(filterTypeViewName, suffix);
            }
        }
    }

    function buildCutoffFilters(values, modifiedValues) {
        buildFilterType('KPIDataByCategoryElementNode', values, 'Unbalance');
        buildInCalcDateFilter('KPIDataByCategoryElementNode', values);
        buildFilterType('KPIPreviousDateDataByCategoryElementNode', values, 'Unbalance');
        buildInCalcDatePrevFilter('KPIPreviousDateDataByCategoryElementNode', modifiedValues);
        buildContainsFilter('MovementDetailsWithoutOwner', values, true);
        buildContainsFilter('AttributeDetailsWithoutOwner', values);
        buildInCalcDateFilter('MovementsByProductWithoutOwner', values);
        buildFilterType('MovementsByProductWithoutOwner', values, 'Unbalance');
        buildNodeNameInFilter('ReportHeaderDetails', 'NodeName', values);
        buildCategoryElementFilter('ReportHeaderDetails', 'Category', 'Element', values);
        buildCalculationDateFilter('DimDate', 'Date', values);
        buildContainsFilter('BackupMovementDetailsWithoutOwner', values);
        values.initialDate = dateService.subtract(dateService.parse(values.initialDate), 1, 'days');
        buildInCalcDateFilter('InventoryDetailsWithoutOwner', values);
        buildInCalcDateFilter('QualityDetailsWithoutOwner', values);
    }

    function buildNonOperationalSegmentOwnershipFilters(values) {
        buildNodeNameInFilter('ReportHeaderDetails', 'NodeName', values);
        buildCategoryElementFilter('ReportHeaderDetails', 'Category', 'Element', values);
        buildCalculationDateFilter('DimDate', 'Date', values);
        buildExecutionIdFilter('OperationalInventoryOwnerNonSon', values.executionId);
        buildExecutionIdFilter('OperationalInventoryQualityNonSon', values.executionId);
        buildExecutionIdFilter('OperationalMovementOwnerNonSon', values.executionId);
        buildExecutionIdFilter('OperationalMovementQualityNonSon', values.executionId);
        buildExecutionIdFilter('OperationalNonSon', values.executionId);
    }

    function buildOwnershipFilters(values, modifiedValues) {
        buildFilterType('KPIDataByCategoryElementNodeWithOwnership', values, 'Ownership');
        buildInCalcDateFilter('KPIDataByCategoryElementNodeWithOwnership', values);
        buildFilterType('KPIPreviousDateDataByCategoryElementNodeWithOwner', values, 'Ownership');
        buildInCalcDatePrevFilter('KPIPreviousDateDataByCategoryElementNodeWithOwner', modifiedValues);
        buildInCalcDateFilter('MovementsByProductWithOwner', values);
        buildFilterType('MovementsByProductWithOwner', values, 'Ownership');
        buildContainsFilter('MovementDetailsWithOwner', values, true);
        buildContainsFilter('MovementDetailsWithOwnerOtherSegment', values, true);
        buildContainsFilter('AttributeDetailsWithOwner', values);
        buildNodeNameInFilter('ReportHeaderDetails', 'NodeName', values);
        buildCategoryElementFilter('ReportHeaderDetails', 'Category', 'Element', values);
        buildCalculationDateFilter('DimDate', 'Date', values);
        const subtractedInitialDateValues = Object.assign({}, values, {
            initialDate: dateService.subtract(dateService.parse(values.initialDate), 1, 'days')
        });
        buildInCalcDateFilter('InventoryDetailsWithOwner', subtractedInitialDateValues);
        buildInCalcDateFilter('QualityDetailsWithOwner', subtractedInitialDateValues);
        buildContainsFilter('BackupMovementDetailsWithOwner', values);
        buildInCalcDateFilter('MovementsByProductWithOwnerWithOwnership', values);
    }

    function buildWithoutCutoffFilters(values) {
        buildCalculationDateFilter('DimDate', 'Date', values);
        buildNodeNameInFilter('ReportHeaderDetails', 'NodeName', values);
        buildCategoryElementFilter('ReportHeaderDetails', 'Category', 'Element', values);
        buildExecutionIdFilter('Operational', values.executionId);
        buildExecutionIdFilter('OperationalMovement', values.executionId);
        buildExecutionIdFilter('OperationalMovementQuality', values.executionId);
        buildExecutionIdFilter('OperationalMovementOwner', values.executionId);
        buildExecutionIdFilter('OperationalInventory', values.executionId);
        buildExecutionIdFilter('OperationalInventoryQuality', values.executionId);
        buildExecutionIdFilter('OperationalInventoryOwner', values.executionId);
    }

    return {
        build: function (values) {
            filters.length = 0;
            const diff = dateService.getDiff(values.finalDate, values.initialDate, 'd');
            const updatedDiff = diff + 1;
            const modifiedInitialDate = dateService.subtract(dateService.parse(values.initialDate), updatedDiff, 'days');
            const modifiedFinalDate = dateService.subtract(dateService.parse(values.finalDate), updatedDiff, 'days');
            const modifiedValues = Object.assign({}, values, {
                initialDate: modifiedInitialDate,
                finalDate: modifiedFinalDate
            });
            if (values.reportType === constants.Report.WithoutCutoff) {
                buildWithoutCutoffFilters(values);
            } else if (values.reportType === constants.Report.WithoutOwner) {
                buildCutoffFilters(values, modifiedValues);
            } else if (values.reportType === constants.Report.NonSonWithOwnerReport) {
                buildNonOperationalSegmentOwnershipFilters(values);
            } else {
                buildOwnershipFilters(values, modifiedValues);
            }
            return filters;
        }
    };
}());

export { cutoffReportFilterBuilder };
