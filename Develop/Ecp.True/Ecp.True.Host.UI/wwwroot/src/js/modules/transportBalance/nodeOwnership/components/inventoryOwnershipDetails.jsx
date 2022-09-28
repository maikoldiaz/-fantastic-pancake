import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, SubmissionError } from 'redux-form';
import { required, length } from 'redux-form-validators';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import { inputSelect, inputTextarea } from '../../../../common/components/formControl/formControl.jsx';
import OwnershipMovementInventoryGrid from './ownershipMovementInventoryGrid.jsx';
import { updateNodeMovementInventoryData, setMovementInventoryOwnershipData, startEdit } from '../actions';
import { showError, showInfoWithButton } from './../../../../common/actions';
import { utilities } from '../../../../common/services/utilities';
import { dateService } from '../../../../common/services/dateService';
import Tooltip from '../../../../common/components/tooltip/tooltip.jsx';
import { dataService } from '../services/dataService';
import { ownersService } from '../services/ownersService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

class InventoryOwnershipDetails extends React.Component {
    constructor() {
        super();
        this.isEditor = this.isEditor.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
    }

    isEditor() {
        const nodeOwnershipStatus = this.props.nodeDetails.ownershipStatus;
        const editor = this.props.nodeDetails.editor;
        if (nodeOwnershipStatus !== constants.OwnershipNodeStatus.LOCKED || (nodeOwnershipStatus === constants.OwnershipNodeStatus.LOCKED && this.props.currentUser === editor)) {
            return true;
        }
        return false;
    }

    sumPercentage(total, num) {
        return parseFloat(total ? parseFloat(total).toFixed(2) : 0) + parseFloat(num ? parseFloat(num).toFixed(2) : 0);
    }

    onSubmit(formValues) {
        if (this.isEditor()) {
            let validVolumeValues = true;
            let validPercentageValues = true;
            this.props.movementInventoryOwnershipData.forEach(x => {
                if (utilities.isNullOrWhitespace(x.ownershipVolume) || (Math.sign(x.ownershipVolume) === -1)) {
                    validVolumeValues = false;
                }
                if (utilities.isNullOrWhitespace(x.ownershipPercentage) || (Math.sign(x.ownershipPercentage) === -1)) {
                    validPercentageValues = false;
                }
            });
            if (validVolumeValues && validPercentageValues) {
                const totalPercentage = this.props.movementInventoryOwnershipData.map(a => a.ownershipPercentage);

                if (utilities.parseFloat(totalPercentage.reduce(this.sumPercentage)) !== utilities.parseFloat(100)) {
                    const error = resourceProvider.read('percentageValidationMessage');
                    this.props.showError(error, true);
                    throw new SubmissionError({ _error: error });
                }

                const totalVolume = this.props.movementInventoryOwnershipData.reduce((a, b) => (parseFloat(a) + parseFloat(b.ownershipVolume ? parseFloat(b.ownershipVolume).toFixed(2) : 0)), 0);

                if (utilities.parseFloat(totalVolume) !== utilities.parseFloat(this.props.movementInventoryOwnershipData[0].netVolume)) {
                    const error = resourceProvider.read('inventoryOwnershipError');
                    this.props.showError(error, true);
                    throw new SubmissionError({ _error: error });
                } else {
                    const categoryElementId = formValues.reasonForChange.elementId;
                    const categoryElementName = formValues.reasonForChange.name;
                    const comment = formValues.comment;
                    const movementInventoryOwnershipData = Array.from(this.props.movementInventoryOwnershipData);
                    movementInventoryOwnershipData.forEach(item => {
                        item.reasonId = categoryElementId;
                        item.reason = categoryElementName;
                        item.comment = comment;
                        item.status = !item.status ? constants.Modes.Update : item.status;
                        item.ownershipFunction = 'Propiedad Manual';
                        item.ruleVersion = 1;
                    });
                    this.props.updateNodeMovementInventoryData(movementInventoryOwnershipData);
                    this.props.startEdit(this.props.startEditToggler);
                    this.props.closeModal();
                }
            } else {
                if (!validVolumeValues) {
                    const error = resourceProvider.read('inventoryVolumeError');
                    this.props.showError(error, true);
                    throw new SubmissionError({ _error: error });
                }

                if (!validPercentageValues) {
                    const error = resourceProvider.read('inventoryPercentageError');
                    this.props.showError(error, true);
                    throw new SubmissionError({ _error: error });
                }
            }
        } else {
            this.props.closeModal();
            const message = resourceProvider.readFormat('balanceIsBeingEditedMessage', [this.props.nodeDetails.editor]);
            const title = resourceProvider.read('balanceIsBeingEditedTitle');
            this.props.showInfoWithButton(message, title, 'requestUnlockButtonText');
        }
    }

