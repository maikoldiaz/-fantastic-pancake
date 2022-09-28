import { excelService } from '../../../common/services/excelService';

describe('excel Service',

    () => {
        const browseFile = { name: 'movement.xlsx' };
        const mandatorySheets = ['INVENTARIOS', 'MOVIMIENTOS'];
        const columns = {
            INVENTARIOS: ['SistemaOrigen', 'IdInventario', 'FechaInventario', 'IdNodo', 'Observaciones', 'Escenario', 'Producto', 'TipoProducto',
                'VolumenProductoNSV', 'VolumenProductoGSV', 'UnidadMedida', 'Tolerancia'],
            PROPIETARIOSINV: ['IdInventario', 'FechaInventario', 'IdNodo', 'Producto', 'IdPropietario', 'ValorPropiedad', 'UnidadValorPropiedad'],
            CALIDADINV: ['IdInventario', 'FechaInventario', 'IdNodo', 'Producto', 'IdAtributo', 'ValorAtributo', 'UnidadValorAtributo', 'DescripcionAtributo'],
            MOVIMIENTOS: ['SistemaOrigen', 'IdMovimiento', 'IdTipoMovimiento', 'FechaHoraInicial', 'FechaHoraFinal', 'OrigenMovimiento',
                'IdProductoOrigen', 'IdTipoProductoOrigen', 'DestinoMovimiento', 'IdProductoDestino', 'IdTipoProductoDestino', 'VolumenBruto',
                'VolumenNeto', 'UnidadMedida', 'Escenario', 'Observaciones', 'Clasificacion', 'Tolerancia'],
            PROPIETARIOSMOV: ['IdMovimiento', 'IdPropietario', 'ValorPropiedad', 'UnidadValorPropiedad'],
            CALIDADMOV: ['IdMovimiento', 'IdAtributo', 'ValorAtributo', 'UnidadValorAtributo', 'DescripcionAtributo']
        };

        it('should call validate excel',
            () => {
                const mock = excelService.validateExcel = jest.fn(() => browseFile, mandatorySheets, columns);
                excelService.validateExcel(browseFile, mandatorySheets, columns);
                expect(mock.mock.calls.length).toBe(1);
            });

        it('should call has records',
            () => {
                const mock = excelService.hasRecords = jest.fn(() => browseFile, mandatorySheets, columns);
                excelService.hasRecords(browseFile, mandatorySheets, columns);
                expect(mock.mock.calls.length).toBe(1);
            });

        it('should call validate columns',
            () => {
                const mock = excelService.validateColumns = jest.fn(() => browseFile, mandatorySheets, columns);
                excelService.validateColumns(browseFile, mandatorySheets, columns);
                expect(mock.mock.calls.length).toBe(1);
            });

        it('should validate excel',
            () => {
                const result = excelService.validateExcel(browseFile, mandatorySheets, columns);
                expect(result).toBeDefined();
            });
    });
