import RouterConfigBase from '../../../common/router/routerConfig';
import manageSegments from './components/manageSegments.jsx';
// import { constants } from './../../../common/services/constants';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                operationalSegments: {
                    routeKey: 'manage',
                    title: 'operationalSegments',
                    component: manageSegments
                }
            }
        };

        super('operationalSegments', routeConfig);
    }
}
