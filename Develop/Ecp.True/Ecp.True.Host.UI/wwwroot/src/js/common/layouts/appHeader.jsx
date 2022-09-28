import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../services/resourceProvider';
import { dateService } from '../services/dateService';
import { utilities } from '../services/utilities';

class AppHeader extends React.Component {
    constructor() {
        super();
        this.getName = this.getName.bind(this);
    }

    logOut() {
        window.location.href = '/Account/SignOut?forceSignout=true';
    }

    getName() {
        if (utilities.isNullOrUndefined(this.props.context.userId)) {
            return '';
        }

        return `, ${this.props.context.userId}`;
    }

    render() {
        return (
            <header className="ep-header">
                <div className="ep-logo">
                    <img className="ep-logo__img float-l" src="./../dist/images/logo/logomascot-small.png" alt={resourceProvider.read('epMascot')} />
                    <h1 className="ep-logo__ttl">{resourceProvider.read('trueLogo')}</h1>
                </div>
                <div className="ep-header__profile">
                    <div className="ep-header__profile-pic">
                        {this.props.image ? <img alt={resourceProvider.read('profilepic')} src={this.props.image} /> : <span className="ep-icn ep-icn--user" />}
                    </div>
                    <div className="ep-header__profile-info">
                        <span className="ep-header__profile-name" id="lbl_user_name">
                            <span className="ep-header__profile-user" id="spn_user_name">
                                {`${dateService.wishMe()}${this.getName()}`}
                            </span>
                        </span>
                    </div>
                    <i className="fas fa-chevron-down fas--primary" />
                    <div className="ep-header__logout">
                        <a className="ep-header__logout-btn" id="lnk_user_logout" onClick={this.logOut}>
                            <i className="fas fa-sign-out-alt fs-18 m-l-1 m-r-2" />
                            {resourceProvider.read('logout')}
                        </a>
                    </div>
                </div>
            </header>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        context: state.root.context,
        image: state.root.context.image
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps)(AppHeader);
