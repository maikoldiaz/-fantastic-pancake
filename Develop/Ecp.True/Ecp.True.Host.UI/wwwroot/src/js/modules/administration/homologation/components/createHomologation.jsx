import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import { required } from 'redux-form-validators';
import { inputSelect } from './../../../../common/components/formControl/formControl.jsx';
import { initHomologationGroup } from './../actions';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { getSystemTypes, getCategories, showLoader, hideLoader, showError, hideNotification } from './../../../../common/actions';
import { serverValidator } from './../../../../common/services/serverValidator';
import { navigationService } from './../../../../common/services/navigationService';
import { constants } from '../../../../common/services/constants';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

class CreateHomologation extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
    }

    async onSubmit(val) {
        if (val.sourceSystem.systemTypeId === val.destinationSystem.systemTypeId) {
            this.props.showError(resourceProvider.read('systemTypeCannotBeSame'), true, resourceProvider.read('error'));
            return;
        }

        if (val.sourceSystem.systemTypeId !== constants.SystemType.TRUE && val.destinationSystem.systemTypeId !== constants.SystemType.TRUE) {
            this.props.showError(resourceProvider.read('trueMustBeSystemType'), true, resourceProvider.read('error'));
            return;
        }

        const homologationGroup = {
            sourceSystemId: val.sourceSystem.systemTypeId,
            destinationSystemId: val.destinationSystem.systemTypeId,
            groupId: val.group.categoryId
        };

        this.props.showLoader();
        const data = await serverValidator.validateHomologationGroup(homologationGroup);

        if (data && data.errorCodes.length > 0) {
            this.props.hideLoader();
            this.props.showError(resourceProvider.read('homologationGroupAlreadyExists'), true, resourceProvider.read('error'));
            return;
        }
        this.props.initHomologationGroup(val);
        navigationService.navigateTo(`manage/create`);
        this.props.hideLoader();
        this.props.closeModal();
    }

    render() {
        return (
            <form id={`frm_${this.props.mode}_homologation`} className="ep-form" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                <section className="ep-modal__content">
                    <h2 className="ep-control-group-title">{resourceProvider.read('selectCriteria')}</h2>
                    <div className="ep-control-group">
                        <label className="ep-label" htmlFor="dd_originSystem_sel">{resourceProvider.read('sourceSystem')}</label>
                        <Field id="dd_originSystem" component={inputSelect} name="sourceSystem" inputId="dd_originSystem_sel" onChange={this.props.resetFields}
                            options={this.props.systemTypes} getOptionLabel={x => x.name} getOptionValue={x => x.systemTypeId}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                    </div>
                    <div className="ep-control-group">
                        <label className="ep-label" htmlFor="dd_destinationSystem_sel">{resourceProvider.read('destinationSystem')}</label>
                        <Field id="dd_destinationSystem" component={inputSelect} name="destinationSystem" inputId="dd_destinationSystem_sel" onChange={this.props.resetFields}
                            options={this.props.systemTypes} getOptionLabel={x => x.name} getOptionValue={x => x.systemTypeId}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                    </div>
                    <div className="ep-control-group">
                        <label className="ep-label" htmlFor="dd_group_sel">{resourceProvider.read('group')}</label>
                        <Field id="dd_group" component={inputSelect} name="group" inputId="dd_group_sel" onChange={this.props.resetFields}
                            options={this.props.homologationCategories} getOptionLabel={x => x.name} getOptionValue={x => x.categoryId}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('createHomologation', { acceptText: 'continue' })} />
            </form>
        );
    }

    componentDidMount() {
        this.props.getCategories();
        this.props.getSystemTypes();
    }

    componentWillUnmount() {
        this.props.hideError();
    }
}

const mapStateToProps = state => {
    return {
        systemTypes: state.shared.systemTypes,
        homologationCategories: state.shared.homologationCategories
    };
};

const mapDispatchToProps = dispatch => {
    return {
        initHomologationGroup: homologationGroup => {
            dispatch(initHomologationGroup(constants.Modes.Create, homologationGroup));
        },
        getCategories: () => {
            dispatch(getCategories());
        },
        getSystemTypes: () => {
            dispatch(getSystemTypes());
        },
        showError: (message, showOnModal, title) => {
            dispatch(showError(message, showOnModal, title));
        },
        showLoader: () => {
            dispatch(showLoader);
        },
        hideLoader: () => {
            dispatch(hideLoader);
        },
        hideError: () => {
            dispatch(hideNotification());
        }
    };
};

const CreateHomologationForm = reduxForm({
    form: 'createHomologation',
    enableReinitialize: true
})(CreateHomologation);

export default connect(mapStateToProps, mapDispatchToProps)(CreateHomologationForm);
