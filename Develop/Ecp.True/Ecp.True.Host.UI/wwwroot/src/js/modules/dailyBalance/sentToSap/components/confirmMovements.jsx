import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { confirmSentToSap } from '../actions.js';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

class ConfirmMovements extends React.Component {
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
                        resourceProvider.read('confirmationMovementsMessage')
                            .replace('{0}', this.props.selectedMovements.length)
                            .replace('{1}', this.props.countTotalMovements)
                    }
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('confirmSentMovements', {
                    onAccept: this.confirm,
                    disableAccept: (this.props.selectedMovements.length === 0),
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
            dispatch(confirmSentToSap());
        }
    };
};

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        selectedMovements: state.sendToSap[state.sendToSap.name].selectedMovements,
        countTotalMovements: state.sendToSap[state.sendToSap.name].countTotalMovements
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(ConfirmMovements);
