import GridService from '../../../../common/components/grid/gridService.js';
import { dispatcher } from '../../../../common/store/dispatcher.js';
import QueryBuilder from '../../../../common/components/grid/queryBuilder.js';

describe('Actions for gridService', () => {
    const gridProps = {
        name: 'categoriesGrid',
        columns: [
            { Header: 'Category Id', accessor: 'categoryId', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Name', accessor: 'name', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Description', accessor: 'description', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Created Date', accessor: 'createdDate', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Active', accessor: 'isActive', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Grouper', accessor: 'isGrouper', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: '', accessor: '', sortable: false, filterable: false }
        ],
        config: { filterable: { filterBuilder: jest.fn() }, name: 'categoriesGrid', idField: 'categoryId', apiUrl: 'http://localhost:23062/v1/odata/categories' },
        items: [], totalItems: 0, selection: [], selectAll: false,
        pageFilters: {},
        page: {}
    };

    const gridPropsWithSortableAndFilterable = {
        name: 'categoriesGrid',
        columns: [
            { Header: 'Category Id', accessor: 'categoryId', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Name', accessor: 'name', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Description', accessor: 'description', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Created Date', accessor: 'createdDate', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Active', accessor: 'isActive', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Grouper', accessor: 'isGrouper', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: '', accessor: '', sortable: false, filterable: false }
        ],
        config: { name: 'categoriesGrid', idField: 'categoryId', apiUrl: 'http://localhost:23062/v1/odata/categories', sortable: false, filterable: false },
        items: [], totalItems: 0, selection: [], selectAll: false,
        pageFilters: {}
    };

    it('should process', () => {
        const gridService = new GridService();
        gridService.initialize(gridProps);
        gridService.process(gridProps);
        expect(gridService.gridState).toBeDefined();
    });

    it('should toggleAll', () => {
        const gridService = new GridService();
        gridService.initialize(gridProps);
        dispatcher.dispatch = jest.fn(() => {
            return 1;
        });
        gridService.toggleAll();
        expect(dispatcher.dispatch.mock.calls).toHaveLength(1);
        expect(dispatcher.dispatch.mock.results[0].value).toEqual(1);
    });

    it('should toggleSelection', () => {
        const gridService = new GridService();
        gridService.initialize(gridProps);
        dispatcher.dispatch = jest.fn(() => {
            return 1;
        });
        gridService.toggleSelection('select-');
        expect(dispatcher.dispatch.mock.calls).toHaveLength(1);
        expect(dispatcher.dispatch.mock.results[0].value).toEqual(1);
    });

    it('should onPageChange', () => {
        const gridService = new GridService();
        gridService.initialize(gridProps);
        dispatcher.dispatch = jest.fn(() => {
            return 1;
        });
        gridService.onPageChange(1);
        expect(dispatcher.dispatch.mock.calls).toHaveLength(1);
        expect(dispatcher.dispatch.mock.results[0].value).toEqual(1);
    });

    it('should onPageSizeChange', () => {
        const gridService = new GridService();
        gridService.initialize(gridProps);
        dispatcher.dispatch = jest.fn(() => {
            return 1;
        });
        gridService.onPageSizeChange(10, 1);
        expect(dispatcher.dispatch.mock.calls).toHaveLength(1);
        expect(dispatcher.dispatch.mock.results[0].value).toEqual(1);
    });

    it('should onSortedChange', () => {
        const gridService = new GridService();
        gridService.initialize(gridProps);
        dispatcher.dispatch = jest.fn(() => {
            return 1;
        });
        gridService.onSortedChange([{ id: 'nodeId', desc: true }]);
        expect(dispatcher.dispatch.mock.calls).toHaveLength(1);
        expect(dispatcher.dispatch.mock.results[0].value).toEqual(1);
    });

    it('should buildGridState', () => {
        const gridService = new GridService();
        gridService.initialize(gridProps);
        const returned = gridService.buildGridState('filtered', 'node');
        expect(returned).toEqual('node');
    });

    it('should buildGridApiUrl', () => {
        const gridService = new GridService();
        gridService.initialize(gridProps);
        const returned = gridService.buildGridApiUrl({}, {});
        expect(returned).toEqual(`${gridProps.config.apiUrl}?$top=10`);
    });

    it('should buildQuery', () => {
        const gridState = {
            page: { page: 0, pageSize: 10, sorted: [], filtered: {} },
            config: { filterable: { defaultFilter: 'isHomologation eq false' } },
            overrides: { defaultFilter: [] },
            sorted: [{}]
        };
        const queryBuilder = new QueryBuilder(gridState.config, gridState.overrides);
        const top = gridState.page.pageSize;
        const skip = top * (gridState.page.page);
        const sortable = { defaultSort: 'createdDate desc' };
        const filterValues = {};

        const returned = queryBuilder.withPagination(top, skip, true).andSorting(sortable, gridState.page.sorted).havingFilters(filterValues, gridState.page.filtered).build();
        expect(returned).toEqual('$top=10&$orderby=createdDate desc&$filter=isHomologation eq false');
    });

    it('should buildGridObjectState', () => {
        const gridService = new GridService();
        gridService.initialize(gridProps);
        const returned = gridService.buildGridObjectState('sortable', true);
        expect(returned).toEqual(true);
    });

    it('should buildGridObjectState for new config(filterable)', () => {
        const gridService = new GridService();
        gridService.initialize(gridPropsWithSortableAndFilterable);
        const returned = gridService.buildGridObjectState('sortable', true);
        expect(returned).toEqual(gridPropsWithSortableAndFilterable.config.sortable);
        const filterable = gridService.buildGridObjectState('filterable', true);
        expect(filterable).toEqual(gridPropsWithSortableAndFilterable.config.filterable);
    });

    it('should isColumnSortable', () => {
        const gridService = new GridService();
        gridService.initialize(gridProps);
        let column = { actionColumn: true }; // Action  Column
        let returned = gridService.isColumnSortable(column);
        expect(returned).toEqual(false);
        column = {}; // Non Action  Column, but grid and column are by default sortable
        returned = gridService.isColumnSortable(column);
        expect(returned).toEqual(true);
        column = { sortable: false }; // Non Action  Column, grid is by default sortable, column not sortable
        returned = gridService.isColumnSortable(column);
        expect(returned).toEqual(false);
        gridService.initialize(gridPropsWithSortableAndFilterable); // Grid with Sort and Filter are false
        column = { sortable: true }; // Non Action  Column, but grid is by default not sortable
        returned = gridService.isColumnSortable(column);
        expect(returned).toEqual(false);
    });

    it('should isColumnFilterable', () => {
        const gridService = new GridService();
        gridService.initialize(gridProps);
        let column = { actionColumn: true }; // Action  Column
        let returned = gridService.isColumnFilterable(column);
        expect(returned).toEqual(false);
        column = {}; // Non Action  Column, but grid and column are by default filterable
        returned = gridService.isColumnFilterable(column);
        expect(returned).toEqual(true);
        column = { filterable: false }; // Non Action  Column, grid is by default filterable, column not filterable
        returned = gridService.isColumnFilterable(column);
        expect(returned).toEqual(false);
        gridService.initialize(gridPropsWithSortableAndFilterable); // Grid with Sort and Filter are false
        column = { filterable: true }; // Non Action  Column, but grid is by default not filterable
        returned = gridService.isColumnFilterable(column);
        expect(returned).toEqual(false);
    });

    it('should on reset page index', () => {
        const gridService = new GridService();
        gridService.initialize(gridProps);
        dispatcher.dispatch = jest.fn(() => {
            return 1;
        });
        gridService.onPageReset();
        expect(dispatcher.dispatch.mock.calls).toHaveLength(0);
    });
});
