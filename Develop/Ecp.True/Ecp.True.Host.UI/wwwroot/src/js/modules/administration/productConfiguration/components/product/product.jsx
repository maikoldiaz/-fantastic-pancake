import React from 'react';
import { connect } from 'react-redux';
import { reduxForm, FormSection } from 'redux-form';
import { utilities } from '../../../../../common/services/utilities';
import ProductSection from './productSection.jsx';
import { closeModal } from '../../../../../common/actions';
import { constants } from '../../../../../common/services/constants';
import { saveProduct, initProduct, updateProduct } from '../../actions';
import { dataService } from '../../dataservice';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';

export class Product extends React.Component {
    constructor() {
        super();
        this.saveProduct = this.saveProduct.bind(this);
    }

    saveProduct(values) {
        if (this.props.mode === constants.Modes.Create) {
            this.props.saveProduct(dataService.buildProductObject(values, this.props.mode, this.props.initialValues));
        } else {
            this.props.updateProduct(dataService.buildProductObject(values, this.props.mode, this.props.initialValues), this.props.initialValues.product.productSapId);
        }
    }

    render() {
        return (
            <form id={`frm_product_${this.props.mode}`} className="ep-form" onSubmit={this.props.handleSubmit(this.saveProduct)}>
                <section className="ep-modal__content">
                    <div className="row">
                        <FormSection name="product">
                            <ProductSection name="productSection" mode={this.props.mode} />
                        </FormSection>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('product')} />
            </form>
        );
    }

    componentDidMount() {
        if (this.props.mode === constants.Modes.Create) {
            this.props.initTypes();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        initialValues: state.products.initialValues
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        initTypes: () => {
            dispatch(initProduct(dataService.buildResetValues()));
        },
        saveProduct: values => {
            dispatch(saveProduct(values));
            dispatch(closeModal('Product'));
        },
        updateProduct: (values, productSapId) => {
            dispatch(updateProduct(values, productSapId));
            dispatch(closeModal('Product'));
        }
    };
};

/* istanbul ignore next */
const productForm = reduxForm({
    form: 'product',
    enableReinitialize: true,
    shouldAsyncValidate: utilities.shouldAsyncValidate
})(Product);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(productForm);
