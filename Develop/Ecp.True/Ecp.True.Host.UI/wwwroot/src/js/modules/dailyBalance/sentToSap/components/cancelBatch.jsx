import React from 'react';
import { connect } from 'react-redux';
import { openModal } from '../../../../common/actions';
import { resourceProvider } from '../../../../common/services/resourceProvider';

class CancelBatch extends React.Component {
    constructor() {
        super();
    }

    render() {
        return (
            <button className="ep-btn float-r" onClick={() => this.props.cancelBatch(resourceProvider.read('titleCancelBatchConfirmationModal'), this.props.ticketId)}>
                {resourceProvider.read('cancelBatch')}
            </button>
        );
    }
}

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        cancelBatch: title => {
            dispatch(openModal('cancelBatchConfirmation', '', title));
        }
    };
};

/* istanbul ignore next */
export default connect(null, mapDispatchToProps)(CancelBatch);
