import React from 'react';
import { connect } from 'react-redux';
import { balanceControlRequestFinalTicket, balanceControlSaveReportFilter } from '../actions';
import NodeFilter from '../../../../common/components/filters/nodeFilter.jsx';
import { dateService } from '../../../../common/services/dateService';
import { navigationService } from '../../../../common/services/navigationService';
import { constants } from '../../../../common/services/constants';
import { nodeFilterConfigService } from '../../../../common/services/nodeFilterConfigService';

class BalanceControlChart extends React.Component {
    constructor() {
        super();
        this.filterElements = this.filterElements.bind(this);
        this.updateEndDate = this.updateEndDate.bind(this);
        this.onSubmitFilter = this.onSubmitFilter.bind(this);
    }

    updateEndDate(selectedItem) {
        if (selectedItem) {
            this.props.balanceControlRequestFinalTicket(selectedItem.elementId, 'Cutoff');
        }
    }

    filterElements(categoryElements) {
        return categoryElements.filter(x => x.categoryId === constants.Category.Segment && x.isActive && x.isOperationalSegment);
    }

    onSubmitFilter(formValues) {
        const filters = Object.assign({}, formValues, {
            elementName: formValues.element.name,
            nodeName: formValues.node.name,
            nodeId: formValues.node.nodeId,
            reportType: constants.Report.BalanceControlChart,
            initialDate: dateService.convertToColombian(formValues.initialDate),
            finalDate: dateService.convertToColombian(formValues.finalDate)
        });
        this.props.balanceControlSaveReportFilter(filters);
        navigationService.navigateTo('view');
    }

    getEndDateProps(selectedElement) {
        const dateProps = {};
        if (this.props) {
            const currentTicket = this.props.ticket;
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

    getConfig() {
        return nodeFilterConfigService.getBalanceControlConfig(this.props, this.filterElements, this.updateEndDate, this.getEndDateProps, this.onSubmitFilter);
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
        ticket: state.report.balanceControlChart.ticket
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        balanceControlRequestFinalTicket: (elementId, ticketTypeId) => {
            dispatch(balanceControlRequestFinalTicket(elementId, ticketTypeId));
        },
        balanceControlSaveReportFilter: formValues => {
            dispatch(balanceControlSaveReportFilter(formValues));
        }
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(BalanceControlChart);

