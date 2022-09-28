import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { Field, reduxForm, change } from 'redux-form';
import { required, length } from 'redux-form-validators';
import { inputSelect, inputTextbox } from '../../../../../common/components/formControl/formControl.jsx';
import {
    refreshTransferPointRow, requestUpdateTransferPoint, getTransferSourceNodes, getTransferDestinationNodes,
    getTransferSourceProducts, getNodeType, resetNodeType, refreshCreateSuccess
} from '../actions';
import { showError } from '../../../../../common/actions';
import { constants } from '../../../../../common/services/constants';
import { utilities } from '../../../../../common/services/utilities';
import { asyncValidate } from './../asyncValidate';
import { footerConfigService } from '../../../../../common/services/footerConfigService';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { refreshGrid } from '../../../../../common/components/grid/actions';

export class CreateTransferPointOperational extends React.Component {
    constructor() {
        super();
        this.saveTransferPointOperational = this.saveTransferPointOperational.bind(this);
    }

    saveTransferPointOperational(values) {
        const transferPointOperational = {
            operativeNodeRelationshipId: this.props.mode === constants.Modes.Update ? this.props.initialValues.operativeNodeRelationshipId : 0,
            transferPoint: values.transferPoint.name,
            sourceField: values.sourceField,
            fieldWaterProduction: values.fieldWaterProduction,
            relatedSourceField: values.relatedSourceField,
            destinationNode: values.destinationNode.destinationNode.name,
            destinationNodeType: this.props.destinationNodeType,
            movementType: values.movementType.name,
            sourceNode: values.sourceNode.sourceNode.name,
            sourceNodeType: this.props.sourceNodeType,
            sourceProduct: values.sourceProduct.product.name,
            sourceProductType: values.sourceProductType.name,
            isDeleted: false,
            rowVersion: this.props.mode === constants.Modes.Update ? this.props.initialValues.rowVersion : null
        };
        if (this.props.mode === constants.Modes.Create) {
            this.props.requestUpdateTransferPoint(transferPointOperational, 'POST');
        } else {
            this.props.requestUpdateTransferPoint(transferPointOperational, 'PUT');
        }
    }

