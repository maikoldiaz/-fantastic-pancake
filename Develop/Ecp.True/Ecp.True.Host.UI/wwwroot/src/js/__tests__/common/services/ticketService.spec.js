import { ticketService } from '../../../modules/transportBalance/cutOff/services/ticketService';
import { systemConfigService } from '../../../common/services/systemConfigService';
describe('ticket service',
    () => {
        it('should get ticketInfo', () => {
            systemConfigService.getDefaultCutoffLastDays = jest.fn();
            systemConfigService.getDefaultOwnershipCalculationLastDays = jest.fn();
            systemConfigService.getDefaultLogisticsTicketLastDays = jest.fn();
            systemConfigService.getDefaultDeltaTicketLastDays = jest.fn();
            systemConfigService.getDefaultOfficialDeltaTicketLastDays = jest.fn();
            const props = {
                componentType: 1
            };
            const ticketInfo = ticketService.getGridInfo(props);
            expect(ticketInfo.filter).toBe('ticketTypeId eq \'Cutoff\'');
            expect(ticketInfo.title).toBe('Error al procesar corte operativo');
            expect(ticketInfo.popupName).toBe('showError');
        });
    });

