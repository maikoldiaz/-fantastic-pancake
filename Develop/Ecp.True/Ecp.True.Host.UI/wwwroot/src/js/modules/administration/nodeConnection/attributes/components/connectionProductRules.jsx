import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { ActionCell } from '../../../../../common/components/grid/gridCells.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils.js';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../../common/services/apiService';
import { routerActions } from '../../../../../common/router/routerActions.js';
import { triggerPopup, enableDisablePageAction } from '../../../../../common/actions.js';
import PopupFactory from '../../../../../common/components/modal/popupFactory.jsx';
import RuleSynchronizer from '../../../../../common/components/ruleSynchronizer/ruleSynchronizer.jsx';
import { constants } from '../../../../../common/services/constants.js';

class ConnectionProductRules extends React.Component {
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

        columns.push(gridUtils.buildTextColumn('sourceOperator', this.props));
        columns.push(gridUtils.buildTextColumn('destinationOperator', this.props));
        columns.push(gridUtils.buildTextColumn('sourceNode', this.props));
        columns.push(gridUtils.buildTextColumn('destinationNode', this.props));
        columns.push(gridUtils.buildTextColumn('product', this.props));
        columns.push(gridUtils.buildTextColumn('ruleName', this.props, null, 'ilFunction'));
        columns.push(gridUtils.buildActionColumn(actions, null, 100));

        return columns;
    }

    render() {
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <RuleSynchronizer name={constants.RuleType.NodeConnectionProduct} />
                    <Grid wrapperClassName="ep-table-wrap--sync" name={this.props.name} columns={this.getColumns()} />
                </div>
                <PopupFactory type={constants.RuleType.NodeConnectionProduct} />
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

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        edit: true,
        selection: state.grid.nodeConnectionProductRules ? state.grid.nodeConnectionProductRules.selection : [],
        editable: state.ruleSynchronizer[constants.RuleType.NodeConnectionProduct] ? state.ruleSynchronizer[constants.RuleType.NodeConnectionProduct].enabled : true
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        enableDisableChangeStrategy: disabled => {
            dispatch(enableDisablePageAction('changeStrategy', disabled));
        },
        onEdit: selectedItem => {
            dispatch(triggerPopup([selectedItem], constants.RuleType.NodeConnectionProduct));
        },
        triggerPopup: selection => {
            dispatch(triggerPopup(selection, constants.RuleType.NodeConnectionProduct));
        }
    };
};

const gridConfig = () => {
    return {
        name: 'nodeConnectionProductRules',
        idField: 'nodeConnectionProductId',
        defaultPageSize: 100,
        selectable: true,
        apiUrl: apiService.nodeConnection.getOwnershipRules()
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(ConnectionProductRules, gridConfig));
