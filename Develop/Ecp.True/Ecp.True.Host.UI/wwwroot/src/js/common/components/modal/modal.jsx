import React from 'react';
import { connect } from 'react-redux';
import { closeModal, hideNotification } from '../../actions.js';
import { modalService } from './../../services/modalService.js';
import { utilities } from '../../services/utilities';
import { resourceProvider } from '../../../common/services/resourceProvider';
import Notification from './../../../common/components/notification/notification.jsx';
import classNames from 'classnames/bind';

export class Modal extends React.Component {
    constructor() {
        super();

        this.onCancel = this.onCancel.bind(this);
        this.onAccept = this.onAccept.bind(this);
        this.onClose = this.onClose.bind(this);
        this.onEscape = this.onEscape.bind(this);
    }

    onCancel() {
        const value = this.props.modalState.cancelAction;
        if (utilities.isArray(value)) {
            value.forEach(element => {
                this.props.fireAction(element);
            });
        } else if (!utilities.isNullOrUndefined(value)) {
            this.props.fireAction(value);
        }
        this.props.closeModal();
    }

    onAccept() {
        const value = this.props.modalState.acceptActionAndClose || this.props.modalState.acceptAction;

        if (utilities.isNullOrUndefined(value)) {
            this.props.closeModal();
            return;
        }

        if (utilities.isArray(value)) {
            value.forEach(element => {
                this.props.fireAction(element);
            });
        } else {
            this.props.fireAction(value);
        }

        if (!utilities.isNullOrUndefined(this.props.modalState.acceptActionAndClose)) {
            this.props.closeModal();
        }
    }

    onClose() {
        const value = this.props.modalState.closeAction;
        if (utilities.isArray(value)) {
            value.forEach(element => {
                this.props.fireAction(element);
            });
        } else if (!utilities.isNullOrUndefined(value)) {
            this.props.fireAction(this.props.modalState.closeAction);
        }
        this.props.closeModal();
    }

    onEscape(event) {
        if (event.keyCode === 27) {
            this.onClose();
        }
    }

    render() {
        if (!this.props.modalState.isOpen) {
            modalService.reset();
            return null;
        }

        if (!this.props.modalState.messageOnly) {
            modalService.initialize(this.props.modalState);
            const title = `${this.props.modalState.mode}${modalService.getTitle(this.props.modalState.title)}`;

            const CurrentComponent = modalService.getCurrentComponent();
            const props = modalService.getProps();

            return (
                <div className={this.props.modalState.className} id={`cont_modal_${this.props.modalState.modalKey}`}>
                    <div className="ep-modal__overlay">
                        <section className={this.props.modalState.bodyClassName}>
                            <header className="ep-modal__header">
                                <h1 className={`ep-modal__title ${this.props.modalState.titleClassName}`} id="h1_modal_title">{resourceProvider.read(title)}</h1>
                                <span className="ep-modal__close" id="lbl_modal_close" onClick={this.props.closeModal}><i className="far fa-times-circle" /></span>
                            </header>
                            <Notification isOnModal={true} />
                            <CurrentComponent mode={this.props.modalState.mode} closeModal={this.props.closeModal} {...props} />
                        </section>
                    </div>
                </div>
            );
        }

        return (
            <div className={classNames(this.props.modalState.className)} id={`cont_modal_${this.props.modalState.title}`}>
                <div className="ep-modal__overlay">
                    <section className={this.props.modalState.bodyClassName}>
                        <header className="ep-modal__header">
                            <h1 className={`ep-modal__title ${this.props.modalState.titleClassName}`} id="h1_modal_title">{resourceProvider.read(this.props.modalState.title)}</h1>
                            <span className="ep-modal__close" id="lbl_modal_close" onClick={this.onClose}><i className="far fa-times-circle" /></span>
                        </header>
                        <div className="ep-modal__content" id="cont_confirm_message">{this.props.modalState.message}</div>
                        <footer className="ep-modal__footer">
                            <div className="ep-modal__footer-actions">
                                <span className="float-r">
                                    {this.props.modalState.canCancel &&
                                        <button id="btn_confirm_cancel" type="button" className="ep-btn ep-btn--link" onClick={this.onCancel}>
                                            {resourceProvider.read(this.props.modalState.cancelActionTitle)}</button>
                                    }
                                    <button id="btn_confirm_accept" type="button" className="ep-btn ep-btn--sm" onClick={this.onAccept}>
                                        {resourceProvider.read(this.props.modalState.acceptActionTitle)}
                                    </button>
                                </span>
                            </div>
                        </footer>
                    </section>
                </div>
            </div>
        );
    }

    componentDidUpdate(prevProps) {
        if (this.props.modalState.isOpen !== prevProps.modalState.isOpen) {
            if (this.props.modalState.isOpen === true) {
                document.addEventListener('keydown', this.onEscape, false);
            } else {
                document.removeEventListener('keydown', this.onEscape, false);
            }
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        modalState: state.modal
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        closeModal: () => {
            dispatch(hideNotification());
            dispatch(closeModal(true));
        },
        fireAction: action => {
            dispatch(action);
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(Modal);
