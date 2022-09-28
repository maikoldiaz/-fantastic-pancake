import React from 'react';
import { connect } from 'react-redux';
import { Field, change } from 'redux-form';
import { required, length, numericality } from 'redux-form-validators';
import { inputTextbox, inputToggler } from '../../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { constants } from '../../../../../common/services/constants';
import { utilities } from '../../../../../common/services/utilities';

export class ProductSection extends React.Component {
    render() {
        const isDisable = !this.props.isEditable;
        return (
            <>
                <div className="col-md-6">
                    <div className="ep-control-group">
                        <label id="txt_sap_id" className="ep-label" htmlFor={`${this.props.name}_sapid_label`}>{resourceProvider.read('productSapId')}</label>
                        <Field component={inputTextbox} name={`productSapId`} id={`${this.props.name}_sapId`}
                            placeholder={resourceProvider.read('sapIdPlaceHolder')} disabled={isDisable}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                length({ max: 13, msg: resourceProvider.read('productSapIdLengthValidation') }),
                                numericality({ integer: true, msg: resourceProvider.read('productSapIdNumericValidation') })]} />
                    </div>
                    { this.props.mode === constants.Modes.Update &&
                        <div className="ep-control-group  m-b-0">
                            <label className="ep-label">{resourceProvider.read('active')}</label>
                            <Field component={inputToggler} name="isActive" id="tog_category_active" />
                        </div>
                    }
                </div>
                <div className="col-md-6">
                    <div className="ep-control-group">
                        <label id="txt_sap_name" className="ep-label" htmlFor={`${this.props.name}_sap_name_label`}>{resourceProvider.read('productSapName')}</label>
                        <Field component={inputTextbox} name={`productSapName`} id={`dd_${this.props.name}_sapName`}
                            placeholder={resourceProvider.read('sapNamePlaceHolder')}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                length({ max: 20, msg: resourceProvider.read('productSapNameLengthValidation') })]} />
                    </div>
                </div>
                { this.props.mode === constants.Modes.Create &&
                <div className="col-md-12">
                    <div className="ep-control-group">
                        <label id="txt_sap_name" className="ep-label" htmlFor={`${this.props.name}_sap_name_label`}>{resourceProvider.read('messageStatusCreateProduct')}</label>
                    </div>
                </div>
                }
            </>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        isEditable: state.modal.mode === constants.Modes.Create ? true : false
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        dispatchActions: actions => {
            actions.forEach(dispatch);
        },
        changeField: (field, value) => {
            dispatch(change('product', field, value));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(ProductSection);
