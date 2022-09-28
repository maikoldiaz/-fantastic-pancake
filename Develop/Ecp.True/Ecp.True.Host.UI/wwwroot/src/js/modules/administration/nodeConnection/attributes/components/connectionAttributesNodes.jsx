import React from 'react';
import { connect } from 'react-redux';
import { change, Field, FieldArray, formValueSelector, reduxForm } from 'redux-form';
import { closeModal, getCategoryElements, openMessageModal, openModal } from '../../../../../common/actions.js';
import { inputSelect } from '../../../../../common/components/formControl/formControl.jsx';
import { Wrapper } from '../../../../../common/components/wrapper/wrapper.jsx';
import { constants } from '../../../../../common/services/constants.js';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { createNodeConnection, getNodesBySegmentId, initCostCenterDuplicates, notifyNodeCostCenterDuplicates } from '../actions.js';
import { dataService } from '../dataService.js';
import classNames from 'classnames/bind';
import { navigationService } from '../../../../../common/services/navigationService.js';


export class ConnectionAttributesNodes extends React.Component {
    constructor() {
        super();
        this.connectionComponent = this.connectionComponent.bind(this);
        this.renderConnectionComponent = this.renderConnectionComponent.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
        this.handleSourceSegmentChanges = this.handleSourceSegmentChanges.bind(this);
        this.handleCancelButton = this.handleCancelButton.bind(this);
    }

    componentDidMount() {
        this.props.getCategoryElements();
    }

    componentDidUpdate() {
        const {
            nodeConnectionDuplicates = [],
            isNodeConnectionDuplicatesNotified,
            getFieldNameFromForm,
            openNodeCostCenterDuplicatesModal
        } = this.props;

        const nodeConnectionHasNoData = nodeConnectionDuplicates.length < 1;

        if (nodeConnectionHasNoData || isNodeConnectionDuplicatesNotified) {
            return;
        }

        let nodeConnections = getFieldNameFromForm('connections');

        nodeConnections = Array.from(new Set(nodeConnections.map(JSON.stringify))).map(JSON.parse);

        const result = dataService.getDuplicateNodeConnection(nodeConnectionDuplicates, nodeConnections);

        this.props.notifyNodeCostCenterDuplicates();

        openNodeCostCenterDuplicatesModal(result);
    }

    handleCancelButton() {
        const { getFieldNameFromForm } = this.props;
        let connections = getFieldNameFromForm('connections');

        const navigateToManage = () => navigationService.navigateTo('manage');

        connections = connections ? connections : [];
        const connectionsHasValidData = connections.length >= 1
            && connections.filter(conn => conn && (
                conn.sourceSegment ||
                conn.sourceNode ||
                conn.destinationSegment ||
                conn.destinationNode)
            ).length > 0;


        if (connectionsHasValidData) {
            this.props.openModal(
                {
                    title: 'saveCostCenterWithDuplicates',
                    canCancel: true,
                    acceptAction: () => {
                        this.props.closeModal();
                        navigateToManage();
                    }
                },
                resourceProvider.read('assignCostCenterCancelMessage')
            );
        } else {
            navigateToManage();
        }
    }


    handleSourceSegmentChanges(data = {}, index, isSource = true) {
        const { onSegmentChanges, onBlurNodes } = this.props;
        if (data) {
            const { elementId = -1 } = data;
            onSegmentChanges(elementId, index, isSource);
        }

        onBlurNodes(index, isSource);
    }

    connectionComponent({
        addItem,
        removeItem,
        isLastOne = false,
        index = -1,
        inputName = ''
    } = {}) {
        const { groupedCategoryElements = {}, newConnections = {} } = this.props;

        let segments = groupedCategoryElements[constants.Category.Segment] || [];
        let sourceNodes = newConnections[index] ? newConnections[index].sourceNodes || [] : [];
        let destinationNodes = newConnections[index] ? newConnections[index].destinationNodes || [] : [];

        segments = segments.filter(s => s.isActive);
        sourceNodes = sourceNodes.filter(n => n.isActive);
        destinationNodes = destinationNodes.filter(n => n.isActive);
        return (<Wrapper
            isLastOne={isLastOne}
            key={index}
            onAddItem={addItem}
            onRemoveItem={removeItem ? () => removeItem(index) : undefined}>

            <div className="col-md-3">
                <div className="ep-control-group">
                    <label id="lbl_sourceSegment_name" className="ep-label" htmlFor="txt_sourceSegment_name">{resourceProvider.read('sourceSegment')}</label>
                    <Field component={inputSelect} name={inputName === '' ? '' : `${inputName}.sourceSegment`}
                        placeholder={resourceProvider.read('nodeType')} options={segments}
                        onChange={d => this.handleSourceSegmentChanges(d, index)}
                        getOptionLabel={x => x.name} getOptionValue={x => x.elementId} />
                </div>
            </div>
            <div className="col-md-3">
                <div className="ep-control-group">
                    <label id="lbl_sourceNode_name" className="ep-label" htmlFor="txt_sourceNode_name">{resourceProvider.read('sourceNode')}</label>
                    <Field component={inputSelect} name={inputName === '' ? '' : `${inputName}.sourceNode`}
                        placeholder={resourceProvider.read('nodeType')} options={sourceNodes}
                        getOptionLabel={x => x.name} getOptionValue={x => x.nodeId} />
                </div>
            </div>
            <div className="col-md-3">
                <div className="ep-control-group">
                    <label id="lbl_destinationSegment_name" className="ep-label" htmlFor="txt_destinationSegment_name">{resourceProvider.read('destinationSegment')}</label>
                    <Field component={inputSelect} name={inputName === '' ? '' : `${inputName}.destinationSegment`}
                        placeholder={resourceProvider.read('nodeType')} options={segments}
                        onChange={d => this.handleSourceSegmentChanges(d, index, false)}
                        getOptionLabel={x => x.name} getOptionValue={x => x.elementId} />
                </div>
            </div>
            <div className="col-md-3">
                <div className="ep-control-group">
                    <label id="lbl_destinationNode_name" className="ep-label" htmlFor="txt_destinationNode_name">{resourceProvider.read('destinationNode')}</label>
                    <Field component={inputSelect} name={inputName === '' ? '' : `${inputName}.destinationNode`}
                        placeholder={resourceProvider.read('nodeType')} options={destinationNodes}
                        getOptionLabel={x => x.name} getOptionValue={x => x.nodeId} />
                </div>
            </div>
        </Wrapper>);
    }

