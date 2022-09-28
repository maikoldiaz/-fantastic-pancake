import ModalConfigBase from './../../../../common/components/modal/modalConfig';
import CreateElement from './components/createElement.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            createElement: {
                component: CreateElement,
                title: 'Element'
            }
        };

        super(modalConfig);
    }
}
