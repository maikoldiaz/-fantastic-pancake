import React from 'react';
import { connect } from 'react-redux';
import { change, Field, FieldArray, formValueSelector, reduxForm } from 'redux-form';
import { required } from 'redux-form-validators';
import { constants } from '../../../../../../common/services/constants';
import { inputSelect } from '../../../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../../../common/services/resourceProvider';
import Wrapper from '../../../../../../common/components/wrapper/wrapper.jsx';
import { blurSourceNode, createNodeCostCenter, getActiveNodes, getDestinationNodeByNodeId, initCostCenterDuplicates, notifyNodeCostCenterDuplicates } from '../../actions';
import { navigationService } from '../../../../../../common/services/navigationService';
import { openMessageModal, closeModal, openModal, getCategoryElements, changeTab } from '../../../../../../common/actions';
import classNames from 'classnames/bind';
import { dataService } from '../../dataService';


export class AssignmentCostCenter extends React.Component {
    constructor() {
        super();

        this.movementTypeAndCostCenterComponent = this.movementTypeAndCostCenterComponent.bind(this);
        this.renderMovementTypeAndCostCenterComponent = this.renderMovementTypeAndCostCenterComponent.bind(this);
        this.submitCreateAssignmentCostCenter = this.submitCreateAssignmentCostCenter.bind(this);
        this.handleCancelButton = this.handleCancelButton.bind(this);
        this.mapDuplicateNodeCostCenter = this.mapDuplicateNodeCostCenter.bind(this);
        this.handleBlurSourceNode = this.handleBlurSourceNode.bind(this);
    }

    componentDidMount() {
        this.props.getActiveNodes();
        this.props.getCategoryElements();
        this.props.changeTabPanel('connectionAttributesPanel', 'costCenter');
    }

    componentDidUpdate() {
        const {
            nodeCostCenterDuplicates,
            isNodeCostCenterDuplicatesNotified,
            openNodeCostCenterDuplicatesModal
        } = this.props;
        const nodeCostCenterDuplicatesHasNoData = nodeCostCenterDuplicates ? nodeCostCenterDuplicates.length < 1 : true;

        if (nodeCostCenterDuplicatesHasNoData || isNodeCostCenterDuplicatesNotified) {
            return;
        }

        this.props.notifyNodeCostCenterDuplicates();
        const result = this.mapDuplicateNodeCostCenter();

        openNodeCostCenterDuplicatesModal(result);
    }

    mapDuplicateNodeCostCenter() {
        const { nodeCostCenterDuplicates, getFieldNameFromForm } = this.props;
        let movementTypeAndCostCenter = getFieldNameFromForm('movementTypeAndCostCenter');
        movementTypeAndCostCenter = Array.from(new Set(movementTypeAndCostCenter.map(JSON.stringify))).map(JSON.parse);
        return dataService.getDuplicateNodeCostCenter(nodeCostCenterDuplicates, movementTypeAndCostCenter);
    }

    movementTypeAndCostCenterComponent({
        addItem,
        removeItem,
        isLastOne = false,
        inputName = '',
        index = -1
    } = {}) {
        let movementType = this.props.groupedCategoryElements[constants.Category.MovementType];
        let costCenter = this.props.groupedCategoryElements[constants.Category.CostCenter];

        movementType = movementType && movementType.filter(m => m.isActive);
        costCenter = costCenter && costCenter.filter(m => m.isActive);

        return (<Wrapper
            isLastOne={isLastOne}
            key={`${index}`}
            onAddItem={addItem}
            onRemoveItem={removeItem ? () => removeItem(index) : undefined}>
            <div className="col-md-4">
                <div className="ep-control-group">
                    <label id="lbl_movementType_name" className="ep-label" htmlFor="txt_movementType_name">{resourceProvider.read('sourceCategoryElement')}</label>
                    <Field component={inputSelect} name={inputName === '' ? '' : `${inputName}.movementType`}
                        placeholder={resourceProvider.read('nodeType')} options={movementType}
                        getOptionLabel={x => x.name} getOptionValue={x => x.elementId} />
                </div>
            </div>
            <div className="col-md-4">
                <div className="ep-control-group">
                    <label id="lbl_costCenter_name" className="ep-label" htmlFor="txt_costCenter_name">{resourceProvider.read('costCenter')}</label>
                    <Field component={inputSelect} name={inputName === '' ? '' : `${inputName}.costCenter`}
                        placeholder={resourceProvider.read('nodeType')} options={costCenter}
                        getOptionLabel={x => x.name} getOptionValue={x => x.elementId} />
                </div>
            </div>
        </Wrapper>);
    }

