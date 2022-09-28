import ModalConfigBase from '../../../../common/components/modal/modalConfig';
import ControlLimit from './../attributes/components/controlLimit.jsx';
import Uncertainty from './components/uncertainty.jsx';
import OwnersPie from './components/ownersPie.jsx';
import EditOwners from './components/ownersWizard.jsx';
import Properties from './components/properties.jsx';
import NodeCostCenter from './components/costCenter/nodeCostCenter.jsx';
import BulkUpdate from '../../../../common/components/modal/bulkUpdate.jsx';
import BulkUpdateConfirm from '../../../../common/components/modal/bulkUpdateConfirm.jsx';
import { constants } from '../../../../common/services/constants';
import NodeCostCenterDuplicates from './components/costCenter/nodeCostCenterDuplicates.jsx';
import NodeConnectionDuplicates from './components/nodeConnectionDuplicates.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            editConnControlLimit: {
                component: ControlLimit,
                title: 'editControlLimit'
            },
            editUncertainty: {
                component: Uncertainty,
                title: 'editUncertainty'
            },
            ownersPie: {
                component: OwnersPie,
                title: 'properties'
            },
            editOwners: {
                component: EditOwners,
                title: 'editProperties'
            },
            editPriorityRules: {
                component: Properties,
                title: 'editPropertyInformation'
            },
            nodeCostCenterDuplicatesModal: {
                component: NodeCostCenterDuplicates,
                title: 'saveCostCenterWithDuplicates'
            },
            nodeConnectionDuplicatesModal: {
                component: NodeConnectionDuplicates,
                title: 'saveCostCenterWithDuplicates'
            },
            [`${constants.RuleType.NodeConnectionProduct}Confirm`]: {
                component: BulkUpdateConfirm,
                title: 'confirmation',
                props: { type: constants.RuleType.NodeConnectionProduct, gridName: constants.GridNames.NodeConnectionProduct }
            },
            [`${constants.RuleType.NodeConnectionProduct}BulkUpdate`]: {
                component: BulkUpdate,
                title: 'editPropertyFunctions',
                props: { type: constants.RuleType.NodeConnectionProduct, gridName: constants.GridNames.NodeConnectionProduct }
            },
            [`${constants.RuleType.ConnectionProduct}BulkUpdate`]: {
                component: BulkUpdate,
                title: 'editPropertyFunctions',
                props: { type: constants.RuleType.ConnectionProduct, gridName: constants.GridNames.ConnectionProduct }
            },
            editCostCenter: {
                component: NodeCostCenter,
                title: 'NodeCostCenter'
            }
        };

        super(modalConfig);
    }
}
