import { systemConfigService } from '../../../../common/services/systemConfigService';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { utilities } from '../../../../common/services/utilities';

const ticketService = (function () {
    let ticketTypeInfo = null;

    function initialize() {
        ticketTypeInfo = {
            1:
            {
                filter: `ticketTypeId eq 'Cutoff'`,
                title: resourceProvider.read('ticketErrorDetailTitle'),
                popupName: resourceProvider.read('showErrorPopUp'),
                days: systemConfigService.getDefaultCutoffLastDays()
            },
            2:
            {
                filter: `ticketTypeId eq 'Ownership'`,
                title: resourceProvider.read('ownershipCalculationErrorDetailTitle'),
                popupName: resourceProvider.read('showErrorPopUp'),
                days: systemConfigService.getDefaultOwnershipCalculationLastDays()
            },
            3:
            {
                filter: `ticketTypeId eq 'Logistics'`,
                title: resourceProvider.read('logisticsErrorDetailsHeader'),
                popupName: resourceProvider.read('showErrorPopUp'),
                days: systemConfigService.getDefaultLogisticsTicketLastDays()
            },
            4:
            {
                filter: `ticketTypeId eq 'Delta'`,
                title: resourceProvider.read('operationalDeltasErrorDetailTitle'),
                popupName: resourceProvider.read('operationalDeltaTechnicalErrorPopUp'),
                days: systemConfigService.getDefaultDeltaTicketLastDays()
            },
            5:
            {
                filter: `ticketTypeId eq 'OfficialDelta'`,
                title: resourceProvider.read('operationalOfficialDeltasErrorDetailTitle'),
                popupName: resourceProvider.read('operationalDeltaTechnicalErrorPopUp'),
                days: systemConfigService.getDefaultOfficialDeltaTicketLastDays()
            },
            6:
            {
                filter: `ticketTypeId eq 'OfficialLogistics'`,
                title: resourceProvider.read('officialLogisticsErrorDetailsHeader'),
                popupName: resourceProvider.read('showErrorPopUp'),
                days: systemConfigService.getDefaultOfficialDeltaTicketLastDays()
            },
            7:
            {
                filter: `ticketTypeId eq 'LogisticMovements' and state ne 'Cancelado'`,
                title: resourceProvider.read('logisticMovementsErrorDetailsHeader'),
                popupName: resourceProvider.read('showErrorPopUp'),
                days: systemConfigService.getDefaultLogisticsTicketLastDays()
            }
        };

        return ticketTypeInfo;
    }

    return {
        getGridInfo: function (props) {
            if (utilities.isNullOrUndefined(ticketTypeInfo)) {
                initialize();
            }

            return ticketTypeInfo[props.componentType];
        }
    };
}());

export { ticketService };
