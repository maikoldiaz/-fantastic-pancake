import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import classNames from 'classnames/bind';
import { dataService } from '../services/dataService';
import { navigationService } from '../../../../common/services/navigationService';
import {
    requestUnlockNode,
    acceptUnlockNode,
    requestOwnershipNodeMovementInventoryData,
    requestPublishOwnership,
    onNodePublishing,
    requestSendOwnershipNodeForApproval,
    setIsPublishing
} from '../actions';
import { openMessageModal, closeModal, showError } from '../../../../common/actions';
import { utilities } from '../../../../common/services/utilities';
import { dateService } from '../../../../common/services/dateService';
import { saveCutOffReportFilter } from '../../../report/cutOff/actions';
import { refreshSilent } from '../../../../common/components/grid/actions';

export class OwnershipActions extends React.Component {
    constructor() {
        super();
        this.onPublishSuccess = this.onPublishSuccess.bind(this);
        this.onNodePublish = this.onNodePublish.bind(this);
        this.enableActionsDropdown = this.enableActionsDropdown.bind(this);
        this.onDownload = this.onDownload.bind(this);
    }

    enableActionsDropdown(nodeOwnershipStatus, nodeEditorName) {
        const reportStatusArray = [constants.OwnershipNodeStatus.FAILED, constants.OwnershipNodeStatus.SENT];
        const approvaleStatusArray = [constants.OwnershipNodeStatus.OWNERSHIP, constants.OwnershipNodeStatus.UNLOCKED, constants.OwnershipNodeStatus.PUBLISHED,
            constants.OwnershipNodeStatus.REJECTED, constants.OwnershipNodeStatus.REOPENED, constants.OwnershipNodeStatus.RECONCILED, constants.OwnershipNodeStatus.NOTRECONCILED];

        const enableViewReport = !dataService.compareStatus(nodeOwnershipStatus, reportStatusArray, 'eq', 'or');
        const enablePublishAndUnlock = nodeOwnershipStatus === constants.OwnershipNodeStatus.LOCKED && nodeEditorName === this.props.currentUser;
        const enableSubmitToApproval = (dataService.compareStatus(nodeOwnershipStatus, approvaleStatusArray, 'eq', 'or')) &&
            this.props.ownershipNodeBalanceData && this.props.ownershipNodeBalanceData.length > 0;
        return {
            report: enableViewReport,
            publish: enablePublishAndUnlock,
            submitApproval: enableSubmitToApproval,
            unlock: enablePublishAndUnlock
        };
    }

    onDownload() {
        const nodeDetails = this.props.nodeDetails;
        const data = {
            categoryName: nodeDetails.ticket.categoryElement.category.name,
            elementName: nodeDetails.ticket.categoryElement.name,
            nodeName: nodeDetails.node && nodeDetails.node.name,
            initialDate: dateService.parse(nodeDetails.ticket.startDate),
            finalDate: dateService.parse(nodeDetails.ticket.endDate),
            reportType: constants.Report.WithOwner
        };

        this.props.saveCutOffReportFilter(data);
        navigationService.navigateToModule(`cutoffreport/manage/view`);
    }

    onNodePublish() {
        this.props.onNodePublishing(this.props.ownershipNode.publishingNodeToggler);
    }

