import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { mount, shallow } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../../areas/setup';
import Notification, { Notification as NotificationComponent } from '../../../../common/components/notification/notification';

function mountWithRealStore() {
    const notification = {
        message: 'test',
        state: 'open',
        show: true,
        enableLink: true,
        launchComponent: 'error'
    };
    const notificationButton = {
        invokeNotificationButtonToggler: false
    };

    const reducers = {
        form: formReducer,
        notification: jest.fn(() => notification),
        notificationButton: jest.fn(() => notificationButton)
    };
    const store = createStore(combineReducers(reducers));
    const props = {
        onClose: jest.fn(() => {
            return 1;
        }),
        showErrors: jest.fn(() => {
            return 1;
        })
    };

    const enzymeWrapper = mount(<Provider store={store}><Notification {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('notification', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should click to close', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(`#page_notification`).exists()).toEqual(true);
        enzymeWrapper.find(`#page_notification`).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should open modal on click of link', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(`a`).prop(`id`)).toEqual(`launchmodal`);
        enzymeWrapper.find(`a`).find(`#launchmodal`).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should open modal on click of link', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(`a`).prop(`id`)).toEqual(`launchmodal`);
        enzymeWrapper.find(`a`).find(`#launchmodal`).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should have proper style for notification type info', () => {
        const props = {
            show: true,
            isOnModal: true,
            showOnModal: true,
            state: 'info'
        };
        const wrapper = shallow(<NotificationComponent {...props} />);
        wrapper.find('i').at(0).hasClass('fas fa-info-circle');
    });

    it('should have proper style for notification type success', () => {
        const props = {
            show: true,
            isOnModal: true,
            showOnModal: true,
            state: 'success'
        };
        const wrapper = shallow(<NotificationComponent {...props} />);
        wrapper.find('i').at(0).hasClass('far fa-check-circle');
    });

    it('should have proper style for notification type success', () => {
        const props = {
            show: true,
            isOnModal: true,
            showOnModal: true,
            state: 'error'
        };
        const wrapper = shallow(<NotificationComponent {...props} />);
        wrapper.find('i').at(0).hasClass('fas fa-exclamation-circle');
    });

    it('should have proper style for notification type warning', () => {
        const props = {
            show: true,
            isOnModal: true,
            showOnModal: true,
            state: 'warning'
        };
        const wrapper = shallow(<NotificationComponent {...props} />);
        wrapper.find('i').at(0).hasClass('fas fa-exclamation-triangle');
    });

    it('should have proper style for notification type unknown', () => {
        const props = {
            show: true,
            isOnModal: true,
            showOnModal: true,
            state: 'test'
        };
        const wrapper = shallow(<NotificationComponent {...props} />);
        wrapper.find('i').at(0).hasClass('fas fa-info-circle');
    });

    it('should call invokeButtonAction on click of notification action', () => {
        const props = {
            show: true,
            isOnModal: true,
            showOnModal: true,
            state: 'test',
            isButton: true,
            invokeButtonAction: jest.fn()
        };
        const wrapper = shallow(<NotificationComponent {...props} />);
        wrapper.find('button').at(0).simulate('click');
        expect(props.invokeButtonAction.mock.calls).toHaveLength(1);
    });

    it('should have a custom class name', () => {
        const props = {
            show: true,
            isOnModal: true,
            showOnModal: true,
            state: 'test',
            className: 'custom-class'
        };
        const wrapper = shallow(<NotificationComponent {...props} />);
        wrapper.find('.ep-notification').at(0).hasClass('custom-class');
    });

    it('should render tittle when send the title property', () => {
        const props = {
            show: true,
            isOnModal: true,
            showOnModal: true,
            state: 'test',
            title: 'title'
        };
        const wrapper = shallow(<NotificationComponent {...props} />);        
        expect(wrapper.find('.ep-notification__title')).toHaveLength(1);
    });

    it('should render the component sent in the props', () => {
        const props = {
            show: true,
            isOnModal: true,
            showOnModal: true,
            state: 'test',
            component: () => (<div id="message-component">MessageComponent Test</div>)
        };
        const wrapper = mount(<NotificationComponent {...props} />);        
        expect(wrapper.find('#message-component').at(0).text()).toEqual('MessageComponent Test');
    });
});
