import FileUploadGrid from './components/fileUploadGrid.jsx';
import { constants } from '../../../common/services/constants';
import RouterConfigBase from '../../../common/router/routerConfig';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                attributes: {
                    routeKey: 'manage',
                    component: FileUploadGrid,
                    actions: [
                        {
                            title: 'fileUpload',
                            iconClass: 'fas fa-file-upload',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'modal',
                            key: 'uploadFileModal'
                        },
                        {
                            title: 'search',
                            iconClass: 'fas fa-search',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'flyout',
                            key: 'fileUploadFilter'
                        }
                    ],
                    props: {
                        systemType: constants.SystemType.EXCEL
                    },
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances
                    ]
                }
            }
        };

        super('fileupload', routeConfig);
    }
}
