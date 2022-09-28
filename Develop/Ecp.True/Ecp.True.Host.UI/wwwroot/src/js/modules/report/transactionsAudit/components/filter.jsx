import React from 'react';
import { connect } from 'react-redux';
import { dateService } from '../../../../common/services/dateService';
import { nodeFilterConfigService } from '../../../../common/services/nodeFilterConfigService';
import NodeFilter from '../../../../common/components/filters/nodeFilter.jsx';
import { optionService } from '../../../../common/services/optionService';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { navigationService } from '../../../../common/services/navigationService';
import { saveTransactionsAuditFilter, onSelectedElement, resetTransactionsAuditFilter, resetBackNavigation } from '../actions';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';

export class TransactionsAuditReport extends React.Component {
    constructor() {
        super();
        this.filterElements = this.filterElements.bind(this);
        this.onSubmitFilter = this.onSubmitFilter.bind(this);
        this.validateDateRange = this.validateDateRange.bind(this);
        this.onSegmentChange = this.onSegmentChange.bind(this);
    }

    onSegmentChange(selectedItem) {
        this.props.onSelectedElement(selectedItem);
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

    getReportTypes() {
        const array = optionService.getTransactionsAuditReportTypes().map(function (option) {
            return { label: resourceProvider.read(option.label), value: option.value };
        });
        return array;
    }

    filterElements(categoryElements) {
        return categoryElements.filter(x => x.categoryId === 2 && x.isActive);
    }

    validateDateRange(initialDate, finalDate) {
        const difference = dateService.getDiff(finalDate, initialDate, 'd');
        const dateRange = systemConfigService.getDefaultAuditReportValidDays();
        if (difference >= dateRange) {
            return resourceProvider.readFormat('MAXIMUM_DAYS_FOR_PERIOD_RANGEVALIDATION', [dateRange]);
        }
        return null;
    }

    onSubmitFilter(formValues) {
        const filters = {
            element: {
                name: formValues.element.name,
                elementId: formValues.element.elementId
            },
            initialDate: formValues.initialDate,
            finalDate: formValues.finalDate,
            reportType: formValues.reportType ? formValues.reportType : constants.Report.MovementAuditReport
        };
        this.props.saveFilter(filters);
        navigationService.navigateTo('view');
    }

    getConfig() {
        const functions = {
            props: this.props,
            filterElements: this.filterElements,
            getStartDateProps: this.getStartDateProps,
            getEndDateProps: this.getEndDateProps,
            onSubmitFilter: this.onSubmitFilter,
            getReportTypes: this.getReportTypes,
            validateDateRange: this.validateDateRange,
            onSegmentChange: this.onSegmentChange
        };
        return nodeFilterConfigService.getTransactionsAuditConfig(functions);
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
        initialDate: state.report.transactionsAudit.initialDate,
        initialValues: state.report.transactionsAudit.initialValues,
        backNavigation: state.report.transactionsAudit.backNavigation
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        saveFilter: filters => {
            dispatch(saveTransactionsAuditFilter(filters));
        },
        onSelectedElement: element => {
            dispatch(onSelectedElement(element));
        },
        resetFilter: () => {
            dispatch(resetTransactionsAuditFilter());
        },
        resetBackNavigation: () => {
            dispatch(resetBackNavigation());
        }
    };
};

export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(TransactionsAuditReport);
