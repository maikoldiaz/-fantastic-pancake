import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, change, SubmissionError } from 'redux-form';
import { required, length } from 'redux-form-validators';
import { inputTextbox, inputAutocomplete } from './../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import { requestSearchData, clearSearchData } from './../actions';
import { showError } from './../../../../common/actions';
import { utilities } from './../../../../common/services/utilities';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { addUpdateGridData } from '../../../../common/components/grid/actions';

class AddHomologationData extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
        this.searchData = this.searchData.bind(this);
        this.selectData = this.selectData.bind(this);
        this.getData = this.getData.bind(this);
    }

    generateTempId() {
        return new Date().valueOf();
    }

    getSourceDestinationValKey() {
        const categoryId = this.props.homologationGroup.group && this.props.homologationGroup.group.categoryId;
        switch (categoryId) {
        case constants.Category.Nodes:
            return 'nodeId';
        case constants.Category.Products:
            return 'productId';
        case constants.Category.StorageLocations:
            return 'storageLocationId';
        default:
            return 'elementId';
        }
    }

    checkDuplicateSourceDestinationValue(existingDataMappings, newDataMapping) {
        const error = existingDataMappings.filter(v => v.sourceValue.toString() === newDataMapping.sourceValue.toString() && v.tempId !== newDataMapping.tempId).length > 0;
        if (error === true) {
            throw new SubmissionError({
                sourceValue: resourceProvider.read('duplicationOriginValue')
            });
        }
    }

    onSubmit(val) {
        let homologationGroupData = {};
        const isSourceSystemTrue = this.isSourceSystemTrue();

        if (this.props.mode === constants.Modes.Create) {
            const tempId = this.generateTempId();
            homologationGroupData = {
                sourceValue: isSourceSystemTrue ? val.sourceValue[this.getSourceDestinationValKey()] : val.sourceValue,
                destinationValue: isSourceSystemTrue ? val.destinationValue : val.destinationValue[this.getSourceDestinationValKey()],
                value: isSourceSystemTrue ? val.sourceValue.name : val.destinationValue.name,
                tempId
            };
        } else {
            homologationGroupData = {
                sourceValue: isSourceSystemTrue && val.sourceValue[this.getSourceDestinationValKey()] ? val.sourceValue[this.getSourceDestinationValKey()] : val.sourceValue,
                destinationValue: !isSourceSystemTrue && val.destinationValue[this.getSourceDestinationValKey()] ? val.destinationValue[this.getSourceDestinationValKey()] : val.destinationValue,
                value: isSourceSystemTrue ? val.sourceValue.name || val.value : val.destinationValue.name || val.value,
                tempId: val.tempId,
                homologationDataMappingId: val.homologationDataMappingId
            };
        }

        if (isSourceSystemTrue && !homologationGroupData.sourceValue) {
            throw new SubmissionError({
                sourceValue: resourceProvider.read('invalidOriginValue')
            });
        }

        if (!isSourceSystemTrue && !homologationGroupData.destinationValue) {
            throw new SubmissionError({
                destinationValue: resourceProvider.read('invalidDestinationValue')
            });
        }

        this.checkDuplicateSourceDestinationValue(this.props.homologationGroupDataMappings, homologationGroupData);

        this.props.createUpdateHomologationDataMappings(homologationGroupData);
        this.props.closeModal();
    }

    searchData(searchText) {
        const categoryId = this.props.homologationGroup.group && this.props.homologationGroup.group.categoryId;
        let pathType = '';
        let searchCategory = null;
        switch (categoryId) {
        case constants.Category.Nodes:
            pathType = 'nodes';
            break;
        case constants.Category.Products:
            pathType = 'products';
            break;
        case constants.Category.StorageLocations:
            pathType = 'storagelocations';
            break;
        default:
            pathType = 'categoryelements';
            searchCategory = categoryId;
            break;
        }

        if (utilities.isNullOrWhitespace(searchText)) {
            this.props.clearData();
        } else {
            this.props.searchData(searchText, pathType, searchCategory);
        }
    }

    selectData(data) {
        const el = this.isSourceSystemTrue() ? 'sourceValue' : 'destinationValue';
        this.props.selectData(el, data);
    }

    renderItem(item, highlight) {
        return (
            <div style={{ padding: '10px 12px', background: highlight ? '#eee' : '#fff' }}>
                {item.name}
            </div>
        );
    }

    isSourceSystemTrue() {
        const homologationGroup = this.props.homologationGroup;
        return homologationGroup.sourceSystem.systemTypeId === constants.SystemType.TRUE;
    }

    getData() {
        return this.props.searchedData;
    }

    componentDidMount() {
        this.props.clearData();
    }

    render() {
        const homologationGroup = this.props.homologationGroup;
        return (
            <form id={`frm_${this.props.mode}_homologationData`} className="ep-form" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                <section className="ep-modal__content">
                    <div className="row">
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label className="ep-label" htmlFor="txt_sourceValue">{resourceProvider.read('originData')}</label>
                                {homologationGroup.sourceSystem.systemTypeId !== constants.SystemType.TRUE &&
                                    <Field id="txt_sourceValue" component={inputTextbox} name="sourceValue"
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                            length({ max: 100, msg: resourceProvider.read('sourceDestinationLengthValidation') })]} />
                                }

                                {homologationGroup.sourceSystem.systemTypeId === constants.SystemType.TRUE &&
                                    <Field type="text" id={`dd_${this.props.name}_sourceValue`} component={inputAutocomplete} name="sourceValue"
                                        onSelect={this.selectData} shouldChangeValueOnSelect={true} onChange={this.searchData}
                                        shouldItemRender={(item, value) => item.name.toLowerCase().indexOf(value.toLowerCase()) > -1}
                                        renderItem={this.renderItem} items={this.getData()} getItemValue={n => n.name}
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} defaultValue={this.props.initialValues.value} />
                                }
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="ep-control-group">
                                <label className="ep-label" htmlFor="txt_destinationValue">{resourceProvider.read('destinationData')}</label>
                                {homologationGroup.destinationSystem.systemTypeId !== constants.SystemType.TRUE &&
                                    <Field id="txt_destinationValue" component={inputTextbox} name="destinationValue"
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                            length({ max: 100, msg: resourceProvider.read('sourceDestinationLengthValidation') })]} />
                                }
                                {homologationGroup.destinationSystem.systemTypeId === constants.SystemType.TRUE &&
                                    <Field type="text" id={`dd_${this.props.name}_destinationValue`} component={inputAutocomplete} name="destinationValue"
                                        onSelect={this.selectData} shouldChangeValueOnSelect={true} onChange={this.searchData}
                                        shouldItemRender={(item, value) => item.name.toLowerCase().indexOf(value.toLowerCase()) > -1}
                                        renderItem={this.renderItem} items={this.getData()} getItemValue={n => n.name}
                                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} defaultValue={this.props.initialValues.value} />
                                }
                            </div>
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('addHomologationData', { acceptText: 'add' })} />
            </form>
        );
    }
}

const mapStateToProps = (state, ownProps) => {
    return {
        initialValues: ownProps.mode === constants.Modes.Create ? {} : state.homologations.dataMapping,
        homologationGroup: state.homologations.homologationGroup,
        searchedData: state.homologations.dataMappings,
        homologationGroupDataMappings: state.grid.homologationGroupData ? state.grid.homologationGroupData.items : []
    };
};

const mapDispatchToProps = dispatch => {
    return {
        searchData: (searchText, groupType, categoryId) => {
            dispatch(requestSearchData(searchText, groupType, categoryId));
        },
        selectData: (el, data) => {
            dispatch(change('addHomologationData', el, data));
        },
        clearData: () => {
            dispatch(clearSearchData());
        },
        createUpdateHomologationDataMappings: data => {
            dispatch(addUpdateGridData('homologationGroupData', 'tempId', data));
        },
        showErrorNotification: errorMsg => {
            dispatch(showError(errorMsg, true));
        }
    };
};

const AddHomologationDataForm = reduxForm({
    form: 'addHomologationData',
    enableReinitialize: true
})(AddHomologationData);

export default connect(mapStateToProps, mapDispatchToProps)(AddHomologationDataForm);
