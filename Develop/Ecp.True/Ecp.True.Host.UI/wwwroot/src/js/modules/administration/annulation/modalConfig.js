import ModalConfigBase from './../../../common/components/modal/modalConfig';
import Annulation from './components/annulation.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            createRelation: {
                component: Annulation,
                title: 'Annulation'
            }
        };

        super(modalConfig);
    }
}
