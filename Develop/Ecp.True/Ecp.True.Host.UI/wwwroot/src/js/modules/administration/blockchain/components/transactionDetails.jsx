import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import { dateService } from '../../../../common/services/dateService';
import { requestTransaction, resetTransaction } from '../actions';
import { utilities } from '../../../../common/services/utilities';
import { wizardNextStep } from '../../../../common/actions';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import EventDetails from './eventDetails.jsx';

export class TransactionDetails extends React.Component {
    render() {
        return (
            <>
                <div className="ep-modal__content">
                    <div className="row">
                        <div className="col-xs-8">
                            <h1 className="fw-600 fs-16 text-uppercase m-a-0 m-b-4">{resourceProvider.read('transactionInfo')}</h1>
                            <div className="m-b-3">
                                <label className="ep-label">{resourceProvider.read('blockNumber')}</label>
                                <span className="ep-data" id="lbl_transaction_blockNumber">{this.props.transactionDetails.blockNumber}</span>
                            </div>
                            <div className="m-b-3">
                                <label className="ep-label">{resourceProvider.read('blockHash')}</label>
                                <span className="ep-data" id="lbl_transaction_blockHash">{this.props.transactionDetails.blockHash}</span>
                            </div>
                            <div className="m-b-3">
                                <label className="ep-label">{resourceProvider.read('transactionHash')}</label>
                                <span className="ep-data" id="lbl_transaction_transactionHash">{this.props.transactionDetails.transactionHash}</span>
                            </div>
                            <div className="row">
                                <div className="col-xs-4">
                                    <div className="m-b-3">
                                        <label className="ep-label">{resourceProvider.read('gasUsed')}</label>
                                        <span className="ep-data" id="lbl_transaction_gasUsed">{this.props.transactionDetails.gasUsed}</span>
                                    </div>
                                </div>
                                <div className="col-xs-4">
                                    <div className="m-b-3">
                                        <label className="ep-label">{resourceProvider.read('gasLimit')}</label>
                                        <span className="ep-data" id="lbl_transaction_gasLimit">{this.props.transactionDetails.gasLimit}</span>
                                    </div>
                                </div>
                                <div className="col-xs-4">
                                    <div className="m-b-8">
                                        <label className="ep-label">{resourceProvider.read('createdDate')}</label>
                                        <span className="ep-data text-caps" id="lbl_transaction_createdDate">
                                            {dateService.format(this.props.transactionDetails.transactionTime, 'DD-MMM-YY HH:mm')}
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="col-xs-4">
                            <h1 className="fw-600 fs-16 text-uppercase m-a-0 m-b-4">{resourceProvider.read('contractInfo')}</h1>
                            <div className="m-b-3">
                                <label className="ep-label">{resourceProvider.read('type')}</label>
                                <span className="ep-data" id="lbl_transaction_type">{constants.BlockChainPageType[this.props.transactionDetails.type]}</span>
                            </div>
                            <div className="m-b-3">
                                <label className="ep-label">{resourceProvider.read('contractAddress')}</label>
                                <span className="ep-data" id="lbl_transaction_contractAddress">{this.props.transactionDetails.address}</span>
                            </div>
                            <div className="m-b-3">
                                <label className="ep-label">{resourceProvider.read('identifier')}</label>
                                <span className="ep-data" id="lbl_transaction_identifier">{this.props.transactionDetails.id}</span>
                            </div>
                        </div>
                    </div>
                    <h1 className="fw-600 fs-16 text-uppercase m-a-0 m-b-4">{resourceProvider.read('content')}</h1>
                    <EventDetails content={this.props.transactionDetails.content} maxCols={5} />
                </div>
                <ModalFooter config={footerConfigService.getAcceptConfig('transaction', { closeModal: true, acceptText: 'accept' })} />
            </>
        );
    }

    componentDidMount() {
        this.props.getTransaction(this.props.transaction);
    }

    componentWillUnmount() {
        if (!utilities.isNullOrUndefined(this.props.config)) {
            this.props.wizardSetStep(this.props.config.wizardName);
        }
        this.props.resetTransaction();
    }
}

const mapStateToProps = state => {
    return {
        transaction: state.blockchain.transaction,
        transactionDetails: state.blockchain.transactionDetails
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        getTransaction: transaction => {
            dispatch(requestTransaction(transaction));
        },
        wizardSetStep: name => {
            dispatch(wizardNextStep(name, 1));
            ownProps.goToStep(name, 1);
        },
        resetTransaction: () => {
            dispatch(resetTransaction());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(TransactionDetails);
