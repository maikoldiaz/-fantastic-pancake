import React from 'react';
import { reduxForm, Field } from 'redux-form';
import { required } from 'redux-form-validators';
import { inputSelect } from '../../../../common/components/formControl/formControl.jsx';
import { connect } from 'react-redux';
import {
    fileRegistrationAddFile, requestFileRegistrationUpload,
    requestFileRegistrationUploadAccessInfo, onFileRegistrationValidation,
    resetFileregistrationUploadPopup, setSelectedFileType,
    setFileUploadProgressStatus
} from '../actions';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import uuid from 'uuid';
import { utilities } from '../../../../common/services/utilities';
import { hideLoader, showLoader, getCategoryElements, getSystemTypes, openMessageModal } from '../../../../common/actions';
import { excelService } from '../../../../common/services/excelService';
import blobService from '../../../../common/services/blobService';
import { constants } from '../../../../common/services/constants';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { refreshGrid } from '../../../../common/components/grid/actions';

class UploadFile extends React.Component {
    constructor() {
        super();

        this.saveUploadFile = this.saveUploadFile.bind(this);
        this.onUploadError = this.onUploadError.bind(this);
        this.onUploadSuccess = this.onUploadSuccess.bind(this);
        this.onValidateSuccess = this.onValidateSuccess.bind(this);
        this.saveFileInformation = this.saveFileInformation.bind(this);
        this.onTypeChange = this.onTypeChange.bind(this);
    }

    onUploadError(error) {
        this.props.onFileRegistrationValidation({ status: false, message: error.message });
    }

    onUploadSuccess() {
        this.props.hideLoader();
        this.saveFileInformation(this.props.uploadAccessInfo);
    }

    onValidateSuccess(validationResult) {
        this.props.hideLoader();
        this.props.onFileRegistrationValidation(validationResult);
        if (validationResult.status) {
            this.props.requestFileRegistrationUploadAccess(uuid.v4().toString(), this.props.selectedFileType);
        } else {
            this.props.setFileUploadProgressStatus(false);
        }
    }

    saveUploadFile(formValues) {
        this.props.showLoader();
        this.props.setFileUploadProgressStatus(true);
        this.formValues = formValues;
        excelService.validateExcel(this.props.browseFile, this.props.systemType === constants.SystemType.EXCEL ? this.props.systemType : this.props.selectedFileType, this.onValidateSuccess);
    }

    saveFileInformation(uploadAccessInfo) {
        const fileRegistration = {
            uploadId: uploadAccessInfo.blobName,
            name: this.props.fileName,
            actionType: this.formValues.actions.value,
            segmentId: this.props.systemType === constants.SystemType.EXCEL ? this.formValues.segment.elementId : null,
            systemTypeId: this.props.systemType === constants.SystemType.EXCEL ? this.props.systemType : this.props.selectedFileType,
            blobPath: uploadAccessInfo.blobPath
        };

        if (!utilities.isNullOrUndefined(this.props.reInjectFileInfo)) {
            fileRegistration.previousUploadId = this.props.reInjectFileInfo.uploadId;
        }

        this.props.requestFileRegistrationUpload(fileRegistration);
    }

    isContractOrEvent() {
        return (this.props.systemType === constants.SystemType.CONTRACT || this.props.systemType === constants.SystemType.EVENTS);
    }

    onTypeChange(selectedType) {
        if (utilities.isNullOrUndefined(selectedType)) {
            return;
        }
        this.props.setSelectedFileType(selectedType.systemTypeId);
    }

