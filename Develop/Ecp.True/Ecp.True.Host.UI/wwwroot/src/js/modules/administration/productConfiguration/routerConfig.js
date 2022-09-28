import RouterConfigBase from '../../../common/router/routerConfig.js';
import productConfigurationPanel from './components/productConfigurationPanel.jsx';
import CreateAssociation from './components/storageLocationProduct/createAssociation.jsx';


export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                productConfiguration: {
                    routeKey: 'manage',
                    component: productConfigurationPanel,
                    actions: [
                    ]
                },
                createassociation: {
                    routeKey: 'createassociation',
                    actionType: 'navigate',
                    component: CreateAssociation
                }
            }
        };

        super('productConfiguration', routeConfig);
    }
}
