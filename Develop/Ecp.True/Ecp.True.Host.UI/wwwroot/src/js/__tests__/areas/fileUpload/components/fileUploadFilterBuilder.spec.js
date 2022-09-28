import { fileUploadFilterBuilder } from '../../../../modules/transportBalance/fileUploads/fileUploadFilterBuilder';
import { dateService } from '../../../../common/services/dateService';

describe('fileUploadFilterBuilder', () =>{
    it('should return no filter with empty values', () =>{
        const values = {};
        const actualFilterQuery = fileUploadFilterBuilder.build(values);
        const defaultFilterDate = dateService.parseToFilterString(-1);
        expect(actualFilterQuery).toEqual(null);
    });

    it('should handle final Date filter', () =>{
        const values = {
            finalDate: '12/12/2019'
        };
        const actualFilterQuery = fileUploadFilterBuilder.build(values);
        expect(actualFilterQuery.trim()).toEqual('createdDate le 2019-12-13T00:00:00.000Z');
    });

    it('should handle state filter', () =>{
        const values = {
            state: { value: 'Processed' }
        };
        const actualFilterQuery = fileUploadFilterBuilder.build(values);
        expect(actualFilterQuery.trim()).toEqual('status eq \'Processed\'');
    });

    it('should handle user filter', () =>{
        const values = {
            username: { name: 'testUser' }
        };
        const actualFilterQuery = fileUploadFilterBuilder.build(values);
        expect(actualFilterQuery.trim()).toEqual('createdBy eq \'testUser\'');
    });

    it('should handle action filter', () =>{
        const values = {
            action: { value: 'testActionValue' }
        };
        const actualFilterQuery = fileUploadFilterBuilder.build(values);
        expect(actualFilterQuery.trim()).toEqual('actionType eq \'testActionValue\'');
    });
});

