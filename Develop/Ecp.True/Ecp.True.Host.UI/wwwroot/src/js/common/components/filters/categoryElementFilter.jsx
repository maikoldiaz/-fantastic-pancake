import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, FieldArray, initialize } from 'redux-form';
import Flyout from '../../components/flyout/flyout.jsx';
import {
    closeFlyout, onCategorySelection, saveCategoryElementFilter, getCategoryElements, categoryElementFilterResetFields, initCategoryElementFilter
} from '../../actions.js';
import { inputSelect } from '../../components/formControl/formControl.jsx';
import { utilities } from '../../services/utilities';
import { resourceProvider } from '../../services/resourceProvider';
import FlyoutFooter from '../footer/flyoutFooter.jsx';
import { footerConfigService } from '../../services/footerConfigService';
import { applyFilter, resetFilter } from '../grid/actions';

const categoryElements = props => (
    <section className="ep-filters">
        {props.fields.map((item, index) => (
            <article key={index} className="ep-filter">
                <div className="ep-filter__count">{index + 1}</div>
                <div className="ep-filter__body">
                    <div className="ep-control-group m-b-3">
                        <label className="ep-label" htmlFor={`dd_${props.parentName}_category_sel`}>{resourceProvider.read('category')}</label>
                        <Field component={inputSelect} options={props.categoryOptions} id={`dd_${props.parentName}_category`}
                            getOptionLabel={x => x.name} getOptionValue={x => x.categoryId} inputId={`dd_${props.parentName}_category_sel`}
                            onChange={selectedItem => props.onCategorySelection(selectedItem, index)}
                            placeholder={resourceProvider.read('category')} name={`${item}.category`} />
                    </div>
                    <div className="ep-control-group m-b-0">
                        <label className="ep-label" htmlFor={`dd_${props.parentName}_element_sel`}>{resourceProvider.read('element')}</label>
                        <Field component={inputSelect} getOptionLabel={x => x.name} getOptionValue={x => x.elementId} id={`dd_${props.parentName}_element`}
                            options={props.categoryElementOptions.filter(x => !utilities.isNullOrUndefined(props.selectedCategories[index])
                                && x.categoryId === props.selectedCategories[index].categoryId)} inputId={`dd_${props.parentName}_element_sel`}
                            placeholder={resourceProvider.read('element')} name={`${item}.element`} />
                    </div>
                    {index > 0 &&
                        <label className="ep-filter__type">
                            <input className="ep-filter__type-inp" type="checkbox" checked={!props.fields.get(index).or} />
                            <span className="ep-filter__type-y" id={`lbl_${props.parentName}_and`}>Y</span>
                            <span className="ep-filter__type-o" id={`lbl_${props.parentName}_or`}>O</span>
                        </label>
                    }
                </div>
                {(props.showTrash || index > 0) &&
                    <button type="button" className="ep-filter__action" id={`btn_${props.parentName}_remove`} onClick={() => props.fields.remove(index)}>
                        <i className="fas fa-trash" />
                        <span className="sr-only">{resourceProvider.read('delete')}</span>
                    </button>}
            </article>
        ))}

        {props.fields.length < 3 && <div className="m-t-3 float-r">
            <button type="button" id={`btn_${props.parentName}_or`} className="ep-btn ep-btn--link" onClick={() => props.fields.push({ or: true })}>{resourceProvider.read('addOrFilter')}</button>
            <button type="button" id={`btn_${props.parentName}_and`} className="ep-btn ep-btn--link" onClick={() => props.fields.push({ or: false })}>{resourceProvider.read('addAndFilter')}</button>
        </div>}

    </section>
);

class CategoryElementFilterClass extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
    }

    onSubmit(formValues) {
        const category = formValues.categoryElements.filter(v => utilities.isNullOrWhitespace(v.element) || (v.element && v.element.elementId));
        const values = Object.assign({}, formValues, { categoryElements: category });

        this.props.saveCategoryElementFilter(values);
        this.props.closeFlyout(this.props.name);
    }

    render() {
        return (
            <Flyout name={this.props.name}>
                <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                    <header className="ep-flyout__header">
                        <button type="button" id={`btn_${this.props.name}_cancel`} className="ep-btn ep-btn--tr" onClick={() => this.props.closeFlyout(this.props.name)}>
                            {resourceProvider.read('cancel')}</button>
                        <h1 className="ep-flyout__title">{resourceProvider.read('searchCriteria')}</h1>
                        <button type="button" id={`btn_${this.props.name}_reset`} className="ep-btn ep-btn--tr"
                            onClick={() => this.props.resetForm(this.props.defaultValues)}>{resourceProvider.read('reset')}</button>
                    </header>
                    <section className="ep-flyout__body">
                        <FieldArray name="categoryElements" parentName={this.props.name} component={categoryElements}
                            categoryOptions={this.props.categories} categoryElementOptions={this.props.categoryElements}
                            selectedCategories={this.props.selectedCategories}
                            onCategorySelection={this.props.onCategorySelection} showTrash={this.props.showTrash} />
                    </section>
                    <FlyoutFooter config={footerConfigService.getFlyoutConfig(this.props.name, 'applyFilters')} />
                </form>
            </Flyout>
        );
    }

    componentDidMount() {
        this.props.initCategoryElementFilter();
        this.props.getCategoryElements();
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    const filterState = state.categoryElementFilter[ownProps.name];
    return {
        form: ownProps.name,
        defaultValues: state.categoryElementFilter.defaultValues,
        initialValues: filterState ? filterState.values : state.categoryElementFilter.defaultValues,
        categories: state.shared.categories,
        categoryElements: state.shared.categoryElements,
        selectedCategories: filterState ? filterState.selectedCategories : [],
        showTrash: ownProps.showTrash ? ownProps.showTrash : false
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        closeFlyout: flyoutName => {
            dispatch(closeFlyout(flyoutName));
        },
        initCategoryElementFilter: () => {
            dispatch(initCategoryElementFilter(ownProps.name));
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        saveCategoryElementFilter: values => {
            dispatch(saveCategoryElementFilter(ownProps.name, values));
            if (values.categoryElements.filter(x => !utilities.isNullOrUndefined(x.element)).length > 0) {
                dispatch(applyFilter(ownProps.name, values));
            } else {
                dispatch(resetFilter(ownProps.name));
            }
        },
        onCategorySelection: (selectedItem, index) => {
            dispatch(onCategorySelection(selectedItem, index, ownProps.name));
        },
        resetForm: values => {
            dispatch(categoryElementFilterResetFields(ownProps.name));
            dispatch(initialize(ownProps.name, values));
        }
    };
};

const CategoryElementFilterForm = reduxForm({})(CategoryElementFilterClass);
export const CategoryElementFilterComponent = connect(mapStateToProps, mapDispatchToProps)(CategoryElementFilterForm);
