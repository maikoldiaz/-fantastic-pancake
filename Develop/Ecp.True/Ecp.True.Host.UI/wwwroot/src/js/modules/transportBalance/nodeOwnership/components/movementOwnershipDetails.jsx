import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, SubmissionError } from 'redux-form';
import { required, length } from 'redux-form-validators';
import { inputSelect, inputTextarea } from './../../../../common/components/formControl/formControl.jsx';
import { updateNodeMovementInventoryData, setContractData, startEdit, displayDataDropdown, clearSelectedContract } from '../actions';
import { showInfoWithButton, showError } from './../../../../common/actions';
import { constants } from './../../../../common/services/constants';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import OwnershipMovementInventoryGrid from './ownershipMovementInventoryGrid.jsx';
import { utilities } from '../../../../common/services/utilities';
import { dateService } from '../../../../common/services/dateService';
import Tooltip from '../../../../common/components/tooltip/tooltip.jsx';
import { dataService } from '../services/dataService';
import { ownersService } from '../services/ownersService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

class MovementOwnershipDetails extends React.Component {
    constructor() {
        super();
        this.isEditor = this.isEditor.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
        this.isContractEdit = this.isContractEdit.bind(this);
    }

    updateMovementInventoryData(formValues, mode) {
        const movementInventoryOwnershipData = dataService.updateMovementInventoryOwnershipObject(formValues, Array.from(this.props.movementInventoryOwnershipData), true, mode);
        this.props.updateNodeMovementInventoryData(movementInventoryOwnershipData);
        this.props.startEdit(this.props.startEditToggler);
        this.props.closeModal();
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
        const isDeleteMode = this.props.mode === constants.Modes.Delete;
        if (this.isEditor()) {
            if (isDeleteMode) {
                this.updateMovementInventoryData(formValues, this.props.mode);
                return;
            }
            let validVolumeValues = true;
            let validPercentageValues = true;
            this.props.movementInventoryOwnershipData.forEach(x => {
                const isContractChanged = this.props.selectedContract;

                if (utilities.isNullOrWhitespace(x.ownershipVolume) || (Math.sign(x.ownershipVolume) === -1)) {
                    validVolumeValues = false;
                }
                if (utilities.isNullOrWhitespace(x.ownershipPercentage) || (Math.sign(x.ownershipPercentage) === -1)) {
                    validPercentageValues = false;
                }
                x.documentNumber = isContractChanged ? this.props.selectedContract.documentNumber : x.documentNumber;
                x.position = isContractChanged ? this.props.selectedContract.position : x.position;
                x.contractId = isContractChanged ? this.props.selectedContract.contractId : x.contractId;
                x.ownershipFunction = 'Propiedad Manual';
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
                    const error = resourceProvider.read('movementOwnershipError');
                    this.props.showError(error, true);
                    throw new SubmissionError({ _error: error });
                } else {
                    this.updateMovementInventoryData(formValues, this.props.mode);
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

    isContractEdit(movementType) {
        return (movementType === constants.MovementType.Compra || movementType === constants.MovementType.Venta) && this.props.mode === constants.Modes.Update;
    }

    render() {
        const reasonforChange = this.props.groupedCategoryElements[constants.Category.ReasonForChange];
        const operativeInformation = this.props.movementInventoryOwnershipData ? this.props.movementInventoryOwnershipData[0] : [];
        const isReadMode = this.props.mode === constants.Modes.Read;
        const isDeleteMode = this.props.mode === constants.Modes.Delete;
        const isShowReason = isReadMode ? this.props.initialValues.reasonForChange : true;
        const isShowComment = isReadMode ? this.props.initialValues.comment : true;
        const movementData = dataService.buildMovementDataTable(operativeInformation, this.isContractEdit(operativeInformation.movementType), this.props.selectedContract);
        const movementOriginArray = [constants.MovementType.Compra, constants.MovementType.Venta, constants.MovementType.AceEntrada, constants.MovementType.AceSalida];

        return (
            <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                <section className="ep-modal__content">
                    <h2 className="ep-control-group-title text-uppercase">{resourceProvider.read('operativeInformation')}</h2>
                    <div className="ep-label-wrap">
                        <label className="ep-label">{resourceProvider.read('movementType')}:</label><span className="ep-data m-l-2 fw-600" >{operativeInformation.movementType}</span>
                    </div>
                    <section className="ep-table-wrap">
                        <div className="ep-table ep-table--smpl m-b-6">
                            <table role="grid">
                                <thead>
                                    <tr>
                                        {
                                            movementData[0].map(mov => {
                                                return (
                                                    <th key={mov.key}>
                                                        <Tooltip body={resourceProvider.read(mov.key)} overlayClassName="ep-tooltip--lt">
                                                            {resourceProvider.read(mov.key)}
                                                        </Tooltip></th>);
                                            })
                                        }
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        {
                                            movementData[0].map(mov => {
                                                return (<td key={mov.key}>
                                                    {utilities.hasProperty(mov, 'isDate') ? dateService.format(mov.value) : mov.value}
                                                </td>);
                                            })
                                        }
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </section>
                    <section className="ep-table-wrap">
                        <div className="ep-table ep-table--smpl m-b-6">
                            <table role="grid">
                                <thead>
                                    <tr>
                                        <th>
                                            <Tooltip body={resourceProvider.read('origenMovement')} overlayClassName="ep-tooltip--lt">
                                                {resourceProvider.read('origenMovement')}
                                            </Tooltip>
                                        </th>
                                        {
                                            movementData[1].map(mov => {
                                                return (
                                                    <th key={mov.key}
                                                        className={`${utilities.hasProperty(mov, 'isVolume') || utilities.hasProperty(mov, 'isOwnershipFunction') ? 'text-right' : ''}`}>
                                                        <Tooltip body={resourceProvider.read(mov.key)} overlayClassName="ep-tooltip--lt">
                                                            {resourceProvider.read(mov.key)}
                                                        </Tooltip></th>);
                                            })}
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>{dataService.compareStatus(operativeInformation.movementType, movementOriginArray, 'eq', 'or') &&
                                            !utilities.isNullOrUndefined(operativeInformation.sourceMovementId) ? operativeInformation.sourceMovementId : constants.DashedGridCell}</td>
                                        {
                                            movementData[1].map(function (mov) {
                                                if (utilities.hasProperty(mov, 'isVolume') || utilities.hasProperty(mov, 'isOwnershipFunction')) {
                                                    return <td key={mov.key} className="text-right">{mov.value}</td>;
                                                } else if (utilities.hasProperty(mov, 'isDashedGridCell')) {
                                                    return <td key={mov.key}>{mov.value || constants.DashedGridCell}</td>;
                                                }

                                                return <td key={mov.key}>{mov.value}</td>;
                                            })
                                        }
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </section>
                    {this.isContractEdit(operativeInformation.movementType) &&
                        <>
                            <div className="ep-label-wrap">
                                <span className="ep-data fw-bold m-b-2" >{resourceProvider.read('selectContractTitle')}:</span>
                            </div>
                            <section className="ep-table-wrap">
                                <div className="ep-table ep-table--smpl ep-table--smpl-nobr m-b-6">
                                    <table role="grid">
                                        <thead>
                                            <tr>
                                                <th>
                                                    <Tooltip body={utilities.buildDocumentNumberPosition(resourceProvider.read('documentNumber'),
                                                        resourceProvider.read('position'))} overlayClassName="ep-tooltip--lt">
                                                        {utilities.buildDocumentNumberPosition(resourceProvider.read('documentNumber'), resourceProvider.read('position'))}
                                                    </Tooltip>
                                                </th>
                                                {
                                                    movementData[2].map(mov => {
                                                        return (
                                                            <th key={mov.key}>
                                                                <Tooltip body={resourceProvider.read(mov.key)} overlayClassName="ep-tooltip--lt">
                                                                    {resourceProvider.read(mov.key)}
                                                                </Tooltip></th>);
                                                    })
                                                }
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td width="210">
                                                    <div className="ep-control-group">
                                                        <Field component={inputSelect} onChange={this.props.displayData} options={this.props.dropdown} getOptionLabel={x => x.name}
                                                            getOptionValue={x => x.id} />
                                                    </div>
                                                </td>
                                                {
                                                    movementData[2].map(mov => {
                                                        if (this.props.selectedContract && this.props.displayDataToggler) {
                                                            if (utilities.hasProperty(mov, 'isDate')) {
                                                                return <td key={mov.key} className="rt-td">{dateService.format(mov.value)}</td>;
                                                            }

                                                            return <td key={mov.key} className="rt-td">{mov.value}</td>;
                                                        }

                                                        return <td key={mov.key} className="rt-td">{constants.DashedGridCell}</td>;
                                                    })
                                                }
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </section>
                        </>
                    }
                    <div className="ep-pane">
                        <h2 className="ep-control-group-title text-uppercase m-t-3">{resourceProvider.read('owners')}</h2>
                        <div className="ep-actionbar">
                            {!isReadMode && !isDeleteMode && <button id="btn_movementOwnership_AddOwner" className="ep-btn m-l-2" type="button"
                                onClick={() => this.props.dispatchAction(ownersService.getOwnersData(this.props, 'movementOwnershipDetails'))}>{resourceProvider.read('addOwner')}</button>}
                        </div>
                    </div>
                    <div className="m-b-6">
                        <OwnershipMovementInventoryGrid {...this.props} />
                    </div>
                    {isShowReason &&
                        <div className="row">
                            <div className="col-md-4">
                                <div className="ep-control-group">
                                    <label id="lbl_movementOwnership_reasonForChange" className="ep-label" htmlFor="dd_movementOwnership_reasonForChange_sel">
                                        {resourceProvider.read('reasonForChange')}:</label>
                                    <Field id="dd_movementOwnership_reasonForChange" component={inputSelect} name="reasonForChange" isDisabled={isReadMode}
                                        options={reasonforChange} getOptionLabel={x => x.name} getOptionValue={x => x.elementId} inputId="dd_movementOwnership_reasonForChange_sel"
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                                </div>
                            </div>
                        </div>
                    }
                    {isShowComment &&
                        <div className="ep-control-group">
                            <label className="ep-label fs-16 text-uppercase" htmlFor="txt_movementOwnership_comment">{resourceProvider.read('comments')}</label>
                            <Field type="text" id="txt_movementOwnership_comment" component={inputTextarea}
                                name="comment" readOnly={isReadMode}
                                validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                    length({ max: 150, msg: resourceProvider.read('shortNameLengthValidation') })]} />
                        </div>
                    }
                </section>
                {isReadMode &&
                    <ModalFooter config={footerConfigService.getCancelConfig('movementOwnershipDetails',
                        { cancelText: 'accept', cancelClassName: 'ep-btn ep-btn--sm' })} />
                }
                {!isReadMode &&
                    <ModalFooter config={footerConfigService.getCommonConfig('movementOwnershipDetails', {})} />
                }
            </form>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.movementOwnersDataToggler !== this.props.movementOwnersDataToggler || prevProps.inventoryOwnersDataToggler !== this.props.inventoryOwnersDataToggler) {
            this.props.dispatchAction(ownersService.addOwnersData(this.props, 'movementOwnershipDetails'));
        }
    }

    componentDidMount() {
        this.props.setContractData(this.props.movementInventoryOwnershipData[0]);
    }

    componentWillUnmount() {
        this.props.clearSelectedContract();
    }
}

const mapStateToProps = state => {
    return {
        groupedCategoryElements: state.shared.groupedCategoryElements,
        movementInventoryOwnershipData: state.nodeOwnership.ownershipNode.movementInventoryOwnershipData,
        movementOwners: state.nodeOwnership.ownershipNode.movementOwners,
        inventoryOwners: state.nodeOwnership.ownershipNode.inventoryOwners,
        contractData: state.nodeOwnership.ownershipNode.contractData,
        dropdown: state.nodeOwnership.ownershipNode.dropdown,
        movementOwnersDataToggler: state.nodeOwnership.ownershipNode.movementOwnersDataToggler,
        inventoryOwnersDataToggler: state.nodeOwnership.ownershipNode.inventoryOwnersDataToggler,
        initialValues: state.nodeOwnership.ownershipNode.initialValues,
        currentUser: state.root.context.userId,
        startEditToggler: state.nodeOwnership.ownershipNode.startEditToggler,
        nodeDetails: state.nodeOwnership.ownershipNode.nodeDetails,
        editorInfo: state.nodeOwnership.ownershipNode.editorInfo,
        displayDataToggler: state.nodeOwnership.ownershipNode.displayData,
        selectedContract: state.nodeOwnership.ownershipNode.selectedContract
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
        setContractData: data => {
            dispatch(setContractData(data));
        },
        startEdit: startEditToggler => {
            dispatch(startEdit(startEditToggler));
        },
        displayData: contract => {
            dispatch(displayDataDropdown(contract.id));
        },
        clearSelectedContract: () => {
            dispatch(clearSelectedContract());
        },
        dispatchAction: action => {
            dispatch(action);
        }
    };
};

const MovementOwnershipDetailsForm = reduxForm({ form: 'movementOwnershipDetails', enableReinitialize: true })(MovementOwnershipDetails);
export default connect(mapStateToProps, mapDispatchToProps)(MovementOwnershipDetailsForm);
