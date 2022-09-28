import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, reset } from 'redux-form';
import { required } from 'redux-form-validators';
import { asyncValidate } from '../asyncValidate';
import { inputTextbox } from '../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { utilities } from '../../../../common/services/utilities';
import { viewTransaction } from '../actions';
import { hideNotification } from '../../../../common/actions';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

export class BlockSearch extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
    }

    onSubmit(formValues) {
        this.props.hideError();

        const transaction = { blockNumber: formValues.blockNumber, transactionHash: formValues.transactionHash };
        this.props.saveTransaction(transaction);

        this.props.onNext(this.props.config.wizardName);
    }

    render() {
        return (
            <form className="ep-form" id="frm_blockchain_search" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                <section className="ep-modal__content">
                    <h2 className="ep-control-group-title">{resourceProvider.read('blockchainSearch')}</h2>
                    <div className="ep-control-group">
                        <label id="lbl_blockchainSearch_blockNumber" className="ep-label" htmlFor="txt_blockchainSearch_blockNumber">
                            {resourceProvider.read('blockNumber')}</label>
                        <Field type="text" id="txt_blockchainSearch_blockNumber" component={inputTextbox}
                            placeholder={resourceProvider.read('blockNumber')} name="blockNumber"
                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                    </div>
                    <div className="ep-control-group">
                        <label id="lbl_blockchainSearch_transactionHash" className="ep-label" htmlFor="txt_blockchainSearch_transactionHash">
                            {resourceProvider.read('transactionHash')}</label>
                        <Field type="text" id="txt_blockchainSearch_transactionHash" component={inputTextbox}
                            placeholder={resourceProvider.read('transactionHash')} name="transactionHash"
                            validate={[required({ msg: { presence: resourceProvider.read('transactionIdEmpty') } })]} />
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('blockchain_search', { acceptText: 'query' })} />
            </form>
        );
    }

    componentWillUnmount() {
        this.props.resetForm();
    }
}

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        saveTransaction: transaction => {
            dispatch(viewTransaction(transaction));
        },
        hideError: () => {
            dispatch(hideNotification());
        },
        resetForm: () => {
            dispatch(reset('blockSearch'));
        }
    };
};

/* istanbul ignore next */
const BlockSearchForm = reduxForm({
    form: 'blockSearch',
    asyncValidate,
    enableReinitialize: true,
    shouldAsyncValidate: utilities.shouldAsyncValidate
})(BlockSearch);

/* istanbul ignore next */
export default connect(null, mapDispatchToProps, utilities.merge)(BlockSearchForm);
