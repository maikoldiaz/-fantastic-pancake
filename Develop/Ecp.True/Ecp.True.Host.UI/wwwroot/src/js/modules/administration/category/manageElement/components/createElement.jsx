import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, change } from 'redux-form';
import { required, length } from 'redux-form-validators';
import { inputTextbox, inputToggler, inputTextarea, inputSelect } from '../../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { createElement, updateElement, refreshStatus, getIcons, openIconModal } from '../actions';
import { getCategories } from '../../manageCategory/actions';
import { constants } from './../../../../../common/services/constants';
import { utilities } from './../../../../../common/services/utilities';
import { asyncValidate } from './../asyncValidate';
import { resetColorPicker, resetIconPicker, showError, getCategoryElements } from '../../../../../common/actions';
import ColorPicker from './../../../../../common/components/colorPicker/colorPicker.jsx';
import IconModal from './iconModal.jsx';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';
import { refreshGrid } from '../../../../../common/components/grid/actions.js';

class CreateElement extends React.Component {
    constructor() {
        super();
        this.saveElement = this.saveElement.bind(this);
    }

    saveElement(values) {
        const element = Object.assign({}, values, {
            name: values.name.trim(),
            category: null,
            categoryId: values.category.categoryId,
            color: this.props.color,
            iconId: this.props.icon.id,
            icon: null
        });
        if (this.props.mode !== constants.Modes.Create) {
            if (element.iconId === null) {
                element.iconId = this.props.categoryElement.iconId;
            }
            element.elementId = this.props.categoryElement.elementId;
            this.props.updateElement(element);
        } else {
            this.props.createElement(element);
        }
    }

    render() {
        return (
            <>
                <form id={`frm_${this.props.mode}_element`} className="ep-form" onSubmit={this.props.handleSubmit(this.saveElement)}>
                    <section className="ep-modal__content">
                        <div className="row">
                            <div className="col-md-6">
                                <div className="ep-control-group">
                                    <label className="ep-label" htmlFor="dd_category_sel">{resourceProvider.read('category')}</label>
                                    <Field id="dd_category" component={inputSelect} name="category" isDisabled={this.props.mode === constants.Modes.Update}
                                        options={this.props.categories} getOptionLabel={x => x.name} getOptionValue={x => x.categoryId} inputId="dd_category_sel"
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                </div>
                            </div>
                            <div className="col-md-6">
                                <div className="ep-control-group">
                                    <label className="ep-label" htmlFor="txt_element_name">{resourceProvider.read('name')}</label>
                                    <Field type="text" id="txt_element_name" component={inputTextbox}
                                        placeholder={resourceProvider.read('element')} name="name"
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                            length({ max: 150, msg: resourceProvider.read('nameLengthValidation') })]} />
                                </div>
                            </div>
                        </div>

