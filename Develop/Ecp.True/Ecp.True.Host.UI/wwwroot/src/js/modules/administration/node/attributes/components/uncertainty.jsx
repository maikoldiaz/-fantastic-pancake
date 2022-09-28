import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import { required, numericality } from 'redux-form-validators';
import { inputDecimal } from '../../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { updateProduct } from './../actions';
import { utilities } from '../../../../../common/services/utilities';
import { constants } from '../../../../../common/services/constants';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';
import { refreshGrid } from '../../../../../common/components/grid/actions.js';

class Uncertainty extends React.Component {
    constructor() {
        super();
        this.updateProduct = this.updateProduct.bind(this);
    }

    updateProduct(values) {
        const product = Object.assign({}, values, {
            owners: null,
            product: null,
            nodeStorageLocation: null
        });
        this.props.updateProduct(product);
    }

    render() {
        return (
            <form id="frm_nodeProducts_uncertainty" className="ep-form" onSubmit={this.props.handleSubmit(this.updateProduct)}>
                <section className="ep-modal__content">
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label">{resourceProvider.read('storageLocation')}</label>
                                <span className="ep-data" id="lbl_nodeProducts_product_name">{utilities.getValue(this.props.initialValues, 'nodeStorageLocation.name')}</span>
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label">{resourceProvider.read('product')}</label>
                                <span className="ep-data" id="lbl_nodeProducts_product_name">{utilities.getValue(this.props.initialValues, 'product.name')}</span>
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group m-b-0">
                                <label className="ep-label" htmlFor="txt_percentage_uncertaintyPercentage">{resourceProvider.read('uncertainty')}</label>
                                <Field id={`txt_percentage_uncertaintyPercentage`}
                                    type={constants.InputType.Percentage}
                                    component={inputDecimal} name="uncertaintyPercentage"
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                        numericality({
                                            greaterThanOrEqualTo: constants.PercentageRange.greaterThanOrEqualTo,
                                            lessThanOrEqualTo: constants.PercentageRange.Max, msg: {
                                                greaterThanOrEqualTo: resourceProvider.read('invalidRange'),
                                                lessThanOrEqualTo: resourceProvider.read('percentageValidationMessage')
                                            }
                                        })]}
                                    hasAddOn={true} addOnClass="fas fa-percentage" />
                            </div>
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('nodeProducts_uncertainty')} />
            </form>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.productToggler !== this.props.productToggler) {
            this.props.refreshGrid();
            this.props.closeModal();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        initialValues: state.node.attributes.nodeProduct,
        productToggler: state.node.attributes.productToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        updateProduct: product => {
            dispatch(updateProduct(product));
        },
        refreshGrid: () => {
            dispatch(refreshGrid('nodeProducts'));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(reduxForm({ form: 'editNodeProductUncertainty' })(Uncertainty));
