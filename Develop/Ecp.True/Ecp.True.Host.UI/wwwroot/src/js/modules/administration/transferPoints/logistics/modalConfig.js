import ModalConfigBase from './../../../../common/components/modal/modalConfig';
import CreateTransferPointLogistic from './components/create.jsx';
import DeleteTransferPointLogistic from './components/delete.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            create: {
                component: CreateTransferPointLogistic,
                title: 'createTransferPoint'
            },
            delete: {
                component: DeleteTransferPointLogistic,
                title: 'deleteTransferPoint'
            }
        };

        super(modalConfig);
    }
}
