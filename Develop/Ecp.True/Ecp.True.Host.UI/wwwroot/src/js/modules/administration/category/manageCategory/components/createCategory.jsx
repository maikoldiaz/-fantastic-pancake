import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import { required, length, format } from 'redux-form-validators';
import { inputTextbox, inputToggler, inputTextarea } from './../../../../../common/components/formControl/formControl.jsx';
import { createCategory, updateCategory } from '../actions';
import { resourceProvider } from './../../../../../common/services/resourceProvider';
import { constants } from './../../../../../common/services/constants';
import { asyncValidate } from './../asyncValidate';
import { utilities } from './../../../../../common/services/utilities';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';
import { refreshGrid } from '../../../../../common/components/grid/actions.js';

class CreateCategory extends React.Component {
    constructor() {
        super();
        this.saveCategory = this.saveCategory.bind(this);
    }

    saveCategory(category) {
        if (this.props.mode !== constants.Modes.Create) {
            this.props.updateCategory(Object.assign({}, category, { name: category.name.trim() }));
        } else {
            this.props.createCategory(Object.assign({}, category, { name: category.name.trim() }));
        }
    }

    render() {
        return (
            <form id={`frm_${this.props.mode}_category`} className="ep-form" onSubmit={this.props.handleSubmit(this.saveCategory)}>
                <section className="ep-modal__content">
                    <div className="ep-control-group">
                        <label className="ep-label" htmlFor="txt_category_name">{resourceProvider.read('name')}</label>
                        <Field type="text" id="txt_category_name" component={inputTextbox}
                            placeholder={resourceProvider.read('category')} name="name"
                            validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                length({ max: 150, msg: resourceProvider.read('nameLengthValidation') }),
                                format({
                                    with: constants.FieldValidation.Category,
                                    message: resourceProvider.read('nameFormatValidation')
                                })]} />
                    </div>
                    <div className="ep-control-group">
                        <label className="ep-label" htmlFor="txtarea_category_description">{resourceProvider.read('description')}</label>
                        <Field component={inputTextarea} name="description" id="txtarea_category_description"
                            placeholder={resourceProvider.read('description')}
                            validate={[length({ max: 1000, msg: resourceProvider.read('descriptionLengthValidation') })]} />
                    </div>
                    <div className="d-flex">
                        <div className="ep-control-group m-b-0 m-r-6">
                            <label className="ep-label">{resourceProvider.read('active')}</label>
                            <Field component={inputToggler} name="isActive" id="tog_category_active" />
                        </div>
                        <div className="ep-control-group m-b-0">
                            <label className="ep-label">{resourceProvider.read('grouper')}</label>
                            <Field component={inputToggler} name="isGrouper" id="tog_category_grouper" />
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('category')} />
            </form>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.refreshToggler !== this.props.refreshToggler) {
            this.props.refreshGrid();
            this.props.closeModal();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        initialValues: ownProps.mode === constants.Modes.Create ? { isActive: true, isGrouper: true } : state.category.manageCategory.category,
        refreshToggler: state.category.manageCategory.refreshToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        createCategory: category => {
            dispatch(createCategory(category));
        },
        updateCategory: category => {
            dispatch(updateCategory(category));
        },
        refreshGrid: () => {
            dispatch(refreshGrid('categories'));
        }
    };
};

const CreateCategoryForm = reduxForm({
    form: 'createCategory',
    asyncValidate,
    enableReinitialize: true,
    shouldAsyncValidate: utilities.shouldAsyncValidate
})(CreateCategory);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(CreateCategoryForm);
