const httpService = (function () {
    let token = null;
    function setToken(value) {
        token = value;
    }

    function getToken() {
        return token;
    }

    function getQueryStringParameterByName(name, url) {
        const location = !url ? window.location.href : url;

        const paramName = name.replace(/[[\]]/g, '\\$&');
        const regex = new RegExp(`[?&]${paramName}(=([^&#]*)|&|#|$)`);
        const results = regex.exec(location);

        if (!results) {
            return null;
        }
        if (!results[2]) {
            return '';
        }
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }

    function getPlaceholders() {
        const location = window.location.pathname;
        let placeholders = location.split('/');
        placeholders = placeholders.filter(x => x !== null && x.length > 0);
        return placeholders;
    }

    function getCurrentPathName() {
        return window.location.pathname;
    }

    function getModuleName() {
        const placeholders = getPlaceholders();
        if (!placeholders || placeholders.length === 0) {
            return '';
        }
        if (!placeholders[0]) {
            return '';
        }

        return placeholders[0];
    }

    function getSubModuleName() {
        const placeholders = getPlaceholders();
        if (!placeholders || placeholders.length === 0) {
            return '';
        }
        if (!placeholders[1]) {
            return '';
        }

        return placeholders[1];
    }

    function getDetailsModuleName() {
        const placeholders = getPlaceholders();
        if (!placeholders || placeholders.length === 0) {
            return null;
        }
        if (!placeholders[2]) {
            return '';
        }

        return placeholders[2] || '';
    }

    return {
        readAntiforgeryToken: function () {
            return getToken();
        },
        getCurrentCulture: function () {
            return getQueryStringParameterByName('culture') ? getQueryStringParameterByName('culture') : 'es';
        },
        getParamByName: function () {
            const placeholders = getPlaceholders();
            return placeholders[placeholders.length - 1];
        },
        getModuleName: function () {
            return getModuleName();
        },
        getSubModuleName: function () {
            return getSubModuleName();
        },
        getDetailsModuleName: function () {
            return getDetailsModuleName();
        },
        hasHash: hashName => {
            return window.location.href.includes(`#${hashName}`);
        },
        getCurrentPathName: () => {
            return getCurrentPathName();
        },
        getQueryString: name => {
            return getQueryStringParameterByName(name);
        },
        setAntiforgeryRequestToken: tkn => {
            setToken(tkn);
        }
    };
}());

export { httpService };
