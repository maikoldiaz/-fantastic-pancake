import React from 'react';
import { connect } from 'react-redux';
import { toPbiReport } from '../../../../common/components/reports/pbiReport.jsx';
import { reportsFilterBuilder } from '../../../../common/components/filterBuilder/reportsFilterBuilder';
import { navigationService } from '../../../../common/services/navigationService';
import { routerActions } from '../../../../common/router/routerActions';
import { utilities } from '../../../../common/services/utilities';

export class OfficialPendingBalanceReport extends React.Component {
    constructor() {
        super();
        this.onReturn = this.onReturn.bind(this);
        routerActions.configure('backToPendingBalance', this.onReturn);
    }

    onReturn() {
        this.props.setBackNavigation();
        navigationService.navigateTo(`manage`);
    }

    render() {
        if (!this.props.filters) {
            navigationService.navigateTo(`manage`);
            return null;
        }
        const pbiFilters = reportsFilterBuilder.build(this.props.filters);
        const ReportComponent = toPbiReport(this.props.filters.reportType, pbiFilters);
        return (
            <ReportComponent />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        filters: state.report.pendingBalance.filters
    };
};


/* istanbul ignore next */
export default connect(mapStateToProps, utilities.merge)(OfficialPendingBalanceReport);
