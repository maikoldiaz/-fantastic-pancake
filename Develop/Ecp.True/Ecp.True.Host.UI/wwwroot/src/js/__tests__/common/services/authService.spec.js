import { authService } from '../../../common/services/authService.js';

const ctx = {
    name: 'True User',
    userId: 'trueUser@Something.com',
    image: 'some base 64 image string',
    roles: ['Administrator', 'ProfessionalSegmentBalances']
};

describe('auth service',
    () => {
        beforeAll(() => {
            authService.initialize(ctx);
        });

        it('should get Name',
            () => {
                expect(authService.getUserName())
                    .toMatch('True User');
            });
        it('should get userId',
            () => {
                expect(authService.getUserId())
                    .toMatch('trueUser@Something.com');
            });
        it('should get Image',
            () => {
                expect(authService.getUserImage()).toMatch('some base 64 image string');
            });
        it('should check return true if the user is authorized',
            () => {
                expect(authService.isAuthorized('Administrator'))
                    .toBeTruthy();
            });
        it('should check return false if the user is not authorized',
            () => {
                expect(authService.isAuthorized('Other role'))
                    .toBeFalsy();
            });
    });
