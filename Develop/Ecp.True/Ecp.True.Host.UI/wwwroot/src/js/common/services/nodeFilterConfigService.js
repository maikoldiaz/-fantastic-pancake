import { resourceProvider } from './resourceProvider';

const nodeFilterConfigService = (function () {
    return {
        getGraphicalNodeConfig: (filterCategories, onSubmitFilter) => {
            return {
                category: {
                    label: resourceProvider.read('category'),
                    filterCategoryItems: filterCategories
                },
                categoryElement: {
                    label: resourceProvider.read('element')
                },
                node: {},
                initialDate: {
                    hidden: true
                },
                finalDate: {
                    hidden: true
                },
                reportType: {
                    hidden: true
                },
                dateRange: {
                    hidden: true
                },
                submitText: resourceProvider.read('show'),
                parentPage: 'graphicalNetworkFilter',
                onSubmitFilter
            };
        },

        getBalanceControlConfig: (props, filterElements, updateEndDate, getEndDateProps, onSubmitFilter) => {
            return {
                category: {
                    hidden: true,
                    label: resourceProvider.read('category')
                },
                categoryElement: {
                    label: resourceProvider.read('segment'),
                    filterCategoryElementsItem: filterElements
                },
                node: {},
                initialDate: {},
                finalDate: {
                    allowAfterNow: false
                },
                reportType: {
                    hidden: true
                },
                dateRange: {
                    hidden: true
                },
                submitText: resourceProvider.read('viewReport'),
                parentPage: 'balanceControlChart',
                onSegmentChange: updateEndDate,
                getEndDateProps,
                onSubmitFilter,
                props
            };
        },

        getEventContractConfig: (filterElements, onSubmitFilter, getReportRequest) => {
            return {
                category: {
                    hidden: true,
                    label: resourceProvider.read('category')
                },
                categoryElement: {
                    label: resourceProvider.read('segment'),
                    filterCategoryElementsItem: filterElements
                },
                node: {},
                initialDate: {},
                finalDate: {
                    allowAfterNow: true
                },
                reportType: {
                    hidden: true
                },
                dateRange: {
                    hidden: true
                },
                submitText: resourceProvider.read('viewReport'),
                parentPage: 'eventContractReport',
                onSubmitFilter,
                getReportRequest
            };
        },

        getNodeConfigurationConfig: functions => {
            return {
                category: {
                    label: resourceProvider.read('category'),
                    filterCategoryItems: functions.filterCategories
                },
                categoryElement: {
                    label: resourceProvider.read('element'),
                    filterCategoryElementsItem: functions.filterElements
                },
                node: {
                    hidden: true
                },
                initialDate: {
                    hidden: true
                },
                finalDate: {
                    hidden: true
                },
                reportType: {
                    hidden: true
                },
                dateRange: {
                    hidden: true
                },
                submitText: resourceProvider.read('viewReport'),
                parentPage: 'nodeConfigurationReport',
                onSubmitFilter: functions.onSubmitFilter,
                getReportRequest: functions.getReportRequest
            };
        },

        getNodeStatusConfig: functions => {
            return {
                category: {
                    hidden: true,
                    label: resourceProvider.read('category')
                },
                categoryElement: {
                    label: resourceProvider.read('segment'),
                    filterCategoryElementsItem: functions.filterElements
                },
                node: {
                    hidden: true
                },
                initialDate: {},
                finalDate: {
                    allowAfterNow: false
                },
                reportType: {
                    hidden: true
                },
                dateRange: {
                    hidden: true
                },
                getTicket: functions.getTicket,
                submitText: resourceProvider.read('viewReport'),
                parentPage: 'nodeStatusReport',
                getStartDateProps: functions.getStartDateProps,
                getEndDateProps: functions.getEndDateProps,
                validateDateRange: functions.validateDateRange,
                onSubmitFilter: functions.onSubmitFilter,
                getReportRequest: functions.getReportRequest
            };
        },

        getTransactionsAuditConfig: functions => {
            return {
                category: { hidden: true },
                categoryElement: {
                    label: resourceProvider.read('segment'),
                    filterCategoryElementsItem: functions.filterElements
                },
                initialDate: {},
                finalDate: {
                    allowAfterNow: true
                },
                node: {
                    hidden: true
                },
                dateRange: {
                    hidden: true
                },
                reportType: {},
                submitText: resourceProvider.read('viewReport'),
                parentPage: 'transactionsAuditReport',
                onSegmentChange: functions.onSegmentChange,
                getStartDateProps: functions.getStartDateProps,
                getEndDateProps: functions.getEndDateProps,
                onSubmitFilter: functions.onSubmitFilter,
                getReportTypes: functions.getReportTypes,
                validateDateRange: functions.validateDateRange
            };
        },

        getSettingsAuditConfig: (validateDateRange, onSubmitFilter, getStartDateProps, getEndDateProps) => {
            return {
                category: {
                    hidden: true,
                    label: resourceProvider.read('category')
                },
                categoryElement: {
                    hidden: true,
                    label: resourceProvider.read('segment')
                },
                node: {
                    hidden: true
                },
                initialDate: {},
                finalDate: { allowAfterNow: false },
                reportType: {
                    hidden: true
                },
                dateRange: { hidden: true },
                submitText: resourceProvider.read('viewReport'),
                parentPage: 'settingsAuditReport',
                validateDateRange,
                onSubmitFilter,
                getStartDateProps,
                getEndDateProps,
                setDateProps: true
            };
        },

        getPendingBalance: (filterElements, onSubmitFilter) => {
            return {
                category: { hidden: true },
                categoryElement: {
                    label: resourceProvider.read('segment'),
                    filterCategoryElementsItem: filterElements
                },
                initialDate: { hidden: true },
                finalDate: {
                    hidden: true
                },
                node: {
                    hidden: true
                },
                reportType: {
                    hidden: true
                },
                dateRange: {
                    hidden: true
                },
                submitText: resourceProvider.read('viewReport'),
                parentPage: 'previousPeriodPendingOfficial',
                onSubmitFilter
            };
        },

        getOfficialNodeBalance: (filterElements, onSubmitFilter) => {
            return {
                category: { hidden: true },
                categoryElement: {
                    label: resourceProvider.read('segment'),
                    filterCategoryElementsItem: filterElements
                },
                initialDate: { hidden: true },
                finalDate: {
                    hidden: true
                },
                reportType: {
                    hidden: true
                },
                dateRange: {
                    hidden: false
                },
                node: { },
                submitText: resourceProvider.read('viewReport'),
                parentPage: 'officialBalancePerNode',
                onSubmitFilter
            };
        },
        getOfficialInitialBalance: (filterElements, onSubmitFilter) => {
            return {
                category: { hidden: true },
                categoryElement: {
                    label: resourceProvider.read('segment'),
                    filterCategoryElementsItem: filterElements
                },
                initialDate: { hidden: true },
                finalDate: {
                    hidden: true
                },
                reportType: {
                    hidden: true
                },
                dateRange: {
                    hidden: false
                },
                node: { },
                submitText: resourceProvider.read('viewReport'),
                parentPage: 'officialBalanceLoaded',
                onSubmitFilter
            };
        }
    };
}());

export { nodeFilterConfigService };
