import React from 'react';
import { connect } from 'react-redux';
import Select from 'react-select';
import { requestOwnershipNodeMovementInventoryData, setMovementInventoryOwnershipData, initUpdateFilters, clearMovementInventoryFilter } from '../actions';
import { getCategoryElements, getVariableTypes, openModal } from '../../../../common/actions';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';
import { dataService } from '../services/dataService';
import OwnershipNodeDataGrid from './ownershipNodeDataGrid.jsx';
import { resetPageIndex } from '../../../../common/components/grid/actions';

export class NodeDetails extends React.Component {
    constructor() {
        super();
        this.onChangeDropdown = this.onChangeDropdown.bind(this);
    }

    onChangeDropdown(val) {
        const filter = dataService.buildNodeDetailsFilter(val);
        this.props.resetPageIndex();
        this.props.initUpdateFilters(filter);
    }

    disableNewMovement() {
        const statusArray = [constants.OwnershipNodeStatus.OWNERSHIP, constants.OwnershipNodeStatus.UNLOCKED, constants.OwnershipNodeStatus.PUBLISHED,
            constants.OwnershipNodeStatus.REJECTED, constants.OwnershipNodeStatus.REOPENED,
            constants.OwnershipNodeStatus.RECONCILED, constants.OwnershipNodeStatus.NOTRECONCILED, constants.OwnershipNodeStatus.CONCILIATIONFAILED];
        const nodeOwnershipStatus = this.props.ownershipNodeDetails && this.props.ownershipNodeDetails.ownershipStatus;
        const nodeEditorName = this.props.ownershipNodeDetails && this.props.ownershipNodeDetails.editor;
        return !((nodeOwnershipStatus === constants.OwnershipNodeStatus.LOCKED && nodeEditorName === this.props.currentUser) ||
            (dataService.compareStatus(nodeOwnershipStatus, statusArray, 'eq', 'or')));
    }

    getProducts(data) {
        const products = [];
        if (data && data.length > 0) {
            data.forEach(v => {
                if (!utilities.contains(products, 'productId', v.productId)) {
                    products.push({ productId: v.productId, product: v.product });
                }
            });
        }

        return products;
    }

