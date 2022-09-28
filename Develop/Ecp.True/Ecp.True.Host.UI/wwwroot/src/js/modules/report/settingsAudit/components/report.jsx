import React from 'react';
import { connect } from 'react-redux';
import { settingsAuditReportFilterBuilder } from '../filterBuilder';
import { toPbiReport } from '../../../../common/components/reports/pbiReport.jsx';
import { setBackNavigation } from '../actions';
import { navigationService } from '../../../../common/services/navigationService';
import { routerActions } from '../../../../common/router/routerActions';
import { utilities } from '../../../../common/services/utilities';

export class SettingsAudit extends React.Component {
    constructor() {
        super();
        this.onReturn = this.onReturn.bind(this);
        routerActions.configure('backToSettingsAudit', this.onReturn);
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
        const pbiFilters = settingsAuditReportFilterBuilder.build(this.props.filters);
        const ReportComponent = toPbiReport(this.props.filters.reportType, pbiFilters);
        return (
            <ReportComponent />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        filters: state.report.settingsAuditReport.filters
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        setBackNavigation: () => {
            dispatch(setBackNavigation());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(SettingsAudit);
