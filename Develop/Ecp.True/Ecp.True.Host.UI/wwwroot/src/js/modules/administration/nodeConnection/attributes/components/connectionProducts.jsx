import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import {
    ActionCell,
    OwnershipCell
} from '../../../../../common/components/grid/gridCells.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils.js';
import { dataGrid } from '../../../../../common/components/grid/gridComponent.js';
import { apiService } from '../../../../../common/services/apiService.js';
import { openModal, initTargetItems, triggerPopup, configureDualSelect, clearSearchText } from '../../../../../common/actions.js';
import { initProduct, clearOwners } from './../actions';
import { constants } from '../../../../../common/services/constants';
import { navigationService } from '../../../../../common/services/navigationService.js';
import { utilities } from '../../../../../common/services/utilities.js';
import PopupFactory from '../../../../../common/components/modal/popupFactory.jsx';

class ConnectionProducts extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
    }

    getColumns() {
        const UncertaintyFunction = rowProps => (<ActionCell {...this.props} onEdit={this.props.onUncertaintyFunctionEdit} {...rowProps} editTitle="editPriority" />);
        const Ownership = rowProps => (<OwnershipCell {...rowProps} {...this.props} />);
        const PriorityFunction = rowProps => (<ActionCell {...this.props} onEdit={this.props.onPriorityFunctionEdit} {...rowProps} editTitle="editPriority" />);
        const UpdateRule = rowProps => (
            <ActionCell {...this.props} wrapClassName="m-r-3" editTitle="editRules"
                {...this.props.nodeOwnershipRule} onEdit={data => this.props.onRuleEdit(data)} {...rowProps} editActionKey={this.props.editRule} />
        );

        const columns = [];

        columns.push(gridUtils.buildTextColumn('product.name', this.props, null, 'product'));
        columns.push(gridUtils.buildDecimalColumn('uncertaintyPercentage', this.props, 'uncertainty', { filterable: false, suffix: constants.Suffix }),
            gridUtils.buildActionColumn(UncertaintyFunction, '', 120),
            gridUtils.buildTextColumn('owners', this.props, Ownership, 'owners', { sortable: false }),
            gridUtils.buildNumberColumn('priority', this.props, 'priority', { filterable: false }),
            gridUtils.buildActionColumn(PriorityFunction, '', 120),
            gridUtils.buildTextColumn('nodeConnectionProductRule.ruleName', this.props, UpdateRule, 'ilFunction', { filterable: false })
        );

        return columns;
    }
    render() {
        return (
            <>
                <Grid className="ep-table--addon" name={this.props.name} columns={this.getColumns()} />
                <PopupFactory type={constants.RuleType.ConnectionProduct} ruleNamePath="nodeConnectionProductRule.ruleName" />
            </>
        );
    }

    componentDidMount() {
        this.props.configureDualSelect('connectionOwners');
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
            dispatch(initProduct(product));
            dispatch(openModal('editUncertainty'));
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
            dispatch(clearSearchText('connectionOwners'));
            dispatch(initProduct(product));
            dispatch(initTargetItems(owners, 'connectionOwners'));

            dispatch(
                openModal(owners && owners.length > 0 ? 'ownersPie' : 'editOwners')
            );
        },
        onUncertaintyFunctionEdit: product => {
            dispatch(initProduct(product));
            dispatch(openModal('editUncertainty'));
        },
        editRule: row => {
            if (!utilities.isNullOrWhitespace(row.nodeConnectionProductRule)) {
                return `${row.nodeConnectionProductRule.ruleName}`;
            }
            return '';
        },
        onPriorityFunctionEdit: product => {
            dispatch(initProduct(product));
            dispatch(openModal('editPriorityRules'));
        },
        onRuleEdit: selectedItem => {
            dispatch(triggerPopup([selectedItem], constants.RuleType.ConnectionProduct, 'nodeConnectionProductRule.ruleName'));
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
        name: 'connectionProducts',
        idField: 'nodeConnectionProductId',
        apiUrl: apiService.nodeConnection.queryProducts(navigationService.getParamByName('nodeConnectionId')),
        filterable: false
    };
};

/* istanbul ignore next */
export default connect(
    mapStateToProps,
    mapDispatchToProps
)(dataGrid(ConnectionProducts, gridConfig));
