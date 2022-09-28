import ModalConfigBase from '../../../common/components/modal/modalConfig';
import Transformation from './components/transformation.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            transformation: {
                component: Transformation,
                title: 'transformation'
            }
        };

        super(modalConfig);
    }
}

