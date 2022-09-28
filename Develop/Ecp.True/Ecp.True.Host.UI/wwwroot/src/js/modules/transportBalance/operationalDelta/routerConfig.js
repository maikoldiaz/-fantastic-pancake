import RouterConfigBase from '../../../common/router/routerConfig.js';
import OperationalDeltaGrid from './components/operationalDeltasGrid.jsx';
import OperationalDeltaWizard from './components/operationalDeltasWizard.jsx';
import { constants } from '../../../common/services/constants';

export default class RouterConfig extends RouterConfigBase {
    constructor() {
        const routeConfig = {
            routes: {
                operationalDelta: {
                    routeKey: 'manage',
                    component: OperationalDeltaGrid,
                    actions: [
                        {
                            title: 'newDeltasCalculation',
                            iconClass: 'fa fa-plus-square',
                            type: constants.RouterActions.Type.Button,
                            actionType: 'navigate',
                            route: 'deltaCalculation/create'
                        }
                    ],
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances
                    ],
                    props: {
                        componentType: constants.TicketType.Delta
                    }
                },
                operationalDeltaCreate: {
                    routeKey: 'create',
                    component: OperationalDeltaWizard,
                    roles: [
                        constants.Roles.ProfessionalSegmentBalances
                    ],
                    actions: [
                        {
                            title: 'returnListing',
                            type: constants.RouterActions.Type.Link,
                            iconClass: 'fas fa-angle-left',
                            actionType: 'navigate',
                            route: 'deltaCalculation/manage'
                        }
                    ]
                }
            }
        };

        super('operationalDelta', routeConfig);
    }
}
