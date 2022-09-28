import React from 'react';
import { connect } from 'react-redux';
import { apiService } from '../../../../common/services/apiService';
import Grid from '../../../../common/components/grid/grid.jsx';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { updateCurrentVolumeControl } from '../actions';
import { calculationService } from '../services/calculationService';
import NumberFormatter from '../../../../common/components/formControl/numberFormatter.jsx';
import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';

class SummaryGrid extends React.Component {
    constructor() {
        super();
        this.ownershipNodeTotalTemplate = this.ownershipNodeTotalTemplate.bind(this);
        this.sumTotalOwnershipBalance = this.sumTotalOwnershipBalance.bind(this);
        this.sumTotalControlPercent = this.sumTotalControlPercent.bind(this);
        this.calculateTotalControlPercent = this.calculateTotalControlPercent.bind(this);
    }

    calculateControlPercent(data) {
        const result = calculationService.calculateControlPercent(data);
        if (utilities.isNullOrUndefined(result)) {
            return null;
        }
        return (
            <>
                {result.inputs === 0 ?
                    <span className="fc-error float-r fw-600">{resourceProvider.read('error')}</span> :
                    <NumberFormatter
                        className="float-r fw-600"
                        value={Math.abs(parseFloat(result.volume / result.inputs)).toFixed(2) === '0.00' ? '0.00' : parseFloat(result.volume / result.inputs).toFixed(2)}
                        suffix={constants.Suffix}
                        displayType="text"
                        isNumericString={true} />
                }
            </>
        );
    }

    calculateTotalControlPercent(rows) {
        const result = calculationService.calculateTotalControlPercent(rows);
        const acceptableBalance = this.props.nodeDetails.node && this.props.nodeDetails.node.acceptableBalancePercentage;
        if (utilities.isNullOrUndefined(result)) {
            return null;
        }
        return (
            <>
                <i className={`fas fa-circle ${acceptableBalance < (result.volumeTotal / result.inputsTotal) ? 'fas--error' : 'fas--success'}`} />
                {result.inputsTotal === 0 ?
                    <span className="fc-error float-r fw-600">{resourceProvider.read('error')}</span> :
                    <NumberFormatter
                        className="float-r fw-600"
                        value={Math.abs(parseFloat(result.volumeTotal / result.inputsTotal)).toFixed(2) === '0.00' ? '0.00' : parseFloat(result.volumeTotal / result.inputsTotal).toFixed(2)}
                        suffix={constants.Suffix}
                        displayType="text"
                        isNumericString={true} />
                }
            </>
        );
    }

    sumTotalOwnershipBalance(rows, key) {
        const sumTotal = calculationService.sumTotalOwnershipBalance(rows, key);
        if (utilities.isNullOrUndefined(sumTotal)) {
            return null;
        }
        if (key === 'volume') {
            this.props.currentVolumeControl(utilities.parseFloat(sumTotal));
        }
        return (<NumberFormatter className="float-r fw-600" value={utilities.parseFloat(sumTotal)} displayType="text" isNumericString={true} />);
    }

    sumTotalControlPercent(rows, key) {
        const totalSum = calculationService.sumTotalOwnershipBalance(rows, key);
        if (utilities.isNullOrUndefined(totalSum)) {
            return null;
        }
        const acceptableBalance = this.props.nodeDetails.node && this.props.nodeDetails.node.acceptableBalancePercentage;
        return (
            <>
                <i className={`fas fa-circle ${acceptableBalance < totalSum ? 'fas--error' : 'fas--success'}`} />
                <NumberFormatter
                    className="float-r fw-600"
                    value={utilities.parseFloat(totalSum)}
                    suffix={constants.Suffix}
                    displayType="text"
                    isNumericString={true} />
            </>
        );
    }

    aggregatedTextFooterTemplate(rows, key) {
        return rows.data.length > 0 ? <span className="fw-600">{rows.data[0][key]}</span> : null;
    }

    ownershipNodeTotalTemplate() {
        const nodeName = this.props.nodeDetails.node && this.props.nodeDetails.node.name;
        return <span className="fw-600 text-uppercase">{`${resourceProvider.read('total')} ${nodeName}`}</span>;
    }

    aggregatedSumOwnershipBalanceGroup(row) {
        return (<NumberFormatter className="float-r fw-600" value={utilities.parseFloat(row.value)} displayType="text" isNumericString={true} />);
    }

