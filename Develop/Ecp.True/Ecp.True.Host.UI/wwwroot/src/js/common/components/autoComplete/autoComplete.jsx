import React from 'react';
import ReactAutocomplete from 'react-autocomplete';
import { DebounceInput } from 'react-debounce-input';

export class Autocomplete extends React.Component {
    constructor() {
        super();
        this.onChange = this.onChange.bind(this);
        this.onSelect = this.onSelect.bind(this);
        this.state = {
            value: ''
        };
    }

    onChange(e) {
        this.setState({ value: e.target.value });
        this.props.onChange(e);
    }

    onSelect(value, item) {
        if (this.props.shouldChangeValueOnSelect) {
            this.setState({ value: this.props.getItemValue(item) });
        }
        this.props.onSelect(item);
        this.setState({ value: value });
    }

    onMenuRender(items, value, style) {
        return (
            <div style={items.length > 0 ? { ...style, ...this.menuStyle } : {}}>
                {items}
            </div>);
    }

    getStyles() {
        const wrapperStyle = {
            display: 'block',
            width: '100%'
        };
        const menuStyle = {
            borderRadius: '3px',
            boxShadow: '0 2px 12px rgba(0, 0, 0, 0.1)',
            background: 'rgba(255, 255, 255, 0.9)',
            fontSize: '90%',
            position: 'absolute',
            top: '37px',
            left: '0',
            overflow: 'auto',
            maxHeight: '300px',
            border: 'solid 1px #9C9C9C',
            zIndex: '100'
        };

        return { menuStyle, wrapperStyle };
    }

    render() {
        if (this.props.clear === true) {
            this.setState({ value: '' });
        }
        return (
            <ReactAutocomplete {...this.getStyles()} getItemValue={this.props.getItemValue}
                items={this.props.items} shouldItemRender={this.props.shouldItemRender}
                renderItem={this.props.renderItem} renderMenu={this.onMenuRender}
                value={this.state.value || this.props.defaultValue}
                onChange={this.onChange} onSelect={this.onSelect}
                inputProps={{ placeholder: this.props.placeholder, disabled: this.props.disabled || false }}
            />
        );
    }
}

export class DebounceAutocomplete extends React.Component {
    constructor() {
        super();
        this.onChange = this.onChange.bind(this);
    }
    onChange(e) {
        const val = e.target.value;
        this.props.handleChange(val);
    }
    render() {
        return (
            <DebounceInput
                minLength={1}
                debounceTimeout={500}
                element={Autocomplete}
                onChange={this.onChange}
                {...this.props} />
        );
    }
}
