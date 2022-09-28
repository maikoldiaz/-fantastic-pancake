import React from 'react';
import { connect } from 'react-redux';
import { Field, FieldArray, formValueSelector, reduxForm, change, untouch } from 'redux-form';
import { resourceProvider } from './../../../../../common/services/resourceProvider';
import { Wrapper } from '../../../../../common/components/wrapper/wrapper.jsx';
import { inputSelect } from '../../../../../common/components/formControl/formControl.jsx';
import { dataService } from './../../dataservice.js';
import { navigationService } from '../../../../../common/services/navigationService.js';
import { requestLogisticCenters, openMessageModal, closeModal, openModal, changeTab } from '../../../../../common/actions.js';
import { getProducts, getStorageLocationsByLogisticCenter, clearStorageListByPosition, clearAllStorageList, createAssociations } from './../../actions.js';
import classNames from 'classnames/bind';

export class CreateAssociation extends React.Component {
    constructor() {
        super();
        this.associationComponent = this.associationComponent.bind(this);
        this.renderAssociationComponent = this.renderAssociationComponent.bind(this);
        this.handleCancelButton = this.handleCancelButton.bind(this);
        this.handleLogisticCenterChange = this.handleLogisticCenterChange.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
    }

    handleCancelButton() {
        const { getFieldNameFromForm } = this.props;
        let associations = getFieldNameFromForm('associations');

        const navigateToManage = () => navigationService.navigateTo('manage');

        associations = associations ? associations : [];
        const associationsHasValidData = associations.length >= 1
            && associations.filter(association => association && (
                association.logisticsCenter ||
                association.storageLocation ||
                association.product)
            ).length > 0;


        if (associationsHasValidData) {
            this.props.openModal(
                {
                    title: 'cancelProductSettings',
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

    handleLogisticCenterChange(logisticCenter, index) {
        if (logisticCenter) {
            const { logisticCenterId = -1 } = logisticCenter;
            this.props.getStorageByLogisticCenter(logisticCenterId, index);
        } else {
            this.props.clearStorageListByPosition(index);
        }
        this.props.resetField(`associations[${index}].storageLocation`);
    }

    onSubmit(values) {
        const { associations } = values;
        const result = associations.map(association => ({
            storageLocationId: association.storageLocation.storageLocationId,
            productId: association.product.productId,
            isActive: true
        }));
        this.props.createAssociations(result);
    }

    associationComponent({
        addItem,
        removeItem,
        isLastOne = false,
        index = -1,
        inputName = ''
    } = {}) {
        const { newAssociation = {} } = this.props;

        let logisticsCenters = this.props.logisticsCenter || [];
        let storages = newAssociation[index] ? newAssociation[index].storageLocation || [] : [];
        let products = this.props.products || [];

        logisticsCenters = logisticsCenters.filter(center => center.isActive);
        storages = storages.filter(storage => storage.isActive);
        products = products.filter(product => product.isActive);

        return (<Wrapper
            isLastOne={isLastOne}
            key={index}
            onAddItem={addItem}
            onRemoveItem={removeItem ? () => removeItem(index) : undefined}>

            <div className="col-md-4">
                <div className="ep-control-group">
                    <label className="ep-label">{resourceProvider.read('logisticsCenter')}</label>
                    <Field component={inputSelect} name={inputName === '' ? '' : `${inputName}.logisticsCenter`}
                        options={logisticsCenters} getOptionLabel={x => x.name} getOptionValue={x => x.logisticCenterId}
                        onChange={c => this.handleLogisticCenterChange(c, index)} />
                </div>
            </div>
            <div className="col-md-4">
                <div className="ep-control-group">
                    <label className="ep-label">{resourceProvider.read('storageLocation')}</label>
                    <Field component={inputSelect} name={inputName === '' ? '' : `${inputName}.storageLocation`}
                        options={storages} getOptionLabel={x => x.name} getOptionValue={x => x.storageLocationId} />
                </div>
            </div>
            <div className="col-md-4">
                <div className="ep-control-group">
                    <label className="ep-label">{resourceProvider.read('productSapName')}</label>
                    <Field component={inputSelect} name={inputName === '' ? '' : `${inputName}.product`}
                        options={products} getOptionLabel={x => x.name} getOptionValue={x => x.productId} />
                </div>
            </div>
        </Wrapper>);
    }

    renderAssociationComponent({ fields }) {
        const { maxAssociationCreationEdition } = this.props;
        const canAdd = fields.length < maxAssociationCreationEdition;
        const addItem = canAdd ? fields.push : undefined;

        return (<>
            {fields.map((indexName, index) => this.associationComponent({
                inputName: indexName,
                index,
                removeItem: fields.length > 1 ? fields.remove : undefined
            }))}
            {canAdd && this.associationComponent({ isLastOne: true, addItem })}
        </>);
    }

    render() {
        const { maxAssociationCreationEdition } = this.props;
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <div className="ep-section ep-section--panel">
                        <div className="ep-section__body ep-section__body--h">
                            <form id={`frm_create_associations`} className="ep-form" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                                <div className="row">
                                    <div className="col-md-12">
                                        <div className="d-flex">
                                            <h3 className="fs-16 fw-bold fas--primary m-b-2">{resourceProvider.read('newAssociation')}</h3>
                                        </div>
                                    </div>
                                    <div className="col-md-12">
                                        <FieldArray name="associations" component={this.renderAssociationComponent} />
                                    </div>
                                    <div className="col-md-12">
                                        {maxAssociationCreationEdition > 0 &&
                                            <div className="text-right m-t-4 m-b-4">
                                                <button type="button" className="ep-btn ep-btn--link m-l-4 fs-14" id="btn_associations_cancel" onClick={() => this.handleCancelButton()}>
                                                    {resourceProvider.read('cancel')}
                                                </button>
                                                <button type="submit" className="ep-btn ep-btn--sm">
                                                    {resourceProvider.read('submit')}
                                                </button>
                                            </div>
                                        }
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </section>
        );
    }

    componentDidMount() {
        this.props.getLogisticsCenter();
        this.props.getProducts();
        this.props.changeTabPanel('productConfigurationPanel', 'storageLocationProducts');
    }

    componentDidUpdate(prevProps) {
        if (prevProps.createToggler !== this.props.createToggler) {
            this.props.openModalAssosiationsSaved();
        }
    }

    componentWillUnmount() {
        this.props.clearAllStorageList();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    const selector = formValueSelector('association');
    return {
        initialValues: {
            associations: [undefined]
        },
        logisticsCenter: state.shared.logisticCenters,
        products: state.products.products,
        newAssociation: state.products.newAssociation,
        associationsCreated: state.products.associationsCreated,
        createToggler: state.products.createToggler,
        getFieldNameFromForm: fieldName => selector(state, fieldName),
        maxAssociationCreationEdition: state.root.systemConfig.maxProductStorageLocationMappingCreationEdition
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        getLogisticsCenter: () => {
            dispatch(requestLogisticCenters());
        },
        getStorageByLogisticCenter: (logisticCenterId, index) => {
            dispatch(getStorageLocationsByLogisticCenter(logisticCenterId, index));
        },
        getProducts: () => {
            dispatch(getProducts());
        },
        openModal: (opts, message) => {
            dispatch(openMessageModal(message, opts));
        },
        closeModal: () => {
            dispatch(closeModal());
        },
        resetField: fieldName => {
            dispatch(change('association', fieldName, null));
            dispatch(untouch('association', fieldName));
        },
        clearStorageListByPosition: position => {
            dispatch(clearStorageListByPosition(position));
        },
        clearAllStorageList: () => {
            dispatch(clearAllStorageList());
        },
        createAssociations: json => {
            dispatch(createAssociations(json));
        },
        openModalAssosiationsSaved: () => {
            dispatch(openModal('associationSavedModal', '', '', '', '', classNames('ep-modal__body')));
        },
        changeTabPanel: (name, activeTab) => {
            dispatch(changeTab(name, activeTab));
        }
    };
};

const associationForm = reduxForm({
    form: 'association',
    validate: values => dataService
        .validationsForRequiredFieldArrayItems(
            values,
            {
                fieldName: 'associations',
                requiredItems: ['logisticsCenter', 'storageLocation', 'product']
            })
})(CreateAssociation);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(associationForm);
