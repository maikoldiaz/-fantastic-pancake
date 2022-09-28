import ModalConfigBase from './../../../../common/components/modal/modalConfig';
import CreateCategory from './components/createCategory.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            createCategory: {
                component: CreateCategory,
                title: 'Category'
            }
        };

        super(modalConfig);
    }
}
