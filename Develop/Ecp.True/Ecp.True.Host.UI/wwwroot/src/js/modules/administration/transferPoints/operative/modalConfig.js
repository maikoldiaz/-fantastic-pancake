import ModalConfigBase from './../../../../common/components/modal/modalConfig';
import CreateTransferPointOperational from './components/create.jsx';
import DeleteTransferPointOperational from './components/delete.jsx';
import { constants } from '../../../../common/services/constants';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            createTransferPointOperational: {
                component: CreateTransferPointOperational,
                title: constants.NodeConnectionState.TransferPoint
            },
            updateTransferPointOperational: {
                component: CreateTransferPointOperational,
                title: constants.NodeConnectionState.TransferPoint
            },
            deleteTransferPointOperational: {
                component: DeleteTransferPointOperational,
                title: constants.NodeConnectionState.TransferPoint
            }
        };

        super(modalConfig);
    }
}
