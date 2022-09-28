import React from 'react';
import { connect } from 'react-redux';
import { toPbiReport } from '../../../../common/components/reports/pbiReport.jsx';
import { cutoffReportFilterBuilder } from '../filterBuilder';
import { navigationService } from '../../../../common/services/navigationService';
import { requestOwnershipNode, parseReportFilters, buildReportFilters, clearCutOffReportFilter } from '../actions';
import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';
import { httpService } from '../../../../common/services/httpService';
import { getReportDetails } from '../../reports/actions';

export class CutOffReport extends React.Component {
    constructor() {
        super();

        this.onDataSelected = this.onDataSelected.bind(this);
        this.navigate = this.navigate.bind(this);
        this.handleView = this.handleView.bind(this);

        this.navigated = false;
        this.nodeName = null;
    }

    navigate(nodeName) {
        const filters = {
            reportType: constants.Report.WithOwner,
            nodeName,
            categoryName: this.props.filters.categoryName,
            elementName: this.props.filters.elementName,
            initialDate: this.props.filters.initialDate,
            finalDate: this.props.filters.finalDate
        };

        const encoded = `?ef=${utilities.base64Encode(filters)}`;
        const url = window.location.href.concat(encoded);
        window.open(url, '_blank');
    }

    onDataSelected(e) {
        if (!utilities.equalsIgnoreCase(this.props.filters.reportType, constants.Report.BalanceOperativeWithPropertyReport) ||
            !utilities.equalsIgnoreCase(e.detail.page.name, 'ReportSection')) {
            return;
        }

        const identities = [];
        e.detail.dataPoints.forEach(element => {
            if (utilities.isArray(element.identity)) {
                element.identity.forEach(i => identities.push(i));
            }
        });

        if (identities.length > 0) {
            const identity = identities.find(a => utilities.equalsIgnoreCase(a.target.column, 'NodeName'));
            const nodeName = identity.equals;

            this.navigated = utilities.equalsIgnoreCase(this.nodeName, nodeName);
            this.nodeName = nodeName;
        }

        if (this.navigated) {
            this.navigated = identities.length > 0;
            this.nodeName = this.navigated === true ? this.nodeName : null;
            return;
        }

        this.navigate(this.nodeName);
        this.navigated = true;
    }

    render() {
        if (!this.props.filters) {
            return null;
        }
        const pbiFilters = cutoffReportFilterBuilder.build(this.props.filters);
        const ReportComponent = toPbiReport(this.props.filters.reportType, pbiFilters);
        return (
            <ReportComponent onDataSelected={this.onDataSelected} />
        );
    }

    handleView() {
        if (!utilities.isNullOrUndefined(httpService.getQueryString('ef'))) {
            this.props.parseFilters(httpService.getQueryString('ef'));
        } else if (!utilities.isNullOrUndefined(httpService.getQueryString('ex'))) {
            this.props.getReportDetails(httpService.getQueryString('ex'));
        } else if (!this.props.filters) {
            navigationService.navigateTo(`manage`);
        }
    }

    componentDidMount() {
        const reportId = navigationService.getParamByName('reportId');
        if (utilities.isNullOrUndefined(reportId)) {
            return;
        }
        if (reportId.startsWith('view')) {
            this.handleView();
        } else {
            this.props.getOwnershipNode(reportId);
        }
    }

    componentDidUpdate(prevProps) {
        if (prevProps.reportToggler !== this.props.reportToggler) {
            this.props.buildReportFilters(this.props.execution);
        }
    }

    componentWillUnmount() {
        this.props.clearFilters();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        filters: state.report.cutOffReport.filters,
        execution: state.report.reportExecution.execution,
        reportToggler: state.report.reportExecution.reportToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getOwnershipNode: nodeId => {
            dispatch(requestOwnershipNode(nodeId));
        },
        parseFilters: encoded => {
            dispatch(parseReportFilters(encoded));
        },
        getReportDetails: executionId => {
            dispatch(getReportDetails(executionId));
        },
        buildReportFilters: execution => {
            dispatch(buildReportFilters(execution));
        },
        clearFilters: () => {
            dispatch(clearCutOffReportFilter());
        }
    };
};


/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(CutOffReport);
