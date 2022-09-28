import * as pbi from 'powerbi-client';

const pbiService = (function () {
    return {
        buildEmbedConfig: (config, locale) => {
            return {
                tokenType: 1,
                type: 'report',
                id: config.reportId,
                pageName: 'ReportSection',
                embedUrl: `https://app.powerbi.com/reportEmbed?reportId=${config.reportId}&groupId=${config.groupId}`,
                accessToken: config.token,
                permissions: 7,
                viewMode: 0,
                settings: {
                    localeSettings: {
                        language: 'es',
                        formatLocale: locale
                    },
                    filterPaneEnabled: false,
                    customLayout: {
                        pageSize: {
                            type: pbi.models.PageSizeType.Widescreen
                        },
                        displayOption: pbi.models.DisplayOption.FitToWidth
                    },
                    layoutType: pbi.models.LayoutType.Custom
                }
            };
        }
    };
}());

export { pbiService };
