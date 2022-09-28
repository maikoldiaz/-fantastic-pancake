import React from 'react';
import { connect } from 'react-redux';
import classNames from 'classnames/bind';

export class Loader extends React.Component {
    render() {
        return (
            <div className={classNames('ep-loader', { ['ep-loader--hidden']: !this.props.showLoader })}>
                <div className="ep-loader__content">
                    <div className="loader-inner line-scale">
                        <div />
                        <div />
                        <div />
                    </div>
                </div>
            </div>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        showLoader: state.loader.counter > 0
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps)(Loader);
