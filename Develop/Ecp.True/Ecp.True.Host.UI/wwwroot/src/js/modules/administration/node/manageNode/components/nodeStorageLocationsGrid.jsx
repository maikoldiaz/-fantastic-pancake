import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { ActionCell, StatusCell, ProductsCell } from '../../../../../common/components/grid/gridCells.jsx';
import { openModal } from '../../../../../common/actions';
import { constants } from '../../../../../common/services/constants';
import { initializeNodeStorageLocationEdit, initializeNodeStorageLocationProducts, removeNodeStorageLocations } from '../actions';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { optionService } from '../../../../../common/services/optionService';
import { removeGridData, receiveGridData } from '../../../../../common/components/grid/actions';

class NodeStorageLocationsGrid extends React.Component {
    getColumns() {
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} />;
        const status = rowProps => <StatusCell {...this.props} {...rowProps} />;
        const products = gridProps => <ProductsCell {...this.props} {...gridProps} />;
        const columns = [];

        columns.push(gridUtils.buildTextColumn('name', this.props));

        columns.push(gridUtils.buildTextColumn('storageLocationType.name', this.props, null, 'type'));
        columns.push(gridUtils.buildTextColumn('description', this.props));
        columns.push(gridUtils.buildSelectColumn('isActive', this.props, status, 'state', {
            values: optionService.getGridStatusTypes()
        }));
        columns.push(gridUtils.buildTextColumn('products', this.props, products, 'products', { filterable: false }));
        columns.push(gridUtils.buildActionColumn(actions, '', 100));

        return columns;
    }

    render() {
        return (
            <>
                <section className="ep-content">
                    <header className="d-block ep-content__header ep-content__header--h71 p-t-0">
                        <span className="float-r">
                            <button id="btn_nodeStorageLocationGrid_create" type="button"
                                onClick={this.props.addStorageLocation} className="ep-btn"><i className="fas fa-plus-square m-r-1" />
                                <span className="ep-btn__txt">{resourceProvider.read('createStorageLocation')}</span></button>
                        </span>
                    </header>
                    <div className="ep-content__body">
                        <Grid name={this.props.name} columns={this.getColumns()} />
                    </div>
                </section>
            </>
        );
    }

    componentDidMount() {
        this.props.requestNodeStorageLocations(this.props.nodeStorageLocations);
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        edit: true,
        trueClass: 'ep-pill m-r-1 ep-pill--active',
        trueKey: 'active',
        falseClass: 'ep-pill m-r-1 ep-pill--inactive',
        falseKey: 'inActive',
        idField: 'nodeStorageLocationId',
        node: state.node.manageNode.node,
        nodeStorageLocations: state.node.manageNode.nodeStorageLocations
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onEdit: nodeStorageLocation => {
            dispatch(initializeNodeStorageLocationEdit(nodeStorageLocation));
            dispatch(openModal('createStorageLocation', constants.Modes.Update));
        },
        onDelete: nodeStorageLocation => {
            dispatch(removeGridData('nodeStorageLocation', [nodeStorageLocation]));
            dispatch(removeNodeStorageLocations(nodeStorageLocation));
        },
        addStorageLocation: () => {
            dispatch(openModal('createStorageLocation', constants.Modes.Create));
        },
        addProducts: nodeStorageLocation => {
            dispatch(initializeNodeStorageLocationProducts(nodeStorageLocation));
            dispatch(openModal('addProducts', '', `${resourceProvider.read('addProducts')} | ${nodeStorageLocation.name}`));
        },
        requestNodeStorageLocations: nodeStorageLocations => {
            dispatch(receiveGridData(nodeStorageLocations, 'nodeStorageLocation'));
        }
    };
};

const nodeStorageLocationGridConfig = () => {
    return {
        name: 'nodeStorageLocation',
        odata: false,
        idField: 'nodeStorageLocationId'
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(NodeStorageLocationsGrid, nodeStorageLocationGridConfig));
