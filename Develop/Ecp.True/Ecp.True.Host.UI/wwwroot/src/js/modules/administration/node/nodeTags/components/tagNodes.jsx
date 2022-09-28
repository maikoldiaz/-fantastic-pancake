import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, SubmissionError } from 'redux-form';
import { required } from 'redux-form-validators';
import { inputSelect, inputDatePicker } from '../../../../../common/components/formControl/formControl.jsx';
import { requestGroupNodeCategory, initErrorNodes } from '../actions';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { utilities } from '../../../../../common/services/utilities';
import { closeModal, showLinkError, showSuccess, getCategoryElements, showError, hideNotification } from '../../../../../common/actions';
import { dateService } from '../../../../../common/services/dateService';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';
import { refreshGrid } from '../../../../../common/components/grid/actions';

export class TagNodes extends React.Component {
    constructor() {
        super();
        this.onHandleSubmit = this.onHandleSubmit.bind(this);
        this.getLabelForNodeTag = this.getLabelForNodeTag.bind(this);
        this.getLabelForSwitchTo = this.getLabelForSwitchTo.bind(this);
        this.getDisabledRows = this.getDisabledRows.bind(this);
        this.getErrorMessage = this.getErrorMessage.bind(this);
        this.getSelectedItemsCount = this.getSelectedItemsCount.bind(this);
        this.getNodeErrorMessage = this.getNodeErrorMessage.bind(this);
    }

    getLabelForNodeTag(mode) {
        if (mode === 'new') {
            return resourceProvider.read('startDateForGrouping');
        }

        return mode === 'change' ? resourceProvider.read('startDateForGrouping') : resourceProvider.read('selectExpirationDate');
    }

    getLabelForSwitchTo(mode) {
        if (mode === 'new') {
            return resourceProvider.read('fromNode');
        }
        return mode === 'change' ? resourceProvider.read('switchToElement') : resourceProvider.read('expirationDate');
    }

    getDisabledRows() {
        return this.props.items.filter(x => x.elementId === this.props.selectedElement.elementId);
    }

    getNodeErrorMessage(mode) {
        const selectedItems = mode === 'new' ? this.props.selectedItems.filter(x => x.elementId !== this.props.selectedElement.elementId) : this.props.selectedItems;
        // get first node name
        const node = selectedItems.map(x => x.node).map(y => y.name)[0];
        // concatinatate the node with name when there are more than nodes.
        const nodeMessage = selectedItems.length > 1 ? node.concat(` (+${selectedItems.length - 1})`) : node;
        return { nodeMessage, selectedItems };
    }

    getErrorMessage(errorMessage) {
        const element = this.props.selectedElement.name;
        const category = this.props.selectedCategory.name;
        const error = this.getNodeErrorMessage(this.props.mode);

        const nodeErrorMessage = error.nodeMessage;
        let message = '';
        if (errorMessage === 'TIME_OVERLAP') {
            message = resourceProvider.readFormat(errorMessage, [nodeErrorMessage, category, element]);
        } else if (errorMessage === 'UNIQUENESS_FAILED') {
            message = resourceProvider.readFormat(errorMessage, [nodeErrorMessage, category]);
        } else {
            message = resourceProvider.read(errorMessage);
        }

        return { message, nodes: error.selectedItems };
    }

    onHandleSubmit(values) {
        let typeOfAction;
        if (this.props.mode === 'new') {
            typeOfAction = 1;
        } else {
            typeOfAction = this.props.mode === 'change' ? 2 : 3;
        }

        const taggedNodesData = [];
        const items = typeOfAction === 1 ? this.props.selectedItems.filter(x => x.elementId !== this.props.selectedElement.elementId) : this.props.selectedItems;
        const operationDate = dateService.parseFieldToISOString(values.operationDate);
        const error = resourceProvider.read('tagAssociationEndDateValidation');
        if (items.length === 0) {
            items.push(this.props.expireNode);
        }

        items.forEach(element => {
            if (element.startDate > operationDate) {
                this.props.showErrorDetail(error, true);
                throw new SubmissionError({
                    _error: error
                });
            }
        });

        items.forEach(element => {
            taggedNodesData.push({
                NodeTagId: element.nodeTagId,
                nodeId: element.nodeId
            });
        });

        const taggedNodeInfo = {
            operationalType: typeOfAction,
            elementId: this.props.mode === 'change' ? values.element.elementId : this.props.selectedElement.elementId,
            inputDate: operationDate,
            taggedNodes: taggedNodesData
        };

        this.props.groupNodeCategory(taggedNodeInfo);
    }

    getSelectedItemsCount(mode) {
        return mode === 'new' ? this.props.selectedItems.filter(x => x.elementId !== this.props.selectedElement.elementId).length : this.props.selectedItems.length;
    }

