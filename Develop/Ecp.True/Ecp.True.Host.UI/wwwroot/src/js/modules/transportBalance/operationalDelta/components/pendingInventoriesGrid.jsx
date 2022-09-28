import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../common/components/grid/gridComponent';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { DateCell } from '../../../../common/components/grid/gridCells.jsx';
import { apiService } from '../../../../common/services/apiService';
import { dateService } from '../../../../common/services/dateService';
import { requestPendingInventories } from './../actions';
import { utilities } from '../../../../common/services/utilities';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { wizardNextStep } from '../../../../common/actions.js';

export class PendingInventoriesGrid extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
        this.getPendingInventories = this.getPendingInventories.bind(this);
    }

    getColumns() {
        const columns = [];
        const date = rowProps => <DateCell {...this.props} {...rowProps} />;
        columns.push(gridUtils.buildTextColumn('inventoryId', this.props, null, 'inventoryId', { type: 'text' }));
        columns.push(gridUtils.buildTextColumn('node', this.props));
        columns.push(gridUtils.buildTextColumn('product', this.props));
        columns.push(gridUtils.buildDecimalColumn('amount', this.props, 'netQuantity', { type: 'decimal' }));
        columns.push(gridUtils.buildTextColumn('unit', this.props));
        columns.push(gridUtils.buildDateColumn('inventoryDate', this.props, date, 'inventoryDate'));
        columns.push(gridUtils.buildTextColumn('action', this.props));
        return columns;
    }

    getPendingInventories(apiUrl) {
        const ticket = {
            startDate: this.props.deltaTicket.startDate,
            endDate: this.props.deltaTicket.endDate,
            categoryElementId: this.props.deltaTicket.segment.elementId
        };
        this.props.getPendingInventories(apiUrl, ticket, this.props.name);
    }

    render() {
        return (
            <div className="ep-section">
                <header className="ep-section__header">
                    <h1 className="ep-section__title m-y-3">
                        {resourceProvider.readFormat('pendingInventoriesGridMessage', [
                            dateService.capitalize(this.props.deltaTicket.startDate),
                            dateService.capitalize(this.props.deltaTicket.endDate),
                            this.props.deltaTicket.segment.name
                        ])}
                    </h1>
                </header>
                <div className="ep-section__body ep-section__body--hf p-a-0">
                    <Grid name={this.props.name} columns={this.getColumns()} requestGridData={this.getPendingInventories} className="ep-table--row-sm ep-table--wizard" />
                </div>
                <SectionFooter floatRight={true} config={footerConfigService.getCommonConfig('pendingInventoriesGrid',
                    {
                        acceptActions: [wizardNextStep(this.props.config.wizardName)], onCancel: this.props.cancelDeltaCalculation,
                        acceptText: 'next', acceptClassName: 'ep-btn', acceptType: 'button'
                    })} />

            </div>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        deltaTicket: state.operationalDelta.deltaTicket
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        cancelDeltaCalculation: () => {
            ownProps.goToStep('operationalDelta', 1);
        },
        getPendingInventories: (apiUrl, deltaTicket, name) => {
            dispatch(requestPendingInventories(apiUrl, deltaTicket, name));
        }
    };
};

/* istanbul ignore next */
const gridConfig = () => {
    return {
        name: 'pendingInventories',
        odata: false,
        apiUrl: apiService.operationalDelta.getPendingInventories()
    };
};

/* istanbul ignore next */
export default dataGrid(connect(mapStateToProps, mapDispatchToProps, utilities.merge)(PendingInventoriesGrid), gridConfig);
