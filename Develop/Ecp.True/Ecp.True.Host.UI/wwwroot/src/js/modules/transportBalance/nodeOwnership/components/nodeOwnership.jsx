import React from 'react';
import { connect } from 'react-redux';
import SummaryGrid from './summaryGrid.jsx';
import NodeDetails from './nodeDetails.jsx';
import { requestNodeOwnershipDetails } from '../actions';
import OwnershipHeader from './ownershipHeader.jsx';
import { utilities } from '../../../../common/services/utilities';

export class NodeOwnership extends React.Component {
    render() {
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <section className="ep-section ep-section--panel ep-section--fluid m-b-5">
                        <OwnershipHeader />
                        <div className="ep-section__body ep-section__body--hf">
                            <SummaryGrid ownershipNodeId={this.props.ownershipNodeId} />
                            <NodeDetails ownershipNodeId={this.props.ownershipNodeId} />
                        </div>
                    </section>
                </div>
            </section>
        );
    }

    componentDidMount() {
        this.props.getOwnershipNodeDetails(this.props.ownershipNodeId);
    }
}

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getOwnershipNodeDetails: ownershipNodeId => {
            dispatch(requestNodeOwnershipDetails(ownershipNodeId));
        }
    };
};

/* istanbul ignore next */
export default connect(null, mapDispatchToProps, utilities.merge)(NodeOwnership);
