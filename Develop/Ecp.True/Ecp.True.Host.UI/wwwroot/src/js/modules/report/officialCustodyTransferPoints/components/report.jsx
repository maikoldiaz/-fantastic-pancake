import React from 'react';
import { toPbiReport } from '../../../../common/components/reports/pbiReport.jsx';
import { constants } from '../../../../common/services/constants';

export default class OfficialCustodyTransferPointsReport extends React.Component {
    render() {
        const ReportComponent = toPbiReport(constants.Report.OfficialCustodyTransferPointsReport, null);
        return (
            <ReportComponent />
        );
    }
}
