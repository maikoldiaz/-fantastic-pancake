import React from 'react';
import TicketsGrid from '../../cutOff/components/ticketsGrid.jsx';

export default class OperationalDeltasGrid extends React.Component {
    render() {
        return (
            <>
                <TicketsGrid {...this.props} />
            </>
        );
    }
}