                        <div className="ep-control-group">
                            <label className="ep-label" htmlFor="txtarea_element_description">{resourceProvider.read('description')}</label>
                            <Field component={inputTextarea} id="txtarea_element_description" name="description" placeholder={resourceProvider.read('description')}
                                validate={[length({ max: 1000, msg: resourceProvider.read('descriptionLengthValidation') })]} />
                        </div>
                        <div className="row">
                            <div className="col-md-6">
                                <div className="ep-control-group">
                                    <label className="ep-label" htmlFor="txt_icon">{resourceProvider.read('icon')}</label>
                                    <div className="ep-control ep-control--addon">
                                        <div className="ep-control__inner ep-control__inner-input">
                                            <Field id="txt_icon" type="text" component={inputTextbox}
                                                placeholder={resourceProvider.read('icon')} name="icon" disabled={true} />
                                            <span className="ep-control__inner-addon" onClick={this.props.selectIconsModal}><i className="fas fa-search" /></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="col-md-6">
                                <div className="ep-control-group">
                                    <label className="ep-label">{resourceProvider.read('color')}</label>
                                    <ColorPicker id="color_picker" name="elementColor" color={this.props.color} />
                                </div>
                            </div>
                        </div>
                        <div className="ep-control-group  m-b-0">
                            <label className="ep-label">{resourceProvider.read('active')}</label>
                            <Field component={inputToggler} name="isActive" id="tog_element_active" />
                        </div>
                    </section>
                    <ModalFooter config={footerConfigService.getCommonConfig('element')} />
                </form>
                {this.props.iconModal &&
                    <IconModal {...this.props} />
                }
            </>
        );
    }

    componentDidMount() {
        this.props.getCategories();
        this.props.resetColorPicker();
        this.props.getIcons();
        this.props.refreshStatus();
        this.props.resetIconPicker();
    }

    componentDidUpdate(prevProps) {
        if ((prevProps.status !== this.props.status) && !utilities.isNullOrWhitespace(this.props.status.message)) {
            if (this.props.status.message !== constants.Element.ElementCreatedSuccessful && this.props.status.message !== constants.Element.ElementUpdatedSuccessful) {
                this.props.showError(this.props.status.message);
                this.props.refreshStatus();
            } else {
                this.props.refreshElements();
                this.props.resetColorPicker();
                this.props.closeModal();
                this.props.refreshStatus();
                this.props.resetIconPicker();
            }
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    const isCreate = ownProps.mode === constants.Modes.Create;
    let color = null;
    if (!isCreate) {
        if (utilities.isNullOrUndefined(state.colorPicker.elementColor) || utilities.isNullOrUndefined(state.colorPicker.elementColor.color)) {
            color = state.category.manageElement.categoryElement.color;
        } else {
            color = state.colorPicker.elementColor.color;
        }
    } else if (!utilities.isNullOrUndefined(state.colorPicker.elementColor)) {
        color = state.colorPicker.elementColor.color;
    }
    return {
        categories: state.category.manageCategory.categories,
        initialValues: ownProps.mode === constants.Modes.Create ? { isActive: true } : state.category.manageElement.initialValues,
        color,
        refreshToggler: state.category.manageElement.refreshToggler,
        colorPicked: state.colorPicker.elementColor ? state.colorPicker.elementColor.color : null,
        status: state.category.manageElement.status ? state.category.manageElement.status : null,
        iconModal: state.category.manageElement.iconModal,
        icons: state.category.manageElement.icons,
        icon: state.iconPicker ? state.iconPicker : null,
        categoryElement: state.category.manageElement.categoryElement ? state.category.manageElement.categoryElement : null
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getCategories: () => {
            dispatch(getCategories());
        },
        createElement: element => {
            dispatch(createElement(element));
        },
        updateElement: element => {
            dispatch(updateElement(element));
        },
        refreshElements: () => {
            dispatch(getCategoryElements(true));
            dispatch(refreshGrid('elements'));
        },
        selectIconsModal: () => {
            dispatch(openIconModal());
        },
        resetColorPicker: () => {
            dispatch(resetColorPicker('elementColor'));
        },
        showError: message => {
            dispatch(showError(resourceProvider.read('categoryColorAlreadyExists') + ' ' + message, true, resourceProvider.read('categoryColorAlreadyExists') + ' ' + message));
        },
        getIcons: () => {
            dispatch(getIcons());
        },
        selectIcon: value => {
            dispatch(openIconModal());
            dispatch(change('createElement', 'icon', value + '.svg'));
        },
        refreshStatus: () => {
            dispatch(refreshStatus());
        },
        closeIconModal: () => {
            dispatch(openIconModal());
        },
        resetIconPicker: () => {
            dispatch(resetIconPicker());
        }
    };
};


const CreateElementForm = reduxForm({
    form: 'createElement',
    asyncValidate,
    enableReinitialize: true,
    shouldAsyncValidate: utilities.shouldAsyncValidate
})(CreateElement);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(CreateElementForm);