    renderMovementTypeAndCostCenterComponent({ fields }) {
        const { maxNodeCostCenterBatchCreation } = this.props;
        const canAdd = fields.length < maxNodeCostCenterBatchCreation;
        const addItem = canAdd ? fields.push : undefined;

        if (!maxNodeCostCenterBatchCreation || maxNodeCostCenterBatchCreation === 0) {
            return <></>;
        }

        return (<>
            {fields.map((indexName, index) => this.movementTypeAndCostCenterComponent({
                inputName: indexName,
                index,
                removeItem: fields.length > 1 ? fields.remove : undefined
            }))}
            {canAdd && this.movementTypeAndCostCenterComponent({ isLastOne: true, addItem })}
        </>);
    }

    submitCreateAssignmentCostCenter(values) {
        const { createNodeConstCenter } = this.props;
        const sourceNodeId = values.sourceNode.nodeId;
        const destinationNodeId = values.destinationNode ? values.destinationNode.nodeId : undefined;

        const formDataParsed = values.movementTypeAndCostCenter.map(mtc => ({
            sourceNodeId,
            destinationNodeId: destinationNodeId,
            movementTypeId: mtc.movementType.elementId,
            costCenterId: mtc.costCenter.elementId,
            isActive: true
        }));

        createNodeConstCenter(formDataParsed);
    }

