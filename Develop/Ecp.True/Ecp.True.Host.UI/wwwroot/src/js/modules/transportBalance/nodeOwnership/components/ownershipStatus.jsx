import React from 'react';
import { connect } from 'react-redux';
import Tooltip from '../../../../common/components/tooltip/tooltip.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';
import classNames from 'classnames/bind';
import { dateService } from '../../../../common/services/dateService';
import { signalRClient } from '../../../../common/services/signalRClient';
import { showInfo, closeModal, showInfoWithButton, hideNotification, openMessageModal, openModal, intAddComment } from './../../../../common/actions';
import {
    requestNodeOwnershipDetails, requestOwnershipNodeMovementInventoryData, receiveEditorInfo,
    onOwnershipNodePublish, requestTicketReopen, nodePublishSuccess, nodePublishFailure, resetNodeIsPublishing, acceptUnlockNode
} from '../actions';
import { navigationService } from '../../../../common/services/navigationService';
import NavigationPrompt from '../../../../common/components/awareness/navigationPrompt.jsx';
import Awareness from '../../../../common/components/awareness/awareness.jsx';
import { dataService } from '../services/dataService';
import uuid from 'uuid';

class OwnershipStatus extends React.Component {
    constructor() {
        super();

        this.joinGroup = this.joinGroup.bind(this);
        this.leaveGroup = this.leaveGroup.bind(this);
        this.startEdit = this.startEdit.bind(this);
        this.onEditEnd = this.onEditEnd.bind(this);
        this.onRequestUnLock = this.onRequestUnLock.bind(this);
        this.onAcceptUnLockRequest = this.onAcceptUnLockRequest.bind(this);
        this.when = this.when.bind(this);
        this.onPublish = this.onPublish.bind(this);
        this.onNodePublishingRequest = this.onNodePublishingRequest.bind(this);
    }

    joinGroup() {
        signalRClient.connect().then(() => signalRClient.publish('joinGroup', this.props.nodeDetails.ownershipNodeId.toString()));
    }

    leaveGroup() {
        signalRClient.connect().then(() => signalRClient.publish('leaveGroup', this.props.nodeDetails.ownershipNodeId.toString()));
    }

    startEdit() {
        signalRClient.connect().then(() => signalRClient.publish('onEditStart', this.props.nodeDetails.ownershipNodeId.toString()));
    }

    onEditEnd() {
        const ownershipNodeId = this.props.nodeDetails.ownershipNodeId.toString();
        signalRClient.connect().then(() => signalRClient.publish('onEditEnd', ownershipNodeId));
    }

    onRequestUnLock() {
        signalRClient.connect().then(() => signalRClient.publish('onrequestUnLock', this.props.nodeDetails.ownershipNodeId.toString()));
    }

    onAcceptUnLockRequest() {
        const ownershipNodeId = this.props.nodeDetails.ownershipNodeId.toString();
        signalRClient.connect().then(() => {
            signalRClient.publish('leaveGroup', ownershipNodeId);
            signalRClient.publish('onAcceptUnLock', ownershipNodeId);
        });
    }

    onNodePublishingRequest() {
        const body = {
            ownershipNodeId: this.props.nodeDetails.ownershipNodeId.toString(),
            sessionId: uuid()
        };
        signalRClient.connect().then(() => {
            signalRClient.publish('onNodePublishing', body);
        });
    }

    when() {
        if (this.props.movementInventoryOwnershipUpdated) {
            return true;
        }
        return false;
    }

    onPublish() {
        this.props.onPublish(this.props.publishOwnershipToggler);
    }

