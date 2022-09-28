import { utilities } from '../../../common/services/utilities';

describe('api service', () => {
    it('hasProperty',
        () => {
            const obj = { key1: 'value1' };
            expect(utilities.hasProperty(obj, 'key1')).toEqual(true);
        });

    it('isDeepEqual- Should retrun true if tow objects are equal',
        () => {
            const arr = [{ a: 'a' }];
            const objShared = { a: 'b' };
            const func = new function () {
                return 1;
            }();
            const date = Date.now();
            const objA = { key0: arr, key1: date, key2: func, key3: objShared, key4: 4 };
            const objB = { key0: arr, key1: date, key2: func, key3: objShared, key4: 4 };

            expect(utilities.isDeepEqual(objA, objB)).toEqual(true);
        });


    it('isDeepEqual- Should retrun true if tow objects with different properties length', () => {
        const arr = [{ a: 'a' }];
        const objShared = { a: 'b' };
        const func = new function () {
            return 1;
        }();
        const date = Date.now();
        const objA = { key0: arr, key1: date, key2: func, key3: objShared, key4: 4 };
        const objB = { key0: arr, key1: date, key2: func, key3: objShared };

        expect(utilities.isDeepEqual(objA, objB)).toEqual(false);
    });

    it('isDeepEqual- Should retrun true if tow objects with different properties', () => {
        const arr = [{ a: 'a' }];
        const objShared = { a: 'b' };
        const func = new function () {
            return 1;
        }();
        const date = Date.now();
        const objA = { key0: arr, key1: date, key2: func, key3: objShared, key4: 4 };
        const objB = { key0: arr, key1: date, key2: func, key3: objShared, key5: 5 };

        expect(utilities.isDeepEqual(objA, objB)).toEqual(false);
    });

    it('isDeepEqual- Should retrun true if tow objects with different Array properties', () => {
        const arr = [1, 2, 3];
        const objShared = { a: 'b' };
        const func = new function () {
            return 1;
        }();
        const date = Date.now();
        const objA = { key0: arr, key1: date, key2: func, key3: objShared, key4: 4 };
        const objB = { key0: [1, 2, 3, 4], key1: date, key2: func, key3: objShared, key4: 4 };
        const objC = { key0: {}, key1: date, key2: func, key3: objShared, key4: 4 };

        expect(utilities.isDeepEqual(objA, objB)).toEqual(false);
        expect(utilities.isDeepEqual(objA, objC)).toEqual(false);
    });


    it('isDeepEqual- Should verify Date types', () => {
        const currentDate = new Date();
        const nextDate = new Date();
        nextDate.setDate(currentDate.getDate() + 29);
        const objA = { key1: currentDate };
        const objB = { key1: 'value' };
        const objC = { key1: nextDate };

        expect(utilities.isDeepEqual(objA, objB)).toEqual(false);
        expect(utilities.isDeepEqual(objA, objC)).toEqual(false);
    });

    it('isDeepEqual- Should verify Function types', () => {
        const func = function () {
            return 1;
        };
        const objA = { key1: func };
        const objB = { key1: 'value' };

        expect(utilities.isDeepEqual(objA, objB)).toEqual(false);
    });

    it('isDeepEqual- Should verify Object types', () => {
        const obj = { a: 'b' };
        const objA = { key1: obj };
        const objB = { key1: 'value' };
        const objC = { key1: { a: 'b', b: 'c' } };

        expect(utilities.isDeepEqual(objA, objB)).toEqual(false);
        expect(utilities.isDeepEqual(objA, objC)).toEqual(false);

        objA.key1 = objA;
        expect(utilities.isDeepEqual(objA, objC)).toEqual(false);
    });

    it('isEqual - Should retrun true if tow objects are equal', () => {
        expect(utilities.isEqual([1, 2], [1, 2, 3])).toEqual(false);
        expect(utilities.isEqual([1, 2, 3], [1, 2, 3])).toEqual(true);
        expect(utilities.isEqual([1, 2, 3], [1, 2, 4])).toEqual(false);
        expect(utilities.isEqual('a', 'a')).toEqual(true);
    });

    it('areEqualArrays - Should retrun true if tow objects are equal', () => {
        expect(utilities.areEqualArrays([1, 2, 3], 'value')).toEqual(false);
        expect(utilities.areEqualArrays([1, 2], [1, 2, 3])).toEqual(false);
        expect(utilities.areEqualArrays([1, 2, 3], [1, 2, 3])).toEqual(true);
    });

    it('getPropsCount - Should count valid properties', () => {
        const objA = { key1: 'Value1', Key2: [1, 2] };
        expect(utilities.getPropsCount(objA)).toEqual(2);
    });

    it('loadScript - Should Load script', () => {
        const attribute = { key1: 'value1' };
        const func = function () {
            return 1;
        };
        const error = () => {
            utilities.loadScript('', attribute, '', func);
        };

        // Inside method, document is accessed to append child, which is not available during test, so, just check the exception.
        expect(error).toThrow(TypeError);
    });

    it('normalize', () => {
        const arr = [{ key: 'value1' }, { key: 'value2' }];
        const normalized = { value1: { key: 'value1' }, value2: { key: 'value2' } };
        expect(utilities.normalize(arr, 'key')).toEqual(normalized);
    });

    it('denormalize', () => {
        const denormalizeObject = [{ key: 'value1' }, { key: 'value2' }];
        const normalized = { value1: { key: 'value1' }, value2: { key: 'value2' } };
        expect(utilities.denormalize(normalized)).toEqual(denormalizeObject);
    });

    it('toCsv', () => {
        const expected = 'header1,header2' + '\n' + 'row1,row11 ';
        const headers = ['header1', 'header2'];
        const rows = [{ header1: 'row1', header2: 'row11' }];
        expect(utilities.toCsv(headers, rows).trim()).toEqual(expected.trim());
    });

    it('groupBy', () => {
        const arr = [{ key: 'value1' }, { key: 'value2' }];
        const map = new Map();
        map.set('value1', [{ key: 'value1' }]);
        map.set('value2', [{ key: 'value2' }]);
        expect(utilities.groupBy(arr, 'key')).toEqual(map);
    });

    it('normalizedGroupBy', () => {
        const arr = [{ key: 'value1' }, { key: 'value2' }];
        const map = { value1: [{ key: 'value1' }], value2: [{ key: 'value2' }] };
        expect(utilities.normalizedGroupBy(arr, 'key')).toEqual(map);
    });

    it('calculatePartition', () => {
        expect(utilities.calculatePartition(100, 5, 5)).toEqual(5);
        expect(utilities.calculatePartition(100, 5, 4)).toEqual(5);
        expect(utilities.calculatePartition(100, 50, 6)).toEqual(50);
    });

    it('isChrome', () => {
        expect(utilities.isChrome()).toEqual(false);
    });

    it('isDefault', () => {
        expect(utilities.isDefault('00000000-0000-0000-0000-000000000000', 'guid')).toEqual(true);
        expect(utilities.isDefault(10, Number)).toEqual(false);
    });

    it('getChartTitle', () => {
        expect(utilities.getChartTitle('title')).toEqual({ font: '12px Roboto', text: 'title' });
        expect(utilities.getChartTitle('default')).toEqual({ font: '12px Roboto', text: 'default' });
    });

    it('getChartAxisLabels', () => {
        expect(utilities.getChartAxisLabels(Number)).toEqual({ font: '12px Roboto', padding: 3 });

        let labels = utilities.getChartAxisLabels('datetime');
        expect(labels.padding).toEqual(3);

        labels = utilities.getChartAxisLabels('date');
        expect(labels.padding).toEqual(3);
    });

    it('validateMissingValuesInArray', () => {
        expect(utilities.validateMissingValuesInArray([1, 2, 3, 4], [1, 2, 3, 7])).toEqual([7]);
    });

    it('getSessionId', () => {
        expect(utilities.getSessionId()).toBeGreaterThan(1000000000);
    });

    it('findExcept', () => {
        expect(utilities.findExcept('string', ['x'])).toEqual([ 'x' ]);
    });

    it('isUndefined', () => {
        const obj = {};
        expect(utilities.isUndefined(obj.value)).toEqual(true);
    });

    it('contains', () => {
        expect(utilities.contains([1, 2, 3], null, 1)).toEqual(true);
        expect(utilities.contains([], null, 1)).toEqual(false);
    });

    it('find', () => {
        expect(utilities.find([1, 2, 3], null, 1)).toEqual(1);
        expect(utilities.find([], null, 1)).toEqual(null);
    });

    it('getArrayOrDefault', () => {
        expect(utilities.getArrayOrDefault([1, 2, 3])).toEqual([1, 2, 3]);
        expect(utilities.getArrayOrDefault([])).toEqual([]);
    });

    it('pluralize', () => {
        expect(utilities.pluralize('string')).toEqual('strings');
        expect(utilities.pluralize('commodity')).toEqual('commodities');
    });

    it('findOne', () => {
        expect(utilities.findOne('string', ['s'])).toEqual(true);
    });

    it('findEvery', () => {
        expect(utilities.findEvery('strings', ['s'])).toEqual(true);
        expect(utilities.findEvery('strings', ['s', 'x'])).toEqual(false);
    });

    it('checkIfDateTime', () => {
        expect(utilities.checkIfDateTime(null)).toEqual(false);
        expect(utilities.checkIfDateTime(new Date())).toEqual(true);
    });

    it('checkIfNumber', () => {
        expect(utilities.checkIfNumber(null)).toEqual(false);
        expect(utilities.checkIfNumber(100)).toEqual(true);
    });

    it('filterArrayElements', () => {
        expect(utilities.filterArrayElements(['a', 'b'], ['a', 'c'])).toEqual(['b']);
        expect(utilities.filterArrayElements(null, null)).toEqual(null);
    });

    it('distinct', () => {
        expect(utilities.distinct([1, 2, 2, 3])).toEqual([1, 2, 3]);
        expect(utilities.distinct([1, 2, 3])).toEqual([1, 2, 3]);
    });

    it('toTitleCase', () => {
        expect(utilities.toTitleCase('test')).toEqual('Test');
        expect(utilities.toTitleCase(null)).toEqual(null);
        expect(utilities.toTitleCase('t')).toEqual('T');
    });

    it('toCamelCase', () => {
        expect(utilities.toCamelCase('test')).toEqual('test');
        expect(utilities.toCamelCase(null)).toEqual(null);
        expect(utilities.toCamelCase('t')).toEqual('t');
    });

    it('toLowerCase', () => {
        expect(utilities.toLowerCase('TEST')).toEqual('test');
        expect(utilities.toLowerCase({ key: 'VALUE' }, ['key'])).toEqual({ key: 'value' });
    });

    it('toUpperCase', () => {
        expect(utilities.toUpperCase('test')).toEqual('TEST');
        expect(utilities.toUpperCase(null)).toEqual(null);
    });

    it('toPascalCase', () => {
        expect(utilities.toPascalCase('test')).toEqual('Test');
        expect(utilities.toPascalCase('s')).toEqual('S');
        expect(utilities.toPascalCase(null)).toEqual(null);
    });

    it('sanitizeCell', () => {
        expect(utilities.sanitizeCell('@test@')).toEqual('\'@test@');
        expect(utilities.sanitizeCell(null, 'test')).toEqual('test');
    });

    it('getValueOrDefault', () => {
        expect(utilities.getValueOrDefault('test', 'default')).toEqual('test');
        expect(utilities.getValueOrDefault(null, 'default')).toEqual('default');
    });

    it('removeSpaces', () => {
        expect(utilities.removeSpaces('test ')).toEqual('test');
    });

    it('normalizeBoolean', () => {
        expect(utilities.normalizeBoolean('true')).toEqual(true);
        expect(utilities.normalizeBoolean('false')).toEqual(false);
        expect(utilities.normalizeBoolean(null)).toEqual(null);
    });

    it('getExtension', () =>{
        expect(utilities.getExtension('file.txt')).toEqual('txt');
    });

    it('isNumber', () =>{
        expect(utilities.isNumber(100)).toEqual(true);
        expect(utilities.isNumber('a')).toEqual(false);
    });

    it('isIE', () =>{
        expect(utilities.isIE()).toEqual(false);
    });

    it('formatBytes', () => {
        expect(utilities.formatBytes(0)).toEqual('0 Bytes');
        expect(utilities.formatBytes(1024)).toEqual('1 KB');
    });

    it('shouldAsyncValidate', () => {
        const validationFails = {
            syncValidationPasses: false
        };
        const submitParams = {
            syncValidationPasses: true,
            trigger: 'submit'
        };
        const blurParams = {
            syncValidationPasses: true,
            trigger: 'blur'
        };
        const unknownTrigger = {
            syncValidationPasses: true,
            trigger: 'test'
        };
        expect(utilities.shouldAsyncValidate(submitParams)).toEqual(true);
        expect(utilities.shouldAsyncValidate(blurParams)).toEqual(false);
        expect(utilities.shouldAsyncValidate(unknownTrigger)).toEqual(false);
        expect(utilities.shouldAsyncValidate(validationFails)).toEqual(false);
    });

    it('parseFloat', () => {
        expect(utilities.parseFloat(2.343444555)).toEqual('2.34');
    });

    it('Stringify JSON', () => {
        const obj = { name: 'NodeOne' };
        obj.Circular = obj;
        const result = utilities.stringifyJson(obj);
        expect(JSON.parse(result)).toEqual({ name: 'NodeOne' });
    });
    it('matchesAny', () => {
        expect(utilities.matchesAny(1, [1, 2, 3, 7])).toEqual(true);
    });
    it('isArrayNotEmpty', () => {
        expect(utilities.isArrayNotEmpty([1, 2, 3, 7])).toEqual(true);
        expect(utilities.isArrayNotEmpty([1, null, 3, 7])).toEqual(false);
    });
    it('select filter', () => {
        const filter = utilities.selectFilter();
        expect(filter).toEqual(expect.any(Function));
    });
    it('isArray', () => {
        expect(utilities.isArray([1, 2, 3, 7])).toEqual(true);
        expect(utilities.isArray(1)).toEqual(false);
    });
    it('parse float', () => {
        const value = utilities.parseFloat(3.1234);
        expect(value).toEqual('3.12');
    });
    it('sort list', () => {
        const value = utilities.sortBy([{ name: 'b' }, { name: 'a' }], 'name');
        expect(value).toEqual([{ name: 'a' }, { name: 'b' }]);
    });
    it('should return error message - getErrorMessageFromApiResponse', () => {
        const apiResponse = {
            errorCodes: [{
                message: 'api error'
            }]};
        const response = utilities.getErrorMessageFromApiResponse(apiResponse);
        expect(response).toEqual('api error');
    });
    it('should return default message - getErrorMessageFromApiResponse', () => {
        const response = utilities.getErrorMessageFromApiResponse(null, 'default error');
        expect(response).toEqual('default error');
    });
});
