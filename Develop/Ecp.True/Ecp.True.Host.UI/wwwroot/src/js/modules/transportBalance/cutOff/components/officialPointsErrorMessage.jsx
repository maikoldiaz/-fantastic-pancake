import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dateService } from '../../../../common/services/dateService';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';

export class OfficialPointsErrorMessage extends React.Component {
    render() {
        return (
            <>
                <div className="ep-modal__content" id="cont_confirm_message">
                    <div className="d-flex">
                        <label id="lbl_officialPointsCommentMessage_movementId" className="ep-label d-inline-block m-r-1">{resourceProvider.read('officialPointErrorMovementId')}:</label>
                        <span id="lbl_officialPointsCommentMessage_movementIdValue" className="fw-bold">{this.props.officialPoint.movementId}</span>
                    </div>
                    <div className="d-flex">
                        <label id="lbl_officialPointsCommentMessage_movementType" className="ep-label d-inline-block m-r-1">{resourceProvider.read('movementTypeTransferOperational')}:</label>
                        <span id="lbl_officialPointsCommentMessage_movementTypeName" className="fw-bold">{this.props.officialPoint.movementTypeName}</span>
                    </div>
                    <div className="d-flex">
                        <label id="lbl_officialPointsCommentMessage_operationalDate" className="ep-label d-inline-block m-r-1">{resourceProvider.read('dateOperational')}:</label>
                        <span id="lbl_officialPointsCommentMessage_operationalDateValue" className="fw-bold text-caps">{dateService.format(this.props.officialPoint.operationalDate)}</span>
                    </div>
                    {this.props.officialPoint.errors.length === 0 &&
                        <div className="d-flex m-t-5">
                            <label id="lbl_officialPointsCommentMessage_error" className="ep-label d-inline-block m-r-1">{resourceProvider.read('error')}:</label>
                            <span id="lbl_officialPointsCommentMessage_errorMessage" className="fw-bold">{this.props.officialPoint.errorMessage}</span>
                        </div>
                    }
                    {this.props.officialPoint.errors.length > 0 &&
                        <>
                            <div className="ep-label-wrap">
                                <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('errorRecordCount')}</label>
                                <span id="lbl_officialPointsCommentMessage_errorlength" className="ep-data m-l-2 fw-bold fc-green">{this.props.officialPoint.errors.length}</span>
                            </div>
                            <section className="ep-table-wrap">
                                <div className="ep-table ep-table--smpl ep-table--alt-row ep-table--mh120 ep-table--h170" id="tbl_officialPoints_errorMessages">
                                    <table>
                                        <colgroup>
                                            <col style={{ width: '25%' }} />
                                        </colgroup>
                                        <thead>
                                            <tr>
                                                <th className=" fw-bold">{resourceProvider.read('errorCode')}</th>
                                                <th className=" fw-bold">{resourceProvider.read('errorDescription')}</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {
                                                this.props.officialPoint.errors.map((item, index) => {
                                                    return (
                                                        <tr key={`row-${index}`}>
                                                            <td>{item.errorCode}</td>
                                                            <td>{item.errorDescription}</td>
                                                        </tr>
                                                    );
                                                })
                                            }
                                        </tbody>
                                    </table>
                                </div>
                            </section>
                        </>
                    }
                </div>
                <ModalFooter config={footerConfigService.getAcceptConfig('officialPointsErrorMessage',
                    { closeModal: true, acceptText: 'accept', acceptClassName: 'ep-btn ep-btn--sm ep-btn--primary' })} />
            </>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        officialPoint: state.cutoff.ticketInfo.officialPoint
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps)(OfficialPointsErrorMessage);

