import ModalConfigBase from '../../../common/components/modal/modalConfig';
import { UploadFileComponent } from '../fileUploads/components/uploadFile.jsx';
import { constants } from '../../../common/services/constants';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            uploadFileModal: {
                component: UploadFileComponent,
                title: 'loadNewFile',
                props: {
                    systemType: constants.SystemType.CONTRACT
                }
            }
        };

        super(modalConfig);
    }
}
