import setup from '../../../areas/setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { constants } from '../../../../common/services/constants';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { navigationService } from '../../../../common/services/navigationService';
import { httpService } from '../../../../common/services/httpService';
import NodeFilter, { NodeFilter as NodeFilterComponent } from '../../../../common/components/filters/nodeFilter.jsx';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';

const initialValues = {
    selectedCategory: {},
    selectedElement: {},
    searchedNodes: []
};
const sharedInitialState = {
    categoryElements: [],
    allCategories: []
};

const nodeFilter = {
    selectedCategory: '',
    selectedElement: '',
    selectedNode: '',
    reportToggler: false
};

function mountWithRealStore() {
    const reducers = {
        shared: jest.fn(() => sharedInitialState),
        nodeFilter: jest.fn(() => nodeFilter),
        form: formReducer,
        formValues: jest.fn(() => initialValues)
    };

    const store = createStore(combineReducers(reducers));

    const props = {
        category: {
            hidden: true,
            label: resourceProvider.read('category')
        },
        categoryElement: {
            hidden: false,
            label: resourceProvider.read('segment'),
            filterCategoryElementsItem: jest.fn()
        },
        reportType: {
            hidden: true
        },
        node: {
            hidden: true
        },
        initialDate: {
            hidden: false
        },
        finalDate: {
            hidden: false,
            allowAfterNow: false
        },
        dateRange: {
            hidden: true
        },
        getTicket: jest.fn(),
        submitText: resourceProvider.read('viewReport'),
        parentPage: 'nodeStatusReport',
        getStartDateProps: jest.fn(),
        getEndDateProps: jest.fn(),
        validateDateRange: jest.fn(),
        onSubmitFilter: jest.fn(),
        showError: jest.fn()
    };

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <NodeFilter config={props} />
    </Provider>);


    return { store, enzymeWrapper, props };
}