    renderConnectionComponent({ fields }) {
        const { maxNodeConnectionCreationEdition } = this.props;
        const canAdd = fields.length < maxNodeConnectionCreationEdition;
        const addItem = canAdd ? fields.push : undefined;

        return (<>
            {fields.map((indexName, index) => this.connectionComponent({
                inputName: indexName,
                index,
                removeItem: fields.length > 1 ? fields.remove : undefined
            }))}
            {canAdd && this.connectionComponent({ isLastOne: true, addItem })}
        </>);
    }

    onSubmit(values) {
        const { connections } = values;

        const result = connections.map(c => ({
            sourceNodeId: c.sourceNode.nodeId,
            destinationNodeId: c.destinationNode.nodeId,
            isActive: true
        }));
        this.props.createNodeConnection(result);
    }

    render() {
        const { maxNodeConnectionCreationEdition } = this.props;
        return (<section className="ep-content">
            <div className="ep-content__body">
                <form id={`frm_create_node`} className="ep-form" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="ep-control-group">
                                <h3 className="fs-16 fw-bold fas--primary m-b-2" id="txt_segments_left_title">{resourceProvider.read('newConnection')}</h3>
                                <p className="m-b-6 fs-16">
                                    {resourceProvider.read('newConnectionDescription')}
                                </p>
                            </div>
                        </div>
                        <div className="col-md-12">
                            <FieldArray name="connections" component={this.renderConnectionComponent} />
                        </div>
                        <div className="col-md-12">
                            {maxNodeConnectionCreationEdition > 0 &&
                                <div className="text-right m-t-4">
                                    <button type="button" className="ep-btn ep-btn--link m-l-4 fs-14" id="btn_nodeConnections_cancel" onClick={() => this.handleCancelButton()}>
                                        {resourceProvider.read('cancel')}
                                    </button>
                                    <button type="submit" className="ep-btn ep-btn--sm" id="btn_nodeConnections_submit">
                                        {resourceProvider.read('submit')}
                                    </button>
                                </div>
                            }
                        </div>
                    </div>
                </form>
            </div>
        </section>);
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    const selector = formValueSelector('connectionAttributesNodes');
    return {
        initialValues: {
            connections: [undefined]
        },
        groupedCategoryElements: state.shared.groupedCategoryElements,
        newConnections: state.nodeConnection.attributes.newConnections,
        nodeConnectionDuplicates: state.nodeConnection.nodeCostCenters.duplicates,
        isNodeConnectionDuplicatesNotified: state.nodeConnection.nodeCostCenters.isDuplicatedNotified,
        getFieldNameFromForm: fieldName => selector(state, fieldName),
        maxNodeConnectionCreationEdition: state.root.systemConfig.maxNodeConnectionCreationEdition

    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        onSegmentChanges: (segmentIdSelected, position, isSource) => {
            dispatch(getNodesBySegmentId(segmentIdSelected, position, isSource));
        },
        onBlurNodes: (position, isSource) => {
            const sourceOrDestination = isSource ? 'source' : 'destination';
            dispatch(change('connectionAttributesNodes', `connections[${position}].${sourceOrDestination}Node`, undefined));
        },
        createNodeConnection: body => {
            dispatch(createNodeConnection(body));
        },
        openNodeCostCenterDuplicatesModal: data => {
            dispatch(initCostCenterDuplicates(data));
            dispatch(openModal('nodeConnectionDuplicatesModal', '', '', '', '', classNames('ep-modal__body', 'ep-modal__body--large')));
        },
        notifyNodeCostCenterDuplicates: () => {
            dispatch(notifyNodeCostCenterDuplicates());
        },
        openModal: (opts, message) => {
            dispatch(openMessageModal(message, opts));
        },
        closeModal: () => {
            dispatch(closeModal());
        }
    };
};

const connectionAttributesNodesForm = reduxForm({
    form: 'connectionAttributesNodes',
    validate: values => dataService
        .validationsForRequiredFieldArrayItems(
            values,
            {
                fieldName: 'connections',
                requiredItems: ['sourceSegment', 'sourceNode', 'destinationSegment', 'destinationNode']
            })
})(ConnectionAttributesNodes);

export default connect(mapStateToProps, mapDispatchToProps)(connectionAttributesNodesForm);
