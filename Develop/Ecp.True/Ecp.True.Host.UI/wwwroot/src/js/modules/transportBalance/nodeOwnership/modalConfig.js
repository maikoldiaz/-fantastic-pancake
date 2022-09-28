import ModalConfigBase from '../../../common/components/modal/modalConfig';
import OwnershipNodeErrorDetailsGrid from './components/ownershipNodeErrorDetailsGrid.jsx';
import MovementOwnershipDetails from './components/movementOwnershipDetails.jsx';
import AddComment from '../../../common/components/modal/addComment.jsx';
import InventoryOwnershipDetails from './components/inventoryOwnershipDetails.jsx';
import NavigationPrompt from '../../../common/components/awareness/navigationPrompt.jsx';
import CreateMovement from './components/createMovement.jsx';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            showError: {
                component: OwnershipNodeErrorDetailsGrid,
                title: 'ownershipNodeError'
            },
            movementOwnership: {
                component: MovementOwnershipDetails,
                title: 'editMovement'
            },
            addComment: {
                component: AddComment,
                title: 'addReopenComment',
                props: {
                    name: 'ownershipNode'
                }
            },
            inventoryOwnership: {
                component: InventoryOwnershipDetails,
                title: 'inventoryOwnershipEdit'
            },
            navigationConfirmation: {
                component: NavigationPrompt,
                title: 'NavigationConfirmation',
                props: {
                }
            },
            createMovement: {
                component: CreateMovement,
                title: 'newMovement'
            }
        };

        super(modalConfig);
    }
}