    render() {
        const owners = this.props.categoryElements.filter(v => v.categoryId === constants.Category.Owner);
        const products = this.getProducts(this.props.ownershipNodeBalance);

        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <h1 className="fs-16 text-uppercase text-center m-t-8 m-b-4">{resourceProvider.read('titleMovementInventoryDetail')}</h1>
                    <div className="row p-y-2 nowrap">
                        <div className="col-md-2">
                            <div className="ep-control-group m-b-0">
                                <label className="ep-label" id="lbl_editOwnershipNode_product" htmlFor="dd_editOwnershipNode_product_sel">
                                    {resourceProvider.read('product')}</label>
                                <div className="ep-control">
                                    <Select classNamePrefix="ep-select" className="ep-select" id="dd_editOwnershipNode_product"
                                        placeholder={resourceProvider.read('select')} inputId="dd_editOwnershipNode_product_sel"
                                        options={products}
                                        getOptionLabel={x => x.product} getOptionValue={x => x.productId}
                                        value={this.props.movementInventoryfilters.product}
                                        onChange={this.onChangeDropdown}
                                        components={{
                                            IndicatorSeparator: () => null,
                                            ClearIndicator: () => null
                                        }}
                                        filterOption={utilities.selectFilter()}
                                        noOptionsMessage={() => resourceProvider.read('noOptionsMessage')} />
                                </div>
                            </div>
                        </div>
                        <div className="col-md-2">
                            <div className="ep-control-group m-b-0">
                                <label className="ep-label" id="lbl_editOwnershipNode_variable" htmlFor="dd_editOwnershipNode_variable_sel">
                                    {resourceProvider.read('variable')}</label>
                                <div className="ep-control">
                                    <Select classNamePrefix="ep-select" className="ep-select" id="dd_editOwnershipNode_variable"
                                        placeholder={resourceProvider.read('select')} inputId="dd_editOwnershipNode_variable_sel"
                                        options={this.props.variableTypes}
                                        getOptionLabel={x => x.name} getOptionValue={x => x.variableTypeId}
                                        onChange={this.onChangeDropdown}
                                        value={this.props.movementInventoryfilters.variableType}
                                        components={{
                                            IndicatorSeparator: () => null,
                                            ClearIndicator: () => null
                                        }}
                                        filterOption={utilities.selectFilter()}
                                        noOptionsMessage={() => resourceProvider.read('noOptionsMessage')} />
                                </div>
                            </div>
                        </div>
                        <div className="col-md-2">
                            <div className="ep-control-group m-b-0">
                                <label className="ep-label" id="lbl_editOwnershipNode_owner" htmlFor="dd_editOwnershipNode_owner_sel">
                                    {resourceProvider.read('owner')}</label>
                                <div className="ep-control">
                                    <Select classNamePrefix="ep-select" className="ep-select" id="dd_editOwnershipNode_owner"
                                        placeholder={resourceProvider.read('select')} inputId="dd_editOwnershipNode_owner_sel"
                                        options={owners}
                                        getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                                        onChange={this.onChangeDropdown}
                                        value={this.props.movementInventoryfilters.owner}
                                        components={{
                                            IndicatorSeparator: () => null,
                                            ClearIndicator: () => null
                                        }}
                                        filterOption={utilities.selectFilter()}
                                        noOptionsMessage={() => resourceProvider.read('noOptionsMessage')} />
                                </div>
                            </div>
                        </div>
                        <div className="col-md-6">
                            <span className="float-r m-t-4"><button className="ep-btn" id="btn_editOwnershipNode_newMovement" onClick={this.props.createMovement} disabled={this.disableNewMovement()}>
                                <i className="fas fa-plus-square m-r-1" /><span className="ep-btn__txt">{resourceProvider.read('newMovement')}</span></button></span>
                        </div>
                    </div>
                    <OwnershipNodeDataGrid />
                </div>
            </section>
        );
    }

    componentDidMount() {
        this.props.getCategoryElements();
        this.props.getVariableTypes();
        this.props.getMovementInventoryData(this.props.ownershipNodeId);
    }

    componentDidUpdate(prevProps) {
        if (this.props.variableTypes.length > 0 && (this.props.variableTypes !== prevProps.variableTypes || !this.props.movementInventoryfilters.variableType)) {
            const defaultVariableType = this.props.variableTypes.filter(v => v.variableTypeId === constants.VariableType.InitialInventory);
            this.onChangeDropdown(defaultVariableType[0]);
        }

        if (this.props.categoryElements.length > 0 && (this.props.categoryElements !== prevProps.categoryElements || !this.props.movementInventoryfilters.owner)) {
            const defaultOwner = this.props.categoryElements.filter(v => v.categoryId === constants.Category.Owner && v.name === 'ECOPETROL');
            this.onChangeDropdown(defaultOwner[0]);
        }

        if (!this.props.movementInventoryfilters.product && this.props.ownershipNodeBalance.length > 0) {
            const defaultProduct = this.getProducts(this.props.ownershipNodeBalance);
            this.onChangeDropdown(defaultProduct[0]);
        }
    }

    componentWillUnmount() {
        this.props.clearMovementInventoryFilter();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        ownershipNodeDetails: state.nodeOwnership.ownershipNode.nodeDetails,
        ownershipNodeBalance: state.grid.ownershipNodeBalance.items,
        categoryElements: state.shared.categoryElements,
        variableTypes: state.shared.variableTypes,
        nodeMovementInventoryData: state.nodeOwnership.ownershipNode.nodeMovementInventoryData,
        movementInventoryfilters: state.nodeOwnership.ownershipNodeDetails.movementInventoryfilters,
        currentUser: state.root.context.userId
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        getVariableTypes: () => {
            dispatch(getVariableTypes());
        },
        initUpdateFilters: filter => {
            dispatch(initUpdateFilters(filter));
        },
        getMovementInventoryData: ownershipNodeId => {
            dispatch(requestOwnershipNodeMovementInventoryData(ownershipNodeId));
        },
        createMovement: () => {
            dispatch(setMovementInventoryOwnershipData([]));
            dispatch(openModal('createMovement'));
        },
        clearMovementInventoryFilter: () => {
            dispatch(clearMovementInventoryFilter());
        },
        resetPageIndex: () => {
            dispatch(resetPageIndex('ownershipNodeData'));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(NodeDetails);
