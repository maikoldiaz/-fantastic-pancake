import React from 'react';
import { connect } from 'react-redux';
import { initFlyout, closeFlyout } from './../../actions';
import classNames from 'classnames/bind';

class Flyout extends React.Component {
    constructor(props) {
        super(props);
        this.props.initFlyout(this.props.name, false);
    }
    render() {
        const flyoutState = this.props.flyoutState;
        return (
            <>
                { flyoutState &&
                <section className={classNames('ep-flyout', { ['ep-flyout--active']: flyoutState.isOpen })} id={`fly_${this.props.name}`}>
                    <div className="ep-flyout__content" id={`cont_fly_${this.props.name}`}>
                        {this.props.children}
                    </div>
                </section>}
            </>

        );
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        flyoutState: state.flyout[ownProps.name]
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        closeFlyout: () => {
            dispatch(closeFlyout());
        },
        initFlyout: (name, isOpen) => {
            dispatch(initFlyout(name, isOpen));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(Flyout);
