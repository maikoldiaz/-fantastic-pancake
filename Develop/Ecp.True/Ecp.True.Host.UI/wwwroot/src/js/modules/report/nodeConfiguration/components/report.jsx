import React from 'react';
import { connect } from 'react-redux';
import { toPbiReport } from '../../../../common/components/reports/pbiReport.jsx';
import { utilities } from '../../../../common/services/utilities';
import { nodeConfigurationFilterBuilder } from '../filterBuilder';
import { navigationService } from '../../../../common/services/navigationService';

export class NodeConfiguration extends React.Component {
    render() {
        if (!this.props.filters) {
            navigationService.navigateTo(`manage`);
            return null;
        }
        const pbiFilters = nodeConfigurationFilterBuilder.build(this.props.filters);
        const ReportComponent = toPbiReport(this.props.filters.reportType, pbiFilters);
        return (
            <ReportComponent />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        filters: state.report.nodeConfigurationReport.filters
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, {}, utilities.merge)(NodeConfiguration);
