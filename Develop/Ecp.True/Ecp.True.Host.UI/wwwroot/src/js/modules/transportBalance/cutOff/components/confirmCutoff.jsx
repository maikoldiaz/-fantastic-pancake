import React from 'react';
import { connect } from 'react-redux';
import { reduxForm } from 'redux-form';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { confirmCutoff } from '../actions.js';
import { dateService } from '../../../../common/services/dateService';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';

export class ConfirmCutoff extends React.Component {
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
            <form id="frm_confirm_cutoff" className="ep-form" onSubmit={this.props.handleSubmit(this.confirm)}>
                <div className="ep-modal__content">
                    <p>
                        <span className="fw-bold">
                            {resourceProvider.read('operationalCutConfirmationMessage')}
                        </span>
                    </p>
                    <div className="d-flex m-t-5">
                        <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('category')}:</label>
                        <span className="fw-bold">{this.props.ticket.segment.category.name}</span>
                    </div>
                    <div className="d-flex">
                        <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('element')}:</label>
                        <span className="fw-bold">{this.props.ticket.segment.name}</span>
                    </div>
                    <div className="d-flex">
                        <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('period')}:</label>
                        <span className="fw-bold m-r-1">{dateService.capitalize(this.props.ticket.startDate)}</span>
                        <span className="fw-sb fc-label m-r-1">{resourceProvider.read('to')}</span>
                        <span className="fw-bold">{dateService.capitalize(this.props.ticket.endDate)}</span>
                    </div>
                </div>
                <ModalFooter config={footerConfigService.getCommonConfig('confirmCutoff', { acceptText: 'accept' })} />
            </form>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        ticket: state.cutoff.operationalCut.ticket
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        confirm: () => {
            dispatch(confirmCutoff());
        }
    };
};

const confirmCutoffForm = reduxForm({
    form: 'confirmCutoff'
})(ConfirmCutoff);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(confirmCutoffForm);
