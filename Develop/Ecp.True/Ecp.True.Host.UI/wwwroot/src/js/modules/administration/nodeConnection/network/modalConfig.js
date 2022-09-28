import ModalConfigBase from '../../../../common/components/modal/modalConfig';
import CreateStorageLocation from '../../node/manageNode/components/createStorageLocation.jsx';
import AddProducts from '../../node/manageNode/components/addProducts.jsx';
import AutoOrderNodes from '../../node/manageNode/components/autoOrderNodes.jsx';
import ControlLimit from '../../nodeConnection/attributes/components/controlLimit.jsx';

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
            },
            editConnControlLimit: {
                component: ControlLimit,
                title: 'editControlLimit'
            }
        };

        super(modalConfig);
    }
}
