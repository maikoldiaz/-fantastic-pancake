import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../../common/services/apiService';
import { ActionCell } from '../../../../../common/components/grid/gridCells.jsx';
import { routerActions } from '../../../../../common/router/routerActions.js';
import { enableDisablePageAction, triggerPopup } from '../../../../../common/actions.js';
import PopupFactory from '../../../../../common/components/modal/popupFactory.jsx';
import { constants } from '../../../../../common/services/constants.js';
import RuleSynchronizer from '../../../../../common/components/ruleSynchronizer/ruleSynchronizer.jsx';

class NodeProductRules extends React.Component {
    constructor() {
        super();

        routerActions.configure('changeStrategy', () => this.props.triggerPopup(this.props.selection));
        this.getColumns = this.getColumns.bind(this);
    }

    getColumns() {
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} enableEdit={() => this.props.editable} />;

        const columns = [];

        columns.push(gridUtils.buildTextColumn('segment', this.props));
        columns.push(gridUtils.buildTextColumn('operator', this.props));
        columns.push(gridUtils.buildTextColumn('nodeType', this.props));
        columns.push(gridUtils.buildTextColumn('nodeName', this.props, null, 'node'));
        columns.push(gridUtils.buildTextColumn('storageLocation', this.props));
        columns.push(gridUtils.buildTextColumn('product', this.props));
        columns.push(gridUtils.buildTextColumn('ruleName', this.props, null, 'ilFunction'));
        columns.push(gridUtils.buildActionColumn(actions, null, 100));

        return columns;
    }

    render() {
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <RuleSynchronizer name={constants.RuleType.NodeProduct} />
                    <Grid wrapperClassName="ep-table-wrap--sync" name={this.props.name} columns={this.getColumns()} />
                </div>
                <PopupFactory type={constants.RuleType.NodeProduct} />
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
        selection: state.grid.nodeProductRules ? state.grid.nodeProductRules.selection : [],
        editable: state.ruleSynchronizer[constants.RuleType.NodeProduct] ? state.ruleSynchronizer[constants.RuleType.NodeProduct].enabled : true
    };
};

const mapDispatchToProps = dispatch => {
    return {
        enableDisableChangeStrategy: disabled => {
            dispatch(enableDisablePageAction('changeStrategy', disabled));
        },
        onEdit: selectedItem => {
            dispatch(triggerPopup([selectedItem], constants.RuleType.NodeProduct));
        },
        triggerPopup: selection => {
            dispatch(triggerPopup(selection, constants.RuleType.NodeProduct));
        }
    };
};

/* istanbul ignore next */
const nodeProductRulesGridConfig = () => {
    return {
        name: 'nodeProductRules',
        idField: 'storageLocationProductId',
        apiUrl: apiService.node.getOwnershipRules(),
        selectable: true,
        defaultPageSize: 100
    };
};

export default dataGrid((connect(mapStateToProps, mapDispatchToProps)(NodeProductRules)), nodeProductRulesGridConfig);
