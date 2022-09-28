import React from 'react';
import QueryBuilder from './queryBuilder';
import GridNoData from './gridNoData.jsx';
import GridSelect from './gridSelect.jsx';
import GridPagination from './gridPagination.jsx';
import { resourceProvider } from './../../services/resourceProvider';
import { dispatcher } from './../../../common/store/dispatcher';
import { refreshGrid, selectGridData, selectAllGridData, clearSelection, applyPageFilter } from './actions';
import { utilities } from '../../services/utilities';
import { dateService } from '../../services/dateService';
import GridIncrementalPagination from './gridIncrementalPagination.jsx';

export default class GridService {
    constructor() {
        this.gridState = {
            config: {},
            props: {},
            items: {},
            page: {
                sorted: []
            },
            filters: {}
        };

        // Controlled State Callbacks
        this.onPageChange = this.onPageChange.bind(this);
        this.onPageSizeChange = this.onPageSizeChange.bind(this);
        this.onSortedChange = this.onSortedChange.bind(this);
        this.onFiltered = this.onFiltered.bind(this);
        this.buildFilterMethod = this.buildFilterMethod.bind(this);
        this.toggleSelection = this.toggleSelection.bind(this);
        this.toggleAll = this.toggleAll.bind(this);
        this.isSelected = this.isSelected.bind(this);
        this.onIncrementalPageChange = this.onIncrementalPageChange.bind(this);
        this.onPageReset = this.onPageReset.bind(this);
    }

    onPageChange(pageIndex) {
        const page = Object.assign({}, this.gridState.page, { page: pageIndex });
        this.gridState.page = page;

        if (this.shouldClear()) {
            dispatcher.dispatch(clearSelection(this.gridState.config.name));
        }

        dispatcher.dispatch(refreshGrid(this.gridState.config.name, true));
    }

    onPageReset() {
        const page = Object.assign({}, this.gridState.page, { page: 0 });
        this.gridState.page = page;
    }

    async onIncrementalPageChange(pageIndex, isNext) {
        if (this.gridState.onPageNavigation) {
            const currentPage = await this.gridState.onPageNavigation({ isNext });
            this.gridState.enablePrevious = currentPage === 1 ? false : true;
        }

        if (this.shouldClear()) {
            dispatcher.dispatch(clearSelection(this.gridState.config.name));
        }

        dispatcher.dispatch(refreshGrid(this.gridState.config.name, true));
    }

    onPageSizeChange(pageSize, pageIndex) {
        const page = Object.assign({}, this.gridState.page, {
            page: pageIndex,
            pageSize: Number(pageSize)
        });
        this.gridState.page = page;

        if (this.shouldClear()) {
            dispatcher.dispatch(clearSelection(this.gridState.config.name));
        }

        dispatcher.dispatch(refreshGrid(this.gridState.config.name, true));
    }

    onSortedChange(sorted) {
        // Build a new array and replace . with / for odata query to work on child elements
        const sort = [];
        sorted.forEach(s => {
            const obj = Object.assign({}, s);

            // supports 4 level nesting, hard coded for now
            obj.id = utilities.replace(obj.id.toString(), '.', '/');
            sort.push(obj);
        });

        const page = Object.assign({}, this.gridState.page, { sorted: sort });
        this.gridState.page = page;

        if (this.shouldClear()) {
            dispatcher.dispatch(clearSelection(this.gridState.config.name));
        }

        dispatcher.dispatch(refreshGrid(this.gridState.config.name, true));
    }

    onFiltered(value, id, key, isDecimal = false) {
        const onFiltered = this.gridState.filters[id];

        const page = Object.assign({}, this.gridState.page, { page: 0 });
        this.gridState.page = page;

        if (onFiltered) {
            onFiltered(value);
        } else {
            dispatcher.dispatch(applyPageFilter(this.gridState.config.name, { [id]: { value, id, key } }, isDecimal));
        }
    }

    buildGridState(propName, defaultValue) {
        return utilities.hasProperty(this.gridState.config, propName) ? this.gridState.config[propName] : defaultValue;
    }

    buildGridObjectState(propName, defaultValue) {
        if (utilities.hasProperty(this.gridState.config, propName)) {
            return (typeof (this.gridState.config[propName]) === 'object') || this.gridState.config[propName];
        }
        return defaultValue;
    }

