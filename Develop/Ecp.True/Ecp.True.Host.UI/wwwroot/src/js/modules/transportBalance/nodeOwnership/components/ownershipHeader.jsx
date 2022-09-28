import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dateService } from '../../../../common/services/dateService';
import OwnershipActions from './ownershipActions.jsx';
import { utilities } from '../../../../common/services/utilities';

export class OwnershipHeader extends React.Component {
    buildDataLabel(nodeDetails, key) {
        return dateService.format(nodeDetails.ticket[key]);
    }

    render() {
        const nodeDetails = this.props.nodeDetails;
        return (
            <header className="ep-section__header br-b-0">
                <ul className="ep-section__header-lbls">
                    <li className="ep-section__header-lbl">
                        <label className="ep-label m-b-0 m-r-2">{resourceProvider.read('node')}:</label><span className="ep-data fw-bold">{nodeDetails.node && nodeDetails.node.name}</span>
                    </li>
                    <li className="ep-section__header-lbl">
                        <label className="ep-label m-b-0 m-r-2">{resourceProvider.read('segment')}:</label>
                        <span className="ep-data fw-bold">{nodeDetails.ticket.categoryElement.name}</span>
                    </li>
                    <li className="ep-section__header-lbl">
                        <label className="ep-label m-b-0 m-r-2">{resourceProvider.read('period')}:</label>
                        <span className="ep-data fw-bold text-caps">{this.buildDataLabel(nodeDetails, 'startDate')}</span>
                        <span className="fw-bold m-x-1">{resourceProvider.read('to')}</span>
                        <span className="ep-data fw-bold text-caps">{this.buildDataLabel(nodeDetails, 'endDate')}</span>
                    </li>
                </ul>
                <OwnershipActions />
            </header>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        nodeDetails: state.nodeOwnership.ownershipNode.nodeDetails
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, null, utilities.merge)(OwnershipHeader);
