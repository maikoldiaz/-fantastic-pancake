import React from 'react';
import { Field } from 'redux-form';
import { numericality, required } from 'redux-form-validators';
import { inputDecimal } from '../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import Tooltip from '../../../../common/components/tooltip/tooltip.jsx';

const InputSegment = props => {
    return (
        <>
            {props.fields.map((segment, index) => (
                <div className="col-md-3" key={index}>
                    <div className="ep-control-group">
                        <Tooltip body={props.fields.get(index).name}>
                            <label className="ep-label m-l-4 ellipsis">{props.fields.get(index).name}</label>
                        </Tooltip>
                        <div className="d-flex d-flex--a-center">
                            <span className="m-r-1">{constants.Prefix}</span>
                            <div className="full-width">
                                <Field
                                    type={constants.InputType.Decimal}
                                    component={inputDecimal}
                                    min={constants.DecimalRange.defaultMin}
                                    name={`${segment}.deviationPercentage`}
                                    dirtyValidation
                                    hasAddOn
                                    addOnClass="fas fa-percentage"
                                    validate={[
                                        required({ msg: { presence: resourceProvider.read('required') } }),
                                        numericality({
                                            lessThanOrEqualTo: props.maxDeviationPercentage,
                                            greaterThanOrEqualTo: props.minDeviationPercentage,
                                            notANumber: true,
                                            msg: {
                                                lessThanOrEqualTo: resourceProvider.read('percentageOutOfTolerance'),
                                                notANumber: resourceProvider.read('notNumber'),
                                                greaterThanOrEqualTo: resourceProvider.read('negativeValuesAreNotAllowed')
                                            },
                                            allowBlank: true
                                        })]} />
                            </div>
                        </div>
                    </div>
                </div>
            ))}
        </>
    );
};

export default InputSegment;
