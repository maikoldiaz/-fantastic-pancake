import React from 'react';
import { connect } from 'react-redux';
import Grid from './../../../../common/components/grid/grid.jsx';
import { gridUtils } from './../../../../common/components/grid/gridUtils';
import { dataGrid } from './../../../../common/components/grid/gridComponent';
import { OwnershipPercentageInputTextCell, OwnershipVolumeInputNumberCell, OwnershipStatusCell } from './../../../../common/components/grid/gridCells.jsx';
import { receiveGridData } from './../../../../common/components/grid/actions';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { updateMovementOwnershipData } from '../actions';
import { calculationService } from '../services/calculationService';
import { utilities } from '../../../../common/services/utilities';
import NumberFormatter from '../../../../common/components/formControl/numberFormatter.jsx';

export class OwnershipMovementInventoryGrid extends React.Component {
    constructor() {
        super();
        this.getColumns = this.getColumns.bind(this);
        this.sumTotalVolumeOwnership = this.sumTotalVolumeOwnership.bind(this);
        this.sumTotalPercentageOwnership = this.sumTotalPercentageOwnership.bind(this);
        this.ownershipTotalTemplate = this.ownershipTotalTemplate.bind(this);
    }

    sumTotalVolumeOwnership(rows, key) {
        const result = calculationService.sumTotalVolumePercentageOwnership(rows, key);
        if (utilities.isNullOrUndefined(result)) {
            return null;
        }
        return (<span className="ep-ft-cell fw-600 text-right">
            <NumberFormatter
                value={utilities.parseFloat(result)}
                displayType="text"
                isNumericString={true} />
        </span>);
    }

    sumTotalPercentageOwnership(rows, key) {
        const result = calculationService.sumTotalVolumePercentageOwnership(rows, key);
        if (utilities.isNullOrUndefined(result)) {
            return null;
        }
        return (
            <div className="ep-control ep-control--addon">
                <div className="ep-control__inner ep-control__inner-bgwt">
                    <NumberFormatter
                        className="ep-textbox fw-600 text-right p-y-2 fs-14"
                        value={utilities.parseFloat(result)}
                        displayType="text"
                        isNumericString={true} />
                    <span className="ep-control__inner-addon"><i className="fas fa-percentage" aria-hidden="true" /></span>
                </div>
            </div>
        );
    }

    ownershipTotalTemplate() {
        return <span className="fw-600 fs-16 d-block m-t-1">{`${resourceProvider.read('total')}`}</span>;
    }

    getColumns() {
        const percentageInputTextCell = rowProps => <OwnershipPercentageInputTextCell {...this.props} {...rowProps} />;
        const volumeInputNumberCell = rowProps => <OwnershipVolumeInputNumberCell {...this.props} {...rowProps} />;
        const ownershipStatusCell = rowProps => <OwnershipStatusCell {...this.props} {...rowProps} />;

        const columns = [];

        columns.push(gridUtils.buildTextColumn('name', this.props,
            r => <div className="p-rel fw-600 fs-16 p-y-4 p-x-6"><span className="ep-table__cell-bar" style={{ backgroundColor: r.original.color }} />{r.original.ownerName}</div>,
            'ownerName', { filterable: false, footer: this.ownershipTotalTemplate }));
        columns.push(gridUtils.buildEditableNumberColumn('ownershipVolume', this.props, volumeInputNumberCell, 'ownershipVolume', { filterable: false, footer: this.sumTotalVolumeOwnership }));
        columns.push(gridUtils.buildEditableNumberColumn('ownershipPercentage', this.props, percentageInputTextCell, 'ownershipPercentage',
            { filterable: false, footer: this.sumTotalPercentageOwnership }));
        columns.push(gridUtils.buildTextColumn('name', this.props, ownershipStatusCell, 'ownershipPercentage', { filterable: false }));

        return columns;
    }

    render() {
        return (
            <Grid classAlignment="p-t-0" name={this.props.name} columns={this.getColumns()} className="ep-table--bar" />
        );
    }

    componentDidMount() {
        this.props.loadGridData(this.props.movementInventoryOwnershipData);
    }

    componentDidUpdate(prevProps) {
        if (prevProps.updateOwnershipDataToggler !== this.props.updateOwnershipDataToggler) {
            this.props.loadGridData(this.props.movementInventoryOwnershipData);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        movementInventoryOwnershipData: state.nodeOwnership.ownershipNode.movementInventoryOwnershipData,
        updateOwnershipDataToggler: state.nodeOwnership.ownershipNode.updateOwnershipDataToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, props) => {
    return {
        loadGridData: movementInventoryOwnershipData => {
            dispatch(receiveGridData(movementInventoryOwnershipData, 'ownershipMovementInventoryGrid'));
        },
        updateOwnershipVolume: (ownerData, volume) => {
            const updatedVolume = !utilities.isNullOrWhitespace(volume) ? volume : 0;
            const updatedPercentage = updatedVolume / ownerData.netVolume * 100;
            const movementOwnerData = Object.assign({}, ownerData, { ownershipVolume: updatedVolume, ownershipPercentage: updatedPercentage });
            const index = props.movementInventoryOwnershipData.findIndex(x => x.ownerId === movementOwnerData.ownerId);
            const items = [...props.movementInventoryOwnershipData];
            items[index] = movementOwnerData;
            dispatch(receiveGridData(items, 'ownershipMovementInventoryGrid'));
            dispatch(updateMovementOwnershipData(items));
        },
        updateOwnershipPercentage: (ownerData, percentage) => {
            const updatedPercentage = !utilities.isNullOrWhitespace(percentage) ? percentage : 0;
            const updatedVolume = updatedPercentage * ownerData.netVolume / 100;
            const movementOwnerData = Object.assign({}, ownerData, { ownershipVolume: updatedVolume, ownershipPercentage: updatedPercentage });
            const index = props.movementInventoryOwnershipData.findIndex(x => x.ownerId === movementOwnerData.ownerId);
            const items = [...props.movementInventoryOwnershipData];
            items[index] = movementOwnerData;
            dispatch(receiveGridData(items, 'ownershipMovementInventoryGrid'));
            dispatch(updateMovementOwnershipData(items));
        }
    };
};

const ownershipMovementInventoryGridConfig = () => {
    return {
        name: 'ownershipMovementInventoryGrid',
        oData: false,
        idField: 'ownerId',
        showPagination: false,
        filterable: false,
        sortable: false,
        startEmpty: true,
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(OwnershipMovementInventoryGrid, ownershipMovementInventoryGridConfig));
