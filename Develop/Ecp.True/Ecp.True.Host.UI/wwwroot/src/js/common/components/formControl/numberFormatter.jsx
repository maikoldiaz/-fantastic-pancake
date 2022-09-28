import React from 'react';
import NumberFormat from 'react-number-format';
import { numberService } from '../../services/numberService.js';

export default class NumberFormatter extends React.Component {
    render() {
        const value = this.props.value;
        const min = this.props.min;
        const max = this.props.max;
        const step = this.props.step;
        const readOnly = this.props.readOnly;
        const isNumericString = this.props.isNumericString ? this.props.isNumericString : false;
        const displayType = this.props.displayType ? this.props.displayType : 'input';
        const prefix = this.props.prefix;
        const suffix = this.props.suffix;
        const placeholder = this.props.placeholder;
        const decimalScale = this.props.decimalScale;
        const className = this.props.className;
        const allowNegative = this.props.allowNegative;
        const id = this.props.id;
        const defaultValue = this.props.defaultValue;
        const style = this.props.style;
        const inputMode = this.props.inputMode;
        const onValueChange = this.props.onValueChange;
        const isAllowed = this.props.isAllowed;
        const onBlur = this.props.onBlur;
        const onChange = this.props.onChange;
        const onKeyDown = this.props.onKeyDown;

        let decimalSeperator = numberService.getDecimalSymbol();
        if (this.props.isInteger) {
            decimalSeperator = false;
        }
        return (
            <NumberFormat
                value={value}
                min={min}
                max={max}
                step={step}
                readOnly={readOnly}
                isNumericString={isNumericString}
                displayType={displayType}
                prefix={prefix}
                suffix={suffix}
                placeholder={placeholder}
                allowNegative={allowNegative}
                decimalScale={decimalScale}
                decimalSeparator={decimalSeperator}
                thousandSeparator={numberService.getGroupingSymbol()}
                className={className}
                id={id}
                defaultValue={defaultValue}
                style={style}
                inputMode= {inputMode}
                onValueChange={onValueChange}
                onBlur={onBlur}
                isAllowed={isAllowed}
                onChange={onChange}
                onKeyDown={onKeyDown}/>
        );
    }
}