    render() {
        const isCreate = this.props.mode === constants.Modes.Create;
        const nodeTypeValidate = () => this.props.nodeTypeFailure ? resourceProvider.read('nodeTypeNotFound') : null;
        return (
            <>
                <form name="createTransferPointOperational" id="frm_transferPointOperational_createUpdate" onSubmit={this.props.handleSubmit(this.saveTransferPointOperational)} className="ep-form">
                    <section className="ep-modal__content" id="sec_modal_group">
                        <div className="ep-content__body">
                            <form id="frm_transferPointOperational_createUpdate" className="ep-form">
                                <div className="row">
                                    <div className="col-md-12">
                                        <div className="ep-control-group">
                                            <label id="lbl_transferPointOperational_transferPoint" className="ep-label" htmlFor="dd_transferPointOperational_transferPoint_sel">
                                                {resourceProvider.read('transferPoint')}</label>
                                            <Field component={inputSelect} name="transferPoint" id="dd_transferPointOperational_transferPoint"
                                                placeholder={resourceProvider.read('transferPoint')} inputId="dd_transferPointOperational_transferPoint_sel"
                                                validate={[required({ msg: { presence: resourceProvider.read('required') } })]}
                                                isDisabled={!isCreate} getOptionLabel={x => x.name} getOptionValue={x => x.name}
                                                options={this.props.transferPoints} />
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-12">
                                        <div className="ep-control-group">
                                            <label id="lbl_transferPointOperational_transferPointType" className="ep-label" htmlFor="dd_transferPointOperational_movementType_sel">
                                                {resourceProvider.read('movementTypeTransferOperational')}</label>
                                            <Field component={inputSelect} name="movementType" id="dd_transferPointOperational_movementType"
                                                placeholder={resourceProvider.read('movementTypeTransferOperational')} inputId="dd_transferPointOperational_movementType_sel"
                                                validate={[required({ msg: { presence: resourceProvider.read('required') } })]}
                                                isDisabled={!isCreate} options={this.props.movementTypes}
                                                getOptionLabel={x => x.name} getOptionValue={x => x.name} />
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="ep-control-group">
                                            <label id="lbl_transferPointOperational_sourceNode" className="ep-label" htmlFor="dd_transferPointOperational_type_sel">
                                                {resourceProvider.read('sourceNodeTransferOperational')}</label>
                                            <Field component={inputSelect} name="sourceNode" id="dd_transferPointOperational_type"
                                                placeholder={resourceProvider.read('sourceNodeTransferOperational')} inputId="dd_transferPointOperational_type_sel"
                                                validate={[required({ msg: { presence: resourceProvider.read('required') } }), nodeTypeValidate]}
                                                isDisabled={!isCreate} options={this.props.sourceNodes}
                                                getOptionLabel={x => x.sourceNode.name} getOptionValue={x => x.sourceNode.name} />
                                        </div>
                                    </div>
                                    <div className="col-md-6">
                                        <div className="ep-control-group">
                                            <label id="lbl_transferPointOperational_nodeType" className="ep-label" htmlFor="dd_transferPointOperational_destinationNode_sel">
                                                {resourceProvider.read('destinationNodeTransferOperational')}</label>
                                            <Field component={inputSelect} name="destinationNode" id="dd_transferPointOperational_destinationNode"
                                                placeholder={resourceProvider.read('destinationNodeTransferOperational')} inputId="dd_transferPointOperational_destinationNode_sel"
                                                validate={[required({ msg: { presence: resourceProvider.read('required') } }), nodeTypeValidate]}
                                                isDisabled={!isCreate} options={this.props.destinationNodes ? this.props.destinationNodes : null}
                                                getOptionLabel={x => x.destinationNode.name} getOptionValue={x => x.destinationNode.name} />
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-3">
                                            <label className="ep-label">{resourceProvider.read('sourceNodeType')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointOperational_sourceNodeType">
                                                {this.props.sourceNodeType}
                                            </span>
                                        </div>
                                    </div>
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-3">
                                            <label className="ep-label">{resourceProvider.read('destinationNodeType')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointOperational_destinationNodeType">
                                                {this.props.destinationNodeType}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="ep-control-group">
                                            <label id="lbl_transferPointOperational_nodeType" className="ep-label" htmlFor="dd_transferPointOperational_sourceProduct_sel">
                                                {resourceProvider.read('sourceProductTransferOperational')}</label>
                                            <Field component={inputSelect} name="sourceProduct" id="dd_transferPointOperational_sourceProduct"
                                                placeholder={resourceProvider.read('sourceProductTransferOperational')} inputId="dd_transferPointOperational_sourceProduct_sel"
                                                validate={[required({ msg: { presence: resourceProvider.read('required') } })]}
                                                isDisabled={!isCreate} options={this.props.sourceProducts}
                                                getOptionLabel={x => x.product.name} getOptionValue={x => x.product.name} />
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="ep-control-group">
                                            <label id="lbl_transferPointOperational_nodeType" className="ep-label" htmlFor="dd_transferPointOperational_sourceProductType_sel">
                                                {resourceProvider.read('sourceProductType')}</label>
                                            <Field component={inputSelect} name="sourceProductType" id="dd_transferPointOperational_sourceProductType"
                                                placeholder={resourceProvider.read('sourceProductType')} inputId="dd_transferPointOperational_sourceProductType_sel"
                                                validate={[required({ msg: { presence: resourceProvider.read('required') } })]}
                                                isDisabled={!isCreate} options={this.props.productTypes}
                                                getOptionLabel={x => x.name} getOptionValue={x => x.name} />
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-12">
                                        <div className="ep-control-group">
                                            <label id="lbl_transferPointOperational_name" className="ep-label" htmlFor="txt_createTransferPointOperational_camp">
                                                {resourceProvider.read('camp')}</label>
                                            <Field type="text" id="txt_createTransferPointOperational_camp" component={inputTextbox}
                                                placeholder={resourceProvider.read('camp')} name="sourceField"
                                                validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                                    length({ max: 200, msg: resourceProvider.read('transferpointsoperationalLengthValidation') })]} />
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-12">
                                        <div className="ep-control-group">
                                            <label id="lbl_transferPointOperational_name" className="ep-label" htmlFor="txt_createTransferPointOperational_waterCamp">
                                                {resourceProvider.read('waterCamp')}</label>
                                            <Field type="text" id="txt_createTransferPointOperational_waterCamp" component={inputTextbox}
                                                placeholder={resourceProvider.read('waterCamp')} name="fieldWaterProduction"
                                                validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                                    length({ max: 200, msg: resourceProvider.read('transferpointsoperationalLengthValidation') })]} />
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-12">
                                        <div className="ep-control-group">
                                            <label id="lbl_transferPointOperational_name" className="ep-label" htmlFor="txt_createTransferPointOperational_correlatedCases">
                                                {resourceProvider.read('correlatedCases')}</label>
                                            <Field type="text" id="txt_createTransferPointOperational_correlatedCases" component={inputTextbox}
                                                placeholder={resourceProvider.read('correlatedCases')} name="relatedSourceField"
                                                validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                                    length({ max: 200, msg: resourceProvider.read('transferpointsoperationalLengthValidation') })]} />
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </section>
                    <ModalFooter config={footerConfigService.getCommonConfig('createOperative')} />
                </form>
            </>
        );
    }

    componentDidMount() {
        this.props.getTransferSourceNodes();
        this.props.resetNodeType();
    }

    componentDidUpdate(prevProps) {
        if (prevProps.createSuccess !== this.props.createSuccess) {
            this.props.resetNodeType();
            this.props.closeModal();
            this.props.refreshGrid();
        }
        if (prevProps.updateSuccess !== this.props.updateSuccess) {
            this.props.refreshGrid();
            this.props.closeModal();
        }
        if (prevProps.fieldChange.fieldChangeToggler !== this.props.fieldChange.fieldChangeToggler) {
            if (this.props.fieldChange.currentModifiedField === 'sourceNode') {
                this.props.resetValue([`destinationNode`, `sourceProduct`]);
                if (!utilities.isNullOrUndefined(this.props.fieldChange.currentModifiedValue)) {
                    this.props.getTransferDestinationNodes(this.props.fieldChange.currentModifiedValue.sourceNodeId);
                    this.props.getTransferSourceProducts(this.props.fieldChange.currentModifiedValue.sourceNodeId);
                    this.props.getNodeType(this.props.fieldChange.currentModifiedValue.sourceNodeId, true);
                }
            } else if (this.props.fieldChange.currentModifiedField === 'destinationNode') {
                if (!utilities.isNullOrUndefined(this.props.fieldChange.currentModifiedValue)) {
                    this.props.getNodeType(this.props.fieldChange.currentModifiedValue.destinationNode.nodeId, false);
                }
            }
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        transferPoint: state.transferPointsOperational.transferPoint ? state.transferPointsOperational.transferPoint : null,
        initialValues: state.modal.mode !== constants.Modes.Create ? state.transferPointsOperational.initialValues : null,
        transferPoints: state.shared.groupedCategoryElements[constants.Category.TransferPoint],
        movementTypes: state.shared.groupedCategoryElements[constants.Category.MovementType],
        productTypes: state.shared.groupedCategoryElements[constants.Category.ProductType],
        sourceNodes: state.transferPointsOperational.sourceNodes,
        destinationNodes: state.transferPointsOperational.destinationNodes,
        sourceProducts: state.transferPointsOperational.sourceProducts,
        createSuccess: state.transferPointsOperational.createSuccess,
        updateSuccess: state.transferPointsOperational.updateSuccess,
        nodeTypeFailure: state.transferPointsOperational.nodeTypeFailure,
        sourceNodeType: state.modal.mode !== constants.Modes.Create ?
            state.transferPointsOperational.initialValues.sourceNodeType : state.transferPointsOperational.sourceNodeType,
        destinationNodeType: state.modal.mode !== constants.Modes.Create ?
            state.transferPointsOperational.initialValues.destinationNodeType : state.transferPointsOperational.destinationNodeType,
        fieldChange: state.transferPointsOperational.fieldChange
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        refreshTransferPointRow: () => {
            dispatch(refreshTransferPointRow());
        },
        requestUpdateTransferPoint: (values, method) => {
            dispatch(requestUpdateTransferPoint(values, method));
        },
        getTransferSourceNodes: () => {
            dispatch(getTransferSourceNodes());
        },
        getTransferDestinationNodes: sourceNodeId => {
            dispatch(getTransferDestinationNodes(sourceNodeId));
        },
        getTransferSourceProducts: sourceNodeId => {
            dispatch(getTransferSourceProducts(sourceNodeId));
        },
        getNodeType: (sourceNodeId, isSource) => {
            dispatch(getNodeType(sourceNodeId, isSource));
        },
        resetValue: fields => {
            fields.map(field => dispatch(change('createTransferPointOperational', field, null)));
        },
        showError: () => {
            dispatch(showError(resourceProvider.read('transferPointOperationalExistsMessage'), true, resourceProvider.read('transferPointOperationalExistsTitle')));
        },
        resetNodeType: () => {
            dispatch(resetNodeType());
        },
        refreshGrid: () => {
            dispatch(refreshGrid('transferPointsOperational'));
        },
        refreshCreateSuccess: () => {
            dispatch(refreshCreateSuccess());
        }
    };
};

const CreateTransferPointOperationalForm = reduxForm({
    form: 'createTransferPointOperational',
    enableReinitialize: true,
    asyncValidate,
    shouldAsyncValidate: utilities.shouldAsyncValidate
})(CreateTransferPointOperational);


/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(CreateTransferPointOperationalForm);