    isColumnSortable(column) {
        if (utilities.getValue(column, 'actionColumn', false) === true || !this.gridState.props.sortable) {
            return false;
        }
        return utilities.getValue(column, 'sortable', true);
    }

    isColumnFilterable(column) {
        if (utilities.getValue(column, 'actionColumn', false) === true || !this.gridState.props.filterable) {
            return false;
        }
        return utilities.getValue(column, 'filterable', true);
    }

    buildGridApiUrl(filterValues, pageFilters, isDecimal) {
        let baseUrl = this.gridState.config.apiUrl;
        if (!this.gridState.props.odata) {
            return baseUrl;
        }

        const top = this.gridState.page.pageSize;
        const skip = top * (this.gridState.page.page);

        baseUrl = baseUrl.includes('?') ? `${baseUrl}&` : `${baseUrl}?`;
        const query = new QueryBuilder(this.gridState.config, this.gridState.overrides)
            .withPagination(top, skip, this.gridState.props.showPagination)
            .andSorting(this.gridState.config.sortable, this.gridState.page.sorted)
            .havingFilters(filterValues, pageFilters)
            .build(isDecimal);

        return `${baseUrl}${query}`;
    }

    toggleSelection(key) {
        dispatcher.dispatch(selectGridData(key.replace('select-', ''), this.gridState.config.name));
    }

    toggleAll() {
        dispatcher.dispatch(selectAllGridData(!this.gridState.props.selectAll, this.gridState.config.name));
    }

    shouldClear() {
        return this.gridState.props.odata === false && this.gridState.props.pageSelection === true;
    }

    isSelected(key) {
        const keyField = this.gridState.props.keyField;
        const result = this.gridState.props.selection.findIndex(p => p[keyField] === key) >= 0;
        return result;
    }

    containsFilter(filter, row) {
        return row[filter.id] && row[filter.id].toString().toLowerCase().includes(filter.value.toLowerCase());
    }

    numberFilter(filter, row) {
        return Number(row[filter.id]).toFixed(2) === Number(filter.value).toFixed(2);
    }

    dateFilter(filter, row) {
        if (filter.value) {
            return dateService.compare(row[filter.id], dateService.parseFieldToISOString(filter.value), true) === 0;
        }

        return true;
    }

    selectFilter(filter, row) {
        return utilities.isNullOrWhitespace(filter.value) || row[filter.id] === filter.value;
    }

    buildFilterMethod(type) {
        if (type === 'text') {
            return this.containsFilter.bind(this);
        }

        if (type === 'decimal') {
            return this.numberFilter.bind(this);
        }

        if (type === 'number') {
            return this.numberFilter.bind(this);
        }

        if (type === 'select') {
            return this.selectFilter.bind(this);
        }

        return this.dateFilter.bind(this);
    }

    defaultExpandedRows(items) {
        if (items.length > 0) {
            return items.map(() => true);
        }
        return null;
    }