    onPublishSuccess() {
        const deletedTransactionIds = [];
        this.props.ownershipNode.nodeMovementInventoryData.forEach(v => {
            if (v.status === constants.Modes.Delete && !deletedTransactionIds.includes(v.transactionId)) {
                deletedTransactionIds.push(v.transactionId);
            }
        });
        const movementInventoryWithStatus = {
            movements: [],
            inventoryOwnerships: [],
            ticketId: this.props.nodeDetails.ticketId,
            deletedTransactionIds: deletedTransactionIds,
            ownershipNodeId: this.props.nodeDetails.ownershipNodeId
        };

        for (const groupedOwnership of dataService.groupByArray(this.props.ownershipNode.nodeMovementInventoryData, 'transactionId')) {
            const modifiedList = groupedOwnership.values.filter(x => x.status === constants.Modes.Update || x.status === constants.Modes.Create || x.status === constants.Modes.Delete);
            if (modifiedList.length > 0) {
                if (modifiedList[0].isMovement) {
                    let movement = dataService.buildMovement(modifiedList, this.props.nodeDetails.ticketId);
                    movement = Object.assign({}, movement, {
                        segmentId: this.props.nodeDetails.ticket !== null ? this.props.nodeDetails.ticket.categoryElementId : -1,
                        classification: 'Movimiento',
                        sourceSystem: 1,
                        scenarioId: 1,
                        period: Object.assign({}, movement.period, { startTime: this.props.nodeDetails.ticket.startDate, endTime: this.props.nodeDetails.ticket.endDate })
                    });
                    movementInventoryWithStatus.movements.push(movement);
                } else {
                    const invOwnerShip = dataService.buildInventory(modifiedList, this.props.nodeDetails.ticketId);
                    movementInventoryWithStatus.inventoryOwnerships.push(invOwnerShip);
                }
            }
        }

        this.props.publishOwnerships(movementInventoryWithStatus);
    }

    onConfirm(message, ownershipNodeId) {
        const opts = {
            canCancel: true,
            acceptAction: [acceptUnlockNode(this.props.unlockNodeToggler),
                refreshSilent('ownershipNodeBalance'),
                requestOwnershipNodeMovementInventoryData(ownershipNodeId)]
        };
        this.props.openConfirmModal(message, opts);
    }

