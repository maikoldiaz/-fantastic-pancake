import ModalConfigBase from './../../../common/components/modal/modalConfig';
import Product from './components/product/product.jsx';
import AssociationSavedModal from './components/storageLocationProduct/associationSavedModal.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            modalProduct: {
                component: Product,
                title: 'Product'
            },
            associationSavedModal: {
                component: AssociationSavedModal,
                title: 'saveCostCenterWithDuplicates'
            }
        };

        super(modalConfig);
    }
}
