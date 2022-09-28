import React from 'react';
import { Field } from 'redux-form';
import Select, { components } from 'react-select';
import DatePicker from 'react-datepicker';
import { Portal } from 'react-overlays';
import { resourceProvider } from '../../services/resourceProvider.js';
import { DebounceAutocomplete } from '../autoComplete/autoComplete.jsx';
import { dateService } from '../../services/dateService.js';
import { utilities } from '../../services/utilities.js';
import NumberFormatter from './numberFormatter.jsx';
import { numberService } from '../../services/numberService.js';
import classNames from 'classnames/bind';
import { constants } from '../../services/constants.js';

const normalizeValue = e => {
    return e.target.value.trim();
};

const checkIfNumberAllowed = (minval, maxval, input) => {
    if (numberService.checkIfDecimalInRange(maxval, minval, input)) {
        return numberService.createBigDecimal(input.value).dp() <= 2;
    }
    return false;
};

// Textbox
export const inputTextbox = ({
    input,
    step,
    placeholder,
    type,
    className,
    ref,
    id,
    readOnly,
    onInputKeyUp,
    onInputMouseUp,
    onInputBlur,
    hasAddOn,
    addOnClass,
    min,
    max,
    disabled,
    meta: { touched, error, warning }
}) => (
    <div className={classNames('ep-control', { ['ep-control--addon']: hasAddOn })}>
        {hasAddOn &&
                <div className="ep-control__inner">
                    <input {...input} placeholder={placeholder} type={type} key="input" step={step} min={min} max={max}
                        className={classNames('ep-textbox', className, { ['ep-textbox--error']: (touched && error) })}
                        ref={ref} id={id} readOnly={readOnly} onKeyUp={onInputKeyUp} onMouseUp={onInputMouseUp}
                        onBlur={e => input.onBlur(onInputBlur ? onInputBlur(e) : normalizeValue(e))} autoComplete="off" />
                    <span className="ep-control__inner-addon"><i className={addOnClass} /></span>
                </div>}
        {!hasAddOn &&
                <input {...input} placeholder={placeholder} type={type} key="input" step={step} min={min} max={max}
                    className={classNames('ep-textbox', className, { ['ep-textbox--error']: (touched && error) })} disabled={disabled}
                    ref={ref} id={id} readOnly={readOnly} onKeyUp={onInputKeyUp} onMouseUp={onInputMouseUp}
                    onBlur={e => input.onBlur(onInputBlur ? onInputBlur(e) : normalizeValue(e))} autoComplete="off" />}
        {touched &&
                ((error &&
                    <span id={`${id}_error}`} className="ep-control__error">
                        <span className="ep-textbox__error-txt">{error}</span>
                    </span>) ||
                    (warning &&
                        <span id={`${id}_warning`} className="ep-control__warning">
                            <span className="ep-control__error-txt">{warning}</span>
                        </span>))}
    </div>);

export const inputDecimal = ({
    input,
    readOnly,
    placeholder,
    className,
    type,
    id,
    step,
    min,
    max,
    decimalScale,
    hasAddOn,
    addOnClass,
    allowNegative,
    isInteger,
    dirtyValidation,
    meta: { touched, error, warning, dirty }
}) => {
    let stepValue;
    let minValue;
    let maxValue;
    if (type === constants.InputType.Decimal) {
        minValue = min ? min : constants.DecimalRange.MinNegative;
        maxValue = max ? max : constants.DecimalRange.Max;
        stepValue = step ? step : constants.DecimalRange.Step;
    } else {
        minValue = min ? min : 0.01;
        maxValue = max ? max : constants.PercentageRange.Max;
        stepValue = step ? step : constants.PercentageRange.Step;
    }
    const showMessageError = dirtyValidation ? dirty : touched;

    return (
        <div className={classNames('ep-control ep-control--decimal', className, { ['ep-control--addon']: hasAddOn })}>
            <div className="ep-control__inner">
                <NumberFormatter
                    isInteger={isInteger}
                    value={input.value}
                    min={minValue}
                    max={maxValue}
                    step={stepValue}
                    readOnly={readOnly}
                    isNumericString={true}
                    placeholder={placeholder}
                    allowNegative={allowNegative}
                    decimalScale={decimalScale}
                    className={className}
                    id={id}
                    onValueChange={values => {
                        input.onChange(utilities.isNullOrWhitespace(values.value) ? values.value : numberService.createBigDecimalString(values.value));
                    }}
                    onBlur={e => checkIfNumberAllowed(minValue, maxValue, e.target.value)}
                    isAllowed={values => checkIfNumberAllowed(minValue, maxValue, values)} />
                {hasAddOn && <span className="ep-control__inner-addon"><i className={addOnClass} /></span>}
            </div>
            {showMessageError &&
                ((error &&
                    <span id={`${id}_error}`} className="ep-control__error">
                        <span className="ep-textbox__error-txt">{error}</span>
                    </span>) ||
                    (warning &&
                        <span id={`${id}_warning`} className="ep-control__warning">
                            <span className="ep-control__error-txt">{warning}</span>
                        </span>))}
        </div>
    );
};

