import ModalConfigBase from '../../../common/components/modal/modalConfig';
import AddComment from './components/addComment.jsx';
import CommonAddComment from '../../../common/components/modal/addComment.jsx';
import OfficialPointsErrorMessage from './components/officialPointsErrorMessage.jsx';
import ConfirmCutoff from './components/confirmCutoff.jsx';
import ErrorDetails from '../cutOff/components/errorDetails.jsx';
import { constants } from '../../../common/services/constants';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            addComments: {
                component: AddComment,
                title: 'addComment',
                props: {
                    componentType: constants.CommentType.Cutoff
                }
            },
            commonAddComment: {
                component: CommonAddComment,
                title: 'addComment',
                props: {
                    name: 'operationalCut'
                }
            },
            cutOffConfirmation: {
                component: ConfirmCutoff,
                title: 'confirmation'
            },
            showError: {
                component: ErrorDetails
            },
            showOfficialPointsError: {
                component: OfficialPointsErrorMessage,
                title: 'officialErrorTitle'
            }
        };

        super(modalConfig);
    }
}


