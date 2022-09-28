import { scenarioService } from '../../../common/services/scenarioService';

const features = [
    { featureId: 1, name: 'some feature1', description: 'some feature1 description', sequence: 1 },
    { featureId: 3, name: 'some feature3', description: 'some feature3 description', sequence: 3 },
    { featureId: 2, name: 'some feature2', description: 'some feature2 description', sequence: 2 }
];

const scenarios = [
    { scenarioId: 1, name: 'some scenario1', sequence: 10, features: features },
    { scenarioId: 2, name: 'some scenario2', sequence: 30, features: features },
    { scenarioId: 3, name: 'some scenario3', sequence: 20, features: features },
    { scenarioId: 4, name: 'some scenario4', sequence: 2, features: features }
];

const expected = {
    'some scenario4':
    {
        scenario: 'some scenario4',
        features: [{name: 'some feature1', description: 'some feature1 description'}, {name: 'some feature2', description: 'some feature2 description'}, {name: 'some feature3', description: 'some feature3 description'}],
        isCollapsible: true,
        isCollapsed: true
    },
    'some scenario1':
    {
        scenario: 'some scenario1',
        features: [{name: 'some feature1', description: 'some feature1 description'}, {name: 'some feature2', description: 'some feature2 description'}, {name: 'some feature3', description: 'some feature3 description'}],
        isCollapsible: true,
        isCollapsed: true
    },
    'some scenario3':
    {
        scenario: 'some scenario3',
        features: [{name: 'some feature1', description: 'some feature1 description'}, {name: 'some feature2', description: 'some feature2 description'}, {name: 'some feature3', description: 'some feature3 description'}],
        isCollapsible: true,
        isCollapsed: true
    },
    'some scenario2':
    {
        scenario: 'some scenario2',
        features: [{name: 'some feature1', description: 'some feature1 description'}, {name: 'some feature2', description: 'some feature2 description'}, {name: 'some feature3', description: 'some feature3 description'}],
        isCollapsible: true,
        isCollapsed: true
    }
};

const expected2 = {
    name: 'some feature1',
    scenario: 'some scenario2',
    description: 'some feature1 description'
};

describe('scenario service',
    () => {
        it('get scenarios should return scenarios',
            () => {
                scenarios.sort(function (obj1, obj2) {
                    return obj1.sequence - obj2.sequence;
                });
                scenarioService.initialize(scenarios);
                expect(scenarioService.getScenarios()).toStrictEqual(expected);
            });
        it('get scenarios should return feature',
            () => {
                scenarios.sort(function (obj1, obj2) {
                    return obj1.sequence - obj2.sequence;
                });
                scenarioService.initialize(scenarios);
                expect(scenarioService.getFeature('some feature1')).toStrictEqual(expected2);
            });
    });
