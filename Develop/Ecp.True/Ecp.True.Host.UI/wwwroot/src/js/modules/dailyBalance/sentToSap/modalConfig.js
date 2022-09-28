import ModalConfigBase from '../../../common/components/modal/modalConfig';
import ConfirmMovements from './components/confirmMovements.jsx';
import ErrorDetails from '../../transportBalance/cutOff/components/errorDetails.jsx';
import NodesInTicketDetail from './components/nodesInTicketDetail.jsx';
import ConfirmCancelBatch from './components/confirmCancelBatch.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            movementsConfirmation: {
                component: ConfirmMovements,
                title: 'confirmation'
            },
            showError: {
                component: ErrorDetails
            },
            nodesInTicket: {
                component: NodesInTicketDetail
            },
            cancelBatchConfirmation: {
                component: ConfirmCancelBatch,
                title: 'confirmation'
            }
        };

        super(modalConfig);
    }
}
