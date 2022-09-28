import React from 'react';
import { connect } from 'react-redux';
import { openModal } from '../../actions';
import { utilities } from '../../services/utilities';

export class PopupFactory extends React.Component {
    render() {
        return null;
    }

    componentDidUpdate(prevProps) {
        if (prevProps.confirmToggler !== this.props.confirmToggler) {
            this.props.openModal(`${this.props.type}Confirm`);
        }
        if (prevProps.updateToggler !== this.props.updateToggler) {
            this.props.openModal(`${this.props.type}BulkUpdate`);
        }
    }
}
/* istanbul ignore next */
const mapStateToProps = (state, ownprops) => {
    return {
        confirmToggler: state.ownershipRules[ownprops.type] ? state.ownershipRules[ownprops.type].confirmToggler : undefined,
        updateToggler: state.ownershipRules[ownprops.type] ? state.ownershipRules[ownprops.type].updateToggler : undefined
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        openModal: name => {
            dispatch(openModal(name));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(PopupFactory);
