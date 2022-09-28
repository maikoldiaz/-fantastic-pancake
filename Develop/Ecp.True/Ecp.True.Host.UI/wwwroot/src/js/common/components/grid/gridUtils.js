import React from 'react';
import { resourceProvider } from './../../services/resourceProvider';
import { TextboxFilter, SelectFilter, DateFilter } from './gridFilters.jsx';
import { utilities } from '../../services/utilities';
import Tooltip from './../../../common/components/tooltip/tooltip.jsx';
import classNames from 'classnames/bind';
import NumberFormatter from './../../../common/components/formControl/numberFormatter.jsx';

export const gridUtils = (function () {
    function addOptions(result, options, key) {
        if (utilities.hasProperty(options, 'sortable')) {
            result.sortable = options.sortable;
        } else {
            result.sortable = true;
        }

        if (utilities.hasProperty(options, 'filterable')) {
            result.filterable = options.filterable;
        } else {
            result.filterable = true;
        }

        if (result.sortable) {
            result.headerClassName = 'rt-sortable-header';
        }

        if (utilities.hasProperty(options, 'pivotValue')) {
            result.PivotValue = row => options.pivotValue(row);
        }

        if (utilities.hasProperty(options, 'footer')) {
            result.Footer = rows => options.footer(rows, key);
        }

        if (utilities.hasProperty(options, 'aggregate')) {
            result.aggregate = row => options.aggregate(row);
        }

        if (utilities.hasProperty(options, 'aggregated')) {
            result.Aggregated = row => options.aggregated(row);
        }

        if (utilities.hasProperty(options, 'hideFilterTextBox')) {
            result.hideFilterTextBox = options.hideFilterTextBox;
        } else {
            result.hideFilterTextBox = false;
        }

        if (utilities.hasProperty(options, 'onFiltered')) {
            result.onFiltered = options.onFiltered;
        }

        if (utilities.hasProperty(options, 'defaultValue')) {
            result.defaultValue = options.defaultValue;
        }
    }

    function getHeaderCell(title, rightAlign = false) {
        return (
            <Tooltip body={resourceProvider.read(title)}>
                <span className={classNames({ ['ep-table__cell-num']: rightAlign })}>{resourceProvider.read(title)}</span>
            </Tooltip>
        );
    }

    function buildTextColumn(key, gridProps, cell, title, options) {
        const columnTitle = utilities.isNullOrUndefined(title) ? key : title;
        const result = {
            Header: options && options.noHeader ? '' : getHeaderCell(columnTitle),
            accessor: key
        };

        if (options && options.width) {
            result.width = options.width;
        }

        if (cell) {
            result.Cell = cell;
        }

        let defaultType;
        if (utilities.toLowerCase(key) === 'movementid' || utilities.toLowerCase(key) === 'transactionid' || utilities.toLowerCase(key) === 'productid') {
            defaultType = 'text';
        } else {
            defaultType = utilities.toLowerCase(key).includes('id') ? 'number' : 'text';
        }
        const type = options && options.type ? options.type : defaultType;
        result.Filter = rowProps => <TextboxFilter {...gridProps} {...rowProps} type={type} />;
        result.type = type;

        addOptions(result, options, key);

        return result;
    }

    function buildNumberColumn(key, gridProps, title, options) {
        const columnTitle = title ? title : key;
        const result = {
            Header: getHeaderCell(columnTitle, true),
            accessor: key
        };

        result.Cell = r => (
            <NumberFormatter
                className="float-r"
                displayType="text"
                value={utilities.getValue(r.original, key) ? Number(utilities.getValue(r.original, key)) : ''}
            />);

        const defaultType = utilities.toLowerCase(key).includes('id') ? 'number' : 'text';
        const type = options && options.type ? options.type : defaultType;
        result.Filter = rowProps => <TextboxFilter {...gridProps} {...rowProps} type={type} />;
        result.type = type;

        addOptions(result, options, key);

        return result;
    }

    function buildDecimalColumn(key, gridProps, title, options) {
        const columnTitle = title ? title : key;
        // const decimals = options.decimals ? options.decimals : 2;
        const result = {
            Header: getHeaderCell(columnTitle, false),
            accessor: key
        };
        const defVal = options && !utilities.isNullOrUndefined(options.defaultVal) ? utilities.parseFloat(options.defaultVal) : '0.00';
        result.Cell = r => (<NumberFormatter
            className="float-r"
            displayType="text"
            value={utilities.getValue(r.original, key) ? utilities.parseFloat(utilities.getValue(r.original, key)) : defVal}
            prefix={utilities.hasProperty(options, 'prefix') ? options.prefix : null}
            suffix={utilities.hasProperty(options, 'suffix') ? options.suffix : null}
            isNumericString={true}
        />);

        const type = 'decimal';
        result.Filter = rowProps => <TextboxFilter {...gridProps} {...rowProps} type={type} isDecimal={true} />;
        result.type = type;

        addOptions(result, options, key);

        return result;
    }

    function buildEditableNumberColumn(key, gridProps, cell, title, options) {
        const columnTitle = title ? title : key;
        const result = {
            Header: getHeaderCell(columnTitle, true),
            accessor: key
        };

        if (cell) {
            result.Cell = cell;
        } else {
            result.Cell = r => <span className="float-r">{utilities.getValue(r.original, key) ? Number(utilities.getValue(r.original, key)) : ''}</span>;
        }
        const defaultType = utilities.toLowerCase(key).includes('id') ? 'number' : 'text';
        const type = options && options.type ? options.type : defaultType;
        result.Filter = rowProps => <TextboxFilter {...gridProps} {...rowProps} type={type} />;
        result.type = type;

        addOptions(result, options, key);

        return result;
    }

    function buildSelectColumn(key, gridProps, cell, title, options) {
        const columnTitle = title ? title : key;
        const result = {
            Header: getHeaderCell(columnTitle),
            accessor: key
        };

        if (options && options.width) {
            result.width = options.width;
        }

        if (cell) {
            result.Cell = cell;
        }

        result.Filter = rowProps => <SelectFilter {...gridProps} {...rowProps} options={options.values} />;
        result.type = 'select';

        addOptions(result, options, key);

        return result;
    }

    function buildDateColumn(key, gridProps, cell, title, options) {
        const columnTitle = title ? title : key;
        const result = {
            Header: getHeaderCell(columnTitle),
            accessor: key
        };
        if (options && options.width) {
            result.width = options.width;
        }
        if (cell) {
            result.Cell = cell;
        }

        result.Filter = rowProps => <DateFilter {...gridProps} {...rowProps} />;
        result.type = 'date';

        addOptions(result, options, key);

        return result;
    }

    function buildActionColumn(cell, title, width) {
        const result = {};

        result.Cell = cell;
        result.sortable = false;
        result.filterable = false;
        result.actionColumn = true;
        result.Header = getHeaderCell(title);
        if (width) {
            result.width = width;
        }

        return result;
    }

    return {
        buildTextColumn,
        buildNumberColumn,
        buildDecimalColumn,
        buildEditableNumberColumn,
        buildSelectColumn,
        buildDateColumn,
        buildActionColumn,
        buildHeaderColumn: (header, ...columns) => {
            return {
                Header: getHeaderCell(header),
                accessor: '',
                headerClassName: 'ep-table__col-group-header',
                columns
            };
        }
    };
}());
