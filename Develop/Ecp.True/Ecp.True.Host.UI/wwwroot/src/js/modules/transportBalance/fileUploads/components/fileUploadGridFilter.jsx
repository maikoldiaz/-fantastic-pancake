import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, SubmissionError, reset } from 'redux-form';
import Flyout from '../../../../common/components/flyout/flyout.jsx';
import { inputSelect, inputDatePicker } from '../../../../common/components/formControl/formControl.jsx';
import { closeFlyout, saveUploadFileFilter, requestUsers, showError, getSystemTypes } from '../../../../common/actions';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { optionService } from '../../../../common/services/optionService';
import { constants } from '../../../../common/services/constants';
import { required } from 'redux-form-validators';
import { dateService } from '../../../../common/services/dateService';
import { utilities } from '../../../../common/services/utilities';
import { systemConfigService } from './../../../../common/services/systemConfigService';
import FlyoutFooter from '../../../../common/components/footer/flyoutFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { resetFilter, applyFilter } from '../../../../common/components/grid/actions';

export class FileuploadGridFilter extends React.Component {
    constructor() {
        super();

        this.onReset = this.onReset.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
    }

    onReset(close) {
        if (close) {
            this.props.closeFlyout(this.props.name);
        } else {
            this.props.resetForm();
        }
    }

    onSubmit(formValues) {
        const initialDate = dateService.parseToDate(formValues.initialDate);
        const finalDate = dateService.parseToDate(formValues.finalDate);
        const difference = dateService.getDiff(formValues.finalDate, formValues.initialDate, 'd');

        if (initialDate > finalDate) {
            const error = resourceProvider.read('inconsistentDates');
            this.props.showError(error);
            this.props.closeFlyout(this.props.name);
            throw new SubmissionError({
                _error: error
            });
        }

        const dateRange = systemConfigService.getDefaultTransportFileUploadDateRange();
        if (this.isContractOrEvent() && difference > dateRange) {
            const error = resourceProvider.readFormat('invalidContractsAndEventsDateRange',
                [dateRange]);
            this.props.showError(error);
            this.props.closeFlyout(this.props.name);
            throw new SubmissionError({
                _error: error
            });
        }

        const values = Object.assign({}, formValues);
        this.props.saveUploadFileFilter(values);
    }

    isContractOrEvent() {
        return (this.props.systemType === constants.SystemType.CONTRACT || this.props.systemType === constants.SystemType.EVENTS);
    }

    render() {
        return (
            <Flyout name={this.props.name}>
                <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                    <header className="ep-flyout__header">
                        <button id="btn_uploadFileFilter_cancel" type="button" className="ep-btn ep-btn--tr" onClick={() => this.onReset(true)}>
                            {resourceProvider.read('cancel')}
                        </button>
                        <h1 className="ep-flyout__title">{resourceProvider.read('searchCriteria')}</h1>
                        <button id="btn_uploadFileFilter_reset" type="button" onClick={() => this.onReset(false)} className="ep-btn ep-btn--tr" disabled={this.props.gridFilter}>
                            {this.isContractOrEvent() ? resourceProvider.read('clean') : resourceProvider.read('reset')}
                        </button>
                    </header>
                    <section className="ep-flyout__body">
                        <section className="ep-filter ep-filter--br">
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label" htmlFor="dt_uploadFileFilter_initialDate">{resourceProvider.read('initialDate')}</label>
                                <Field id="dt_uploadFileFilter_initialDate"
                                    component={inputDatePicker} name="initialDate" disabled={this.props.gridFilter}
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label" htmlFor="dt_uploadFileFilter_finalDate">{resourceProvider.read('finalDate')}</label>
                                <Field id="dt_uploadFileFilter_finalDate"
                                    component={inputDatePicker} name="finalDate" disabled={this.props.gridFilter}
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label" htmlFor="dd_uploadFileFilter_username_sel">{resourceProvider.read('user')}</label>
                                <Field id="dd_uploadFileFilter_username" component={inputSelect} options={this.props.users} name="username"
                                    getOptionLabel={x => x.name} getOptionValue={x => x.userId} inputId="dd_uploadFileFilter_username_sel"
                                />
                            </div>
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label" htmlFor="dd_uploadFileFilter_state_sel">{resourceProvider.read('state')}</label>
                                <Field id="dd_uploadFileFilter_state" component={inputSelect} options={optionService.getCutoffStateTypes()} name="state"
                                    getOptionLabel={x => resourceProvider.read(x.label)} getOptionValue={x => x.value} inputId="dd_uploadFileFilter_state_sel" />
                            </div>
                            <div className="ep-control-group m-b-3">
                                <label className="ep-label" htmlFor="dd_uploadFileFilter_action_sel">{resourceProvider.read('action')}</label>
                                <Field id="dd_uploadFileFilter_action" component={inputSelect} options={optionService.getExcelActionTypes()} name="action"
                                    getOptionLabel={x => resourceProvider.read(x.label)} getOptionValue={x => x.value} inputId="dd_uploadFileFilter_action_sel" />
                            </div>
                            {this.isContractOrEvent() &&
                                <div className="ep-control-group m-b-3">
                                    <label className="ep-label" htmlFor="dd_uploadFileFilter_type_sel">{resourceProvider.read('fileType')}</label>
                                    <Field id="dd_uploadFileFilter_type" component={inputSelect} options={this.props.fileTypes} name="fileType"
                                        getOptionLabel={x => x.name} getOptionValue={x => x.systemTypeId} inputId="dd_uploadFileFilter_type_sel" />
                                </div>
                            }
                        </section>
                    </section>
                    <FlyoutFooter config={footerConfigService.getFlyoutConfig('fileUploadGridFilter', 'applyFilters')} />
                </form>
            </Flyout>
        );
    }

    componentDidMount() {
        this.props.getUsers();
        this.props.getSystemTypes();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        fileTypes: state.shared.fileTypes,
        users: state.shared.users,
        gridFilter: state.messages.fileRegistration.gridFilter,
        initialValues: state.messages.fileRegistration.initialValues
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        closeFlyout: flyoutName => {
            dispatch(closeFlyout(flyoutName));
        },
        resetForm: () => {
            dispatch(reset('fileUploadGridFilter'));
        },
        resetFilters: value => {
            dispatch(resetFilter('fileUploads', value));
        },
        saveUploadFileFilter: filterValues => {
            dispatch(saveUploadFileFilter('fileUploads', filterValues));
            dispatch(applyFilter('fileUploads', filterValues));
            dispatch(closeFlyout(ownProps.name));
        },
        getUsers: () => {
            dispatch(requestUsers());
        },
        getSystemTypes: () => {
            dispatch(getSystemTypes());
        },
        showError: message => {
            dispatch(showError(message));
        }
    };
};

/* istanbul ignore next */
const uploadGridFilterForm = reduxForm({ form: 'fileUploadGridFilter' })(FileuploadGridFilter);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(uploadGridFilterForm);
