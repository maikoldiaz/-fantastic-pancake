import ModalConfigBase from '../../../common/components/modal/modalConfig';
import ErrorDetails from '../../transportBalance/cutOff/components/errorDetails.jsx';
import OfficialLogisticsWizard from './components/officialLogisticsWizard.jsx';
import { constants } from '../../../common/services/constants';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            showError: {
                component: ErrorDetails,
                title: 'officialLogisticsError'
            },
            createOfficialLogistics: {
                component: OfficialLogisticsWizard,
                title: 'createLogistics',
                props: {
                    componentType: constants.TicketType.OfficialLogistics
                }
            }
        };

        super(modalConfig);
    }
}


