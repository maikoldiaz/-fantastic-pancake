import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dateService } from '../../../../common/services/dateService';

class WizardHeader extends React.Component {
    render() {
        if (!this.props.ready || !this.props.lastTicket) {
            return null;
        }
        return (
            <ul className="ep-wizard__lbls">
                <li className="ep-wizard__lbl m-b-1">
                    <span className="fw-sb">{`${resourceProvider.read('operationalCut')}: `}</span>
                    <span className="text-caps">{dateService.format(this.props.lastTicket.endDate)}</span>
                </li>
                <li className="ep-wizard__lbl">
                    <span className="fw-sb">{`${resourceProvider.read('executionDate')}: `}</span>
                    <span className="text-caps">{dateService.format(this.props.lastTicket.createdDate)}</span>
                </li>
            </ul>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        lastTicket: state.cutoff.operationalCut.lastTicket,
        ready: state.cutoff.operationalCut.ready
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps)(WizardHeader);
