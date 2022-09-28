import React from 'react';
import { BarChart, Bar, Cell, XAxis, YAxis, CartesianGrid, ResponsiveContainer } from 'recharts';
import { constants } from './../../../../common/services/constants';

export default class RecordsCreatedChart extends React.Component {
    render() {
        const colors = constants.ChartColors;
        return (
            <ResponsiveContainer>
                <BarChart
                    data={this.props.data}
                    layout="vertical"
                    margin={{
                        top: 30, right: 30, left: 30, bottom: 30
                    }} >
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis type="number" />
                    <YAxis type="category" dataKey="name" />
                    <Bar dataKey="value" label={{ position: 'right', fill: '#000000' }}>
                        {
                            this.props.data.map((entry, index) => (
                                <Cell key={`cell-${index}`} fill={colors[entry.color]} />
                            ))
                        }
                    </Bar>
                </BarChart >
            </ResponsiveContainer>
        );
    }
}
