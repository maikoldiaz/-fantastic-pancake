import * as asyncValidation from '../../../modules/administration/transformSettings/asyncValidate';
import { resourceProvider } from '../../../common/services/resourceProvider';

describe('TransformSettings async validate',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
        });
        it('should raise transformation already exist error',
            async () => {
                const values = {
                    origin: {
                        destinationNode: {
                            nodeId: 1
                        },
                        sourceProduct: {
                            productId: 1
                        },
                        destinationProduct: {
                            productId: 1
                        },
                        measurementUnit: {
                            elementId: 1
                        }
                    },
                    originSourceNode: {
                        nodeId: 1
                    }
                };
                const props = {
                    initialValues: {
                        name: ''
                    }
                };
                const dispatch = jest.fn(() => true);
                const data = { body: values };
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
                    name: 'sourceTransformationDuplicate'
                };

                await expect(asyncValidation.asyncValidate(values, dispatch, props)).toBeDefined();
            });
    });
