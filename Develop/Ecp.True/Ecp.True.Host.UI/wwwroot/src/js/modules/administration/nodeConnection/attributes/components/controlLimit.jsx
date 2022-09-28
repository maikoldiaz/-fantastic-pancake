import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, formValueSelector } from 'redux-form';
import { required, numericality } from 'redux-form-validators';
import { inputCheckbox, inputSelect, inputDecimal } from '../../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { utilities } from '../../../../../common/services/utilities';
import { getConnection, updateConnection } from './../actions';
import classNames from 'classnames/bind';
import { constants } from '../../../../../common/services/constants';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';

class ControlLimit extends React.Component {
    constructor() {
        super();
        this.updateConnection = this.updateConnection.bind(this);
    }

    updateConnection(values) {
        const connection = Object.assign({}, values, {
            sourceNode: null,
            destinationNode: null,
            algorithm: null,
            algorithmId: values.isTransfer === true ? values.algorithm.algorithmId : null
        });
        this.props.updateConnection(connection);
    }

    render() {
        return (
            <form id="frm_connAttributes_controlLimit" className="ep-form" onSubmit={this.props.handleSubmit(this.updateConnection)}>
                <section className="ep-modal__content">
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label">{resourceProvider.read('connection')}</label>
                                <span className="ep-data" id="lbl_connAttributes_name">
                                    {`${utilities.getValue(this.props.initialValues, 'sourceNode.name')}-${utilities.getValue(this.props.initialValues, 'destinationNode.name')}`}
                                </span>
                            </div>
                        </div>
                    </div>
                    <div className="row m-t-4">
                        <div className="col-md-6">
                            <div className="ep-control-group m-b-0">
                                <label className="ep-label" htmlFor="txt_decimal_controlLimit">{resourceProvider.read('controlLimit')}</label>
                                <Field id="txt_decimal_controlLimit"
                                    type={constants.InputType.Decimal}
                                    min={constants.DecimalRange.Min} component={inputDecimal} name="controlLimit"
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                        numericality({
                                            greaterThanOrEqualTo: constants.DecimalRange.greaterThanOrEqualTo,
                                            lessThanOrEqualTo: constants.DecimalRange.Max,
                                            msg: {
                                                greaterThanOrEqualTo: resourceProvider.read('invalidRange'),
                                                lessThanOrEqualTo: resourceProvider.read('decimalValidationMessage')
                                            },
                                            allowBlank: false
                                        })]} />
                            </div>
                        </div>
                    </div>
                    <div className="row m-t-4">
                        <div className="col-md-6">
                            <div className="ep-control-group m-b-0">
                                {this.props.controlLimitSource === 'graphicconfigurationnetwork' &&
                                    <label className="ep-label" htmlFor="dd_connAttributes_algorithmId_sel">{resourceProvider.read('predictiveAnalyticalModel')}</label>
                                }
                                {this.props.controlLimitSource === 'connectionattributes' &&
                                    <label className="ep-label" htmlFor="dd_connAttributes_algorithmId_sel">{resourceProvider.read('analyticalModelTitle')}</label>
                                }
                                <Field id="dd_connAttributes_algorithmId"
                                    component={inputSelect} name="algorithm" inputId="dd_connAttributes_algorithmId_sel"
                                    validate={this.props.toggleTransfer ? [required({ msg: { presence: resourceProvider.read('required') } })] : null}
                                    isDisabled={!this.props.toggleTransfer} placeholder={resourceProvider.read('select')}
                                    getOptionLabel={x => x.modelName} getOptionValue={x => x.algorithmId} options={this.props.algorithms} />
                            </div>
                        </div>
                    </div>
                    <div className="row m-t-4">
                        <div className={classNames('col-md-6', { ['hidden']: this.props.controlLimitSource === 'graphicconfigurationnetwork' })}>
                            <div className="ep-control-group m-b-0 d-flex">
                                <Field id="chk_connAttributes_isTransfer" type="checkbox"
                                    component={inputCheckbox}
                                    canUpdate={this.props.controlLimitSource !== 'graphicconfigurationnetwork'}
                                    isDisabled={this.props.controlLimitSource === 'graphicconfigurationnetwork'}
                                    defaultChecked={this.props.initialValues.isTransfer} name="isTransfer" />
                                <label className="ep-label m-l-2 m-b-0">{resourceProvider.read('isTransfer')}</label>
                            </div>
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('connAttributes_controlLimit')} />
            </form>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.connectionToggler !== this.props.connectionToggler && this.props.controlLimitSource === 'connectionattributes') {
            this.props.getConnection(this.props.initialValues.nodeConnectionId);
            this.props.closeModal();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    const selector = formValueSelector('controlLimit');
    const toggleTransfer = selector(state, 'isTransfer');
    return {
        initialValues: state.nodeConnection.attributes.controlLimitSource === 'graphicconfigurationnetwork' ?
            Object.assign({}, state.nodeConnection.attributes.connection, { controlLimit: null, algorithmId: 0, algorithm: null }) : state.nodeConnection.attributes.connection,
        connectionToggler: state.nodeConnection.attributes.connectionToggler,
        algorithms: state.nodeConnection.attributes.algorithms,
        controlLimitSource: state.nodeConnection.attributes.controlLimitSource,
        toggleTransfer
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        updateConnection: connection => {
            dispatch(updateConnection(connection));
        },
        getConnection: connectionId => {
            dispatch(getConnection(connectionId));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(reduxForm({ form: 'controlLimit', enableReinitialize: true })(ControlLimit));
