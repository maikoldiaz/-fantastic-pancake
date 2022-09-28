import React from 'react';
import CutoffReportFilter from '../../../report/cutOff/components/filter.jsx';

export default class Filter extends React.Component {
    render() {
        return (
            <CutoffReportFilter {...this.props} />
        );
    }
}
