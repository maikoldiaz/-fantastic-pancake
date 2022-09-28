import ModalConfigBase from '../../../../common/components/modal/modalConfig.js';
import { tagNodes } from './components/tagNodes.jsx';
import TagErrorsGrid from './components/tagErrorsGrid.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            new: {
                component: tagNodes,
                title: 'NodeCategoryGroup'
            },
            change: {
                component: tagNodes,
                title: 'NodeCategoryGroup'
            },
            expire: {
                component: tagNodes,
                title: 'NodeCategoryGroup'
            },
            error: {
                component: TagErrorsGrid,
                title: 'tagErrors'
            }
        };

        super(modalConfig);
    }
}

