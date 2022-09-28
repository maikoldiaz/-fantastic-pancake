import moment from 'moment-timezone';
import { resourceProvider } from './resourceProvider.js';
import es from 'date-fns/locale/es';
import { registerLocale, setDefaultLocale } from 'react-datepicker';
import { utilities } from './utilities.js';
import { httpService } from './httpService.js';

const formatParser = {
    Hour12: 'hh:mm:ss A',
    Hour24: 'HH:mm:ss'
};

const dateService = (function () {
    let dateFormat = null;
    let timeFormat = null;
    let dateTimeFormat = null;

    function formatDate(value, format = dateFormat) {
        if (utilities.isNullOrWhitespace(value)) {
            return '';
        }

        return moment.isMoment(value) ? value.format(format) : moment.utc(value).format(format);
    }

    function add(momentObject, amount, datePart) {
        return momentObject.add(amount, datePart);
    }

    function subtract(momentObject, amount, datePart) {
        return momentObject.subtract(amount, datePart);
    }

    function parse(value, format) {
        if (utilities.isNullOrWhitespace(value)) {
            return null;
        }

        return format ? moment.utc(value, format) : moment.utc(value);
    }

    return {
        initialize: () => {
            dateFormat = 'DD-MMM-YY';
            timeFormat = formatParser.Hour24;
            dateTimeFormat = `${dateFormat} ${timeFormat}`;
            moment.locale(httpService.getCurrentCulture());
            registerLocale('es', es);
            setDefaultLocale('es');
        },
        format: function (value, format = dateFormat) {
            return formatDate(value, format);
        },
        formatFromDate: function (value, format = dateFormat) {
            return moment(value).format(format);
        },
        capitalize: function (value, format = dateFormat) {
            const date = formatDate(value, format);
            const dateParts = date.split('-');
            dateParts[1] = utilities.toPascalCase(dateParts[1]);
            if (dateParts.length <= 2) {
                return `${dateParts[0]}-${dateParts[1]}`;
            }
            return `${dateParts[0]}-${dateParts[1]}-${dateParts[2]}`;
        },
        isMinDate: function (value) {
            return value === formatDate('01-Jan-01', dateFormat);
        },
        isValid: function (value, format) {
            if (!value) {
                return false;
            }
            return moment(value, format || dateTimeFormat, true).isValid();
        },
        isValidDate: function (value, format) {
            if (!value) {
                return false;
            }
            return moment(value, format || dateFormat, true).isValid();
        },
        valueOf: function () {
            return moment().valueOf();
        },
        now: function () {
            return moment();
        },
        nowAsString: function () {
            return moment().format(dateFormat);
        },
        today: function () {
            return moment.utc().startOf('day').toDate();
        },
        add: (momentObject, amount, datePart) => {
            return add(momentObject, amount, datePart);
        },
        subtract: (momentObject, amount, datePart) => {
            return subtract(momentObject, amount, datePart);
        },
        getDiff: (to, from, datePart, format = dateFormat) => {
            return parse(to, format).diff(parse(from, format), datePart);
        },
        toISOString: function (momentObject) {
            return moment.isMoment(momentObject) ? momentObject.toISOString() : '';
        },
        parseToISOString: function (value, format = dateFormat) {
            const momentObject = parse(value, format);
            return momentObject.toISOString();
        },
        parseFieldToISOString: function (value, format = dateFormat) {
            const valueString = typeof value === 'string' ? value : moment(value).format(format);
            const momentObject = parse(valueString, format);
            return momentObject.toISOString();
        },
        parseToPBIString: function (value, format = dateFormat) {
            const momentObject = parse(value, format);
            return momentObject.format('YYYY-MM-DD HH:mm:ss');
        },
        parseToPBIDateString: function (value, format = dateFormat) {
            const momentObject = parse(value, format);
            return momentObject.format('YYYY-MM-DD');
        },
        parseToDate: function (value, format = dateFormat) {
            const valueString = typeof value === 'string' ? value : moment(value).format(format);
            const momentObject = moment(valueString, format);
            return momentObject.toDate();
        },
        getDefaultFormat: function () {
            return dateFormat;
        },
        parse: function (value, format) {
            return parse(value, format);
        },
        toDate: function (values) {
            return moment.utc(values).toDate();
        },
        parseToFilterString: function (amount) {
            return this.parseToISOString(moment().add(amount, 'd').format(dateFormat), dateFormat);
        },
        startOf: function (startOf) {
            return moment().startOf(startOf);
        },
        compare: function (comparedValue, referenceValue, compareDateOnly) {
            const comparedDateTime = moment.isMoment(comparedValue) ? comparedValue : moment.utc(comparedValue);
            const referenceDateTime = moment.isMoment(referenceValue) ? referenceValue : moment.utc(referenceValue);
            if (moment.isMoment(comparedDateTime) && moment.isMoment(referenceDateTime)) {
                if (compareDateOnly) {
                    if (comparedDateTime.startOf('days').isSame(referenceDateTime.startOf('days'))) {
                        return 0;
                    }

                    return comparedDateTime.startOf('days').isAfter(referenceDateTime.startOf('days')) ? 1 : -1;
                }

                if (comparedDateTime.isSame(referenceDateTime)) {
                    return 0;
                }

                return comparedDateTime.isAfter(referenceDateTime) ? 1 : -1;
            }
            return null;
        },
        wishMe: () => {
            const today = new Date();
            const curHr = today.getHours();

            if (curHr < 12) {
                return resourceProvider.read('morning');
            } else if (curHr < 18) {
                return resourceProvider.read('afternoon');
            }

            return resourceProvider.read('evening');
        },
        getMonthRange: (dateRange, year) => {
            const monthRange = [];
            if (utilities.isNullOrUndefined(year)) {
                return monthRange;
            }
            const validMonths = dateRange[year];
            if (utilities.isNullOrUndefined(validMonths)) {
                return monthRange;
            }
            validMonths.forEach(v => {
                monthRange.push({
                    start: v.startDay ? dateService.parseToDate(moment.utc([year, (v.month - 1), v.startDay])) : dateService.parseToDate(moment.utc([year, (v.month - 1)]).startOf('month')),
                    end: v.endDay ? dateService.parseToDate(moment.utc([year, (v.month - 1), v.endDay])) : dateService.parseToDate(moment.utc([year, (v.month - 1)]).endOf('month')),
                    isDefault: v.isDefault
                });
            });
            return monthRange;
        },
        convertToColombian: value => {
            return subtract(parse(value), 5, 'hours');
        }
    };
}());

export {
    dateService
};
