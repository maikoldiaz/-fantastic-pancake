import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { Field, reduxForm } from 'redux-form';
import { required } from 'redux-form-validators';
import { inputSelect } from '../../../../../common/components/formControl/formControl.jsx';
import { constants } from '../../../../../common/services/constants';
import { utilities } from '../../../../../common/services/utilities';
import { getTransferSourceNodes, requestUpdateTransferPoint, resetOnSourceNodeChange } from '../actions';
import { asyncValidate } from './../asyncValidate';
import { actionProvider } from '../../actionProvider';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';
import { refreshGrid } from '../../../../../common/components/grid/actions';

export class CreateTransferPointLogistic extends React.Component {
    constructor() {
        super();
        this.saveTransferPointLogistics = this.saveTransferPointLogistics.bind(this);
    }

    saveTransferPointLogistics(values) {
        const transferPoint = {
            transferPoint: values.transferPoint.name,
            logisticSourceCenter: this.props.sourceStorageLocation,
            logisticDestinationCenter: this.props.destinationStorageLocation,
            sourceProduct: values.sourceProduct.product.name,
            destinationProduct: values.destinationProduct.product.name,
            isDeleted: false
        };
        this.props.requestUpdateTransferPoint(transferPoint, 'POST');
    }

    render() {
        const nodeTypeValidate = () => this.props.nodeTypeFailure ? resourceProvider.read('nodeTypeNotFound') : null;
        return (
            <form name="createTransferPointOperational" id="frm_transferPointLogistics_create" className="ep-form" onSubmit={this.props.handleSubmit(this.saveTransferPointLogistics)}>
                <section className="ep-modal__content" id="sec_modal_group">
                    <div className="ep-content__body">
                        <form id="frm_transferPointLogistics_create" className="ep-form">
                            <div className="row">
                                <div className="col-md-12">
                                    <div className="ep-control-group">
                                        <label id="lbl_transferPointLogistics_transferPoint" className="ep-label" htmlFor="dd_transferPointLogistics_transferPoint_sel">
                                            {resourceProvider.read('transferPoint')}</label>
                                        <Field component={inputSelect} name="transferPoint" id="dd_transferPointLogistics_transferPoint"
                                            placeholder={resourceProvider.read('transferPoint')} inputId="dd_transferPointLogistics_transferPoint_sel"
                                            options={this.props.transferPoints} getOptionLabel={x => x.name} getOptionValue={x => x.name}
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                    </div>
                                </div>
                            </div>
                            <div className="row">
                                <div className="col-md-6">
                                    <div className="ep-control-group">
                                        <label id="lbl_transferPointLogistics_sourceNode" className="ep-label" htmlFor="dd_transferPointLogistics_sourceNode_sel">
                                            {resourceProvider.read('sourceNodeTransferOperational')}</label>
                                        <Field component={inputSelect} name="sourceNode" id="dd_transferPointLogistics_sourceNode"
                                            placeholder={resourceProvider.read('sourceNodeTransferOperational')} inputId="dd_transferPointLogistics_sourceNode_sel"
                                            options={this.props.sourceNodes} getOptionLabel={x => x.sourceNode.name} getOptionValue={x => x.sourceNode.name}
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } }), nodeTypeValidate]} />
                                    </div>
                                </div>
                                <div className="col-md-6">
                                    <div className="ep-control-group">
                                        <label id="lbl_transferPointLogistics_destinationNode" className="ep-label" htmlFor="dd_transferPointLogistics_destinationNode_sel">
                                            {resourceProvider.read('destinationNodeTransferOperational')}</label>
                                        <Field component={inputSelect} name="destinationNode" id="dd_transferPointLogistics_destinationNode"
                                            placeholder={resourceProvider.read('destinationNodeTransferOperational')} inputId="dd_transferPointLogistics_destinationNode_sel"
                                            options={this.props.destinationNodes ? this.props.destinationNodes : null}
                                            getOptionLabel={x => x.destinationNode.name} getOptionValue={x => x.destinationNode.name}
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } }), nodeTypeValidate]} />
                                    </div>
                                </div>
                            </div>
                            <div className="row">
                                <div className="col-md-6">
                                    <div className="ep-control-group">
                                        <label id="lbl_transferPointLogistics_sourceStorageLocation" className="ep-label" htmlFor="dd_transferPointLogistics_sourceStorageLocation_sel">
                                            {resourceProvider.read('sourceStorageLocation')}</label>
                                        <Field component={inputSelect} name="sourceStorageLocation" id="dd_transferPointLogistics_sourceStorageLocation"
                                            placeholder={resourceProvider.read('sourceNodeTransferOperational')} inputId="dd_transferPointLogistics_sourceStorageLocation_sel"
                                            options={this.props.sourceStorageLocations ? this.props.sourceStorageLocations : null}
                                            getOptionLabel={x => x.name} getOptionValue={x => x.name}
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } }), nodeTypeValidate]} />
                                    </div>
                                </div>
                                <div className="col-md-6">
                                    <div className="ep-control-group">
                                        <label id="lbl_transferPointLogistics_destinationStorageLocation" className="ep-label" htmlFor="dd_transferPointLogistics_destinationStorageLocation_sel">
                                            {resourceProvider.read('destinationStorageLocation')}</label>
                                        <Field component={inputSelect} name="destinationStorageLocation" id="dd_transferPointLogistics_destinationStorageLocation"
                                            placeholder={resourceProvider.read('destinationNodeTransferOperational')} inputId="dd_transferPointLogistics_destinationStorageLocation_sel"
                                            options={this.props.destinationStorageLocations ? this.props.destinationStorageLocations : null}
                                            getOptionLabel={x => x.name} getOptionValue={x => x.name}
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } }), nodeTypeValidate]} />
                                    </div>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col-md-6">
                                    <div className="ep-control-group m-b-3">
                                        <label className="ep-label">{resourceProvider.read('logisticSourceCenter')}</label>
                                        <span className="ep-data fw-500" id="lbl_transferPointLogistics_logisticSourceCenter">
                                            {!utilities.isNullOrUndefined(this.props.sourceStorageLocation) && !utilities.isNullOrUndefined(this.props.sourceLogisticCenter) ?
                                                this.props.sourceStorageLocation : null}
                                        </span>
                                    </div>
                                </div>
                                <div className="col-md-6">
                                    <div className="ep-control-group m-b-3">
                                        <label className="ep-label">{resourceProvider.read('logisticDestinationCenter')}</label>
                                        <span className="ep-data fw-500" id="lbl_transferPointLogistics_logisticDestinationCenter">
                                            {!utilities.isNullOrUndefined(this.props.destinationStorageLocation) && !utilities.isNullOrUndefined(this.props.destinationLogisticCenter) ?
                                                this.props.destinationStorageLocation : null}
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div className="row">
                                <div className="col-md-6">
                                    <div className="ep-control-group">
                                        <label id="lbl_transferPointLogistics_sourceProduct" className="ep-label" htmlFor="dd_transferPointLogistics_sourceProduct_sel">
                                            {resourceProvider.read('sourceProductTransferOperational')}</label>
                                        <Field component={inputSelect} name="sourceProduct" id="dd_transferPointLogistics_sourceProduct"
                                            placeholder={resourceProvider.read('sourceProductTransferOperational')} inputId="dd_transferPointLogistics_sourceProduct_sel"
                                            options={this.props.sourceProducts ? this.props.sourceProducts : null}
                                            getOptionLabel={x => x.product.name} getOptionValue={x => x.product.name}
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                    </div>
                                </div>
                                <div className="col-md-6">
                                    <div className="ep-control-group">
                                        <label id="lbl_transferPointLogistics_destinationProduct" className="ep-label" htmlFor="dd_transferPointLogistics_destinationProduct_sel">
                                            {resourceProvider.read('destinationProductTransferPoints')}</label>
                                        <Field component={inputSelect} name="destinationProduct" id="dd_transferPointLogistics_destinationProduct"
                                            placeholder={resourceProvider.read('destinationProductTransferOperational')} inputId="dd_transferPointLogistics_destinationProduct_sel"
                                            options={this.props.destinationProducts ? this.props.destinationProducts : null}
                                            getOptionLabel={x => x.product.name} getOptionValue={x => x.product.name}
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                    </div>
                                </div>
                            </div>
                        </form>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('transferPointLogisticsCreate')} />
            </form>
        );
    }

    componentDidMount() {
        this.props.resetOnSourceNodeChange();
        this.props.getTransferSourceNodes();
    }

    componentDidUpdate(prevProps) {
        if (prevProps.createToggler !== this.props.createToggler) {
            this.props.refreshGrid();
            this.props.closeModal();
        }
        if (prevProps.fieldChange.fieldChangeToggler !== this.props.fieldChange.fieldChangeToggler) {
            const actions = actionProvider.getlogisticsActions(this.props.fieldChange.currentModifiedField, this.props.fieldChange.currentModifiedValue);
            if (actions.length === 0) {
                return;
            }

            this.props.dispatchActions(actions);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        mode: state.modal.modalKey,
        transferPoints: state.shared.groupedCategoryElements[constants.Category.TransferPoint],
        sourceNodes: state.transferPointsLogistics.sourceNodes,
        destinationNodes: state.transferPointsLogistics.destinationNodes,
        sourceStorageLocations: state.transferPointsLogistics.sourceStorageLocations,
        destinationStorageLocations: state.transferPointsLogistics.destinationStorageLocations,
        sourceProducts: state.transferPointsLogistics.sourceProducts,
        destinationProducts: state.transferPointsLogistics.destinationProducts,
        sourceLogisticCenter: state.transferPointsLogistics.sourceLogisticCenter,
        destinationLogisticCenter: state.transferPointsLogistics.destinationLogisticCenter,
        sourceStorageLocation: state.transferPointsLogistics.sourceStorageLocation,
        destinationStorageLocation: state.transferPointsLogistics.destinationStorageLocation,
        createToggler: state.transferPointsLogistics.createToggler,
        fieldChange: state.transferPointsLogistics.fieldChange
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getTransferSourceNodes: () => {
            dispatch(getTransferSourceNodes());
        },
        requestUpdateTransferPoint: (transferPoint, method) => {
            dispatch(requestUpdateTransferPoint(transferPoint, method));
        },
        resetOnSourceNodeChange: () => {
            dispatch(resetOnSourceNodeChange());
        },
        refreshGrid: () => {
            dispatch(refreshGrid('transferPointsLogistics'));
        },
        dispatchActions: actions => {
            actions.forEach(dispatch);
        }
    };
};

const CreateTransferPointLogisticForm = reduxForm({
    form: 'createTransferPointLogistics',
    enableReinitialize: true,
    asyncValidate,
    shouldAsyncValidate: utilities.shouldAsyncValidate
})(CreateTransferPointLogistic);


/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(CreateTransferPointLogisticForm);
