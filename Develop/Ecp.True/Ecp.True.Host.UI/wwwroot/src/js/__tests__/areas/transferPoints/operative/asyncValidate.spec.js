import * as asyncValidation from '../../../../modules/administration/transferPoints/operative/asyncValidate';
import { resourceProvider } from '../../../../common/services/resourceProvider';

describe('Transfer point operative async validate',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
        });
        it('should raise node already exist error',
            async () => {
                const values = {
                    transferPoint: {
                        name: 'TestTransferPoint'
                    },
                    sourceNode: {
                        sourceNode: {
                            name: 'someName'
                        }
                    },
                    sourceProduct: {
                        product: {
                            name: 'someName'
                        }
                    },
                    sourceProductType: {
                        name: 'someName'
                    },
                    destinationNode: {
                        destinationNode: {
                            name: 'someName'
                        }
                    },
                    movementType: {
                        name: 'someName'
                    }
                };
                const props = {
                    initialValues: {
                        name: ''
                    },
                    sourceNodeType: 'someType'
                };
                const dispatch = jest.fn(() => true);
                const data = { isValid: false, errorCodes: 'transferPointAlreadyExists' };
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
                    name: 'transferPointAlreadyExists'
                };

                await expect(asyncValidation.asyncValidate(values, dispatch, props)).toBeDefined();
            });
    });
