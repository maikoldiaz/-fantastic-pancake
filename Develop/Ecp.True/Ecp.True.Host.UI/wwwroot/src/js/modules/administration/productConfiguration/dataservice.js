import { constants } from '../../../common/services/constants';
import { resourceProvider } from './../../../common/services/resourceProvider';

const dataService = (function () {
    return {
        buildProductObject: (values, mode, initialValues) => {
            return {
                productId: values.product.productSapId,
                name: values.product.productSapName,
                isActive: mode === constants.Modes.Update ? values.product.isActive : true,
                rowVersion: mode === constants.Modes.Update ? initialValues.product.rowVersion : null
            };
        },
        buildInitialValues: row => {
            const result = {
                product: {
                    productSapId: row.productId,
                    productSapName: row.name,
                    isActive: row.isActive,
                    rowVersion: row.rowVersion
                }
            };

            return result;
        },
        buildResetValues: () => {
            const result = {
                product: {
                    productSapId: undefined,
                    productSapName: undefined,
                    isActive: undefined,
                    rowVersion: undefined
                }
            };
            return result;
        },
        validationsForRequiredFieldArrayItems: (values, { fieldName, requiredItems }) => {
            const errors = {};
            const getRequiredItems = (item = {}) => {
                const result = {};
                requiredItems.forEach(element => {
                    const exists = item[element];
                    if (!exists) {
                        result[element] = resourceProvider.read('required');
                    }
                });
                return result;
            };
            const fieldNameVal = values[fieldName];
            if (!fieldNameVal) {
                errors[fieldName] = [getRequiredItems()];
            } else {
                const result = [];
                values[fieldName].forEach(v => {
                    result.push(getRequiredItems(v));
                });
                errors[fieldName] = result;
            }
            return errors;
        }
    };
}());

export { dataService };
