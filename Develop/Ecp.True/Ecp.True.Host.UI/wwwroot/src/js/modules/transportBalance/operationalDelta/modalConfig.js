import ModalConfigBase from '../../../common/components/modal/modalConfig';
import OperationalDeltasWizard from './components/operationalDeltasWizard.jsx';
import DeltaCalculationsTechnicalError from '../../../common/components/modal/deltaCalculationsTechnicalError.jsx';
import DeltaCalculationBusinessError from '../../../common/components/modal/deltaCalculationBusinessError.jsx';
import ConfirmDeltaCalculation from './components/confirmDeltaCalculation.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            operationalDeltas: {
                component: OperationalDeltasWizard,
                title: 'executeNewVolumeBalance'
            },
            showDeltaError: {
                component: DeltaCalculationsTechnicalError
            },
            showDeltaBusinessError: {
                component: DeltaCalculationBusinessError
            },
            confirmDeltaCalculation: {
                component: ConfirmDeltaCalculation
            }
        };

        super(modalConfig);
    }
}