    render() {
        const nodeOwnershipStatus = this.props.nodeDetails.ownershipStatus;
        const statusIcnClass = dataService.getStatusIconClass(nodeOwnershipStatus);

        if (this.props.nodeDetails && this.props.nodeDetails.ownershipNodeId) {
            this.joinGroup();
        }
        return (
            <>
                <NavigationPrompt blockNavigation={this.props.blockNavigation} when={this.when} onDiscard={this.onEditEnd} onPublish={this.onPublish}>
                    {(isOpen, onDiscard, onCancel, close, onPublish) => (
                        <Awareness isOpen={isOpen} close={close} onCancel={onCancel} onDiscard={onDiscard} onPublish={onPublish} enablePublish={true} />
                    )}
                </NavigationPrompt>

                {nodeOwnershipStatus === constants.OwnershipNodeStatus.LOCKED &&
                    <>
                        <Tooltip body={
                            <>
                                <div className="fw-sb">{this.props.nodeDetails.editor}</div>
                                <i>{resourceProvider.read('edited')}</i>
                            </>} overlayClassName="ep-tooltip--lt" >
                            <div className="ep-actionbar__card ep-actionbar__card--cir ep-actionbar__card--dvdr">
                                <span className="ep-actionbar__card-icn">
                                    <i className="fas fa-user" />
                                </span>
                            </div>
                        </Tooltip>
                        <div className="ep-actionbar__card">
                            <i className="fas fa-history m-r-1" aria-hidden="true" /><span className="fw-sb m-r-2">{resourceProvider.read('lastUpdate')}:</span>
                            <span>{this.props.nodeDetails && dateService.format(this.props.nodeDetails.lastModifiedDate, 'HH:mm DD-MMM-YY')}</span>
                        </div>
                    </>
                }
                <div className="ep-actionbar__card">
                    <span className="fw-sb m-r-2">{resourceProvider.read('state')}:</span>
                    <i className={statusIcnClass} aria-hidden="true" />
                    <span className={classNames({ ['m-r-8']: nodeOwnershipStatus === constants.OwnershipNodeStatus.APPROVED })}>
                        {resourceProvider.read(nodeOwnershipStatus === constants.OwnershipNodeStatus.OWNERSHIP ? 'ownershipoption' : utilities.toLowerCase(nodeOwnershipStatus))}
                    </span>
                    {nodeOwnershipStatus === constants.OwnershipNodeStatus.APPROVED &&
                        <Tooltip body={resourceProvider.read('reopen')} overlayClassName="ep-tooltip--lt">
                            <button className="ep-actionbar__card-btn" onClick={this.props.showCommentModal}>
                                <i className="fas fa-redo m-r-1" aria-hidden="true" />
                            </button>
                        </Tooltip>
                    }
                </div>
            </>
        );
    }

    componentDidMount() {
        const callbacks = [
            {
                operation: 'onConnected',
                fn: this.props.receiveOwnershipNodeDetails
            },
            {
                operation: 'onEditStart',
                fn: this.props.onEditStart
            },
            {
                operation: 'onEditEnd',
                fn: this.props.onEditEnd
            },
            {
                operation: 'onRequestUnLock',
                fn: editorInfo => {
                    const ownershipNodeId = this.props.nodeDetails && this.props.nodeDetails.ownershipNodeId;
                    const confirmOpts = {
                        canCancel: true, cancelActionTitle: 'refuse', title: resourceProvider.read('requestToUnlockTitle'),
                        acceptAction: () => this.props.onRequestUnLockConfirmation(ownershipNodeId)
                    };
                    this.props.onRequestUnLock(editorInfo, confirmOpts);
                }
            },
            {
                operation: 'onAcceptUnLock',
                fn: this.props.onAcceptUnLock
            },
            {
                operation: 'onExiting',
                fn: this.props.onEditRemoteExit
            },
            {
                operation: 'dbException',
                fn: this.props.onPublishFailure
            },
            {
                operation: 'publishSuccess',
                fn: this.props.onPublishSuccess
            }
        ];

        signalRClient.register(callbacks);
    }

    componentWillUnmount() {
        this.leaveGroup();
        this.props.resetIsNodePublishing();
    }

    componentDidUpdate(prevProps) {
        if (this.props.startEditToggler !== prevProps.startEditToggler) {
            this.startEdit();
        }
        if (this.props.endEditToggler !== prevProps.endEditToggler) {
            this.onEditEnd();
        }
        if (this.props.requestUnlockToggler !== prevProps.requestUnlockToggler) {
            this.onRequestUnLock();
        }
        if (this.props.unlockNodeToggler !== prevProps.unlockNodeToggler) {
            this.onAcceptUnLockRequest();
        }
        if (this.props.invokeNotificationButtonToggler !== prevProps.invokeNotificationButtonToggler) {
            this.onRequestUnLock();
        }
        if (this.props.publishingNodeToggler !== prevProps.publishingNodeToggler) {
            this.onNodePublishingRequest();
        }
        if (this.props.commentToggler !== prevProps.commentToggler) {
            this.props.updateTicketStatus(this.props.ownershipNodeId, this.props.comment, this.props.status);
        }
    }
}

