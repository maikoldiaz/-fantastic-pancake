import { utilities } from '../../../common/services/utilities.js';
import buildQuery from 'odata-query';
import { dateService } from '../../../common/services/dateService.js';
import { constants } from '../../../common/services/constants';

const fileUploadFilterBuilder = (function () {
    return {
        build: function (values) {
            let query = null;
            if (utilities.isNotEmpty(values)) {
                const filter = {};
                const dateFilter = {};

                if (!utilities.isNullOrUndefined(values.initialDate)) {
                    dateFilter.ge = dateService.parseToDate(values.initialDate);
                }

                if (!utilities.isNullOrUndefined(values.finalDate)) {
                    const finalDate = dateService.parseToISOString(values.finalDate);
                    dateFilter.le = dateService.add(dateService.parse(finalDate), 1, 'day').toDate();
                }

                filter.createdDate = dateFilter;

                if (!utilities.isNullOrUndefined(values.state)) {
                    filter.status = values.state.value;
                }

                if (!utilities.isNullOrUndefined(values.username)) {
                    filter.createdBy = values.username.name;
                }

                if (!utilities.isNullOrUndefined(values.action)) {
                    filter.actionType = values.action.value;
                }

                if (!utilities.isNullOrUndefined(values.fileType)) {
                    const systemTypeMapping = {};
                    Object.keys(constants.SystemType).forEach(x => {
                        systemTypeMapping[constants.SystemType[x]] = x;
                    });
                    filter.systemTypeId = systemTypeMapping[values.fileType.systemTypeId];
                }

                if (Object.keys(filter).length > 0) {
                    query = buildQuery({ filter }).replace('?$filter=', '');
                } else {
                    query = '';
                }
            }

            return query;
        }
    };
}());

export { fileUploadFilterBuilder };
