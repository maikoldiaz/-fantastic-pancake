import ModalConfigBase from '../../../common/components/modal/modalConfig';
import ConfirmOfficialDelta from './components/confirm.jsx';
import DeltaCalculationsTechnicalError from '../../../common/components/modal/deltaCalculationsTechnicalError.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            confirmOfficialDelta: {
                component: ConfirmOfficialDelta
            },
            showDeltaError: {
                component: DeltaCalculationsTechnicalError,
                title: 'officialDelta'
            }
        };

        super(modalConfig);
    }
}


