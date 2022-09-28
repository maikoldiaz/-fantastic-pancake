import { utilities } from './utilities';
import { constants } from './constants';

const scenarioService = (function () {
    const scenarioList = {};
    const featureList = {};

    const featureOverrides = {};

    return {
        initialize: scenarios => {
            scenarios
                .sort((a, b) => a.sequence - b.sequence)
                .forEach(m => {
                    if (!utilities.hasProperty(scenarioList, m.name)) {
                        scenarioList[m.name] = {
                            scenario: m.name,
                            features: [],
                            isCollapsible: m.features.length > 0,
                            isCollapsed: true
                        };
                    }

                    if (!utilities.isNullOrUndefined(m.features)) {
                        m.features
                            .sort((a, b) => a.sequence - b.sequence)
                            .forEach(feature => {
                                // feature name should be lower case since this will be picked up from url.
                                featureList[utilities.toLowerCase(feature.name)] = {
                                    scenario: m.name,
                                    name: feature.name,
                                    description: feature.description
                                };

                                if (!utilities.hasProperty(scenarioList[m.name].features, feature.name)) {
                                    scenarioList[m.name].features.push({
                                        name: feature.name,
                                        description: feature.description
                                    });
                                }
                            });

                        scenarioList[m.name].isCollapsible = true;
                    }
                });
        },
        getScenarios: () => {
            return scenarioList;
        },
        getFeature: feature => {
            return utilities.getValue(featureList, feature) || utilities.getValue(featureOverrides, feature);
        },
        getReportTypes: () => {
            const featureNames = Object.values(featureList).map(m => m.name);
            return Object.keys(constants.ReportType).filter(t => featureNames.includes(constants.ReportType[t]));
        }
    };
}());

export { scenarioService };
