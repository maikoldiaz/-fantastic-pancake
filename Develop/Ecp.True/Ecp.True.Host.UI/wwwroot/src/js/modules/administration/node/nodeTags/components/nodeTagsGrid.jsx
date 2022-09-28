import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../../common/services/apiService';
import { disableGridItems, fetchGridData, selectGridData } from '../../../../../common/components/grid/actions';
import { disablePageActions, openModal, togglePageActions, openFlyout, resetPageAction, categoryElementFilterResetFields } from '../../../../../common/actions';
import { ActionCell, DateCell } from '../../../../../common/components/grid/gridCells.jsx';
import { nodeTagsFilterBuilder } from '../nodeTagsFilterBuilder';
import { CategoryElementFilterComponent } from '../../../../../common/components/filters/categoryElementFilter.jsx';
import { utilities } from '../../../../../common/services/utilities';
import { initExpireNode } from '../actions';

export class NodeTagsGrid extends React.Component {
    constructor() {
        super();
        this.keyValues = [];
        this.getColumns = this.getColumns.bind(this);
        this.onReceiveData = this.onReceiveData.bind(this);
        this.isEnabled = this.isEnabled.bind(this);
        this.requestGridData = this.requestGridData.bind(this);
    }

    isEnabled(row) {
        return (new Date(row.endDate).getFullYear() === 9999);
    }

    getColumns() {
        const columns = [];

        const actions = gridProps => <ActionCell id={this.props.name} {...this.props} {...gridProps} enableExpire={this.isEnabled} />;
        const date = rowProps => <DateCell {...this.props} {...rowProps} ignoreMax={true} />;

        columns.push(gridUtils.buildTextColumn('node.name', this.props, null, 'node'));
        columns.push(gridUtils.buildTextColumn('categoryElement.category.name', this.props, null, 'category'));
        columns.push(gridUtils.buildTextColumn('categoryElement.name', this.props, null, 'element'));
        columns.push(gridUtils.buildDateColumn('startDate', this.props, date));
        columns.push(gridUtils.buildDateColumn('endDate', this.props, date));

        columns.push(gridUtils.buildActionColumn(actions, '', 100));

        return columns;
    }

    onReceiveData(items) {
        const filters = this.props.filters.filter(x => !utilities.isNullOrUndefined(x.element));
        if (filters.length > 1) {
            const selectedElementId = filters[0].element.elementId;
            this.keyValues = items.filter(x => x.categoryElement.elementId === selectedElementId).map(x => x.nodeTagId);
            this.props.updateGridItems(this.keyValues);
        } else {
            this.keyValues = [];
        }
    }

    requestGridData(apiUrl) {
        const filterCount = this.props.filters.filter(x => !utilities.isNullOrUndefined(x.element)).length;
        if (filterCount > 0) {
            this.props.fetchGridData(apiUrl, this.props.name);
        }
    }

    render() {
        return (
            <>
                <Grid name={this.props.name} onReceiveData={this.onReceiveData} columns={this.getColumns()}
                    requestGridData={this.requestGridData} onNoData={this.props.openNodeTagsFilterFlyout} />
                <CategoryElementFilterComponent name={this.props.name} />
            </>
        );
    }

    componentDidUpdate(prevProps) {
        const filterCount = this.props.filters.filter(x => !utilities.isNullOrUndefined(x.element)).length;
        if (this.props.filters !== prevProps.filters) {
            this.props.disablePageActions(filterCount > 1 ? ['change', 'expire'] : ['new']);
        }

        if (filterCount > 0 && this.props.selection.filter(x => !this.keyValues.includes(x.nodeTagId)).length > 0) {
            this.props.togglePageActions(true);
        } else {
            this.props.togglePageActions(false);
        }
    }

    componentWillUnmount() {
        this.props.resetFilter(this.props.name);
        this.props.resetPageAction();
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        selection: state.grid[ownProps.name].selection,
        items: state.grid[ownProps.name].items,
        name: ownProps.name,
        expire: true,
        filters: state.categoryElementFilter[ownProps.name] ? state.categoryElementFilter[ownProps.name].values.categoryElements : []
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        togglePageActions: enabled => {
            dispatch(togglePageActions(enabled));
        },
        disablePageActions: disabledActions => {
            dispatch(disablePageActions(disabledActions));
        },
        onExpire: node => {
            dispatch(initExpireNode(node));
            dispatch(openModal('expire', 'expire'));
        },
        updateGridItems: keyValues => {
            keyValues.forEach(x => {
                dispatch(selectGridData(`${x}`, ownProps.name));
            });

            dispatch(disableGridItems(ownProps.name, 'nodeTagId', keyValues));
        },
        fetchGridData: (apiUrl, name) => {
            dispatch(fetchGridData(apiUrl, name));
        },
        openNodeTagsFilterFlyout: () => {
            dispatch(openFlyout('nodeTags'));
        },
        resetFilter: name => {
            dispatch(categoryElementFilterResetFields(name));
        },
        resetPageAction: () => {
            dispatch(resetPageAction());
        }
    };
};

const nodeTagsGridConfig = () => {
    return {
        name: 'nodeTags',
        filterable: {
            filterBuilder: nodeTagsFilterBuilder.build
        },
        idField: 'nodeTagId',
        selectable: true,
        startEmpty: true,
        apiUrl: apiService.node.queryTags(),
        section: true
    };
};

/* istanbul ignore next */
export default dataGrid((connect(mapStateToProps, mapDispatchToProps, utilities.merge)(NodeTagsGrid)), nodeTagsGridConfig);
