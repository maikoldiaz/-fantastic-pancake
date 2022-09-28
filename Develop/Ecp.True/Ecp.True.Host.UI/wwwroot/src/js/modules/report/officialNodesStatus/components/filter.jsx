import React from 'react';
import { connect } from 'react-redux';
import { Field, Fields, reduxForm, change, untouch } from 'redux-form';
import { required } from 'redux-form-validators';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { navigationService } from '../../../../common/services/navigationService';
import { constants } from '../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';
import { dateService } from '../../../../common/services/dateService';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { inputSelect, InputDateRange } from '../../../../common/components/formControl/formControl.jsx';
import { hideNotification, getCategoryElements, showError, openMessageModal } from '../../../../common/actions';
import { resetOfficialNodeStatusPeriods, getOfficialPeriods, requestOfficialNodeStatusReport, saveOfficialNodeStatusFilter } from '../actions';

export class OfficialNodesStatusReportFilter extends React.Component {
    constructor() {
        super();
        this.officialPeriodValidate = this.officialPeriodValidate.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
    }

    resetOfficialPeriod() {
        this.props.resetOfficialNodeStatusPeriods();
        this.props.resetField('periods');
        this.props.resetField('year');
        if (this.props.segment) {
            this.props.requestOfficialPeriodRange(
                this.props.segment.elementId,
                this.props.officialPeriodsYearsRange
            );
        }
    }

    getSegmentList() {
        return this.props.categoryElements
            .filter(obj => obj.isActive && obj.categoryId === 2);
    }

    officialPeriodValidate(noMonths) {
        if (noMonths.selectedYear && noMonths.selectedMonths.length === 0) {
            this.props.showError(resourceProvider.read('officialDeltaNoPeriodForOneSegment'), resourceProvider.read('officialDeltaCannotContinue'));
        }

        if (noMonths.selectedMonths.length > 0) {
            this.props.hideError();
        }
    }

    saveFilters(formValues) {
        if (Object.keys(formValues.periods).length !== 0) {
            const initialDate = dateService.parseToDate(formValues.periods.start);
            const finalDate = dateService.parseToDate(formValues.periods.end);
            const reportFilters = {
                executionId: systemConfigService.getSessionId(),
                categoryId: formValues.segment.categoryId,
                categoryName: formValues.segment.category.name,
                elementId: formValues.segment.elementId,
                elementName: formValues.segment.name,
                initialDate: dateService.convertToColombian(initialDate),
                finalDate: dateService.convertToColombian(finalDate),
                reportType: constants.Report.OfficialNodeStatusReport
            };
            this.props.saveOfficialNodeStatusFilter(reportFilters);
        }
    }

    sendReportRequest(formValues) {
        const reportExecution = {
            executionId: systemConfigService.getSessionId(),
            elementId: formValues.segment.elementId,
            elementName: formValues.segment.name,
            startDate: dateService.parseToDate(formValues.periods.start, 'MM-DD-YYYY'),
            endDate: dateService.parseToDate(formValues.periods.end, 'MM-DD-YYYY')
        };
        this.props.requestOfficialNodeStatusReport(reportExecution);
    }

    onSubmit(formValues) {
        this.saveFilters(formValues);
        this.sendReportRequest(formValues);
    }

    render() {
        const segmentList = this.getSegmentList();

        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <div className="ep-section ep-section--panel">
                        <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                            <div className="ep-section__body ep-section__body--h">
                                <section className="ep-section__content-w500 m-t-6">
                                    <h2 className="ep-control-group-title">{resourceProvider.read('selectCriteria')}</h2>
                                    <div className="ep-control-group">
                                        <label id="lbl_officialNodesStatus_segment" className="ep-label" htmlFor="dd_officialNodesStatus_segment_sel">{resourceProvider.read('segment')}</label>
                                        <Field id="dd_officialNodesStatus_segment" component={inputSelect} name="segment" options={segmentList}
                                            getOptionLabel={x => x.name} getOptionValue={x => x.elementId} inputId="dd_officialNodesStatus_segment_sel"
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                    </div>
                                    <div className="ep-control-group">
                                        <Fields id="dd_officialNodesStatus_period" component={InputDateRange} names={['periods', 'year']}
                                            dateRange={this.props.officialPeriods.officialPeriods} onYearSelect={this.officialPeriodValidate} defaultYear={this.props.officialPeriods.defaultYear}
                                            dateLbl={resourceProvider.read('period')}
                                            validate={{ periods: [required({ msg: { presence: resourceProvider.read('required') } })] }} />
                                    </div>
                                    <SectionFooter floatRight={false} config={footerConfigService.getAcceptConfig('reportFilter',
                                        { acceptClassName: 'ep-btn', acceptText: 'seeReport' })} />
                                </section>
                            </div>
                        </form>
                    </div>
                </div>
            </section>
        );
    }

    componentDidMount() {
        this.props.getCategoryElements();
    }

    componentDidUpdate(prevProps) {
        if (prevProps.segmentChangeToggler !== this.props.segmentChangeToggler) {
            this.resetOfficialPeriod();
        }
        if (prevProps.receiveOfficialNodeStatusToggler !== this.props.receiveOfficialNodeStatusToggler) {
            navigationService.navigateTo('view');
        }
        if (prevProps.failureOfficialNodeStatusToggler !== this.props.failureOfficialNodeStatusToggler) {
            this.props.showTechnicalError(resourceProvider.read('systemErrorMessage'), resourceProvider.read('error'), false);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        form: ownProps.type,
        categoryElements: state.shared.categoryElements,
        segmentChangeToggler: state.report.officialNodeStatusReport.segmentChangeToggler,
        segment: state.report.officialNodeStatusReport.segment,
        officialPeriods: state.report.officialNodeStatusReport.officialPeriods,
        receiveOfficialNodeStatusToggler: state.report.officialNodeStatusReport.receiveOfficialNodeStatusToggler,
        failureOfficialNodeStatusToggler: state.report.officialNodeStatusReport.failureOfficialNodeStatusToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        resetOfficialNodeStatusPeriods: () => {
            dispatch(resetOfficialNodeStatusPeriods());
        },
        requestOfficialPeriodRange: (segmentId, years) => {
            dispatch(getOfficialPeriods(segmentId, years));
        },
        resetField: (fieldName, value) => {
            dispatch(change('officialNodeStatusReport', fieldName, value));
            dispatch(untouch('officialNodeStatusReport', fieldName));
        },
        hideError: () => {
            dispatch(hideNotification());
        },
        showTechnicalError: (message, title, canCancel) => {
            dispatch(openMessageModal(message, { title: title, canCancel }));
        },
        showError: (message, title) => {
            dispatch(showError(message, false, title));
        },
        requestOfficialNodeStatusReport: reportExecution => {
            dispatch(requestOfficialNodeStatusReport(reportExecution));
        },
        saveOfficialNodeStatusFilter: reportFilter => {
            dispatch(saveOfficialNodeStatusFilter(reportFilter));
        }
    };
};

/* istanbul ignore next */
const officialNodesStatusReportFilterForm = reduxForm()(OfficialNodesStatusReportFilter);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(officialNodesStatusReportFilterForm);
