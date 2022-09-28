import React from 'react';
import { connect } from 'react-redux';
import { Field, Fields, reduxForm, change, untouch } from 'redux-form';
import { required } from 'redux-form-validators';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { navigationService } from '../../../../common/services/navigationService';
import { constants } from '../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';
import { dateService } from '../../../../common/services/dateService';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { inputSelect, inputDatePicker, InputDateRange } from '../../../../common/components/formControl/formControl.jsx';
import { hideNotification, getCategoryElements, showError, showLoader, hideLoader, openMessageModal, closeModal } from '../../../../common/actions';
import { resetPeriodSendToSapReport, getOfficialPeriods, requestSendToSapReport } from '../actions';

export class SentToSapReportFilter extends React.Component {
    constructor() {
        super();
        this.officialPeriodValidate = this.officialPeriodValidate.bind(this);
        this.validateData = this.validateData.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
        this.showModal = this.showModal.bind(this);
    }

    isOfficialScenario() {
        return this.props.scenario ? this.props.scenario.value === constants.ScenarioTypeId.official : false;
    }

    getSegmentList() {
        return this.props.categoryElements
            .filter(obj => obj.isActive && obj.categoryId === 2);
    }

    getOwnerList() {
        const owners = this.props.categoryElements
            .filter(a => a.elementId === 27 || a.elementId === 30)
            .reverse();
        return [{ name: 'TODOS', elementId: null }, ...owners];
    }

    getScenarioList() {
        return Object
            .entries(constants.ScenarioTypeId)
            .filter(x => x[1] !== 3)
            .map(x => ({ name: resourceProvider.read(x[0]), value: x[1] }));
    }

    officialPeriodValidate(noMonths) {
        if (noMonths.selectedYear && noMonths.selectedMonths.length === 0) {
            this.props.showError(resourceProvider.read('officialDeltaNoPeriodForOneSegment'), resourceProvider.read('officialDeltaCannotContinue'));
        }

        if (noMonths.selectedMonths.length > 0) {
            this.props.hideError();
        }
    }

    resetOfficialPeriod() {
        if (this.isOfficialScenario()) {
            this.props.resetPeriodSendToSapReport();
            this.props.resetField('periods');
            this.props.resetField('year');
            if (this.props.segment) {
                this.props.requestOfficialPeriodRange(
                    this.props.segment.elementId,
                    this.props.officialPeriodsYearsRange
                );
            }
        }
    }

    validateData(formValues, filterReport) {
        const maxMonthsSendToSapOperativeReport = (this.props.maxMonthsSendToSap) * 30;
        let daysDifference;

        if (this.isOfficialScenario()) {
            daysDifference = dateService.getDiff(formValues.periods.end, formValues.periods.start, 'd');
        } else {
            daysDifference = dateService.getDiff(formValues.finalDate, formValues.initialDate, 'd');
        }

        let flagSubmit = true;
        if (daysDifference > maxMonthsSendToSapOperativeReport) {
            flagSubmit = false;
            this.props.showError(resourceProvider.read('errorValidationMaxMonthSendToSapOperativeReport'), resourceProvider.read('officialDeltaCannotContinue'));
        }

        if (daysDifference < 0) {
            flagSubmit = false;
            this.props.showError(resourceProvider.read('errorValidationInitialDateHigherThanFinishDate'), resourceProvider.read('officialDeltaCannotContinue'));
        }

        if (flagSubmit) {
            this.props.hideError();
            this.props.requestSendToSapReport(filterReport);
        }
    }


    onSubmit(formValues) {
        const filterReport = {
            categoryId: formValues.segment.categoryId,
            elementId: formValues.segment.elementId,
            startDate: this.isOfficialScenario() ? dateService.parseToDate(formValues.periods.start, 'MM-DD-YYYY') : dateService.parseToDate(formValues.initialDate, 'MM-DD-YYYY'),
            endDate: this.isOfficialScenario() ? dateService.parseToDate(formValues.periods.end, 'MM-DD-YYYY') : dateService.parseToDate(formValues.finalDate, 'MM-DD-YYYY'),
            reportTypeId: constants.ReportTypeName.SapBalance,
            name: 'SapBalance',
            ownerId: formValues.owner.elementId,
            scenarioId: formValues.scenario.value
        };


        this.validateData(formValues, filterReport);
    }

    showModal(titleKey, messageKey) {
        this.props.showModal(resourceProvider.read(messageKey), {
            title: resourceProvider.read(titleKey),
            canCancel: true,
            cancelActionTitle: 'toClose',
            acceptActionTitle: 'confirmModalSentToSapButton',
            acceptAction: () => {
                this.props.navigateToReports();
            }
        });
    }

