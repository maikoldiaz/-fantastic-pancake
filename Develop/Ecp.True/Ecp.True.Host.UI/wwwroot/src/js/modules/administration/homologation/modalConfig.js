import ModalConfigBase from './../../../common/components/modal/modalConfig';
import CreateHomologation from './components/createHomologation.jsx';
import AddHomologationData from './components/addHomologationData.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            createHomologation: {
                component: CreateHomologation,
                title: 'createHomologation'
            },
            addHomologationData: {
                component: AddHomologationData,
                title: 'addHomologationData'
            }
        };

        super(modalConfig);
    }
}
