import React from 'react';
import { connect } from 'react-redux';
import { change, untouch, reset } from 'redux-form';
import { utilities } from '../../../../common/services/utilities';
import { nodeFilterConfigService } from '../../../../common/services/nodeFilterConfigService';
import NodeFilter from '../../../../common/components/filters/nodeFilter.jsx';
import {
    saveOfficialInitialBalanceFilter,
    onSelectedElement,
    resetOfficialInitialBalanceFilter,
    saveOfficialInitialBalance,
    requestOfficialInitialBalanceStatus,
    refreshStatus,
    clearStatus,
    clearSelectedNode
} from '../actions';
import { constants } from '../../../../common/services/constants';
import { navigationService } from '../../../../common/services/navigationService';
import {
    showLoader,
    hideLoader,
    resetDateRange,
    showError,
    openMessageModal,
    nodeFilterClearSearchNodes
} from '../../../../common/actions';
import { dateService } from '../../../../common/services/dateService';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { navigateToReportsGrid } from '../../cutOff/actions';

export class OfficialInitialBalanceReportFilter extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
        this.refreshStatus = this.refreshStatus.bind(this);
        this.showModal = this.showModal.bind(this);
    }

    filterElements(categoryElements) {
        return categoryElements.filter(x => x.categoryId === constants.Category.Segment && x.isActive);
    }

    refreshStatus() {
        this.props.refreshStatus();
    }

    onSubmit(formValues) {
        if (Object.keys(formValues.periods).length !== 0) {
            const initialDate = dateService.parseToDate(formValues.periods.start);
            const finalDate = dateService.parseToDate(formValues.periods.end);

            const filters = Object.assign({}, formValues, {
                elementName: formValues.element.name,
                nodeName: formValues.node.name,
                nodeId: formValues.node.nodeId,
                initialDate: dateService.convertToColombian(initialDate),
                finalDate: dateService.convertToColombian(finalDate),
                reportType: constants.Report.OfficialInitialBalanceReport
            });

            const data = {
                categoryId: constants.Category.Segment,
                elementId: formValues.element.elementId,
                nodeId: formValues.node.nodeId,
                startDate: initialDate,
                endDate: finalDate,
                reportTypeId: constants.ReportTypeName.OfficialInitialBalance,
                scenarioId: 'OFFICER',
                name: 'OfficialInitialBalanceReport'
            };

            this.props.saveOfficialInitialBalance(data);
            this.props.saveFilter(filters);
        }
    }

    getConfig() {
        return nodeFilterConfigService.getOfficialInitialBalance(this.filterElements, this.onSubmit);
    }

    showModal(titleKey, messageKey) {
        this.props.showModal(resourceProvider.read(messageKey), {
            canCancel: true,
            cancelActionTitle: 'toClose',
            acceptActionTitle: 'goToGeneratedReports',
            title: resourceProvider.read(titleKey),
            acceptActionAndClose: navigateToReportsGrid(),
            cancelAction: [reset('nodeFilter'), nodeFilterClearSearchNodes(), resetDateRange(), clearSelectedNode(true)],
            closeAction: [reset('nodeFilter'), nodeFilterClearSearchNodes(), resetDateRange(), clearSelectedNode(true)]
        });
    }

    render() {
        return (
            <NodeFilter config={this.getConfig()} {...this.props} selectCriteriaKey="selectCriteriaForOfficialBalance" />
        );
    }

    componentDidMount() {
        this.props.resetFilter();
        this.props.resetDateRange();
        this.props.resetField('periods', []);
    }

    componentDidUpdate(prevProps) {
        if (prevProps.initialBalance.reportToggler !== this.props.initialBalance.reportToggler) {
            this.showModal('generateBalance', 'generateReportRequestReceived');
        }

        if (prevProps.initialBalance.errorSaveToggler !== this.props.initialBalance.errorSaveToggler) {
            this.showModal('existingOperativeBalance', 'reportAlreadyProcessing');
        }

        if (prevProps.initialBalance.navigateToggler !== this.props.initialBalance.navigateToggler) {
            navigationService.navigateToModule('generatedsupplychainreport/manage');
        }

        if (prevProps.clearSelectedNodeToggler !== this.props.clearSelectedNodeToggler) {
            this.props.resetClear();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        initialBalance: state.report.initialBalance,
        reportType: constants.Report.OfficialInitialBalanceReport,
        preventOfficialBalanceRedirect: true,
        dateRange: state.nodeFilter.dateRange,
        executionId: state.report.initialBalance.executionId,
        report: constants.ReportTypeName.OfficialInitialBalance,
        resetReportSelectionToggler: state.report.initialBalance.resetReportSelectionToggler,
        errorSaveToggler: state.report.initialBalance.errorSaveToggler,
        clearSelectedNode: state.report.initialBalance.clearSelectedNode,
        clearSelectedNodeToggler: state.report.initialBalance.clearSelectedNodeToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        saveFilter: filters => {
            dispatch(saveOfficialInitialBalanceFilter(filters));
        },
        onSelectedElement: element => {
            dispatch(onSelectedElement(element));
        },
        resetFilter: () => {
            dispatch(resetOfficialInitialBalanceFilter());
        },
        saveOfficialInitialBalance: data => {
            dispatch(saveOfficialInitialBalance(data));
        },
        requestOfficialInitialBalanceStatus: executionId => {
            dispatch(requestOfficialInitialBalanceStatus(executionId));
        },
        refreshStatus: () => {
            dispatch(refreshStatus());
        },
        clearStatus: () => {
            dispatch(clearStatus());
        },
        showLoader: () => {
            dispatch(showLoader());
        },
        hideLoader: () => {
            dispatch(hideLoader());
        },
        resetField: (fieldName, value) => {
            dispatch(change('nodeFilter', fieldName, value));
            dispatch(untouch('nodeFilter', fieldName));
        },
        resetDateRange: () => {
            dispatch(resetDateRange());
        },
        showError: message => {
            dispatch(showError(message));
        },
        showModal: (message, opts) => {
            dispatch(openMessageModal(message, opts));
        },
        resetClear: () => {
            dispatch(clearSelectedNode(false));
        }
    };
};

export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(OfficialInitialBalanceReportFilter);
