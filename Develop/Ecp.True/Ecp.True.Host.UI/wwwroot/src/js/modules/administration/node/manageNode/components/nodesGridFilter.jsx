import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, initialize } from 'redux-form';
import { required } from 'redux-form-validators';
import { inputSelect } from '../../../../../common/components/formControl/formControl.jsx';
import Flyout from '../../../../../common/components/flyout/flyout.jsx';
import { closeFlyout, getCategoryElements } from '../../../../../common/actions';
import { saveNodesGridFilter, changeNodesFilterPersistance, resetNodesFilters } from './../actions';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { constants } from './../../../../../common/services/constants';
import { utilities } from '../../../../../common/services/utilities';
import FlyoutFooter from '../../../../../common/components/footer/flyoutFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';

class NodesGridFilter extends React.Component {
    constructor() {
        super();
        this.onCancel = this.onCancel.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
    }

    onCancel() {
        this.props.resetForm(this.props.initialValues);
        this.props.closeFlyout(this.props.name);
    }

    onSubmit(formValues) {
        const values = Object.assign({}, formValues, {
            nodeTypes: utilities.isNullOrWhitespace(formValues.nodeTypes) ? [] : formValues.nodeTypes,
            operators: utilities.isNullOrWhitespace(formValues.operators) ? [] : formValues.operators
        });
        this.props.saveNodesGridFilter(values);
        this.props.closeFlyout(this.props.name);
    }

    render() {
        const segment = this.props.groupedCategoryElements[constants.Category.Segment];
        const nodeTypes = this.props.groupedCategoryElements[constants.Category.NodeType];
        const operators = this.props.groupedCategoryElements[constants.Category.Operator];
        return (
            <Flyout name={this.props.name}>
                <form id="frm_nodeGridFilter" className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                    <header className="ep-flyout__header">
                        <button id="btn_nodeGridFilter_cancel" type="button" className="ep-btn ep-btn--tr" onClick={this.onCancel}>{resourceProvider.read('cancel')}</button>
                        <h1 className="ep-flyout__title">{resourceProvider.read('searchCriteria')}</h1>
                        <button id="btn_nodeGridFilter_reset" type="button" onClick={() => this.props.resetForm(this.props.defaultValues)}
                            className="ep-btn ep-btn--tr">{resourceProvider.read('clean')}</button>
                    </header>
                    <section className="ep-flyout__body">
                        <section className="ep-filter ep-filter--br">
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label" id="lbl_nodeGridFilter_segment" htmlFor="dd_nodeGridFilter_segment_sel">{resourceProvider.read('segment')}</label>
                                <Field id="dd_nodeGridFilter_segment" component={inputSelect} name="segment" inputId="dd_nodeGridFilter_segment_sel"
                                    options={segment} getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label" id="lbl_nodeGridFilter_nodeType" htmlFor="dd_nodeGridFilter_nodeType_sel">{resourceProvider.read('type')}</label>
                                <Field id="dd_nodeGridFilter_nodeType" component={inputSelect} name="nodeTypes" options={nodeTypes} inputId="dd_nodeGridFilter_nodeType_sel"
                                    getOptionLabel={x => x.name} removeSelected={false} getOptionValue={x => x.elementId} isMulti hideSelectedOptions={false} />
                            </div>
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label" id="lbl_nodeGridFilter_operator" htmlFor="dd_nodeGridFilter_operator_sel">{resourceProvider.read('operator')}</label>
                                <Field id="dd_nodeGridFilter_operator" component={inputSelect} name="operators" options={operators} inputId="dd_nodeGridFilter_operator_sel"
                                    getOptionLabel={x => x.name} removeSelected={false} getOptionValue={x => x.elementId} isMulti hideSelectedOptions={false} />
                            </div>
                        </section>
                    </section>
                    <FlyoutFooter config={footerConfigService.getFlyoutConfig('nodeGridFilter', 'applyFilters')} />
                </form>
            </Flyout>
        );
    }

    componentDidMount() {
        if (this.props.persist) {
            this.props.removePersistance();
        } else {
            this.props.resetFilters();
            this.props.resetForm(this.props.defaultValues);
        }
        this.props.getCategoryElements();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    const filterState = state.node.manageNode;
    return {
        groupedCategoryElements: state.shared.groupedCategoryElements,
        defaultValues: filterState.defaultFilterValues,
        initialValues: filterState.filterValues ? filterState.filterValues : filterState.defaultFilterValues,
        persist: state.node.manageNode.persist
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        closeFlyout: () => {
            dispatch(closeFlyout(ownProps.name));
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        saveNodesGridFilter: filterValues => {
            dispatch(saveNodesGridFilter(filterValues));
            dispatch(closeFlyout(ownProps.name));
        },
        resetForm: values => {
            dispatch(initialize('nodesGridFilter', values));
        },
        removePersistance: () => {
            dispatch(changeNodesFilterPersistance(false));
        },
        resetFilters: () => {
            dispatch(resetNodesFilters());
        }
    };
};

const nodesGridFilterForm = reduxForm({ form: 'nodesGridFilter' })(NodesGridFilter);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(nodesGridFilterForm);