describe('NodeFilter', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should mount relevant Fields', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_nodeFilter_element')).toBe(true);
        expect(enzymeWrapper.exists('#dt_nodeFilter_initialDate')).toBe(true);
        expect(enzymeWrapper.exists('#dt_nodeFilter_finalDate')).toBe(true);
        expect(enzymeWrapper.exists('#dd_nodeFilter_category')).toBe(false);
        expect(enzymeWrapper.exists('#txt_nodeFilter_node')).toBe(false);
    });

    it('should give error when click on view reports and field values are not given', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find(ModalFooter).find('#btn_nodeFilter_submit').simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should mount component', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.instance().componentDidMount();
    });

    it('should update component', () => {
        const { enzymeWrapper } = mountWithRealStore();
        nodeFilter.reportToggler = true;
        enzymeWrapper.instance().componentDidUpdate(nodeFilter);
    });

    it('should handle submit', () => {
        const props = {
            handleSubmit: jest.fn(),
            getCategories: jest.fn(),
            getCategoryElements: jest.fn(),
            nodeFilterResetFields: jest.fn(),
            reportToggler: true,
            categoryElements: [
                {
                    categoryId: 'someId'
                }
            ],
            selectedCategory: {
                categoryId: 'someId'
            },
            config: {
                parentPage: 'balanceControlChart',
                categoryElement: {
                    filterCategoryElementsItem: jest.fn()
                },
                category: {
                    filterCategoryItems: jest.fn(() => {
                        return true;
                    })
                },
                reportType: {
                    hidden: true
                },
                node: {
                    isVisible: true
                },
                initialDate: {
                    isVisible: true
                },
                finalDate: {
                    isVisible: true
                },
                dateRange: {
                    hidden: true
                }
            }
        };
        const wrapper = shallow(<NodeFilterComponent {...props} />);
        wrapper.find('form').simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(1);
    });

    it('should call component did update on update of props', () => {
        const props = {
            handleSubmit: jest.fn(),
            getCategories: jest.fn(),
            getCategoryElements: jest.fn(),
            nodeFilterResetFields: jest.fn(),
            reportToggler: true,
            hideError: jest.fn(),
            categoryElements: [
                {
                    categoryId: 'someId'
                }
            ],
            selectedCategory: {
                categoryId: 'someId'
            },
            config: {
                parentPage: 'balanceControlChart',
                categoryElement: {
                    filterCategoryElementsItem: jest.fn()
                },
                category: {
                    filterCategoryItems: jest.fn(() => {
                        return true;
                    })
                },
                node: {
                    isVisible: true
                },
                reportType: {
                    hidden: true
                },
                initialDate: {
                    isVisible: true
                },
                finalDate: {
                    isVisible: true
                },
                dateRange: {
                    hidden: true
                }
            }
        };
        const navigationServiceMock = navigationService.navigateTo = jest.fn();
        const wrapper = shallow(<NodeFilterComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { reportToggler: false }));
        expect(navigationServiceMock.mock.calls).toHaveLength(1);
    });

    it('should fire on select category', () => {
        const props = {
            nodeFilterOnSelectCategory: jest.fn(),
            resetField: jest.fn(),
            nodeFilterClearSearchNodes: jest.fn(),
            handleSubmit: jest.fn(),
            getCategories: jest.fn(),
            getCategoryElements: jest.fn(),
            nodeFilterResetFields: jest.fn(),
            reportToggler: true,
            categoryElements: [
                {
                    categoryId: 'someId'
                }
            ],
            selectedCategory: {
                categoryId: 'someId'
            },
            config: {
                parentPage: 'balanceControlChart',
                categoryElement: {
                    filterCategoryElementsItem: jest.fn()
                },
                category: {
                    isVisible: true,
                    filterCategoryItems: jest.fn(() => {
                        return true;
                    })
                },
                reportType: {
                    hidden: true
                },
                node: {
                    isVisible: true
                },
                initialDate: {
                    isVisible: true
                },
                finalDate: {
                    isVisible: true
                },
                dateRange: {
                    hidden: true
                }
            }
        };
        const wrapper = shallow(<NodeFilterComponent {...props} />);
        wrapper.find('#dd_nodeFilter_category').simulate('change');
        expect(props.nodeFilterOnSelectCategory.mock.calls).toHaveLength(1);
        expect(props.resetField.mock.calls).toHaveLength(3);
        expect(props.nodeFilterClearSearchNodes.mock.calls).toHaveLength(1);
    });

    it('should fire on select segment if the category element is visible', () => {
        const props = {
            nodeFilterOnSelectElement: jest.fn(),
            resetField: jest.fn(),
            nodeFilterClearSearchNodes: jest.fn(),
            handleSubmit: jest.fn(),
            getCategories: jest.fn(),
            getCategoryElements: jest.fn(),
            nodeFilterResetFields: jest.fn(),
            reportToggler: true,
            categoryElements: [
                {
                    categoryId: 'someId'
                }
            ],
            selectedCategory: {
                categoryId: 'someId'
            },
            config: {
                getTicket: jest.fn(),
                parentPage: 'nodeStatusReport',
                categoryElement: {
                    isVisible: true,
                    filterCategoryElementsItem: jest.fn()
                },
                category: {
                    filterCategoryItems: jest.fn(() => {
                        return true;
                    })
                },
                reportType: {
                    hidden: true
                },
                node: {
                    isVisible: true
                },
                initialDate: {
                    isVisible: true
                },
                finalDate: {
                    isVisible: true
                },
                dateRange: {
                    hidden: true
                },
                onSegmentChange: jest.fn(() => {
                    return true;
                })
            }
        };
        const wrapper = shallow(<NodeFilterComponent {...props} />);
        wrapper.find('#dd_nodeFilter_element').simulate('change');
        expect(props.nodeFilterOnSelectElement.mock.calls).toHaveLength(1);
        expect(props.resetField.mock.calls).toHaveLength(3);
        expect(props.nodeFilterClearSearchNodes.mock.calls).toHaveLength(1);
        expect(props.config.getTicket.mock.calls).toHaveLength(1);
        expect(props.config.onSegmentChange.mock.calls).toHaveLength(1);
    });

    it('should request search node if the node is visible', () => {
        const props = {
            nodeFilterOnSelectElement: jest.fn(),
            resetField: jest.fn(),
            nodeFilterClearSearchNodes: jest.fn(),
            handleSubmit: jest.fn(),
            getCategories: jest.fn(),
            getCategoryElements: jest.fn(),
            nodeFilterResetFields: jest.fn(),
            reportToggler: true,
            categoryElements: [
                {
                    categoryId: 'someId'
                }
            ],
            selectedCategory: {
                categoryId: 'someId'
            },
            config: {
                getTicket: jest.fn(),
                parentPage: 'nodeStatusReport',
                categoryElement: {
                    isVisible: true,
                    filterCategoryElementsItem: jest.fn()
                },
                reportType: {
                    hidden: true
                },
                category: {
                    filterCategoryItems: jest.fn(() => {
                        return true;
                    })
                },
                node: {
                    isVisible: true
                },
                initialDate: {
                    isVisible: true
                },
                finalDate: {
                    isVisible: true
                },
                dateRange: {
                    hidden: true
                },
                onSegmentChange: jest.fn(() => {
                    return true;
                })
            }
        };
        const wrapper = shallow(<NodeFilterComponent {...props} />);
        wrapper.find('#txt_nodeFilter_node').simulate('change');
        expect(props.nodeFilterClearSearchNodes.mock.calls).toHaveLength(1);
    });

    it('should request report type if reportType is visible', () => {
        const props = {
            resetField: jest.fn(),
            handleSubmit: jest.fn(),
            getCategories: jest.fn(),
            getCategoryElements: jest.fn(),
            nodeFilterResetFields: jest.fn(),
            reportToggler: true,
            initialValues: {
                reportType: constants.Report.MovementAuditReport
            },
            categoryElements: [
                {
                    categoryId: 'someId'
                }
            ],
            selectedCategory: {
                categoryId: 'someId'
            },
            config: {
                getTicket: jest.fn(),
                parentPage: 'nodeStatusReport',
                categoryElement: {
                    isVisible: true,
                    filterCategoryElementsItem: jest.fn()
                },
                reportType: {
                    hidden: false
                },
                dateRange: {
                    hidden: true
                },
                category: {
                    filterCategoryItems: jest.fn(() => {
                        return true;
                    })
                },
                node: {
                    isVisible: false
                },
                initialDate: {
                    isVisible: true
                },
                finalDate: {
                    isVisible: true,
                    hidden: true
                },
                onSegmentChange: jest.fn(() => {
                    return true;
                }),
                getReportTypes: jest.fn()
            }
        };
        const wrapper = shallow(<NodeFilterComponent {...props} />);
        wrapper.find('#r_reportFilter_type').simulate('onChange');
        expect(wrapper).toHaveLength(1);
        expect(props.config.getReportTypes.mock.calls).toHaveLength(1);
    });
});
