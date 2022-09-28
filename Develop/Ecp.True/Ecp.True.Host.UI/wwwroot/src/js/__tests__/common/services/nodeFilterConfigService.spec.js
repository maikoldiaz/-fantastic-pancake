import { nodeFilterConfigService } from '../../../common/services/nodeFilterConfigService';
import { resourceProvider } from '../../../common/services/resourceProvider';

describe('Node Filter Config Service',
    () => {
        it('should build Graphical Node Config',
            () => {
                const filterCategories = jest.fn();
                const onSubmitFilter = jest.fn();

                const config = {
                    category: {
                        label: resourceProvider.read('category'),
                        filterCategoryItems: filterCategories
                    },
                    categoryElement: {
                        label: resourceProvider.read('element')
                    },
                    node: {},
                    reportType: {
                        hidden: true,
                    },
                    initialDate: {
                        hidden: true
                    },
                    finalDate: {
                        hidden: true
                    },
                    reportType: { hidden: true },
                    dateRange: { hidden: true },
                    submitText: resourceProvider.read('show'),
                    parentPage: 'graphicalNetworkFilter',
                    onSubmitFilter
                };

                expect(nodeFilterConfigService.getGraphicalNodeConfig(filterCategories, onSubmitFilter)).toEqual(config);
            });

        it('should build Balance Control Config',
            () => {
                const props = {};
                const updateEndDate = jest.fn();
                const getEndDateProps = jest.fn();
                const filterElements = jest.fn();
                const onSubmitFilter = jest.fn();

                const config = {
                    category: {
                        hidden: true,
                        label: resourceProvider.read('category')
                    },
                    categoryElement: {
                        label: resourceProvider.read('segment'),
                        filterCategoryElementsItem: filterElements
                    },
                    node: {},
                    reportType: {
                        hidden: true,
                    },
                    dateRange: { hidden: true },
                    initialDate: {},
                    finalDate: {
                        allowAfterNow: false
                    },
                    reportType: { hidden: true },

                    submitText: resourceProvider.read('viewReport'),
                    parentPage: 'balanceControlChart',
                    onSegmentChange: updateEndDate,
                    getEndDateProps,
                    onSubmitFilter,
                    props
                };

                expect(nodeFilterConfigService.getBalanceControlConfig(props, filterElements, updateEndDate, getEndDateProps, onSubmitFilter)).toEqual(config);
            });

        it('should build Event Contract Config',
            () => {
                const getReportRequest = jest.fn();
                const filterElements = jest.fn();
                const onSubmitFilter = jest.fn();

                const config = {
                    category: {
                        hidden: true,
                        label: resourceProvider.read('category')
                    },
                    categoryElement: {
                        label: resourceProvider.read('segment'),
                        filterCategoryElementsItem: filterElements
                    },
                    node: {},
                    reportType: {
                        hidden: true,
                    },
                    initialDate: {},
                    finalDate: {
                        allowAfterNow: true
                    },
                    dateRange: { hidden: true },
                    reportType: { hidden: true },
                    submitText: resourceProvider.read('viewReport'),
                    parentPage: 'eventContractReport',
                    onSubmitFilter,
                    getReportRequest
                };

                expect(nodeFilterConfigService.getEventContractConfig(filterElements, onSubmitFilter, getReportRequest)).toEqual(config);
            });

        it('should build Node Configuration Config',
            () => {
                const filterCategories = jest.fn();
                const filterElements = jest.fn();
                const onSubmitFilter = jest.fn();
                const getReportRequest = jest.fn();

                const functions = {
                    filterCategories,
                    filterElements,
                    onSubmitFilter,
                    getReportRequest
                };

                const config = {
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
                    reportType: {
                        hidden: true,
                    },
                    initialDate: {
                        hidden: true
                    },
                    finalDate: {
                        hidden: true
                    },
                    reportType: { hidden: true },
                    dateRange: { hidden: true },
                    submitText: resourceProvider.read('viewReport'),
                    parentPage: 'nodeConfigurationReport',
                    onSubmitFilter: functions.onSubmitFilter,
                    getReportRequest: functions.getReportRequest
                };

                expect(nodeFilterConfigService.getNodeConfigurationConfig(functions)).toEqual(config);
            });

        it('should build Node Status Config',
            () => {
                const functions = {
                    filterElements: jest.fn(),
                    getTicket: jest.fn(),
                    getStartDateProps: jest.fn(),
                    getEndDateProps: jest.fn(),
                    validateDateRange: jest.fn(),
                    onSubmitFilter: jest.fn(),
                    getReportRequest: jest.fn()
                };

                const config = {
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
                    reportType: {
                        hidden: true,
                    },
                    initialDate: {},
                    finalDate: {
                        allowAfterNow: false
                    },
                    reportType: { hidden: true },
                    dateRange: { hidden: true },
                    getTicket: functions.getTicket,
                    submitText: resourceProvider.read('viewReport'),
                    parentPage: 'nodeStatusReport',
                    getStartDateProps: functions.getStartDateProps,
                    getEndDateProps: functions.getEndDateProps,
                    validateDateRange: functions.validateDateRange,
                    onSubmitFilter: functions.onSubmitFilter,
                    getReportRequest: functions.getReportRequest
                };

                expect(nodeFilterConfigService.getNodeStatusConfig(functions)).toEqual(config);
            });

            it('should build Transactions Audit Config',
            () => {
                const functions = {
                    onSegmentChange: jest.fn(),
                    getStartDateProps: jest.fn(),
                    getEndDateProps: jest.fn(),
                    onSubmitFilter: jest.fn(),
                    getReportTypes: jest.fn(),
                    validateDateRange: jest.fn()
                };

                const config = {
                    category: {
                        hidden: true
                    },
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
                    reportType: {},
                    dateRange: { hidden: true },
                    submitText: resourceProvider.read('viewReport'),
                    parentPage: 'transactionsAuditReport',
                    onSegmentChange: functions.onSegmentChange,
                    getStartDateProps: functions.getStartDateProps,
                    getEndDateProps: functions.getEndDateProps,
                    onSubmitFilter: functions.onSubmitFilter,
                    getReportTypes: functions.getReportTypes,
                    validateDateRange: functions.validateDateRange
                };

                expect(nodeFilterConfigService.getTransactionsAuditConfig(functions)).toEqual(config);
            });

        it('should build Settings Audit Config',
            () => {
                const getStartDateProps = jest.fn();
                const getEndDateProps = jest.fn();
                const validateDateRange = jest.fn();
                const onSubmitFilter = jest.fn();

                const config = {
                    category: {
                        hidden: true,
                        label: resourceProvider.read('category')
                    },
                    categoryElement: {
                        hidden: true,
                        label: resourceProvider.read('segment')
                    },
                    node: { hidden: true },
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

                expect(nodeFilterConfigService.getSettingsAuditConfig(validateDateRange, onSubmitFilter, getStartDateProps, getEndDateProps)).toEqual(config);
            });

            it('should build Official Initial Balance',
            () => {
                const filterCategories = jest.fn();
                const onSubmitFilter = jest.fn();

                const config = {
                    category: { hidden: true },
                    categoryElement: { filterCategoryElementsItem : filterCategories, label: resourceProvider.read('segment') },
                    finalDate: { hidden: true },
                    initialDate: { hidden: true },
                    node: {},
                    dateRange: { hidden: false },
                    onSubmitFilter: onSubmitFilter,
                    parentPage: "officialBalanceLoaded",
                    reportType: { hidden: true },
                    submitText: resourceProvider.read('viewReport')
                };

                expect(nodeFilterConfigService.getOfficialInitialBalance(filterCategories, onSubmitFilter)).toEqual(config);
            });

            it('should build Official Node Balance',
            () => {
                const filterCategories = jest.fn();
                const onSubmitFilter = jest.fn();

                const config = {
                    category: { hidden: true },
                    categoryElement: { filterCategoryElementsItem : filterCategories, label: resourceProvider.read('segment') },
                    finalDate: { hidden: true },
                    initialDate: { hidden: true },
                    node: {},
                    reportType: { hidden: true },
                    dateRange: { hidden: false },
                    onSubmitFilter: onSubmitFilter,
                    parentPage: "officialBalancePerNode",
                    submitText: resourceProvider.read('viewReport')
                };

                expect(nodeFilterConfigService.getOfficialNodeBalance(filterCategories, onSubmitFilter)).toEqual(config);
            });

            it('should build Official Pending Balance',
            () => {
                const filterCategories = jest.fn();
                const onSubmitFilter = jest.fn();

                const config = {
                    category: { hidden: true },
                    categoryElement: { filterCategoryElementsItem : filterCategories, label: resourceProvider.read('segment') },
                    finalDate: { hidden: true },
                    initialDate: { hidden: true },
                    node: {hidden: true},
                    onSubmitFilter: onSubmitFilter,
                    parentPage: "previousPeriodPendingOfficial",
                    reportType: { hidden: true },
                    dateRange: { hidden: true },
                    submitText: resourceProvider.read('viewReport')
                };

                expect(nodeFilterConfigService.getPendingBalance(filterCategories, onSubmitFilter)).toEqual(config);
            });
    });
