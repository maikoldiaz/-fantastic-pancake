import React from 'react';
import { connect } from 'react-redux';
import { nodeStatusReportSaveReportFilter, nodeStatusReportRequestFinalTicket } from '../actions';
import NodeFilter from '../../../../common/components/filters/nodeFilter.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { navigationService } from '../../../../common/services/navigationService';
import { constants } from '../../../../common/services/constants';
import { dateService } from '../../../../common/services/dateService';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { nodeFilterConfigService } from '../../../../common/services/nodeFilterConfigService';

class NodeStatusReport extends React.Component {
    constructor() {
        super();
        this.filterElements = this.filterElements.bind(this);
        this.onSubmitFilter = this.onSubmitFilter.bind(this);
        this.getTicket = this.getTicket.bind(this);
        this.validateDateRange = this.validateDateRange.bind(this);
        this.getStartDateProps = this.getStartDateProps.bind(this);
        this.getEndDateProps = this.getEndDateProps.bind(this);
    }

    filterElements(categoryElements) {
        return categoryElements.filter(x => x.categoryId === constants.Category.Segment && x.isActive && x.isOperationalSegment);
    }

    validateDateRange(initialDate, finalDate) {
        const difference = dateService.getDiff(finalDate, initialDate, 'd');
        const dateRange = systemConfigService.getDefaultNodeStatusReportValidDays();
        if (difference >= dateRange) {
            return resourceProvider.readFormat('MAXIMUM_DAYS_FOR_PERIOD_RANGEVALIDATION', [dateRange]);
        }
        return null;
    }

    getTicket(element) {
        if (element) {
            this.props.nodeStatusReportRequestFinalTicket(element.elementId, true);
            this.props.nodeStatusReportRequestFinalTicket(element.elementId, false);
        }
    }

    onSubmitFilter(formValues) {
        const filters = Object.assign({}, formValues, {
            elementName: formValues.element.name,
            initialDate: dateService.convertToColombian(formValues.initialDate),
            finalDate: dateService.convertToColombian(formValues.finalDate),
            reportType: constants.Report.NodeStatusReport
        });
        this.props.nodeStatusReportSaveReportFilter(filters);
        navigationService.navigateTo('view');
    }

    getStartDateProps(selectedElement) {
        const dateProps = {};
        if (this.props) {
            const currentTicket = this.props.startDateTicket;
            if (selectedElement && currentTicket) {
                const ticketEndDate = currentTicket.startDate;
                const date = dateService.format(ticketEndDate);
                const difference = dateService.getDiff(dateService.nowAsString(), date, 'd');
                dateProps.maxDate = dateService.subtract(dateService.now(), difference, 'd').toDate();
            } else {
                dateProps.minDate = dateService.now().toDate();
                dateProps.maxDate = dateService.subtract(dateService.now(), 1, 'd').toDate();
            }
        }
        return dateProps;
    }

    getEndDateProps(selectedElement) {
        const dateProps = {};
        if (this.props) {
            const currentTicket = this.props.endDateTicket;
            if (selectedElement && currentTicket) {
                const ticketEndDate = currentTicket.endDate;
                const date = dateService.format(ticketEndDate);
                const difference = dateService.getDiff(dateService.nowAsString(), date, 'd');
                dateProps.maxDate = dateService.subtract(dateService.now(), difference, 'd').toDate();
            } else {
                dateProps.minDate = dateService.now().toDate();
                dateProps.maxDate = dateService.subtract(dateService.now(), 1, 'd').toDate();
            }
        }
        return dateProps;
    }

    getReportRequest(values) {
        const executionId = systemConfigService.getSessionId();
        const ansConfigurationDays = systemConfigService.getDefaultAnsConfigurationDays();
        const initialDate = dateService.parseToDate(values.initialDate);
        const finalDate = dateService.parseToDate(values.finalDate);
        const data = {
            elementName: values.element.name,
            startDate: initialDate,
            endDate: finalDate,
            ansConfigurationDays,
            executionId
        };
        return data;
    }

    getConfig() {
        const functions = {
            filterElements: this.filterElements,
            getTicket: this.getTicket,
            getStartDateProps: this.getStartDateProps,
            getEndDateProps: this.getEndDateProps,
            validateDateRange: this.validateDateRange,
            onSubmitFilter: this.onSubmitFilter,
            getReportRequest: this.getReportRequest
        };
        return nodeFilterConfigService.getNodeStatusConfig(functions);
    }

    render() {
        return (
            <NodeFilter config={this.getConfig()} {...this.props} />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        startDateTicket: state.report.nodeStatusReport.startDateTicket,
        endDateTicket: state.report.nodeStatusReport.endDateTicket
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        nodeStatusReportRequestFinalTicket: (elementId, start) => {
            dispatch(nodeStatusReportRequestFinalTicket(elementId, start));
        },
        nodeStatusReportSaveReportFilter: formValues => {
            dispatch(nodeStatusReportSaveReportFilter(formValues));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(NodeStatusReport);


