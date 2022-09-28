import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';

class ErrorGridComponent extends React.Component {
    render() {
        /* eslint no-underscore-dangle: 0 */
        const props = this.props.row;
        return (
            <div className="d-flex p-a-4 p-t-1 m-l-10">
                <strong>{resourceProvider.read('error')}:</strong>
                <span className="m-l-2">{props._original.error}</span>
            </div>
        );
    }
}

/* istanbul ignore next */
export default connect(null, null)(ErrorGridComponent);
