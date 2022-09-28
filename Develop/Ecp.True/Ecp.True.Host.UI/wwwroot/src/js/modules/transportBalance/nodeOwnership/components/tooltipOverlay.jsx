import React from 'react';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dateService } from '../../../../common/services/dateService';

export class TooltipOverlay extends React.Component {
    render() {
        return (
            <>
                <div className="ep-label-wrap">
                    <label className="ep-label fc-white">{resourceProvider.read('initialDate')}:</label>
                    <span className="ep-data m-l-2">{dateService.format(this.props.data.startDate)}</span>
                </div>
                <div className="ep-label-wrap">
                    <label className="ep-label fc-white">{resourceProvider.read('finalDate')}:</label>
                    <span className="ep-data m-l-2">{dateService.format(this.props.data.endDate)}</span>
                </div>
                <div className="ep-label-wrap">
                    <label className="ep-label fc-white">{resourceProvider.read('dailyVolume')}:</label>
                    <span className="ep-data m-l-2">{this.props.data.volume}</span>
                </div>
                <div className="ep-label-wrap">
                    <label className="ep-label fc-white">{resourceProvider.read('units')}:</label>
                    <span className="ep-data m-l-2">{this.props.data.measurementUnitDetail.name}</span>
                </div>
            </>
        );
    }
}
