import ModalConfigBase from '../../../common/components/modal/modalConfig';
import OwnershipCalculationWizard from './components/ownershipCalculationWizard.jsx';
import ErrorDetails from './../cutOff/components/errorDetails.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            ownershipCalculation: {
                component: OwnershipCalculationWizard,
                title: 'executeNewVolumeBalance'
            },
            showError: {
                component: ErrorDetails
            }
        };

        super(modalConfig);
    }
}


