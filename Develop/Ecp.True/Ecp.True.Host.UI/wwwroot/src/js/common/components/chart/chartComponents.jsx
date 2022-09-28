import React from 'react';
import NumberFormatter from '../formControl/numberFormatter.jsx';
import { utilities } from '../../services/utilities.js';

export const ChartLegend = props => {
    const { payload, units } = props;
    return (
        <ul className="ep-chart__leg">
            {
                payload.map((entry, index) => (
                    <li key={`item-${index}`} className="ep-chart__leg-itm">
                        <span className="ep-chart__leg-title">
                            <span className={`ep-chart__leg-typ-${entry.type}`} style={{ backgroundColor: entry.color }} />
                            {entry.value}
                        </span>
                        <NumberFormatter
                            className="ep-chart__leg-val"
                            value={utilities.parseFloat(entry.payload.value)}
                            suffix={units}
                            isNumericString={true}
                            displayType="text" />
                    </li>
                ))
            }
        </ul>
    );
};
