import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { Field, reduxForm } from 'redux-form';
import { required, length } from 'redux-form-validators';
import { inputTextarea } from '../../../../../common/components/formControl/formControl.jsx';
import { requestUpdateTransferPoint } from '../actions';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';
import { refreshGrid } from '../../../../../common/components/grid/actions';

class DeleteTransferPointOperational extends React.Component {
    constructor() {
        super();
        this.deleteTransferPoint = this.deleteTransferPoint.bind(this);
    }

    deleteTransferPoint(values) {
        values.isDeleted = true;
        values.rowVersion = this.props.initialValues.rowVersion;
        this.props.requestUpdateTransferPoint(values);
    }

    render() {
        return (
            <>
                <form id="frm_transferPointOperational_delete" className="ep-form" onSubmit={this.props.handleSubmit(this.deleteTransferPoint)}>
                    <section className="ep-modal__content" id="sec_modal_group">
                        <div className="ep-content__body">
                            <form id="frm_transferPointOperational_delete" className="ep-form">
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-4">
                                            <span className="ep-data" id="lbl_transferPointOperational_name">
                                                {resourceProvider.read('transferPointOperationalDeleteHeading')}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-4">
                                            <label className="ep-label">{resourceProvider.read('transferPoint')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointOperational_transferPoint">
                                                {this.props.initialValues.transferPoint}
                                            </span>
                                        </div>
                                    </div>
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-4">
                                            <label className="ep-label">{resourceProvider.read('movementTypeTransferOperational')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointOperational_movementType">
                                                {this.props.initialValues.movementType}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-4">
                                            <label className="ep-label">{resourceProvider.read('sourceNodeTransferOperational')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointOperational_nodeOrigin">
                                                {this.props.initialValues.sourceNode}
                                            </span>
                                        </div>
                                    </div>
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-4">
                                            <label className="ep-label">{resourceProvider.read('destinationNodeTransferOperational')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointOperational_nodeDestination">
                                                {this.props.initialValues.destinationNode}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-4">
                                            <label className="ep-label">{resourceProvider.read('sourceNodeType')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointOperational_nodeOriginType">
                                                {this.props.initialValues.sourceNodeType}
                                            </span>
                                        </div>
                                    </div>
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-4">
                                            <label className="ep-label">{resourceProvider.read('destinationNodeType')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointOperational_nodeDestinationType">
                                                {this.props.initialValues.destinationNodeType}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-12">
                                        <div className="ep-control-group m-b-4">
                                            <label className="ep-label">{resourceProvider.read('sourceProductTransferOperational')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointOperational_product">
                                                {this.props.initialValues.sourceProduct}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-12">
                                        <div className="ep-control-group m-b-4">
                                            <label className="ep-label">{resourceProvider.read('sourceProductType')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointOperational_productType">
                                                {this.props.initialValues.sourceProductType}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-12">
                                        <div className="ep-control-group">
                                            <label id="lbl_transferPointOperational_notes" className="ep-label">{resourceProvider.read('note')}</label>
                                            <Field id="txtarea_transferPointOperational_notes" component={inputTextarea}
                                                placeholder={resourceProvider.read('note')} name="notes"
                                                validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                                    length({ max: 1000, msg: resourceProvider.read('shortDescriptionLengthValidation') })]} />
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </section>
                    <ModalFooter config={footerConfigService.getCommonConfig('deleteOperative',
                        { acceptText: 'acceptTransferOperational' })} />
                </form>
            </>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.deleteSuccess !== this.props.deleteSuccess) {
            this.props.refreshGrid();
            this.props.closeModal();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        initialValues: state.transferPointsOperational.transferPoint,
        deleteSuccess: state.transferPointsOperational.deleteSuccess
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        requestUpdateTransferPoint: values => {
            dispatch(requestUpdateTransferPoint(values, 'DELETE'));
        },
        refreshGrid: () => {
            dispatch(refreshGrid('transferPointsOperational'));
        }
    };
};

const DeleteTransferPointOperationalForm = reduxForm({
    form: 'deleteTransferPointOperational',
    enableReinitialize: true
})(DeleteTransferPointOperational);


/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(DeleteTransferPointOperationalForm);
