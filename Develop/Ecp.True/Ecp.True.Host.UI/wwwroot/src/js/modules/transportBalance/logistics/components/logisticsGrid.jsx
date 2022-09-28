import React from 'react';
import TicketsGrid from '../../cutOff/components/ticketsGrid.jsx';

export default class LogisticsGrid extends React.Component {
    render() {
        return (
            <TicketsGrid {...this.props} />
        );
    }
}