// Textarea
export const inputTextarea = ({
    input,
    placeholder,
    type,
    className,
    ref,
    id,
    readOnly,
    onInputKeyUp,
    onInputMouseUp,
    onInputBlur,
    meta: { touched, error, warning }
}) => (
    <div className="ep-control">
        <textarea {...input} placeholder={placeholder} type={type} key="input"
            className={classNames('ep-textarea', className, { ['ep-textarea--error']: (touched && error) })}
            ref={ref} id={id} readOnly={readOnly} onKeyUp={onInputKeyUp} onMouseUp={onInputMouseUp}
            onBlur={e => input.onBlur(onInputBlur ? onInputBlur(e) : normalizeValue(e))} />
        {touched &&
                ((error &&
                    <span id={`${id}_error}`} className="ep-control__error">
                        <span className="ep-textbox__error-txt">{error}</span>
                    </span>) ||
                    (warning &&
                        <span id={`${id}_warning`} className="ep-control__warning">
                            <span className="ep-control__warning-txt">{warning}</span>
                        </span>))}
    </div>);

// Select
export const inputSelect = props => (
    <div className="ep-control">
        <Select classNamePrefix="ep-select" className={classNames('ep-select', { ['ep-select--multi']: props.isMulti, ['ep-select--error']: (props.meta.touched && props.meta.error) })}
            {...props}
            key="select"
            noOptionsMessage={() => resourceProvider.read('noOptionsMessage')}
            placeholder={resourceProvider.read(props.placeholderKey || 'select')}
            value={props.input.value}
            onChange={value => props.input.onChange(value)}
            onBlur={() => props.input.onBlur(props.input.value)}
            options={props.options} isClearable={true}
            filterOption={utilities.selectFilter()} blurInputOnSelect={false} />
        {props.meta.touched &&
            ((props.meta.error &&
                <span className="ep-control__error">
                    <span className="ep-control__error-txt">
                        {props.meta.error}
                    </span>
                </span>) ||
                (props.meta.warning &&
                    <span className="ep-control__warning">
                        <span className="ep-control__warning-txt">{props.meta.warning}</span>
                    </span>))}
    </div>
);

// Datepicker
export const datePickerContainer = ({ children }) => {
    const el = document.getElementById('epPortals');
    return (
        <Portal container={el}>
            {children}
        </Portal>
    );
};

export const inputDatePicker = ({
    input, ref, id, customClass, disabled, maxDate, minDate, placeholder, meta: { touched, error, warning } }) =>
    (
        <div className="ep-control">
            <div className={classNames('ep-datepicker', { ['ep-datepicker--error']: (touched && error) })}>
                <DatePicker {...input} selected={input.value ? dateService.parseToDate(input.value) : null}
                    key="DatePicker"
                    placeholderText={placeholder ? placeholder : resourceProvider.read('date')}
                    popperContainer={datePickerContainer}
                    disabled={disabled} dateFormat={'dd-MMM-yy'}
                    maxDate={maxDate}
                    minDate={minDate}
                    useWeekdaysShort={true}
                    autoComplete="off"
                    peekNextMonth showMonthDropdown showYearDropdown dropdownMode="scroll"
                    className="ep-datepicker__input" ref={ref} id={id} popperClassName={'ep-datepicker-container ' + customClass}
                    onBlur={() => { }} />
                <span className="ep-datepicker__icn"><i className="far fa-calendar-alt" /></span>
            </div>
            {touched &&
                ((error &&
                    <span className="ep-control__error">
                        <span className="ep-control__error-txt">{error}</span>
                    </span>) ||
                    (warning &&
                        <span className="ep-control__warning">
                            <span className="ep-control__warning-txt">
                                {warning}
                            </span>
                        </span>))}
        </div>
    );


