import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, formValueSelector, change } from 'redux-form';
import { required, length, format, numericality } from 'redux-form-validators';
import { inputTextbox, inputSelect, inputToggler, inputTextarea, inputDecimal } from '../../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { constants } from './../../../../../common/services/constants';
import { getCategoryElements, getLogisticCenters } from '../../../../../common/actions.js';
import { clearProducts } from '../actions.js';
import { asyncValidate } from './../asyncValidate';

class CreateNode extends React.Component {
    constructor() {
        super();
        this.onLogisticCenterChange = this.onLogisticCenterChange.bind(this);
    }

    onLogisticCenterChange(logisticCenter) {
        this.props.clearProducts(logisticCenter);
    }

    render() {
        const segment = this.props.groupedCategoryElements[constants.Category.Segment];
        const nodeType = this.props.groupedCategoryElements[constants.Category.NodeType];
        const operator = this.props.groupedCategoryElements[constants.Category.Operator];
        const isSendToSAP = this.props.isSendToSAP;
        const logisticCenters = this.props.logisticCenters;
        const isCreateMode = this.props.mode === constants.Modes.Create;
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <form id={`frm_${this.props.mode}_node`} className="ep-form">
                        <div className="row">
                            <div className="col-md-6">
                                <div className="ep-control-group">
                                    <label id="lbl_createNode_name" className="ep-label" htmlFor="txt_createNode_name">{resourceProvider.read('name')}</label>
                                    <Field type="text" id="txt_createNode_name" component={inputTextbox}
                                        placeholder={resourceProvider.read('name')} name="name" validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                            length({ max: 150, msg: resourceProvider.read('shortNameLengthValidation') }),
                                            format({
                                                with: constants.FieldValidation.Node,
                                                message: resourceProvider.read('shortNameFormatValidation')
                                            })]} />
                                </div>
                            </div>
                            <div className="col-md-6">
                                <div className="ep-control-group">
                                    <label id="lbl_createNode_nodeType" className="ep-label" htmlFor="dd_createNode_type_sel">{resourceProvider.read('nodeType')}</label>
                                    <Field component={inputSelect} name="nodeType" id="dd_createNode_type" inputId="dd_createNode_type_sel"
                                        placeholder={resourceProvider.read('nodeType')} options={nodeType}
                                        getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} isDisabled={!isCreateMode} />
                                </div>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-md-6">
                                <div className="ep-control-group">
                                    <label id="lbl_createNode_operator" className="ep-label" htmlFor="dd_createNode_operator_sel">{resourceProvider.read('operator')}</label>
                                    <Field id="dd_createNode_operator" component={inputSelect} options={operator} inputId="dd_createNode_operator_sel"
                                        placeholder={resourceProvider.read('operator')} name="operator"
                                        getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} isDisabled={!isCreateMode} />
                                </div>
                            </div>
                            <div className="col-md-6">
                                <div className="ep-control-group">
                                    <label id="lbl_createNode_segment" className="ep-label" htmlFor="dd_createNode_segment_sel">{resourceProvider.read('segment')}</label>
                                    <Field component={inputSelect} name="segment" id="dd_createNode_segment" inputId="dd_createNode_segment_sel"
                                        placeholder={resourceProvider.read('segment')} options={segment}
                                        getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} isDisabled={!isCreateMode} />
                                </div>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-md-6">
                                <div className="ep-control-group">
                                    <label id="lbl_createNode_order" className="ep-label" htmlFor="txt_decimal_order">{resourceProvider.read('order')}</label>
                                    <Field id={`txt_decimal_order`} type="number" isInteger={true}
                                        min="1" max={constants.DecimalRange.MaxIntValue} step="1" component={inputDecimal} name="order"
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                            numericality({
                                                greaterThanOrEqualTo: 1,
                                                lessThanOrEqualTo: 2147483647,
                                                msg: {
                                                    greaterThanOrEqualTo: resourceProvider.read('invalidRange'),
                                                    lessThanOrEqualTo: resourceProvider.read('invalidRange')
                                                },
                                                allowBlank: false
                                            })]} />
                                </div>
                            </div>
                            <div className="col-md-6">
                                <div className="ep-control-group">
                                    <label id="lbl_createNode_logisticCenter" className="ep-label" htmlFor="dd_createNode_logisticCenter_sel">{resourceProvider.read('sapLogistic')}</label>
                                    <Field component={inputSelect} name="logisticCenter" id="dd_createNode_logisticCenter" options={logisticCenters} inputId="dd_createNode_logisticCenter_sel"
                                        onChange={this.onLogisticCenterChange}
                                        placeholder={resourceProvider.read('sapLogistic')} getOptionLabel={x => `${x.logisticCenterId} - ${x.name}`} getOptionValue={x => x.logisticCenterId}
                                        validate={isSendToSAP ? [required({ msg: { presence: resourceProvider.read('required') } })] : null} isDisabled={!isSendToSAP} />
                                </div>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-md-6">
                                <div className="ep-control-group">
                                    <label id="lbl_createNode_capacity" className="ep-label" htmlFor="txt_decimal_capacity">{resourceProvider.read('capacity')}
                                        <span className="d-inline-block m-x-1">-</span>
                                        <i className="d-inline-block fw-n fw-r text-fl-ucase">{resourceProvider.read('optional')}</i>
                                    </label>
                                    <Field component={inputDecimal} name="capacity" id="txt_decimal_capacity" type={constants.InputType.Decimal}
                                        min={constants.DecimalRange.Min} max={constants.DecimalRange.Max} step={0.01} />
                                </div>
                            </div>
                            <div className="col-md-6">
                                <div className="ep-control-group">
                                    <label id="lbl_createNode_unit" className="ep-label" htmlFor="dd_createNode_unit_sel">{resourceProvider.read('unit')}</label>
                                    <Field id="dd_createNode_unit" component={inputSelect} name="unit" inputId="dd_createNode_unit_sel"
                                        options={this.props.units} getOptionLabel={x => x.name} getOptionValue={x => x.elementId} />
                                </div>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-md-6">
                                <div className="d-flex">
                                    <div className="ep-control-group m-r-6">
                                        <span id="lbl_createNode_active" className="ep-label">{resourceProvider.read('active')}</span>
                                        <Field component={inputToggler} name="isActive" id="tog_createNode_active" readOnly={isCreateMode} />
                                    </div>
                                    <div className="ep-control-group">
                                        <span id="lbl_createNode_sendToSAP" className="ep-label">{resourceProvider.read('sendToSAP')}</span>
                                        <Field component={inputToggler} name="sendToSap" id="tog_createNode_sendToSAP" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="ep-control-group">
                            <label id="lbl_createNode_description" className="ep-label" htmlFor="txtarea_createNode_description">{resourceProvider.read('description')}</label>
                            <Field component={inputTextarea} name="description" id="txtarea_createNode_description"
                                placeholder={resourceProvider.read('description')} validate={[length({ max: 1000, msg: resourceProvider.read('shortDescriptionLengthValidation') })]} />
                        </div>
                    </form>
                </div>
            </section>
        );
    }
    componentDidUpdate() {
        if (!this.props.isSendToSAP) {
            this.props.resetField('logisticCenter', null);
            this.props.clearProducts(null);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    const selector = formValueSelector('createNode');
    const isSendToSAP = (selector(state, 'sendToSap') || state.node.manageNode.node.sendToSap);
    return {
        groupedCategoryElements: state.shared.groupedCategoryElements,
        logisticCenters: state.shared.logisticCenters,
        initialValues: state.node.manageNode.node,
        isSendToSAP,
        units: state.shared.groupedCategoryElements[constants.Category.UnitMeasurement]
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        resetField: (field, value) => {
            dispatch(change('createNode', field, value));
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        getLogisticCenters: () => {
            dispatch(getLogisticCenters());
        },
        clearProducts: logisticCenter => {
            dispatch(clearProducts(logisticCenter));
        }
    };
};

const createNodeForm = reduxForm({
    form: 'createNode',
    asyncValidate,
    asyncBlurFields: ['name'],
    enableReinitialize: true
})(CreateNode);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(createNodeForm);
