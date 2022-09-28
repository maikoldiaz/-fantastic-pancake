import setup from "../../../../setup";
import React from "react";
import { mount, shallow } from "enzyme";
import { Provider } from "react-redux";
import { createStore, combineReducers } from "redux";
import { resourceProvider } from "../../../../../../common/services/resourceProvider";
import AssignmentCostCenter, { AssignmentCostCenter as AssignmentCostCenterComponent, movementTypeAndCostCenterValidations } from "../../../../../../modules/administration/nodeConnection/attributes/components/costCenter/AssignmentCostCenter";
import { httpService } from "../../../../../../common/services/httpService";
import { navigationService } from "../../../../../../common/services/navigationService";

function mountWithRealStore(formData = []) {
    const data = {
        nodeConnection: {
            attributes: {
                nodes: []
            },
            nodeCostCenters: {
                duplicates: []
            }
        },
        shared: {
            groupedCategory: [],
            groupedCategoryElements: []
        },
        root: {
            systemConfig: {
                maxNodeCostCenterBatchCreation: 1
            }
        },
        groupedCategoryElements: [],
        selector: jest.fn(() => []),
        openModal: jest.fn()

    };

    const reducers = {
        onClose: jest.fn(() => Promise.resolve()),
        nodeConnection: jest.fn(() => data.nodeConnection),
        shared: jest.fn(() => data.shared),
        nodeCostCenters: jest.fn(() => data.nodeConnection.nodeCostCenters),
        root: jest.fn(() => data.root),

    };

    const props = {
        getFieldNameFromForm: jest.fn(() => formData),
        openModal: jest.fn()
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <AssignmentCostCenter {...props} />
    </Provider>);

    return { store, enzymeWrapper, props };
}

describe('AssignmentCostCenter', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-US');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should have cancel and submit buttons', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find("#btn_assignmentCostCenter_cancel").text()).toEqual('cancel');
        expect(enzymeWrapper.find("#btn_assignmentCostCenter_submit").text()).toEqual('submit');
    });

    it('should handle form submit', () => {
        const props = {
            initialValues: {
                movementTypeAndCostCenter: [undefined]
            },
            groupedCategoryElements: {},
            nodes: [],
            destinationNodes: [],
            nodeCostCenterDuplicates: [],
            isNodeCostCenterDuplicatesNotified: undefined,
            maxNodeCostCenterBatchCreation: 0,
            handleSubmit: jest.fn(),
            getActiveNodes: jest.fn(),
            getCategoryElements: jest.fn(),
            createNodeConstCenter: jest.fn(),
            changeTabPanel: jest.fn()
        };
        const wrapper = shallow(<AssignmentCostCenterComponent {...props} />);
        wrapper.find('form').simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(1);
    });

    it('should handle node cost center duplicates - notification', () => {
        const props = {
            initialValues: {
                movementTypeAndCostCenter: [undefined]
            },
            groupedCategoryElements: {},
            nodeCostCenterDuplicates: [],
            destinationNodes: [],
            nodeCostCenterDuplicates: [],
            isNodeCostCenterDuplicatesNotified: false,
            maxNodeCostCenterBatchCreation: 0,
            getFieldNameFromForm: jest.fn(() => []),
            handleSubmit: jest.fn(),
            getActiveNodes: jest.fn(),
            getCategoryElements: jest.fn(),
            notifyNodeCostCenterDuplicates: jest.fn(),
            openNodeCostCenterDuplicatesModal: jest.fn(),
            changeTabPanel: jest.fn()
        };
        const wrapper = shallow(<AssignmentCostCenterComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, {
            nodeCostCenterDuplicates: [{
                costCenterName: "Test cost center name",
                movementTypeName: "Test movement type name",
                status: true
            }]
        }));
        expect(wrapper.instance().props.notifyNodeCostCenterDuplicates).toBeCalled();
    });

    it('should simulate cancel button and redirect', () => {
        const { enzymeWrapper } = mountWithRealStore();
        const navigationServiceMock = navigationService.navigateTo = jest.fn(() => 'manage');
        enzymeWrapper.find('#btn_assignmentCostCenter_cancel').simulate('click');
        expect(navigationServiceMock).toBeCalled();

    });


    it('should simulate cancel button and open modal', () => {
        const props = {
            initialValues: {
                movementTypeAndCostCenter: [undefined]
            },
            groupedCategoryElements: {},
            nodeCostCenterDuplicates: [],
            destinationNodes: [],
            nodeCostCenterDuplicates: [],
            isNodeCostCenterDuplicatesNotified: false,
            maxNodeCostCenterBatchCreation: 1,
            handleSubmit: jest.fn(),
            getActiveNodes: jest.fn(),
            getCategoryElements: jest.fn(),
            notifyNodeCostCenterDuplicates: jest.fn(),
            openModal: jest.fn(),
            getFieldNameFromForm: jest.fn(() => []),
            changeTabPanel: jest.fn()

        };
        const wrapper = shallow(<AssignmentCostCenterComponent {...props} />);
        wrapper.find('#btn_assignmentCostCenter_cancel').prop('onClick')();
        expect(wrapper.instance().props.openModal).toBeCalled();
    });

    it('should handle blur source node', () => {
        const props = {
            initialValues: {
                movementTypeAndCostCenter: [undefined]
            },
            groupedCategoryElements: {},
            nodeCostCenterDuplicates: [],
            destinationNodes: [],
            nodeCostCenterDuplicates: [],
            isNodeCostCenterDuplicatesNotified: false,
            maxNodeCostCenterBatchCreation: 1,
            handleSubmit: jest.fn(),
            getActiveNodes: jest.fn(),
            getCategoryElements: jest.fn(),
            notifyNodeCostCenterDuplicates: jest.fn(),
            openModal: jest.fn(),
            handleBlurSourceNode: jest.fn(),
            onBlurSourceNode: jest.fn(),
            changeTabPanel: jest.fn()

        };
        const wrapper = shallow(<AssignmentCostCenterComponent {...props} />);
        wrapper.find('#sourceNode_name_sel').prop('onBlur')();
        expect(wrapper.instance().props.onBlurSourceNode).toBeCalled();

    });

    it('should add validations for movement type and costCenter ', () => {
        const data = {
            movementTypeAndCostCenter: [undefined]
        };
        const result = movementTypeAndCostCenterValidations(data);
        const expected = { movementTypeAndCostCenter: [{ movementType: 'required', costCenter: 'required' }] };

        expect(result).toEqual(expected);
    });

    it('should add validation only for movement type and cost center - empty values', () => {
        const data = {
            movementTypeAndCostCenter: [{}]
        };
        const result = movementTypeAndCostCenterValidations(data);

        const expected = {
            movementTypeAndCostCenter: [
                { movementType: "required", costCenter: "required" }
            ]
        };

        expect(result).toEqual(expected);
    });

});