    getColumns() {
        const columns = [];
        columns.push(gridUtils.buildTextColumn('product', this.props, null, 'product',
            {
                width: 220, noHeader: true, aggregate: calculationService.aggregatedTextColTemplate, footer: this.ownershipNodeTotalTemplate,
                pivotValue: ({ value }) => <span className="fw-600 text-uppercase">{value}</span>
            }));

        columns.push(gridUtils.buildTextColumn('owner', this.props, null, 'owner',
            { noHeader: true, aggregate: () => '' }));

        columns.push(gridUtils.buildDecimalColumn('initialInventory', this.props, 'initialInventory',
            {
                aggregate: calculationService.sumOwnershipBalanceGroup,
                aggregated: row => this.aggregatedSumOwnershipBalanceGroup(row), footer: this.sumTotalOwnershipBalance, defaultVal: 0
            }));

        columns.push(gridUtils.buildDecimalColumn('inputs', this.props, 'inputs',
            {
                aggregate: calculationService.sumOwnershipBalanceGroup,
                aggregated: row => this.aggregatedSumOwnershipBalanceGroup(row), footer: this.sumTotalOwnershipBalance, defaultVal: 0
            }));

        columns.push(gridUtils.buildDecimalColumn('outputs', this.props, 'outputs',
            {
                aggregate: calculationService.sumOwnershipBalanceGroup,
                aggregated: row => this.aggregatedSumOwnershipBalanceGroup(row), footer: this.sumTotalOwnershipBalance, defaultVal: 0
            }));

        columns.push(gridUtils.buildDecimalColumn('identifiedLosses', this.props, 'identifiedLoses',
            {
                aggregate: calculationService.sumOwnershipBalanceGroup,
                aggregated: row => this.aggregatedSumOwnershipBalanceGroup(row), footer: this.sumTotalOwnershipBalance, defaultVal: 0
            }));

        columns.push(gridUtils.buildDecimalColumn('interface', this.props, 'interface',
            {
                aggregate: calculationService.sumOwnershipBalanceGroup,
                aggregated: row => this.aggregatedSumOwnershipBalanceGroup(row), footer: this.sumTotalOwnershipBalance, defaultVal: 0
            }));

        columns.push(gridUtils.buildDecimalColumn('tolerance', this.props, 'tolerance',
            {
                aggregate: calculationService.sumOwnershipBalanceGroup,
                aggregated: row => this.aggregatedSumOwnershipBalanceGroup(row), footer: this.sumTotalOwnershipBalance, defaultVal: 0
            }));

        columns.push(gridUtils.buildDecimalColumn('unidentifiedLosses', this.props, 'unidentifiedLosses',
            {
                aggregate: calculationService.sumOwnershipBalanceGroup,
                aggregated: row => this.aggregatedSumOwnershipBalanceGroup(row), footer: this.sumTotalOwnershipBalance, defaultVal: 0
            }));

        columns.push(gridUtils.buildDecimalColumn('finalInventory', this.props, 'invFinal',
            {
                aggregate: calculationService.sumOwnershipBalanceGroup,
                aggregated: row => this.aggregatedSumOwnershipBalanceGroup(row), footer: this.sumTotalOwnershipBalance, defaultVal: 0
            }));

        columns.push(gridUtils.buildDecimalColumn('volume', this.props, 'volumeControl',
            {
                aggregate: calculationService.sumOwnershipBalanceGroup,
                aggregated: row => this.aggregatedSumOwnershipBalanceGroup(row), footer: this.sumTotalOwnershipBalance, defaultVal: 0
            }));

        columns.push(gridUtils.buildTextColumn('measurementUnit', this.props, null, 'measurementUnit',
            { width: 90, aggregate: calculationService.aggregatedTextColTemplate, footer: this.aggregatedTextFooterTemplate }));

        columns.push(gridUtils.buildTextColumn('control', this.props, null, 'control',
            {
                width: 90, aggregated: r => (<span className="fw-600 float-r">{this.calculateControlPercent(r)}</span>),
                footer: this.calculateTotalControlPercent, defaultVal: 0
            }));

        return columns;
    }

    render() {
        return (
            <Grid name={this.props.name} columns={this.getColumns()} className="ep-table--pivotal" wrapperClassName="ep-table-wrap--mh100" />
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        nodeDetails: state.nodeOwnership.ownershipNode.nodeDetails
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        currentVolumeControl: totalVolume => {
            dispatch(updateCurrentVolumeControl(totalVolume));
        }
    };
};

/* istanbul ignore next */
const gridConfig = props => {
    return {
        name: 'ownershipNodeBalance',
        apiUrl: apiService.ownershipNode.getOwnershipNodeBalance(props.ownershipNodeId),
        pivotBy: ['product'],
        showPagination: false,
        sortable: false,
        filterable: false,
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(SummaryGrid, gridConfig));
