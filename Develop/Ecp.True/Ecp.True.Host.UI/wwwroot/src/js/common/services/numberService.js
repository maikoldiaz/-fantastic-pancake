import { utilities } from '../../common/services/utilities.js';
import { httpService } from '../../common/services/httpService.js';
import BigNumber from 'bignumber.js';

const numberService = (function () {
    return {

        getGroupingSymbol: function () {
            const culture = httpService.getQueryString('culture');
            const value = utilities.equalsIgnoreCase(culture, 'en') ? ',' : '.';
            return value;
        },

        getDecimalSymbol: function () {
            const culture = httpService.getQueryString('culture');
            const value = (utilities.equalsIgnoreCase(culture, 'en')) ? '.' : ',';
            return value;
        },

        createBigDecimal: function (input) {
            return BigNumber(input);
        },

        createBigDecimalString: function (input) {
            return BigNumber(input).toString();
        },

        maxOfBigNumber: function (max, input) {
            return BigNumber.maximum(max, input).toString();
        },

        minOfBigNumber: function (min, input) {
            return BigNumber.minimum(min, input).toString();
        },

        checkIfDecimalInRange: function (maxval, minval, input) {
            const x = numberService.maxOfBigNumber(minval, input.value);
            const y = numberService.minOfBigNumber(maxval, input.value);
            const max = (numberService.createBigDecimalString(x) === numberService.createBigDecimalString(input.value)) ? true : false;
            const min = (numberService.createBigDecimalString(y) === numberService.createBigDecimalString(input.value)) ? true : false;

            return max && min;
        },

        changeFormat: function (value) {
            const numberParts = value.split(',');
            let numberFormatted = null;
            if (numberParts[1]) {
                numberFormatted = numberParts[0].split('.').join('') + '.' + numberParts[1];
            } else {
                numberFormatted = numberParts[0].split('.').join('');
            }
            return numberFormatted;
        }

    };
}());

export {
    numberService
};
