import React from 'react';
import { Field } from 'redux-form';
import { required, numericality } from 'redux-form-validators';
import { inputTextbox } from './formControl.jsx';
import { resourceProvider } from '../../services/resourceProvider.js';
import { constants } from '../../services/constants.js';
import { utilities } from '../../services/utilities.js';

export class DecimalField extends React.Component {
    render() {
        const step = this.props.step ? this.props.step : 0.01;
        const min = this.props.min ? this.props.min : -9999999999999999.99;
        const max = this.props.max ? this.props.max : constants.DecimalRange.Max;
        const msg = this.props.msg ? this.props.msg : resourceProvider.read('decimalValidationMessage');
        const value = this.props.value;
        const onChange = this.props.onChange;
        const parse = this.props.parse;
        const notRequired = utilities.checkIfBoolean(utilities.normalizeBoolean(this.props.notRequired)) ? this.props.notRequired : false;

        return (
            <Field id={`txt_decimal_${this.props.name}`} type="number" min={min} max={max} step={step} component={inputTextbox} name={this.props.name}
                value={value} onChange={onChange} parse={parse}
                validate={[required({ msg: { presence: resourceProvider.read('required') }, if: () => !notRequired }),
                    numericality({
                        greaterThanOrEqualTo: min, lessThanOrEqualTo: max, msg: {
                            greaterThanOrEqualTo: resourceProvider.read('invalidRange'),
                            lessThanOrEqualTo: msg
                        },
                        allowBlank: notRequired
                    })]} />
        );
    }
}

export class PercentageField extends React.Component {
    render() {
        const step = this.props.step ? this.props.step : 0.01;
        const min = this.props.min ? this.props.min : 0.01;
        const max = this.props.max ? this.props.max : 100;
        const msg = this.props.msg ? this.props.msg : resourceProvider.read('percentageValidationMessage');

        return (
            <Field id={`txt_percentage_${this.props.name}`} type="number" min={min} max={max} step={step} component={inputTextbox} name={this.props.name}
                validate={[required({ msg: { presence: resourceProvider.read('required') }, if: () => !this.props.notRequired }),
                    numericality({
                        greaterThanOrEqualTo: min, lessThanOrEqualTo: max, msg: {
                            greaterThanOrEqualTo: resourceProvider.read('invalidRange'),
                            lessThanOrEqualTo: msg
                        }
                    })]}
                hasAddOn={true} addOnClass="fas fa-percentage" />
        );
    }
}
