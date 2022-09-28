import React from 'react';
import Tooltip from '../../../../common/components/tooltip/tooltip.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { utilities } from '../../../../common/services/utilities';

export default class EventDetails extends React.Component {
    getTables() {
        const content = this.props.content || {};
        const keys = Object.keys(content);

        const tables = [];

        let index = 0;
        let start = index;
        while (index < Math.floor(keys.length / this.props.maxCols)) {
            const end = start + this.props.maxCols;
            const section = {};
            keys.slice(start, end).forEach(v => {
                section[v] = utilities.getStringOrDefault(content[v].toString(), '--');
            });

            tables.push(section);

            start = end;
            index++;
        }

        const remainder = keys.length % this.props.maxCols;
        if (remainder > 0) {
            index = 0;
            start = keys.length - remainder;
            const section = {};

            keys.slice(start, keys.length).forEach(v => {
                section[v] = utilities.getStringOrDefault(content[v].toString(), '--');
            });

            // To-Do: Adding empty cols to mantain spacing, should be handled in css.
            while (index < this.props.maxCols - remainder) {
                section[`Empty${index}`] = '';
                index++;
            }

            tables.push(section);
        }

        return tables;
    }

    getColWidth() {
        return `${Math.round(100 / this.props.maxCols)}%`;
    }

    render() {
        return (
            <>
                {this.getTables().map((section, index) => {
                    return (
                        <section className="ep-table-wrap" key={index}>
                            <div className="ep-table ep-table--smpl m-b-6">
                                <table role="grid">
                                    <colgroup>
                                        {[...Array(this.props.maxCols)].map((e, i) => <col style={{ width: this.getColWidth() }} key={i}/>)}
                                    </colgroup>
                                    <thead>
                                        <tr>
                                            {Object.keys(section).map((s, i) => (
                                                <th key={i}>
                                                    {!s.startsWith('Empty') &&
                                                    <Tooltip body={resourceProvider.read(s)} overlayClassName="ep-tooltip--lt">
                                                        {resourceProvider.read(s)}
                                                    </Tooltip>}
                                                </th>
                                            ))}
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            {Object.keys(section).map((s, i) => (
                                                <td key={i}>{section[s]}</td>
                                            ))}
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </section>);
                })}
            </>
        );
    }
}
