import { responseHandler } from '../../../common/services/responseHandler';
import { navigationService } from '../../../common/services/navigationService';
import { modalService } from '../../../common/services/modalService';
import { resourceProvider } from '../../../common/services/resourceProvider';
import { httpService } from '../../../common/services/httpService';
import 'isomorphic-fetch';

describe('response handler', () => {
    it('should handle error', () => {
        const failureHandler = (body, status) => { return 1; };
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ title: 'React POST Request Example' })
        };
        fetch('https://someLink.com', requestOptions)
            .then(response => {
                response.append('Link', ['<http://localhost/>', '<http://localhost:3000/>']);
                response.append('Set-Cookie', 'foo=bar; Path=/; HttpOnly');
                response.append('Warning', '199 Miscellaneous warning');
                response.status = 401;
                const result = responseHandler.handleError(failureHandler, response);
                expect(result).toBeDefined();
            });

        fetch('https://someLink.com', requestOptions)
            .then(response => {
                response.append('Link', ['<http://localhost/>', '<http://localhost:3000/>']);
                response.append('Set-Cookie', 'foo=bar; Path=/; HttpOnly');
                response.append('Warning', '199 Miscellaneous warning');
                response.status = 400;
                const result = responseHandler.handleError(failureHandler, response);
                expect(result).toBeDefined();
            });

        fetch('https://someLink.com', requestOptions)
            .then(response => {
                response.append('Link', ['<http://localhost/>', '<http://localhost:3000/>']);
                response.append('Set-Cookie', 'foo=bar; Path=/; HttpOnly');
                response.append('Warning', '199 Miscellaneous warning');
                response.status = 401;
                const result = responseHandler.handleError(failureHandler, response);
                expect(result).toBeDefined();
            });

        fetch('https://someLink.com', requestOptions)
            .then(response => {
                response.append('Link', ['<http://localhost/>', '<http://localhost:3000/>']);
                response.append('Set-Cookie', 'foo=bar; Path=/; HttpOnly');
                response.append('Warning', '199 Miscellaneous warning');
                const result = responseHandler.handleResponse(failureHandler, response);
                expect(result).toBeDefined();
            });
    });

    it('should handle redirect failure handler is passed', () => {
        navigationService.handleForbidden = jest.fn();
        global.window = Object.create(window);
        var response = {
            'Link': ['<http://localhost/>', '<http://localhost:3000/>'],
            'Set-Cookie': 'foo=bar; Path=/; HttpOnly',
            'Warning': '199 Miscellaneous warning',
            'headers': new Map([['RedirectPathOnAuthFailure', 'http://localhost/'], ['b', 2], ['c', 3]]),
             status: 401
        };

        const result = responseHandler.handleError(null, response);
        expect(global.window.location.href).toBe('http://localhost/');
    });

    it('should handle error and navigation service should handle the error if failure handler is not passed', () => {
        navigationService.handleError = jest.fn();
        var response = {
            'Link': ['<http://localhost/>', '<http://localhost:3000/>'],
            'Set-Cookie': 'foo=bar; Path=/; HttpOnly',
            'Warning': '199 Miscellaneous warning',
            'headers': new Map([['a', 1], ['b', 2], ['c', 3]]),
            status: 400
        };

        const result = responseHandler.handleError(null, response);
        expect(navigationService.handleError.mock.calls).toHaveLength(1);

    });

    it('should handle error with 401 status', () => {
        const failureHandler = (body, status) => { return 1; };
        var response = {
            'Link': ['<http://localhost/>', '<http://localhost:3000/>'],
            'Set-Cookie': 'foo=bar; Path=/; HttpOnly',
            'Warning': '199 Miscellaneous warning',
            'headers': new Map([['a', 1], ['b', 2], ['c', 3]]),
            status: 401
        };

        const result = responseHandler.handleError(failureHandler, response);
        expect(failureHandler).toHaveLength(2);
    });

    it('should handle error with 401 status when no failure handler is passed', () => {
        navigationService.handleError = jest.fn();
        var response = {
            'Link': ['<http://localhost/>', '<http://localhost:3000/>'],
            'Set-Cookie': 'foo=bar; Path=/; HttpOnly',
            'Warning': '199 Miscellaneous warning',
            'headers': new Map([['a', 1], ['b', 2], ['c', 3]]),
            status: 401
        };

        const result = responseHandler.handleError(null, response);
        expect(navigationService.handleError.mock.calls).toHaveLength(1);
    });

    it('should handle error with 409 status', () => {
        modalService.isOpen = jest.fn();
        resourceProvider.read = jest.fn();
        const failureHandler = (body, status) => { return 1; };
        var response = {
            'Link': ['<http://localhost/>', '<http://localhost:3000/>'],
            'Set-Cookie': 'foo=bar; Path=/; HttpOnly',
            'Warning': '199 Miscellaneous warning',
            'headers': new Map([['a', 1], ['b', 2], ['c', 3]]),
            status: 409
        };

        const result = responseHandler.handleError(failureHandler, response);
        expect(modalService.isOpen.mock.calls).toHaveLength(1);
        expect(resourceProvider.read.mock.calls).toHaveLength(1);
    });

    it('should handle error with 409 status', () => {
        modalService.isOpen = jest.fn();
        resourceProvider.read = jest.fn();
        httpService.setAntiforgeryRequestToken = jest.fn();

        const failureHandler = (body, status) => { return 1; };
        var response = {
            'Link': ['<http://localhost/>', '<http://localhost:3000/>'],
            'Set-Cookie': 'foo=bar; Path=/; HttpOnly',
            'Warning': '199 Miscellaneous warning',
            'headers': new Map([['XSRF-TOKEN', 1], ['b', 2], ['c', 3]]),
            status: 433
        };

        const result = responseHandler.handleError(failureHandler, response);
        expect(httpService.setAntiforgeryRequestToken.mock.calls).toHaveLength(1);
    });

    it('should handle failure response', () => {
        modalService.isOpen = jest.fn();
        resourceProvider.read = jest.fn();
        httpService.setAntiforgeryRequestToken = jest.fn();

        const failureHandler = (body, status) => { return 1; };
        const successHandler = (body, status) => { return 1; };
        var response = {
            'Link': ['<http://localhost/>', '<http://localhost:3000/>'],
            'Set-Cookie': 'foo=bar; Path=/; HttpOnly',
            'Warning': '199 Miscellaneous warning',
            'headers': new Map([['XSRF-TOKEN', 1], ['b', 2], ['c', 3]]),
            status: 401
        };

        const result = responseHandler.handleResponse(successHandler, failureHandler, response);
        expect(successHandler).toHaveLength(2);
    });

    it('should handle 200 array response', () => {
        modalService.isOpen = jest.fn();
        resourceProvider.read = jest.fn();
        httpService.setAntiforgeryRequestToken = jest.fn();

        const failureHandler = (body, status) => { return 1; };
        const successHandler = [(body, status) => { return 1; }];
        var response = {
            'Link': ['<http://localhost/>', '<http://localhost:3000/>'],
            'Set-Cookie': 'foo=bar; Path=/; HttpOnly',
            'Warning': '199 Miscellaneous warning',
            'headers': new Map([['XSRF-TOKEN', 1], ['b', 2], ['c', 3]]),
            status: 200
        };

        const result = responseHandler.handleResponse(successHandler, failureHandler, response);
        expect(successHandler).toHaveLength(1);
    });

    it('should handle 200 response', () => {
        modalService.isOpen = jest.fn();
        resourceProvider.read = jest.fn();
        httpService.setAntiforgeryRequestToken = jest.fn();

        const failureHandler = (body, status) => { return 1; };
        const successHandler = (body, status) => { return 1; };
        var response = {
            'Link': ['<http://localhost/>', '<http://localhost:3000/>'],
            'Set-Cookie': 'foo=bar; Path=/; HttpOnly',
            'Warning': '199 Miscellaneous warning',
            'headers': new Map([['XSRF-TOKEN', 1], ['b', 2], ['c', 3]]),
            status: 200
        };

        const result = responseHandler.handleResponse(successHandler, failureHandler, response);
        expect(successHandler).toHaveLength(2);
    });

    it('should handle 409 response', () => {
        modalService.isOpen = jest.fn();
        resourceProvider.read = jest.fn();
        httpService.setAntiforgeryRequestToken = jest.fn();

        const failureHandler = (body, status) => { return 1; };
        const successHandler = (body, status) => { return 1; };
        var response = {
            'Link': ['<http://localhost/>', '<http://localhost:3000/>'],
            'Set-Cookie': 'foo=bar; Path=/; HttpOnly',
            'Warning': '199 Miscellaneous warning',
            'headers': new Map([['XSRF-TOKEN', 1], ['b', 2], ['c', 3]]),
            status: 409
        };

        const result = responseHandler.handleResponse(successHandler, failureHandler, response);
        expect(failureHandler).toHaveLength(2);
    });

    it('should handle 201 response', () => {
        modalService.isOpen = jest.fn();
        resourceProvider.read = jest.fn();
        httpService.setAntiforgeryRequestToken = jest.fn();

        const failureHandler = (body, status) => { return 1; };
        const successHandler = (body, status) => { return 1; };
        var response = {
            'Link': ['<http://localhost/>', '<http://localhost:3000/>'],
            'Set-Cookie': 'foo=bar; Path=/; HttpOnly',
            'Warning': '199 Miscellaneous warning',
            'headers': new Map([['XSRF-TOKEN', 1], ['b', 2], ['c', 3]]),
            status: 409
        };

        const result = responseHandler.handleResponse(successHandler, failureHandler, response);
        expect(failureHandler).toHaveLength(2);
    });
});