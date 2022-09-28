import React from 'react';
import { connect } from 'react-redux';
import { constants } from '../../../../common/services/constants';
import { required, length, numericality } from 'redux-form-validators';
import { reduxForm, Field, change } from 'redux-form';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { inputTextbox, inputSelect, inputTextarea, inputDecimal } from './../../../../common/components/formControl/formControl.jsx';
import OwnershipMovementInventoryGrid from './ownershipMovementInventoryGrid.jsx';
import { dateService } from '../../../../common/services/dateService';
import { dataService } from '../services/dataService';
import { utilities } from '../../../../common/services/utilities';
import { showError, showInfoWithButton } from './../../../../common/actions';
import Tooltip from '../../../../common/components/tooltip/tooltip.jsx';
import { TooltipOverlay } from './tooltipOverlay.jsx';
import {
    requestOwnersForInventory, requestOwnersForMovement, setMovementInventoryOwnershipData,
    setSourceNodes, setDestinationNodes, updateNodeMovementInventoryData, startEdit,
    clearSelectedData, setDestinationProducts, setSourceProducts, clearOwnershipData,
    setDate, requestContractData, receiveContractData
} from '../actions';
import { movementValidator } from '../services/movementValidator';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';

export class CreateMovement extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
        this.getOwnersData = this.getOwnersData.bind(this);
        this.isEditor = this.isEditor.bind(this);
        this.getContractData = this.getContractData.bind(this);
        this.destinationProductDisabled = this.destinationProductDisabled.bind(this);
        this.sourceProductRequired = this.sourceProductRequired.bind(this);
        this.destinationProductRequired = this.destinationProductRequired.bind(this);
        this.contractRequired = this.contractRequired.bind(this);
    }

    getOwnersData() {
        if (this.props.selectedData) {
            const sourceNodes = this.props.selectedData.sourceNodes;
            const destinationNodes = this.props.selectedData.destinationNodes;
            const sourceProduct = this.props.selectedData.sourceProduct;
            const destinationProduct = this.props.selectedData.destinationProduct;
            const variable = this.props.selectedData.variable;
            const statusArray = [constants.VariableType.Input, constants.VariableType.Output];
            if (utilities.isArrayNotEmpty([sourceNodes, destinationNodes, variable, sourceProduct]) && dataService.compareStatus(variable.variableTypeId, statusArray, 'eq', 'or')) {
                this.props.getOwnersForMovement(sourceNodes.nodeId, destinationNodes.nodeId, sourceProduct.productId);
            } else if (utilities.isArrayNotEmpty([sourceNodes, variable, sourceProduct]) && dataService.compareStatus(variable.variableTypeId, statusArray, 'ne', 'and')) {
                this.props.getOwnersForInventory(sourceNodes.nodeId, sourceProduct.productId);
            } else if (utilities.isArrayNotEmpty([destinationNodes, destinationProduct, variable]) && dataService.compareStatus(variable.variableTypeId, statusArray, 'ne', 'and')) {
                this.props.getOwnersForInventory(destinationNodes.nodeId, destinationProduct.productId);
            } else if (utilities.isArrayNotEmpty([destinationNodes, variable, destinationProduct]) && dataService.compareStatus(variable.variableTypeId, [constants.VariableType.Input], 'eq', 'or')) {
                this.props.getOwnersForInventory(destinationNodes.nodeId, destinationProduct.productId);
            } else if (utilities.isArrayNotEmpty([sourceNodes, variable, sourceProduct]) && dataService.compareStatus(variable.variableTypeId, [constants.VariableType.Output], 'eq', 'or')) {
                this.props.getOwnersForInventory(sourceNodes.nodeId, sourceProduct.productId);
            }
        }
    }

    getContractData() {
        if (this.props.selectedData
            && utilities.isArrayNotEmpty([this.props.selectedData.movementType, this.props.selectedData.sourceProduct, this.props.selectedData.destinationProduct])
            && (this.props.selectedData.movementType.elementId === constants.MovementType.Purchase || this.props.selectedData.movementType.elementId === constants.MovementType.Sale)
            && (this.props.selectedData.sourceProduct.productId !== null || this.props.selectedData.destinationProduct.productId !== null)) {
            const date = dateService.parseToISOString(this.props.ownershipNode.nodeDetails.ticket.endDate, constants.DateFormat.FullDate);
            this.props.getContractData(this.props.selectedData, date);
        } else {
            this.props.setContracts([]);
            this.props.selectContract();
        }
    }

    addOwnersData() {
        let ownersData = null;

        if (!utilities.isNullOrUndefined(this.props.selectedData.variable) && (this.props.selectedData.variable.variableTypeId === constants.VariableType.Input ||
            this.props.selectedData.variable.variableTypeId === constants.VariableType.Output) && this.props.ownershipNode.movementOwners) {
            ownersData = this.props.ownershipNode.movementOwners;
        } else {
            ownersData = this.props.ownershipNode.inventoryOwners;
        }
        let movementInventoryOwnershipData = [];

        const existingOwnerIds = this.props.movementInventoryOwnershipData ? this.props.movementInventoryOwnershipData.map(x => x.ownerId) : [];
        const newOwnerIds = ownersData ? ownersData.map(x => x.ownerId) : [];

        let noNewOwner = existingOwnerIds.every(item => {
            return newOwnerIds.includes(item);
        });

        if (noNewOwner) {
            noNewOwner = newOwnerIds.every(item => {
                return existingOwnerIds.includes(item);
            });
        }

        if (!noNewOwner) {
            if (!utilities.isNullOrUndefined(ownersData) && ownersData.length > 0) {
                movementInventoryOwnershipData = ownersData.map(x => {
                    return {
                        netVolume: parseFloat(this.props.selectedData.netVolume),
                        ownershipVolume: parseFloat(x.ownershipPercentage * this.props.selectedData.netVolume / 100),
                        ownershipPercentage: parseInt(x.ownershipPercentage, 10),
                        ownerId: x.ownerId,
                        ownerName: x.owner.name,
                        color: x.owner.color || constants.DefaultColorCode
                    };
                });

                this.props.setMovementInventoryOwnershipData(movementInventoryOwnershipData);
            }
        }
    }

    isSourceNodeDisabled() {
        if (utilities.isNullOrUndefined(this.props.selectedData.variable)) {
            return true;
        }
        return dataService.compareStatus(this.props.selectedData.variable.variableTypeId,
            [constants.VariableType.Input, constants.VariableType.Tolerance, constants.VariableType.UnidentifiedLoss], 'ne', 'and');
    }

    isDestinationNodeDisabled() {
        if (utilities.isNullOrUndefined(this.props.selectedData.variable)) {
            return true;
        }
        return dataService.compareStatus(this.props.selectedData.variable.variableTypeId,
            [constants.VariableType.Output, constants.VariableType.Tolerance, constants.VariableType.UnidentifiedLoss], 'ne', 'and');
    }

    isEditor() {
        const nodeOwnershipStatus = this.props.ownershipNode.nodeDetails.ownershipStatus;
        const editor = this.props.ownershipNode.nodeDetails.editor;
        return nodeOwnershipStatus !== constants.OwnershipNodeStatus.LOCKED || (nodeOwnershipStatus === constants.OwnershipNodeStatus.LOCKED && this.props.currentUser === editor);
    }

    onSubmit(formValues) {
        if (!this.isEditor()) {
            this.props.closeModal();
            const message = resourceProvider.readFormat('balanceIsBeingEditedMessage', [this.props.ownershipNode.nodeDetails.editor]);
            const title = resourceProvider.read('balanceIsBeingEditedTitle');
            this.props.showInfoWithButton(message, title, 'requestUnlockButtonText');
        } else {
            const errorCheckObject = dataService.buildErrorCheckObject(formValues);
            const isValid = movementValidator.validateMovement(formValues, this.props.movementInventoryOwnershipData, errorCheckObject);

            if (isValid && this.props.movementInventoryOwnershipData.length > 0) {
                let transactionId = this.props.ownershipNode.nodeMovementInventoryData && this.props.ownershipNode.nodeMovementInventoryData.length > 0 ?
                    this.props.ownershipNode.nodeMovementInventoryData.reduce((min, p) => p.transactionId < min ? p.transactionId : min,
                        this.props.ownershipNode.nodeMovementInventoryData[0].transactionId) : 0;
                transactionId = transactionId > 0 ? -1 : transactionId - 1;
                let movementInventoryOwnershipData = dataService.buildMovementInventoryOwnershipArray(this.props.movementInventoryOwnershipData, formValues);
                movementInventoryOwnershipData = movementInventoryOwnershipData.map(item => {
                    return Object.assign({}, item, {
                        transactionId: transactionId,
                        operationalDate: this.props.ownershipNode.nodeDetails.ticket.endDate,
                        sourceNode: errorCheckObject.sourceNode,
                        sourceNodeId: errorCheckObject.sourceNodeId,
                        destinationNode: errorCheckObject.destinationNode,
                        destinationNodeId: errorCheckObject.destinationNodeId,
                        sourceProduct: errorCheckObject.sourceProduct,
                        sourceProductId: errorCheckObject.sourceProductId,
                        destinationProduct: errorCheckObject.destinationProduct,
                        destinationProductId: errorCheckObject.destinationProductId,
                        ownershipFunction: 'Propiedad Manual',
                        ruleVersion: 1,
                        isMovement: 1
                    });
                });
                this.props.setMovementInventoryOwnershipData(movementInventoryOwnershipData);
                this.props.updateNodeMovementInventoryData(movementInventoryOwnershipData);
                this.props.startEdit(this.props.ownershipNode.startEditToggler);
                this.props.closeModal();
            }
        }
    }

    destinationProductDisabled() {
        return utilities.isArrayNotEmpty([this.props.selectedData.variable, this.props.selectedData])
            && this.props.selectedData.variable.variableTypeId === constants.VariableType.IdentifiedLoss;
    }

    sourceProductRequired() {
        const variableList = [constants.VariableType.Tolerance, constants.VariableType.UnidentifiedLoss];
        if (this.props.selectedData.variable !== null && this.props.selectedData.variable.variableTypeId === constants.VariableType.Input && !this.props.selectedData.sourceNodes) {
            variableList.push(constants.VariableType.Input);
        }
        return this.props.selectedData.variable === null ? true
            : dataService.compareStatus(this.props.selectedData.variable.variableTypeId, variableList, 'ne', 'and');
    }

    destinationProductRequired() {
        const variableList = [constants.VariableType.Tolerance, constants.VariableType.UnidentifiedLoss, constants.VariableType.IdentifiedLoss];
        if (this.props.selectedData.variable !== null && this.props.selectedData.variable.variableTypeId === constants.VariableType.Output && !this.props.selectedData.destinationNodes) {
            variableList.push(constants.VariableType.Output);
        }
        return this.props.selectedData.variable === null ? true : dataService.compareStatus(this.props.selectedData.variable.variableTypeId, variableList, 'ne', 'and');
    }

    contractRequired() {
        return (this.props.selectedData.variable === null || this.props.selectedData.movementType === null) ? true
            : (this.props.selectedData.variable.variableTypeId === constants.VariableType.UnidentifiedLoss &&
                dataService.compareStatus(this.props.selectedData.variable.variableTypeId, [constants.MovementType.Purchase, constants.MovementType.Sale], 'eq', 'or'));
    }

    render() {
        return (
            <form id="frm_createMovement_create" className="ep-form" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                <section className="ep-modal__content">
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label id="lbl_createMovement_date" className="ep-label" htmlFor="dt_createMovement_date">{resourceProvider.read('date')}</label>
                                <Field id="dt_createMovement_date" className="text-caps" component={inputTextbox} name="movementDate"
                                    disabled={true} />
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label id="lbl_createMovement_netVolume" className="ep-label" htmlFor="txt_decimal_netVolume">{resourceProvider.read('volume')}</label>
                                <Field id="txt_decimal_netVolume" type={constants.InputType.Decimal}
                                    min={constants.DecimalRange.MinNegative} max={constants.DecimalRange.MaxIntValue} step={constants.DecimalRange.Step}
                                    component={inputDecimal} name="netVolume"
                                    value={parseFloat(this.props.selectedData.netVolume)}
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                        numericality({
                                            greaterThanOrEqualTo: constants.DecimalRange.MinNegative,
                                            lessThanOrEqualTo: constants.DecimalRange.MaxIntValue,
                                            msg: {
                                                greaterThanOrEqualTo: resourceProvider.read('invalidRange'),
                                                lessThanOrEqualTo: resourceProvider.read('invalidRange')
                                            },
                                            allowBlank: false
                                        })]} />
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label className="ep-label" htmlFor="dd_createMovement_unit_sel">{resourceProvider.read('unit')}</label>
                                <Field id="dd_createMovement_unit" component={inputSelect} name="unit" inputId="dd_createMovement_unit_sel"
                                    options={this.props.units} getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label className="ep-label" id="lbl_createMovement_variable" htmlFor="dd_createMovement_variable_sel">{resourceProvider.read('variable')}</label>
                                <Field id="dd_createMovement_variable" component={inputSelect} name="variable" inputId="dd_createMovement_variable_sel"
                                    options={this.props.variableTypes} getOptionLabel={x => x.name} getOptionValue={x => x.variableTypeId}
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label className="ep-label" htmlFor="dd_createMovement_sourceNode_sel">{resourceProvider.read('sourceNode')}</label>
                                <Field id="dd_createMovement_sourceNode" component={inputSelect} name="sourceNodes" inputId="dd_createMovement_sourceNode_sel"
                                    options={this.props.ownershipNode.sourceNodes} getOptionLabel={x => x.sourceNode.name} getOptionValue={x => x.sourceNode.nodeId}
                                    isDisabled={this.isSourceNodeDisabled()} />
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label className="ep-label" htmlFor="dd_createMovement_destinationNode_sel">{resourceProvider.read('destinationNode')}</label>
                                <Field id="dd_createMovement_destinationNode" component={inputSelect} name="destinationNodes" inputId="dd_createMovement_destinationNode_sel"
                                    options={this.props.ownershipNode.destinationNodes} getOptionLabel={x => x.destinationNode.name} getOptionValue={x => x.destinationNode.nodeId}
                                    isDisabled={this.isDestinationNodeDisabled()} />
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label className="ep-label" htmlFor="dd_createMovement_sourceProduct_sel">{resourceProvider.read('sourceProduct')}</label>
                                <Field id="dd_createMovement_sourceProduct" component={inputSelect} name="sourceProduct" inputId="dd_createMovement_sourceProduct_sel"
                                    options={this.props.ownershipNode.sourceProducts} getOptionLabel={x => x.product.name} getOptionValue={x => x.product.productId}
                                    validate={this.sourceProductRequired() ? [required({ msg: { presence: resourceProvider.read('required') } })] : null} />
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label className="ep-label" htmlFor="dd_createMovement_destinationProduct_sel">{resourceProvider.read('destinationProduct')}</label>
                                <Field id="dd_createMovement_destinationProduct" component={inputSelect} name="destinationProduct" inputId="dd_createMovement_destinationProduct_sel"
                                    options={this.props.ownershipNode.destinationProducts} getOptionLabel={x => x.product.name} getOptionValue={x => x.product.productId}
                                    isDisabled={this.destinationProductDisabled()}
                                    validate={this.destinationProductRequired() ? [required({ msg: { presence: resourceProvider.read('required') } })] : null} />
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label className="ep-label" htmlFor="dd_createMovement_movementType_sel">{resourceProvider.read('movementType')}</label>
                                <Field id="dd_createMovement_movementType" component={inputSelect} name="movementType" inputId="dd_createMovement_movementType_sel"
                                    options={this.props.movementTypes} getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label className="ep-label" htmlFor="dd_createMovement_contract_sel">{resourceProvider.read('orderPosition')}</label>
                                <Field id="dd_createMovement_contract" component={inputSelect} name="contract" inputId="dd_createMovement_contract_sel"
                                    options={this.props.ownershipNode.contracts}
                                    getOptionLabel={x => utilities.buildDocumentNumberPosition(x.documentNumber, x.position)} getOptionValue={x => x.contractId}
                                    validate={this.contractRequired() ? [required({ msg: { presence: resourceProvider.read('required') } })] : null} />
                                {this.props.selectedData.contract &&
                                    <div className="ep-control-group__tip">
                                        <Tooltip body={<TooltipOverlay data={this.props.selectedData.contract} />} />
                                        <span className="m-l-2">
                                            {resourceProvider.read('properties') + ': ' + this.props.selectedData.contract.owner1.name + ' - ' + this.props.selectedData.contract.owner2.name}
                                        </span>
                                    </div>
                                }
                            </div>
                        </div>
                    </div>
                    <div className="ep-pane">
                        <h2 className="ep-control-group-title text-uppercase m-t-3">{resourceProvider.read('owners')}</h2>
                    </div>
                    <div className="row">
                        <div className="col-md-12 m-b-6">
                            <OwnershipMovementInventoryGrid {...this.props} />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label id="lbl_createMovement_reasonForChange" className="ep-label" htmlFor="dd_createMovement_reasonForChange_sel">
                                    {resourceProvider.read('reasonForChange')}:</label>
                                <Field id="dd_createMovement_reasonForChange" component={inputSelect} name="reasonForChange" inputId="dd_createMovement_reasonForChange_sel"
                                    options={this.props.reasonforChange} getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="ep-control-group">
                                <label className="ep-control-group-title text-uppercase d-block m-b-2" htmlFor="txt_createMovement_comments">{resourceProvider.read('comments')}</label>
                                <Field type="text" id="txt_createMovement_comments" component={inputTextarea} name="comment"
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                        length({ max: 150, msg: resourceProvider.read('shortNameLengthValidation') })]} />
                            </div>
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('createMovement', { acceptText: 'accept' })} />
            </form>
        );
    }

    componentDidMount() {
        this.props.setDate();
    }

    componentDidUpdate(prevProps) {
        if (prevProps.ownershipNode.selectedDataToggler !== this.props.ownershipNode.selectedDataToggler) {
            this.getOwnersData();
            this.getContractData();
        }
        if (prevProps.ownershipNode.movementOwnersDataToggler !== this.props.ownershipNode.movementOwnersDataToggler ||
            prevProps.ownershipNode.inventoryOwnersDataToggler !== this.props.ownershipNode.inventoryOwnersDataToggler) {
            this.addOwnersData();
        }
        if (prevProps.selectedData[this.props.selectedField] !== this.props.selectedData[this.props.selectedField] && this.props.selectedData[this.props.selectedField]) {
            const params = {
                nodeId: this.props.ownershipNode.nodeDetails.nodeId,
                sourceNode: [{
                    sourceNode: {
                        nodeId: this.props.ownershipNode.nodeDetails.nodeId,
                        name: this.props.ownershipNode.nodeDetails.node.name
                    }
                }],
                destinationNode: [{
                    destinationNode: {
                        nodeId: this.props.ownershipNode.nodeDetails.nodeId,
                        name: this.props.ownershipNode.nodeDetails.node.name
                    }
                }],
                selection: this.props.selectedData[this.props.selectedField]
            };
            const actions = dataService.getDispatchActions(this.props.selectedField, params);
            if (actions.length === 0) {
                return;
            }
            this.props.dispatchActions(actions);
        }
    }

    componentWillUnmount() {
        this.props.clearSelectedData();
        this.props.clearOwnershipData();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    const selectedData = state.nodeOwnership.ownershipNode.selectedData;
    return {
        movementInventoryOwnershipData: state.nodeOwnership.ownershipNode.movementInventoryOwnershipData,
        ownershipNode: state.nodeOwnership.ownershipNode,
        variableTypes: state.shared.variableTypes.filter(item => item.variableTypeId !== constants.VariableType.InitialInventory && item.variableTypeId !== constants.VariableType.FinalInventory),
        units: state.shared.groupedCategoryElements[constants.Category.UnitMeasurement],
        movementTypes: state.shared.groupedCategoryElements[constants.Category.MovementType],
        reasonforChange: state.shared.groupedCategoryElements[constants.Category.ReasonForChange],
        selectedData,
        currentUser: state.root.context.userId,
        initialValues: state.nodeOwnership.ownershipNode.initialDate,
        selectedField: state.nodeOwnership.ownershipNode.selectedField
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getOwnersForMovement: (sourceNodeId, destinationNodeId, productId) => {
            dispatch(requestOwnersForMovement(sourceNodeId, destinationNodeId, productId));
        },
        getOwnersForInventory: (nodeId, productId) => {
            dispatch(requestOwnersForInventory(nodeId, productId));
        },
        setMovementInventoryOwnershipData: data => {
            dispatch(setMovementInventoryOwnershipData(data));
        },
        setSourceNodes: node => {
            dispatch(setSourceNodes(node));
        },
        setDestinationNodes: node => {
            dispatch(setDestinationNodes(node));
        },
        showError: (message, showOnModal, title) => {
            dispatch(showError(message, showOnModal, title));
        },
        updateNodeMovementInventoryData: movementInventoryOwnershipData => {
            dispatch(updateNodeMovementInventoryData(movementInventoryOwnershipData));
        },
        startEdit: startEditToggler => {
            dispatch(startEdit(startEditToggler));
        },
        showInfoWithButton: (message, title, buttonText) => {
            dispatch(showInfoWithButton(message, false, title, buttonText));
        },
        clearSelectedData: () => {
            dispatch(clearSelectedData());
        },
        setDestinationProducts: product => {
            dispatch(setDestinationProducts(product));
        },
        setSourceProducts: product => {
            dispatch(setSourceProducts(product));
        },
        clearOwnershipData: () => {
            dispatch(clearOwnershipData());
        },
        setDate: () => {
            dispatch(setDate());
        },
        getContractData: (selectedData, date) => {
            dispatch(requestContractData(selectedData, date));
        },
        selectContract: () => {
            dispatch(change('createMovement', `contract`, null));
        },
        setContracts: contracts => {
            dispatch(receiveContractData(contracts));
        },
        dispatchActions: actions => {
            actions.forEach(dispatch);
        }
    };
};

const CreateMovementForm = reduxForm({ form: 'createMovement' })(CreateMovement);
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(CreateMovementForm);
