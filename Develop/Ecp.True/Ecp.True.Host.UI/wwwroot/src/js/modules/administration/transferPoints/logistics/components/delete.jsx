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

class DeleteTransferPointLogistic extends React.Component {
    constructor() {
        super();
        this.deleteTransferPoint = this.deleteTransferPoint.bind(this);
    }

    deleteTransferPoint(values) {
        values.isDeleted = true;
        values.rowVersion = this.props.initialValues.rowVersion;
        this.props.requestUpdateTransferPoint(values, 'DELETE');
    }

    render() {
        return (
            <>
                <form id="frm_transferPointLogistics_delete" className="ep-form" onSubmit={this.props.handleSubmit(this.deleteTransferPoint)}>
                    <section className="ep-modal__content" id="sec_modal_group">
                        <div className="ep-content__body">
                            <form id="frm_transferPointLogistics_delete" className="ep-form">
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-4">
                                            <span className="ep-data fw-500" id="lbl_transferPointLogistics_name">
                                                {resourceProvider.read('transferPointOperationalDeleteHeading')}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-4">
                                            <label className="ep-label">{resourceProvider.read('transferPoint')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointLogistics_transferPoint">
                                                {this.props.initialValues.transferPoint}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-4">
                                            <label className="ep-label">{resourceProvider.read('logisticSourceCenter')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointLogistics_logisticSourceCenter">
                                                {this.props.initialValues.logisticSourceCenter}
                                            </span>
                                        </div>
                                    </div>
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-4">
                                            <label className="ep-label">{resourceProvider.read('logisticDestinationCenter')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointLogistics_logisticDestinationCenter">
                                                {this.props.initialValues.logisticDestinationCenter}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-4">
                                            <label className="ep-label">{resourceProvider.read('sourceProductTransferOperational')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointLogistics_sourceProduct">
                                                {this.props.initialValues.sourceProduct}
                                            </span>
                                        </div>
                                    </div>
                                    <div className="col-md-6">
                                        <div className="ep-control-group m-b-4">
                                            <label className="ep-label">{resourceProvider.read('destinationProductTransferPoints')}</label>
                                            <span className="ep-data fw-500" id="lbl_transferPointLogistics_destinationProduct">
                                                {this.props.initialValues.destinationProduct}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-12">
                                        <div className="ep-control-group">
                                            <label id="lbl_transferPointLogistics_notes" className="ep-label" htmlFor="txtarea_transferPointLogistics_notes">{resourceProvider.read('note')}</label>
                                            <Field id="txtarea_transferPointLogistics_notes" component={inputTextarea}
                                                placeholder={resourceProvider.read('note')} name="notes"
                                                validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                                    length({ max: 1000, msg: resourceProvider.read('transferPointsLogisticsMaximumNotesLength') })]} />
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </section>
                    <ModalFooter config={footerConfigService.getCommonConfig('transferPointLogisticsDelete',
                        { acceptText: 'acceptTransferOperational' })} />
                </form>
            </>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.deleteToggler !== this.props.deleteToggler) {
            this.props.refreshGrid();
            this.props.closeModal();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        mode: state.modal.modalKey,
        initialValues: state.transferPointsLogistics.transferPoint,
        deleteToggler: state.transferPointsLogistics.deleteToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        requestUpdateTransferPoint: (transferPoint, method) => {
            dispatch(requestUpdateTransferPoint(transferPoint, method));
        },
        refreshGrid: () => {
            dispatch(refreshGrid('transferPointsLogistics'));
        }
    };
};

const DeleteTransferPointLogisticForm = reduxForm({
    form: 'deleteTransferPointLogistics',
    enableReinitialize: true
})(DeleteTransferPointLogistic);


/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(DeleteTransferPointLogisticForm);