// Toggler
export const inputToggler = ({
    input,
    className,
    id,
    readOnly
}) => (<label className={classNames('ep-toggler', className, { ['ep-toggler--disabled']: readOnly })}>
    <input id={id} className="ep-toggler__input" type="checkbox"
        value={input.value} defaultChecked={input.value}
        checked={input.value} disabled={readOnly}
        onChange={event => {
            if (!readOnly) {
                return input.onChange(event.target.checked);
            }
            return true;
        }
        } />
    <span className="ep-toggler__action" />
    <span className="sr-only">{resourceProvider.read('label')}</span>
</label>);

// Toggler
export class RadioButtonGroup extends React.Component {
    getCheckedValue(option, input, isSame, defaultValue) {
        if (!utilities.isNullOrUndefined(defaultValue) && utilities.isNullOrWhitespace(input.value)) {
            return defaultValue === option.value;
        }
        return isSame ? input.value === option.value : input.value.indexOf(option.value) !== -1;
    }
    RadioButtonGroup() {
        const { options, input, isSame, disabled, idPrefix, defaultValue } = this.props;
        return options.map((option, index) => {
            return (
                <div className="ep-radio-toggler__item" key={index}>
                    <label className="ep-radio-toggler__label" htmlFor={!utilities.isNullOrUndefined(idPrefix) ? `${idPrefix}${option.value}` : option.value + '_' + index}>
                        <input type="radio" className="ep-radio-toggler__input"
                            id={!utilities.isNullOrUndefined(idPrefix) ? `${idPrefix}${option.value}` : option.value + '_' + index}
                            name={input.name}
                            value={option.value}
                            disabled={disabled}
                            checked={this.getCheckedValue(option, input, isSame, defaultValue)}
                            onChange={event => {
                                return input.onChange(event.target.checked ? (option.value) : null);
                            }} />
                        <span className="ep-radio-toggler__action" />
                        <span className="ep-radio-toggler__text">{option.label}</span>
                    </label>
                </div>);
        });
    }
    render() {
        const { meta, className } = this.props;
        const hasError = meta.touched && meta.error;
        return (
            <div className={classNames('ep-radio-toggler', className)}>
                {this.RadioButtonGroup()}
                {hasError && <span className="ep-control__error">
                    <span className="ep-control__error-txt">
                        {meta.error}
                    </span>
                </span>}
            </div>
        );
    }
}

// checkbox
export const inputCheckbox = ({
    input,
    className,
    id,
    readOnly,
    canUpdate,
    label
}) => (<label className={classNames('ep-checkbox', className, { ['ep-checkbox--disabled']: readOnly })}>
    <input id={id} className="ep-checkbox__input" type="checkbox"
        value={input.value} defaultChecked={input.value}
        checked={input.value} disabled={readOnly}
        onChange={event => {
            if (canUpdate) {
                return input.onChange(event.target.checked);
            }
            return true;
        }
        } />
    <span className="ep-checkbox__action" />
    {label && <span className="ep-checkbox__txt">{label}</span>}
</label>);

// Radio
export const inputRadio = ({
    input,
    className,
    id,
    readOnly,
    canUpdate,
    label
}) => (<label className={classNames('ep-radio', className, { ['ep-radio--disabled']: readOnly })}>
    <input id={id} className="ep-radio__input" type="radio"
        value={input.value} defaultChecked={input.value}
        checked={input.value} disabled={readOnly}
        onChange={event => {
            if (canUpdate) {
                return input.onChange(event.target.checked);
            }
            return true;
        }
        } />
    <span className="ep-radio__action" />
    {label && <span className="ep-radio__txt">{label}</span>}
</label>);

// Fileupload
export const inputFileupload = ({
    input,
    label
}) => (
    <section className="ep-fupload">
        <div className="ep-fupload__control">
            <label className="ep-fupload__input">
                <input type="file" />
                <i className="fas fa-file-upload m-r-2" />
                    Browse
            </label>
        </div>
        <ul className="ep-fupload__list">
            <li className="ep-fupload__file">
                <span className="ep-fupload__icn"><i className="fas fa-file-excel" /></span>
                <p className="ep-fupload__info">
                    <span className="ep-fupload__info-title">Archivo1.xls</span>
                    <span className="ep-fupload__info-stitle">1.2MB</span>
                </p>
                <span className="ep-fupload__del"><i className="fas fa-trash" /></span>
            </li>
            <li className="ep-fupload__file">
                <span className="ep-fupload__icn"><i className="fas fa-file-excel" /></span>
                <p className="ep-fupload__info">
                    <span className="ep-fupload__info-title">Archivo1.xls</span>
                    <span className="ep-fupload__info-stitle">1.2MB</span>
                </p>
                <span className="ep-fupload__del"><i className="fas fa-trash" /></span>
            </li>
        </ul>
    </section>);

