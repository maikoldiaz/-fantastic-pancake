import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { confirmCancelBatch } from '../actions.js';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

class ConfirmCancelBatch extends React.Component {
    constructor() {
        super();
        this.confirm = this.confirm.bind(this);
    }

    confirm() {
        this.props.confirm();
        this.props.closeModal();
    }

    render() {
        return (
            <div>
                <section className="ep-modal__content">
                    {
                        resourceProvider.read('confirmationCancelBatchMessage')
                    }
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('confirmCancelBatch', {
                    onAccept: this.confirm,
                    disableAccept: this.props.ticketId === '',
                    acceptText: resourceProvider.read('confirm'),
                    cancelText: resourceProvider.read('toClose')
                })} />
            </div>
        );
    }
}


/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        confirm: () => {
            dispatch(confirmCancelBatch());
        }
    };
};

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        ticketId: state.sendToSap.confirmWizard ? state.sendToSap.confirmWizard.ticketId : ''
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(ConfirmCancelBatch);
