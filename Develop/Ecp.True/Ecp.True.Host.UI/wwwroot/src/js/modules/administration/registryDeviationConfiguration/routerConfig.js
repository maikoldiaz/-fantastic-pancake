import RouterConfigBase from '../../../common/router/routerConfig.js';
import Segments from './components/segments.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                registryDeviationConfiguration: {
                    routeKey: 'manage',
                    bcrumbsKey: '',
                    component: Segments
                }
            }
        };

        super('registryDeviationConfiguration', routeConfig);
    }
}
