import React from 'react';
import { connect } from 'react-redux';
import { navigationService } from './../../../../common/services/navigationService';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { dateService } from './../../../../common/services/dateService';
import RecordsCreatedChart from './recordsCreatedChart.jsx';
import TotalRecordsChart from './totalRecordsChart.jsx';
import { utilities } from '../../../../common/services/utilities';
import { requestTicketDetails, resetTicketDetails } from './../actions';
import { showInfo, hideNotification } from '../../../../common/actions';
import { constants } from '../../../../common/services/constants';

export class TicketDetail extends React.Component {
    constructor() {
        super();
        this.buildHeader = this.buildHeader.bind(this);
    }

    buildHeader() {
        if (utilities.isNotEmpty(this.props.ticketInfo.ticket)) {
            return resourceProvider.read('ticketChartHeader')
                .replace('{0}', dateService.format(this.props.ticketInfo.ticket.startDate, 'DD'))
                .replace('{1}', dateService.format(this.props.ticketInfo.ticket.startDate, ' MMMM YYYY'))
                .replace('{3}', dateService.format(this.props.ticketInfo.ticket.endDate, 'DD'))
                .replace('{4}', dateService.format(this.props.ticketInfo.ticket.endDate, ' MMMM YYYY'))
                .replace('{6}', this.props.ticketInfo.ticket.categoryElement.name);
        }
        return '';
    }

    render() {
        const showChart = utilities.isNotEmpty(this.props.ticketInfo.ticket) && this.props.ticketInfo.ticket.status === constants.Status.Processed;
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <div className="ep-section ep-section--panel">
                        <header className="ep-section__header ep-section__header--h98 p-x-8">
                            <div className=" d-flex d-flex--a-center d-flex--jc-sb full-height">
                                <div className="ep-section__titles">
                                    <h1 id="heading_chart_detail" className="fs-20 fw-sb">{resourceProvider.read('operatingCutResult')}</h1>
                                    <h2 id="heading_chart_identifier" className="fs-16 fw-sb">
                                        <span id="lbl_ticketDetail_identifier" className="fw-r m-r-2">{resourceProvider.read('identifier')}</span>
                                        <span id="lbl_ticketDetail_ticketId">{utilities.isNotEmpty(this.props.ticketInfo) && this.props.ticketInfo.ticket.ticketId}</span>
                                    </h2>
                                </div>
                                <p className="fw-sb fs-20 text-right p-l-4 m-a-0" id="p_ticketDetail_header">{this.buildHeader()}</p>
                            </div>
                        </header>
                        <div className="ep-section__body ep-section__body--f p-a-8">
                            <div className="row">
                                <div className="col-md-3">
                                    <section className="ep-chart-wrap">
                                        <header className="ep-chart-wrap__header">
                                            <h1 id="h1_ticketDetail_records_total" className="ep-chart-wrap__title">{resourceProvider.read('totalRecords')}</h1>
                                        </header>
                                        <article className="ep-chart-wrap__body p-x-1" id="cont_ticketDetail_records_total">
                                            {showChart && <TotalRecordsChart data={this.props.ticketInfo.total} />}
                                            {!showChart &&
                                                <div className="ep-chart-wrap__ph">
                                                    <img id="img_ticketDetail_records_total_pending" src="./../dist/images/ep_piechart_placeholder.png"
                                                        alt={resourceProvider.read('piechart')} />
                                                </div>
                                            }
                                        </article>
                                    </section>
                                </div>
                                <div className="col-md-5">
                                    <section className="ep-chart-wrap">
                                        <header className="ep-chart-wrap__header"><h1 id="h1_ticketDetail_records_processed" className="ep-chart-wrap__title">
                                            {resourceProvider.read('processedRecords')}</h1></header>
                                        <article className="ep-chart-wrap__body" id="cont_ticketDetail_records_processed">
                                            {showChart && <RecordsCreatedChart data={this.props.ticketInfo.processed} />}
                                            {!showChart &&
                                                <div className="ep-chart-wrap__ph">
                                                    <img id="img_ticketDetail_records_processed_pending" src="./../dist/images/ep_barchart_placeholder.png"
                                                        alt={resourceProvider.read('barchart')} />
                                                </div>
                                            }
                                        </article>
                                    </section>
                                </div>
                                <div className="col-md-4">
                                    <section className="ep-chart-wrap">
                                        <header className="ep-chart-wrap__header">
                                            <h1 id="h1_ticketDetail_records_created" className="ep-chart-wrap__title">
                                                {resourceProvider.read('recordsCreated')}
                                            </h1>
                                        </header>
                                        <article className="ep-chart-wrap__body" id="cont_ticketDetail_records_new">
                                            {showChart && <RecordsCreatedChart data={this.props.ticketInfo.generated} />}
                                            {!showChart &&
                                                <div className="ep-chart-wrap__ph">
                                                    <img id="img_ticketDetail_records_new_pending" src="./../dist/images/ep_barchart_placeholder.png"
                                                        alt={resourceProvider.read('barchart')} />
                                                </div>
                                            }
                                        </article>
                                    </section>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        );
    }

    componentDidMount() {
        this.props.requestTicketDetails();
    }

    componentDidUpdate(prevProps) {
        if (prevProps.ticketInfo.dataToggler !== this.props.ticketInfo.dataToggler) {
            if (this.props.ticketInfo.ticket.status === 1) {
                this.props.showNotification();
            } else {
                this.props.hideNotification();
            }
        }
    }

    componentWillUnmount() {
        this.props.resetTicketDetails();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        ticketInfo: state.cutoff.ticketInfo
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        requestTicketDetails: () => {
            dispatch(requestTicketDetails(navigationService.getParamByName('ticketId')));
        },
        resetTicketDetails: () => {
            dispatch(resetTicketDetails());
        },
        showNotification: () => {
            dispatch(showInfo(resourceProvider.read('ticketProcessing')));
        },
        hideNotification: () => {
            dispatch(hideNotification());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(TicketDetail);
