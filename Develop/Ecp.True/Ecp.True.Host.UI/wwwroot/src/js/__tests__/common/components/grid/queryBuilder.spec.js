import QueryBuilder from '../../../../common/components/grid/queryBuilder.js';

it('should paginate', () => {
    const gridState = {
        page: { page: 0, pageSize: 10, sorted: [], filtered: {} },
        config: { filterable: {}, sortable: {} },
        overrides: {},
        sorted: [{}]
    };
    const queryBuilder = new QueryBuilder(gridState.config, gridState.overrides);
    const top = gridState.page.pageSize;
    const skip = top * (gridState.page.page);
    const returned = queryBuilder.withPagination(top, skip, true).build();
    expect(returned).toEqual("$top=10");
});

it('should sort', () => {
    const gridState = {
        page: { page: 0, pageSize: 10, sorted: [], filtered: {} },
        config: { filterable: {}, sortable: { defaultSort: "createdDate desc" } },
        overrides: {},
        sorted: [{}]
    };
    const queryBuilder = new QueryBuilder(gridState.config, gridState.overrides);
    const returned = queryBuilder.andSorting(gridState.config.sortable, gridState.page.sorted).build();
    expect(returned).toEqual("$orderby=createdDate desc");
});

it('should filter', () => {
    const gridState = {
        page: { page: 0, pageSize: 10, sorted: [], filtered: { status: { value: "Procesando", id: "status", key: "eq" } } },
        config: { filterable: { defaultFilter: "isHomologation eq false" } },
        overrides: {},
        sorted: [{}]
    };
    const filterValues = {};
    const queryBuilder = new QueryBuilder(gridState.config, gridState.overrides);
    const returned = queryBuilder.havingFilters(filterValues, gridState.page.filtered).build();
    expect(returned).toEqual("$filter=status eq 'Procesando' and isHomologation eq false");
});

it('should doPaginationWithSortingHavingFilters', () => {
    const gridState = {
        page: { page: 0, pageSize: 10, sorted: [], filtered: { status: { value: "Procesando", id: "status", key: "eq" } } },
        config: { filterable: { defaultFilter: "isHomologation eq false" }, sortable: { defaultSort: "createdDate desc" } },
        overrides: {},
        sorted: [{}]
    };
    const filterValues = {};
    const queryBuilder = new QueryBuilder(gridState.config, gridState.overrides);
    const top = gridState.page.pageSize;
    const skip = top * (gridState.page.page);
    const returned = queryBuilder.withPagination(top, skip, true).andSorting(gridState.config.sortable, gridState.page.sorted).havingFilters(filterValues, gridState.page.filtered).build();
    expect(returned).toEqual("$top=10&$orderby=createdDate desc&$filter=status eq 'Procesando' and isHomologation eq false");
});