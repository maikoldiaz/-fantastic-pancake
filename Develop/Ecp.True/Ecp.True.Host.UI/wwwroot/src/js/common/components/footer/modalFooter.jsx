import React from 'react';
import { resourceProvider } from '../../services/resourceProvider';
import { closeModal, hideNotification } from '../../actions';
import { connect } from 'react-redux';
import { utilities } from '../../services/utilities';
import { constants } from '../../services/constants';

export class ModalFooter extends React.Component {
    constructor() {
        super();
        this.onClick = this.onClick.bind(this);
        this.onCancel = this.onCancel.bind(this);
        this.onAccept = this.onAccept.bind(this);
        this.getButtons = this.getButtons.bind(this);
    }

    onClick(values) {
        if (utilities.isArray(values)) {
            values.forEach(element => this.props.fireAction(element));
        } else {
            this.props.fireAction(values);
        }
    }

    onCancel() {
        if (this.props.config.cancel.actions.length > 0) {
            this.onClick(this.props.config.cancel.actions);
        } else {
            this.props.closeModal();
        }
    }

    onAccept() {
        if (this.props.config.accept.actions.length > 0) {
            this.onClick(this.props.config.accept.actions);
        } else if (this.props.config.accept.closeModal) {
            this.props.closeModal();
        }
    }

    getButtons(config) {
        return (
            <>
                {config.cancel &&
                    <button id={`btn_${config.cancel.key}_cancel`} className={config.cancel.className} type={config.cancel.type}
                        onClick={config.cancel.onClick || this.onCancel} disabled={config.cancel.disable ? config.cancel.disable : false}>
                        {resourceProvider.read(config.cancel.text ? config.cancel.text : 'cancel')}
                    </button>
                }
                {config.accept &&
                    <button id={`btn_${config.accept.key}_submit`} className={config.accept.className} type={config.accept.type}
                        onClick={config.accept.onClick || this.onAccept} disabled={config.accept.disable ? config.accept.disable : false}>
                        {resourceProvider.read(config.accept.text ? config.accept.text : 'submit')}
                    </button>
                }
            </>
        );
    }
    render() {
        return (
            <>
                {this.props.type === constants.Footer.Section && this.props.floatRight &&
                    <footer className="ep-section__footer">
                        <span className="float-r">
                            {this.getButtons(this.props.config)}
                        </span>
                    </footer>
                }
                {this.props.type === constants.Footer.Section && !this.props.floatRight &&
                    <footer className="ep-section__footer">
                        <div className="text-center">
                            {this.getButtons(this.props.config)}
                        </div>
                    </footer>
                }
                {this.props.type !== constants.Footer.Section &&
                    <footer className="ep-modal__footer">
                        <div className="ep-modal__footer-actions">
                            <span className="float-r">
                                {this.getButtons(this.props.config)}
                            </span>
                        </div>
                    </footer>
                }
            </>
        );
    }
}

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
export default connect(null, mapDispatchToProps, utilities.merge)(ModalFooter);
