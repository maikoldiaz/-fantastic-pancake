import RouterConfigBase from '../../../common/router/routerConfig';
import FlowSettings from './components/flowSettings.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                attributes: {
                    routeKey: 'manage',
                    component: FlowSettings
                }
            }
        };

        super('flowsettings', routeConfig);
    }
}
