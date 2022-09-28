import React from 'react';
import { connect } from 'react-redux';
import { eventContractReportRequestFinalTicket, eventContractReportSaveReportFilter } from '../actions';
import NodeFilter from '../../../../common/components/filters/nodeFilter.jsx';
import { dateService } from '../../../../common/services/dateService';
import { navigationService } from '../../../../common/services/navigationService';
import { constants } from '../../../../common/services/constants';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { nodeFilterConfigService } from '../../../../common/services/nodeFilterConfigService';

class EventContractReport extends React.Component {
    constructor() {
        super();
        this.filterElements = this.filterElements.bind(this);
        this.onSubmitFilter = this.onSubmitFilter.bind(this);
    }

    filterElements(categoryElements) {
        return categoryElements.filter(x => x.categoryId === 2);
    }

    onSubmitFilter(formValues) {
        const filters = Object.assign({}, formValues, {
            elementName: formValues.element.name,
            nodeName: formValues.node.name,
            nodeId: formValues.node.nodeId,
            reportType: constants.Report.EventContractReport,
            initialDate: dateService.convertToColombian(formValues.initialDate),
            finalDate: dateService.convertToColombian(formValues.finalDate)
        });
        this.props.eventContractReportSaveReportFilter(filters);
        navigationService.navigateTo('view');
    }

    getReportRequest(values) {
        const executionId = systemConfigService.getSessionId();
        const initialDate = dateService.parseToDate(values.initialDate);
        const finalDate = dateService.parseToDate(values.finalDate);
        const data = {
            elementName: values.element.name,
            node: values.node.name,
            startDate: initialDate,
            endDate: finalDate,
            executionId
        };
        return data;
    }

    getConfig() {
        return nodeFilterConfigService.getEventContractConfig(this.filterElements, this.onSubmitFilter, this.getReportRequest);
    }

    render() {
        return (
            <NodeFilter config={this.getConfig()} />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = () => {
    return {
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        eventContractReportRequestFinalTicket: (elementId, ticketTypeId) => {
            dispatch(eventContractReportRequestFinalTicket(elementId, ticketTypeId));
        },
        eventContractReportSaveReportFilter: formValues => {
            dispatch(eventContractReportSaveReportFilter(formValues));
        }
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(EventContractReport);


