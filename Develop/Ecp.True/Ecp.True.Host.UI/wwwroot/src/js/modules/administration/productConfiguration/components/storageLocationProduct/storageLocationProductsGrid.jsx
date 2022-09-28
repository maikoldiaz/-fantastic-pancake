import React from 'react';
import { connect } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import PageActions from '../../../../../common/router/pageActions.jsx';
import { gridUtils } from '../../../../../common/components/grid/gridUtils';
import { dataGrid } from '../../../../../common/components/grid/gridComponent';
import { ActionCell } from '../../../../../common/components/grid/gridCells.jsx';
import { constants } from '../../../../../common/services/constants';
import { apiService } from '../../../../../common/services/apiService';
import { openMessageModal, closeModal } from '../../../../../common/actions';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { deleteAssociationCenterStorageProduct } from './../../actions.js';
import { refreshGrid } from './../../../../../common/components/grid/actions';

export class StorageLocationProductsGrid extends React.Component {
    constructor() {
        super();
    }

    getColumns() {
        const actions = gridProps => <ActionCell {...this.props} {...gridProps} delete={true} onDelete={v => this.onDelete(v)} />;

        const columns = [];

        columns.push(gridUtils.buildTextColumn('storageLocation.logisticCenter.name', this.props, '', 'logisticsCenter'));
        columns.push(gridUtils.buildTextColumn('storageLocation.name', this.props, '', 'storageLocation'));
        columns.push(gridUtils.buildTextColumn('product.name', this.props, '', 'productSapName'));

        columns.push(gridUtils.buildActionColumn(actions, 'actions', 100));

        return columns;
    }

    onDelete(association) {
        const options = {
            title: resourceProvider.read('deleteAssociationCenterStorageProduct'), canCancel: true, acceptAction: () => {
                this.props.deleteAssociationCenterStorageProduct(association.storageLocationProductMappingId);
            },
            className: 'ep-modal'
        };
        const message = resourceProvider.read('confirmDeleteCenterStorageProduct');
        this.props.openModal(message, options);
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
                                            title: 'createAssociation',
                                            type: constants.RouterActions.Type.Button,
                                            actionType: 'navigate',
                                            mode: constants.Modes.Create,
                                            iconClass: 'fas fa-plus-square',
                                            route: 'productconfiguration/createassociation'
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

    componentDidUpdate(prevProps) {
        if (prevProps.deleteToggler !== this.props.deleteToggler) {
            if (!this.props.isDeleted) {
                const options = {
                    title: resourceProvider.read('validationTitleOfAssociatedMovements'), canCancel: false, acceptAction: () => {
                        this.props.closeModal();
                    },
                    className: 'ep-modal'
                };
                const message = resourceProvider.read('validationMessageOfAssociatedMovements');
                this.props.openModal(message, options);
            } else {
                this.props.refreshGrid('associations');
            }
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        isDeleted: state.products.deleted,
        deleteToggler: state.products.deleteToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        deleteAssociationCenterStorageProduct: associationId => {
            dispatch(deleteAssociationCenterStorageProduct(associationId));
            dispatch(closeModal());
        },
        openModal: (message, options) => {
            dispatch(openMessageModal(message, options));
        },
        closeModal: () => {
            dispatch(closeModal());
        },
        refreshGrid: name => {
            dispatch(refreshGrid(name));
        }
    };
};

const gridConfig = () => {
    return {
        name: 'associations',
        apiUrl: apiService.getStorageLocationProductMappings(),
        idField: 'storageLocationProductMappingId',
        section: true,
        sortable: {
            defaultSort: 'storageLocation/logisticCenter/name asc'
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(dataGrid(StorageLocationProductsGrid, gridConfig));
