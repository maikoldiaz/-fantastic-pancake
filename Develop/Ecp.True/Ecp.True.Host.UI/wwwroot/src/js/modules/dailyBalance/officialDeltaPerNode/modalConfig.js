import ModalConfigBase from '../../../common/components/modal/modalConfig';
import DeltaCalculationBusinessError from '../../../common/components/modal/deltaCalculationBusinessError.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            showDeltaBusinessError: {
                component: DeltaCalculationBusinessError,
                title: 'officialDeltaPerNodeError'
            }
        };

        super(modalConfig);
    }
}
