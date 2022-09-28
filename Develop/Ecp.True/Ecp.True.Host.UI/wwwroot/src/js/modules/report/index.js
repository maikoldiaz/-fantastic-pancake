import { combineReducers } from 'redux';
import { bootstrapService } from '../../common/services/bootstrapService';
import analyticsRouterConfig from './analytics/routerConfig';
import cutOffRouterConfig from './cutOff/routerConfig';
import nodeConfigurationReportRouterConfig from './nodeConfiguration/routerConfig';
import nodeStatusReportRouterConfig from './nodeStatus/routerConfig';
import balanceControlChartRouterConfig from './balanceControl/routerConfig';
import eventContractReportRouterConfig from './eventContract/routerConfig';
import settingsAuditReportRouterConfig from './settingsAudit/routerConfig';
import sentToSapReportRouterConfig from './sentToSap/routerConfig';
import officialNodesStatusReportRouterConfig from './officialNodesStatus/routerConfig';
import UserRolesAndPermissionsRouterConfig from './userRolesAndPermissions/routerConfig';
import { eventContractReport } from './eventContract/reducers';
import { balanceControlChart } from './balanceControl/reducers';
import { nodeStatusReport } from './nodeStatus/reducers';
import { settingsAuditReport } from './settingsAudit/reducers';
import { nodeConfigurationReport } from './nodeConfiguration/reducers';
import { cutOffReport } from './cutOff/reducers';
import { sentToSapReport } from './sentToSap/reducers';
import { officialNodeStatusReport } from './officialNodesStatus/reducers';
import { userRolesAndPermissions } from './userRolesAndPermissions/reducers';
import transactionsAuditRouterConfig from './transactionsAudit/routerConfig';
import { transactionsAudit } from './transactionsAudit/reducers';
import officialInitialBalanceRouterConfig from './officialInitialBalance/routerConfig';
import { initialBalance } from './officialInitialBalance/reducers';
import officialPendingBalanceRouterConfig from './officialPendingBalance/routerConfig';
import { pendingBalance } from './officialPendingBalance/reducers';
import reportExecutionRouterConfig from './reports/routerConfig';
import { reportExecution } from './reports/reducers';
import officialCustodyTransferPointsRouterConfig from './officialCustodyTransferPoints/routerConfig';
import officialDeltaRouterConfig from './officialDeltaNode/routerConfig';
import officialDeltaModalConfig from './officialDeltaNode/modalConfig';
import { officialDeltaNode } from './officialDeltaNode/reducers';

// Register modules
bootstrapService.initModule('analyticalModel', {
    routerConfig: analyticsRouterConfig,
    modalConfig: {}
});

bootstrapService.initModule('cutOffReport', {
    routerConfig: cutOffRouterConfig,
    modalConfig: {}
});

bootstrapService.initModule('nodeConfigurationReport', {
    routerConfig: nodeConfigurationReportRouterConfig,
    modalConfig: {}
});

bootstrapService.initModule('nodeStatusReport', {
    routerConfig: nodeStatusReportRouterConfig,
    modalConfig: {}
});

bootstrapService.initModule('balanceControlChart', {
    routerConfig: balanceControlChartRouterConfig,
    modalConfig: {}
});

bootstrapService.initModule('eventContractReport', {
    routerConfig: eventContractReportRouterConfig,
    modalConfig: {}
});

bootstrapService.initModule('settingsAudit', {
    routerConfig: settingsAuditReportRouterConfig,
    modalConfig: {}
});

bootstrapService.initModule('transactionsAudit', {
    routerConfig: transactionsAuditRouterConfig,
    modalConfig: {}
});

bootstrapService.initModule('officialbalanceloaded', {
    routerConfig: officialInitialBalanceRouterConfig,
    modalConfig: {}
});

bootstrapService.initModule('previousperiodpendingofficial', {
    routerConfig: officialPendingBalanceRouterConfig,
    modalConfig: {}
});

bootstrapService.initModule('generatedreport', {
    routerConfig: reportExecutionRouterConfig,
    modalConfig: {}
});

bootstrapService.initModule('officialcustodytransferpoints', {
    routerConfig: officialCustodyTransferPointsRouterConfig,
    modalConfig: {}
});

bootstrapService.initModule('officialdeltanode', {
    routerConfig: officialDeltaRouterConfig,
    modalConfig: officialDeltaModalConfig
});

bootstrapService.initModule('senttosapreport', {
    routerConfig: sentToSapReportRouterConfig,
    modalConfig: {}
});

bootstrapService.initModule('officialnodestatusreport', {
    routerConfig: officialNodesStatusReportRouterConfig,
    modalConfig: {}
});

bootstrapService.initModule('userrolesandpermissions', {
    routerConfig: UserRolesAndPermissionsRouterConfig,
    modalConfig: {}
});

// Register reducer
const reportReducers = combineReducers({
    balanceControlChart,
    eventContractReport,
    cutOffReport,
    nodeConfigurationReport,
    nodeStatusReport,
    settingsAuditReport,
    transactionsAudit,
    initialBalance,
    pendingBalance,
    reportExecution,
    officialDeltaNode,
    sentToSapReport,
    officialNodeStatusReport,
    userRolesAndPermissions });
bootstrapService.registerReducer({ report: reportReducers });
