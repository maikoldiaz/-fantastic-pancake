import ModalConfigBase from '../../../../common/components/modal/modalConfig';
import ControlLimit from './../attributes/components/controlLimit.jsx';
import Uncertainty from './components/uncertainty.jsx';
import OwnersPie from './components/ownersPie.jsx';
import Properties from './components/properties.jsx';
import EditOwners from './components/ownersWizard.jsx';
import BulkUpdateConfirm from '../../../../common/components/modal/bulkUpdateConfirm.jsx';
import BulkUpdate from '../../../../common/components/modal/bulkUpdate.jsx';
import { constants } from '../../../../common/services/constants';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            editNodeControlLimit: {
                component: ControlLimit,
                title: 'editNodeControlLimit'
            },
            editUncertainty: {
                component: Uncertainty,
                title: 'editNodeUncertainty'
            },
            ownersPie: {
                component: OwnersPie,
                title: 'properties'
            },
            editOwners: {
                component: EditOwners,
                title: 'editProperties'
            },
            editProperties: {
                component: Properties,
                title: 'editPropertyFunctions'
            },
            [`${constants.RuleType.NodeProduct}Confirm`]: {
                component: BulkUpdateConfirm,
                title: 'confirmation',
                props: { type: constants.RuleType.NodeProduct, gridName: constants.GridNames.Node }
            },
            [`${constants.RuleType.NodeProduct}BulkUpdate`]: {
                component: BulkUpdate,
                title: 'editPropertyFunctions',
                props: { type: constants.RuleType.NodeProduct, gridName: constants.GridNames.NodeProduct }
            },
            [`${constants.RuleType.Node}Confirm`]: {
                component: BulkUpdateConfirm,
                title: 'confirmation',
                props: { type: constants.RuleType.Node, gridName: constants.GridNames.Node }
            },
            [`${constants.RuleType.Node}BulkUpdate`]: {
                component: BulkUpdate,
                title: 'editPropertyFunctions',
                props: { type: constants.RuleType.Node, gridName: constants.GridNames.Node }
            },
            [`${constants.RuleType.StorageLocationProductVariable}BulkUpdate`]: {
                component: BulkUpdate,
                title: 'editPropertyFunctionsNodeProduct',
                props: { type: constants.RuleType.StorageLocationProductVariable, gridName: constants.GridNames.NodeAttributeProduct }
            },
            [`${constants.RuleType.NodeAttribute}BulkUpdate`]: {
                component: BulkUpdate,
                title: 'editPropertyFunctions',
                props: { type: constants.RuleType.NodeAttribute, gridName: constants.GridNames.NodeAttribute }
            }
        };

        super(modalConfig);
    }
}
