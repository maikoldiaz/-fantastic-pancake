import React from 'react';
import { connect } from 'react-redux';
import { toPbiReport } from '../../../../common/components/reports/pbiReport.jsx';
import { sentToSapReportFilterBuilder } from '../filterBuilder';
import { navigationService } from '../../../../common/services/navigationService';
import { buildReportFilters } from '../actions';
import { utilities } from '../../../../common/services/utilities';
import { getReportDetails } from '../../reports/actions';

export class SentToSapReport extends React.Component {
    validationData(validation) {
        if (!validation) {
            navigationService.navigateTo(`manage`);
            return false;
        }
        return true;
    }

    render() {
        const executionId = this.getExecutionId();
        if (!this.props.filters || this.props.filters.executionId !== +executionId) {
            return null;
        }
        const pbiFilters = sentToSapReportFilterBuilder.build(this.props.filters);
        const ReportComponent = toPbiReport(this.props.filters.reportType, pbiFilters);
        return (
            <ReportComponent />
        );
    }

    componentDidMount() {
        const executionId = this.getExecutionId();
        if (this.validationData(!utilities.isNullOrUndefined(executionId))) {
            this.props.getReportDetails(executionId);
        }
    }

    componentDidUpdate(prevProps) {
        if (prevProps.reportToggler !== this.props.reportToggler) {
            this.props.buildReportFilters(this.props.execution);
        }
    }

    getExecutionId() {
        return navigationService.getParamByName('executionId');
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        filters: state.report.sentToSapReport.filters,
        execution: state.report.reportExecution.execution,
        reportToggler: state.report.reportExecution.reportToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getReportDetails: executionId => {
            dispatch(getReportDetails(executionId));
        },
        buildReportFilters: execution => {
            dispatch(buildReportFilters(execution));
        }
    };
};


/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(SentToSapReport);