export const inputAutocomplete = props => (
    <div className="ep-control ep-control--ac">
        <div className="ep-control__inner">
            <DebounceAutocomplete {...props} handleChange={props.input.onChange} />
            <span className="ep-control__inner-addon"><i className="fas fa-search" /></span>
        </div>
        {props.meta.touched &&
            ((props.meta.error &&
                <span className="ep-control__error">
                    <span className="ep-control__error-txt">
                        {props.meta.error}
                    </span>
                </span>) ||
                (props.meta.warning &&
                    <span className="ep-control__warning">
                        <span className="ep-control__warning-txt">{props.meta.warning}</span>
                    </span>))}
    </div>
);

// Date Range

export class InputDateRange extends React.Component {
    constructor() {
        super();
        this.state = {
            year: null
        };
        this.monthInpRef = React.createRef();
    }

    onYearChange(selectedYear) {
        this.monthInpRef.current.select.clearValue('');
        this.setState({ year: selectedYear });

        if (this.props.onYearSelect) {
            this.props.onYearSelect({
                selectedMonths: dateService.getMonthRange(this.props.dateRange, selectedYear),
                selectedYear: selectedYear
            });
        }
    }

    renderSingleValue(props) {
        return (
            <components.SingleValue {...props}>
                <div className="d-flex d-flex--jc-cen">
                    <span className="p-r-4 br-r-1">
                        {dateService.capitalize(props.data.start, 'DD-MMM').split('.')[0]}
                    </span>
                    <span className="p-l-4">
                        {dateService.capitalize(props.data.end, 'DD-MMM').split('.')[0]}
                    </span>
                </div>
            </components.SingleValue>
        );
    }

    renderMonthRangeOptions(month) {
        return (
            <div className="ep-select__option-month">
                <span className="text-right">
                    {dateService.capitalize(month.start, 'DD-MMM').split('.')[0]}
                </span>
                <span className="text-center">
                    <i className="fas fa-calendar-week" />
                </span>
                <span>
                    {dateService.capitalize(month.end, 'DD-MMM').split('.')[0]}
                </span>
            </div>
        );
    }

    render() {
        const dataRange = this.props.dateRange;
        const years = Object.keys(dataRange).sort((a, b) => (b - a));
        let year = this.state.year ? this.state.year : years[0];
        if (Object.keys(dataRange).length === 0) {
            year = null;
        }
        const months = dateService.getMonthRange(dataRange, year);
        const defaultMonth = months.find(v => v.isDefault);
        const isDisabled = months.length === 0;

        return (
            <div className="ep-daterange">
                <div className="ep-daterange__body">
                    <div className="ep-daterange__date">
                        <label className="ep-label">{this.props.dateLbl ? this.props.dateLbl : resourceProvider.read('processingPeriod')}</label>
                        <div className="ep-control">
                            <Select ref={this.monthInpRef} classNamePrefix="ep-select"
                                className={classNames('ep-select', { ['ep-select--error']: (this.props.periods.meta.touched && this.props.periods.meta.error) })}
                                {...this.props.periods.input}
                                key="select"
                                noOptionsMessage={() => resourceProvider.read('noOptionsMessage')}
                                placeholder={resourceProvider.read(this.props.placeholderKey || 'select')}
                                value={this.props.periods.input.value ? this.props.periods.input.value : defaultMonth}
                                onChange={value => this.props.periods.input.onChange(value)}
                                onBlur={() => this.props.periods.input.onBlur(this.props.periods.input.value)}
                                isDisabled= {isDisabled}
                                options={months} isClearable={true}
                                formatOptionLabel={this.renderMonthRangeOptions}
                                components={{ SingleValue: this.renderSingleValue }}
                                filterOption={(option, inputValue) => {
                                    return dateService.format(option.data.start.toString()).split('-')[1].toLowerCase().includes(inputValue.toLowerCase()) ||
                                    dateService.format(option.data.end.toString()).split('-')[1].toLowerCase().includes(inputValue.toLowerCase());
                                }}
                            />
                        </div>
                    </div>
                    <div className="ep-daterange__year">
                        <label className="ep-label">{this.props.yearLbl ? this.props.yearLbl : resourceProvider.read('year')}</label>
                        <div className="ep-control">
                            <Select classNamePrefix="ep-select" className={classNames('ep-select', { ['ep-select--error']: (this.props.year.meta.touched && this.props.year.meta.error) })}
                                {...this.props.year.input}
                                key="select"
                                placeholder={'--'}
                                options={years}
                                value={[year]}
                                getOptionLabel={x => x} getOptionValue={x => x}
                                onChange={value => this.onYearChange(value)}
                                onBlur={() => this.props.year.input.onBlur(this.props.year.input.value)}
                                isDisabled= {years.length === 0}
                            />
                        </div>
                    </div>
                </div>
                <div className="ep-daterange__mesg">
                    {this.props.periods.meta.touched &&
                        ((this.props.periods.meta.error &&
                            <span className="ep-control__error">
                                <span className="ep-control__error-txt">
                                    {this.props.periods.meta.error}
                                </span>
                            </span>) ||
                            (this.props.periods.meta.warning &&
                                <span className="ep-control__warning">
                                    <span className="ep-control__warning-txt">{this.props.periods.meta.warning}</span>
                                </span>))}
                </div>
                <div className="ep-daterange__mesg">
                    {this.props.year.meta.touched &&
                        ((this.props.year.meta.error &&
                            <span className="ep-control__error">
                                <span className="ep-control__error-txt">
                                    {this.props.year.meta.error}
                                </span>
                            </span>) ||
                            (this.props.year.meta.warning &&
                                <span className="ep-control__warning">
                                    <span className="ep-control__warning-txt">{this.props.year.meta.warning}</span>
                                </span>))}
                </div>
            </div>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.defaultYear !== this.props.defaultYear) {
            this.onYearChange(this.props.defaultYear);
        }
    }
}

