import React from 'react';
import DatePicker from 'react-datepicker';
import { Portal } from 'react-overlays';
import { utilities } from '../../services/utilities';
import { resourceProvider } from '../../services/resourceProvider';
import Select from 'react-select';
import { numberService } from '../../services/numberService';
import NumberFormatter from './../../../common/components/formControl/numberFormatter.jsx';

export class TextboxFilter extends React.Component {
    constructor(props) {
        super(props);

        this.onKeyDown = this.onKeyDown.bind(this);
        this.onChange = this.onChange.bind(this);

        this.filtering = false;
        this.value = null;
    }

    onChange(value) {
        this.value = value.trim();
        if (this.value && this.props.type === 'number') {
            this.value = Number(this.value);
        } else if (this.value && this.props.type === 'decimal') {
            this.value = numberService.changeFormat(this.value);
        } else {
            this.value = this.value.toString();
        }
    }

    onKeyDown(e) {
        if (e.key === 'Enter') {
            if (!this.props.column.onFilter) {
                this.props.onChange(this.value);
                return;
            }

            // If no value is found and col was never filtered
            if (utilities.isNullOrWhitespace(this.value) && !this.filtering) {
                return;
            }

            const column = utilities.replace(this.props.column.id.toString(), '.', '/');
            if (utilities.isNullOrWhitespace(this.value) && this.filtering === true) {
                this.props.column.onFilter(null, column);
                this.filtering = false;
            } else {
                this.filtering = true;
                let value = null;

                if (this.props.type === 'number' || this.props.type === 'decimal') {
                    value = Number(this.value);
                } else {
                    value = this.value.toString();
                }

                const number = this.props.type === 'number' || this.props.type === 'decimal';
                const key = number ? 'eq' : 'contains';

                this.props.column.onFilter(value, column, key, this.props.type === 'decimal');
            }
        }
    }
    getdisplayValue(hideFilter) {
        if (hideFilter) {
            return { display: 'none' };
        }

        return { display: 'block' };
    }
    render() {
        return (
            <div className="ep-table__filter">
                <label className="sr-only" htmlFor={`txt_${this.props.name}_${utilities.replace(this.props.column.id, '.', '_')}`}>{resourceProvider.read('label')}</label>
                {this.props.isDecimal &&
                    <NumberFormatter
                        className="ep-table__filter-txt"
                        id={`txt_${this.props.name}_${utilities.replace(this.props.column.id, '.', '_')}`}
                        defaultValue={this.props.column.defaultValue ? this.props.column.defaultValue.value : null}
                        style={this.getdisplayValue(this.props.column.hideFilterTextBox)}
                        inputMode="numeric"
                        onChange={e => this.onChange(e.target.value)}
                        onKeyDown={this.onKeyDown} />
                }
                {!this.props.isDecimal && <input className="ep-table__filter-txt" type="text"
                    id={`txt_${this.props.name}_${utilities.replace(this.props.column.id, '.', '_')}`}
                    defaultValue={this.props.column.defaultValue ? this.props.column.defaultValue.value : null}
                    onChange={e => this.onChange(e.target.value)} onKeyDown={this.onKeyDown}
                    autoComplete="off" style={this.getdisplayValue(this.props.column.hideFilterTextBox)} />}
            </div>
        );
    }

    componentDidMount() {
        if (this.props.column.defaultValue) {
            this.filtering = true;
        }
    }
}

export class SelectFilter extends React.Component {
    constructor(props) {
        super(props);
        this.onChange = this.onChange.bind(this);
    }

    onChange(option) {
        if (!this.props.column.onFilter) {
            const filterValue = option ? option.value : option;
            this.props.onChange(filterValue);
            return;
        }

        const column = this.props.column.id.toString().replace('.', '/');
        let val = option ? option.value.toString() : -1;

        // Convert to number
        if (!isNaN(val)) {
            val = Number(val);
        }

        // Convert to boolean
        if (val === 'true' || val === 'false') {
            val = val === 'true' || (val === 'false' ? false : val);
        }

        this.props.column.onFilter(val === -1 ? null : val, column, 'eq');
    }

    render() {
        const filterOptions = this.props.options.map(o => {
            return { label: resourceProvider.read(o.label), value: o.value };
        });

        const valueProps = this.props.column.defaultValue ? { defaultValue: filterOptions.filter(f => f.value === this.props.column.defaultValue.value) } : {};
        return (
            <div className="ep-table__filter">
                <Select className="ep-table__filter-dd" classNamePrefix="ep-table__filter-dd" {...valueProps}
                    noOptionsMessage={() => resourceProvider.read('noOptionsMessage')} filterOption={utilities.selectFilter()}
                    id={`dd_${this.props.name}_${this.props.column.id}`} options={filterOptions} onChange={opt => this.onChange(opt)}
                    isClearable={true} components={{ IndicatorSeparator: () => null }} placeholder="" />
            </div>
        );
    }
}

export const datePickerContainer = ({ children }) => {
    const el = document.getElementById('epPortals');
    return (
        <Portal container={el}>
            {children}
        </Portal>
    );
};

export class DateFilter extends React.Component {
    constructor(props) {
        super(props);

        this.state = {};
        this.cleared = false;

        this.onChange = this.onChange.bind(this);
        this.getValue = this.getValue.bind(this);
    }

    onChange(date) {
        this.setState({ value: date });
        this.cleared = date === null;

        if (!this.props.column.onFilter) {
            this.props.onChange(date);
            return;
        }

        const column = this.props.column.id.toString().replace('.', '/');
        this.props.column.onFilter(date, column, 'dt');
    }

    getValue() {
        if (this.cleared === true || this.state.value) {
            return this.state.value;
        }

        if (this.props.column.defaultValue) {
            return this.props.column.defaultValue.value;
        }

        return null;
    }

    render() {
        return (
            <div className="ep-table__filter">
                <label className="sr-only" htmlFor={`dt_${this.props.name}_${utilities.replace(this.props.column.id, '.', '_')}`}>{resourceProvider.read('datePicker')}</label>
                <DatePicker autoComplete="off" className="ep-table__filter-dp text-caps"
                    id={`dt_${this.props.name}_${utilities.replace(this.props.column.id, '.', '_')}`}
                    dateFormat={'dd-MMM-yy'} popperContainer={datePickerContainer} showMonthDropdown showYearDropdown
                    popperPlacement="bottom-start"
                    popperModifiers={{
                        offset: {
                            enabled: true,
                            offset: '5px, 10px'
                        },
                        preventOverflow: {
                            enabled: true,
                            escapeWithReference: false,
                            boundariesElement: 'viewport'
                        }
                    }}
                    selected={this.getValue()} onChange={this.onChange} useWeekdaysShort={true} />
                <span className="ep-table__filter-icn"><i className="far fa-calendar-alt" /></span>
            </div>
        );
    }
}
