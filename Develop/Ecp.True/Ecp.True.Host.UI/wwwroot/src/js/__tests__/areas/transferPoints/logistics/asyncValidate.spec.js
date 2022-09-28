import * as asyncValidation from '../../../../modules/administration/transferPoints/logistics/asyncValidate';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { utilities } from '../../../../common/services/utilities';


describe('Element async validate',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
            //utilities.buildCenterStorageLocationName = jest.fn(() => 'value');
        });
        it('should raise transfer point operational exists error',
            async () => {
                const values = {
                    transferPoint:{
                        name: 'TransferPoint'
                    },
                    sourceProduct: {
                        product: {
                            name: 'sourceProduct'
                        }
                    },
                    destinationProduct: {
                        product: {
                            name: 'sourceProduct'
                        }
                    }
                };
                const props = {
                    sourceLogisticCenter: 'srcLogisticsCenter',
                    sourceStorageLocation : 'srcStorageLocation',
                    destinationLogisticCenter: 'dstStorageLocation',
                    destinationStorageLocation: 'dstStorageLocation'
                };
                const dispatch = jest.fn(() => true);
                const data = { 
                    body :{
                        errorCodes: 'transferPointOperationalExistsMessage'
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
