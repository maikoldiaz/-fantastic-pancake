import React from 'react';
import { connect } from 'react-redux';
import { setIconId } from '../../actions';

export class IconPicker extends React.Component {
    constructor() {
        super();
        this.setIcon = this.setIcon.bind(this);
        this.getIconSet = this.getIconSet.bind(this);
    }

    setIcon(event) {
        this.props.setIconId(this.props.icons[parseInt(event.target.value, 10)].iconId, this.props.icons[parseInt(event.target.value, 10)].name);
    }

    getIconSet() {
        if (typeof this.props.icon.name !== 'undefined') {
            return this.props.icon.name;
        } else if (typeof this.props.initialValues.icon !== 'undefined' && this.props.initialValues.icon !== null) {
            return this.props.initialValues.icon.split('.')[0];
        }
        return null;
    }

    render() {
        const icons = this.props.icons;
        const iconId = this.getIconSet();
        return (
            <div className="ep-icon-picker">
                {icons.map((icon, index) => {
                    return (
                        <label className="ep-icon-picker__item" key={index}>
                            <input id="icon_picker" name="iconPicker" value={index} className="ep-icon-picker__input" type="radio"
                                onChange={this.setIcon} defaultChecked={iconId === icon.name} />
                            <span id="icon_search" dangerouslySetInnerHTML={{
                                __html: icon.content
                            }} className="ep-icon-picker__action" />
                        </label>
                    );
                })}
            </div>
        );
    }
}

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        setIconId: (iconId, name) => {
            dispatch(setIconId(iconId, name));
        }
    };
};

/* istanbul ignore next */
export default connect(null, mapDispatchToProps)(IconPicker);
