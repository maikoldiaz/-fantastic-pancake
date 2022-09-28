import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import { required, numericality } from 'redux-form-validators';
import { inputTextbox } from '../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { constants } from './../../../../common/services/constants';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

class EditNodeAttributes extends React.Component {
    render() {
        return (
            <>
                <section className="ep-modal__content">
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label">{resourceProvider.read('number')}</label>
                                <span className="ep-data">PR SANTIAGO</span>
                            </div>
                        </div>
                    </div>
                    <div className="row m-t-4">
                        <div className="col-md-6">
                            <div className="ep-control-group m-b-0">
                                <label className="ep-label" htmlFor="txt_decimal_controlLimit">{resourceProvider.read('controlLimit')}</label>
                                <Field id={`txt_decimal_controlLimit`} type="number"
                                    min="-9999999999999999.99" max={constants.DecimalRange.Max} step={constants.DecimalRange.Step}
                                    component={inputTextbox} name="controlLimit"
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                        numericality({
                                            greaterThanOrEqualTo: -9999999999999999.99,
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
                                <label className="ep-label">{resourceProvider.read('acceptableBalance')}</label>
                                <Field component={inputTextbox} name="acceptableBalance" validate={[required({ msg: resourceProvider.read('required') })]} />
                            </div>
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('editNodeAttributes', { acceptText: 'save' })} />
            </>
        );
    }
}

const EditNodeAttributesForm = reduxForm({ form: 'editNodeAttributes' })(EditNodeAttributes);
export default connect()(EditNodeAttributesForm);
