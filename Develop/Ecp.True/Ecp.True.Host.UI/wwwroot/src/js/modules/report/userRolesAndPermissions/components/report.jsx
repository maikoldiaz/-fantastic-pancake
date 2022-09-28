import React from 'react';
import { connect } from 'react-redux';
import { toPbiReport } from '../../../../common/components/reports/pbiReport.jsx';
import { userRolesAndPermissionsFilterBuilder } from '../filterBuilder';
import { utilities } from '../../../../common/services/utilities';
import { navigationService } from '../../../../common/services/navigationService';
import { buildUserRolePermissionReportFilter, cleanUserRolePermissionReportFilter } from '../actions';
import { getReportDetails } from '../../reports/actions';

export class UserRolesAndPermissionsReport extends React.Component {
    validationData(validation) {
        if (!validation) {
            navigationService.navigateTo(`manage`);
            return false;
        }
        return true;
    }

    render() {
        if (!this.props.filters) {
            return null;
        }
        const pbiFilters = userRolesAndPermissionsFilterBuilder.build(this.props.filters);
        const ReportComponent = toPbiReport(this.props.filters.reportType, pbiFilters);
        return (
            <ReportComponent />
        );
    }

    componentDidMount() {
        const executionId = navigationService.getParamByName('executionId');
        if (this.validationData(!utilities.isNullOrUndefined(executionId))) {
            this.props.getReportDetails(executionId);
        }
    }

    componentDidUpdate(prevProps) {
        if (prevProps.reportToggler !== this.props.reportToggler) {
            this.props.buildReportFilters(this.props.execution);
        }
    }

    componentWillUnmount() {
        this.props.cleanReportFilter();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        filters: state.report.userRolesAndPermissions.filters,
        execution: state.report.reportExecution.execution,
        reportToggler: state.report.reportExecution.reportToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        cleanReportFilter: () => {
            dispatch(cleanUserRolePermissionReportFilter());
        },
        getReportDetails: executionId => {
            dispatch(getReportDetails(executionId));
        },
        buildReportFilters: execution => {
            dispatch(buildUserRolePermissionReportFilter(execution));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(UserRolesAndPermissionsReport);
