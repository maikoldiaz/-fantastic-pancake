import * as asyncValidation from '../../../../modules/administration/category/manageElement/asyncValidate';
import { resourceProvider } from '../../../../common/services/resourceProvider';

describe('Element async validate',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
        });
        it('should raise element name already exist error',
            async () => {
                const values = {
                    name: 'TestElement',
                    category: {
                        categoryId: 1
                    }
                };
                const props = {
                    initialValues: {
                        name: '',
                        category: {
                            categoryId: {}
                        }
                    }
                };
                const dispatch = jest.fn(() => true);
                const data = { 
                    body :{
                        errorCodes: 'elementNameAlreadyExists'
                    }
                };
                global.fetch = jest.fn(() => {
                    return new Promise((resolve => {
                        resolve({
                            json: function () {
                                return data;
                            }
                        });
                    }));
                });

                await expect(asyncValidation.asyncValidate(values, dispatch, props)).resolve;
            });
    });
