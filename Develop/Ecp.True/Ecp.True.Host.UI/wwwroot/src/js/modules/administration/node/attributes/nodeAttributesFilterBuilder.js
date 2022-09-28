import { categoryElementFilterBuilder } from '../../../../common/components/filters/categoryElementFilterBuilder.js';

const nodeAttributesFilterBuilder = (function () {
    return {
        build: function (values) {
            const queryPrefix = 'NodeTags/any(x:x/CategoryElement/ElementId eq';
            return categoryElementFilterBuilder.build(values, queryPrefix);
        }
    };
}());

export { nodeAttributesFilterBuilder };
