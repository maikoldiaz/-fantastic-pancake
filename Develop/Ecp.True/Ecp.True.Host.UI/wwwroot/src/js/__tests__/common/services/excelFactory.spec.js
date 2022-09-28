import { excelFactory } from '../../../common/services/excelFactory';

describe('excel Factory',

    () => {
        it('should get the excel file info for system type excel',
            () => {
                const excel = excelFactory.getExcelFileInfo('EXCEL');
                expect(excel).toBeDefined();
            });

            it('should get the excel file info for system type contract',
            () => {
                const excel = excelFactory.getExcelFileInfo('CONTRACT');
                expect(excel).toBeDefined();
            });

            it('should get the excel file info for system type event',
            () => {
                const excel = excelFactory.getExcelFileInfo('EVENTS');
                expect(excel).toBeDefined();
            });
    });
