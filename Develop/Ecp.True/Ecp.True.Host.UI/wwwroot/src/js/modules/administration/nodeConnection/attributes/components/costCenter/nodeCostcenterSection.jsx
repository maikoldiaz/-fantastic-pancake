import React from 'react';
import { connect } from 'react-redux';
import { Field, change } from 'redux-form';
import { required } from 'redux-form-validators';
import { inputSelect, inputToggler } from '../../../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../../../common/services/resourceProvider';
import { constants } from '../../../../../../common/services/constants';
import { utilities } from '../../../../../../common/services/utilities';


export class NodeCostcenterSection extends React.Component {
    render() {
        let { costCenter = [] } = this.props;

        costCenter = costCenter.filter(cc => cc.isActive);

        return (
            <>
                <div className="col-md-6">
                    <div className="ep-control-group">
                        <label id="lbl_sourceNode_name" className="ep-label" htmlFor="txt_sourceNode_name">{resourceProvider.read('sourceNode')}</label>
                        <Field component={inputSelect} name="sourceNode" id="dd_sourceNode_type" inputId="dd_sourceNode_type_sel"
                            placeholder={resourceProvider.read('nodeType')} options={[this.props.sourceNode]} isDisabled={true}
                            getOptionLabel={x => x.name} getOptionValue={x => x.sourceNodeId}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                    </div>
                    <div className="ep-control-group">
                        <label id="lbl_destinationNode_name" className="ep-label" htmlFor="txt_destinationNode_name">{resourceProvider.read('destinationNode')}</label>
                        <Field component={inputSelect} name="destinationNode" id="dd_destinationNode_type" inputId="dd_destinationNode_type_sel"
                            placeholder={resourceProvider.read('nodeType')} options={[this.props.destinationNode]} isDisabled={true}
                            getOptionLabel={x => x.name} getOptionValue={x => x.destinationNodeId}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                    </div>
                    <div className="ep-control-group  m-b-0">
                        <label className="ep-label">{resourceProvider.read('active')}</label>
                        <Field component={inputToggler} name="isActive" id="tog_category_active" />
                    </div>
                </div>
                <div className="col-md-6">
                    <div className="ep-control-group">
                        <label id="lbl_movementType_name" className="ep-label" htmlFor="txt_movementType_name">{resourceProvider.read('sourceCategoryElement')}</label>
                        <Field component={inputSelect} name="movementType"
                            placeholder={resourceProvider.read('nodeType')} options={[this.props.movementType]} isDisabled={true}
                            getOptionLabel={x => x.name} getOptionValue={x => x.movementTypeId}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                    </div>
                    <div className="ep-control-group" >
                        <label id="lbl_costCenter_name" className="ep-label" htmlFor="txt_costCenter_name">{resourceProvider.read('costCenter')} <span style={{ color: 'red' }} >*</span></label>
                        <Field component={inputSelect} name="costCenter"
                            placeholder={resourceProvider.read('nodeType')} options={costCenter}
                            getOptionLabel={x => x.name} getOptionValue={x => x.costCenterId}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } })]} />
                    </div>
                </div>

            </>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        sourceNode: state.nodeConnection.nodeCostCenters.initialValues.sourceNode,
        destinationNode: state.nodeConnection.nodeCostCenters.initialValues.destinationNode,
        movementType: state.nodeConnection.nodeCostCenters.initialValues.movementType,
        costCenter: state.shared.groupedCategoryElements[constants.Category.CostCenter]
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        dispatchActions: actions => {
            actions.forEach(dispatch);
        },
        changeField: (field, value) => {
            dispatch(change('nodeCostCenter', field, value));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(NodeCostcenterSection);
