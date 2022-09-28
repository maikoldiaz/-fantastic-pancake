import { constants } from './constants';
import { utilities } from './utilities';

const excelFactory = (function () {
    const excelFileInfo = {
        excel: {
            mandatorySheets: ['INVENTARIOS', 'MOVIMIENTOS'],
            columns: {
                INVENTARIOS: ['SistemaOrigen', 'IdInventario', 'FechaInventario', 'IdNodo', 'Tanque', 'BatchId', 'Sistema', 'Observaciones', 'IdEscenario', 'Producto', 'TipoProducto',
                    'CantidadBruta', 'CantidadNeta', 'UnidadMedida', 'Incertidumbre', 'Version', 'Operador'],
                PROPIETARIOSINV: ['IdInventario', 'FechaInventario', 'IdNodo', 'Tanque', 'BatchId', 'Producto', 'IdPropietario', 'ValorPropiedad', 'UnidadValorPropiedad'],
                CALIDADINV: ['IdInventario', 'FechaInventario', 'IdNodo', 'Tanque', 'BatchId', 'Producto', 'IdAtributo', 'TipoAtributo', 'ValorAtributo',
                    'UnidadValorAtributo', 'DescripcionAtributo'],
                MOVIMIENTOS: ['SistemaOrigen', 'IdMovimiento', 'BatchId', 'IdTipoMovimiento', 'Sistema', 'FechaHoraInicial', 'FechaHoraFinal', 'OrigenMovimiento',
                    'IdProductoOrigen', 'IdTipoProductoOrigen', 'DestinoMovimiento', 'IdProductoDestino', 'IdTipoProductoDestino', 'CantidadBruta',
                    'CantidadNeta', 'UnidadMedida', 'IdEscenario', 'Version', 'Observaciones', 'Clasificacion', 'Incertidumbre', 'Operador'],
                PROPIETARIOSMOV: ['IdMovimiento', 'IdPropietario', 'ValorPropiedad', 'UnidadValorPropiedad'],
                CALIDADMOV: ['IdMovimiento', 'IdAtributo', 'TipoAtributo', 'ValorAtributo', 'UnidadValorAtributo', 'DescripcionAtributo']
            }
        },
        contracts: {
            mandatorySheets: ['Pedidos'],
            columns: {
                Pedidos: ['NumeroDocumento', 'Posicion', 'Tipo', 'NodoOrigen', 'NodoDestino', 'Producto', 'FechaInicio', 'FechaFin',
                    'Valor', 'Unidad', 'Propietario1', 'Propietario2', 'TipoOrden', 'Estado', 'VolumenPresupuestado', 'Porcentajetolerancia', 'Frecuencia']
            }
        },
        events: {
            mandatorySheets: ['Eventos'],
            columns: {
                Eventos: ['EventoPropiedad', 'NodoOrigen', 'NodoDestino', 'ProductoOrigen', 'ProductoDestino', 'Fecha Inicio', 'FechaFin', 'Propietario1', 'ValorDiario',
                    'Unidad', 'Propietario2']
            }
        }
    };

    const systemTypeExcelInfo = {
        [constants.SystemType.EXCEL]: excelFileInfo.excel,
        [constants.SystemType.CONTRACT]: excelFileInfo.contracts,
        [constants.SystemType.EVENTS]: excelFileInfo.events
    };

    return {
        getExcelFileInfo: function (currentSystemType) {
            return !utilities.isNullOrUndefined(systemTypeExcelInfo[currentSystemType]) ? systemTypeExcelInfo[currentSystemType] : excelFileInfo.excel;
        }
    };
}());

export { excelFactory };
