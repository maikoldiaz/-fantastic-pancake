import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { submitWithAutoReOrder } from '../../manageNode/actions';
import { utilities } from '../../../../../common/services/utilities';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';

class AutoOrderNodes extends React.Component {
    constructor() {
        super();
        this.processSaveNode = this.processSaveNode.bind(this);
    }

    processSaveNode() {
        this.props.saveNode();
        this.props.closeModal();
    }

    render() {
        return (
            <>
                <section className="ep-modal__content">
                    <div className="row">
                        <div className="col-md-12">
                            <div className="ep-control-group m-b-3">
                                <p className="fw-bold">{resourceProvider.readFormat('duplicateOrderText', [this.props.name])}</p>
                                <p className="fw-bold">{resourceProvider.read('autoOrderNodes')}</p>
                                {this.props.showCreateNodePanel &&
                                    <p className="fw-bold">{resourceProvider.read('graphicalNodeReOrderInitialState')}</p>}
                            </div>
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('autoOrderNodes',
                    { onAccept: this.processSaveNode, acceptText: 'accept' })} />
            </>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        name: state.node.manageNode.existingNode.name,
        showCreateNodePanel: utilities.isNullOrUndefined(state.nodeGraphicalConnection.showCreateNodePanel) ? false : state.nodeGraphicalConnection.showCreateNodePanel
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        saveNode: () => {
            dispatch(submitWithAutoReOrder());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(AutoOrderNodes);
