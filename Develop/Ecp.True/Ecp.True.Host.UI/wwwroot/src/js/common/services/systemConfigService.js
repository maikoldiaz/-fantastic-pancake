import { utilities } from './utilities';
import uuid from 'uuid';

const systemConfigService = (function () {
    let systemConfiguration = null;
    const sessionId = uuid.v4();

    return {
        initialize: configuration => {
            systemConfiguration = configuration;
        },
        getAcceptableBalancePercentage: () => {
            return utilities.getValueOrDefault(systemConfiguration.acceptableBalancePercentage, 0);
        },
        getControlLimit: () => {
            return utilities.getValueOrDefault(systemConfiguration.controlLimit, 0);
        },
        getDefaultCutoffLastDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.cutOff.lastDays, 0);
        },
        getStandardUncertaintyPercentage: () => {
            return utilities.getValueOrDefault(systemConfiguration.standardUncertaintyPercentage, 0);
        },
        getDefaultOwnershipCalculationLastDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.ownershipCalculation.lastDays, 0);
        },
        getDefaultOwnershipNodeLastDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.ownershipNode.lastDays, 0);
        },
        getDefaultTransportFileUploadLastDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.transportFileUpload.lastDays, 0);
        },
        getDefaultTransportFileUploadDateRange: () => {
            return utilities.getValueOrDefault(systemConfiguration.transportFileUpload.dateRange, 0);
        },
        getDefaultErrorLastDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.error.lastDays, 0);
        },
        getDefaultLogisticsTicketLastDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.logisticsOwnership.lastDays, 0);
        },
        getDefaultDeltaTicketLastDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.delta.lastDays, 0);
        },
        getDefaultOfficialDeltaTicketLastDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.officialDelta.lastDays, 0);
        },
        getDefaultOfficialDeltaTicketDateRange: () => {
            return utilities.getValueOrDefault(systemConfiguration.officialDelta.dateRange, 0);
        },
        getDefaultOfficialSivTicketLastDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.officialSiv.lastDays, 0);
        },
        getDefaultOfficialSivTicketDateRange: () => {
            return utilities.getValueOrDefault(systemConfiguration.officialSiv.dateRange, 0);
        },
        getDefaultOfficialDeltaPerNodeLastDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.officialDeltaPerNode.lastDays, 0);
        },
        getDefaultLogisticsTicketDayDifference: () => {
            return utilities.getValueOrDefault(systemConfiguration.logisticsOwnership.dateRange, 0);
        },
        getDefaultUnbalanceReportValidDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.unbalanceReportValidDays, 0);
        },
        getDefaultNodeStatusReportValidDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.nodeStatusReportValidDays, 0);
        },
        getDefaultAnsConfigurationDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.ansConfigurationDays, 0);
        },
        getDefaultAuditReportValidDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.auditReportValidDays, 0);
        },
        getDefaultOfficialDeltaReportYears: () => {
            return utilities.getValueOrDefault(systemConfiguration.officialDeltaReportDefaultYears, 0);
        },
        getCurrentMonthValidDays: () => {
            return utilities.getValueOrDefault(systemConfiguration.currentMonthValidDays, 0);
        },
        getReportsCleaningRecurrenceDuration: () => {
            return utilities.getValueOrDefault(systemConfiguration.ReportsCleanupDurationInHours, 0);
        },
        getAutocompleteItemsCount: () => {
            return utilities.getValueOrDefault(systemConfiguration.autocompleteItemsCount, 70);
        },
        getSessionId: () => {
            return sessionId;
        },
        getMaxSessionDuration: () => utilities.getValueOrDefault(systemConfiguration.maxSessionTimeoutDurationInMins, 10)
    };
}());

export { systemConfigService };
