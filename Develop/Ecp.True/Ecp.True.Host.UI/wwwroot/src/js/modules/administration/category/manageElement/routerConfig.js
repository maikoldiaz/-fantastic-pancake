import ElementsGrid from './components/elementsGrid.jsx';
import RouterConfigBase from './../../../../common/router/routerConfig';
import { constants } from './../../../../common/services/constants';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                attributes: {
                    routeKey: 'manage',
                    title: 'manageElements',
                    component: ElementsGrid,
                    actions: [
                        {
                            title: 'createElement',
                            iconClass: 'fas fa-plus-square',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'modal',
                            key: 'createElement',
                            mode: constants.Modes.Create
                        }
                    ]
                }
            }
        };

        super('element', routeConfig);
    }
}
