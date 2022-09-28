import RouterConfigBase from '../../../common/router/routerConfig';
import { constants } from '../../../common/services/constants';
import OfficialDeltasGrid from './components/officialDeltasGrid.jsx';
import OfficialDeltasWizard from './components/officialDeltasWizard.jsx';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                officialDelta: {
                    routeKey: 'manage',
                    component: OfficialDeltasGrid,
                    actions: [
                        {
                            title: 'newDeltasCalculation',
                            iconClass: 'fa fa-plus-square',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'navigate',
                            route: 'officialDelta/create'
                        }
                    ],
                    roles: [
                        constants.Roles.Chain
                    ],
                    props: {
                        componentType: constants.TicketType.OfficialDelta
                    }
                },
                operationalDeltaCreate: {
                    routeKey: 'create',
                    component: OfficialDeltasWizard,
                    roles: [
                        constants.Roles.Chain
                    ],
                    actions: [
                        {
                            title: 'returnListing',
                            type: constants.RouterActions.Type.Link,
                            iconClass: 'fas fa-angle-left',
                            actionType: 'navigate',
                            route: 'officialDelta/manage'
                        }
                    ]
                }
            }
        };

        super('officialDelta', routeConfig);
    }
}
