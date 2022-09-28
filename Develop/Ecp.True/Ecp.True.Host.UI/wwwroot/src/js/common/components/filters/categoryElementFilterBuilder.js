import { utilities } from '../../services/utilities';

const categoryElementFilterBuilder = (function () {
    function getType(isOr) {
        return isOr ? 'or' : 'and';
    }

    return {
        build: function (values, queryPrefix) {
            if (utilities.isNullOrUndefined(values.categoryElements) || values.categoryElements.filter(x => !utilities.isNullOrUndefined(x.element)).length === 0) {
                return '';
            }
            let query;
            const categoryElements = values.categoryElements;
            if (categoryElements.length === 3) {
                query = `${queryPrefix} ${categoryElements[0].element.elementId})`;
                query = `${query} ${getType(categoryElements[1].or)} (${queryPrefix} ${categoryElements[1].element.elementId})`;
                query = `${query} ${getType(categoryElements[2].or)} ${queryPrefix} ${categoryElements[2].element.elementId}))`;
            } else if (categoryElements.length === 2) {
                query = `${queryPrefix} ${categoryElements[0].element.elementId})`;
                query = `${query} ${getType(categoryElements[1].or)} ${queryPrefix} ${categoryElements[1].element.elementId})`;
            } else {
                query = `${queryPrefix} ${categoryElements[0].element.elementId})`;
            }

            return query;
        }
    };
}());

export { categoryElementFilterBuilder };
