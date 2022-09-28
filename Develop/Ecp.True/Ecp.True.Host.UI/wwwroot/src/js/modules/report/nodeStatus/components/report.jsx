import React from 'react';
import { connect } from 'react-redux';
import { toPbiReport } from '../../../../common/components/reports/pbiReport.jsx';
import { nodeStatusFilterBuilder } from '../filterBuilder';
import { navigationService } from '../../../../common/services/navigationService';
import { utilities } from '../../../../common/services/utilities.js';

export class NodeStatus extends React.Component {
    render() {
        if (!this.props.filters) {
            navigationService.navigateTo(`manage`);
            return null;
        }
        const pbiFilters = nodeStatusFilterBuilder.build(this.props.filters);
        const ReportComponent = toPbiReport(this.props.filters.reportType, pbiFilters);
        return (
            <ReportComponent />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        filters: state.report.nodeStatusReport.filters
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, {}, utilities.merge)(NodeStatus);
