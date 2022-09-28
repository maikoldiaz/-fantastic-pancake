import React from 'react';
import { connect } from 'react-redux';
import { toPbiReport } from '../../../../common/components/reports/pbiReport.jsx';
import { officialNodeStatusReportFilterBuilder } from '../filterBuilder';
import { navigationService } from '../../../../common/services/navigationService';
import { utilities } from '../../../../common/services/utilities';

export class OfficialNodesStatusReport extends React.Component {
    render() {
        if (!this.props.filters) {
            navigationService.navigateTo(`manage`);
            return null;
        }
        const pbiFilters = officialNodeStatusReportFilterBuilder.build(this.props.filters);
        const ReportComponent = toPbiReport(this.props.filters.reportType, pbiFilters);
        return (
            <ReportComponent />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        filters: state.report.officialNodeStatusReport.filters
    };
};


/* istanbul ignore next */
export default connect(mapStateToProps, null, utilities.merge)(OfficialNodesStatusReport);