export class InputAutocompleteChipFilter extends React.Component {
    constructor(props) {
        super(props);
        this.onSelect = this.onSelect.bind(this);
    }

    buildSearchedItems(items) {
        let searchedItems = !utilities.isNullOrUndefined(items) ? [...items] : [];

        if (this.props.hasAllOption) {
            const defaultItem = {
                name: resourceProvider.read('all'),
                [this.props.idName]: 0
            };
            searchedItems = [defaultItem, ...searchedItems];
        }

        return searchedItems;
    }

    getAllItems() {
        return this.props.fields.getAll() || [];
    }

    onSelect(item) {
        const selectedItemList = this.getAllItems();
        const existItem = selectedItemList.some(x => x[this.props.idName] === item[this.props.idName]);
        if (!existItem) {
            const currentItemIsAllOption = item[this.props.idName] === 0;
            if (
                (currentItemIsAllOption && selectedItemList.length > 0) ||
                (!currentItemIsAllOption && selectedItemList.some(x => x[this.props.idName] === 0))
            ) {
                this.props.fields.removeAll();
            }
            this.props.fields.push(item);
        }
    }

    onRemoveItem(index) {
        this.props.fields.remove(index);
    }

    render() {
        const searchedItems = this.buildSearchedItems(this.props.searchedItems);

        return (
            <React.Fragment>
                <Field type="text" component={inputAutocomplete} name="inputChipFilter"
                    onSelect={this.onSelect} shouldChangeValueOnSelect={true}
                    onChange={this.props.searchItems} disabled={this.props.disabled}
                    shouldItemRender={(item, value) => item.name === constants.Todos || item.name.toLowerCase().indexOf(value.toLowerCase()) > -1}
                    renderItem={(item, isHighlighted) =>
                        (<div key={item.name} style={{ padding: '10px 12px', background: isHighlighted ? '#eee' : '#fff' }}>
                            {item.name}
                        </div>)
                    }
                    items={searchedItems} getItemValue={n => n.name} />

                <ul className="m-t-4">
                    {this.getAllItems().map((item, index) => (
                        <li key={item[this.props.idName]} className="ep-badge-pill">
                            {item.name}
                            <span className="m-l-2" onClick={() => this.onRemoveItem(index)}>
                                <i className="fas fa-times-circle" />
                            </span>
                        </li>
                    ))}
                </ul>

                {this.props.meta.dirty &&
                    ((this.props.meta.error &&
                        <span className="ep-control__error">
                            <span className="ep-control__error-txt">
                                {this.props.meta.error}
                            </span>
                        </span>) ||
                        (this.props.meta.warning &&
                            <span className="ep-control__warning">
                                <span className="ep-control__warning-txt">{this.props.meta.warning}</span>
                            </span>))}
            </ React.Fragment>
        );
    }
}
