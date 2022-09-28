import React from 'react';
import { resourceProvider } from '../../services/resourceProvider.js';

export default class GridNoData extends React.Component {
    getNoDataText(key) {
        return key === resourceProvider.read(key) ? resourceProvider.read('noDataTxt') : resourceProvider.read(key);
    }

    render() {
        return (
            <div className="rt-noData">
                <i className="fas fa-exclamation-circle m-r-2" />{this.getNoDataText(`${this.props.name}NoDataTxt`)}
                {this.props.onNoData &&
                    <a className="ep-btn ep-btn--link m-l-4 fs-14" onClick={this.props.onNoData}>{resourceProvider.read(`${this.props.name}NoDataLnk`)}</a>
                }
            </div>
        );
    }
}

