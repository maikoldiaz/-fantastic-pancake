import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { apiService } from '../../../../../common/services/apiService';
import { ActionCell, StatusCell } from '../../../../../common/components/grid/gridCells.jsx';
import { openModal, openMessageModal, closeModal } from '../../../../../common/actions';
import { constants } from '../../../../../common/services/constants';
import { refreshGrid } from '../../../../../common/components/grid/actions';
import { optionService } from '../../../../../common/services/optionService';
import PageActions from '../../../../../common/router/pageActions.jsx';
import { dataService } from '../../dataservice';
import { initProduct, clearSuccess, deleteProduct } from '../../actions';
import { resourceProvider } from '../../../../../common/services/resourceProvider';


export class ProductsGrid extends React.Component {
    constructor() {
        super();
        this.getStatus = this.getStatus.bind(this);
        this.getColumns = this.getColumns.bind(this);
    }

    getStatus(row) {
        return row.state;
    }

    getColumns() {
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} onDelete={v => this.onDelete(v)} />;
        const status = rowProps => <StatusCell {...this.props} {...rowProps} />;

        const columns = [];

        columns.push(gridUtils.buildTextColumn('productId', this.props, '', 'productSapId'));
        columns.push(gridUtils.buildTextColumn('name', this.props, '', 'productSapName'));
        columns.push(gridUtils.buildSelectColumn('isActive', this.props, status, 'state', {
            width: 150,
            values: optionService.getGridStatusTypes()
        }));

        columns.push(gridUtils.buildActionColumn(actions, null, 100));

        return columns;
    }

    onDelete(data) {
        const opts = {
            title: resourceProvider.read('deleteProduct'), canCancel: true, acceptAction: () => {
                this.props.deleteProduct(data);
            }
        };
        this.props.openConfirmModal(opts);
    }

    componentDidUpdate(prevProps) {
        if (prevProps.updateToggler !== this.props.updateToggler) {
            this.props.refreshGrid('products');
        }

        if (!this.props.saveSuccess && this.props.saveSuccess !== undefined) {
            const optsError = {
                title: resourceProvider.read('titleValidationCreateProduct'), acceptAction: () => {
                    this.props.clearStatus();
                }
            };
            const status = this.props.isActive ? resourceProvider.read('isActive') : resourceProvider.read('inActive');
            this.props.openErrorModal(resourceProvider.readFormat('validationCreateProduct', [status]), optsError);
        }

        if (!this.props.updateSuccess && this.props.updateSuccess !== undefined) {
            const optsError = {
                title: resourceProvider.read('titleValidationCreateProduct'), acceptAction: () => {
                    this.props.clearStatus();
                }
            };
            this.props.openErrorModal(resourceProvider.read('validationCreateProduct'), optsError);
        }

        if (!this.props.deleteSuccess && this.props.deleteSuccess !== undefined) {
            const optsError = {
                title: resourceProvider.read('titleValidationDeleteProduct'), acceptAction: () => {
                    this.props.clearStatus();
                },
                className: 'ep-modal'
            };
            this.props.openErrorModal(resourceProvider.read('validationDeleteProduct'), optsError);
        }
    }

    render() {
        return (
            <>
                <section className="ep-content">
                    <header className="d-block ep-content__header ep-content__header--h71 p-t-0">
                        <span className="float-r">
                            <PageActions
                                actions={
                                    [
                                        {
                                            title: 'newProduct',
                                            type: constants.RouterActions.Type.Button,
                                            actionType: 'modal',
                                            key: 'modalProduct',
                                            mode: constants.Modes.Create,
                                            iconClass: 'fas fa-plus-square'
                                        }
                                    ]
                                }
                            />
                        </span>
                    </header>
                    <Grid name={this.props.name} columns={this.getColumns()} />
                </section>

            </>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        edit: true,
        delete: true,
        trueClass: 'ep-pill m-r-1 ep-pill--active',
        trueKey: 'active',
        falseClass: 'ep-pill m-r-1 ep-pill--inactive',
        falseKey: 'inActive',
        saveSuccess: state.products.saveSuccess,
        updateSuccess: state.products.updateSuccess,
        updateToggler: state.products.updateToggler,
        deleteSuccess: state.products.deleteSuccess,
        isActive: state.products.isActive
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        refreshGrid: name => {
            dispatch(refreshGrid(name));
        },
        onEdit: row => {
            dispatch(initProduct(dataService.buildInitialValues(row)));
            dispatch(openModal('modalProduct', constants.Modes.Update, 'Product'));
        },
        openErrorModal: (message, options) => {
            dispatch(openMessageModal(message, options));
        },
        clearStatus: () => {
            dispatch(clearSuccess());
            dispatch(closeModal());
        },
        deleteProduct: row => {
            dispatch(deleteProduct(row.productId));
            dispatch(closeModal());
        },
        openConfirmModal: options => {
            dispatch(openMessageModal(resourceProvider.read('confirmDeleteProduct'), options));
        }
    };
};

const gridConfig = () => {
    return {
        name: 'products',
        apiUrl: apiService.product.getProducts(),
        idField: 'productId',
        sortable: {
            defaultSort: 'productId'
        },
        section: true
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(ProductsGrid, gridConfig));
