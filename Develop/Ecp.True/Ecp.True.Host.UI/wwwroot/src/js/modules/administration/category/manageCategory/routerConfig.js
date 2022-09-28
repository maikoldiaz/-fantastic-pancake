import CategoriesGrid from './components/categoriesGrid.jsx';
import RouterConfigBase from './../../../../common/router/routerConfig';
import { constants } from './../../../../common/services/constants';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                category: {
                    routeKey: 'manage',
                    title: 'manageCategories',
                    component: CategoriesGrid,
                    actions: [
                        {
                            title: 'createCategory',
                            iconClass: 'fas fa-plus-square',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'modal',
                            key: 'createCategory',
                            mode: constants.Modes.Create
                        }
                    ]
                }
            }
        };

        super('category', routeConfig);
    }
}
