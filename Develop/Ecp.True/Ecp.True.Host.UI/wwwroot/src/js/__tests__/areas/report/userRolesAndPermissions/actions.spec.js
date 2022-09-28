import * as actions from '../../../../modules/report/userRolesAndPermissions/actions';
import { apiService } from '../../../../common/services/apiService';
import { constants } from '../../../../common/services/constants';
import { dateService } from '../../../../common/services/dateService';

describe('Actions for UserRoleAndPermissions report', () => {
  it('should call BUILD_USER_ROLE_PERMISSION_REPORT_FILTER action', () => {
    const execution = {
      executionId: 'executionId1',
      name: constants.ReportType.UserGroupAccess
    };

    const action = actions.buildUserRolePermissionReportFilter(execution);
    expect(action.type).toEqual(actions.BUILD_USER_ROLE_PERMISSION_REPORT_FILTER);
    expect(action.execution).toEqual(execution);
  });

  it('should call REQUEST_USER_ROLE_PERMISSION_REPORT action', () => {
    const body = {
      initialDate: dateService.parse('2021-01-14T00:00:00Z'),
      finalDate: dateService.parse('2021-01-14T00:00:00Z'),
      reportType: constants.Report.UserGroupAssignmentReport
    };

    const action = actions.requestUserRolePermissionReport(body);
    expect(action.type).toEqual(actions.REQUEST_USER_ROLE_PERMISSION_REPORT);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.reports.requestUserRolePermissionReport());
    expect(action.fetchConfig.success).toBeDefined();
    expect(action.fetchConfig.failure).toBeDefined();

    const receiveAction = action.fetchConfig.success();
    expect(receiveAction.type).toEqual(actions.RECEIVE_USER_ROLE_PERMISSION_REPORT);

    const failureAction = action.fetchConfig.failure();
    expect(failureAction.type).toEqual(actions.FAILED_USER_ROLE_PERMISSION_REPORT);
  });
});
