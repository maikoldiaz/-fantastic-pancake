import React from 'react';
import { connect } from 'react-redux';
import { toPbiReport } from '../../../../common/components/reports/pbiReport.jsx';
import { reportsFilterBuilder } from '../../../../common/components/filterBuilder/reportsFilterBuilder';
import { navigationService } from '../../../../common/services/navigationService';
import { utilities } from '../../../../common/services/utilities';
import { getReportDetails } from '../../reports/actions.js';
import { buildReportFilters, clearOfficialInitialBalanceFilter } from '../actions.js';

export class OfficialInitialBalanceReport extends React.Component {
    constructor() {
        super();
        this.onReturn = this.onReturn.bind(this);
    }

    onReturn() {
        this.props.setBackNavigation();
        navigationService.navigateTo(`manage`);
    }

    render() {
        if (!this.props.filters) {
            return null;
        }
        const pbiFilters = reportsFilterBuilder.build(this.props.filters);
        const ReportComponent = toPbiReport(this.props.filters.reportType, pbiFilters);
        return (
            <ReportComponent />
        );
    }


    componentDidMount() {
        const executionId = navigationService.getParamByName('executionId');
        if (utilities.isNullOrUndefined(executionId)) {
            return;
        }
        this.props.getReportDetails(executionId);
    }

    componentDidUpdate(prevProps) {
        if (prevProps.reportToggler !== this.props.reportToggler) {
            this.props.buildReportFilters(this.props.execution);
        }
    }

    componentWillUnmount() {
        this.props.clearFilters();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        filters: state.report.initialBalance.filters,
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
        },
        clearFilters: () => {
            dispatch(clearOfficialInitialBalanceFilter());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(OfficialInitialBalanceReport);
