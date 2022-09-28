const pbiFilterBuilder = (function () {
    function buildPbiFilter(table, column, operator, value, values) {
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

        return basicFilter;
    }

    function buildAdvancedPbiFilter(table, column, logicalOperator, operatorValues) {
        const advancedFilter = {
            $schema: 'http://powerbi.com/product/schema#advanced',
            target: {
                table: table,
                column: column
            },
            logicalOperator: logicalOperator,
            conditions: operatorValues,
            filterType: 0
        };

        return advancedFilter;
    }

    function buildRelativeDatePbiFilter(table, column, dateConfig) {
        const relativeDateFilter = {
            $schema: 'http://powerbi.com/product/schema#relativeDate',
            target: {
                table: table,
                column: column
            },
            operator: dateConfig.operator,
            timeUnitsCount: dateConfig.timeUnitsCount,
            timeUnitType: dateConfig.timeUnitType,
            includeToday: dateConfig.includeToday,
            filterType: 4
        };

        return relativeDateFilter;
    }

    return {
        buildBasicPbiFilter: function (table, column, operator, value, values) {
            return buildPbiFilter(table, column, operator, value, values);
        },
        buildAdvancedPbiFilter: function (table, column, logicalOperator, operatorValues) {
            return buildAdvancedPbiFilter(table, column, logicalOperator, operatorValues);
        },
        buildRelativeDatePbiFilter: function (table, column, dateConfig) {
            return buildRelativeDatePbiFilter(table, column, dateConfig);
        }
    };
}());

export { pbiFilterBuilder };