    initialize(state) {
        this.gridState.config = Object.assign({}, state.config);
        this.gridState.items = Object.assign({}, state.items);
        this.gridState.overrides = {
            defaultFilter: state.defaultFilter,
            defaultSort: state.defaultSort
        };

        const props = {};

        // Page configuration
        props.showPagination = this.buildGridState('showPagination', true);
        props.pageSizeOptions = this.buildGridState('pageSizes', [10, 50, 100]);
        props.defaultPageSize = this.buildGridState('defaultPageSize', 10);
        props.showPageSizeOptions = this.buildGridState('showPageSizeOptions', true);
        props.pageButtonCount = this.buildGridState('pageButtonCount', 10);
        props.incrementalPagination = this.buildGridState('incrementalPagination', false);
        props.PaginationComponent = props.incrementalPagination ? GridIncrementalPagination : GridPagination;

        // Sort, Filter & Columns
        props.keyField = this.gridState.config.idField;
        props.minRows = this.buildGridState('minRows', 0);
        props.collapseOnSortingChange = false;
        props.collapseOnPageChange = false;
        props.collapseOnDataChange = false;
        props.freezeWhenExpanded = false;
        props.multiSort = false;
        props.odata = this.buildGridState('odata', true);
        props.sortable = this.buildGridObjectState('sortable', true);
        props.filterable = this.buildGridObjectState('filterable', true);
        props.resizable = this.buildGridState('resizable', true);
        props.defaultSortDesc = this.buildGridState('defaultSortDesc', true);
        props.columns = state.columns;
        props.onPageChange = this.onPageChange;
        props.onPageSizeChange = this.onPageSizeChange;
        props.onSortedChange = this.onSortedChange;
        props.onIncrementalPageChange = this.onIncrementalPageChange;

        // Configure callbacks for server side grid
        if (props.odata) {
            props.manual = true;

            // setup page configuration
            this.gridState.page = {
                page: 0,
                pageSize: props.defaultPageSize,
                sorted: [],
                filtered: []
            };
        }

        // Configure labels
        props.noDataText = resourceProvider.read('noGridData');
        props.ofText = resourceProvider.read('of');
        props.rowsText = resourceProvider.read('rows');
        props.rowsSelectorText = resourceProvider.read('rowsPerPage');

        if (this.gridState.config.pivotBy) {
            props.pivotBy = this.gridState.config.pivotBy;
        }

        // Add selection section
        const selectable = this.buildGridState('selectable', false);
        if (utilities.checkIfBoolean(selectable)) {
            props.pageSelection = true;
        } else {
            props.pageSelection = utilities.hasProperty(selectable, 'pageSelection') ? selectable.pageSelection : true;
        }
        props.selectable = utilities.checkIfBoolean(selectable) ? selectable : true;

        if (props.selectable) {
            props.selectType = 'checkbox';
            props.toggleSelection = this.toggleSelection;
            props.toggleAll = this.toggleAll;
            props.selectAll = this.selectAll;
            props.selection = [];
            props.isSelected = this.isSelected;
            props.SelectInputComponent = GridSelect;
            props.SelectAllInputComponent = GridSelect;
        }

        // Add custom component
        const noDataComp = gridProps => <GridNoData {...gridProps} onNoData={this.gridState.onNoData} name={this.gridState.config.name} />;
        props.NoDataComponent = noDataComp;

        // Add Sub Component
        if (this.gridState.config.expandable && this.gridState.config.expandable.component) {
            const SubComponent = this.gridState.config.expandable.component;
            props.SubComponent = gridProps => <SubComponent {...gridProps}/>;
        }

        this.gridState.props = props;
    }

    process(props) {
        this.gridState.onPageNavigation = props.onPageNavigation;
        this.gridState.items = props.items;
        this.gridState.props.columns = props.columns;
        this.gridState.props.totalItems = props.totalItems;
        this.gridState.props.page = this.gridState.page.page;
        this.gridState.props.pageSize = this.gridState.page.pageSize;
        this.gridState.onNoData = props.onNoData;
        this.gridState.props.enablePrevious = this.gridState.enablePrevious;

        this.gridState.props.columns.forEach(c => {
            if (c.columns && c.columns.length > 0) {
                c.columns.forEach(ic => {
                    this.gridState.filters[ic.accessor] = ic.onFiltered;
                    ic.sortable = this.isColumnSortable(ic);
                    ic.filterable = this.isColumnFilterable(ic);
                    if (!this.gridState.props.odata) {
                        ic.filterMethod = this.buildFilterMethod(ic.type);
                    } else if (ic.Filter) {
                        ic.onFilter = this.onFiltered;
                    }
                });
            } else if (!this.gridState.props.odata) {
                this.gridState.filters[c.accessor] = c.onFiltered;
                c.sortable = this.isColumnSortable(c);
                c.filterable = this.isColumnFilterable(c);
                c.filterMethod = this.buildFilterMethod(c.type);
            } else if (c.Filter) {
                this.gridState.filters[c.accessor] = c.onFiltered;
                c.sortable = this.isColumnSortable(c);
                c.filterable = this.isColumnFilterable(c);
                c.onFilter = this.onFiltered;
            }
        });

        if (this.gridState.props.selectable) {
            this.gridState.props.selectAll = props.selectAll;
            this.gridState.props.selection = props.selection;
            this.gridState.props.isSelected = this.isSelected;
        }

        if (this.gridState.config.expandable && this.gridState.config.expandable.is) {
            this.gridState.props.defaultExpanded = this.defaultExpandedRows(this.gridState.items);
        }

        return {
            gridState: this.gridState
        };
    }
}
