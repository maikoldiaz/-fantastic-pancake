import React from 'react';
import { connect } from 'react-redux';
import NodeFilter from '../../../../common/components/filters/nodeFilter.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { navigationService } from '../../../../common/services/navigationService';
import { settingsAuditReportSaveReportFilter, settingsAuditReportResetReportFilter, resetBackNavigation } from '../actions';
import { dateService } from '../../../../common/services/dateService';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { nodeFilterConfigService } from '../../../../common/services/nodeFilterConfigService';
import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';

export class SettingsAuditReport extends React.Component {
    constructor() {
        super();
        this.onSubmitFilter = this.onSubmitFilter.bind(this);
        this.validateDateRange = this.validateDateRange.bind(this);
    }

    validateDateRange(initialDate, finalDate) {
        const difference = dateService.getDiff(finalDate, initialDate, 'd');
        const dateRange = systemConfigService.getDefaultAuditReportValidDays();
        if (difference >= dateRange) {
            return resourceProvider.readFormat('MAXIMUM_DAYS_FOR_PERIOD_RANGEVALIDATION', [dateRange]);
        }
        return null;
    }

    getStartDateProps() {
        const dateProps = {};
        dateProps.maxDate = dateService.subtract(dateService.now(), 1, 'd').toDate();
        return dateProps;
    }

    getEndDateProps() {
        const dateProps = {};
        dateProps.maxDate = dateService.subtract(dateService.now(), 1, 'd').toDate();
        return dateProps;
    }

    onSubmitFilter(formValues) {
        const filters = {
            initialDate: formValues.initialDate,
            finalDate: formValues.finalDate,
            reportType: constants.Report.SettingsAuditReport
        };
        this.props.saveFilter(filters);
        navigationService.navigateTo('view');
    }

    getConfig() {
        return nodeFilterConfigService.getSettingsAuditConfig(this.validateDateRange, this.onSubmitFilter, this.getStartDateProps, this.getEndDateProps);
    }

    render() {
        return (
            <NodeFilter config={this.getConfig()} {...this.props} />
        );
    }

    componentDidMount() {
        if (this.props.backNavigation) {
            this.props.resetBackNavigation();
            return;
        }
        this.props.resetFilter();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        initialValues: state.report.settingsAuditReport.initialValues,
        backNavigation: state.report.settingsAuditReport.backNavigation
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        saveFilter: filters => {
            dispatch(settingsAuditReportSaveReportFilter(filters));
        },
        resetFilter: () => {
            dispatch(settingsAuditReportResetReportFilter());
        },
        resetBackNavigation: () => {
            dispatch(resetBackNavigation());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(SettingsAuditReport);


