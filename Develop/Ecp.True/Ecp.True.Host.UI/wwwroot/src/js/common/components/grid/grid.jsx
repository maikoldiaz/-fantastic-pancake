import React from 'react';
import ReactTable from 'react-table';
import selectTableHOC from 'react-table/lib/hoc/selectTable';
import { connect } from 'react-redux';
import GridService from './gridService';
import { fetchGridData, clearGrid, fetchGridDataSilent, refreshSilent, updatePageItems, clearSelection } from './actions';
import { utilities } from '../../services/utilities';
import classNames from 'classnames/bind';

const ReactSelectTable = selectTableHOC(ReactTable);

export class Grid extends React.Component {
    constructor(props) {
        super(props);

        this.gridService = new GridService();
        this.gridService.initialize(this.props);
        this.buildApiUrl = this.buildApiUrl.bind(this);
        this.fetchGridData = this.fetchGridData.bind(this);
        this.shouldRefresh = this.shouldRefresh.bind(this);
        this.isFiltering = this.isFiltering.bind(this);
        this.renderGrid = this.renderGrid.bind(this);
        this.renderSectionGrid = this.renderSectionGrid.bind(this);

        this.recurrence = null;
        this.filtered = [];
    }

    buildApiUrl() {
        return this.props.buildApiUrl ? this.props.buildApiUrl(this.props.filterValues, this.props.pageFilters, this.props.isDecimal) :
            this.gridService.buildGridApiUrl(this.props.filterValues, this.props.pageFilters, this.props.isDecimal);
    }

    shouldRefresh() {
        if (!this.props.shouldRefresh || this.props.shouldRefresh()) {
            this.props.refreshSilent();
        }
    }

    fetchGridData(apiUrl, notFound) {
        // When there is no API url defined, do nothing.
        if (utilities.isNullOrWhitespace(apiUrl)) {
            return;
        }

        if (this.props.requestGridData) {
            this.props.requestGridData(apiUrl, notFound);
        } else {
            this.props.fetchGridData(apiUrl, notFound);
        }
    }

    isFiltering(state) {
        if (this.filtered.length !== state.filtered.length) {
            this.filtered = state.filtered;
            return true;
        }

        for (let index = 0; index < this.filtered.length; index++) {
            const element = this.filtered[index];
            if (element.value !== state.filtered[index].value) {
                this.filtered = state.filtered;
                return true;
            }
        }

        this.filtered = state.filtered;
        return false;
    }

    renderSectionGrid() {
        return (
            <section className="ep-content">
                <div className={`ep-content__body ${this.props.classAlignment}`}>
                    {this.renderGrid()}
                </div>
            </section>
        );
    }

    renderGrid() {
        const gridInfo = this.gridService.process(this.props).gridState;
        const Table = gridInfo.props.selectable ? ReactSelectTable : ReactTable;
        const idProps = { id: `grd_${this.props.name}` };
        return (
            <section className={classNames('ep-table-wrap', this.props.wrapperClassName)}>
                {gridInfo.props.columns && gridInfo.props.columns.length > 0 &&
                    <>
                        {(this.gridService.shouldClear()) ?
                            <Table getProps={() => idProps} data={gridInfo.items} columns={gridInfo.props.columns}
                                className={classNames('ep-table', this.props.className)}
                                {...gridInfo.props}>{(state, makeTable) => {
                                    if (this.isFiltering(state)) {
                                        this.props.clearSelection();
                                    }
                                    this.props.updatePageItems(state.sortedData.slice(state.startRow, state.endRow));
                                    return makeTable();
                                }}</Table> :
                            <Table getProps={() => idProps} data={gridInfo.items} columns={gridInfo.props.columns}
                                className={classNames('ep-table', this.props.className)}
                                {...gridInfo.props}
                            />}
                    </>}
            </section>
        );
    }

    render() {
        return (
            <>
                {this.props.config.section === true ? this.renderSectionGrid() : this.renderGrid()}
            </>
        );
    }

    componentDidMount() {
        // When you don't want to fetch data at start
        // The caller should call refreshGrid action to fetch data later on.
        if (!this.props.config.startEmpty) {
            this.fetchGridData(this.buildApiUrl(), this.props.config.notFound);
        }

        if (this.props.config.refreshable) {
            const interval = this.props.config.refreshable.interval ? this.props.config.refreshable.interval : 30;
            this.recurrence = setInterval(this.shouldRefresh, interval * 1000);
        }
    }

    componentDidUpdate(prevProps) {
        if (this.props.refreshToggler !== prevProps.refreshToggler) {
            this.fetchGridData(this.buildApiUrl());
        }
        if (this.props.refreshSilentToggler !== prevProps.refreshSilentToggler) {
            this.props.fetchGridDataSilent(this.buildApiUrl());
        }

        // When filter is reset, take the grid back to initial state
        if (this.props.resetToggler !== prevProps.resetToggler) {
            if (!this.props.config.startEmpty) {
                this.fetchGridData(this.buildApiUrl());
            } else {
                this.props.clearGrid();
            }
        }

        if (this.props.receiveDataToggler !== prevProps.receiveDataToggler && this.props.onReceiveData) {
            this.props.onReceiveData(this.props.items);
        }

        if (this.props.resetPageIndexToggler !== prevProps.resetPageIndexToggler) {
            this.gridService.onPageReset();
        }
    }

    componentWillUnmount() {
        if (this.recurrence) {
            clearInterval(this.recurrence);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        refreshToggler: state.grid[ownProps.name].refreshToggler,
        refreshSilentToggler: state.grid[ownProps.name].refreshSilentToggler,
        resetToggler: state.grid[ownProps.name].resetToggler,
        receiveDataToggler: state.grid[ownProps.name].receiveDataToggler,
        config: state.grid[ownProps.name].config,
        items: state.grid[ownProps.name].items,
        totalItems: state.grid[ownProps.name].totalItems,
        selection: state.grid[ownProps.name].selection,
        selectAll: state.grid[ownProps.name].selectAll,
        filterValues: state.grid[ownProps.name].filterValues,
        pageFilters: state.grid[ownProps.name].pageFilters,
        isDecimal: state.grid[ownProps.name].isDecimal,
        resetPageIndexToggler: state.grid[ownProps.name].resetPageIndexToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        fetchGridData: (apiUrl, notFound) => {
            dispatch(fetchGridData(apiUrl, ownProps.name, notFound));
        },
        fetchGridDataSilent: apiUrl => {
            dispatch(fetchGridDataSilent(apiUrl, ownProps.name));
        },
        clearGrid: () => {
            dispatch(clearGrid(ownProps.name));
        },
        refreshSilent: () => {
            dispatch(refreshSilent(ownProps.name));
        },
        updatePageItems: pageItems => {
            dispatch(updatePageItems(ownProps.name, pageItems));
        },
        clearSelection: () => {
            dispatch(clearSelection(ownProps.name));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(Grid);
