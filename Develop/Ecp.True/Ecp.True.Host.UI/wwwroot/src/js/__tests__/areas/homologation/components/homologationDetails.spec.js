import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import HomologationDetails, { HomologationDetails as HomologationDetailsComponent } from '../../../../modules/administration/homologation/components/homologationDetails.jsx';
import { navigationService } from '../../../../common/services/navigationService';
const { constants } = require('../../../../common/services/constants');

const initialValues = {
    homologations: {
        homologationGroup: { sourceSystem: { name: 'name' }, destinationSystem: { name: 'name' }, group: { name: 'name' }, homologationObjectTypes: [] },
        homologationGroupDataMappings: [],
        mode: constants.Modes.Create,
        refreshToggler: false,
        homologationGroupToggler: false,
        objectTypesToggler: false
    },
    homologationGroupData: { items: [], config: { startEmpty: false, apiUrl: '' } },
    objectTypes: { selection: [], config: { startEmpty: false, apiUrl: '' } },
    flyout: { name: 'objectsFlyout' }
};

function mountWithRealStore() {
    const grid = { homologationGroupData: initialValues.homologationGroupData, objectTypes: initialValues.objectTypes };
    const reducers = {
        homologations: jest.fn(() => initialValues.homologations),
        grid: jest.fn(() => grid),
        flyout: jest.fn(() => initialValues.flyout)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} > <HomologationDetails initialValues={initialValues} /> </Provider>);
    return { store, enzymeWrapper };
}

describe('HomologationDetails', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should initialize homologation group on objectTypesToggler update', () => {
        const props = {
            homologationGroup: { sourceSystem: { name: 'name' }, destinationSystem: { name: 'name' }, group: { name: 'name' }, homologationObjectTypes: [] },
            homologationGroupDataMappings: [],
            mode: constants.Modes.Create,
            refreshToggler: false,
            homologationGroupToggler: false,
            objectTypesToggler: false,
            initUpdateHomologationGroup: jest.fn(),
            enableDisableSubmit: jest.fn()
        };

        const wrapper = shallow(<HomologationDetailsComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { objectTypesToggler: true }));
        expect(props.enableDisableSubmit.mock.calls).toHaveLength(1);
        expect(props.initUpdateHomologationGroup.mock.calls).toHaveLength(1);
    });

    it('should get data mappings on homologationGroupToggler update', () => {
        const props = {
            homologationGroup: { sourceSystem: { name: 'name' }, destinationSystem: { name: 'name' }, group: { name: 'name' }, homologationObjectTypes: [] },
            homologationGroupDataMappings: [],
            mode: constants.Modes.Create,
            refreshToggler: false,
            homologationGroupToggler: false,
            objectTypesToggler: false,
            getDataMappings: jest.fn(),
            enableDisableSubmit: jest.fn()
        };

        const wrapper = shallow(<HomologationDetailsComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { homologationGroupToggler: true }));
        expect(props.enableDisableSubmit.mock.calls).toHaveLength(1);
        expect(props.getDataMappings.mock.calls).toHaveLength(1);
    });

    it('should show notification on refreshToggler update', () => {
        const props = {
            homologationGroup: { sourceSystem: { name: 'name' }, destinationSystem: { name: 'name' }, group: { name: 'name' }, homologationObjectTypes: [] },
            homologationGroupDataMappings: [],
            mode: constants.Modes.Create,
            refreshToggler: false,
            homologationGroupToggler: false,
            objectTypesToggler: false,
            showNotification: jest.fn(),
            enableDisableSubmit: jest.fn()
        };

        navigationService.navigateTo = jest.fn();
        const wrapper = shallow(<HomologationDetailsComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { refreshToggler: true }));
        expect(props.enableDisableSubmit.mock.calls).toHaveLength(1);
        expect(props.showNotification.mock.calls).toHaveLength(1);
        expect(navigationService.navigateTo.mock.calls).toHaveLength(1);
    });

    it('should call onsubmit on create', () => {
        const props = {
            homologationGroup: { sourceSystem: { name: 'name' }, destinationSystem: { name: 'name' }, group: { name: 'name' }, homologationObjectTypes: [] },
            homologationGroupDataMappings: [],
            mode: constants.Modes.Create,
            refreshToggler: false,
            homologationGroupToggler: false,
            objectTypesToggler: false,
            requestCreateUpdateHomologationGroup: jest.fn()
        };

        navigationService.getParamByName = jest.fn();
        const wrapper = shallow(<HomologationDetailsComponent {...props} />);
        wrapper.instance().onSubmit();
        expect(props.requestCreateUpdateHomologationGroup.mock.calls).toHaveLength(1);
        expect(navigationService.getParamByName.mock.calls).toHaveLength(0);
    });

    it('should call onsubmit on update', () => {
        const props = {
            homologationGroup: { rowVersion: 'test',
                sourceSystem: { name: 'name', systemTypeId: 2 },
                destinationSystem: { name: 'name', systemTypeId: 3 },
                group: { name: 'name', categoryId: 10 },
                homologationObjectTypes: [{ name: 'name' }] },
            homologationGroupDataMappings: [{ name: 'name' }],
            mode: constants.Modes.Update,
            refreshToggler: false,
            homologationGroupToggler: false,
            objectTypesToggler: false,
            requestCreateUpdateHomologationGroup: jest.fn()
        };

        navigationService.getParamByName = jest.fn(() => 1);
        const wrapper = shallow(<HomologationDetailsComponent {...props} />);
        wrapper.instance().onSubmit();
        const output = {
            homologationGroups: [{ homologationGroupId: 1,
                groupTypeId: 10,
                homologationObjects: [{ name: 'name', homologationGroupId: 1 }],
                homologationDataMapping: [{ name: 'name', homologationGroupId: 1 }],
                rowVersion: 'test' }],
            destinationSystemId: 3,
            sourceSystemId: 2
        };
        expect(props.requestCreateUpdateHomologationGroup.mock.calls).toHaveLength(1);
        expect(props.requestCreateUpdateHomologationGroup).toHaveBeenCalledWith(output);
        expect(navigationService.getParamByName.mock.calls).toHaveLength(1);
    });
});
