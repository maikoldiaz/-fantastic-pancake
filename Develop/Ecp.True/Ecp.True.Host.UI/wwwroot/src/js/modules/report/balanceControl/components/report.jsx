import React from 'react';
import { connect } from 'react-redux';
import { toPbiReport } from '../../../../common/components/reports/pbiReport.jsx';
import { balanceControlChartFilterBuilder } from '../filterBuilder';
import { navigationService } from '../../../../common/services/navigationService';
import { utilities } from '../../../../common/services/utilities';

export class BalanceControl extends React.Component {
    render() {
        if (!this.props.filters) {
            navigationService.navigateTo(`manage`);
            return null;
        }
        const pbiFilters = balanceControlChartFilterBuilder.build(this.props.filters);
        const ReportComponent = toPbiReport(this.props.filters.reportType, pbiFilters);
        return (
            <ReportComponent />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        filters: state.report.balanceControlChart.filters
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, {}, utilities.merge)(BalanceControl);
