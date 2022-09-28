import React from 'react';
import { connect } from 'react-redux';
import { reduxForm, FormSection } from 'redux-form';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import TransformationSection from './transformationSection.jsx';
import { constants } from './../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';
import { createTransformation, updateTransformation, getSourceNodes, getTransformationInfo, resetTransformationPopup } from '../actions';
import { asyncValidate } from './../asyncValidate';
import { closeModal, hideNotification, getCategoryElements } from '../../../../common/actions';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { refreshGrid } from '../../../../common/components/grid/actions';

export class Transformation extends React.Component {
    constructor() {
        super();
        this.saveTransformation = this.saveTransformation.bind(this);
    }

    saveTransformation(values) {
        const transformation = {
            transformationId: this.props.transformation ? this.props.transformation.transformationId : 0,
            messageTypeId: this.props.currentTab === 'movements' ? 'Movement' : 'Inventory',
            origin: {
                sourceNodeId: values.originSourceNode.nodeId,
                destinationNodeId: values.origin.destinationNode && values.origin.destinationNode.nodeId,
                sourceProductId: values.origin.sourceProduct.productId,
                destinationProductId: values.origin.destinationProduct && values.origin.destinationProduct.productId,
                measurementUnitId: values.origin.measurementUnit.elementId
            },
            destination: {
                sourceNodeId: values.destinationSourceNode.nodeId,
                destinationNodeId: values.destination.destinationNode && values.destination.destinationNode.nodeId,
                sourceProductId: values.destination.sourceProduct.productId,
                destinationProductId: values.destination.destinationProduct && values.destination.destinationProduct.productId,
                measurementUnitId: values.destination.measurementUnit.elementId
            },
            rowVersion: this.props.mode === constants.Modes.Update ? this.props.transformation.rowVersion : null
        };
        this.props.saveTransformation(transformation);
    }

    render() {
        return (
            <form id={`frm_transformation_${this.props.currentTab}_${this.props.mode}`}
                className="ep-form" onSubmit={this.props.handleSubmit(this.saveTransformation)}>
                <section className="ep-modal__content">
                    <div className="row">
                        <div className="col-md-6">
                            <h3 className="fs-16 m-b-3">{resourceProvider.read('origin')}</h3>
                            {this.props.ready &&
                                <FormSection name="origin">
                                    <TransformationSection name="origin" mode={this.props.mode} />
                                </FormSection>}
                        </div>
                        <div className="col-md-6">
                            <h3 className="fs-16 m-b-3">{resourceProvider.read('destination')}</h3>
                            {this.props.ready &&
                                <FormSection name="destination">
                                    <TransformationSection name="destination" mode={this.props.mode} />
                                </FormSection>}
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('transformation')} />
            </form>
        );
    }

    componentDidMount() {
        if (this.props.units && this.props.units.length > 0) {
            this.props.getSourceNodes(this.props.units, this.props.mode);
        } else {
            this.props.getCategoryElements();
        }
    }

    componentDidUpdate(prevProps) {
        if (prevProps.refreshToggler !== this.props.refreshToggler) {
            this.props.refreshGrid(this.props.currentTab === 'movements' ? 'movements' : 'inventories');
            this.props.closeModal();
        }

        if (this.props.mode === constants.Modes.Update && this.props.nodesReadyToggler !== prevProps.nodesReadyToggler) {
            this.props.getTransformationInfo(this.props.transformation);
        }

        if (this.props.categoryElementsDataToggler !== prevProps.categoryElementsDataToggler) {
            this.props.getSourceNodes(this.props.units, this.props.mode);
        }
    }

    componentWillUnmount() {
        this.props.resetTransformationPopup();
        this.props.hideNotification();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        currentTab: state.tabs.transformSettingsPanel ? state.tabs.transformSettingsPanel.activeTab : null,
        ready: state.transformSettings.ready,
        transformation: state.transformSettings.transformation,
        initialValues: state.transformSettings.initialValues,
        refreshToggler: state.transformSettings.refreshToggler,
        nodesReadyToggler: state.transformSettings.nodesReadyToggler,
        units: state.shared.groupedCategoryElements[constants.Category.UnitMeasurement],
        categoryElementsDataToggler: state.shared.categoryElementsDataToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        saveTransformation: transformation => {
            dispatch(ownProps.mode === constants.Modes.Create ? createTransformation(transformation) : updateTransformation(transformation));
        },
        getSourceNodes: (units, mode) => {
            dispatch(getSourceNodes(units, mode));
        },
        getCategoryElements: () => {
            dispatch(getCategoryElements());
        },
        getTransformationInfo: transformation => {
            dispatch(getTransformationInfo(transformation));
        },
        resetTransformationPopup: () => {
            dispatch(resetTransformationPopup());
        },
        refreshGrid: name => {
            dispatch(refreshGrid(name));
        },
        closeModal: () => {
            dispatch(closeModal('transformation'));
        },
        hideNotification: () => {
            dispatch(hideNotification());
        }
    };
};

/* istanbul ignore next */
const transformationForm = reduxForm({
    form: 'transformation',
    asyncValidate,
    enableReinitialize: true,
    shouldAsyncValidate: utilities.shouldAsyncValidate
})(Transformation);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(transformationForm);
