import buildQuery from 'odata-query';
import { utilities } from '../../services/utilities';
import { dateService } from '../../services/dateService.js';

export default class QueryBuilder {
    constructor(config, overrides) {
        this.config = config;
        this.overrides = overrides;
        this.state = {
            page: '',
            sort: '',
            filter: ''
        };
    }

    withPagination(top, skip, show) {
        this.state.page = buildQuery(show ? { top, skip } : {}).replace('?', '');
        return this;
    }

    andSorting(sortable, pageSort) {
        let query = '';
        const orderBy = pageSort.reduce((a, b) => a + (`${b.id} ${b.desc ? 'desc' : ''}`.trim()), '');
        query = buildQuery({ orderBy }).replace('?', '');

        if (sortable && sortable.defaultSort && pageSort.length === 0) {
            if (utilities.isFunction(sortable.defaultSort)) {
                query = `${query}$orderby=${sortable.defaultSort(query)}`;
            } else {
                query = `${query}$orderby=${sortable.defaultSort}`;
            }
        }

        this.state.sort = query;
        return this;
    }

    havingFilters(filterValues, filtered = {}) {
        const filter = {};
        let filters = false;
        let query = '';

        Object.values(filtered).filter(v => v.value !== null).forEach(v => {
            if (v.key === 'dt') {
                const startDate = dateService.toDate([v.value.getFullYear(), v.value.getMonth(), v.value.getDate()]);
                const endDate = dateService.add(dateService.parse([v.value.getFullYear(), v.value.getMonth(), v.value.getDate()]), 1, 'day').toDate();
                filter[v.id] = { ge: startDate, lt: endDate };
            } else {
                filter[v.id] = { [v.key]: v.value };
            }
        });

        let filterQuery = buildQuery({ filter }).replace('?', '');
        if (!utilities.isNullOrUndefined(filterValues) && !utilities.isNullOrUndefined(this.config.filterable) && !utilities.isNullOrUndefined(this.config.filterable.filterBuilder)) {
            const filterBuilder = this.config.filterable.filterBuilder;
            const filterBuilderQuery = filterBuilder(filterValues);

            if (utilities.isNullOrWhitespace(filterQuery)) {
                if (utilities.isNullOrWhitespace(filterBuilderQuery)) {
                    filterQuery = null;
                } else {
                    filterQuery = `$filter=${filterBuilderQuery}`;
                }
            } else if (!utilities.isNullOrWhitespace(filterBuilderQuery)) {
                filterQuery = `${filterQuery} and (${filterBuilderQuery})`;
            }
        }

        if (!utilities.isNullOrWhitespace(filterQuery)) {
            query = filterQuery;
            filters = true;
        }
        const filterable = this.config.filterable;
        let defaultFilter = filterable ? filterable.defaultFilter : null;

        if (!defaultFilter) {
            defaultFilter = this.overrides.defaultFilter;
        }

        if (defaultFilter && filterable && (!filterable.override || !filters)) {
            if (utilities.isFunction(defaultFilter)) {
                query = `${query}${!filters ? '$filter=' : ' and '}${defaultFilter(query)}`;
            } else {
                query = `${query}${!filters ? '$filter=' : ' and '}${defaultFilter}`;
            }
        }

        this.state.filter = query;
        return this;
    }

    build(isDecimal = false) {
        const query = Object.values(this.state).filter(e => !utilities.isNullOrWhitespace(e)).join('&');
        if (!isDecimal) {
            return query;
        }

        return query.split('\'').join('');
    }
}
