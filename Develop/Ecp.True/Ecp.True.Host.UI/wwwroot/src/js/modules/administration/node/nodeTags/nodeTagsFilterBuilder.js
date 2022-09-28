import { categoryElementFilterBuilder } from '../../../../common/components/filters/categoryElementFilterBuilder.js';
import { utilities } from '../../../../common/services/utilities.js';

const nodeTagsFilterBuilder = (function () {
    return {
        build: function (values) {
            const queryPrefix = 'Node/NodeTags/any(x:x/CategoryElement/ElementId eq';
            let query = categoryElementFilterBuilder.build(values, queryPrefix);
            if (utilities.isNullOrWhitespace(query)) {
                return '';
            }
            let suffixQuery = null;
            values.categoryElements.forEach(x => {
                suffixQuery = suffixQuery ? `${suffixQuery} or CategoryElement/ElementId eq ${x.element.elementId}` : `CategoryElement/ElementId eq ${x.element.elementId}`;
            });

            query = `(${query}) and (${suffixQuery})`;
            return query;
        }
    };
}());

export { nodeTagsFilterBuilder };
