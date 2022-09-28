import { ApplicationInsights, DistributedTracingModes, SeverityLevel } from '@microsoft/applicationinsights-web';
import { utilities } from '../services/utilities';

const reactPlugin = null;
let appInsights = null;

/**
 * Create the App Insights Telemetry Service
 * @return {{reactPlugin: ReactPlugin, appInsights: Object, initialize: Function}} - Object
 */
/* istanbul ignore next */
const createTelemetryService = () => {
    /**
     * Initialize the Application Insights class
     * @param {string} instrumentationKey - Application Insights Instrumentation Key
     * @param {Object} browserHistory - client's browser history, supplied by the withRouter HOC
     * @return {void}
     */
    const initialize = instrumentationKey => {
        if (!instrumentationKey) {
            throw new Error('Instrumentation key not provided in telemetry service');
        }

        appInsights = new ApplicationInsights({
            config: {
                instrumentationKey: instrumentationKey,
                // W3C
                distributedTracingMode: DistributedTracingModes.AI_AND_W3C,
                // https://docs.microsoft.com/en-us/azure/azure-monitor/app/javascript#single-page-applications
                enableAutoRouteTracking: true,
                maxBatchInterval: 0,
                disableFetchTracking: false
            }
        });

        // https://github.com/microsoft/ApplicationInsights-JS/blob/master/README.md
        appInsights.loadAppInsights();

        appInsights.trackPageView();

        // https://github.com/Azure/react-appinsights/issues/84#issuecomment-486901261
        appInsights.addTelemetryInitializer(envelope => {
            envelope.tags = envelope.tags || [];
            envelope.tags.push({ 'ai.cloud.role': 'ecp.true.ui.client' });
        });
    };

    const random32 = () => {
        return (0x100000000 * utilities.generateRandomNumber()) | 0;
    };

    // Credit: https://github.com/microsoft/ApplicationInsights-JS/blob/master/shared/AppInsightsCommon/src/Util.ts
    const w3cId = () => {
        const hexValues = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'];

        // rfc4122 version 4 UUID without dashes and with lowercase letters
        let oct = ''; let tmp;
        for (let a = 0; a < 4; a++) {
            tmp = random32();
            oct = oct +
                (hexValues[tmp & 0xF] +
                hexValues[(tmp >> 4) & 0xF] +
                hexValues[(tmp >> 8) & 0xF] +
                hexValues[(tmp >> 12) & 0xF] +
                hexValues[(tmp >> 16) & 0xF] +
                hexValues[(tmp >> 20) & 0xF] +
                hexValues[(tmp >> 24) & 0xF] +
                hexValues[(tmp >> 28) & 0xF]);
        }

        // "Set the two most significant bits (bits 6 and 7) of the clock_seq_hi_and_reserved to zero and one, respectively"
        const clockSequenceHi = hexValues[8 + (utilities.generateRandomNumber() * 4) | 0];
        return oct.substr(0, 8) + oct.substr(9, 4) + '4' + oct.substr(13, 3) + clockSequenceHi + oct.substr(16, 3) + oct.substr(19, 12);
    };

    const trackException = (err, inputTraceId) => {
        let traceId = inputTraceId;
        if (!traceId) {
            traceId = w3cId();
        }

        // https://github.com/microsoft/ApplicationInsights-JS/blob/master/README.md#upgrading-from-the-old-version-of-application-insights
        appInsights.context.telemetryTrace.traceID = traceId;

        appInsights.trackException({ error: new Error(err), severityLevel: SeverityLevel.Error });
        appInsights.flush();
    };

    const trackTrace = (msg, inputTraceId) => {
        let traceId = inputTraceId;
        if (!traceId) {
            traceId = w3cId();
        }

        appInsights.context.telemetryTrace.traceID = traceId;
        appInsights.trackTrace({ message: msg, severityLevel: SeverityLevel.Information });
        appInsights.flush();
    };

    const trackEvent = (name, inputTraceId) => {
        let traceId = inputTraceId;
        if (!traceId) {
            traceId = w3cId();
        }

        appInsights.context.telemetryTrace.traceID = traceId;
        appInsights.trackEvent({ name: name });
        appInsights.flush();
    };

    const setTraceId = inputTraceId => {
        let traceId = inputTraceId;
        if (!traceId) {
            traceId = w3cId();
        }

        appInsights.context.telemetryTrace.traceID = traceId;
    };

    const isReady = () => appInsights !== null;

    return { reactPlugin, appInsights, initialize, trackException, trackTrace, trackEvent, w3cId, setTraceId, isReady };
};

/* istanbul ignore next */
export const ai = createTelemetryService();
export const getAppInsights = () => appInsights;
