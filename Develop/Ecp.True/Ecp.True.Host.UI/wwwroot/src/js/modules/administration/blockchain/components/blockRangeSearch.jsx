import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, reset } from 'redux-form';
import { required } from 'redux-form-validators';
import { inputTextbox, inputSelect } from '../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { utilities } from '../../../../common/services/utilities';
import { saveBlockRange } from '../actions';
import { hideNotification } from '../../../../common/actions';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { optionService } from '../../../../common/services/optionService';
import { blockValidator } from '../blockValidationService';

export class BlockRangeSearch extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
    }

    async onSubmit(formValues) {
        const blockRange = { headBlock: formValues.headBlockNumber, tailBlock: formValues.tailBlockNumber, event: formValues.type.value };
        await blockValidator.validateBlockRange(blockRange);
        this.props.saveBlockRange(blockRange);
        this.props.onNext(this.props.config.wizardName);
    }

    render() {
        const eventTypes = [{ label: 'Todos', value: 0 }, ...optionService.getBlockchainTypes()];
        return (
            <div className="ep-section">
                <form className="full-height" id="frm_blockchain_search" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                    <div className="ep-section__body ep-section__body--f">
                        <section className="ep-section__content-w500 m-t-6">
                            <h2 className="ep-control-group-title">{resourceProvider.read('blockchainSearch')}</h2>
                            <div className="ep-control-group">
                                <label id="lbl_blockchainRangeSearch_headBlockNumber" className="ep-label" htmlFor="txt_blockchainRangeSearch_headBlockNumber">
                                    {resourceProvider.read('headBlockNumber')}</label>
                                <Field type="text" id="txt_blockchainRangeSearch_headBlockNumber" component={inputTextbox}
                                    placeholder={resourceProvider.read('headBlockNumber')} name="headBlockNumber"
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                            <div className="ep-control-group">
                                <label id="lbl_blockchainRangeSearch_tailBlockNumber" className="ep-label" htmlFor="txt_blockchainRangeSearch_tailBlockNumber">
                                    {resourceProvider.read('tailBlockNumber')}</label>
                                <Field type="text" id="txt_blockchainRangeSearch_tailBlockNumber" component={inputTextbox}
                                    placeholder={resourceProvider.read('tailBlockNumber')} name="tailBlockNumber"
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                            <div className="ep-control-group">
                                <label id="lbl_blockchainRangeSearch_type" className="ep-label" htmlFor="dd_blockchainRangeSearch_type">
                                    {resourceProvider.read('type')}</label>
                                <Field component={inputSelect} name={`type`} id={`dd_blockchainRangeSearch_type`}
                                    placeholder={resourceProvider.read('select')} options={eventTypes} getOptionLabel={x => x.label} getOptionValue={x => x.value}
                                    validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                            </div>
                        </section>
                    </div>
                    <SectionFooter floatRight={true} config={footerConfigService.getCommonConfig('blockchainRangeSearch', {
                        acceptText: 'query', acceptClassName: 'ep-btn', onCancel: this.props.resetForm
                    })} />
                </form>
            </div >
        );
    }

    componentWillUnmount() {
        this.props.resetForm();
    }

    componentDidMount() {
        this.props.resetForm();
    }
}

const mapStateToProps = () => {
    return {
        initialValues: { type: { label: 'Todos', value: 0 } }
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        saveBlockRange: blockRange => {
            dispatch(saveBlockRange(blockRange));
        },
        resetForm: () => {
            dispatch(reset('blockRangeSearch'));
            dispatch(hideNotification());
        }
    };
};

/* istanbul ignore next */
const BlockSearchForm = reduxForm({ form: 'blockRangeSearch', enableReinitialize: true })(BlockRangeSearch);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(BlockSearchForm);
