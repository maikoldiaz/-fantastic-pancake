import React from 'react';
import { connect } from 'react-redux';
import { constants } from '../../../../common/services/constants';
import { toPowerAutomate } from '../../../../common/components/powerAutomate/powerAutomate.jsx';

class NodeApproval extends React.Component {
    render() {
        const PowerAutomate = toPowerAutomate(constants.PowerAutomate.Approvals);
        return (
            <PowerAutomate />
        );
    }
}

export default connect(null, null)(NodeApproval);
