import { systemConfigService } from '../../../common/services/systemConfigService';
describe('system configuration service',
    () => {
        it('should get acceptable balance percentage', () =>{
            const configuration = {
                acceptableBalancePercentage: 10
            };
            systemConfigService.initialize(configuration);
            const acceptablePercentage = systemConfigService.getAcceptableBalancePercentage();
            expect(acceptablePercentage).toBe(10);
        });

        it('should get control limit', () =>{
            const configuration = {
                controlLimit: 20
            };
            systemConfigService.initialize(configuration);
            const controlLimit = systemConfigService.getControlLimit();
            expect(controlLimit).toBe(20);
        });

        it('should get default cut off last days', () =>{
            const configuration = {
                cutOff: {
                    lastDays: 5
                }
            };
            systemConfigService.initialize(configuration);
            const lastDays = systemConfigService.getDefaultCutoffLastDays();
            expect(lastDays).toBe(5);
        });

        it('should get standard uncertainty percentage', () =>{
            const configuration = {
                standardUncertaintyPercentage: 0.4
            };
            systemConfigService.initialize(configuration);
            const uncertaintyPercentage = systemConfigService.getStandardUncertaintyPercentage();
            expect(uncertaintyPercentage).toBe(0.4);
        });

        it('should get ownership calculation last days', () =>{
            const configuration = {
                ownershipCalculation: {
                    lastDays: 6
                }
            };
            systemConfigService.initialize(configuration);
            const ownershipCalculation = systemConfigService.getDefaultOwnershipCalculationLastDays();
            expect(ownershipCalculation).toBe(6);
        });

        it('should get default ownership node last days', () =>{
            const configuration = {
                ownershipNode: {
                    lastDays: 7
                }
            };
            systemConfigService.initialize(configuration);
            const lastDays = systemConfigService.getDefaultOwnershipNodeLastDays();
            expect(lastDays).toBe(7);
        });

        it('should get default transport file upload last days', () =>{
            const configuration = {
                transportFileUpload: {
                    lastDays: 8
                }
            };
            systemConfigService.initialize(configuration);
            const lastDays = systemConfigService.getDefaultTransportFileUploadLastDays();
            expect(lastDays).toBe(8);
        });

        it('should get default transport file upload date range', () =>{
            const configuration = {
                transportFileUpload: {
                    dateRange: 9
                }
            };
            systemConfigService.initialize(configuration);
            const dateRange = systemConfigService.getDefaultTransportFileUploadDateRange();
            expect(dateRange).toBe(9);
        });

        it('should get default error last days', () =>{
            const configuration = {
                error: {
                    lastDays: 11
                }
            };
            systemConfigService.initialize(configuration);
            const lastDays = systemConfigService.getDefaultErrorLastDays();
            expect(lastDays).toBe(11);
        });

        it('should get default logistics ticket last days', () =>{
            const configuration = {
                logisticsOwnership: {
                    lastDays: 11
                }
            };
            systemConfigService.initialize(configuration);
            const lastDays = systemConfigService.getDefaultLogisticsTicketLastDays();
            expect(lastDays).toBe(11);
        });

        it('should get default logistics ticket date range', () =>{
            const configuration = {
                logisticsOwnership: {
                    dateRange: 11
                }
            };
            systemConfigService.initialize(configuration);
            const dateRange = systemConfigService.getDefaultLogisticsTicketDayDifference();
            expect(dateRange).toBe(11);
        });

        it('should get default unbalance report valid days', () =>{
            const configuration = {
                unbalanceReportValidDays: 23
            };
            systemConfigService.initialize(configuration);
            const dateRange = systemConfigService.getDefaultUnbalanceReportValidDays();
            expect(dateRange).toBe(23);
        });

        it('should get default unbalance report valid days', () =>{
            const configuration = {
                nodeStatusReportValidDays: 25
            };
            systemConfigService.initialize(configuration);
            const dateRange = systemConfigService.getDefaultNodeStatusReportValidDays();
            expect(dateRange).toBe(25);
        });

        it('should get default ans configuration days', () =>{
            const configuration = {
                ansConfigurationDays: 78
            };
            systemConfigService.initialize(configuration);
            const dateRange = systemConfigService.getDefaultAnsConfigurationDays();
            expect(dateRange).toBe(78);
        });

        it('should get default setting audit report valid days', () =>{
            const configuration = {
                auditReportValidDays: 62
            };
            systemConfigService.initialize(configuration);
            const dateRange = systemConfigService.getDefaultAuditReportValidDays();
            expect(dateRange).toBe(62);
        });

        it('should get default setting session max duration', () =>{
            const configuration = {
                maxSessionTimeoutDurationInMins: 60
            };
            systemConfigService.initialize(configuration);
            const dateRange = systemConfigService.getMaxSessionDuration();
            expect(dateRange).toBe(60);
        });

        it('should get autocomplete items count', () =>{
            const configuration = { autocompleteItemsCount: 5 };
            systemConfigService.initialize(configuration);
            const autocompleteItems = systemConfigService.getAutocompleteItemsCount();
            expect(autocompleteItems).toBe(5);
        });
    });

