import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, FormSection } from 'redux-form';
import { utilities } from '../../../../common/services/utilities';
import { inputSelect, inputToggler } from '../../../../common/components/formControl/formControl.jsx';
import AnnulationSection from './annulationSection.jsx';
import { closeModal } from '../../../../common/actions';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import { initTypes, saveAnnulation } from '../actions';
import { dataService } from '../dataService';
import { asyncValidate } from '../asyncValidate';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { refreshGrid } from '../../../../common/components/grid/actions';
import { required } from 'redux-form-validators';

export class Annulation extends React.Component {
    constructor() {
        super();
        this.saveAnnulation = this.saveAnnulation.bind(this);
    }

    saveAnnulation(values) {
        this.props.saveAnnulation(dataService.buildAnnulationObject(values, this.props.mode, this.props.initialValues), this.props.mode);
    }

    render() {
        let sapTransactionCodeOptions = this.props.groupedElements[constants.Category.SapTransactionCode] || [];
        sapTransactionCodeOptions = sapTransactionCodeOptions.filter(s => s.isActive);
        return (
            <form id={`frm_annulation_${this.props.mode}`} className="ep-form" onSubmit={this.props.handleSubmit(this.saveAnnulation)}>
                <section className="ep-modal__content">
                    <div className="row">
                        <div className="col-md-6">
                            <FormSection name={constants.Annulations.Sections.Source}>
                                <AnnulationSection name={constants.Annulations.Sections.Source} mode={this.props.mode} />
                            </FormSection>
                        </div>
                        <div className="col-md-6">
                            <FormSection name={constants.Annulations.Sections.Annulation}>
                                <AnnulationSection name={constants.Annulations.Sections.Annulation} mode={this.props.mode} />
                            </FormSection>
                        </div>
                        <div className="col-md-6">
                            <div className="ep-control-group  m-b-0">
                                <label className="ep-label">{resourceProvider.read('active')}</label>
                                <Field component={inputToggler} name="isActive" id="tog_category_active" />
                            </div>
                        </div>
                        <div className="col-md-6">
                            <div className="ep-control-group  m-b-0">
                                <label className="ep-label">{resourceProvider.read('sapTransactionCode')}</label>
                                <Field component={inputSelect} name="sapTransactionCode" id={`dd_sapTransactionCode_sel`} inputId={`dd_sapTransactionCode_sel`}
                                    placeholder={resourceProvider.read('select')}
                                    options={sapTransactionCodeOptions}
                                    getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('annulation')} />
            </form>
        );
    }

    componentDidMount() {
        if (this.props.mode === constants.Modes.Create) {
            this.props.initTypes();
        }
    }

    componentDidUpdate(prevProps) {
        if (prevProps.saveSuccess !== this.props.saveSuccess) {
            this.props.refreshGrid();
            this.props.closeModal();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        initialValues: state.annulations.initialValues,
        saveSuccess: state.annulations.saveSuccess,
        groupedElements: state.shared.groupedCategoryElements
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        initTypes: () => {
            dispatch(initTypes());
        },
        saveAnnulation: (values, mode) => {
            dispatch(saveAnnulation(values, mode));
        },
        closeModal: () => {
            dispatch(closeModal('annulation'));
        },
        refreshGrid: () => {
            dispatch(refreshGrid('annulations'));
        }
    };
};

/* istanbul ignore next */
const annulationForm = reduxForm({
    form: 'annulation',
    asyncValidate,
    enableReinitialize: true,
    shouldAsyncValidate: utilities.shouldAsyncValidate
})(Annulation);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(annulationForm);
