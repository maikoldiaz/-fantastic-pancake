import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, change } from 'redux-form';
import { required } from 'redux-form-validators';
import classNames from 'classnames/bind';
import {
    bulkUpdateRules, requestNodeRules, requestNodeProductRules, requestNodeConnectionProductRules,
    requestVariableTypes, setVariableState
} from '../../actions';
import { inputSelect } from '../../components/formControl/formControl.jsx';
import { resourceProvider } from '../../services/resourceProvider';
import { utilities } from '../../services/utilities';
import { getNode } from '../../../modules/administration/node/attributes/actions';
import { navigationService } from '../../services/navigationService';
import { constants } from '../../services/constants';
import ModalFooter from '../footer/modalFooter.jsx';
import { footerConfigService } from '../../services/footerConfigService';
import { refreshGrid } from '../grid/actions';

export class BulkUpdate extends React.Component {
    constructor() {
        super();
        this.updateRules = this.updateRules.bind(this);
        this.getRules = this.getRules.bind(this);
        this.getType = this.getType.bind(this);
        this.getVariables = this.getVariables.bind(this);
        this.onSelectOwnershipStrategy = this.onSelectOwnershipStrategy.bind(this);
        this.onSelectVariable = this.onSelectVariable.bind(this);
    }

    getType() {
        if (this.props.type === constants.RuleType.ConnectionProduct) {
            return constants.RuleType.NodeConnectionProduct;
        }
        if (this.props.type === constants.RuleType.StorageLocationProductVariable) {
            return constants.RuleType.NodeProduct;
        }
        if (this.props.type === constants.RuleType.NodeAttribute) {
            return constants.RuleType.Node;
        }
        return this.props.type;
    }

    updateRules(values) {
        const element = {
            ownershipRuleType: this.getOwnershipType(),
            ownershipRuleId: values.ownershipRules.ruleId,
            ids: this.getSelectedIds(),
            variableTypeIds: this.getSelectedVariableIds()
        };
        this.props.updateRules(element, this.props.type);
    }

    getOwnershipType() {
        return this.props.type === constants.RuleType.StorageLocationProductVariable ?
            constants.RuleType.StorageLocationProductVariable : this.getType();
    }

    getSelectedIds() {
        return this.props.initialValues.items.map(item => {
            return {
                id: item[`${this.getType()}Id`],
                rowVersion: item.version || item.rowVersion
            };
        });
    }

    getSelectedVariableIds() {
        if (this.props.type === constants.RuleType.StorageLocationProductVariable && !utilities.isNullOrWhitespace(this.props.initialValues.variableTypes)) {
            // Check if selected strategy is 'Inventário Final' then remove 'INVENTARIO' variable
            if (this.props.initialValues.ownershipRules.ruleId === this.props.systemConfig.ownershipStrategy) {
                return this.props.initialValues.variableTypes.filter(x => x.variableTypeId !== constants.VariableType.FinalInventory).map(x => x.variableTypeId);
            }
            return this.props.initialValues.variableTypes.map(x => x.variableTypeId);
        }
        return null;
    }

    getRules() {
        if (this.props.initialValues.rules && this.props.initialValues.rules.length === 1) {
            if (!this.props.initialValues.isContractualStrategy) {
                const existingRuleName = this.props.initialValues.rules[0];
                return this.props.nodeRules.filter(x => x.ruleName !== existingRuleName);
            }
        }
        return this.props.nodeRules;
    }

    getVariables() {
        const configVariables = this.props.configurableVariableTypes.filter(c => c.isConfigurable === true);
        if (!utilities.isNullOrUndefined(configVariables)) {
            if (!utilities.isNullOrUndefined(this.props.initialValues.ownershipRules) &&
                this.props.initialValues.ownershipRules.ruleId === this.props.systemConfig.ownershipStrategy) {
                return configVariables.filter(x => x.variableTypeId !== constants.VariableType.FinalInventory);
            }
        }
        return configVariables;
    }

    onSelectOwnershipStrategy(selectedItem) {
        if (!utilities.isNullOrUndefined(selectedItem)) {
            let variables = this.props.initialValues.variableTypes;
            if (selectedItem.ruleId === this.props.systemConfig.ownershipStrategy) {
                variables = this.props.initialValues.variableTypes.filter(x => x.variableTypeId !== constants.VariableType.FinalInventory);
            }
            this.props.onChangeEvent(selectedItem, variables, this.props.type);
            this.props.resetField(this.props.form, 'variableTypes', variables);
            this.getVariables();
        }
    }

    onSelectVariable(selectedItem) {
        this.props.onChangeEvent(this.props.initialValues.ownershipRules, selectedItem, this.props.type);
    }

