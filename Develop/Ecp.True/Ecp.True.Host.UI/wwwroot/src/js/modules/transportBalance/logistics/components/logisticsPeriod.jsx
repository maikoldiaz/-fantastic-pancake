import React from 'react';
import { connect } from 'react-redux';
import { reduxForm, Field, SubmissionError } from 'redux-form';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { inputDatePicker } from './../../../../common/components/formControl/formControl.jsx';
import { required } from 'redux-form-validators';
import { utilities } from './../../../../common/services/utilities.js';
import { dateService } from './../../../../common/services/dateService.js';
import { hideNotification, showError } from './../../../../common/actions.js';
import { setLogisticsInfo, getLogisticsValidationData } from '../actions';
import { systemConfigService } from './../../../../common/services/systemConfigService';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { constants } from './../../../../common/services/constants';

class LogisticsPeriod extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
    }

    onSubmit(formValues) {
        const startDate = dateService.parseFieldToISOString(formValues.initialDate);
        const endDate = dateService.parseFieldToISOString(formValues.finalDate);
        this.validate(formValues.initialDate, formValues.finalDate, formValues.owner);

        const logisticsInfo = Object.assign({}, formValues, {
            startDate,
            endDate,
            segmentId: this.props.logisticsInfo.segmentId,
            segmentName: this.props.logisticsInfo.segmentName,
            owner: this.props.logisticsInfo.owner,
            node: this.props.logisticsInfo.node
        });

        const ticket = {
            startDate: startDate,
            endDate: endDate,
            categoryElementId: this.props.logisticsInfo.segmentId,
            nodeId: this.props.logisticsInfo.node.nodeId,
            categoryElementName: this.props.logisticsInfo.segmentName,
            ownerId: this.props.logisticsInfo.owner[0].elementId
        };

        this.props.getLogisticsValidationData(ticket);

        this.props.setLogisticsInfo(logisticsInfo);
        this.props.hideError();
    }

    getInitialDateProps() {
        const maxDateProps = {};
        let difference = 1;
        if (utilities.isNullOrUndefined(this.props.lastOwnershipDate)) {
            this.props.hideError();
        } else {
            difference = dateService.getDiff(dateService.nowAsString(), this.props.lastOwnershipDate, 'd');
            maxDateProps.maxDate = dateService.subtract(dateService.now(), difference, 'd').toDate();
        }

        const minDateProps = {};
        if (this.props.lastTicket) {
            difference = dateService.getDiff(dateService.nowAsString(), this.props.lastOwnershipDate, 'd');
            minDateProps.minDate = dateService.subtract(dateService.now(), difference, 'd').toDate();
        }

        return { minDateProps, maxDateProps };
    }

    getFinalDateProps() {
        const minFinalDateProps = {};
        if (!utilities.isNullOrUndefined(this.props.initialDate)) {
            const selectedInitialDate = dateService.formatFromDate(this.props.initialDate);
            const difference = dateService.getDiff(dateService.nowAsString(), selectedInitialDate, 'd');
            minFinalDateProps.minDate = dateService.subtract(dateService.now(), difference, 'd').toDate();
        }

        return minFinalDateProps;
    }

    validate(initialDate, finalDate) {
        if (dateService.parseToDate(finalDate) > dateService.now().toDate()) {
            const error = resourceProvider.read('ENDDATE_BEFORENOWVALIDATION');
            this.props.showError(error);
            throw new SubmissionError({
                _error: error
            });
        }

        if (dateService.parseToDate(initialDate) > dateService.parseToDate(finalDate)) {
            const error = resourceProvider.read('DATES_INCONSISTENT');
            this.props.showError(error);
            throw new SubmissionError({
                _error: error
            });
        }

        const difference = dateService.getDiff(finalDate, initialDate, 'd');
        const dateRange = systemConfigService.getDefaultLogisticsTicketDayDifference();
        if (difference > dateRange) {
            const error = resourceProvider.readFormat('MAXIMUM_LOGISTICS_DATA_DAYS_RANGEVALIDATION',
                [dateRange]);
            this.props.showError(error);
            throw new SubmissionError({
                _error: error
            });
        }
    }

    render() {
        const { minDateProps, maxDateProps } = this.getInitialDateProps();
        const minFinalDateProps = this.getFinalDateProps();
        return (
            <form id="frm_logisticsPeriod" className="ep-form" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                <section className="ep-modal__content">
                    <h2 className="ep-control-group-title">{resourceProvider.read('selectCriteria')}</h2>
                    <div className="ep-control-group">
                        <label id="lbl_logisticsPeriod_initialDate" className="ep-label">{resourceProvider.read('initialDate')}</label>
                        <Field id="dt_logisticsPeriod_initialDate" component={inputDatePicker} name="initialDate" {...minDateProps} {...maxDateProps}
                            onChange={this.onInitialDateSelect}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                    </div>
                    <div className="ep-control-group">
                        <label id="lbl_logisticsPeriod_finalDate" className="ep-label">{resourceProvider.read('finalDate')}</label>
                        <Field id="dt_logisticsPeriod_finalDate" component={inputDatePicker} name="finalDate"
                            {...minFinalDateProps} {...maxDateProps}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('logisticsPeriod',
                    { onAccept: this.props.onWizardNext, acceptText: 'next' })} />
            </form>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.validationDataToggler !== this.props.validationDataToggler) {
            this.props.onNext(this.props.config.wizardName);
        }
    }
}

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    const name = constants.LogisticsType[ownProps.componentType];
    return {
        hideError: () => {
            dispatch(hideNotification());
        },
        showError: message => {
            dispatch(showError(message, true));
        },
        setLogisticsInfo: logisticsInfo => {
            dispatch(setLogisticsInfo(logisticsInfo, name));
        },
        getLogisticsValidationData: ticket => {
            dispatch(getLogisticsValidationData(ticket, name));
        }
    };
};

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    const name = constants.LogisticsType[ownProps.componentType];
    return {
        lastOwnershipDate: state.logistics[name].lastOwnershipDate,
        initialDate: state.logistics[name].initialDate,
        refreshDateToggler: state.logistics[name].refreshDateToggler,
        logisticsInfo: state.logistics[name].logisticsInfo,
        validationDataToggler: state.logistics[name].validationDataToggler
    };
};

/* istanbul ignore next */
const LogisticsPeriodForm = reduxForm({ form: 'logisticsPeriod', enableReinitialize: true })(LogisticsPeriod);
export default connect(mapStateToProps, mapDispatchToProps)(LogisticsPeriodForm);
