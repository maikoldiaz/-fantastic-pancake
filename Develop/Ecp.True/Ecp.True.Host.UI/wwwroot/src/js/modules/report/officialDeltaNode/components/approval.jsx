import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { navigationService } from '../../../../common/services/navigationService';
import { openModal, openMessageModal } from '../../../../common/actions.js';

export class Approval extends React.Component {
    render() {
        return null;
    }

    componentDidUpdate(prevProps) {
        if (prevProps.approveToggler !== this.props.approveToggler) {
            if (this.props.sendForApprovalResponse && this.props.sendForApprovalResponse.isValidOfficialDeltaNode) {
                navigationService.navigateToModule('officialdeltapernode/manage');
            } else {
                let errorText = resourceProvider.read('nodeWithoutPredecessorApprovals');
                if (!this.props.sendForApprovalResponse.isApproverExist) {
                    errorText = resourceProvider.read('nodeLevelOneApproverNotFound');
                }
                this.props.showError(errorText);
            }
        }

        if (prevProps.reopenToggler !== this.props.reopenToggler) {
            this.props.showReopenModel();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        sendForApprovalResponse: state.report.officialDeltaNode.sendForApprovalResponse,
        approveToggler: state.report.officialDeltaNode.approveToggler,
        reopenToggler: state.report.officialDeltaNode.reopenToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        showError: content => {
            dispatch(openMessageModal(content, {
                title: resourceProvider.read('nodeApprovalsErrorTitle'),
                canCancel: false
            }));
        },
        showReopenModel: () => {
            dispatch(openModal('reopenDeltaNode'));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(Approval);
