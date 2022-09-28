import ModalConfigBase from '../../../../common/components/modal/modalConfig';
import CreateStorageLocation from './components/createStorageLocation.jsx';
import AddProducts from './components/addProducts.jsx';
import AutoOrderNodes from './components/autoOrderNodes.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            createStorageLocation: {
                component: CreateStorageLocation,
                title: 'StorageLocation'
            },
            addProducts: {
                component: AddProducts,
                title: 'addProducts'
            },
            autoOrderNodes: {
                component: AutoOrderNodes,
                title: 'duplicateOrderTitle'
            }
        };

        super(modalConfig);
    }
}