    render() {
        const selectedItemCount = this.getSelectedItemsCount(this.props.mode);
        return (
            <>
                <form className="ep-form" onSubmit={this.props.handleSubmit(this.onHandleSubmit)}>
                    <section className="ep-modal__content" id="sec_modal_group">
                        <p className="ep-modal__txt" id="p_nodeTag_select">{this.props.mode === 'expire' ?
                            resourceProvider.readFormat('selectNodesForGrouping', [selectedItemCount])
                            : resourceProvider.readFormat('selectNodeForChange', [selectedItemCount])}</p>
                        <p className="ep-modal__txt" id="p_nodeTag_selectDate">{this.getLabelForNodeTag(this.props.mode)}</p>
                        <div className="row m-t-4">
                            {
                                (this.props.mode === 'new' || this.props.mode === 'change') &&
                                <div className="col-md-6">
                                    <h1 className="ep-control-group-header" id="h1_nodeTag_selectCategoryElement">{this.props.mode === 'new' ?
                                        resourceProvider.read('nodesWillBeGroupedWith') : resourceProvider.read('categoryCurrentElement')}</h1>
                                    <div className="ep-control-group m-b-3">
                                        <label className="ep-label" id="lbl_nodeTag_categorySelected">{resourceProvider.read('category')}</label>
                                        <span className="ep-data" id="lbl_nodeTag_categorySelectedText">{this.props.selectedCategory.name}</span>
                                    </div>
                                    <div className="ep-control-group m-b-0">
                                        <label className="ep-label" id="lbl_nodeTag_elementSelected">{resourceProvider.read('element')}</label>
                                        <span className="ep-data" id="lbl_nodeTag_elementSelectedText">{this.props.selectedElement.name}</span>
                                    </div>
                                </div>
                            }
                            <div className="col-md-6">
                                <h1 className="ep-control-group-header" id="h1_nodeTag_FromNode">{this.getLabelForSwitchTo(this.props.mode)}</h1>
                                {
                                    (this.props.mode === 'change') &&
                                    <div className="ep-control-group m-b-3">
                                        <label className="ep-label" id="lbl_nodeTag_changeElement" htmlFor="dp_nodeTag_changeElement_sel">{resourceProvider.read('element')}</label>
                                        <Field component={inputSelect} placeholder="element" name="element" inputId="dp_nodeTag_changeElement_sel"
                                            getOptionLabel={x => x.name} getOptionValue={x => x.elementId} options={this.props.elements}
                                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                    </div>
                                }
                                <div className="ep-control-group m-b-0">
                                    <label className="ep-label sr-only" htmlFor="dp_nodeTag_operationDate">{resourceProvider.read('operationDate')}</label>
                                    <Field id="dp_nodeTag_operationDate" minDate={new Date()} component={inputDatePicker} name="operationDate"
                                        placeholder={resourceProvider.read('date')} validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                </div>
                            </div>
                        </div>
                    </section>
                    <ModalFooter config={footerConfigService.getCommonConfig('nodeTag')} />
                </form>

            </>
        );
    }
    componentDidMount() {
        this.props.getCategoryElements();
        this.props.hideError();
    }

    componentDidUpdate(prevProps) {
        if (this.props.failureToggler !== prevProps.failureToggler) {
            this.props.closeModal();
            const error = this.getErrorMessage(this.props.message);
            this.props.showError(error.message, error.nodes);
        }

        if (this.props.refreshToggler !== prevProps.refreshToggler) {
            this.props.refreshGrid();
            this.props.closeModal();
            this.props.showSuccess(resourceProvider.read('nodeTaggingSuccess'));
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    const filterState = state.categoryElementFilter.nodeTags;
    const selectedCategory = filterState && filterState.values.categoryElements[0].category ? filterState.values.categoryElements[0].category : {};
    const selectedElement = filterState && filterState.values.categoryElements[0].element ? filterState.values.categoryElements[0].element : {};
    let categoryElements = [];
    if (!utilities.isNullOrUndefined(selectedCategory)) {
        categoryElements = state.shared.categoryElements.filter(x => x.categoryId === selectedCategory.categoryId);
    }

    if (!utilities.isNullOrUndefined(selectedElement) && categoryElements.length > 0) {
        categoryElements = categoryElements.filter(x => x.elementId !== selectedElement.elementId);
    }
    const initialValues = Object.assign({}, state.node.nodeTags.defaultValues, {
        element: null
    });
    const errorResponse = state.node.nodeTags.errorResponse;
    const message = errorResponse && errorResponse.errorCodes ? errorResponse.errorCodes[0].message : '';
    return {
        initialValues,
        elements: categoryElements,
        selectedElement,
        selectedCategory,
        items: state.grid.nodeTags.items,
        selectedItems: state.grid.nodeTags.selection,
        refreshToggler: state.node.nodeTags.refreshToggler,
        failureToggler: state.node.nodeTags.failureToggler,
        expireNode: state.node.nodeTags.expireNode,
        message
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        groupNodeCategory: taggedNodeInfo => {
            dispatch(requestGroupNodeCategory(taggedNodeInfo));
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        closeModal: () => {
            dispatch(closeModal());
        },
        refreshGrid: () => {
            dispatch(refreshGrid('nodeTags'));
        },
        showError: (message, nodes) => {
            dispatch(initErrorNodes(nodes));
            dispatch(showLinkError(message, 'error', 'nodeGroupError', 'nodeGroupErrorLink'));
        },
        showErrorDetail: (message, showOnModal, title) => {
            dispatch(showError(message, showOnModal, title));
        },
        showSuccess: message => {
            dispatch(showSuccess(message));
        },
        hideError: () => {
            dispatch(hideNotification());
        }
    };
};

/* istanbul ignore next */
export const tagNodes = connect(mapStateToProps, mapDispatchToProps, utilities.merge)(reduxForm({ form: 'tagNodes' })(TagNodes));
