import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';

export class NodesInTicketDetail extends React.Component {
    render() {
        return (
            <>
                <div className="ep-modal__content">
                    <p className="m-t-0">{resourceProvider.read('nodesInTicketTitle')}</p>
                    {this.props.nodesInTicket.length > 0 && <ul>
                        {this.props.nodesInTicket.map(node => (
                            <li key={node.elementId}>{node.name}</li>
                        ))}
                    </ul>}
                    {this.props.nodesInTicket.length === 0 && <p>
                        {resourceProvider.read('all')}
                    </p>}
                </div>
                <ModalFooter config={footerConfigService.getAcceptConfig('NodesInTicketDetail', {
                    closeModal: true, acceptText: 'accept', acceptClassName: 'ep-btn ep-btn--sm ep-btn--primary'
                })} />
            </>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        nodesInTicket: state.sendToSap.nodesInTicket
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps)(NodesInTicketDetail);
