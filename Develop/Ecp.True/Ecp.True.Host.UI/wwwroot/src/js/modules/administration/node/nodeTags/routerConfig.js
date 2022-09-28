import NodeTagsGrid from './components/nodeTagsGrid.jsx';
import RouterConfigBase from '../../../../common/router/routerConfig.js';
import { constants } from '../../../../common/services/constants.js';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                attributes: {
                    routeKey: 'manage',
                    component: NodeTagsGrid,
                    actions: [
                        {
                            title: 'groupingTitle',
                            iconClass: 'fas fa-link',
                            type: constants.RouterActions.Type.Dropdown,
                            name: 'nodeTags',
                            options: [{
                                title: 'new',
                                iconClass: 'fas fa-plus-square',
                                actionType: 'modal',
                                key: 'new',
                                mode: 'new'
                            },
                            {
                                title: 'change',
                                iconClass: 'fas fa-edit',
                                actionType: 'modal',
                                key: 'change',
                                mode: 'change'
                            },
                            {
                                title: 'expire',
                                iconClass: 'fas fa-calendar-times',
                                actionType: 'modal',
                                key: 'expire',
                                mode: 'expire'
                            }]
                        },
                        {
                            title: 'search',
                            iconClass: 'fas fa-search',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'flyout',
                            key: 'nodeTags'
                        }
                    ]
                }
            }
        };

        super('nodeTags', routeConfig);
    }
}

