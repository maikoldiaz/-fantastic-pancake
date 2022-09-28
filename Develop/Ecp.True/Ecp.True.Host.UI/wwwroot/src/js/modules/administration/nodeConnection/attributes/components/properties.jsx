import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import { numericality, required } from 'redux-form-validators';
import { inputDecimal } from '../../../../../common/components/formControl/formControl.jsx';
import { updateProduct } from '../actions';
import { constants } from './../../../../../common/services/constants';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { utilities } from '../../../../../common/services/utilities';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';
import { refreshGrid } from '../../../../../common/components/grid/actions';

export class Properties extends React.Component {
    constructor() {
        super();
        this.updateProduct = this.updateProduct.bind(this);
    }
    updateProduct(values) {
        const product = Object.assign({}, values, {
            owners: null,
            product: null,
            rule: null
        });
        this.props.updateProduct(product);
    }

    render() {
        return (
            <form id="frm_connAttributes_info" className="ep-form" onSubmit={this.props.handleSubmit(this.updateProduct)}>
                <section className="ep-modal__content">
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label" htmlFor="txt_decimal_priority">{resourceProvider.read('priority')}</label>
                                <Field id="txt_decimal_priority" type={constants.InputType.Percentage} isInteger={true}
                                    min={constants.PercentageRange.Min} max="1000000" step="1"
                                    component={inputDecimal} name="priority"
                                    parse={value => value === '' ? '' : Number(value)}
                                    validate={[
                                        numericality({
                                            greaterThanOrEqualTo: constants.PercentageRange.Min,
                                            lessThanOrEqualTo: 1000000,
                                            msg: {
                                                greaterThanOrEqualTo: resourceProvider.read('invalidRange'),
                                                lessThanOrEqualTo: resourceProvider.read('invalidRange'),
                                                notANumber: resourceProvider.read('required')
                                            },
                                            allowBlank: false
                                        }),
                                        required({ msg: { presence: resourceProvider.read('required') } })

                                    ]} />
                            </div>
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('connAttributes_info')} />
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
        initialValues: state.nodeConnection.attributes.connectionProduct,
        productToggler: state.nodeConnection.attributes.productToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        updateProduct: product => {
            dispatch(updateProduct(product));
        },
        refreshGrid: () => {
            dispatch(refreshGrid('connectionProducts'));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(reduxForm({ form: 'connectionProductsProperties', enableReinitialize: true })(Properties));
