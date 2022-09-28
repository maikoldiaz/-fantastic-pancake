import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import { required, length, format } from 'redux-form-validators';
import { inputTextbox, inputToggler, inputTextarea, inputSelect } from '../../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { constants } from '../../../../../common/services/constants';
import { getStorageLocations } from '../../../../../common/actions';
import { utilities } from '../../../../../common/services/utilities';
import { updateNodeStorageLocations } from '../actions';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';
import { addUpdateGridData } from '../../../../../common/components/grid/actions';

class CreateStorageLocation extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
        this.checkIsNodeStorageLocationsExists = this.checkIsNodeStorageLocationsExists.bind(this);
    }
    onSubmit(formValues) {
        const values = Object.assign({}, formValues, {
            storageLocationTypeId: formValues.storageLocationType.elementId,
            storageLocationId: formValues.storageLocation ? formValues.storageLocation.storageLocationId : null,
            nodeStorageLocationId: formValues.nodeStorageLocationId ? formValues.nodeStorageLocationId : `9999${utilities.getRandomNumber()}`,
            sendToSap: this.props.node.sendToSap
        });
        if (this.props.mode === constants.Modes.Create) {
            values.products = [];
        }
        if (this.props.nodeActionMode === constants.Modes.Update) {
            values.nodeId = this.props.node.nodeId;
        }

        // empty products if storage location is changed
        const prevStorageLocation = this.props.nodeStorageLocations.find(x => x.nodeStorageLocationId === values.nodeStorageLocationId);
        if (prevStorageLocation) {
            const existingToCompare = utilities.isNullOrUndefined(prevStorageLocation.storageLocation) ? {} : prevStorageLocation.storageLocation;
            const newToCompare = utilities.isNullOrUndefined(values.storageLocation) ? {} : values.storageLocation;
            if (existingToCompare.storageLocationId !== newToCompare.storageLocationId) {
                values.products = [];
            }
        }
        this.props.createUpdateNodeStorageLocation(values, this.props.nodeStorageLocations);
        this.props.closeModal();
    }

    checkIsNodeStorageLocationsExists(name) {
        if (this.props.initialValues.name === name) {
            return null;
        }
        return this.props.nodeStorageLocations.find(item => item.name === name) ? `${resourceProvider.read('nodeStorageLocationAlreadyExists')}` : null;
    }

    render() {
        const nodeType = this.props.groupedCategoryElements[constants.Category.NodeType];
        const node = this.props.node;
        const isActiveReadonly = this.props.nodeActionMode === constants.Modes.Create;
        const sapStorageLocations = this.props.storageLocations.filter(x => !utilities.isNullOrUndefined(node.logisticCenter) &&
            x.logisticCenterId === node.logisticCenter.logisticCenterId);
        return (
            <form id={`frm_${this.props.mode}_nodeStorageLocation`} className="ep-form" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                <section className="ep-modal__content">
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label id="lbl_nodeStorageLocation_name" className="ep-label" htmlFor="txt_nodeStorageLocation_name">{resourceProvider.read('name')}</label>
                                <Field type="text" id="txt_nodeStorageLocation_name" component={inputTextbox} placeholder={resourceProvider.read('name')} name="name"
                                    validate={[this.checkIsNodeStorageLocationsExists, required({ msg: { presence: resourceProvider.read('required') } }),
                                        length({ max: 150, msg: resourceProvider.read('shortNameLengthValidation') }),
                                        format({
                                            with: constants.FieldValidation.StorageLocation,
                                            message: resourceProvider.read('shortNameFormatValidation')
                                        })]} />
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label id="lbl_nodeStorageLocation_storageLocationType" className="ep-label" htmlFor="dd_nodeStorageLocation_storageLocationType_sel">
                                    {resourceProvider.read('storageLocationType')}</label>
                                <Field component={inputSelect} options={nodeType} id="dd_nodeStorageLocation_storageLocationType"
                                    getOptionLabel={x => x.name} getOptionValue={x => x.elementId} name="storageLocationType"
                                    placeholder={resourceProvider.read('storageLocationType')} inputId="dd_nodeStorageLocation_storageLocationType_sel"
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label id="lbl_nodeStorageLocation_storageLocations" className="ep-label" htmlFor="dd_nodeStorageLocation_storageLocations_sel">
                                    {resourceProvider.read('storageLocations')}</label>
                                <Field component={inputSelect} options={sapStorageLocations}
                                    id="dd_nodeStorageLocation_storageLocations" inputId="dd_nodeStorageLocation_storageLocations_sel"
                                    getOptionLabel={x => node.sendToSap === true ? `${x.storageLocationId} - ${x.name}` : resourceProvider.read('select')}
                                    getOptionValue={x => node.sendToSap === true ? x.storageLocationId : null} name="storageLocation"
                                    placeholder={resourceProvider.read('storageLocations')} isDisabled={!node.sendToSap}
                                    validate={node.sendToSap ? [required({ msg: { presence: resourceProvider.read('required') } })] : null} />
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label id="lbl_nodeStorageLocation_active" className="ep-label">{resourceProvider.read('active')}</label>
                                <Field component={inputToggler} name="isActive" id="tog_nodeStorageLocation_active" readOnly={isActiveReadonly} />
                            </div>
                        </div>
                    </div>
                    <div className="ep-control-group">
                        <label id="lbl_nodeStorageLocation_description" className="ep-label" htmlFor="txtarea_nodeStorageLocation_description">
                            {resourceProvider.read('description')}</label>
                        <Field component={inputTextarea} name="description" id="txtarea_nodeStorageLocation_description"
                            placeholder={resourceProvider.read('description')}
                            validate={[length({ max: 1000, msg: resourceProvider.read('shortDescriptionLengthValidation') })]} />
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('nodeStorageLocation')} />
            </form>
        );
    }

    componentDidMount() {
        this.props.getStorageLocations();
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        initialValues: ownProps.mode === constants.Modes.Create ? { isActive: true } : state.node.manageNode.nodeStorageLocation,
        groupedCategoryElements: state.shared.groupedCategoryElements,
        storageLocations: state.shared.storageLocations,
        nodeStorageLocations: state.grid.nodeStorageLocation.items,
        node: state.node.manageNode.node,
        nodeActionMode: state.node.manageNode.mode
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        createUpdateNodeStorageLocation: (nodeStorageLocation, nodeStorageLocations) => {
            dispatch(addUpdateGridData('nodeStorageLocation', 'nodeStorageLocationId', nodeStorageLocation));
            dispatch(updateNodeStorageLocations(nodeStorageLocation, nodeStorageLocations));
        },
        getStorageLocations: () => {
            dispatch(getStorageLocations());
        }
    };
};

const createStorageLocationForm = reduxForm({
    form: 'createStorageLocation',
    enableReinitialize: true
})(CreateStorageLocation);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(createStorageLocationForm);
