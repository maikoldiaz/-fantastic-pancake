import * as asyncValidation from '../../../modules/administration/annulation/asyncValidate';
import { resourceProvider } from '../../../common/services/resourceProvider';
import { constants } from '../../../common/services/constants';

describe('Reversal async validate',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
        });
        it('should raise source relationship already exist error',
            async () => {
                const values = {
                    source: {
                        movement: { elementId: 1 },
                        product: { originTypeId: 1 },
                        node: { originTypeId: 1 }
                    },
                    annulation: {
                        movement: { elementId: 1 },
                        product: { originTypeId: 1 },
                        node: { originTypeId: 1 }
                    },
                    isActive: true
                };
                const props = { };
                const dispatch = jest.fn(() => true);
                const data = { isValid: false, body: { errorCodes: 'annulationSourceExists', type: constants.Annulations.Sections.Source } };
                global.fetch = jest.fn(() => {
                    return new Promise((resolve => {
                        resolve({
                            json: function () {
                                return data;
                            }
                        });
                    }));
                });

                await expect(asyncValidation.asyncValidate(values, dispatch, props)).toBeDefined();
            });
    });
