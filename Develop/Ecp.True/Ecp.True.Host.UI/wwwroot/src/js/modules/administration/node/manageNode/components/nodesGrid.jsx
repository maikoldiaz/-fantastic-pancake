import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../../common/services/apiService';
import { ActionCell } from '../../../../../common/components/grid/gridCells.jsx';
import NodesGridFilter from './nodesGridFilter.jsx';
import { requestFilteredNodes, initUpdateNode, initCreateNode, requestNodeStorageLocations } from '../actions';
import { navigationService } from '../../../../../common/services/navigationService';
import { openFlyout } from './../../../../../common/actions';
import { utilities } from '../../../../../common/services/utilities';
import { routerActions } from '../../../../../common/router/routerActions';
import { refreshGrid } from '../../../../../common/components/grid/actions';

class NodesGrid extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
        this.getNodes = this.getNodes.bind(this);
        this.onCreateNode = this.onCreateNode.bind(this);
        routerActions.configure('createNode', this.onCreateNode);
    }

    onCreateNode() {
        this.props.onCreateNode();
    }

    getColumns() {
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} />;

        const columns = [];

        columns.push(gridUtils.buildTextColumn('segment.name', this.props, null, 'segment'));
        columns.push(gridUtils.buildTextColumn('name', this.props));
        columns.push(gridUtils.buildTextColumn('nodeType.name', this.props, null, 'type'));
        columns.push(gridUtils.buildTextColumn('operator.name', this.props, null, 'operator'));
        columns.push(gridUtils.buildTextColumn('order', this.props));
        columns.push(gridUtils.buildActionColumn(actions, null, 100));

        return columns;
    }

    getNodes() {
        const filters = {
            segmentId: this.props.filters.segment.elementId,
            nodeTypeIds: this.props.filters.nodeTypes.map(v => v.elementId),
            operatorIds: this.props.filters.operators.map(v => v.elementId)
        };
        this.props.getNodes(this.props.name, filters);
    }

    render() {
        return (
            <>
                <Grid name={this.props.name} columns={this.getColumns()} requestGridData={this.getNodes} onNoData={this.props.openNodesGridFilter} />
                <NodesGridFilter name="nodesGridFilter" />
            </>
        );
    }

    componentDidMount() {
        if (!utilities.isNullOrUndefined(this.props.filters.segment) && this.props.persist) {
            this.props.refreshGrid();
        }
    }

    componentDidUpdate(prevProps) {
        if (this.props.filters !== prevProps.filters && !utilities.isNullOrUndefined(this.props.filters.segment)) {
            this.props.refreshGrid();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        groupedCategoryElements: state.shared.groupedCategoryElements,
        filters: state.node.manageNode.filterValues ? state.node.manageNode.filterValues : state.node.manageNode.defaultFilterValues,
        edit: true,
        persist: state.node.manageNode.persist
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        getNodes: (name, filter) => {
            dispatch(requestFilteredNodes(name, filter.segmentId));
        },
        refreshGrid: () => {
            dispatch(refreshGrid(ownProps.name));
        },
        onEdit: node => {
            navigationService.navigateTo(`manage/${node.nodeId}`);
            dispatch(initUpdateNode(node));
            dispatch(requestNodeStorageLocations(node.nodeId));
        },
        onCreateNode: () => {
            dispatch(initCreateNode());
        },
        openNodesGridFilter: () => {
            dispatch(openFlyout('nodesGridFilter'));
        }
    };
};

/* istanbul ignore next */
const nodesGridConfig = () => {
    return {
        name: 'nodes',
        odata: true,
        startEmpty: true,
        apiUrl: apiService.node.QuerygetFilterNode(),
        section: true
    };
};

export default dataGrid((connect(mapStateToProps, mapDispatchToProps)(NodesGrid)), nodesGridConfig);
