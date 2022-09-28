import React from 'react';
import ReportsGrid from '../../../report/reports/components/reportsGrid.jsx';

export default class OfficialReportsGrid extends React.Component {
    render() {
        return (
            <ReportsGrid {...this.props} />
        );
    }
}
