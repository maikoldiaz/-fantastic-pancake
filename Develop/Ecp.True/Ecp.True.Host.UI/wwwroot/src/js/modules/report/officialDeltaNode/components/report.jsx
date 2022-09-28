import React from 'react';
import { connect } from 'react-redux';
import { toPbiReport } from '../../../../common/components/reports/pbiReport.jsx';
import { reportsFilterBuilder } from '../../../../common/components/filterBuilder/reportsFilterBuilder';
import { navigationService } from '../../../../common/services/navigationService';
import { routerActions } from '../../../../common/router/routerActions';
import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';
import { closeModal, enableDisablePageAction, hidePageAction, openModal, refreshAnyReport, showPageAction } from '../../../../common/actions';
import {
    submitForApproval,
    submitForReopen,
    requestDeltaNode,
    resetNodeFilter,
    resetDeltaNodeSource
} from '../actions';
import Approval from './approval.jsx';

export class OfficialDeltaReport extends React.Component {
    constructor() {
        super();
        this.handleView = this.handleView.bind(this);
        this.onReturn = this.onReturn.bind(this);
        this.onApprove = this.onApprove.bind(this);
        this.onReopen = this.onReopen.bind(this);
        this.openManuelMovements = this.openManuelMovements.bind(this);
        routerActions.configure('returnDeltaNodeListing', this.onReturn);
        routerActions.configure('submitForApproval', this.onApprove);
        routerActions.configure('submitForReopen', this.onReopen);
        routerActions.configure('addManualMovementsDeltaNode', this.openManuelMovements);
        this.renderActionLinks = this.renderActionLinks.bind(this);
    }

    onReturn() {
        navigationService.goBack();
    }

    onApprove() {
        this.props.submitForApproval(Object.assign({}, this.props.filters, { segmentId: this.props.filters.elementId }));
    }

    onReopen() {
        this.props.submitForReopen(this.props.filters.deltaNodeId);
    }

    openManuelMovements() {
        this.props.openManuelMovements();
    }

    getAction(nodeStatus) {
        const action = {};
        if (nodeStatus === constants.OwnershipNodeStatusType.SUBMITFORAPPROVAL) {
            action.name = 'submitForApproval';
            action.status = true;
        } else if (nodeStatus === constants.OwnershipNodeStatusType.APPROVED) {
            action.name = 'submitForReopen';
            action.status = false;
        } else if (nodeStatus === constants.OwnershipNodeStatusType.REJECTED
            || nodeStatus === constants.OwnershipNodeStatusType.REOPENED
            || nodeStatus === constants.OwnershipNodeStatusType.DELTA) {
            action.name = 'submitForApproval';
            action.status = false;
        }
        return action;
    }

    viewAddManualMovementsButton(nodeStatus) {
        if (nodeStatus !== constants.OwnershipNodeStatusType.APPROVED && nodeStatus !== constants.OwnershipNodeStatusType.SUBMITFORAPPROVAL) {
            this.props.showPageAction('addManualMovementsDeltaNode');
        }
    }

    renderActionLinks() {
        if (!this.props.filters || !this.props.filters.nodeStatus) {
            return;
        }

        const action = this.getAction(this.props.filters.nodeStatus);
        this.viewAddManualMovementsButton(this.props.filters.nodeStatus);
        this.props.showPageAction(action.name);
        this.props.enableDisablePageAction(action.name, action.status);
    }

    render() {
        if (!this.props.filters) {
            return null;
        }

        const pbiFilters = reportsFilterBuilder.build(this.props.filters);
        const ReportComponent = toPbiReport(this.props.filters.reportType, pbiFilters);
        return (
            <>
                <ReportComponent />
                <Approval />
            </>
        );
    }

    handleView() {
        if (!this.props.filters) {
            navigationService.navigateTo(`manage`);
        }
    }

    componentDidMount() {
        const deltaNodeId = navigationService.getParamByName('deltaNodeId');
        if (utilities.isNullOrUndefined(deltaNodeId)) {
            return;
        }

        if (this.props.source === 'grid') {
            this.props.showPageAction('returnDeltaNodeListing');
        }

        if (deltaNodeId.startsWith('view')) {
            this.handleView();
        } else {
            this.props.getDeltaNode(deltaNodeId);
        }
    }

    componentDidUpdate(prevProps) {
        if (prevProps.reportToggler !== this.props.reportToggler) {
            this.renderActionLinks();
        }
    }

    shouldComponentUpdate(nextProp) {
        if (this.props.isSaveForm !== nextProp.isSaveForm) {
            if (nextProp.isSaveForm) {
                this.props.refreshReport(this.props.filters.reportType);
            }
            return false;
        }
        if (this.props.isOpenModal !== nextProp.isOpenModal) {
            return false;
        }
        return true;
    }

    componentWillUnmount() {
        this.props.resetFilter();
        this.props.resetFromGrid();
        if (this.props.isOpenModal) {
            this.props.closeModal();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        filters: state.report.officialDeltaNode.filters,
        source: state.report.officialDeltaNode.source,
        reportToggler: state.report.officialDeltaNode.reportToggler,
        isSaveForm: state.report.officialDeltaNode.isSaveForm,
        isOpenModal: !!state.modal.isOpen
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        submitForApproval: approvalrequest => {
            dispatch(submitForApproval(approvalrequest));
        },
        submitForReopen: reopenrequest => {
            dispatch(submitForReopen(reopenrequest));
        },
        openManuelMovements: () => {
            dispatch(openModal('addManualMovementsDeltaNode', '', '', 'ep-modal--lg'));
        },
        enableDisablePageAction: (disabledActions, type) => {
            dispatch(enableDisablePageAction(disabledActions, type));
        },
        hidePageAction: hideAction => {
            dispatch(hidePageAction(hideAction));
        },
        showPageAction: showAction => {
            dispatch(showPageAction(showAction));
        },
        getDeltaNode: deltaNodeId => {
            dispatch(requestDeltaNode(deltaNodeId));
        },
        resetFilter: () => {
            dispatch(resetNodeFilter());
        },
        resetFromGrid: () => {
            dispatch(resetDeltaNodeSource());
        },
        refreshReport: reportType => {
            dispatch(refreshAnyReport(reportType));
        },
        closeModal: () => {
            dispatch(closeModal());
        }
    };
};


/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(OfficialDeltaReport);
