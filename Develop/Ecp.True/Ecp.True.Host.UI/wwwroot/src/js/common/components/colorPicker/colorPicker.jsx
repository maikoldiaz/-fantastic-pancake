import React from 'react';
import { connect } from 'react-redux';
import { SwatchesPicker } from 'react-color';
import { toggleColorPicker, setColorPicker } from './../../actions';

class ColorPicker extends React.Component {
    constructor() {
        super();
        this.handleClose = this.handleClose.bind(this);
        this.onColorPickerChange = this.onColorPickerChange.bind(this);
    }

    handleClose() {
        this.props.toggleColorPicker(false);
    }

    onColorPickerChange(color) {
        this.props.setColorPicker(color.hex);
        this.props.toggleColorPicker(false);
    }

    componentDidMount() {
        this.props.toggleColorPicker(false);
    }
    render() {
        const isOpen = this.props.isOpen;
        return (
            <section className="ep-col-picker">
                <div id="col-picker_control" className="ep-col-picker__control" onClick={() =>this.props.toggleColorPicker(!this.props.isOpen)} style={{ backgroundColor: this.props.color }} />
                { isOpen ? <div className="ep-col-picker__menu">
                    <div id="col-picker_overlay" className="ep-col-picker__overlay" onClick={this.handleClose}/>
                    <SwatchesPicker id="swatches_picker" className="ep-col-picker__colors" color={this.props.color} onChange={this.onColorPickerChange} />
                </div> : null }
            </section>
        );
    }
}

const mapStateToProps = (state, ownProps) => {
    return {
        isOpen: state.colorPicker[ownProps.name] && state.colorPicker[ownProps.name].isOpen,
        color: ownProps.color ? ownProps.color : state.colorPicker[ownProps.name] && state.colorPicker[ownProps.name].color
    };
};

const mapDispatchToProps = (dispatch, ownProps) =>{
    return {
        toggleColorPicker: isOpen => {
            dispatch(toggleColorPicker(ownProps.name, isOpen));
        },
        setColorPicker: color => {
            dispatch(setColorPicker(ownProps.name, color));
        }
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(ColorPicker);
