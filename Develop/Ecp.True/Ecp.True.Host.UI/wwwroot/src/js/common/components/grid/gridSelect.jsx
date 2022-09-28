import React from 'react';
import { resourceProvider } from '../../services/resourceProvider';

export default class GridSelect extends React.Component {
    render() {
        return (
            <div className="p-rel">
                <label className="ep-checkbox ep-checkbox--gr-sel">
                    <input className="ep-checkbox__input" id={`chk_${this.props.id}`}
                        type={this.props.selectType || 'checkbox'}
                        disabled={this.props.row && this.props.row.disabled}
                        checked={this.props.checked}
                        onClick={e => {
                            const { shiftKey } = e;
                            e.stopPropagation();
                            this.props.onClick(this.props.id, shiftKey, this.props.row);
                        }}
                        onChange={() => { }}
                    />
                    <span className="ep-checkbox__action" />
                    <span className="sr-only">{resourceProvider.read('checkbox')}</span>
                </label>
            </div>
        );
    }
}
