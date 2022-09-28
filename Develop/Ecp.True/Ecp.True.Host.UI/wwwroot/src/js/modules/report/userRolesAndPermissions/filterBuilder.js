import { pbiFilterBuilder } from '../../../common/services/pbiFilterBuilder';
import { constants } from '../../../common/services/constants';

const userRolesAndPermissionsFilterBuilder = (function () {
    const filters = [];

    function buildExecutionIdFilter(tableName, executionId) {
        filters.push(pbiFilterBuilder.buildBasicPbiFilter(tableName, 'ExecutionId', 'In', null, [executionId]));
    }

    return {
        build: function (values) {
            filters.length = 0;

            if (values.reportTypeName === constants.ReportTypeName.UserGroupAccessReport) {
                buildExecutionIdFilter('FeatureRoleReport', values.executionId);
            } else {
                buildExecutionIdFilter('UserRoleReport', values.executionId);
            }

            return filters;
        }
    };
}());

export { userRolesAndPermissionsFilterBuilder };
