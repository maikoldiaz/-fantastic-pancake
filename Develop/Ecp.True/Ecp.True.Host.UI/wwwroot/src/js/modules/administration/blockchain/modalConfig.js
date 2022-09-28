import ModalConfigBase from '../../../common/components/modal/modalConfig';
import TransactionDetails from './components/transactionDetails.jsx';
import BlockWizard from './components/blockWizard.jsx';
import BlockRangeWizard from './components/blockRangeWizard.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            transactionDetails: {
                component: TransactionDetails,
                title: 'transactionDetails'
            },
            blockWizard: {
                component: BlockWizard,
                title: 'searchWizard'
            },
            blockRangeWizard: {
                component: BlockRangeWizard,
                title: 'searchWizard'
            }
        };

        super(modalConfig);
    }
}
