import { devResources } from './../resources/resources.en';
import { prodResources } from './../resources/resources.es';
import { httpService } from './httpService';
import { utilities } from './utilities';

const resourceProvider = (function () {
    let resources = null;
    function getResources() {
        if (utilities.isNullOrUndefined(resources)) {
            resources = prodResources;

            const culture = httpService.getQueryString('culture');
            if (utilities.equalsIgnoreCase(culture, 'en')) {
                resources = devResources;
            } else if (utilities.equalsIgnoreCase(culture, 'es')) {
                resources = prodResources;
            }
        }

        return resources;
    }

    return {
        read: function (key) {
            return getResources().getResource(key);
        },
        readFormat: function (key, params) {
            let localizedString = getResources().getResource(key);
            for (let i = 0; i < params.length; i++) {
                localizedString = localizedString.replace(`{${i}}`, params[i]);
            }

            return localizedString;
        }
    };
}());

export { resourceProvider };
