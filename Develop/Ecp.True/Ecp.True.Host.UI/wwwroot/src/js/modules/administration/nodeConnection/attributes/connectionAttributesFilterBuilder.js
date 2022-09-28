import { categoryElementFilterBuilder } from '../../../../common/components/filters/categoryElementFilterBuilder.js';

const connectionAttributesFilterBuilder = (function () {
    return {
        build: function (values) {
            const queryPrefix = 'SourceNode/NodeTags/any(x:x/CategoryElement/ElementId eq';
            return categoryElementFilterBuilder.build(values, queryPrefix);
        }
    };
}());

export { connectionAttributesFilterBuilder };
