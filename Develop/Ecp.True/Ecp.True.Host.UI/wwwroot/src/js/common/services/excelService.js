import XLSX from 'xlsx';
import { utilities } from '../../common/services/utilities.js';
import { resourceProvider } from '../../common/services/resourceProvider.js';
import { excelFactory } from './excelFactory.js';

const excelService = (function () {
    const getHeaderRow = sheet => {
        const columnHeaders = [];
        const rowValues = [];
        const finalexcelData = [];
        const range = XLSX.utils.decode_range(sheet['!ref']);
        for (let C = range.s.c; C <= range.e.c; ++C) {
            const cellref = XLSX.utils.encode_cell({ c: C, r: 0 });
            if (cellref && sheet[cellref]) {
                const hdr = XLSX.utils.format_cell(sheet[cellref]);
                columnHeaders.push(hdr);
            }

            for (let R = range.s.r; R <= range.e.r; ++R) {
                const cellrowref = XLSX.utils.encode_cell({ c: C, r: R });
                if (cellrowref && sheet[cellrowref] && sheet[cellrowref].v) {
                    rowValues.push(sheet[cellrowref].v);
                }
            }
        }
        finalexcelData.push(columnHeaders);
        finalexcelData.push(rowValues);
        return finalexcelData;
    };

    const validateColumns = (resultColumnData, sheetname, columns) => {
        let missingColumnNames = [];
        Object.keys(columns).forEach(function (key) {
            if (key === sheetname) {
                missingColumnNames = utilities.validateMissingValuesInArray(resultColumnData, columns[key]);
            }
        });
        return missingColumnNames;
    };

    const hasRecords = (data, sheetname, mandatorySheets) => {
        let isDataExist = false;
        mandatorySheets.forEach(element => {
            if (element === sheetname && data[1].length > data[0].length) {
                isDataExist = true;
            }
        });
        return isDataExist;
    };

    const validateExcel = (file, currentSystemType, onValidate) => {
        let validationResult;
        const { mandatorySheets, columns } = excelFactory.getExcelFileInfo(currentSystemType);
        try {
            if (file.name.substring(file.name.lastIndexOf('.') + 1).toLocaleLowerCase() === 'xlsx') {
                const reader = new FileReader();
                let hasMinimumRecords = false;
                reader.readAsArrayBuffer(file);
                reader.onload = l => {
                    const workbook = XLSX.read(l.target.result, { type: 'array' });
                    if (mandatorySheets.some(r => workbook.SheetNames.includes(r))) {
                        workbook.SheetNames.forEach(n => {
                            const ws = workbook.Sheets[n];
                            const resultFileData = getHeaderRow(ws);
                            const missingColumnNames = validateColumns(resultFileData[0], n, columns);
                            if (missingColumnNames.length > 0) {
                                validationResult = { status: false, message: resourceProvider.read('sourceColumnNotFound').replace('{0}', missingColumnNames) };
                                onValidate(validationResult);
                                throw new Error(resourceProvider.read('sourceColumnNotFound').replace('{0}', missingColumnNames));
                            }
                            if (hasRecords(resultFileData, n, mandatorySheets)) {
                                hasMinimumRecords = true;
                            }
                        });
                        if (!hasMinimumRecords) {
                            validationResult = { status: false, message: resourceProvider.read('fileMinimumRowsNotValid') };
                            onValidate(validationResult);
                            throw new Error(resourceProvider.read('fileMinimumRowsNotValid'));
                        }

                        validationResult = { status: true };
                        onValidate(validationResult);
                        return;
                    }

                    validationResult = { status: false, message: resourceProvider.read('fileMinimumSheetNotFound') };
                    onValidate(validationResult);
                };
            } else {
                validationResult = { status: false, message: resourceProvider.read('fileExtensionNotValid') };
                onValidate(validationResult);
            }
        } catch (e) {
            validationResult = { status: false, message: e.message };
            onValidate(validationResult);
        }
    };

    return {
        validateExcel: (file, currentSystemType, onValidate) => {
            return validateExcel(file, currentSystemType, onValidate);
        }
    };
}());

export { excelService };
