import React from 'react';
import { connect } from 'react-redux';
import { hideNotification, openModal, invokeButton } from './../../actions.js';
import { constants } from './../../services/constants.js';
import { resourceProvider } from './../../services/resourceProvider';
import classNames from 'classnames/bind';
import { utilities } from '../../services/utilities.js';

export class Notification extends React.Component {
    getNotifyIcnClass(type) {
        let cssClass = '';
        switch (type) {
        case constants.NotificationType.Success:
            cssClass = 'far fa-check-circle';
            break;
        case constants.NotificationType.Error:
            cssClass = 'fas fa-exclamation-circle';
            break;
        case constants.NotificationType.Warning:
            cssClass = 'fas fa-exclamation-triangle';
            break;
        case constants.NotificationType.Info:
            cssClass = 'fas fa-info-circle';
            break;
        default:
            cssClass = 'fas fa-info-circle';
            break;
        }

        return cssClass;
    }
    render() {
        const MessageComponent = this.props.component;
        const extraClasses = this.props.className ? this.props.className : '';
        const modalCLass = this.props.isOnModal ? ' ep-notification--modal' : '';

        return (
            <>
                {this.props.show && (this.props.isOnModal === this.props.showOnModal) && (
                    <div className={`ep-notification ep-notification--${this.props.state}${modalCLass} ${extraClasses}`}>
                        <span className="ep-notification__icn">
                            <i className={this.getNotifyIcnClass(this.props.state)} />
                        </span>
                        <div className="ep-notification__content">
                            <div className={classNames('ep-notification__info', { ['ep-notification__info--sm']: this.props.isButton })}>
                                {this.props.title && <h1 className="ep-notification__title">{this.props.title}</h1>}
                                <div className="ep-notification__msg">
                                    {MessageComponent && <MessageComponent />}
                                    {!utilities.isNullOrWhitespace(this.props.message) ? this.props.message : null}
                                    <br />
                                    {this.props.enableLink && (
                                        <p>
                                            {` ${resourceProvider.read(this.props.linkDescription)} `}
                                            <u>
                                                <b>
                                                    <a id="launchmodal" onClick={() => this.props.showErrors(this.props.launchComponent)}>                                                {' '}
                                                        <i className="fa fa-eye" />{` ${resourceProvider.read(this.props.linkMessage)} `}
                                                    </a>
                                                </b>
                                            </u>
                                        </p>
                                    )}
                                </div>
                            </div>
                            {this.props.isButton && (
                                <div className="ep-notification__action">
                                    <button className="ep-btn ep-btn--sm" onClick={() => this.props.invokeButtonAction(this.props.invokeNotificationButtonToggler)}>
                                        <i className="fas m-r-1 fa-unlock" /><span className="ep-btn__txt">{` ${resourceProvider.read(this.props.buttonText)} `}</span>
                                    </button>
                                </div>
                            )}
                        </div>
                        {this.props.onClose && <span className="ep-notification__close" id="page_notification" onClick={this.props.onClose}>
                            <i className="fas fa-times-circle" />
                        </span>}
                    </div>
                )}
            </>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        message: state.notification.message,
        state: state.notification.state,
        enableLink: state.notification.enableLink,
        launchComponent: state.notification.launchComponent,
        linkDescription: state.notification.linkDescription,
        linkMessage: state.notification.linkMessage,
        show: state.notification.show,
        showOnModal: state.notification.showOnModal,
        title: state.notification.title,
        isButton: state.notification.isButton,
        buttonText: state.notification.buttonText,
        component: state.notification.component,
        invokeNotificationButtonToggler: state.notificationButton.invokeNotificationButtonToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onClose: () => {
            dispatch(hideNotification());
        },
        showErrors: component => {
            dispatch(openModal(component));
        },
        invokeButtonAction: invokeNotificationButtonToggler => {
            dispatch(invokeButton(invokeNotificationButtonToggler));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(Notification);
