import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm, FieldArray } from 'redux-form';
import { numericality } from 'redux-form-validators';
import { inputDecimal } from '../../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { wizardPrevStep } from '../../../../../common/actions';
import { utilities } from '../../../../../common/services/utilities';
import { updateOwners } from './../actions';
import { constants } from '../../../../../common/services/constants';
import { refreshGrid } from '../../../../../common/components/grid/actions';

const ownership = props => (
    <div className="ep-control">
        <div className="row">
            {props.fields.map((item, index) => (
                <div key={`col-${index}`} className="col-md-6">
                    <div key={item.id} className="ep-control-group">
                        <label id={`lbl_nodeProducts_owner_${props.fields.get(index).name}`} className="ep-label">{props.fields.get(index).name}</label>
                        <Field id={`txt_percentage_${item}.value`}
                            type={constants.InputType.Percentage}
                            min={constants.OwnershipMinPrecentage} max={constants.PercentageRange.Max} step={constants.PercentageRange.Step}
                            component={inputDecimal} name={`${item}.value`}
                            validate={[
                                numericality({
                                    greaterThanOrEqualTo: constants.OwnershipMinPrecentage,
                                    lessThanOrEqualTo: constants.PercentageRange.Max,
                                    msg: {
                                        greaterThanOrEqualTo: resourceProvider.read('invalidRange'),
                                        lessThanOrEqualTo: resourceProvider.read('percentageValidationMessage')
                                    }
                                })]}
                            hasAddOn={true} addOnClass="fas fa-percentage" />
                    </div>
                </div>
            ))}
        </div>
        {(props.meta.error &&
            <span id="nodeProducts_error" className="ep-control__error">
                <span className="ep-textbox__error-txt">{props.meta.error.owners}</span>
            </span>)}
    </div>
);
class Ownership extends React.Component {
    constructor() {
        super();
        this.updateOwnership = this.updateOwnership.bind(this);
        this.validateOwnership = this.validateOwnership.bind(this);
    }

    updateOwnership(values) {
        const owners = [];
        values.owners.forEach(v => {
            owners.push({
                ownerId: v.id,
                ownershipPercentage: Number(v.value)
            });
        });

        const data = {
            owners,
            rowVersion: this.props.rowVersion,
            productId: this.props.productId
        };

        this.props.updateOwners(data);
    }

    validateOwnership(owners) {
        if (utilities.isNullOrUndefined(owners)) {
            return owners;
        }

        let totalOwnership = 0;
        owners.forEach(v => {
            totalOwnership = totalOwnership + Number(v.value);
        });

        if (totalOwnership !== 100) {
            return {
                owners: this.props.dirty ? resourceProvider.read('ownershipValidation') : ''
            };
        }

        return undefined;
    }

    render() {
        return (
            <form id="frm_nodeProducts_ownership" className="ep-form" onSubmit={this.props.handleSubmit(this.updateOwnership)}>
                <section className="ep-modal__content">
                    <FieldArray name="owners" component={ownership} validate={this.validateOwnership} />
                </section>
                <footer className="ep-modal__footer">
                    <div className="ep-modal__footer-actions">
                        <button id="btn_nodeProducts_ownership_prev" className="ep-btn ep-btn--sm" onClick={this.props.onWizardPrev}>{resourceProvider.read('prev')}</button>
                        <span className="float-r">
                            <button type="button" className="ep-btn ep-btn--link" id="btn_nodeProducts_ownership_cancel" onClick={this.props.closeModal}>{resourceProvider.read('cancel')}</button>
                            <button type="submit" id="btn_nodeProducts_ownership_submit" className="ep-btn ep-btn--sm" disabled={!this.props.dirty}>{resourceProvider.read('submit')}</button>
                        </span>
                    </div>
                </footer>
            </form>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.ownersUpdateToggler !== this.props.ownersUpdateToggler) {
            this.props.refreshGrid();
            this.props.closeModal();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        initialValues: { owners: state.dualSelect.productOwners ? state.dualSelect.productOwners.target : [] },
        productId: state.node.attributes.nodeProduct.storageLocationProductId,
        rowVersion: state.node.attributes.nodeProduct.rowVersion,
        ownersUpdateToggler: state.node.attributes.ownersUpdateToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onWizardPrev: () => {
            dispatch(wizardPrevStep('editNodeOwnership'));
        },
        updateOwners: data => {
            dispatch(updateOwners(data));
        },
        refreshGrid: () => {
            dispatch(refreshGrid('nodeProducts'));
        }
    };
};

const EditPropertyPercentagesForm = reduxForm({ form: 'editNodeOwnership' })(Ownership);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(EditPropertyPercentagesForm);