    render() {
        const segmentList = this.getSegmentList();
        const ownerList = this.getOwnerList();
        const scenarioList = this.getScenarioList();

        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <div className="ep-section ep-section--panel">
                        <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                            <div className="ep-section__body ep-section__body--h">
                                <section className="ep-section__content-w500 m-t-6">
                                    <h2 className="ep-control-group-title">{resourceProvider.read('selectCriteria')}</h2>
                                    <div className="ep-control-group">
                                        <label id="lbl_sapReport_segment" className="ep-label" htmlFor="dd_sapReport_segment_sel">{resourceProvider.read('segment')}</label>
                                        <Field id="dd_sapReport_segment" component={inputSelect} name="segment" options={segmentList}
                                            getOptionLabel={x => x.name} getOptionValue={x => x.elementId} inputId="dd_sapReport_segment_sel"
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                    </div>
                                    <div className="ep-control-group">
                                        <label id="lbl_sapReport_scenario" className="ep-label" htmlFor="dd_sapReport_scenario_sel">{resourceProvider.read('stage')}</label>
                                        <Field id="dd_sapReport_scenario" component={inputSelect} name="scenario" options={scenarioList}
                                            getOptionLabel={x => x.name} getOptionValue={x => x.value} inputId="dd_sapReport_scenario_sel"
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                    </div>
                                    {this.isOfficialScenario() && <div className="ep-control-group">
                                        <Fields id="dd_sapReport_period" component={InputDateRange} names={['periods', 'year']}
                                            dateRange={this.props.officialPeriods.officialPeriods} onYearSelect={this.officialPeriodValidate} defaultYear={this.props.officialPeriods.defaultYear}
                                            dateLbl={resourceProvider.read('period')}
                                            validate={{ periods: [required({ msg: { presence: resourceProvider.read('required') } })] }} />
                                    </div>}
                                    {!this.isOfficialScenario() && <>
                                        <div className="ep-control-group">
                                            <label id="lbl_sapReport_initialDate" className="ep-label" htmlFor="dt_sapReport_initialDate">{resourceProvider.read('initialDate')}</label>
                                            <Field id="dt_sapReport_initialDate" component={inputDatePicker} name="initialDate"
                                                maxDate={dateService.subtract(dateService.now(), 1, 'd').toDate()}
                                                validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                        </div>
                                        <div className="ep-control-group">
                                            <label id="lbl_sapReport_finalDate" className="ep-label" htmlFor="dt_sapReport_finalDate">{resourceProvider.read('finalDate')}</label>
                                            <Field id="dt_sapReport_finalDate" component={inputDatePicker} name="finalDate"
                                                isDisabled={!this.props.initialDate}
                                                maxDate={dateService.subtract(dateService.now(), 1, 'd').toDate()}
                                                validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                        </div>
                                    </>}
                                    <div className="ep-control-group">
                                        <label id="lbl_sapReport_owner" className="ep-label" htmlFor="dd_sapReport_owner_sel">{resourceProvider.read('owner')}</label>
                                        <Field id="dd_sapReport_owner" component={inputSelect} name="owner" options={ownerList}
                                            getOptionLabel={x => x.name} getOptionValue={x => x.elementId} inputId="dd_sapReport_owner_sel"
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
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
        if (
            prevProps.scenarioChangeToggler !== this.props.scenarioChangeToggler ||
            prevProps.segmentChangeToggler !== this.props.segmentChangeToggler
        ) {
            this.resetOfficialPeriod();
        }
        if (this.props && this.props.status !== undefined) {
            if (prevProps.reportToggler !== this.props.reportToggler) {
                this.showModal(resourceProvider.read('confirmModalSentToSapTitle'), resourceProvider.read('confirmModalSentToSapMessage'));
            }

            if (prevProps.errorSaveToggler !== this.props.errorSaveToggler) {
                this.showModal(resourceProvider.read('existsModalSentToSapTitle'), resourceProvider.read('existModalSentToSapMessage'));
            }
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        form: ownProps.type,
        categoryElements: state.shared.categoryElements,
        scenario: state.report.sentToSapReport.scenario,
        segment: state.report.sentToSapReport.segment,
        officialPeriods: state.report.sentToSapReport.officialPeriods,
        segmentChangeToggler: state.report.sentToSapReport.segmentChangeToggler,
        scenarioChangeToggler: state.report.sentToSapReport.scenarioChangeToggler,
        maxMonthsSendToSap: state.root.systemConfig.maxMonthsSendToSapOperativeReport,
        errorSaveToggler: state.report.sentToSapReport.errorSaveToggler,
        executionId: state.report.initialBalance.executionId,
        reportToggler: state.report.sentToSapReport.reportToggler,
        status: state.report.sentToSapReport.status
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        resetPeriodSendToSapReport: () => {
            dispatch(resetPeriodSendToSapReport());
        },
        requestOfficialPeriodRange: (segmentId, years) => {
            dispatch(getOfficialPeriods(segmentId, years));
        },
        resetField: (fieldName, value) => {
            dispatch(change('sentToSapReport', fieldName, value));
            dispatch(untouch('sentToSapReport', fieldName));
        },
        hideError: () => {
            dispatch(hideNotification());
        },
        hideLoader: () => {
            dispatch(hideLoader());
        },
        showLoader: () => {
            dispatch(showLoader());
        },
        showError: (message, title) => {
            dispatch(showError(message, false, title));
        },
        requestSendToSapReport: filterReport => {
            dispatch(requestSendToSapReport(filterReport));
        },
        showModal: (message, options) => {
            dispatch(openMessageModal(message, options));
        },
        navigateToReports: () => {
            dispatch(closeModal());
            navigationService.navigateToModule('generatedsupplychainreport/manage');
        }
    };
};

/* istanbul ignore next */
const SentToSapReportFilterForm = reduxForm()(SentToSapReportFilter);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(SentToSapReportFilterForm);
