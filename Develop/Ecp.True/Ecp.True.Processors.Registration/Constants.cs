// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Registration
{
    /// <summary>
    /// The Constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Gets the node validation failed.
        /// </summary>
        /// <value>
        /// The node validation failed.
        /// </value>
        public static string NodeValidationFailed => "4004-El nodo no existe";

        /// <summary>
        /// Gets the node validation failed.
        /// </summary>
        /// <value>
        /// The node validation failed.
        /// </value>
        public static string OwnerValueUnitValidationFailed => "4006-Los registros de propiedad no tienen la misma unidad";

        /// <summary>
        /// Gets the node validation failed.
        /// </summary>
        /// <value>
        /// The node validation failed.
        /// </value>
        public static string OwnerValueUnitEmptyValidationFailed => "Los registros de propiedad deben tener unidad";

        /// <summary>
        /// Gets the node validation failed.
        /// </summary>
        /// <value>
        /// The node validation failed.
        /// </value>
        public static string OwnershipValueFailed => "4007-La sumatoria de los valores de propiedad debe ser 100%";

        /// <summary>
        /// Gets the node validation failed.
        /// </summary>
        /// <value>
        /// The node validation failed.
        /// </value>
        public static string OwnershipValueVolumeFailed => "4008-La sumatoria de los valores de propiedad debe ser igual al volumen del producto";

        /// <summary>
        /// Gets the type of the invalid data.
        /// </summary>
        /// <value>
        /// The type of the invalid data.
        /// </value>
        public static string InvalidDataType => "4010-Debe ser de tipo:";

        /// <summary>
        /// Gets the invalid data type for inventory.
        /// </summary>
        /// <value>
        /// The invalid data type for inventory.
        /// </value>
        public static string InvalidProductForNodeStorageLocation => "4005-{0} no pertenece al nodo";

        /// <summary>
        /// Gets the Movement Source Product Not Found.
        /// </summary>
        /// <value>
        /// The Source Product Search Failed.
        /// </value>
        public static string MovementSourceProductNotFound => "4011-El producto origen no pertenece al nodo origen ";

        /// <summary>
        /// Gets the Movement Destination Product Not Found.
        /// </summary>
        /// <value>
        /// The Destination Product Search Failed.
        /// </value>
        public static string MovementDestinationProductNotFound => "4012-El producto destino no pertenece al nodo destino ";

        /// <summary>
        /// Gets the Movement update Failed.
        /// </summary>
        /// <value>
        /// The MOVEMENT UPDATE NOTFOUND.
        /// </value>
        public static string MovementUpdateNotFound => "4013-El identificador del movimiento a ajustar no existe";

        /// <summary>
        /// Gets the Movement delete Failed.
        /// </summary>
        /// <value>
        /// The MOVEMENT DELETE NOTFOUND.
        /// </value>
        public static string MovementDeleteNotFound => "4014-El identificador del movimiento a anular no existe";

        /// <summary>
        /// Gets the Movement create conflict.
        /// </summary>
        /// <value>
        /// The MOVEMENT CREATE CONFLICT.
        /// </value>
        public static string MovementCreateConflict => "4015-El identificador del movimiento ya existe en el sistema";

        /// <summary>
        /// Gets the version required.
        /// </summary>
        /// <value>
        /// The the version required.
        /// </value>
        public static string VersionRequired => "La versión es obligatoria.";

        /// <summary>
        /// Gets the Movement unidentified loss validation.
        /// </summary>
        /// <value>
        /// The MOVEMENT unidentified loss VALIDATION.
        /// </value>
        public static string MovementUnIdentifiedLossNodeRequired => "4017-El movimiento de tipo perdida identificada debe tener un nodo origen o un nodo destino";

        /// <summary>
        /// Gets the nodeConnection Not Found.
        /// </summary>
        /// <value>
        /// The Node Connection not found.
        /// </value>
        public static string NodeConnectionConnectionNotFound => "4018-No existe una conexión para los nodos origen y destino recibidos";

        /// <summary>
        /// Gets the inventory create conflict.
        /// </summary>
        /// <value>
        /// The inventory create conflict.
        /// </value>
        public static string InventoryCreateConflict => "4020-El identificador del inventario ya existe en el sistema";

        /// <summary>
        /// Gets the inventory update conflict.
        /// </summary>
        /// <value>
        /// The inventory update conflict.
        /// </value>
        public static string InventoryUpdateConflict => "4021-El identificador del inventario a ajustar no existe";

        /// <summary>
        /// Gets the inventory delete conflict.
        /// </summary>
        /// <value>
        /// The inventory delete conflict.
        /// </value>
        public static string InventoryDeleteConflict => "4022-El identificador del inventario a anular no existe";

        /// <summary>
        /// Gets the NodeSegmentInvalid.
        /// </summary>
        /// <value>
        /// The Node Segment Validation failed.
        /// </value>
        public static string NodeSegmentInvalid => "4025-El segmento elegido en el sitio y los valores para segmento de los nodos origen y destino no coinciden.";

        /// <summary>
        /// Gets the invalid movement type identifier.
        /// </summary>
        /// <value>
        /// The invalid movement type identifier.
        /// </value>
        public static string InvalidMovementTypeId => "Identificador del tipo de movimiento no encontrado";

        /// <summary>
        /// Gets the invalid source product type identifier.
        /// </summary>
        /// <value>
        /// The invalid source product type identifier.
        /// </value>
        public static string InvalidSourceProductTypeId => "Identificador del tipo de producto origen no encontrado";

        /// <summary>
        /// Gets the invalid destination product type identifier.
        /// </summary>
        /// <value>
        /// The invalid destination product type identifier.
        /// </value>
        public static string InvalidDestinationProductTypeId => "Identificador del tipo de producto destino no encontrado";

        /// <summary>
        /// Gets the invalid unit identifier.
        /// </summary>
        /// <value>
        /// The invalid unit identifier.
        /// </value>
        public static string InvalidUnitId => "Identificador de la unidad no encontrado";

        /// <summary>
        /// Gets the manadatory unit identifier.
        /// </summary>
        /// <value>
        /// The manadatory unit identifier.
        /// </value>
        public static string ManadatoryUnitId => "La unidad es obligatoria";

        /// <summary>
        /// Gets the invalid owner identifier.
        /// </summary>
        /// <value>
        /// The invalid owner identifier.
        /// </value>
        public static string InvalidOwnerId => "Identificador del propietario no encontrado";

        /// <summary>
        /// Gets the invalid product type identifier.
        /// </summary>
        /// <value>
        /// The invalid product type identifier.
        /// </value>
        public static string InvalidProductTypeId => "Identificador del tipo de producto no encontrado";

        /// <summary>
        /// Gets the invalid operator identifier.
        /// </summary>
        /// <value>
        /// The invalid operator identifier.
        /// </value>
        public static string InvalidOperatorId => "Identificador del operador no encontrado";

        /// <summary>
        /// Gets the invalid system identifier.
        /// </summary>
        /// <value>
        /// The invalid system identifier.
        /// </value>
        public static string InvalidSystemId => "Identificador del sistema no encontrado";

        /// <summary>
        /// Gets the invalid source storage location identifier.
        /// </summary>
        /// <value>
        /// The invalid source storage location identifier.
        /// </value>
        public static string InvalidSourceStorageLocationId => "Identificador del almacenes origen no encontrado";

        /// <summary>
        /// Gets the invalid destination storage location identifier.
        /// </summary>
        /// <value>
        /// The invalid destination storage location identifier.
        /// </value>
        public static string InvalidDestinationStorageLocationId => "Identificador del almacenes destino no encontrado";

        /// <summary>
        /// Gets the invalid source system identifier.
        /// </summary>
        /// <value>
        /// The invalid source system identifier.
        /// </value>
        public static string InvalidSourceSystemId => "Identificador del sistema origen no encontrado";

        /// <summary>
        /// Gets the invalid segment identifier.
        /// </summary>
        /// <value>
        /// The invalid segment identifier.
        /// </value>
        public static string InvalidSegmentId => "Identificador del segmento no encontrado";

        /// <summary>
        /// Gets the invalid attribute identifier.
        /// </summary>
        /// <value>
        /// The invalid attribute identifier.
        /// </value>
        public static string InvalidAttributeId => "Identificador del atributos no encontrado";

        /// <summary>
        /// Gets the invalid attribute identifier.
        /// </summary>
        /// <value>
        /// The invalid attribute identifier.
        /// </value>
        public static string InvalidValueAttributeUnitId => "Identificador del Unidades Atributos no encontrado";

        /// <summary>
        /// Gets the Event create conflict.
        /// </summary>
        /// <value>
        /// The EVENT CREATE CONFLICT.
        /// </value>
        public static string EventCreateConflict => "El evento ya existe";

        /// <summary>
        /// Gets the Event not found.
        /// </summary>
        /// <value>
        /// The EVENT NOT FOUND.
        /// </value>
        public static string EventNotFound => "El evento no existe";

        /// <summary>
        /// Gets the event source node validation failed.
        /// </summary>
        /// <value>
        /// The event source node validation failed.
        /// </value>
        public static string EventSourceNodeValidationFailed => "Identificador del nodo origen no encontrado";

        /// <summary>
        /// Gets the event destination node validation failed.
        /// </summary>
        /// <value>
        /// The event destination node validation failed.
        /// </value>
        public static string EventDestinationNodeValidationFailed => "Identificador del nodo destino no encontrado";

        /// <summary>
        /// Gets the invalid source product.
        /// </summary>
        /// <value>
        /// The invalid source product.
        /// </value>
        public static string EventInvalidSourceProduct => "Identificador del producto origen no encontrado";

        /// <summary>
        /// Gets the invalid destination product.
        /// </summary>
        /// <value>
        /// The invalid destination product.
        /// </value>
        public static string EventInvalidDestinationProduct => "Identificador del producto destino no encontrado";

        /// <summary>
        /// Gets the event end date validation.
        /// </summary>
        /// <value>
        /// The end date validation.
        /// </value>
        public static string EventEndDateValidation => "La fecha final debe ser mayor a la fecha inicial";

        /// <summary>
        /// Gets the event end date greater now validation.
        /// </summary>
        /// <value>
        /// The end date greater now validation.
        /// </value>
        public static string EventEndDateGreaterNowValidation => "La fecha final debe ser mayor o igual a la fecha actual";

        /// <summary>
        /// Gets the event period already exist.
        /// </summary>
        /// <value>
        /// The event period already exist.
        /// </value>
        public static string EventPeriodAlreadyExists => "Ya existe un evento que contiene el periodo";

        /// <summary>
        /// Gets the Inventory Product Not Found.
        /// </summary>
        /// <value>
        /// The Product Search Failed.
        /// </value>
        public static string InventoryProductNotFound => "El producto  no pertenece al nodo";

        /// <summary>
        /// Gets the contract end date validation.
        /// </summary>
        /// <value>
        /// The end date validation.
        /// </value>
        public static string ContractEndDateValidation => "La fecha final debe ser mayor a la fecha inicial";

        /// <summary>
        /// Gets the contract end date greater now validation.
        /// </summary>
        /// <value>
        /// The end date greater now validation.
        /// </value>
        public static string ContractEndDateGreaterNowValidation => "La fecha final debe ser mayor o igual a la fecha actual";

        /// <summary>
        /// Gets the contract start date greater now less parameter validation.
        /// </summary>
        /// <value>
        /// The  start date greater now less parameter validation.
        /// </value>
        public static string ContractStartDateGreaterNowLessParameterValidation => "La fecha inicial debe ser mayor o igual a la fecha {0}";

        /// <summary>
        /// Gets the contract endate date greater now less parameter validation.
        /// </summary>
        /// <value>
        /// The  start date greater now less parameter validation.
        /// </value>
        public static string ContractEndDateGreaterNowLessParameterValidation => "La fecha final debe ser mayor o igual a la fecha {0}";

        /// <summary>
        /// Gets the event end date greater now validation.
        /// </summary>
        /// <value>
        /// The end date greater now validation.
        /// </value>
        public static string InventoryMovementCreateDateValidation => "Solo se permite el registro de información operativa del mes anterior durante los {0} primeros días del mes";

        /// <summary>
        /// Gets the official movement date validation.
        /// </summary>
        /// <value>
        /// The official movement date validation.
        /// </value>
        public static string OfficialMovementDateValidation => "No es posible registrar un movimiento oficial con fecha del mes actual.";

        /// <summary>
        /// Gets the official inventory date validation.
        /// </summary>
        /// <value>
        /// The official inventory date validation.
        /// </value>
        public static string OfficialInventoryDateValidation => "No es posible registrar un inventario oficial con fecha del mes actual.";

        /// <summary>
        /// Gets the event end date greater now validation.
        /// </summary>
        /// <value>
        /// The end date greater now validation.
        /// </value>
        public static string Movement_Create_DateValidation => "La fecha operativa del movimiento debe estar entre {0} y {1}";

        /// <summary>
        /// Gets the event end date greater now validation.
        /// </summary>
        /// <value>
        /// The end date greater now validation.
        /// </value>
        public static string Inventory_Create_DateValidation => "La fecha del inventario debe estar entre {0} y {1}";

        /// <summary>
        /// Gets the operations enddate validation.
        /// </summary>
        /// <value>
        /// The operations enddate validation.
        /// </value>
        public static string Operations_EndDate_Validation => "La fecha final de la operación debe ser mayor o igual a la fecha inicial.";

        /// <summary>
        /// Gets the Contract not found.
        /// </summary>
        /// <value>
        /// The Contract NOT FOUND.
        /// </value>
        public static string ContractNotFound => "El pedido no existe";

        /// <summary>
        /// Gets the Contract was deleted.
        /// </summary>
        /// <value>
        /// The Contract WAS DELETED.
        /// </value>
        public static string ContractWasDeleted => "El pedido ya se encuentra eliminado";

        /// <summary>
        /// Gets the Contract not found.
        /// </summary>
        /// <value>
        /// The Contract NOT FOUND.
        /// </value>
        public static string ContractAlreadyExists => "El pedido ya existe";

        /// <summary>
        /// Gets the event period already exist.
        /// </summary>
        /// <value>
        /// The event period already exist.
        /// </value>
        public static string PurchaseAndSellPeriodAlreadyExists => "Ya existe un pedido que contiene el periodo";

        /// <summary>
        /// Gets the movement contract already exist.
        /// </summary>
        /// <value>
        /// The movement contract already exist.
        /// </value>
        public static string MovementContractAlreadyExists => "El pedido ya tiene movimientos asociados y no se puede actualizar ó eliminar";

        /// <summary>
        /// Gets the inventory closing date in the past validation.
        /// </summary>
        /// <value>
        /// The inventory closing date in the past validation.
        /// </value>
        public static string Inventory_ClosingDate_InThePast_Validation => "La fecha del inventario debe ser menor a la fecha actual.";

        /// <summary>
        /// Gets the movement operational date in the past validation.
        /// </summary>
        /// <value>
        /// The movement operational date in the past validation.
        /// </value>
        public static string Movement_OperationalDate_InThePast_Validation => "La fecha operacional debe ser menor a la fecha actual.";

        /// <summary>
        /// Gets the events owner r1 notfound.
        /// </summary>
        /// <value>
        /// The events owner r1 notfound.
        /// </value>
        public static string Owner1Notfound => "Identificador del propietario 1 no encontrado";

        /// <summary>
        /// Gets the events owner r2 notfound.
        /// </summary>
        /// <value>
        /// The events owner r2 notfound.
        /// </value>
        public static string Owner2Notfound => "Identificador del propietario 2 no encontrado";

        /// <summary>
        /// Gets the invalid classification message.
        /// </summary>
        /// <value>
        /// The invalid classification message.
        /// </value>
        public static string InvalidClassificationMessage => "La clasificación solo admite los valores:  Movimiento, PerdidaIdentificada y OperacionTrazable";

        /// <summary>
        /// Gets the value Daily.
        /// </summary>
        /// <value>
        /// The value Daily.
        /// </value>
        public static string Daily => "Diario";

        /// <summary>
        /// Gets the value Biweekly.
        /// </summary>
        /// <value>
        /// The value Biweekly.
        /// </value>
        public static string Biweekly => "Quincenal";

        /// <summary>
        /// Gets the value Monthly.
        /// </summary>
        /// <value>
        /// The value Monthly.
        /// </value>
        public static string Monthly => "Mensual";

        /// <summary>
        /// Gets the value frequency not found message.
        /// </summary>
        /// <value>
        /// The value frequency not found message.
        /// </value>
        public static string ValueNotFoundFrequency => "La Frecuencia solo permite los Valores: Diario, Quincenal y Mensual";

        /// <summary>
        /// Gets the value status not found message.
        /// </summary>
        /// <value>
        /// The value status not found message.
        /// </value>
        public static string ValueNotFoundStatus => "El estado solo permite los valores : Activa o Desautorizada";

        /// <summary>
        /// Gets the value Unauthorized.
        /// </summary>
        /// <value>
        /// The value Unauthorized.
        /// </value>
        public static string Unauthorized => "Desautorizada";

        /// <summary>
        /// Gets the value Active.
        /// </summary>
        /// <value>
        /// The value Active.
        /// </value>
        public static string Active => "Activa";

        /// <summary>
        /// Gets the value Status.
        /// </summary>
        /// <value>
        /// The value Status.
        /// </value>
        public static string Status => "Status";

        /// <summary>
        /// Gets the Error Deviation Percentage.
        /// </summary>
        /// <value>
        /// The Error Deviation Percentage.
        /// </value>
        public static string ErrorDeviationPercentage => "La sumatoria de los porcentajes de propiedad esta por {0} de la desviación permitida para el segmento.";

        /// <summary>
        /// Gets the Error Deviation Distribution.
        /// </summary>
        /// <value>
        /// The Error Deviation Distribution.
        /// </value>
        public static string ErrorDeviationDistribution => "La sumatoria de la distribución de propiedad con respecto a la cantidad neta esta por {0} de la desviación permitida para el segmento.";

        /// <summary>
        /// Gets the error ownership required.
        /// </summary>
        /// <value>
        /// The error ownership required.
        /// </value>
        public static string ErrorOwnershipRequired => "Los propietarios son obligatorios.";

        /// <summary>
        /// Gets the property distribution required.
        /// </summary>
        /// <value>
        /// The property distribution required.
        /// </value>
        public static string PropertyDistribution => "La distribución de propiedad debe ser igual a cero";

        /// <summary>
        /// Gets the Over Text.
        /// </summary>
        /// <value>
        /// The Over Text.
        /// </value>
        public static string OverText => "encima";

        /// <summary>
        /// Gets the Above Text.
        /// </summary>
        /// <value>
        /// The Above Text.
        /// </value>
        public static string AboveText => "debajo";

        /// <summary>
        /// Gets the error node approved.
        /// </summary>
        /// <value>
        /// The error node approved.
        /// </value>
        public static string ErrorNodeApproved => "No es posible actualizar o eliminar un movimiento manual cuando el nodo origen o destino se encuentre aprobado o enviado a aprobación.";

        /// <summary>
        /// Gets the Error Negative Value No Annulation.
        /// </summary>
        /// <value>
        /// The Error Negative Value No Annulation.
        /// </value>
        public static string ErrorNegativeValueNoAnnulation => "Volumen neto o Volumen Bruto, negativo.cuando no es movimiento de tipo anulacion.";

        /// <summary>
        /// Gets the invalid data for inventory product.
        /// </summary>
        /// <value>
        /// The invalid data type for inventory product.
        /// </value>
        public static string OwnershipValueDecimalFailed => "El valor de propiedad es superior a 2 cifras decimales. Por favor realice el ajuste y cargue nuevamente.";
    }
}