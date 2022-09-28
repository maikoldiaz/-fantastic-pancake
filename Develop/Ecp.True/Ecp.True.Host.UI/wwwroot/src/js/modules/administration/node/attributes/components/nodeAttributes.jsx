import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { ActionCell } from '../../../../../common/components/grid/gridCells.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../../common/services/apiService';
import { utilities } from '../../../../../common/services/utilities';
import { navigationService } from '../../../../../common/services/navigationService';
import { CategoryElementFilterComponent } from '../../../../../common/components/filters/categoryElementFilter.jsx';
import { nodeAttributesFilterBuilder } from '../nodeAttributesFilterBuilder';
import { togglePageActions, openFlyout } from '../../../../../common/actions';
import { constants } from '../../../../../common/services/constants';

class NodeAttributes extends React.Component {
    constructor() {
        super();

        this.getColumns = this.getColumns.bind(this);
        this.buildControlLimit = this.buildControlLimit.bind(this);
        this.buildBalance = this.buildBalance.bind(this);
        this.enableEdit = this.enableEdit.bind(this);
    }

    buildControlLimit(controlLimit) {
        return !utilities.isNullOrUndefined(controlLimit) ? `${constants.Prefix}${controlLimit.toString()}` : '';
    }

    buildBalance(balance) {
        return !utilities.isNullOrUndefined(balance) ? `${balance}%` : '';
    }
    enableEdit() {
        return this.props.enableEditGrid;
    }

    getColumns() {
        const actions = rowProps => <ActionCell id={this.props.name} {...this.props} {...rowProps} edit={true} />;

        const columns = [];

        columns.push(gridUtils.buildTextColumn('name', this.props, row => utilities.toUpperCase(row.original.name), 'node'));
        columns.push(gridUtils.buildDecimalColumn('controlLimit', this.props, 'controlLimit', { prefix: constants.Prefix }));
        columns.push(gridUtils.buildDecimalColumn('acceptableBalancePercentage', this.props, 'acceptableBalance', { suffix: constants.Suffix }));
        columns.push(gridUtils.buildTextColumn('nodeOwnershipRule.ruleName', this.props, null, 'ilFunction'));
        columns.push(gridUtils.buildActionColumn(actions, null, 100));

        return columns;
    }

    render() {
        return (
            <>
                <Grid name={this.props.name} columns={this.getColumns()} onNoData={this.props.openNodeAttributesFilterFlyout} />
                <CategoryElementFilterComponent name={this.props.name} showTrash={true} />
            </>
        );
    }

    componentDidMount() {
        this.props.toggleSubmit(true);
    }
}

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onEdit: data => {
            navigationService.navigateTo(`manage/${data.nodeId}`);
        },
        openNodeAttributesFilterFlyout: () => {
            dispatch(openFlyout('nodeAttributes'));
        },
        toggleSubmit: enabled => {
            dispatch(togglePageActions(enabled));
        }
    };
};

const gridConfig = () => {
    return {
        name: 'nodeAttributes',
        apiUrl: apiService.node.query(),
        idField: 'nodeId',
        filterable: {
            filterBuilder: nodeAttributesFilterBuilder.build
        },
        section: true
    };
};

/* istanbul ignore next */
export default connect(null, mapDispatchToProps)(dataGrid(NodeAttributes, gridConfig));
