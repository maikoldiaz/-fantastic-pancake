import { userRolesAndPermissionsFilterBuilder } from '../../../../modules/report/userRolesAndPermissions/filterBuilder';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';

describe('UserRolesAndPermissions Report Filter Builder Validate', () => {
  beforeAll(() => {
    resourceProvider.read = jest.fn(key => key);
  });
  it('should return filter for UserGroupAccessReport', async () => {
    const values = {
      executionId: '1',
      reportType: constants.Report.UserGroupAccessReport,
      reportTypeName: constants.ReportTypeName.UserGroupAccessReport
    };

    const result = userRolesAndPermissionsFilterBuilder.build(values);

    await expect(result.length).toBe(1);
    await expect(result[0].target.table).toBe('FeatureRoleReport');
    await expect(result[0].target.column).toBe('ExecutionId');
  });
  it('should return filter for UserGroupAndAssignedUserAccessReport', async () => {
    const values = {
      executionId: '1',
      reportType: constants.Report.UserGroupAndAssignedUserAccessReport,
      reportTypeName: constants.ReportTypeName.UserGroupAndAssignedUserAccessReport
    };

    const result = userRolesAndPermissionsFilterBuilder.build(values);

    await expect(result.length).toBe(1);
    await expect(result[0].target.table).toBe('UserRoleReport');
    await expect(result[0].target.column).toBe('ExecutionId');
  });
});
