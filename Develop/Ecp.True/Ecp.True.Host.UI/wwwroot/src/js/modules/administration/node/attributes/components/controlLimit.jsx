import React from 'react';
import { connect } from 'react-redux';
import { reduxForm, Field } from 'redux-form';
import { required, numericality } from 'redux-form-validators';
import { inputDecimal } from '../../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { getNode, updateNode } from './../actions';
import { constants } from '../../../../../common/services/constants';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';

class ControlLimit extends React.Component {
    constructor() {
        super();
        this.updateNode = this.updateNode.bind(this);
    }

    updateNode(values) {
        const node = {
            nodeId: values.nodeId,
            controlLimit: values.controlLimit,
            acceptableBalancePercentage: Number(values.acceptableBalancePercentage),
            isActive: false,
            name: values.name,
            sendToSap: false,
            rowVersion: values.rowVersion
        };
        this.props.updateNode(node);
    }

    render() {
        return (
            <form id="frm_nodeAttributes_controlLimit" className="ep-form" onSubmit={this.props.handleSubmit(this.updateNode)}>
                <section className="ep-modal__content">
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label">{resourceProvider.read('name')}</label>
                                <span className="ep-data" id="lbl_node_name">
                                    {this.props.initialValues.name}
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
                        <div className="col-md-6">
                            <div className="ep-control-group m-b-0">
                                <label className="ep-label" htmlFor="txt_percentage_acceptableBalancePercentage">{resourceProvider.read('acceptableBalance')}</label>
                                <Field id="txt_percentage_acceptableBalancePercentage" type={constants.InputType.Percentage}
                                    component={inputDecimal} name="acceptableBalancePercentage"
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                        numericality({
                                            greaterThanOrEqualTo: 0.01,
                                            lessThanOrEqualTo: constants.PercentageRange.Max,
                                            msg: {
                                                greaterThanOrEqualTo: resourceProvider.read('invalidRange'),
                                                lessThanOrEqualTo: resourceProvider.read('percentageValidationMessage')
                                            }
                                        })]}
                                    hasAddOn={true} addOnClass="fas fa-percentage" />
                            </div>
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('nodeAttributes_controlLimit')} />
            </form>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.nodeToggler !== this.props.nodeToggler) {
            this.props.getNode(this.props.initialValues.nodeId);
            this.props.closeModal();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        initialValues: state.node.attributes.node,
        nodeToggler: state.node.attributes.nodeToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        updateNode: node => {
            dispatch(updateNode(node));
        },
        getNode: nodeId => {
            dispatch(getNode(nodeId));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(reduxForm({ form: 'nodeControlLimit' })(ControlLimit));
