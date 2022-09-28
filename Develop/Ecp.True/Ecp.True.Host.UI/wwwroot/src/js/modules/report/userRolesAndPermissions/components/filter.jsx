import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import { required } from 'redux-form-validators';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { navigationService } from '../../../../common/services/navigationService';
import { constants } from '../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';
import { dateService } from '../../../../common/services/dateService';
import { optionService } from '../../../../common/services/optionService';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { inputSelect } from '../../../../common/components/formControl/formControl.jsx';
import { openMessageModal, closeModal } from '../../../../common/actions';
import { requestUserRolePermissionReport } from '../actions';

export class UserRolesAndPermissionsFilter extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
    }

    onSubmit(formValues) {
        const reportExecution = {
            startDate: dateService.now(),
            endDate: dateService.now(),
            reportTypeId: constants.ReportType.UserRolesAndPermissions,
            scenarioId: 2,
            name: formValues.reportType.value
        };

        this.props.reportRequest(reportExecution);
    }

    getReportTypes() {
        const reportTypes = optionService.getUserRolesAndPermissionsReportTypes();
        return reportTypes.map(option => (
            { name: resourceProvider.read(option.label), value: option.value }
        ));
    }

    render() {
        const reportOptions = this.getReportTypes();

        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <div className="ep-section ep-section--panel">
                        <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                            <div className="ep-section__body ep-section__body--h">
                                <section className="ep-section__content-w300 m-t-6">
                                    <h2 className="ep-control-group-title">{resourceProvider.read('selectReport')}</h2>
                                    <div className="ep-control-group">
                                        <label id="lbl_userRolePermission_reportType" className="ep-label" htmlFor="dd_userRolePermission_reportType_sel">
                                            {resourceProvider.read('generateReport')}
                                        </label>
                                        <Field id="dd_userRolePermission_reportType" component={inputSelect} name="reportType" options={reportOptions}
                                            getOptionLabel={x => x.name} getOptionValue={x => x.value} inputId="dd_userRolePermission_reportType_sel"
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

    componentDidUpdate(prevProps) {
        if (prevProps.receiveReportToggler !== this.props.receiveReportToggler) {
            const opts = {
                title: resourceProvider.read('confirmModalUserRoleTitle'),
                canCancel: true,
                cancelActionTitle: 'toClose',
                acceptActionTitle: 'confirmModalReportButton',
                acceptAction: () => {
                    this.props.redirect();
                }
            };

            this.props.openConfirmModal(resourceProvider.read('confirmModalReportMessage'), opts);
        }
        if (prevProps.failureReportToggler !== this.props.failureReportToggler) {
            this.props.confirm(resourceProvider.read('systemErrorMessage'), resourceProvider.read('error'), false);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        form: ownProps.type,
        receiveReportToggler: state.report.userRolesAndPermissions.receiveReportToggler,
        failureReportToggler: state.report.userRolesAndPermissions.failureReportToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        confirm: (message, title, canCancel) => {
            dispatch(openMessageModal(message, {
                title: title,
                canCancel: canCancel
            }));
        },
        reportRequest: reportExecution => {
            dispatch(requestUserRolePermissionReport(reportExecution));
        },
        openConfirmModal: (title, options) => {
            dispatch(openMessageModal(title, options));
        },
        redirect: () => {
            dispatch(closeModal());
            navigationService.navigateToModule('generatedsupplychainreport/manage');
        }
    };
};

/* istanbul ignore next */
const UserRolesAndPermissionsFilterForm = reduxForm()(UserRolesAndPermissionsFilter);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(UserRolesAndPermissionsFilterForm);
