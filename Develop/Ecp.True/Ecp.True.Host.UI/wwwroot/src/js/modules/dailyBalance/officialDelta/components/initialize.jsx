import React from 'react';
import { connect } from 'react-redux';
import { Field, Fields, reduxForm, reset, change, untouch } from 'redux-form';
import { required } from 'redux-form-validators';
import { inputSelect, InputDateRange } from '../../../../common/components/formControl/formControl.jsx';
import { utilities } from '../../../../common/services/utilities';
import { initOfficialDelta, setIsValid, setOfficialDeltaTicketInfo, getOfficialPeriods, resetOfficialPeriods } from '../actions';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { hideNotification, setModuleName, getCategoryElements, showLoader, hideLoader, showError, showErrorComponent } from '../../../../common/actions';
import { constants } from '../../../../common/services/constants';
import { dateService } from '../../../../common/services/dateService';
import { ticketValidator } from '../ticketValidationService';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';

export class InitOfficialDeltaTicket extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
        this.validate = this.validate.bind(this);
    }

    async onSubmit(formValues) {
        await ticketValidator.validateOfficialDeltaInProgress(this.props, formValues.segment.elementId);
        const ticket = {
            categoryElementId: formValues.segment.elementId,
            name: formValues.segment.name,
            isSon: formValues.segment.elementId === 0 ? true : this.props.categoryElements.filter(x => x.elementId === formValues.segment.elementId)[0].isOperationalSegment,
            startDate: dateService.parseFieldToISOString(formValues.periods.start),
            endDate: dateService.parseFieldToISOString(formValues.periods.end)
        };
        await ticketValidator.validatePreviousOfficialPeriod(this.props, ticket);
        this.props.hideError();
        this.props.setOfficialDeltaTicketInfo(ticket);
        this.props.onNext(this.props.config.wizardName);
    }

    validate(noMonths) {
        this.props.setIsValid(true);
        if (noMonths.selectedYear && noMonths.selectedMonths.length === 0) {
            this.props.showError(resourceProvider.read('officialDeltaNoPeriod'), resourceProvider.read('officialDeltaCannotContinue'));
        }

        if (noMonths.selectedMonths.length > 0) {
            this.props.hideError();
            this.props.setIsValid(false);
        }
    }

    render() {
        const categoryElements = !utilities.isNullOrUndefined(this.props.categoryElements) ? this.props.categoryElements.filter(x => x.isActive) : [];
        const elements = [{ name: 'Todos', elementId: 0 }, ...categoryElements];
        const dateRange = !utilities.isNullOrUndefined(this.props.dateRange) ? this.props.dateRange.officialPeriods : [];
        const defaultYear = !utilities.isNullOrUndefined(this.props.dateRange) ? this.props.dateRange.defaultYear : null;
        return (
            <div className="ep-section">
                <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                    <div className="ep-section__body ep-section__body--auto">
                        <h2 className="ep-control-group-title text-center m-t-6">{resourceProvider.read('deltasSelectProcessingInfo')}</h2>
                        <section className="ep-section__content-w500 m-t-6">
                            <div className="ep-control-group">
                                <label id="lbl_initOfficialDeltaTicket_segment" className="ep-label" htmlFor="dd_initOfficialDeltaTicket_segment_sel">
                                    {resourceProvider.read('segment')}:</label>
                                <Field id="dd_initOfficialDeltaTicket_segment" component={inputSelect} name="segment" inputId="dd_initOfficialDeltaTicket_segment_sel"
                                    options={elements} getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                            <div className="ep-control-group">
                                <Fields id="dd_initOfficialDeltaTicket_period" component={InputDateRange} names={['periods', 'year']}
                                    dateRange={dateRange} onYearSelect={this.validate} defaultYear={defaultYear}
                                    validate={{ periods: [required({ msg: { presence: resourceProvider.read('required') } })] }} />
                            </div>
                            <SectionFooter floatRight={false} config={footerConfigService.getAcceptConfig('initOfficialDeltaTicket',
                                { disableAccept: this.props.isValid, acceptText: 'deltaProcess', acceptClassName: 'ep-btn' })} />
                        </section>
                    </div>
                </form>
            </div>
        );
    }

    componentDidMount() {
        this.props.initOfficialDelta();
        this.props.getCategoryElements();
        this.props.resetOfficialPeriods();
        this.props.resetField('periods');
        this.props.setIsValid(true);
        this.props.hideError();
    }

    componentWillUnmount() {
        this.props.resetModuleName();
    }

    componentDidUpdate(prevProps) {
        if (this.props.segmentChangeToggler !== prevProps.segmentChangeToggler) {
            this.props.resetOfficialPeriods();
            this.props.resetField('periods');
            this.props.hideError();
            if (!utilities.isNullOrUndefined(this.props.segment)) {
                this.props.getOfficialPeriods(this.props.segment.elementId, systemConfigService.getDefaultOfficialDeltaTicketDateRange());
            }
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        categoryElements: state.shared.groupedCategoryElements[constants.Category.Segment],
        segmentChangeToggler: state.officialDelta.segmentChangeToggler,
        segment: state.officialDelta.segment,
        isValid: state.officialDelta.isValid,
        dateRange: state.officialDelta.periods
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        initOfficialDelta: () => {
            dispatch(initOfficialDelta());
            dispatch(setModuleName('initOfficialDeltaTicket'));
        },
        resetModuleName: () => {
            dispatch(setModuleName(null));
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        resetFields: () => {
            dispatch(reset('initTicket'));
            dispatch(initOfficialDelta());
            dispatch(hideNotification());
        },
        hideError: () => {
            dispatch(hideNotification());
        },
        showLoader: () => {
            dispatch(showLoader());
        },
        hideLoader: () => {
            dispatch(hideLoader());
        },
        setIsValid: valid => {
            dispatch(setIsValid(valid));
        },
        showError: (message, title) => {
            dispatch(showError(message, false, title));
        },
        setOfficialDeltaTicketInfo: ticket => {
            dispatch(setOfficialDeltaTicketInfo(ticket));
        },
        getOfficialPeriods: (segmentId, years) => {
            dispatch(getOfficialPeriods(segmentId, years));
        },
        resetOfficialPeriods: () => {
            dispatch(resetOfficialPeriods());
        },
        resetField: fieldName => {
            dispatch(change('initOfficialDeltaTicket', fieldName, []));
            dispatch(untouch('initOfficialDeltaTicket', fieldName));
        },
        showErrorComponent: (component, title) => {
            dispatch(showErrorComponent(component, false, title));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(reduxForm({ form: 'initOfficialDeltaTicket', enableReinitialize: true })(InitOfficialDeltaTicket));
