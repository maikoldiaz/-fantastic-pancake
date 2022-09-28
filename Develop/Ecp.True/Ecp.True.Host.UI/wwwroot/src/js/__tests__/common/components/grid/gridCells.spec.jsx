import {
    OwnershipStatusCell,
    DateCell,
    UploadStatusCell,
    StatusCell,
    CheckBoxCell,
    TogglerCell,
    ProductsCell,
    OwnershipPercentageInputTextCell,
    OwnershipVolumeInputNumberCell, TextWithToolTipCell
} from '../../../../common/components/grid/gridCells.jsx';
import { createStore, combineReducers } from 'redux';
import { dateService } from '../../../../common/services/dateService';
import { utilities } from '../../../../common/services/utilities';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { mount } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../../areas/setup.js';

function mountWithRealStore() {
    const reducers = {
        form: formReducer
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        original: {
            ownershipPercentage: 70,
            color: '#A0A0A0'
        }
    };

    const enzymeWrapper = mount(<Provider store={store}><OwnershipStatusCell {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}
describe('ownership status cell', () => {
    it('should mount successfully with color', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        const element = enzymeWrapper.find('div.ep-sbar__item');
        const backgroundColor = element.props().style.backgroundColor;

        expect(enzymeWrapper.find('div.ep-sbar__item').length).toEqual(1);
        expect(backgroundColor).toEqual(props.original.color);
    });

    it('should mount date cell successfully', () => {
        const props = {
            original: {
                ownershipPercentage: 70,
                color: '#A0A0A0'
            },
            column: {
                id: 'some Id'
            },
            dateWithTime: ''
        };
        let callbackId;
        const mockUtilities = utilities.getValue = jest.fn((original, id) => {
            callbackId = id;
        });
        const dateServiceMock = dateService.format = jest.fn(() => {
            return 'some date';
        });
        const wrapper = mount(<DateCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(mockUtilities.mock.calls).toHaveLength(1);
        expect(callbackId).toBe('some Id');
        expect(dateServiceMock.mock.calls).toHaveLength(1);
    });

    it('should mount upload status cell successfully', () => {
        const props = {
            getStatus: jest.fn(() => {
                return 'PROCESSING';
            }),
            noLocalize: jest.fn()
        };
        const wrapper = mount(<UploadStatusCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(props.getStatus.mock.calls).toHaveLength(1);
        expect(wrapper.find('i').hasClass('fas fa-spinner fas--spin m-r-2')).toEqual(true);
    });

    it('should mount upload status with proper class name based on status', () => {
        let props = {
            getStatus: jest.fn(() => {
                return 'PROCESSING';
            }),
            noLocalize: jest.fn()
        };
        let wrapper = mount(<UploadStatusCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(props.getStatus.mock.calls).toHaveLength(1);
        expect(wrapper.find('i').hasClass('fas fa-spinner fas--spin m-r-2')).toEqual(true);

        props = {
            getStatus: jest.fn(() => {
                return 'FINALIZED';
            }),
            noLocalize: jest.fn()
        };
        wrapper = mount(<UploadStatusCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(props.getStatus.mock.calls).toHaveLength(1);
        expect(wrapper.find('i').hasClass('fas fa-check-circle fas--success m-r-2')).toEqual(true);

        props = {
            getStatus: jest.fn(() => {
                return 'FAILED';
            }),
            noLocalize: jest.fn()
        };
        wrapper = mount(<UploadStatusCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(props.getStatus.mock.calls).toHaveLength(1);
        expect(wrapper.find('i').hasClass('fas fa-times-circle fas--error m-r-2')).toEqual(true);

        props = {
            getStatus: jest.fn(() => {
                return 'SENT';
            }),
            noLocalize: jest.fn()
        };
        wrapper = mount(<UploadStatusCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(props.getStatus.mock.calls).toHaveLength(1);
        expect(wrapper.find('i').hasClass('fas fa-file-import m-r-2')).toEqual(true);

        props = {
            getStatus: jest.fn(() => {
                return 'OWNERSHIP';
            }),
            noLocalize: jest.fn()
        };
        wrapper = mount(<UploadStatusCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(props.getStatus.mock.calls).toHaveLength(1);
        expect(wrapper.find('i').hasClass('fas fa-check-circle fas--success m-r-2')).toEqual(true);

        props = {
            getStatus: jest.fn(() => {
                return 'LOCKED';
            }),
            noLocalize: jest.fn()
        };
        wrapper = mount(<UploadStatusCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(props.getStatus.mock.calls).toHaveLength(1);
        expect(wrapper.find('i').hasClass('fas fa-lock m-r-2')).toEqual(true);

        props = {
            getStatus: jest.fn(() => {
                return 'UNLOCKED';
            }),
            noLocalize: jest.fn()
        };
        wrapper = mount(<UploadStatusCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(props.getStatus.mock.calls).toHaveLength(1);
        expect(wrapper.find('i').hasClass('fas fa-unlock m-r-2')).toEqual(true);

        props = {
            getStatus: jest.fn(() => {
                return 'PUBLISHING';
            }),
            noLocalize: jest.fn()
        };
        wrapper = mount(<UploadStatusCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(props.getStatus.mock.calls).toHaveLength(1);
        expect(wrapper.find('i').hasClass('fas fa-spinner fas--spin m-r-2')).toEqual(true);

        props = {
            getStatus: jest.fn(() => {
                return 'PUBLISHED';
            }),
            noLocalize: jest.fn()
        };
        wrapper = mount(<UploadStatusCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(props.getStatus.mock.calls).toHaveLength(1);
        expect(wrapper.find('i').hasClass('fas fa-upload m-r-2')).toEqual(true);

        props = {
            getStatus: jest.fn(() => {
                return 'SUBMITFORAPPROVAL';
            }),
            noLocalize: jest.fn()
        };
        wrapper = mount(<UploadStatusCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(props.getStatus.mock.calls).toHaveLength(1);
        expect(wrapper.find('i').hasClass('fas fa-file-signature m-r-2')).toEqual(true);

        props = {
            getStatus: jest.fn(() => {
                return 'REOPENED';
            }),
            noLocalize: jest.fn()
        };
        wrapper = mount(<UploadStatusCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(props.getStatus.mock.calls).toHaveLength(1);
        expect(wrapper.find('i').hasClass('fas fa-redo m-r-2')).toEqual(true);

        props = {
            getStatus: jest.fn(() => {
                return 'REOPENEDtest';
            }),
            noLocalize: jest.fn()
        };
        wrapper = mount(<UploadStatusCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(props.getStatus.mock.calls).toHaveLength(1);
        expect(wrapper.find('i').hasClass('fas fa-spinner fas--spin m-r-2')).toEqual(true);
    });

    it('should mount status cell successfully', () => {
        const props = {
            original: {
                ownershipPercentage: 70,
                color: '#A0A0A0'
            },
            column: {
                id: 'some Id'
            },
            trueClass: 'someTrueClass',
            falseClass: 'someFalseClass',
            trueKey: 'someTrueKey',
            falseKey: 'someFalseKey'
        };
        const wrapper = mount(<StatusCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(wrapper.find('i').hasClass('someFalseClass')).toEqual(true);
    });

    it('should mount check box cell successfully', () => {
        const props = {
            original: {
                ownershipPercentage: 70,
                color: '#A0A0A0'
            },
            column: {
                id: 'some Id'
            },
            trueClass: 'someTrueClass',
            falseClass: 'someFalseClass',
            trueKey: 'someTrueKey',
            falseKey: 'someFalseKey',
            name: 'someName'
        };
        const wrapper = mount(<CheckBoxCell {...props} />);
        expect(wrapper).toBeDefined();
        expect(wrapper.find('input').hasClass('ep-checkbox__input')).toEqual(true);
    });

    it('should mount toggler cell successfully', () => {
        const props = {
            original: {
                ownershipPercentage: 70,
                color: '#A0A0A0'
            },
            column: {
                id: 'some Id'
            },
            trueClass: 'someTrueClass',
            falseClass: 'someFalseClass',
            trueKey: 'someTrueKey',
            falseKey: 'someFalseKey',
            name: 'someName',
            updateHomologationObjectType: jest.fn()
        };
        const wrapper = mount(<TogglerCell {...props} />);
        expect(wrapper).toBeDefined();
        wrapper.find('input').simulate('change');
        expect(wrapper.find('input').hasClass('ep-toggler__input')).toEqual(true);
        expect(props.updateHomologationObjectType.mock.calls).toHaveLength(1);
    });

    it('should mount products cell successfully', () => {
        const props = {
            original: {
                ownershipPercentage: 70,
                color: '#A0A0A0',
                someId: {}
            },
            column: {
                id: 'someId'
            },
            addProducts: jest.fn(),
            name: 'someName'
        };
        const wrapper = mount(<ProductsCell {...props} />);
        expect(wrapper).toBeDefined();
        wrapper.find('#btn_someName_addProducts').simulate('click');
        expect(props.addProducts.mock.calls).toHaveLength(1);
    });

    it('should mount ownership percentage input text cell successfully', () => {
        const props = {
            original: {
                ownershipPercentage: 70,
                color: '#A0A0A0'
            },
            column: {
                id: 'some Id'
            },
            updateOwnershipPercentage: jest.fn(),
            checkIfNumberAllowed: jest.fn()
        };
        const wrapper = mount(<OwnershipPercentageInputTextCell {...props} />);
        expect(wrapper).toBeDefined();
    });

    it('should mount ownership Volume input Number cell successfully', () => {
        const props = {
            original: {
                ownershipVolume: 123,
                color: '#A0A0A0'
            },
            column: {
                id: 'some Id'
            },
            updateOwnershipVolume: jest.fn(),
            checkIfNumberAllowed: jest.fn()
        };
        const wrapper = mount(<OwnershipVolumeInputNumberCell {...props} />);
        expect(wrapper).toBeDefined();
    });

    it('should mount text with tooltip successfully', () => {
        const props = {
            original: {
                ownershipVolume: 123,
                color: '#A0A0A0'
            },
            column: {
                id: 'some Id'
            },
            updateOwnershipVolume: jest.fn(),
            checkIfNumberAllowed: jest.fn()
        };
        const wrapper = mount(<TextWithToolTipCell {...props} />);
        expect(wrapper).toBeDefined();
    });
});
