import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, change } from 'redux-form';
import { required } from 'redux-form-validators';
import { inputSelect, inputDatePicker } from '../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { getCategoryElements, showError, hideNotification, showLoader, hideLoader } from '../../../../common/actions';
import { dateService } from '../../../../common/services/dateService';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { initDeltaTicket, initDeltaSegment, initDeltaStartDate, setDeltaEndDate, requestEndDate, setDeltaTicket, setIsReady } from './../actions';
import { utilities } from '../../../../common/services/utilities';
import { ticketValidator } from '../ticketValidationService';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

export class InitDeltaTicket extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
        this.onSegmentSelection = this.onSegmentSelection.bind(this);
    }

    async onSubmit(formValues) {
        await ticketValidator.validateCutoffDeltaCalculation(this.props, formValues.segment.elementId);
        const startDate = dateService.parseFieldToISOString(formValues.startDate);
        const endDate = dateService.parseFieldToISOString(formValues.endDate);

        const ticket = {
            startDate,
            endDate,
            categoryElementId: formValues.segment.elementId
        };

        this.props.hideError();
        this.props.setDeltaTicketInfo(ticket);
        this.props.onNext(this.props.config.wizardName);
    }

    getStartDate() {
        const currentMonthValidDays = systemConfigService.getCurrentMonthValidDays();
        let startDate = '';
        if (dateService.now().format('D') > currentMonthValidDays) {
            startDate = dateService.startOf('M').toDate();
        } else {
            startDate = dateService.subtract(dateService.startOf('M'), 1, 'M').toDate();
        }
        this.props.initDeltaStartDate(startDate);
    }

    onSegmentSelection(segment) {
        this.props.setIsReady(true);
        if (segment) {
            const segmentId = segment.elementId;
            this.props.initDeltaSegment(segment);
            this.props.getEndDate(segmentId);
            this.props.hideError();
        } else {
            this.props.clearEndDate();
        }
    }

    isValidTicket() {
        return this.props.deltaTicket.endDate && !dateService.isMinDate(this.props.deltaTicket.endDate) && this.props.isReady;
    }

    render() {
        const segments = this.props.operationalSegments;
        const isProceed = this.isValidTicket();
        return (
            <div className="ep-section">
                <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                    <div className="ep-section__body ep-section__body--auto">
                        <h2 className="ep-control-group-title text-center m-t-6">{resourceProvider.read('deltasSelectProcessingInfo')}</h2>
                        <section className="ep-section__content-w300 m-t-6">
                            <div className="ep-control-group">
                                <label id="lbl_initDeltaTicket_segment" className="ep-label" htmlFor="dd_initDeltaTicket_segment_sel">{resourceProvider.read('segment')}:</label>
                                <Field id="dd_initDeltaTicket_segment" component={inputSelect} name="segment" options={segments}
                                    getOptionLabel={x => x.name} getOptionValue={x => x.elementId} inputId="dd_initDeltaTicket_segment_sel"
                                    onChange={selectedItem => this.onSegmentSelection(selectedItem)}
                                    validate={[required({ msg: { presence: resourceProvider.read('Requerido') } })]} />
                            </div>
                            <h2 className="ep-control-group-title">{resourceProvider.read('processingPeriod')}</h2>
                            <div className="ep-control-group">
                                <label id="lbl_initDeltaTicket_startDate" className="ep-label" htmlFor="dt_initDeltaTicket_statDate">{resourceProvider.read('initialDate')}</label>
                                <Field id="dt_initDeltaTicket_statDate" component={inputDatePicker} disabled={true} name="startDate"
                                    validate={[required({ msg: { presence: resourceProvider.read('Requerido') } })]} />
                            </div>
                            <div className="ep-control-group">
                                <label id="lbl_initDeltaTicket_endDate" className="ep-label" htmlFor="dt_initDeltaTicket_endDate">{resourceProvider.read('finalDate')}</label>
                                <Field id="dt_initDeltaTicket_endDate" component={inputDatePicker} disabled={true} name="endDate"
                                    validate={[required({ msg: { presence: resourceProvider.read('Requerido') } })]} />
                            </div>
                            <SectionFooter floatRight={false} config={footerConfigService.getAcceptConfig('initDeltaTicket',
                                { acceptClassName: 'ep-btn', acceptText: 'deltaProcess', disableAccept: (this.props.invalid && !isProceed) })} />
                        </section>
                    </div>
                </form>
            </div>
        );
    }

    componentDidMount() {
        this.props.initDeltaTicket();
        this.props.getCategoryElements();
        this.getStartDate();
    }

    componentDidUpdate(prevProps) {
        if ((prevProps.isFinalDateReceivedToggler !== this.props.isFinalDateReceivedToggler)
            && this.props.deltaTicket && this.props.deltaTicket.endDate && dateService.isMinDate(this.props.deltaTicket.endDate)) {
            this.props.showError(resourceProvider.read(`valMsgOwnershipInProgressForSegment`), resourceProvider.read('noInventoriesMovementsForDeltaValMsgTitle'));
            this.props.changeField('endDate', null);
            this.props.setIsReady(false);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        initialValues: state.operationalDelta.deltaTicket,
        operationalSegments: state.shared.operationalSegments,
        deltaTicket: state.operationalDelta.deltaTicket,
        isFinalDateReceivedToggler: state.operationalDelta.isFinalDateReceivedToggler,
        isReady: state.operationalDelta.isReady
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        getEndDate: segmentId => {
            dispatch(requestEndDate(segmentId));
        },
        initDeltaTicket: () => {
            dispatch(initDeltaTicket());
        },
        initDeltaSegment: segment => {
            dispatch(initDeltaSegment(segment));
        },
        initDeltaStartDate: startDate => {
            dispatch(initDeltaStartDate(startDate));
        },
        setDeltaTicketInfo: deltaTicket => {
            dispatch(setDeltaTicket(deltaTicket));
        },
        showError: (message, title) => {
            dispatch(showError(message, false, title));
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
        changeField: (field, value) => {
            dispatch(change('initDeltaTicket', field, value));
        },
        clearEndDate: () => {
            dispatch(setDeltaEndDate(null));
        },
        setIsReady: isReady => {
            dispatch(setIsReady(isReady));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(reduxForm({ form: 'initDeltaTicket', enableReinitialize: true })(InitDeltaTicket));
