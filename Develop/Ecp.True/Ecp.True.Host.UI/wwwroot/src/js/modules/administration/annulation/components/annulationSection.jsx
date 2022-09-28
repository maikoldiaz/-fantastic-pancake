import React from 'react';
import { connect } from 'react-redux';
import { Field } from 'redux-form';
import { required } from 'redux-form-validators';
import { inputSelect } from '../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dataService } from '../dataService';
import { constants } from '../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';

export class AnnulationSection extends React.Component {
    render() {
        const labels = dataService.getLabels(this.props.name);
        return (
            <>
                <div className="ep-control-group">
                    <label id="lbl_annulation_movement" className="ep-label" htmlFor={`dd_${this.props.name}_movement_sel`}>{resourceProvider.read(labels[0])}</label>
                    <Field component={inputSelect} name={`movement`} id={`dd_${this.props.name}_movement`} inputId={`dd_${this.props.name}_movement_sel`}
                        placeholder={resourceProvider.read('select')} isDisabled={dataService.disableField(this.props.mode, this.props.name)}
                        options={this.props.movementTypes.filter(value => value.elementId !== this.props.movement)}
                        getOptionLabel={x => x.name} getOptionValue={x => x.elementId}
                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                </div>

                <div className="ep-control-group">
                    <label id="lbl_annulation_node" className="ep-label" htmlFor={`dd_${this.props.name}_node_sel`}>{resourceProvider.read(labels[1])}</label>
                    <Field component={inputSelect} name={`node`} id={`dd_${this.props.name}_node`} inputId={`dd_${this.props.name}_node_sel`}
                        placeholder={resourceProvider.read('select')}
                        options={this.props.originTypes.filter(value => value.originTypeId !== this.props.node)}
                        getOptionLabel={x => x.name} getOptionValue={x => x.originTypeId}
                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                </div>

                <div className="ep-control-group">
                    <label id="lbl_annulation_product" className="ep-label" htmlFor={`dd_${this.props.name}_product_sel`}>{resourceProvider.read(labels[2])}</label>
                    <Field component={inputSelect} name={`product`} id={`dd_${this.props.name}_product`} inputId={`dd_${this.props.name}_product_sel`}
                        placeholder={resourceProvider.read('select')}
                        options={this.props.originTypes.filter(value => value.originTypeId !== this.props.product)}
                        getOptionLabel={x => x.name} getOptionValue={x => x.originTypeId}
                        validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                </div>
            </>
        );
    }

    componentDidUpdate(prevProps) {
        if (this.props.fieldChange.fieldChangeToggler !== prevProps.fieldChange.fieldChangeToggler) {
            const actions = dataService.getAnnulationActions(this.props.fieldChange.currentModifiedField, this.props.fieldChange.currentModifiedValue);
            this.props.dispatchActions(actions);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        movementTypes: state.shared.groupedCategoryElements[constants.Category.MovementType],
        originTypes: state.shared.originTypes,
        fieldChange: state.annulations.fieldChange,
        movement: state.annulations[ownProps.name].movement,
        node: state.annulations[ownProps.name].node,
        product: state.annulations[ownProps.name].product
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        dispatchActions: actions => {
            actions.forEach(dispatch);
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(AnnulationSection);
