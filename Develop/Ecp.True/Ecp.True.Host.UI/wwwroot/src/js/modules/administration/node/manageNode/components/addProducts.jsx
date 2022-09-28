import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils';
import { ActionCell } from '../../../../../common/components/grid/gridCells.jsx';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { receiveGridData, removeGridData, addUpdateGridData } from '../../../../../common/components/grid/actions';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { DebounceAutocomplete } from '../../../../../common/components/autoComplete/autoComplete.jsx';
import { requestSearchProducts, updateNodeStorageLocations, clearSearchProducts } from '../actions';
import { utilities } from '../../../../../common/services/utilities';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';

class AddProducts extends React.Component {
    constructor() {
        super();
        this.updateNodeStorageLocationProducts = this.updateNodeStorageLocationProducts.bind(this);
        this.requestSearchProducts = this.requestSearchProducts.bind(this);
    }

    getColumns() {
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} />;
        const columns = [];

        columns.push(gridUtils.buildTextColumn('product.name', this.props, null, 'name'));
        columns.push(gridUtils.buildActionColumn(actions, 'actions', 100));

        return columns;
    }

    requestSearchProducts(searchText) {
        if (utilities.isNullOrWhitespace(searchText)) {
            this.props.clearSearchProducts();
            return;
        }

        if (!this.props.node.sendToSap) {
            this.props.requestSearchProducts(searchText);
        }

        if (this.props.node.sendToSap && !utilities.isNullOrUndefined(this.props.nodeStorageLocation.storageLocation)) {
            this.props.requestSearchProducts(searchText, this.props.nodeStorageLocation.storageLocation.storageLocationId);
        }
    }

    updateNodeStorageLocationProducts() {
        const values = Object.assign({}, this.props.nodeStorageLocation, { products: this.props.products });
        this.props.onSubmit(values);
        this.props.closeModal();
    }

    render() {
        const productIds = this.props.products.map(x => x.productId);
        const autoCompleteItems = this.props.searchedProducts.filter(x => !productIds.includes(x.productId));
        return (
            <>
                <section className="ep-modal__content">
                    <div className="ep-control-group">
                        <div className="ep-control ep-control--addon" id="ac_addProducts_products">
                            <div className="ep-control__inner">
                                <DebounceAutocomplete onSelect={this.props.addStorageLocationProduct}
                                    handleChange={this.requestSearchProducts}
                                    placeholder={resourceProvider.read('productName')}
                                    shouldItemRender={(item, value) => item.name.toLowerCase().indexOf(value.toLowerCase()) > -1}
                                    renderItem={(item, isHighlighted) =>
                                        (<div style={{ padding: '10px 12px', background: isHighlighted ? '#eee' : '#fff' }}>
                                            {item.name}
                                        </div>)
                                    }
                                    items={autoCompleteItems} getItemValue={p => p.name} />
                                <span id="btn_addProduct_add" className="ep-control__inner-addon"><i className="fas fa-plus-circle" /></span>
                            </div>
                        </div>
                    </div>

                    <Grid className="ep-table--h200 ep-table--nofilter" name={this.props.name} columns={this.getColumns()} />
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('addProduct',
                    { onAccept: this.updateNodeStorageLocationProducts })} />

            </>
        );
    }

    componentDidMount() {
        this.props.clearSearchProducts();
        this.props.loadProducts(this.props.nodeStorageLocation.products);
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        delete: true,
        nodeStorageLocation: state.node.manageNode.productNodeStorageLocation,
        node: state.node.manageNode.node,
        products: state.grid.products ? state.grid.products.items : [],
        searchedProducts: state.node.manageNode.searchedProducts
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        loadProducts: products => {
            dispatch(receiveGridData(products, 'products'));
        },
        onDelete: product => {
            dispatch(removeGridData('products', [product]));
        },
        addStorageLocationProduct: product => {
            const nodeStorageLocationProduct = Object.assign({}, { product, productId: product.productId, isActive: product.isActive });
            dispatch(addUpdateGridData('products', 'productId', nodeStorageLocationProduct));
        },
        onSubmit: nodeStorageLocation => {
            dispatch(addUpdateGridData('nodeStorageLocation', 'nodeStorageLocationId', nodeStorageLocation));
            dispatch(updateNodeStorageLocations(nodeStorageLocation));
        },
        requestSearchProducts: (searchText, storageLocationId) => {
            dispatch(requestSearchProducts(searchText, storageLocationId));
        },
        clearSearchProducts: () => {
            dispatch(clearSearchProducts());
        }
    };
};
const productsGridConfig = () => {
    return {
        name: 'products',
        odata: false,
        idField: 'productId',
        showPagination: false,
        defaultPageSize: 999999,
        filterable: false,
        sortable: false
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(AddProducts, productsGridConfig));