    handleCancelButton() {
        const { getFieldNameFromForm } = this.props;
        let movementTypeAndCostCenter = getFieldNameFromForm('movementTypeAndCostCenter');
        const sourceNode = getFieldNameFromForm('sourceNode');
        const destinationNode = getFieldNameFromForm('destinationNode');
        const navigateToManage = () => navigationService.navigateTo('manage');

        movementTypeAndCostCenter = movementTypeAndCostCenter ? movementTypeAndCostCenter : [];
        const movementTypeAndCostCenterHasValidData = movementTypeAndCostCenter.length >= 1
            && movementTypeAndCostCenter.filter(movementTCostCenter => movementTCostCenter && (movementTCostCenter.movementType || movementTCostCenter.costCenter)).length > 0;


        if (movementTypeAndCostCenterHasValidData || sourceNode || destinationNode) {
            this.props.openModal(
                {
                    title: 'assignCostCenterCancel',
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

    handleBlurSourceNode(data) {
        if (!data || !data.name) {
            this.props.onBlurSourceNode();
        }
    }

    render() {
        const sourceNode = this.props.nodes;
        const destinationNodes = this.props.destinationNodes;
        const maxNodeCostCenterBatchCreation = this.props.maxNodeCostCenterBatchCreation;

        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <form id={`frm_create_node`} className="ep-form" onSubmit={this.props.handleSubmit(this.submitCreateAssignmentCostCenter)}>
                        <div className="row">
                            <div className="col-md-3">
                                <div className="ep-control-group">

                                    <label id="lbl_sourceNode_name" className="ep-label" htmlFor="txt_sourceNode_name">{resourceProvider.read('sourceNode')}</label>
                                    <Field component={inputSelect} name="sourceNode" id="sourceNode_name_sel"
                                        placeholder={resourceProvider.read('nodeType')} options={sourceNode}
                                        getOptionLabel={x => x.name} getOptionValue={x => x.nodeId}
                                        onChange={e => this.props.getDestinationNodeByNodeId(e ? e.nodeId : undefined)}
                                        onBlur={e => this.handleBlurSourceNode(e)}
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                </div>
                            </div>
                            <div className="col-md-3">
                                <div className="ep-control-group">
                                    <label id="lbl_destinationNode_name" className="ep-label" htmlFor="txt_destinationNode_name">{resourceProvider.read('destinationNode')}</label>
                                    <Field component={inputSelect} name="destinationNode" id="dd_destinationNode_type" inputId="dd_destinationNode_type_sel"
                                        placeholder={resourceProvider.read('nodeType')} options={destinationNodes}
                                        getOptionLabel={x => x.name} getOptionValue={x => x.nodeId} />
                                </div>
                            </div>
                            <div className="col-md-12">
                                <div className="ep-control-group">
                                    <hr />
                                </div>
                            </div>
                            <div className="col-md-12">
                                <FieldArray name="movementTypeAndCostCenter" component={this.renderMovementTypeAndCostCenterComponent} />
                            </div>
                            <div className="col-md-12">
                                {maxNodeCostCenterBatchCreation > 0 &&
                                    <div className="text-right m-t-4">
                                        <button type="button" className="ep-btn ep-btn--link m-l-4 fs-14" id="btn_assignmentCostCenter_cancel" onClick={() => this.handleCancelButton()}>
                                            {resourceProvider.read('cancel')}
                                        </button>
                                        <button type="submit" className="ep-btn ep-btn--sm" id="btn_assignmentCostCenter_submit">
                                            {resourceProvider.read('submit')}
                                        </button>
                                    </div>
                                }
                            </div>
                        </div>
                    </form>
                </div>
            </section >
        );
    }
}

export const movementTypeAndCostCenterValidations = values => {
    const errors = {};

    const { movementTypeAndCostCenter } = values;
    if (!movementTypeAndCostCenter) {
        errors.movementTypeAndCostCenter = [
            { movementType: resourceProvider.read('required') },
            { costCenter: resourceProvider.read('required') }
        ];
    } else {
        errors.movementTypeAndCostCenter = movementTypeAndCostCenter.map(movementTCostCenter => {
            if (!movementTCostCenter) {
                return {
                    movementType: resourceProvider.read('required'),
                    costCenter: resourceProvider.read('required')
                };
            }
            const currentResult = {};
            const movementTypeExists = movementTCostCenter.movementType;
            const costCenterExists = movementTCostCenter.costCenter;

            if (!movementTypeExists) {
                currentResult.movementType = resourceProvider.read('required');
            }


            if (!costCenterExists) {
                currentResult.costCenter = resourceProvider.read('required');
            }


            return currentResult;
        });
    }

    return errors;
};

/* istanbul ignore next */
const mapStateToProps = state => {
    const destinationNodes = state.nodeConnection.attributes.destinationNodes;
    const selector = formValueSelector('assignmentCostCenter');
    return {
        initialValues: {
            movementTypeAndCostCenter: [undefined]
        },
        groupedCategoryElements: state.shared.groupedCategoryElements,
        nodes: state.nodeConnection.attributes.nodes,
        destinationNodes: destinationNodes ? destinationNodes.nodes : [],
        nodeCostCenterDuplicates: state.nodeConnection.nodeCostCenters.duplicates,
        isNodeCostCenterDuplicatesNotified: state.nodeConnection.nodeCostCenters.isDuplicatedNotified,
        maxNodeCostCenterBatchCreation: state.root.systemConfig.maxNodeCostCenterBatchCreation,
        getFieldNameFromForm: fieldName => selector(state, fieldName)
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getActiveNodes: () => {
            dispatch(getActiveNodes());
        },
        getDestinationNodeByNodeId: nodeId => {
            if (!nodeId) {
                return;
            }
            dispatch(change('assignmentCostCenter', 'destinationNode', undefined));
            dispatch(getDestinationNodeByNodeId(nodeId));
        },
        createNodeConstCenter: data => {
            dispatch(createNodeCostCenter(data));
        },
        openModal: (opts, message) => {
            dispatch(openMessageModal(message, opts));
        },
        closeModal: () => {
            dispatch(closeModal());
        },
        notifyNodeCostCenterDuplicates: () => {
            dispatch(notifyNodeCostCenterDuplicates());
        },
        openNodeCostCenterDuplicatesModal: data => {
            dispatch(initCostCenterDuplicates(data));
            dispatch(openModal('nodeCostCenterDuplicatesModal', '', '', '', '', classNames('ep-modal__body', 'ep-modal__body--large')));
        },
        onBlurSourceNode: () => {
            dispatch(blurSourceNode());
            dispatch(change('assignmentCostCenter', 'destinationNode', undefined));
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        changeTabPanel: (name, activeTab) => {
            dispatch(changeTab(name, activeTab));
        }
    };
};

const submitCreateAssignmentCostCenterFrom = reduxForm({
    form: 'assignmentCostCenter',
    validate: movementTypeAndCostCenterValidations
})(AssignmentCostCenter);

export default connect(mapStateToProps, mapDispatchToProps)(submitCreateAssignmentCostCenterFrom);
