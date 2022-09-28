import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { ActionCell, OwnershipCell } from '../../../../../common/components/grid/gridCells.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils.js';
import { dataGrid } from '../../../../../common/components/grid/gridComponent.js';
import { apiService } from '../../../../../common/services/apiService.js';
import { openModal, initTargetItems, triggerPopup, configureDualSelect, clearSearchText } from '../../../../../common/actions.js';
import { initNodeProduct } from '../actions';
import { navigationService } from '../../../../../common/services/navigationService.js';
import { utilities } from '../../../../../common/services/utilities.js';
import PopupFactory from '../../../../../common/components/modal/popupFactory.jsx';
import { constants } from '../../../../../common/services/constants.js';
import { clearOwners } from '../../../nodeConnection/attributes/actions.js';

class NodeProducts extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
    }

    getColumns() {
        const Uncertainty = rowProps => <ActionCell {...this.props} {...rowProps} onEdit={this.props.editUncertainty} />;
        const Ownership = rowProps => <OwnershipCell {...rowProps} {...this.props} />;
        const Function = rowProps => <ActionCell {...this.props} onEdit={data => this.props.onVariableEdit(data)} {...rowProps} editTitle="editRules" editActionKey={this.props.editRule} />;
        const NodeProductVariable = rowProps => (<ActionCell {...this.props} onEdit={data => this.props.onVariableEdit(data)} {...rowProps}
            editActionKey={this.props.editVariable} editTitle="editVariables" wrapClassName="m-r-3" />);
        const columns = [];

        columns.push(gridUtils.buildTextColumn('nodeStorageLocation.name', this.props, null, 'storageLocation'));
        columns.push(gridUtils.buildTextColumn('product.name', this.props, null, 'product'));
        columns.push(
            gridUtils.buildDecimalColumn(
                'uncertaintyPercentage',
                this.props,
                'uncertainty',
                { filterable: false, suffix: constants.Suffix }
            ),
            gridUtils.buildActionColumn(Uncertainty, '', 120),
            gridUtils.buildTextColumn('owners', this.props, Ownership, 'owners', { sortable: false, filterable: false }),
            gridUtils.buildTextColumn('nodeProductRule.ruleName', this.props, Function, 'ilFunction', { filterable: false }),
            gridUtils.buildTextColumn('storageLocationProductVariables', this.props, NodeProductVariable, 'contractualConfiguration', { sortable: false, filterable: false }));
        return columns;
    }
    render() {
        return (
            <>
                <Grid className="ep-table--addon" name={this.props.name} columns={this.getColumns()} />
                <PopupFactory type={constants.RuleType.StorageLocationProductVariable} ruleNamePath="nodeProductRule.ruleName" />
            </>
        );
    }

    componentDidMount() {
        this.props.configureDualSelect('productOwners');
    }

    componentWillUnmount() {
        this.props.clearOwners();
    }
}

/* istanbul ignore next */
const mapStateToProps = () => {
    return {
        edit: true
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onEdit: product => {
            dispatch(initNodeProduct(product));
            dispatch(openModal('editUncertainty'));
        },
        onVariableEdit: product => {
            dispatch(triggerPopup([product], constants.RuleType.StorageLocationProductVariable, 'nodeProductRule.ruleName', true));
        },
        onEditOwners: product => {
            const owners = product.owners.map(o => {
                return {
                    id: o.ownerId,
                    name: o.owner.name,
                    value: o.ownershipPercentage,
                    selected: false
                };
            });
            dispatch(clearSearchText('productOwners'));
            dispatch(initNodeProduct(product));
            dispatch(initTargetItems(owners, 'productOwners'));
            dispatch(openModal(owners && owners.length > 0 ? 'ownersPie' : 'editOwners'));
        },
        editUncertainty: product => {
            dispatch(initNodeProduct(product));
            dispatch(openModal('editUncertainty'));
        },
        editRule: row => {
            if (!utilities.isNullOrWhitespace(row.nodeProductRule)) {
                return `${row.nodeProductRule.ruleName}`;
            }
            return '';
        },
        editVariable: row => {
            if (!utilities.isNullOrUndefined(row.storageLocationProductVariables)) {
                return row.storageLocationProductVariables.map(x => utilities.toUpperCase(x.variableType.ficoName)).join('; ');
            }
            return '';
        },
        configureDualSelect: name => {
            dispatch(configureDualSelect(name));
        },
        clearOwners: () => {
            dispatch(clearOwners());
        }
    };
};

const gridConfig = () => {
    return {
        name: 'nodeProducts',
        idField: 'nodeId',
        apiUrl: apiService.node.queryProducts(navigationService.getParamByName('nodeId')),
        filterable: false
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(NodeProducts, gridConfig));
