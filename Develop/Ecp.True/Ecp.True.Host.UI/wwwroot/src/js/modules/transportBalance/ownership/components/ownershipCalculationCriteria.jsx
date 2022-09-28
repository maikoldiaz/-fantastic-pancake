import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, SubmissionError } from 'redux-form';
import { required } from 'redux-form-validators';
import { inputSelect, inputDatePicker } from './../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { requestOwnershipCalculationDates, setOwnershipCalculationInfo, clearSegmentAndDate } from './../actions';
import { showLoader, hideLoader, showError, hideNotification, getCategoryElements } from './../../../../common/actions';
import { serverValidator } from './../../../../common/services/serverValidator';
import { utilities } from './../../../../common/services/utilities';
import { dateService } from '../../../../common/services/dateService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

export class OwnershipCalculationCriteria extends React.Component {
    constructor() {
        super();
        this.onSelectSegment = this.onSelectSegment.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
    }

    onSelectSegment(val) {
        if (utilities.isNullOrUndefined(val)) {
            return;
        }
        const segmentId = utilities.isNullOrWhitespace(val.elementId) ? [] : val.elementId;
        this.props.getInitialDate(segmentId);
    }

    async validate(categoryElementId, startDate, endDate) {
        const ownership = { categoryElementId, startDate, endDate };

        this.props.showLoader();
        const data = await serverValidator.validateOwnership(ownership);
        this.props.hideLoader();
        if (!data.body) {
            const error = resourceProvider.read('volumetricTicketInprocess');
            const errorTitle = resourceProvider.read('outstandingBalance');
            this.props.showError(error, true, errorTitle);
            throw new SubmissionError({
                _error: error
            });
        }
    }

    async onSubmit(formValues) {
        const categoryElementId = formValues.segment.elementId;
        const categoryElementName = formValues.segment.name;
        const startDate = dateService.parseFieldToISOString(formValues.initialDate);
        const endDate = dateService.parseFieldToISOString(formValues.finalDate);
        await this.validate(categoryElementId, startDate, endDate);
        const ticket = Object.assign({}, {
            startDate,
            endDate,
            categoryElementId: categoryElementId,
            categoryElementName: categoryElementName
        });

        this.props.hideError();
        this.props.setOwnershipCalculationInfo(ticket);
        this.props.onNext(this.props.config.wizardName);
    }

    getDateProps() {
        const dateProps = {};
        if (this.props.initialValues.initialDate && this.props.initialValues.lastCutoffDate) {
            let difference = 1;
            if (dateService.parseToDate(this.props.initialValues.lastCutoffDate) < dateService.now().toDate()) {
                difference = dateService.getDiff(dateService.nowAsString(), this.props.initialValues.lastCutoffDate, 'd');
            }

            dateProps.maxDate = dateService.subtract(dateService.now(), difference, 'd').toDate();
            difference = dateService.getDiff(dateService.nowAsString(), this.props.initialValues.initialDate, 'd');
            dateProps.minDate = dateService.subtract(dateService.now(), difference, 'd').toDate();
        }

        return dateProps;
    }

    isInitialDateValid() {
        const initialDate = this.props.initialValues.initialDate;
        return dateService.isValidDate(initialDate);
    }
    render() {
        const dateProps = this.getDateProps();
        return (
            <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                <section className="ep-modal__content">
                    <div className="ep-section__content-w300 m-a-0">
                        <div className="ep-control-group">
                            <label id="lbl_ownershipCal_segment" className="ep-label" htmlFor="dd_ownershipCal_segment_sel">{resourceProvider.read('segment')}</label>
                            <Field id="dd_ownershipCal_segment" component={inputSelect} name="segment" options={this.props.operationalSegments}
                                getOptionLabel={x => x.name} getOptionValue={x => x.elementId} inputId="dd_ownershipCal_segment_sel"
                                validate={[required({ msg: { presence: resourceProvider.read('required') } })]} onChange={this.onSelectSegment} />
                        </div>
                        <div className="ep-control-group">
                            <label id="lbl_ownershipCal_initialDate" className="ep-label" htmlFor="dt_ownershipCal_initialDate">{resourceProvider.read('initialDate')}</label>
                            <Field id="dt_ownershipCal_initialDate"
                                component={inputDatePicker} name="initialDate" disabled={true} />
                        </div>
                        <div className="ep-control-group">
                            <label id="lbl_ownershipCal_finalDate" className="ep-label" htmlFor="dt_ownershipCal_finalDate">{resourceProvider.read('finalDate')}</label>
                            <Field id="dt_ownershipCal_finalDate" isDisabled={!this.isInitialDateValid()}
                                component={inputDatePicker} name="finalDate" {...dateProps}
                                validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('ownershipCalculationCriteria',
                    { onAccept: this.props.onWizardNext, acceptText: 'next', disableAccept: (!this.isInitialDateValid()) })} />
            </form>
        );
    }

    componentDidMount() {
        this.props.clearSegmentAndDate();
        this.props.getCategoryElements();
    }
    componentDidUpdate(prevProps) {
        if (prevProps.refreshDateToggler !== this.props.refreshDateToggler) {
            this.props.hideError();
            if (!this.isInitialDateValid()) {
                this.props.showError(resourceProvider.read('chainWithoutVolumetricCutoff'), true);
            }
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        operationalSegments: state.shared.operationalSegments,
        initialValues: state.ownership,
        refreshDateToggler: state.ownership.refreshDateToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getInitialDate: segmentId => {
            dispatch(requestOwnershipCalculationDates(segmentId));
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        setOwnershipCalculationInfo: ownershipInfo => {
            dispatch(setOwnershipCalculationInfo(ownershipInfo));
        },
        showLoader: () => {
            dispatch(showLoader());
        },
        hideLoader: () => {
            dispatch(hideLoader());
        },
        showError: (message, showOnModal, title) => {
            dispatch(showError(message, showOnModal, title));
        },
        hideError: () => {
            dispatch(hideNotification());
        },
        clearSegmentAndDate: () => {
            dispatch(clearSegmentAndDate());
        }
    };
};

/* istanbul ignore next */
const OwnershipCalculationCriteriaForm = reduxForm({ form: 'ownershipCalculationCriteria', enableReinitialize: true })(OwnershipCalculationCriteria);

export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(OwnershipCalculationCriteriaForm);
