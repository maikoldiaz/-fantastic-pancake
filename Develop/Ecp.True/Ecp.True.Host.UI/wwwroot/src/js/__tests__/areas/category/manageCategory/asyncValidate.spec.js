import * as asyncValidation from '../../../../modules/administration/category/manageCategory/asyncValidate';
import { resourceProvider } from '../../../../common/services/resourceProvider';

describe('Category async validate',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
        });
        it('should raise category name already exist error',
            async () => {
                const values = {
                    name: 'TestCategory'
                };
                const props = {
                    initialValues: {
                        name: ''
                    }
                };
                const dispatch = jest.fn(() => true);
                const data = { isValid: false, errorCodes: 'categoryNameAlreadyExists' };
                global.fetch = jest.fn(() => {
                    return new Promise((resolve => {
                        resolve({
                            json: function () {
                                return data;
                            }
                        });
                    }));
                });

                const errorValue = {
                    name: 'categoryNameAlreadyExists'
                };

                await expect(asyncValidation.asyncValidate(values, dispatch, props)).rejects.toEqual(errorValue);
            });
    });
