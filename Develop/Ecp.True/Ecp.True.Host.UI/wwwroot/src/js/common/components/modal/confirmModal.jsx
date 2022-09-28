import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../common/services/resourceProvider';
import { closeConfirm } from '../../actions';
import { utilities } from '../../../common/services/utilities';
import ModalFooter from '../footer/modalFooter.jsx';
import { footerConfigService } from '../../services/footerConfigService';

export class ConfirmModal extends React.Component {
    constructor() {
        super();
        this.onConfirm = this.onConfirm.bind(this);
        this.onCancel = this.onCancel.bind(this);
        this.onClose = this.onClose.bind(this);
    }

    onConfirm() {
        this.props.onClose();
        this.props.onConfirm(this.props.confirmModal.data);
    }

    onCancel() {
        this.props.onClose();
        if (this.props.onCancel) {
            this.props.onCancel();
        }
    }

    onClose() {
        if (this.props.handleClose) {
            this.props.handleClose();
        }
        this.props.onClose();
    }
    render() {
        const cancelButtonText = utilities.isNullOrWhitespace(this.props.confirmModal.cancelButtonText) ? resourceProvider.read('cancel') :
            resourceProvider.read(this.props.confirmModal.cancelButtonText);
        if (!this.props.confirmModal.isOpen) {
            return null;
        }
        return (
            <div className="ep-modal ep-modal--confirm" id="cont_confirm_modal">
                <div className="ep-modal__overlay">
                    <section className="ep-modal__body">
                        <header className="ep-modal__header">
                            <h1 className="ep-modal__title" id="h1_confirm_title">{resourceProvider.read(this.props.confirmModal.title)}</h1>
                            <span className="ep-modal__close" id="lbl_confirm_close" onClick={this.onClose}><i className="far fa-times-circle" /></span>
                        </header>
                        <div className="ep-modal__content" id="cont_confirm_message">
                            {this.props.confirmModal.message}
                        </div>
                        {this.props.confirmModal.shouldShowCancelButton &&
                            <ModalFooter config={footerConfigService.getCommonConfig('confirm',
                                { onAccept: this.onConfirm, onCancel: this.onCancel, acceptText: 'accept', acceptType: 'button', cancelText: cancelButtonText })} />
                        }
                        {!this.props.confirmModal.shouldShowCancelButton &&
                            <ModalFooter config={footerConfigService.getAcceptConfig('confirm', { acceptText: 'accept', onAccept: this.onConfirm })} />
                        }
                    </section>
                </div>
            </div>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        confirmModal: state.confirmModal
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onClose: () => {
            dispatch(closeConfirm());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(ConfirmModal);
