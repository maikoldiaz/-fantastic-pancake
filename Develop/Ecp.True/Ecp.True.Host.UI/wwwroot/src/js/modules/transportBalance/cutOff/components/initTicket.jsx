import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, reset } from 'redux-form';
import { required } from 'redux-form-validators';
import { inputSelect, inputDatePicker } from '../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { requestLastTicket, setTicketInfo, initCutOff } from '../actions';
import { utilities } from '../../../../common/services/utilities';
import { showError, hideNotification, setModuleName, getCategoryElements, showLoader, hideLoader } from '../../../../common/actions';
import { dateService } from '../../../../common/services/dateService';
import { constants } from '../../../../common/services/constants';
import { ticketValidator } from '../services/ticketValidationService';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

export class InitTicket extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
    }

    async onSubmit(formValues) {
        const startDate = dateService.parseFieldToISOString(formValues.initialDate);
        const endDate = dateService.parseFieldToISOString(formValues.finalDate);
        await ticketValidator.validateAsync(this.props, formValues.initialDate, formValues.finalDate, formValues.segment.elementId);
        const ticket = Object.assign({}, formValues, {
            startDate,
            endDate,
            categoryElementId: formValues.segment.elementId,
            categoryElementName: formValues.segment.name,
            ticketTypeId: constants.TicketType.Cutoff,
            posted: true
        });

        this.props.hideError();
        this.props.setTicketInfo(ticket);
        this.props.onNext(this.props.config.wizardName);
    }

    getDateProps() {
        const maxDateProps = {};
        const difference = dateService.getDiff(dateService.nowAsString(), this.props.initialValues.initialDate, 'd');
        if (dateService.parseToDate(this.props.initialValues.initialDate) < dateService.now().toDate()) {
            // Final Date should be 1 day less than current date.
            maxDateProps.maxDate = dateService.subtract(dateService.now(), 1, 'd').toDate();
        } else {
            maxDateProps.maxDate = dateService.subtract(dateService.now(), difference, 'd').toDate();
        }

        const minDateProps = {};
        if (this.props.lastTicket) {
            minDateProps.minDate = dateService.subtract(dateService.now(), difference, 'd').toDate();
        }

        return { minDateProps, maxDateProps };
    }

    render() {
        const { minDateProps, maxDateProps } = this.getDateProps();
        return (
            <div className="ep-section">
                <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                    <div className="ep-section__body ep-section__body--f">
                        <section className="ep-section__content-w500 m-t-6">
                            <h2 className="ep-control-group-title">{resourceProvider.read('initTicketInitialTabHeader')}</h2>
                            <div className="ep-control-group">
                                <label id="lbl_initTicket_segment" className="ep-label" htmlFor="dd_initTicket_segment_sel">{resourceProvider.read('segment')}:</label>
                                <Field id="dd_initTicket_segment" component={inputSelect} name="segment" inputId="dd_initTicket_segment_sel"
                                    options={this.props.operationalSegments} getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                                    validate={[required({ msg: { presence: resourceProvider.read('balanceSegmentRequired') } })]} />
                            </div>
                            <h2 className="ep-control-group-title">{resourceProvider.read('initTicketInitialClosingPeriodHeader')}</h2>
                            <div className="row">
                                <div className="col-md-6">
                                    <div className="ep-control-group">
                                        <label id="lbl_initTicket_initialDate" className="ep-label" htmlFor="dt_initTicket_initialDate">{resourceProvider.read('initialDate')}</label>
                                        <Field id="dt_initTicket_initialDate" component={inputDatePicker} name="initialDate"
                                            disabled={!this.props.ready || this.props.lastTicket} maxDate={dateService.subtract(dateService.now(), 1, 'd').toDate()} />
                                    </div>
                                </div>
                                <div className="col-md-6">
                                    <div className="ep-control-group">
                                        <label id="lbl_initTicket_finalDate" className="ep-label" htmlFor="dt_initTicket_finalDate">{resourceProvider.read('finalDate')}</label>
                                        <Field id="dt_initTicket_finalDate" component={inputDatePicker} {...maxDateProps} {...minDateProps}
                                            disabled={!this.props.ready} name="finalDate" validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                    </div>
                                </div>
                            </div>
                        </section>
                    </div>
                    <SectionFooter floatRight={true} config={footerConfigService.getCommonConfig('initTicket',
                        { onCancel: this.props.resetFields, disableAccept: (this.props.invalid || !this.props.ready), acceptText: 'next', acceptClassName: 'ep-btn' })} />
                </form>
            </div>
        );
    }

    componentDidMount() {
        this.props.initCutOff();
        this.props.getCategoryElements();
    }

    componentDidUpdate(prevProps) {
        if (prevProps.dateChangedToggler !== this.props.dateChangedToggler) {
            this.props.hideError();
        }
        if (prevProps.segmentChangeToggler !== this.props.segmentChangeToggler) {
            if (this.props.segment) {
                this.props.requestLastTicket(this.props.segment.elementId);
            } else {
                this.props.resetFields();
            }
        }
    }

    componentWillUnmount() {
        this.props.resetModuleName();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        initialValues: state.cutoff.operationalCut.ticket,
        operationalSegments: state.shared.operationalSegments,
        systemConfig: state.root.systemConfig,
        initToggler: state.cutoff.operationalCut.initToggler,
        lastTicket: state.cutoff.operationalCut.lastTicket,
        ready: state.cutoff.operationalCut.ready,
        segmentChangeToggler: state.cutoff.operationalCut.segmentChangeToggler,
        dateChangedToggler: state.cutoff.operationalCut.dateChangedToggler,
        segment: state.cutoff.operationalCut.segment
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        requestLastTicket: segmentId => {
            dispatch(requestLastTicket(segmentId));
            dispatch(hideNotification());
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        setTicketInfo: ticket => {
            dispatch(setTicketInfo(ticket));
        },
        showError: message => {
            dispatch(showError(message));
        },
        hideError: () => {
            dispatch(hideNotification());
        },
        initCutOff: () => {
            dispatch(initCutOff());
            dispatch(setModuleName('executionOperationalCut'));
        },
        resetModuleName: () => {
            dispatch(setModuleName(null));
        },
        resetFields: () => {
            dispatch(reset('initTicket'));
            dispatch(initCutOff());
            dispatch(hideNotification());
        },
        showLoader: () => {
            dispatch(showLoader());
        },
        hideLoader: () => {
            dispatch(hideLoader());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(reduxForm({ form: 'initTicket', enableReinitialize: true })(InitTicket));
