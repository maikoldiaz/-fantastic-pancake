import ModalConfigBase from '../../../common/components/modal/modalConfig';
import AddComment from '../../transportBalance/cutOff/components/addComment.jsx';
import { constants } from '../../../common/services/constants';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            addComments: {
                component: AddComment,
                title: 'addErrorComment',
                props: {
                    componentType: constants.CommentType.Error
                }
            }
        };

        super(modalConfig);
    }
}
