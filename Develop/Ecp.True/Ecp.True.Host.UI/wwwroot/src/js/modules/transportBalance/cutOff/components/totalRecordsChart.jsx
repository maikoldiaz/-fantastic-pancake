import React from 'react';
import { Cell, PieChart, Pie, Legend, Sector, ResponsiveContainer } from 'recharts';
import { constants } from './../../../../common/services/constants';
import { ChartLegend } from './../../../../common/components/chart/chartComponents.jsx';

const renderActiveShape = props => {
    const totalRecordsCount = props.percent === 0 ? 0 : Math.round(props.value / props.percent, 0);
    return (
        <g>
            <text x={props.cx} y={props.cy} dy={8} textAnchor="middle" fill="#000000;">{totalRecordsCount}</text>
            <Sector
                cx={props.cx}
                cy={props.cy}
                innerRadius={props.innerRadius}
                outerRadius={props.outerRadius}
                startAngle={props.startAngle}
                endAngle={props.endAngle}
                fill={props.fill}
            />
        </g>
    );
};

export default class TotalRecordsChart extends React.Component {
    render() {
        const colors = constants.ChartColors;
        const legend = p => <ChartLegend {...p} units="" />;
        return (
            <ResponsiveContainer>
                <PieChart key="total" className="ep-chart ep-chart--leg-b" margin={{ top: 30, right: 30, left: 30, bottom: 30 }}>
                    <Pie activeIndex={0}
                        activeShape={renderActiveShape}
                        data={this.props.data} dataKey="value" outerRadius={100} innerRadius={60} paddingAngle={0}> {
                            this.props.data.map(entry => <Cell key={entry.key} fill={colors[entry.color]} />)
                        }
                    </Pie>
                    <Legend align="center" verticalAlign="bottom" layout="vertical" iconType="square" height={36} content={legend} />
                </PieChart>
            </ResponsiveContainer>
        );
    }
}
