import React from 'react';
import { connect } from 'react-redux';
import { reduxForm, FormSection } from 'redux-form';
import { utilities } from '../../../../../../common/services/utilities';
import NodeCostcenterSection from './nodeCostcenterSection.jsx';
import { closeModal } from '../../../../../../common/actions';
import { saveNodeCostCenter } from '../../actions';
import { dataService } from '../../dataService';
import ModalFooter from '../../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../../common/services/footerConfigService';
import { refreshGrid } from '../../../../../../common/components/grid/actions';

export class NodeCostCenter extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
    }

    onSubmit(values) {
        this.props.saveNodeCostCenter(dataService.buildNodeCostCenterObject(values.nodeCostCenter));
    }

    render() {
        return (
            <form id={`frm_node_cost_center_${this.props.mode}`} onSubmit={this.props.handleSubmit(this.onSubmit)} className="ep-form" >
                <section className="ep-modal__content">
                    <div className="row">
                        <FormSection name="nodeCostCenter">
                            <NodeCostcenterSection name="nodeCostCenterSection" mode={this.props.mode} />
                        </FormSection>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('nodeCostCenter')} />
            </form>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        initialValues: {
            nodeCostCenter: state.nodeConnection.nodeCostCenters.initialValues
        }
    };
};


/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        saveNodeCostCenter: values => {
            dispatch(saveNodeCostCenter(values));
            dispatch(refreshGrid('nodeCostCenter'));
            dispatch(closeModal('nodeCostCenter'));
        },
        closeModal: () => {
            dispatch(closeModal('nodeCostCenter'));
        },
        refreshGrid: () => {
            dispatch(refreshGrid('nodeCostCenter'));
        }
    };
};

/* istanbul ignore next */
const nodeCostCenterForm = reduxForm({
    form: 'nodeCostCenter',
    enableReinitialize: true,
    shouldAsyncValidate: utilities.shouldAsyncValidate

})(NodeCostCenter);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(nodeCostCenterForm);
