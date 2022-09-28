import React from 'react';
import { connect } from 'react-redux';
import { required } from 'redux-form-validators';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { inputSelect, inputAutocomplete } from '../../../../common/components/formControl/formControl.jsx';
import { Field, change } from 'redux-form';
import {
    getDestinationNodes, getSourceProducts, getDestinationProducts, requestSearchSourceNodes,
    clearSearchSourceNodes, clearTransformationData
} from '../actions';
import { constants } from './../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';
import { hideNotification } from '../../../../common/actions';

class TransformationSection extends React.Component {
    constructor() {
        super();

        this.isMovements = this.isMovements.bind(this);
        this.getSourceName = this.getSourceName.bind(this);
        this.getProductName = this.getProductName.bind(this);
        this.isDisabled = this.isDisabled.bind(this);
        this.getSourceNodes = this.getSourceNodes.bind(this);
        this.searchNodes = this.searchNodes.bind(this);
        this.clearFields = this.clearFields.bind(this);
        this.selectSourceNode = this.selectSourceNode.bind(this);
        this.onNodeChange = this.onNodeChange.bind(this);
    }

    isMovements() {
        return this.props.activeTab === 'movements';
    }

    clearFields() {
        this.props.clearField(`${this.props.name}.destinationNode`);
        this.props.clearField(`${this.props.name}.sourceProduct`);
        this.props.clearField(`${this.props.name}.destinationProduct`);
        this.props.clearTransformationData();
        this.props.hideNotification();
    }

    searchNodes(searchText) {
        this.clearFields();
        return utilities.isNullOrWhitespace(searchText) ? this.props.clearNodes() : this.props.searchNodes(searchText);
    }

    selectSourceNode(searchNode) {
        this.clearFields();
        this.props.selectSourceNode(searchNode);
    }

    getSourceName() {
        return this.isMovements() ? 'transformationSourceNode' : 'node';
    }

    getProductName() {
        return this.isMovements() ? 'transformationSourceProduct' : 'transformationProduct';
    }

    isDisabled() {
        return this.props.mode === constants.Modes.Update && this.props.name === 'origin';
    }

    getSourceNodes() {
        return this.props.searchedSourceNodes.filter(x => this.props.sourceNodes.map(n => n.nodeId).includes(x.nodeId)).slice(0, 5);
    }

    renderItem(item, highlight) {
        return (
            <div style={{ padding: '10px 12px', background: highlight ? '#eee' : '#fff' }}>
                {item.name}
            </div>
        );
    }

    onNodeChange(selectedItem) {
        if (utilities.isNullOrUndefined(selectedItem)) {
            return;
        }
        this.props.getDestinationProducts(selectedItem.nodeId);
    }

    render() {
        return (
            <>
                <div className="ep-control-group">
                    <label className="ep-label">{resourceProvider.read(this.getSourceName())}</label>
                    <Field type="text" id={`dd_${this.props.name}_sourceNode`} disabled={this.isDisabled()} component={inputAutocomplete} name="sourceNode"
                        onSelect={this.selectSourceNode} shouldChangeValueOnSelect={true} onChange={this.searchNodes} defaultValue={this.props.sourceNode.name}
                        shouldItemRender={(item, value) => item.name.toLowerCase().indexOf(value.toLowerCase()) > -1}
                        renderItem={this.renderItem} items={this.getSourceNodes()} getItemValue={n => n.name}
                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                </div>
                {this.isMovements() &&
                    <div className="ep-control-group">
                        <label className="ep-label" htmlFor={`dd_${this.props.name}_destinationNode_sel`}>{resourceProvider.read('transformationDestinationNode')}</label>
                        <Field id={`dd_${this.props.name}_destinationNode`} isDisabled={this.isDisabled()} component={inputSelect} name="destinationNode"
                            onChange={this.onNodeChange} inputId={`dd_${this.props.name}_destinationNode_sel`}
                            options={this.props.destinationNodes} getOptionLabel={x => x.name} getOptionValue={x => x.nodeId}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />

                    </div>}

                <div className="ep-control-group">
                    <label className="ep-label" htmlFor={`dd_${this.props.name}_sourceProduct_sel`}>{resourceProvider.read(this.getProductName())}</label>
                    <Field id={`dd_${this.props.name}_sourceProduct`} isDisabled={this.isDisabled()} component={inputSelect} name="sourceProduct"
                        options={this.props.sourceProducts} getOptionLabel={x => x.product.name} getOptionValue={x => x.productId} inputId={`dd_${this.props.name}_sourceProduct_sel`}
                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                </div>

                {this.isMovements() &&
                    <div className="ep-control-group">
                        <label className="ep-label" htmlFor={`dd_${this.props.name}_destinationProduct_sel`}>{resourceProvider.read('transformationDestinationProduct')}</label>
                        <Field id={`dd_${this.props.name}_destinationProduct`} isDisabled={this.isDisabled()} component={inputSelect} name="destinationProduct"
                            options={this.props.destinationProducts} getOptionLabel={x => x.product.name} getOptionValue={x => x.productId} inputId={`dd_${this.props.name}_destinationProduct_sel`}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                    </div>}

                <div className="ep-control-group">
                    <label className="ep-label" htmlFor={`dd_${this.props.name}_measurementUnit_sel`}>{resourceProvider.read('transformationMeasurement')}</label>
                    <Field id={`dd_${this.props.name}_measurementUnit`} isDisabled={this.isDisabled()} component={inputSelect} name="measurementUnit"
                        options={this.props.units} getOptionLabel={x => x.name} getOptionValue={x => x.elementId} inputId={`dd_${this.props.name}_measurementUnit_sel`}
                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                </div>
            </>
        );
    }

    componentDidMount() {
        if (this.props.mode === constants.Modes.Update) {
            this.props.changeField(`${this.props.name}SourceNode`, this.props.sourceNode);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        infoToggler: state.transformSettings.infoToggler,
        sourceNodes: state.transformSettings[ownProps.name].sourceNodes,
        destinationNodes: state.transformSettings[ownProps.name].destinationNodes,
        sourceProducts: state.transformSettings[ownProps.name].sourceProducts,
        destinationProducts: state.transformSettings[ownProps.name].destinationProducts,
        activeTab: state.tabs.transformSettingsPanel.activeTab,
        units: state.transformSettings[ownProps.name].units,
        searchedSourceNodes: state.transformSettings[ownProps.name].searchedSourceNodes ? state.transformSettings[ownProps.name].searchedSourceNodes : [],
        sourceNode: state.transformSettings.initialValues ? state.transformSettings.initialValues[ownProps.name].sourceNode : {}
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        getDestinationProducts: nodeId => {
            dispatch(getDestinationProducts(nodeId, ownProps.name));
            dispatch(change('transformation', `${ownProps.name}.destinationProduct`, null));
        },
        searchNodes: searchText => {
            dispatch(requestSearchSourceNodes(searchText, ownProps.name));
        },
        clearNodes: () => {
            dispatch(clearSearchSourceNodes(ownProps.name));
        },
        selectSourceNode: sourceNode => {
            dispatch(change('transformation', `${ownProps.name}SourceNode`, sourceNode));
            dispatch(getDestinationNodes(sourceNode.nodeId, ownProps.name));
            dispatch(getSourceProducts(sourceNode.nodeId, ownProps.name));
        },
        changeField: (field, value) => {
            dispatch(change('transformation', field, value));
        },
        clearField: field => {
            dispatch(change('transformation', field, null));
        },
        clearTransformationData: () => {
            dispatch(clearTransformationData(ownProps.name));
        },
        hideNotification: () => {
            dispatch(hideNotification());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(TransformationSection);
