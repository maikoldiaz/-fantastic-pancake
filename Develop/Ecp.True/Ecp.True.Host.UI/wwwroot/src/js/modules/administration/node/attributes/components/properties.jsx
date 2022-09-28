import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import { required } from 'redux-form-validators';
import { updateProduct } from './../actions';
import { getCategoryElements } from '../../../../../common/actions';
import { inputSelect } from '../../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { constants } from './../../../../../common/services/constants';
import { utilities } from './../../../../../common/services/utilities';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';
import { refreshGrid } from '../../../../../common/components/grid/actions';

export class Properties extends React.Component {
    constructor() {
        super();
        this.updateProductRules = this.updateProductRules.bind(this);
    }
    updateProductRules(values) {
        const product = Object.assign({}, values, {
            owners: null,
            product: null,
            nodeStorageLocation: null,
            ownershipRule: null,
            ownershipRuleId: values.ownershipRule.elementId
        });
        this.props.updateProductRules(product);
    }

    buildRuleName(rule) {
        return !utilities.isNullOrUndefined(rule) ? `${rule.name}` : '';
    }

    render() {
        const ownershipRule = this.props.groupedCategoryElements[constants.Category.OwnershipRules];
        return (
            <form id="frm_nodeAttributes_functions" className="ep-form" onSubmit={this.props.handleSubmit(this.updateProductRules)}>
                <section className="ep-modal__content">
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label">{resourceProvider.read('ilFunctionLbl')}</label>
                                <span className="ep-data">{this.buildRuleName(this.props.initialValues.ownershipRule)}</span>
                            </div>
                        </div>
                    </div>
                    <div className="row m-t-4">
                        <div className="col-md-6">
                            <div className="ep-control-group m-b-0">
                                <label className="ep-label" htmlFor="dd_nodeAttributes_to_sel">{resourceProvider.read('newOwnershipStrategy')}</label>
                                <Field id="dd_nodeAttributes_to" component={inputSelect} options={ownershipRule} name="ownershipRule"
                                    getOptionLabel={x => x.name} getOptionValue={x => x.elementId} inputId="dd_nodeAttributes_to_sel"
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('nodeAttributes_properties')} />
            </form>
        );
    }

    componentDidMount() {
        this.props.getCategoryElements();
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
        groupedCategoryElements: state.shared.groupedCategoryElements,
        initialValues: state.node.attributes.nodeProduct,
        productToggler: state.node.attributes.productToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        updateProductRules: productRules => {
            dispatch(updateProduct(productRules));
        },
        refreshGrid: () => {
            dispatch(refreshGrid('nodeProducts'));
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(reduxForm({ form: 'nodeProductsProperties' })(Properties));