    render() {
        const segments = this.props.categoryElements.filter(obj => obj.isActive && obj.categoryId === 2);
        let actionTypeOptions = Object.entries(this.props.actionTypes).map(x => ({ label: resourceProvider.read(x[1]), value: x[0] }));
        if (utilities.isNotEmpty(this.props.reInjectFileInfo)) {
            actionTypeOptions = actionTypeOptions.filter(x =>
                x.value === this.props.reinjectActionTypeKey
            );
        } else {
            actionTypeOptions = actionTypeOptions.filter(x =>
                x.value !== this.props.reinjectActionTypeKey
            );
        }
        return (
            <form className="ep-form" onSubmit={this.props.handleSubmit(this.saveUploadFile)}>
                <section className="ep-modal__content">
                    <h1 className="ep-modal__content-heading"> {this.isContractOrEvent() ? resourceProvider.read('eventAndContractUploadHeader') : resourceProvider.read('fileUploadHeader')}</h1>
                    {this.props.systemType === constants.SystemType.EXCEL &&
                        <div className="ep-control-group">
                            <label id="lbl_fileUpload_segment" className="ep-label" htmlFor="dd_fileUpload_segment_sel">{resourceProvider.read('segment')}</label>
                            <div className="ep-control">
                                <Field id="dd_fileUpload_segment" component={inputSelect} name="segment" inputId="dd_fileUpload_segment_sel"
                                    options={segments} getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                        </div>
                    }
                    {this.isContractOrEvent() &&
                        <div className="ep-control-group">
                            <label id="lbl_fileUpload_fileType" className="ep-label" htmlFor="dd_fileUpload_fileType_sel">{resourceProvider.read('fileType')}</label>
                            <div className="ep-control">
                                <Field id="dd_fileUpload_fileType" component={inputSelect} name="fileType" inputId="dd_fileUpload_fileType_sel"
                                    options={this.props.fileTypes} getOptionLabel={x => x.name} getOptionValue={x => x.systemTypeId}
                                    onChange={this.onTypeChange}
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                        </div>
                    }
                    <div className="ep-control-group">
                        <label className="ep-label" htmlFor="dd_fileUpload_actions_sel">{resourceProvider.read('action')}</label>
                        <div className="ep-control">
                            <Field id="dd_fileUpload_actions" component={inputSelect} name="actions" inputId="dd_fileUpload_actions_sel"
                                options={actionTypeOptions} getOptionLabel={x => x.label} getOptionValue={x => x.value}
                                validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                        </div>
                    </div>
                    <div className="ep-control-group m-b-0">
                        <div className="ep-control">
                            <section className="ep-fupload">
                                <div className="ep-fupload__control">
                                    <label className="ep-fupload__input">
                                        <input type="file" id="input_FileUpload" name="fileupload" onChange={e => {
                                            if (e.target.files.length > 0) {
                                                this.props.addBrowsedFile(e.target.files[0]);
                                            }
                                        }} />
                                        <i className="fas fa-file-upload m-r-2" />
                                        {resourceProvider.read('browse')}
                                    </label>
                                </div>
                                <ul className="ep-fupload__list">
                                    {this.props.fileName &&
                                        <li className="ep-fupload__file">
                                            <span className="ep-fupload__icn"><i className="fas fa-file-excel" /></span>
                                            <p className="ep-fupload__info">
                                                <span className="ep-fupload__info-title">
                                                    {utilities.isNullOrUndefined(this.props.fileName) ? '' : this.props.fileName}
                                                </span>
                                                <span className="ep-fupload__info-stitle">{utilities.formatBytes(this.props.browseFile.size, 0)}</span>
                                            </p>
                                        </li>
                                    }
                                </ul>
                            </section>
                            {!this.props.validationResult.status &&
                                <span className="ep-control__error">
                                    <span id="span_file_error_message" className="ep-textbox__error-txt">{this.props.validationResult.message}</span>
                                </span>
                            }
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('uploadFile', {
                    disableAccept: (this.props.fileUploadInProgress || utilities.isNullOrUndefined(this.props.fileName) || this.props.invalid),
                    acceptText: this.isContractOrEvent() ? 'load' : 'submit'
                })} />
            </form>
        );
    }

    componentDidMount() {
        this.props.getCategoryElements();
        this.props.getSystemTypes();
        this.props.setFileUploadProgressStatus(false);
    }

    componentWillUnmount() {
        this.props.resetFileregistrationUploadPopup();
        this.props.setFileUploadProgressStatus(false);
    }

    componentDidUpdate(prevProps) {
        if (this.props.receiveAccessInfoToggler !== prevProps.receiveAccessInfoToggler) {
            this.props.showLoader();
            blobService.initialize(this.props.uploadAccessInfo.accountName, this.onUploadError, this.onUploadSuccess);
            blobService.uploadToBlob(this.props.uploadAccessInfo.sasToken, this.props.uploadAccessInfo.containerName, this.props.uploadAccessInfo.blobPath, this.props.browseFile);
        }

        if (this.props.receiveStatusToggler !== prevProps.receiveStatusToggler) {
            this.props.closeModal();
            this.props.refreshGrid();
        }

        if ((prevProps.failureToggler !== this.props.failureToggler) && (!utilities.isNullOrUndefined(this.props.failureToggler))) {
            this.props.closeModal();
            this.props.showTechnicalError(resourceProvider.read('systemErrorMessage'), resourceProvider.read('error'), false);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        browseFile: state.messages.fileRegistration.browseFile,
        fileName: state.messages.fileRegistration.browseFile.name,
        actionTypes: state.shared.registrationActionTypes,
        categoryElements: state.shared.categoryElements,
        receiveStatusToggler: state.messages.fileRegistration.receiveStatusToggler,
        receiveAccessInfoToggler: state.messages.fileRegistration.receiveAccessInfoToggler,
        uploadAccessInfo: state.messages.fileRegistration.uploadAccessInfo,
        validationResult: state.messages.fileRegistration.validationResult,
        reInjectFileInfo: state.messages.fileRegistration.reInjectFileInfo,
        reinjectActionTypeKey: state.messages.fileRegistration.reinjectActionTypeKey,
        fileTypes: state.shared.fileTypes,
        selectedFileType: state.messages.fileRegistration.selectedFileType,
        fileUploadInProgress: state.messages.fileRegistration.fileUploadInProgress,
        failureToggler: state.messages.fileRegistration.failureToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        addBrowsedFile: file => {
            dispatch(fileRegistrationAddFile(file));
        },
        requestFileRegistrationUpload: file => {
            dispatch(requestFileRegistrationUpload(file));
        },
        refreshGrid: () => {
            dispatch(refreshGrid('fileUploads'));
        },
        requestFileRegistrationUploadAccess: (fileName, selectedFileType) => {
            dispatch(requestFileRegistrationUploadAccessInfo(fileName, selectedFileType));
        },
        onFileRegistrationValidation: validationResult => {
            dispatch(onFileRegistrationValidation(validationResult));
        },
        resetFileregistrationUploadPopup: () => {
            dispatch(resetFileregistrationUploadPopup());
        },
        showLoader: () => {
            dispatch(showLoader());
        },
        hideLoader: () => {
            dispatch(hideLoader());
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        getSystemTypes: () => {
            dispatch(getSystemTypes());
        },
        setSelectedFileType: selectedType => {
            dispatch(setSelectedFileType(selectedType));
        },
        setFileUploadProgressStatus: status => {
            dispatch(setFileUploadProgressStatus(status));
        },
        showTechnicalError: (message, title, canCancel) => {
            dispatch(openMessageModal(message, { title: title, canCancel: canCancel }));
        }
    };
};

const UploadFileForm = reduxForm({ form: 'uploadFile' })(UploadFile);
const mergeProps = (stateProps, dispatchProps, ownProps) => Object.assign({}, stateProps, dispatchProps, ownProps);

/* istanbul ignore next */
export const UploadFileComponent = connect(mapStateToProps, mapDispatchToProps, mergeProps)(UploadFileForm);

