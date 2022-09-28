import ModalConfigBase from '../../../common/components/modal/modalConfig';
import LogisticsWizard from './components/logisticsWizard.jsx';
import ErrorDetails from './../cutOff/components/errorDetails.jsx';
import { constants } from '../../../common/services/constants';

export default class ModalConfig extends ModalConfigBase {
    constructor() {
        const modalConfig = {
            createLogistics: {
                component: LogisticsWizard,
                title: 'createLogistics',
                props: {
                    componentType: constants.TicketType.Logistics
                }
            },
            showError: {
                component: ErrorDetails
            }
        };

        super(modalConfig);
    }
}


