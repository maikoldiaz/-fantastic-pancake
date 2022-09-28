import { dateService } from './dateService.js';
import { constants } from './constants.js';
import { resourceProvider } from './resourceProvider.js';
import { createFilter } from 'react-select';

const utilities = (function () {
    function hasProperty(obj, prop) {
        return obj && Object.prototype.hasOwnProperty.call(obj, prop);
    }

    function deepEqual(o, p) {
        let i;
        const keysO = Object.keys(o).sort();
        const keysP = Object.keys(p).sort();
        if (keysO.length !== keysP.length) {
            // not the same nr of keys
            return false;
        }
        if (keysO.join('') !== keysP.join('')) {
            // different keys
            return false;
        }
        for (i = 0; i < keysO.length; ++i) {
            if (o[keysO[i]] instanceof Array) {
                if (!(p[keysO[i]] instanceof Array)) {
                    return false;
                }
                if (p[keysO[i]].sort().join('') !== o[keysO[i]].sort().join('')) {
                    return false;
                }
            } else if (o[keysO[i]] instanceof Date) {
                if (!(p[keysO[i]] instanceof Date)) {
                    return false;
                }
                if (('' + o[keysO[i]]) !== ('' + p[keysO[i]])) {
                    return false;
                }
            } else if (o[keysO[i]] instanceof Function) {
                if (!(p[keysO[i]] instanceof Function)) {
                    return false;
                }
            } else if (o[keysO[i]] instanceof Object) {
                if (!(p[keysO[i]] instanceof Object)) {
                    return false;
                }
                if (o[keysO[i]] === o) {
                    if (p[keysO[i]] !== p) {
                        return false;
                    }
                } else if (deepEqual(o[keysO[i]], p[keysO[i]]) === false) {
                    return false;
                }
            }
            if (o[keysO[i]] !== p[keysO[i]]) {
                // not the same value
                return false;
            }
        }
        return true;
    }

    function isEqual(x, y) {
        if (x instanceof Array && y instanceof Array) {
            if (x.length !== y.length) {
                return false;
            }
            for (let i = 0; i < x.length; i++) {
                if (!y.includes(x[i])) {
                    return false;
                }
            }
            return true;
        }
        return x === y;
    }

    function areEqualArrays(x, y, field) {
        if (x instanceof Array && y instanceof Array) {
            if (x.length !== y.length) {
                return false;
            }
            for (let i = 0; i < x.length; i++) {
                if (!y.find(z => z[field] === x[i][field])) {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    function countValidProps(obj) {
        let count = 0;
        for (const prop in obj) {
            if (!hasProperty(obj, prop)) {
                continue;
            }

            const propValue = obj[prop];
            if (Array.isArray(propValue)) {
                if (propValue.length > 0) {
                    count = count + 1;
                }
            } else if (propValue) {
                count = count + 1;
            }
        }

        return count;
    }

    function loadScript(url, attributes, parent, callback) {
        const script = document.createElement('script');

        if (attributes) {
            Object.keys(attributes).forEach(item => {
                script.setAttribute(item, attributes[item]);
            });
        }

        if (callback) {
            // only required for IE <9
            if (script.readyState) {
                script.onreadystatechange = () => {
                    if (script.readyState === 'loaded' || script.readyState === 'complete') {
                        script.onreadystatechange = null;
                        callback();
                    }
                };
            } else {
                script.onload = () => {
                    callback();
                };
            }
        }

        script.src = url;
        document.getElementsByTagName(parent)[0].appendChild(script);
    }

    function normalize(objectArray, id) {
        const normalized = {};
        objectArray.forEach(v => {
            const key = typeof id === 'function' ? id(v) : v[id];
            normalized[key] = v;
        });
        return normalized;
    }

    function denormalize(normalizedObject) {
        const keys = Object.keys(normalizedObject);
        const objectArray = [];
        keys.forEach(v => {
            objectArray.push(normalizedObject[v]);
        });
        return objectArray;
    }

    function getValue(obj, prop, defaultValue) {
        let value = obj;

        if (prop.indexOf('.') < 0) {
            return hasProperty(obj, prop) ? obj[prop] : defaultValue;
        }

        prop.split('.').forEach(p => {
            if (value) {
                value = value[p];
            }
        });

        return value ? value : defaultValue;
    }

    function toCsv(headers, rows) {
        const headersString = headers.join(',');
        let str = '';
        for (const row of rows) {
            let line = '';
            for (const header of headers) {
                if (hasProperty(row, header)) {
                    if (line !== '') {
                        line = line + ',';
                    }

                    line = line + (row[header] ? row[header].toString().replace(/,/g, ';') : row[header]);
                }
            }
            str = str + (line.replace(/null/g, '') + '\r\n');
        }
        return headersString + '\n' + str;
    }

    function formatBytes(bytes, decimals) {
        if (bytes === 0) {
            return '0 Bytes';
        }
        const k = 1024;
        const dm = decimals <= 0 ? 0 : decimals || 2;
        const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
    }

    function groupBy(list, field) {
        const map = new Map();
        const keyGetter = x => x[field];
        list.forEach(item => {
            const key = keyGetter(item);
            if (!map.has(key)) {
                map.set(key, [item]);
            } else {
                map.get(key).push(item);
            }
        });
        return map;
    }

    function normalizedGroupBy(list, field) {
        const grouped = groupBy(list, field);

        const normalized = {};
        grouped.forEach((v, k) => {
            normalized[k] = v;
        });
        return normalized;
    }

    function calculatePartition(value, maxPartition, minPartition) {
        let partitionCount = maxPartition;
        let minValue = maxPartition;
        for (let i = maxPartition; i > minPartition; i--) {
            const mod = value % i;
            if (mod === 0) {
                partitionCount = i;
                break;
            }
            if (mod < minValue) {
                partitionCount = i;
                minValue = mod;
            }
        }
        return partitionCount;
    }

    function isChrome() {
        const isChromium = window.chrome;
        const winNav = window.navigator;
        const vendorName = winNav.vendor;
        const isOpera = typeof window.opr !== 'undefined';
        const isIEdge = winNav.userAgent.indexOf('Edge') > -1;
        const isIOSChrome = winNav.userAgent.match('CriOS');

        if (isIOSChrome) {
            return false;
        } else if (
            isChromium !== null &&
            typeof isChromium !== 'undefined' &&
            vendorName === 'Google Inc.' &&
            isOpera === false &&
            isIEdge === false
        ) {
            return true;
        }

        return false;
    }

    function isDefault(value, type) {
        return type === constants.Types.GUID ? value === '00000000-0000-0000-0000-000000000000' : false;
    }

    function getChartLabels(type, value) {
        const axisLabels = {
            padding: 3,
            font: '12px Roboto'
        };

        switch (type) {
        case constants.Types.DATETIME:
            axisLabels.content = x => dateService.format(x.value, 'D MMM HH:MM');
            break;
        case 'title':
            axisLabels.text = resourceProvider.read(value);
            delete axisLabels.padding;
            break;
        case constants.Types.DATE:
            axisLabels.content = x => dateService.format(x.value, 'DD MMM');
            break;
        default:
            break;
        }

        return axisLabels;
    }

    function changeState(state, property, value) {
        return Object.assign({}, state, {
            [property]: value
        });
    }

    function validateMissingValuesInArray(x, y) {
        let notPresentInData = [];
        if (x instanceof Array && y instanceof Array) {
            notPresentInData = y.filter(val => !x.includes(val));
        }
        return notPresentInData;
    }

    function getNavigatorLanguage() {
        if (navigator.languages && navigator.languages.length) {
            return navigator.languages[0];
        }
        return navigator.userLanguage || navigator.language || navigator.browserLanguage || navigator.systemLanguage || 'es';
    }

    function getCircularReplacer() {
        const seen = new WeakSet();
        return (key, value) => {
            if (typeof value === 'object' && value !== null) {
                if (seen.has(value)) {
                    return;
                }
                seen.add(value);
            }
            // eslint-disable-next-line consistent-return
            return value;
        };
    }

    function compare(a, b, field) {
        let first = a[field];
        let second = b[field];

        if (typeof first === 'string' && typeof second === 'string') {
            first = first.toLowerCase();
            second = second.toLowerCase();
        }

        return first > second ? 1 : -1;
    }

    function sortBy(list, field) {
        return list.sort((a, b) => compare(a, b, field));
    }

    return {
        isDeepEqual: function (value1, value2) {
            return deepEqual(value1, value2);
        },
        isNotEmpty: function (obj) {
            return obj && Object.keys(obj).length > 0;
        },
        isEqual: function (value1, value2) {
            return isEqual(value1, value2);
        },
        getPropsCount: function (obj) {
            return countValidProps(obj);
        },
        loadScript: function (url, attributes, parent, callback) {
            return loadScript(url, attributes, parent, callback);
        },
        getSessionId: function () {
            return dateService.valueOf();
        },
        normalize: function (objectArray, id) {
            return normalize(objectArray, id);
        },
        denormalize: function (normalizedObject) {
            return denormalize(normalizedObject);
        },
        getValue: function (obj, prop, defaultValue) {
            return getValue(obj, prop, defaultValue);
        },
        findExcept: function (haystack, arr) {
            return arr.filter(v => haystack.indexOf(v) < 0);
        },
        areEqualArrays: function (x, y, field) {
            return areEqualArrays(x, y, field);
        },
        isUndefined: function (val) {
            return typeof val === 'undefined';
        },
        isNullOrUndefined: function (val) {
            return typeof val === 'undefined' || val === null;
        },
        isNullOrWhitespace: function (val) {
            return typeof val === 'undefined' || val === null || (typeof val === 'string' && val.trim() === '');
        },
        contains: function (arr, prop, value) {
            if (!arr || arr.length === 0) {
                return false;
            }
            return arr.findIndex(x => prop ? x[prop] === value : x === value) > -1;
        },
        find: function (arr, prop, value) {
            if (!arr || arr.length === 0) {
                return null;
            }

            const index = arr.findIndex(x => prop ? x[prop] === value : x === value);
            return index > -1 ? arr[index] : null;
        },
        isFunction: function (val) {
            return val && {}.toString.call(val) === '[object Function]';
        },
        getArrayOrDefault: function (arr) {
            return arr && arr.length > 0 ? arr : [];
        },
        pluralize: function (str, suffix = 's') {
            if (str.charAt(str.length - 1) === 'y') {
                return `${str.substring(0, str.length - 1)}ies`;
            }
            return `${str}${suffix}`;
        },
        findOne: function (haystack, arr) {
            return arr.some(v => haystack.indexOf(v) >= 0);
        },
        findEvery: function (haystack, arr) {
            return arr.every(v => haystack.indexOf(v) >= 0);
        },
        checkIfBoolean: function (val) {
            return typeof val === 'boolean';
        },
        checkIfDateTime: function (value) {
            return value !== null && typeof value !== 'undefined' && !Array.isArray(value) && dateService.isValid(value);
        },
        checkIfNumber: function (value) {
            return value !== null && typeof value !== 'undefined' && isFinite(String(value).trim() || NaN);
        },
        filterArrayElements: function (inputElements, elementsToRemove) {
            if (!inputElements || !elementsToRemove) {
                return inputElements;
            }
            return inputElements.filter(t => elementsToRemove.findIndex(s => s.toUpperCase() === t.toUpperCase()) < 0);
        },
        distinct: function (input) {
            return [...new Set(input.map(t => t))];
        },
        toTitleCase: function (str) {
            if (str) {
                if (str.length > 1) {
                    return str[0].toUpperCase() + str.substr(1);
                } else if (str.length === 1) {
                    return str[0].toUpperCase();
                }
            }
            return str;
        },
        toSentenceCase: function (str) {
            if (str) {
                if (str.length > 1) {
                    return str[0].toUpperCase() + str.substr(1).toLowerCase();
                } else if (str.length === 1) {
                    return str[0].toUpperCase();
                }
            }
            return str;
        },
        toCamelCase: function (str) {
            if (str) {
                if (str.length > 1) {
                    return str[0].toLowerCase() + str.substr(1);
                } else if (str.length === 1) {
                    return str[0].toLowerCase();
                }
            }
            return str;
        },
        toLowerCase: function (value, props) {
            if (value === null) {
                return value;
            }
            if (typeof value === 'object') {
                props.forEach(x => {
                    if (typeof value[x] === 'string') {
                        value[x] = value[x].toLowerCase();
                    }
                });
            } else if (typeof value === 'string') {
                return value.toLowerCase();
            }

            return value;
        },
        toUpperCase: function (str) {
            if (typeof str === 'string') {
                return str.toUpperCase();
            }
            return str;
        },
        toPascalCase: function (str) {
            if (typeof str === 'string' && str) {
                if (str.length > 1) {
                    const val = str.toLowerCase();
                    return val[0].toUpperCase() + val.substr(1);
                } else if (str.length === 1) {
                    return str[0].toUpperCase();
                }
            }
            return str;
        },
        toCsv: function (headers, rows) {
            return toCsv(headers, rows);
        },
        groupBy: function (list, field) {
            return groupBy(list, field);
        },
        normalizedGroupBy: function (list, field) {
            return normalizedGroupBy(list, field);
        },
        formatBytes: function (bytes, decimals) {
            return formatBytes(bytes, decimals);
        },
        equalsIgnoreCase: function (first, second) {
            // null equals null, undefined equals undefined
            if (!first && !second) {
                return true;
            }
            return first && second && first.toLowerCase() === second.toLowerCase();
        },
        calculatePartition: function (value, maxPartition, minPartition) {
            return calculatePartition(value, maxPartition, minPartition);
        },
        sanitizeCell: function (value, defaultValue = '') {
            if (typeof value === 'undefined' || value === null || (typeof value === 'string' && value.trim() === '')) {
                return defaultValue;
            }

            const ignoreChars = ['@', '+', '-', '='];
            const firstChar = value.toString().substring(0, 1);

            return ignoreChars.some(x => x === firstChar) ? `'${value}` : value;
        },
        getValueOrDefault: function (value, defaultValue = '') {
            return typeof value === 'undefined' || value === null ? defaultValue : value;
        },
        getStringOrDefault: function (value, defaultValue = '') {
            return this.isNullOrWhitespace(value) ? defaultValue : value;
        },
        removeSpaces: str => {
            return str.replace(/\s+/g, '');
        },
        normalizeBoolean: function (value) {
            if (value === 'true') {
                return true;
            }
            if (value === 'false') {
                return false;
            }

            return value;
        },
        getExtension: function (filename) {
            const parts = filename.split('.');
            return parts[parts.length - 1];
        },
        isChrome: function () {
            return isChrome();
        },
        isDefault: function (value, type) {
            return isDefault(value, type);
        },
        getChartAxisLabels: function (type) {
            return getChartLabels(type);
        },
        getChartTitle: function (value) {
            return getChartLabels('title', value);
        },
        hasProperty: function (obj, property) {
            return hasProperty(obj, property);
        },
        changeState: function (state, property, value) {
            return changeState(state, property, value);
        },
        validateMissingValuesInArray: function (array1, array2) {
            return validateMissingValuesInArray(array1, array2);
        },
        isNumber: function (value) {
            return !isNaN(value);
        },
        replace: function (str, find, replace) {
            return str.split(find).join(replace);
        },
        isIE: function () {
            return navigator.userAgent.indexOf('MSIE ') > -1 || navigator.userAgent.indexOf('Trident/') > -1;
        },
        generateRandomNumber() {
            return window.crypto.getRandomValues(new Uint8Array(1))[0] / 255;
        },
        getRandomNumber: function () {
            return 100000 + Math.floor(this.generateRandomNumber() * 900000);
        },
        getLanguage: function () {
            return getNavigatorLanguage();
        },
        buildCenterStorageLocationName: function (center, storageLocation) {
            return center + ':' + storageLocation;
        },
        buildDocumentNumberPosition: function (documentNumber, position) {
            return documentNumber + ' - ' + position;
        },
        merge: (stateProps, dispatchProps, ownProps) => {
            return Object.assign({}, stateProps, dispatchProps, ownProps);
        },
        shouldAsyncValidate: params => {
            if (!params.syncValidationPasses) {
                return false;
            }

            // validate only on submit
            switch (params.trigger) {
            case 'blur':
                return false;
            case 'submit':
                return true;
            default:
                return false;
            }
        },
        matchesAny: (value, values) => {
            return values.some(x => x === value);
        },
        isArrayNotEmpty: input => {
            return input.every(a => !utilities.isNullOrUndefined(a));
        },
        isArray: value => {
            return Array.isArray(value);
        },
        parseFloat: value => {
            return value ? parseFloat(value).toFixed(constants.DecimalPlaces) : '0.00';
        },
        stringifyJson: value => {
            try {
                return JSON.stringify(value, getCircularReplacer());
            } catch (error) {
                return error;
            }
        },
        sortBy: (list, field) => {
            return sortBy(list, field);
        },
        selectFilter: () => {
            return createFilter({ matchFrom: 'any', stringify: option => `${option.label}` });
        },
        base64Encode(obj) {
            return encodeURIComponent(btoa(JSON.stringify(obj)));
        },
        base64Decode(str) {
            return atob(decodeURIComponent(str));
        },
        getErrorMessageFromApiResponse(response, defaultMessage = '') {
            return response && response.errorCodes && response.errorCodes.length > 0
                ? response.errorCodes[0].message
                : defaultMessage;
        }
    };
}());

export { utilities };
