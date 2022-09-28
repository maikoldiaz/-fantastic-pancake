import { constants } from '../../../common/services/constants.js';
import RouterConfigBase from '../../../common/router/routerConfig.js';
import FileUploadGrid from './components/fileUploadGrid.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                attributes: {
                    routeKey: 'manage',
                    component: FileUploadGrid,
                    actions: [
                        {
                            title: 'loadNew',
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
                        systemType: constants.SystemType.CONTRACT
                    },
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances,
                        constants.Roles.Programmer
                    ]
                }
            }
        };

        super('transportContracts', routeConfig);
    }
}
