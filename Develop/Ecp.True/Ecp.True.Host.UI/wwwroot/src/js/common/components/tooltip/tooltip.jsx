import React from 'react';
import { default as RcTooltip } from 'rc-tooltip';
import { constants } from '../../services/constants';

export default class Tooltip extends React.Component {
    render() {
        return (
            <RcTooltip {...this.props} overlayClassName={`ep-tooltip ${this.props.overlayClassName || ''}`}
                placement={this.props.placement || this.props.children ? 'bottom' : 'bottomLeft'}
                mouseEnterDelay={this.props.mouseEnterDelay || constants.Timeouts.TooltipDelay}
                overlay={
                    <div className="ep-tooltip__body">
                        {this.props.body}
                    </div>
                }>
                <span className="ep-tooltip__trigger">
                    {this.props.children || <em className="fas fa-info-circle" aria-hidden="true" />}
                </span>
            </RcTooltip>
        );
    }
}
