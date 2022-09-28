import { resourceProvider } from '../../../../common/services/resourceProvider';
import { dateService } from '../../../../common/services/dateService';
import { ticketValidator } from '../../../../modules/transportBalance/cutOff/services/ticketValidationService';
import { serverValidator } from '../../../../common/services/serverValidator';

describe('Movement Validator',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
        });

        it('should throw error for invalid initial and final dates', async () => {
            const props = {
                showError: jest.fn()
            };
            const initialDate = dateService.now();
            const finalDate = dateService.subtract(dateService.now(), 1, 'day');
            const segment = 123;
            await expect(ticketValidator.validateAsync(props, initialDate, finalDate, segment)).rejects.toThrow('Submit Validation Failed');
        });

        it('should throw error for null or undefined controlLimit', async () => {
            const props = {
                showError: jest.fn(),
                systemConfig: {
                    controlLimit: null
                }
            };
            const initialDate = dateService.now();
            const finalDate = dateService.add(dateService.now(), 1, 'day');
            const segment = 123;
            await expect(ticketValidator.validateAsync(props, initialDate, finalDate, segment)).rejects.toThrow('Submit Validation Failed');
        });

        it('should throw error for null or undefined standardUncertaintyPercentage', async () => {
            const props = {
                showError: jest.fn(),
                systemConfig: {
                    controlLimit: 123,
                    standardUncertaintyPercentage: null
                }
            };
            const initialDate = dateService.now();
            const finalDate = dateService.add(dateService.now(), 1, 'day');
            const segment = 123;
            await expect(ticketValidator.validateAsync(props, initialDate, finalDate, segment)).rejects.toThrow('Submit Validation Failed');
        });

        it('should throw error for null or undefined acceptableBalancePercentage', async () => {
            const props = {
                showError: jest.fn(),
                systemConfig: {
                    controlLimit: 123,
                    standardUncertaintyPercentage: 123,
                    acceptableBalancePercentage: null
                }
            };
            const initialDate = dateService.now();
            const finalDate = dateService.add(dateService.now(), 1, 'day');
            const segment = 123;
            await expect(ticketValidator.validateAsync(props, initialDate, finalDate, segment)).rejects.toThrow('Submit Validation Failed');
        });

        it('should throw error for ticket already exists or processing', async () => {
            const props = {
                showLoader: jest.fn(),
                hideLoader: jest.fn(),
                showError: jest.fn(),
                systemConfig: {
                    controlLimit: 123,
                    standardUncertaintyPercentage: 123,
                    acceptableBalancePercentage: 23
                }
            };

            const json = { body: true };

            serverValidator.validateUniqueSegmentTicket = jest.fn(() => json);

            const initialDate = dateService.now();
            const finalDate = dateService.add(dateService.now(), 1, 'day');
            const segment = 123;
            await expect(ticketValidator.validateAsync(props, initialDate, finalDate, segment)).rejects.toThrow('Submit Validation Failed');
        });

        it('should throw error for delta ticket exists or processing', async () => {
            const props = {
                showLoader: jest.fn(),
                hideLoader: jest.fn(),
                showError: jest.fn(),
                systemConfig: {
                    controlLimit: 123,
                    standardUncertaintyPercentage: 123,
                    acceptableBalancePercentage: 34
                }
            };
            const json = { body: false };
            serverValidator.validateUniqueSegmentTicket = jest.fn(() => json);
            serverValidator.validateDeltaTicket = jest.fn(() => true);

            const initialDate = dateService.now();
            const finalDate = dateService.add(dateService.now(), 1, 'day');
            const segment = 123;
            await expect(ticketValidator.validateAsync(props, initialDate, finalDate, segment)).rejects.toThrow('Submit Validation Failed');
        });

        it('should throw error for ticket with the given criteria already exists or processing', async () => {
            const props = {
                showLoader: jest.fn(),
                hideLoader: jest.fn(),
                showError: jest.fn(),
                systemConfig: {
                    controlLimit: 123,
                    standardUncertaintyPercentage: 123,
                    acceptableBalancePercentage: 55
                }
            };

            const json = { body: false };
            serverValidator.validateUniqueSegmentTicket = jest.fn(() => json);
            serverValidator.validateDeltaTicket = jest.fn(() => false);
            serverValidator.validateTicket = jest.fn(() => true);

            const initialDate = dateService.now();
            const finalDate = dateService.add(dateService.now(), 1, 'day');
            const segment = 123;
            await expect(ticketValidator.validateAsync(props, initialDate, finalDate, segment)).rejects.toThrow('Submit Validation Failed');
        });
    });
