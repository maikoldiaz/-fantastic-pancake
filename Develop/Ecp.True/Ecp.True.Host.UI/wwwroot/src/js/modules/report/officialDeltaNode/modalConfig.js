import ModalConfigBase from '../../../common/components/modal/modalConfig';
import AddManualMovements from './components/addManualMovements.jsx';
import Reopen from './components/reopen.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            reopenDeltaNode: {
                component: Reopen,
                title: 'reopenNodesTitle'
            },
            addManualMovementsDeltaNode: {
                component: AddManualMovements,
                title: 'addManualMovementsDeltaNode'
            }
        };
        super(modalConfig);
    }
}
