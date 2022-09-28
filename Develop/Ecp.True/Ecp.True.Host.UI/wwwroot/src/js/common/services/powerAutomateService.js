import { constants } from './constants';

const powerAutomateService = (function () {
    function loadWidget(config, locale, type) {
        const sdk = new window.MsFlowSdk({
            hostName: window.location.origin,
            locale: locale
        });
        const widget = sdk.renderWidget(type, {
            container: 'div_flow',
            environmentId: config.environmentId,
            enableRegionalPortal: true,
            enableOnBehalfOfTokens: true,
            flowsSettings: {
                tab: 'myFlows'
            },
            approvalCenterSettings: {
                autoNavigateToDetails: false,
                hideInfoPaneCloseButton: true,
                hideLink: false,
                showSimpleEmptyPage: true
            }
        });

        widget.listen('GET_ACCESS_TOKEN', function (requestParam, widgetDoneCallback) {
            widgetDoneCallback(null, {
                token: config.accessToken
            });
        });
    }

    return {
        embedPowerAutomate: function (config, locale) {
            if (config.type === constants.PowerAutomate.Flows) {
                return loadWidget(config, locale, constants.PowerAutomate.Flows);
            }

            return loadWidget(config, locale, constants.PowerAutomate.Approvals);
        }
    };
}());

export { powerAutomateService };