    render() {
        const nodeDetails = this.props.nodeDetails;
        const nodeOwnershipStatus = nodeDetails && nodeDetails.ownershipStatus;
        const nodeEditorName = nodeDetails && nodeDetails.editor;
        const statusFactory = this.enableActionsDropdown(nodeOwnershipStatus, nodeEditorName);
        const ownershipNodeId = nodeDetails && nodeDetails.ownershipNodeId;
        return (
            <>
                <span className="ep-section__header-dd">
                    <div id="dd_actions" className="ep-dropdown">
                        <div className="ep-dropdown__lbl">
                            <span className="ep-dropdown__txt">
                                {resourceProvider.read('actions')}
                            </span>
                            <span className="ep-dropdown__action">
                                <i className="fas fa-angle-down" />
                            </span>
                        </div>
                        <ul className="ep-dropdown__lst" id="">
                            <li className="ep-dropdown__lst-itm">
                                <a onClick={
                                    () =>
                                        statusFactory.report && this.onDownload()
                                } className={classNames('ep-dropdown__lst-lnk', { 'ep-dropdown__lst-lnk--disable': !statusFactory.report })}
                                id="lnk_ownershipNodeDetails_viewReport" disabled={!statusFactory.report}>
                                    <i className="fas fa-file-download m-r-1" />
                                    {resourceProvider.read('seeReport')}
                                </a>
                            </li>
                            <li className="ep-dropdown__lst-itm">
                                <a onClick={() => statusFactory.publish && this.onNodePublish()}
                                    className={classNames('ep-dropdown__lst-lnk', { 'ep-dropdown__lst-lnk--disable': !statusFactory.publish })}
                                    id="lnk_ownershipNodeDetails_publish" disabled={!statusFactory.publish}>
                                    <i className="fas fa-upload m-r-1" />
                                    {resourceProvider.read('publish')}
                                </a>
                            </li>
                            <li className="ep-dropdown__lst-itm">
                                <a onClick={() => statusFactory.submitApproval && this.props.showError(resourceProvider.read('nodeMustBeBalancedToSendForApproval'),
                                    this.props.ownershipNode.totalVolumeControl, this.props.nodeDetails.ownershipNodeId)}
                                className={classNames('ep-dropdown__lst-lnk', { 'ep-dropdown__lst-lnk--disable': !statusFactory.submitApproval })}
                                id="lnk_ownershipNodeDetails_submitToApproval" disabled={!statusFactory.submitApproval}>
                                    <i className="fas fa-file-signature m-r-1" />
                                    {resourceProvider.read('submitToApproval')}
                                </a>
                            </li>
                            <li className="ep-dropdown__lst-itm">
                                <a className={classNames('ep-dropdown__lst-lnk', { 'ep-dropdown__lst-lnk--disable': !statusFactory.unlock })}
                                    id="lnk_ownershipNodeDetails_unlock" disabled={!statusFactory.unlock}
                                    onClick={() => statusFactory.unlock && this.onConfirm(resourceProvider.read('unlockingWillLoseTheUnpublishedChanges'), ownershipNodeId)}>
                                    <i className="fas fa-lock m-r-1" />{resourceProvider.read('unlock')}
                                </a>
                            </li>
                        </ul>
                    </div>
                </span>
            </>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.ownershipNode.publishSuccess !== this.props.ownershipNode.publishSuccess && this.props.ownershipNode.publishSuccess) {
            navigationService.navigateTo('manage');
        }

        if (prevProps.ownershipNode.publishOwnershipToggler !== this.props.ownershipNode.publishOwnershipToggler) {
            this.onNodePublish();
        }

        if (prevProps.ownershipNode.nodeMovementInventoryDataToggler !== this.props.ownershipNode.nodeMovementInventoryDataToggler) {
            this.props.closeConfirmModal();
        }

        if (this.props.isPublishing === false && prevProps.nodeOwnershipPublishSuccessToggler !== this.props.nodeOwnershipPublishSuccessToggler) {
            this.onPublishSuccess();
        }

        if (prevProps.nodeOwnershipPublishFailure !== this.props.nodeOwnershipPublishFailure) {
            this.props.showConcurrencyError();
        }
    }
}
/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        ownershipNode: state.nodeOwnership.ownershipNode,
        nodeDetails: state.nodeOwnership.ownershipNode.nodeDetails,
        currentUser: state.root.context.userId,
        ownershipNodeBalanceData: state.grid.ownershipNodeBalance ? state.grid.ownershipNodeBalance.items : [],
        nodeOwnershipPublishSuccessToggler: state.nodeOwnership.ownershipNode.nodeOwnershipPublishSuccessToggler,
        isPublishing: state.nodeOwnership.ownershipNode.isPublishing,
        nodeOwnershipPublishFailure: state.nodeOwnership.ownershipNode.nodeOwnershipPublishFailure,
        unlockNodeToggler: state.nodeOwnership.ownershipNode.unlockNodeToggler
    };
};


/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        showError: (message, totalVolumeControl, ownershipNodeId) => {
            if (totalVolumeControl !== '0.00') {
                dispatch(showError(message));
            } else {
                dispatch(requestSendOwnershipNodeForApproval(ownershipNodeId));
                navigationService.navigateToModule('ownershipnodes/manage');
            }
        },
        requestUnlockNode: requestUnlockToggler => {
            dispatch(requestUnlockNode(requestUnlockToggler));
        },
        unlockNode: unlockNodeToggler => {
            dispatch(acceptUnlockNode(unlockNodeToggler));
        },
        publishOwnerships: movementInventoryData => {
            dispatch(setIsPublishing());
            dispatch(requestPublishOwnership(movementInventoryData));
        },
        onNodePublishing: publishingNodeToggler => {
            dispatch(onNodePublishing(publishingNodeToggler));
        },
        saveCutOffReportFilter: data => {
            dispatch(saveCutOffReportFilter(data));
        },
        openConfirmModal: (message, opts) => {
            dispatch(openMessageModal(message, opts));
        },
        closeConfirmModal: () => {
            dispatch(closeModal());
        },
        showConcurrencyError: () => {
            dispatch(showError(resourceProvider.read('conflictError'), false));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(OwnershipActions);
