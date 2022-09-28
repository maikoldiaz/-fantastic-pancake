import * as actions from '../../modules/transportBalance/fileUploads/actions';
import { fileRegistration } from '../../modules/transportBalance/fileUploads/reducers';

describe('reducer test for file upload', () => {
    const initialState = {
        state: {
            0: 'Finalizado',
            1: 'Procesando'
        },
        selectedActionType: 1,
        resetFormToggler: false,
        receiveStatusToggler: false,
        receiveAccessInfoToggler: false,
        browseFile: {},
        validationResult: { success: true },
        excelFileInfo: {
            mandatorySheets: ['INVENTARIOS', 'MOVIMIENTOS'],
            Columns: {
                INVENTARIOS: ['SistemaOrigen', 'IdInventario', 'FechaInventario', 'IdNodo', 'Observaciones', 'Escenario', 'Producto', 'TipoProducto',
                    'VolumenProductoNSV', 'VolumenProductoGSV', 'UnidadMedida', 'Tolerancia'],
                PROPIETARIOSINV: ['IdInventario', 'FechaInventario', 'IdNodo', 'Producto', 'IdPropietario', 'ValorPropiedad', 'UnidadValorPropiedad'],
                CALIDADINV: ['IdInventario', 'FechaInventario', 'IdNodo', 'Producto', 'IdAtributo', 'ValorAtributo', 'UnidadValorAtributo', 'DescripcionAtributo'],
                MOVIMIENTOS: ['SistemaOrigen', 'IdMovimiento', 'IdTipoMovimiento', 'FechaHoraInicial', 'FechaHoraFinal', 'OrigenMovimiento',
                    'IdProductoOrigen', 'IdTipoProductoOrigen', 'DestinoMovimiento', 'IdProductoDestino', 'IdTipoProductoDestino', 'VolumenBruto',
                    'VolumenNeto', 'UnidadMedida', 'Escenario', 'Observaciones', 'Clasificacion', 'Tolerancia'],
                PROPIETARIOSMOV: ['IdMovimiento', 'IdPropietario', 'ValorPropiedad', 'UnidadValorPropiedad'],
                CALIDADMOV: ['IdMovimiento', 'IdAtributo', 'ValorAtributo', 'UnidadValorAtributo', 'DescripcionAtributo']
            }
        }
    };

    it('should handle Add New File',
        function () {
            const fileDetail = {
                name: 'book.xls'
            };
            const action = {
                type: actions.FILE_REGISTRATION_ADD_FILE,
                data: fileDetail
            };
            const newState = Object.assign({}, initialState, { isValidSelection: true, browseFile: action.data });
            expect(fileRegistration(initialState, action)).toEqual(newState);
        });

    it('should receive file upload status',
        function () {
            const action = {
                type: actions.RECEIVE_FILE_REGISTRATION_UPLOAD_STATUS,
                status: 'file status'
            };
            const newState = Object.assign({}, initialState, { receiveStatusToggler: action.status ? !initialState.receiveStatusToggler : initialState.receiveStatusToggler });
            expect(fileRegistration(initialState, action)).toEqual(newState);
        });

    it('should receive upload file access info',
        function () {
            const action = {
                type: actions.RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO,
                status: 'file status'
            };
            const newState = Object.assign({}, initialState, { uploadAccessInfo: action.accessInfo, receiveAccessInfoToggler: !initialState.receiveAccessInfoToggler });
            expect(fileRegistration(initialState, action)).toEqual(newState);
        });

    it('should receive upload file read info',
        function () {
            const action = {
                type: actions.RECEIVE_FILEREGISTRATION_READACCESSINFO,
                status: 'file status'
            };
            const newState = Object.assign({}, initialState, { readAccessInfo: action.accessInfo });
            expect(fileRegistration(initialState, action)).toEqual(newState);
        });

    it('should receive file registration validation',
        function () {
            const action = {
                type: actions.ON_FILEREGISTRATION_VALIDATION,
                status: 'file status'
            };
            const newState = Object.assign({}, initialState, { validationResult: action.validationResult });
            expect(fileRegistration(initialState, action)).toEqual(newState);
        });

    it('should reinject file registration ',
        function () {
            const action = {
                type: actions.ON_FILEREGISTRATION_REINJECT,
                status: 'file status'
            };
            const newState = Object.assign({}, initialState, { reInjectFileInfo: action.reInjectFileInfo });
            expect(fileRegistration(initialState, action)).toEqual(newState);
        });

    it('should register upload popup ',
        function () {
            const action = {
                type: actions.RESET_FILEREGISTRATION_UPLOADPOPUP,
                status: 'file status'
            };
            const newState = Object.assign({}, initialState, { reInjectFileInfo: {}, validationResult: {}, isValidSelection: false, browseFile: {}, selectedFileType: null });
            expect(fileRegistration(initialState, action)).toEqual(newState);
        });

    it('should return default state',
        function () {
            const action = {
                type: 'dummy Value'
            };
            const newState = Object.assign({}, initialState);
            expect(fileRegistration(initialState, action)).toEqual(newState);
        });
});
