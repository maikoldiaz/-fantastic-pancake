import * as asyncValidation from '../../../../../modules/administration/node/manageNode/asyncValidate';
import { resourceProvider } from '../../../../../common/services/resourceProvider';

describe('Mange node async validate',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
        });
        it('should raise node already exist error',
            async () => {
                const values = {
                    name: 'TestNode'
                };
                const props = {
                    initialValues: {
                        name: ''
                    }
                };
                const dispatch = jest.fn(() => true);
                const data = { isValid: false, errorCodes: 'nodeNameAlreadyExists' };
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
                    name: 'nodeNameAlreadyExists'
                };

                await expect(asyncValidation.asyncValidate(values, dispatch, props)).rejects.toEqual(errorValue);
            });
    });
