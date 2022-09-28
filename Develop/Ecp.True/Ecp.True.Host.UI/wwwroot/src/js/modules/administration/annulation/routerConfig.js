import AnnulationsGrid from './components/annulationsGrid.jsx';
import { constants } from './../../../common/services/constants';
import RouterConfigBase from './../../../common/router/routerConfig';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                annulations: {
                    routeKey: 'manage',
                    component: AnnulationsGrid,
                    actions: [
                        {
                            title: 'createRelation',
                            iconClass: 'fas fa-plus-square',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'modal',
                            key: 'createRelation',
                            mode: constants.Modes.Create
                        }
                    ]
                }
            }
        };

        super('annulations', routeConfig);
    }
}
