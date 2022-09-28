import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { ActionCell } from '../../../../../common/components/grid/gridCells.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils.js';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../../common/services/apiService';
import { routerActions } from '../../../../../common/router/routerActions.js';
import { enableDisablePageAction, triggerPopup } from '../../../../../common/actions.js';
import PopupFactory from '../../../../../common/components/modal/popupFactory.jsx';
import { constants } from '../../../../../common/services/constants.js';
import RuleSynchronizer from '../../../../../common/components/ruleSynchronizer/ruleSynchronizer.jsx';

class NodeOwnershipRules extends React.Component {
    constructor() {
        super();
        routerActions.configure('changeStrategy', () => this.props.triggerPopup(this.props.selection));
        this.getColumns = this.getColumns.bind(this);
    }

    getColumns() {
        const actions = rowProps => (
            <ActionCell {...this.props} {...rowProps} enableEdit={() => this.props.editable} />
        );
        const columns = [];

        columns.push(gridUtils.buildTextColumn('segment', this.props));
        columns.push(gridUtils.buildTextColumn('operator', this.props));
        columns.push(gridUtils.buildTextColumn('nodeType', this.props));
        columns.push(gridUtils.buildTextColumn('name', this.props, null, 'node'));
        columns.push(gridUtils.buildTextColumn('ruleName', this.props, null, 'ilFunction'));
        columns.push(gridUtils.buildActionColumn(actions, null, 100));

        return columns;
    }

    render() {
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <RuleSynchronizer name={constants.RuleType.Node} />
                    <Grid wrapperClassName="ep-table-wrap--sync" name={this.props.name} columns={this.getColumns()} />
                </div>
                <PopupFactory type={constants.RuleType.Node} />
            </section>
        );
    }

    componentDidMount() {
        this.props.enableDisableChangeStrategy(true);
    }

    componentDidUpdate() {
        this.props.enableDisableChangeStrategy(this.props.selection.length === 0 || !this.props.editable);
    }
}
const mapStateToProps = state => {
    return {
        edit: true,
        editable: state.ruleSynchronizer[constants.RuleType.Node] ? state.ruleSynchronizer[constants.RuleType.Node].enabled : true,
        selection: state.grid.nodeOwnershipRules ? state.grid.nodeOwnershipRules.selection : []
    };
};

const mapDispatchToProps = dispatch => {
    return {
        enableDisableChangeStrategy: disabled => {
            dispatch(enableDisablePageAction('changeStrategy', disabled));
        },
        onEdit: selectedItem => {
            dispatch(triggerPopup([selectedItem], constants.RuleType.Node));
        },
        triggerPopup: selection => {
            dispatch(triggerPopup(selection, constants.RuleType.Node));
        }
    };
};

const gridConfig = () => {
    return {
        name: 'nodeOwnershipRules',
        apiUrl: apiService.node.queryRules(),
        selectable: true,
        idField: 'nodeId',
        defaultPageSize: 100
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(NodeOwnershipRules, gridConfig));
