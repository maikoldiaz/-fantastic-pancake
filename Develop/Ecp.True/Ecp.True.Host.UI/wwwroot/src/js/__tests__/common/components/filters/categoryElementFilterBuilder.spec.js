import { categoryElementFilterBuilder } from '../../../../common/components/filters/categoryElementFilterBuilder';

describe('categoryElementFilterBuilder', () =>{
    it('should return empty filter query if category element is empty', () =>{
        const values = {
            categoryElements: []
        };
        const actualFilterQuery = categoryElementFilterBuilder.build(values, '');
        expect(actualFilterQuery).toEqual('');
    });

    it('should handle if categoryelements length is 1 or greater than 3', () =>{
        const values = {
            categoryElements: [{ element: { elementId: 1 } }]
        };
        const actualFilterQuery = categoryElementFilterBuilder.build(values, 'prefix');

        expect(actualFilterQuery).toEqual('prefix 1)');
    });

    it('should handle categoryelements length == 3', () =>{
        const values = {
            categoryElements: [{ element: { elementId: 1 } }, { element: { elementId: 2 } }, { element: { elementId: 3 } }]
        };
        const actualFilterQuery = categoryElementFilterBuilder.build(values, 'prefix');

        expect(actualFilterQuery).toEqual('prefix 1) and (prefix 2) and prefix 3))');
    });

    it('should handle categoryelements length == 2', () =>{
        const values = {
            categoryElements: [{ element: { elementId: 1 } }, { element: { elementId: 2 } }]
        };
        const actualFilterQuery = categoryElementFilterBuilder.build(values, 'prefix');

        expect(actualFilterQuery).toEqual('prefix 1) and prefix 2)');
    });
});