    render() {
        const reasonforChange = this.props.groupedCategoryElements[constants.Category.ReasonForChange];
        const operativeInformation = this.props.movementInventoryOwnershipData ? this.props.movementInventoryOwnershipData[0] : [];
        const isReadMode = this.props.mode === constants.Modes.Read;
        const isShowReason = isReadMode ? this.props.initialValues.reasonForChange : true;
        const isShowComment = isReadMode ? this.props.initialValues.comment : true;
        const inventoryDataArray = dataService.buildInventoryDataTable(operativeInformation);

        return (
            <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                <section className="ep-modal__content">
                    <h2 className="ep-control-group-title text-uppercase">{resourceProvider.read('operativeInformation')}</h2>
                    <section className="ep-table-wrap">
                        <div className="ep-table ep-table--smpl m-b-6">
                            <table role="grid">
                                <thead>
                                    <tr>
                                        {inventoryDataArray.map(inv => {
                                            return (<th key={inv.key}
                                                className={`${utilities.hasProperty(inv, 'isVolume') || utilities.hasProperty(inv, 'isOwnershipFunction') ? 'text-right' : ''}`}>
                                                <Tooltip body={resourceProvider.read(inv.key)} overlayClassName="ep-tooltip--lt">{resourceProvider.read(inv.key)}</Tooltip></th>);
                                        })}
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        {inventoryDataArray.map(inv => {
                                            return (<td key={inv.key}
                                                className={`${utilities.hasProperty(inv, 'isVolume') || utilities.hasProperty(inv, 'isOwnershipFunction') ? 'text-right' : ''}`}>
                                                {utilities.hasProperty(inv, 'isDate') ? dateService.format(inv.value) : inv.value}</td>);
                                        })}
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </section>
                    <div className="ep-pane">
                        <h2 className="ep-control-group-title text-uppercase">{resourceProvider.read('owners')}</h2>
                        <div className="ep-actionbar">
                            {!isReadMode && <button id="btn_inventoryOwnership_AddOwner" className="ep-btn m-l-2" type="button"
                                onClick={() => this.props.dispatchAction(ownersService.getOwnersData(this.props, 'inventoryOwnershipDetails'))}>{resourceProvider.read('addOwner')}</button>}
                        </div>
                    </div>
                    <div className="m-b-6">
                        <OwnershipMovementInventoryGrid {...this.props} />
                    </div>
                    {isShowReason &&
                        <div className="row">
                            <div className="col-md-4">
                                <div className="ep-control-group">
                                    <label id="lbl_inventoryOwnership_reasonForChange" className="ep-label" htmlFor="dd_inventoryOwnership_reasonForChange_sel">
                                        {resourceProvider.read('reasonForChange')}:</label>
                                    <Field id="dd_inventoryOwnership_reasonForChange" component={inputSelect} name="reasonForChange" isDisabled={isReadMode}
                                        options={reasonforChange} getOptionLabel={x => x.name} getOptionValue={x => x.elementId} inputId="dd_inventoryOwnership_reasonForChange_sel"
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                </div>
                            </div>
                        </div>
                    }
                    {isShowComment &&
                        <div className="ep-control-group">
                            <label className="ep-label fs-16 text-uppercase">{resourceProvider.read('comments')}</label>
                            <Field type="text" id=" " component={inputTextarea}
                                name="comment" readOnly={isReadMode}
                                validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                    length({ max: 150, msg: resourceProvider.read('shortNameLengthValidation') })]} />
                        </div>
                    }
                </section>
                {isReadMode &&
                    <ModalFooter config={footerConfigService.getCancelConfig('inventoryOwnershipDetails',
                        { cancelText: 'accept', cancelClassName: 'ep-btn ep-btn--sm' })} />
                }
                {!isReadMode &&
                    <ModalFooter config={footerConfigService.getCommonConfig('inventoryOwnershipDetails')} />
                }
            </form>
        );
    }
    componentDidUpdate(prevProps) {
        if (prevProps.inventoryOwnersDataToggler !== this.props.inventoryOwnersDataToggler) {
            this.props.dispatchAction(ownersService.addOwnersData(this.props, 'inventoryOwnershipDetails'));
        }
    }
}

const mapStateToProps = state => {
    return {
        groupedCategoryElements: state.shared.groupedCategoryElements,
        movementInventoryOwnershipData: state.nodeOwnership.ownershipNode.movementInventoryOwnershipData,
        inventoryOwners: state.nodeOwnership.ownershipNode.inventoryOwners,
        inventoryOwnersDataToggler: state.nodeOwnership.ownershipNode.inventoryOwnersDataToggler,
        initialValues: state.nodeOwnership.ownershipNode.initialValues,
        currentUser: state.root.context.userId,
        startEditToggler: state.nodeOwnership.ownershipNode.startEditToggler,
        nodeDetails: state.nodeOwnership.ownershipNode.nodeDetails,
        editorInfo: state.nodeOwnership.ownershipNode.editorInfo
    };
};

const mapDispatchToProps = dispatch => {
    return {
        showInfoWithButton: (message, title, buttonText) => {
            dispatch(showInfoWithButton(message, false, title, buttonText));
        },
        showError: (message, showOnModal, title) => {
            dispatch(showError(message, showOnModal, title));
        },
        updateNodeMovementInventoryData: movementInventoryOwnershipData => {
            dispatch(updateNodeMovementInventoryData(movementInventoryOwnershipData));
        },
        setMovementInventoryOwnershipData: data => {
            dispatch(setMovementInventoryOwnershipData(data));
        },
        startEdit: startEditToggler => {
            dispatch(startEdit(startEditToggler));
        },
        dispatchAction: action => {
            dispatch(action);
        }
    };
};

const InventoryOwnershipEditForm = reduxForm({ form: 'inventoryOwnershipDetails', enableReinitialize: true })(InventoryOwnershipDetails);
export default connect(mapStateToProps, mapDispatchToProps)(InventoryOwnershipEditForm);