const mapStateToProps = state => {
    return {
        nodeDetails: state.nodeOwnership.ownershipNode.nodeDetails,
        message: state.nodeOwnership.ownershipNode.reopenComment ? state.nodeOwnership.ownershipNode.reopenComment.message : null,
        ownershipNodeId: state.nodeOwnership.ownershipNode.nodeDetails.ownershipNodeId,
        status: state.nodeOwnership.ownershipNode.nodeDetails.ownershipStatus,
        reopenSuccess: state.nodeOwnership.ownershipNode.reopenSuccess ? state.nodeOwnership.ownershipNode.reopenSuccess : false,
        currentUser: state.root.context.userId,
        startEditToggler: state.nodeOwnership.ownershipNode.startEditToggler,
        endEditToggler: state.nodeOwnership.ownershipNode.endEditToggler,
        requestUnlockToggler: state.nodeOwnership.ownershipNode.requestUnlockToggler,
        unlockNodeToggler: state.nodeOwnership.ownershipNode.unlockNodeToggler,
        invokeNotificationButtonToggler: state.notificationButton.invokeNotificationButtonToggler,
        blockNavigation: false,
        movementInventoryOwnershipUpdated: state.nodeOwnership.ownershipNode.movementInventoryOwnershipUpdated,
        publishOwnershipToggler: state.nodeOwnership.ownershipNode.publishOwnershipToggler,
        publishingNodeToggler: state.nodeOwnership.ownershipNode.publishingNodeToggler,
        commentToggler: state.addComment.ownershipNode ? state.addComment.ownershipNode.commentToggler : false,
        comment: state.addComment.ownershipNode ? state.addComment.ownershipNode.comment : null
    };
};

const mapDispatchToProps = dispatch => {
    return {
        showInfo: (message, title) => {
            dispatch(showInfo(message, false, title));
        },
        receiveOwnershipNodeDetails: ownershipNodeId => {
            dispatch(requestNodeOwnershipDetails(ownershipNodeId));
        },
        onEditStart: editorInfo => {
            // Notify the user that balance edit status is LOCKED.
            const message = resourceProvider.readFormat('balanceIsBeingEditedMessage', [editorInfo.editor]);
            const title = resourceProvider.read('balanceIsBeingEditedTitle');
            dispatch(showInfoWithButton(message, false, title, 'requestUnlockButtonText'));
            dispatch(requestNodeOwnershipDetails(editorInfo.ownershipNodeId));
            dispatch(receiveEditorInfo(editorInfo));
        },
        onEditEnd: editorInfo => {
            // Notify the user that balance edit status is UNLOCKED and
            // Get latest data for inventory and movement ownership.
            dispatch(requestOwnershipNodeMovementInventoryData(editorInfo.ownershipNodeId));
            dispatch(requestNodeOwnershipDetails(editorInfo.ownershipNodeId));
            dispatch(receiveEditorInfo(editorInfo));
            dispatch(hideNotification());
        },
        onRequestUnLock: (editorInfo, opts) => {
            // Notify the user to asking for unlock.
            const message = resourceProvider.readFormat('requestToUnlockMessage', [editorInfo.editor, editorInfo.nodeName]);
            dispatch(receiveEditorInfo(editorInfo));
            dispatch(openMessageModal(message, opts));
        },
        onRequestUnLockConfirmation: ownershipNodeId => {
            dispatch(acceptUnlockNode());
            dispatch(requestNodeOwnershipDetails(ownershipNodeId));
            dispatch(closeModal());
        },
        onAcceptUnLock: editorInfo => {
            // Notify the user stating some one has requested to unblock this editing.
            const message = resourceProvider.readFormat('unlockRequestAcceptedMessage', [editorInfo.nodeName]);
            const title = resourceProvider.read('unlockRequestAcceptedTitle');
            dispatch(showInfo(message, false, title));
            dispatch(requestNodeOwnershipDetails(editorInfo.ownershipNodeId));
            dispatch(requestOwnershipNodeMovementInventoryData(editorInfo.ownershipNodeId));
            dispatch(receiveEditorInfo(editorInfo));
        },
        onEditRemoteExit: () => {
            // Get latest data for inventory and movement ownership.
            const ticketNodeId = navigationService.getParamByName('ticketNodeId');
            const ownershipNodeId = ticketNodeId.split('_')[0];

            dispatch(requestNodeOwnershipDetails(ownershipNodeId));
            dispatch(requestOwnershipNodeMovementInventoryData(ownershipNodeId));
            dispatch(hideNotification());
        },
        showCommentModal: () => {
            dispatch(openModal('addComment'));
            dispatch(intAddComment('ownershipNode', ''));
        },
        onPublish: publishOwnershipToggler => {
            dispatch(onOwnershipNodePublish(publishOwnershipToggler));
        },
        updateTicketStatus: (ownershipNodeId, message, status) => {
            dispatch(requestTicketReopen(ownershipNodeId, message, status));
        },
        onPublishSuccess: () => {
            dispatch(nodePublishSuccess());
        },
        onPublishFailure: () => {
            dispatch(nodePublishFailure());
        },
        resetIsNodePublishing: () => {
            dispatch(resetNodeIsPublishing());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(OwnershipStatus);
