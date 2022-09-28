import setup from './../setup';
import React from 'react';
import { mount } from 'enzyme';
import HomePage from './../../../modules/home/home/components/homePage.jsx';
import { resourceProvider } from '../../../common/services/resourceProvider';
import { httpService } from '../../../common/services/httpService';

describe('home page', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const wrapper = mount(<HomePage />);
        expect(wrapper).toHaveLength(1);
    });

    it('should render the home page image successfully', () => {
        const wrapper = mount(<HomePage />);
        const imgWrapper = wrapper.find('#img_true_home');
        expect(imgWrapper).toHaveLength(1);
        expect(imgWrapper.find({ alt: 'home' })).toHaveLength(1);
        expect(imgWrapper.find({ src: './../dist/images/home.svg' })).toHaveLength(1);
    });
});