    render() {
        return (
            <form id="frm_nodeOwnershipRules_functions" className="ep-form" onSubmit={this.props.handleSubmit(this.updateRules)}>
                <section className="ep-modal__content">
                    {!this.props.initialValues.isContractualStrategy &&
                        <div className="row">
                            <div className="col-md-6">
                                <div className="ep-control-group m-b-3">
                                    <label className="ep-label">{resourceProvider.read('ilFunctionLbl')}</label>
                                    <span className="ep-data" id="lbl_bulkUpdate_rules">{this.props.initialValues.rules.join(', ')}</span>
                                </div>
                            </div>
                        </div>
                    }
                    <div className={classNames('row', { ['m-t-4']: !this.props.initialValues.isContractualStrategy })}>
                        <div className="col-md-6">
                            <div className="ep-control-group m-b-0">
                                <label className="ep-label" htmlFor="dd_nodeAttributes_to_sel">{this.props.initialValues.isContractualStrategy ?
                                    resourceProvider.read('ilFunction') : resourceProvider.read('newOwnershipStrategy')}
                                </label>
                                <Field id="dd_nodeAttributes_to" component={inputSelect} name="ownershipRules" onChange={this.onSelectOwnershipStrategy}
                                    getOptionLabel={x => x.ruleName} getOptionValue={x => x.ruleId} options={this.getRules()} inputId="dd_nodeAttributes_to_sel"
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                        </div>
                    </div>
                    {this.props.initialValues.isContractualStrategy &&
                        <div className="row m-t-4">
                            <div className="col-md-6">
                                <div className="ep-control-group m-b-0">
                                    <label className="ep-label" htmlFor="dd_nodeproductVariables_sel">{resourceProvider.read('variable')}</label>
                                    <Field id="dd_nodeproductVariables" component={inputSelect} name="variableTypes" options={this.getVariables()}
                                        getOptionLabel={x => utilities.toUpperCase(x.ficoName)} getOptionValue={x => x.variableTypeId} inputId="dd_nodeproductVariables_sel"
                                        isMulti hideSelectedOptions={false} removeSelected={false} onChange={this.onSelectVariable} />
                                </div>
                            </div>
                        </div>
                    }
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('bulkUpdate')} />
            </form>
        );
    }

    componentDidMount() {
        const type = this.getType();
        if (type === constants.RuleType.Node) {
            this.props.getNodeRules();
        } else if (type === constants.RuleType.NodeProduct) {
            this.props.getNodeProductRules();
            this.props.getVariableTypes();
        } else {
            this.props.getNodeConnectionProductRules();
        }
    }

    componentDidUpdate(prevProps) {
        if (prevProps.refreshToggler !== this.props.refreshToggler || prevProps.updatedToggler !== this.props.updatedToggler) {
            if (this.props.gridName === constants.GridNames.NodeAttribute) {
                this.props.getNode();
            } else {
                this.props.refreshGrid(this.props.gridName);
            }
            this.props.closeModal();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    const refreshToggler = utilities.isNullOrUndefined(state.ownershipRules[ownProps.type]) ? false : state.ownershipRules[ownProps.type].updateToggler;
    const updatedToggler = utilities.isNullOrUndefined(state.ownershipRules[ownProps.type]) ? false : state.ownershipRules[ownProps.type].receiveUpdatedToggler;
    return {
        initialValues: state.ownershipRules[ownProps.type],
        refreshToggler,
        updatedToggler,
        nodeRules: state.ownershipRules.rules,
        form: ownProps.type,
        configurableVariableTypes: state.shared.variableTypes,
        systemConfig: state.root.systemConfig
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getNodeRules: () => {
            dispatch(requestNodeRules());
        },
        getNodeProductRules: () => {
            dispatch(requestNodeProductRules());
        },
        getNodeConnectionProductRules: () => {
            dispatch(requestNodeConnectionProductRules());
        },
        refreshGrid: name => {
            dispatch(refreshGrid(name, true));
        },
        updateRules: (rules, type) => {
            dispatch(bulkUpdateRules(rules, type));
        },
        getNode: () => {
            dispatch(getNode(navigationService.getParamByName('nodeId')));
        },
        getVariableTypes: () => {
            dispatch(requestVariableTypes());
        },
        onChangeEvent: (ownership, variables, name) => {
            dispatch(setVariableState(ownership, variables, name));
        },
        resetField: (formName, fieldName, values) => {
            dispatch(change(formName, fieldName, values));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(reduxForm({ enableReinitialize: true })(BulkUpdate));
