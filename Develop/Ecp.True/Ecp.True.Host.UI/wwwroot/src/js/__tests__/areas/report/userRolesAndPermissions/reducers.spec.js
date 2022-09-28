import * as actions from '../../../../modules/report/userRolesAndPermissions/actions';
import { userRolesAndPermissions } from '../../../../modules/report/userRolesAndPermissions/reducers';
import { constants } from '../../../../common/services/constants';

describe('Reducers for UserRolesAndPermissions report', () => {
    it('should return status unchanged', () => {
        expect(userRolesAndPermissions()).toEqual({});
    });

    it('should handle action SAVE_USER_ROLE_PERMISSION_REPORT_FILTER', () => {
        const action = {
            type: actions.BUILD_USER_ROLE_PERMISSION_REPORT_FILTER,
            execution: {
                executionId: 'executionId1',
                name: constants.ReportTypeName.UserGroupAccessBalance
            }
        };
        const newState = {
            filters: {
                executionId: 'executionId1',
                reportType: constants.Report.UserGroupAccess,
                ReportTypeName: constants.ReportTypeName.UserGroupAccessBalance
            }
        };
        expect(userRolesAndPermissions({}, action)).toEqual(newState);
    });

    it('should handle action RECEIVE_USER_ROLE_PERMISSION_REPORT', () => {
        const action = {
            type: actions.RECEIVE_USER_ROLE_PERMISSION_REPORT
        };
        const newState = {
            receiveReportToggler: true
        };

        expect(userRolesAndPermissions({}, action)).toEqual(newState);
    });

    it('should handle action FAILED_USER_ROLE_PERMISSION_REPORT', () => {
        const action = {
            type: actions.FAILED_USER_ROLE_PERMISSION_REPORT
        };
        const newState = {
            failureReportToggler: true
        };

        expect(userRolesAndPermissions({}, action)).toEqual(newState);
    });
});

