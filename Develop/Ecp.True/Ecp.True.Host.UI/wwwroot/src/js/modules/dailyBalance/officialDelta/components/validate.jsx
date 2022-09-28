import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dateService } from '../../../../common/services/dateService';
import { getOfficialDeltaValidationData } from '../actions';
import { openModal } from '../../../../common/actions';
import GridNoData from '../../../../common/components/grid/gridNoData.jsx';
import { utilities } from '../../../../common/services/utilities';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';

export class ValidateOfficialDeltaTicket extends React.Component {
    render() {
        return (
            <div className="ep-section">
                <div className="ep-section__body ep-section__body--f">
                    <section className="ep-section__content-w500 m-t-6">
                        <div className="d-flex">
                            <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('segment')}:</label>
                            <span className="ep-data fw-bold">{this.props.ticket.name}</span>
                        </div>
                        <div className="d-flex">
                            <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('period')}:</label>
                            <span className="ep-data text-caps fw-bold m-r-1">{dateService.capitalize(this.props.ticket.startDate)}</span>
                            <span className="fw-sb fc-label m-r-1">{resourceProvider.read('to')}</span>
                            <span className="ep-data text-caps fw-bold">{dateService.capitalize(this.props.ticket.endDate)}</span>
                        </div>
                        <div className="d-flex m-y-6">
                            <label className="ep-label d-inline-block m-r-1">{resourceProvider.read(this.props.unapprovedNodes.length === 0 ? 'validation' : 'error')}:</label>
                            <span className="fw-bold d-inline-block m-r-1 text-unset nowrap">
                                {resourceProvider.read(this.props.unapprovedNodes.length === 0 ? 'officialDeltaValidation' : 'officialDeltaError')}
                            </span>
                        </div>
                        <div className="ep-label-wrap">
                            <label className="ep-label">{resourceProvider.read('errorRecordCount')}</label>
                            <span className="ep-data m-l-2 fw-600 fc-green">{this.props.unapprovedNodes.length}</span>
                        </div>
                        <section className="ep-table-wrap">
                            <div className="ep-table ep-table--smpl ep-table--alt-row ep-table--mh120 ep-table--h170" id="tbl_validate_unapprovedNodes">
                                <table>
                                    <colgroup>
                                        <col style={{ width: '25%' }} />
                                        <col style={{ width: '25%' }} />
                                        <col style={{ width: '25%' }} />
                                    </colgroup>
                                    <thead>
                                        <tr>
                                            <th className=" fw-bold">{resourceProvider.read('segment')}</th>
                                            <th className=" fw-bold">{resourceProvider.read('nodeName')}</th>
                                            <th className=" fw-bold">{resourceProvider.read('date')}</th>
                                            <th className=" fw-bold">{resourceProvider.read('state')}</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {
                                            this.props.unapprovedNodes.map((item, index) => {
                                                return (
                                                    <tr key={`row-${index}`}>
                                                        <td>{item.segmentName}</td>
                                                        <td>{item.nodeName}</td>
                                                        <td>{dateService.capitalize(item.operationDate)}</td>
                                                        <td>{item.nodeStatus}</td>
                                                    </tr>
                                                );
                                            })
                                        }
                                    </tbody>
                                </table>
                                {this.props.unapprovedNodes.length === 0 && <GridNoData {...this.props} />}
                            </div>
                        </section>
                    </section>
                </div>
                <SectionFooter floatRight={true} config={footerConfigService.getCommonConfig('validateOfficialDeltaTicket',
                    {
                        acceptActions: [openModal('confirmOfficialDelta', '', 'confirmation')], onCancel: this.props.cancel,
                        disableAccept: ((this.props.unapprovedNodes.length > 0) || this.props.isValid), acceptText: 'next', acceptClassName: 'ep-btn'
                    })} />
            </div>
        );
    }

    componentDidMount() {
        if (this.props.ticket.isSon) {
            const ticketInfo = {
                categoryElementId: this.props.ticket.categoryElementId,
                startDate: this.props.ticket.startDate,
                endDate: this.props.ticket.endDate
            };

            this.props.getOfficialDeltaValidationData(ticketInfo);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        ticket: state.officialDelta.ticket,
        unapprovedNodes: state.officialDelta.unapprovedNodes,
        isValid: state.officialDelta.isValid
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        getOfficialDeltaValidationData: ticket => {
            dispatch(getOfficialDeltaValidationData(ticket));
        },
        cancel: () => {
            ownProps.goToStep('officialDelta', 1);
        },
        confirm: () => {
            dispatch(openModal('confirmOfficialDelta', '', 'confirmation'));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(ValidateOfficialDeltaTicket);
