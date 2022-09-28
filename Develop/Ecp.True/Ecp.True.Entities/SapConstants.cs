// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapConstants.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities
{
    /// <summary>
    /// The SAP Constants.
    /// </summary>
    public static class SapConstants
    {
        /// <summary>
        /// The source system name required.
        /// </summary>
        public const string SourceSystemNameRequired = "7001-El nombre del sistema origen (SourceSystem) es obligatorio";

        /// <summary>
        /// The destination system name required.
        /// </summary>
        public const string DestinationSystemNameRequired = "7002-DestinationSystem es obligatorio";

        /// <summary>
        /// The event type required.
        /// <summary>
        public const string EventTypeRequired = "7003-El tipo de evento (EventType) es obligatorio";

        /// <summary>
        /// The movement identifier is mandatory.
        /// </summary>
        public const string MovementIdentifierRequired = "7004-El identificador del movimiento (MovementId) es obligatorio";

        /// <summary>
        /// The movement type identifier is mandatory.
        /// </summary>
        public const string MovementTypeRequired = "7005-El identificador del tipo de movimiento (MovementTypeId) es obligatorio";

        /// <summary>
        /// The operational date is mandatory.
        /// </summary>
        public const string OperationDateRequired = "7006-La fecha operacional (OperationDate) es obligatoria";

        /// <summary>
        /// The period is mandatory.
        /// </summary>
        public const string PeriodIsMandatory = "7007-El periodo (Period) es obligatorio";

        /// <summary>
        /// The net standard volume is mandatory.
        /// </summary>
        public const string NetStandardQuantityIsMandatory = "7008-El volumen neto (NetStandardQuantity) es obligatorio";

        /// <summary>
        /// The segment required.
        /// </summary>
        public const string SegmentRequired = "7009-El segmento (SegmentId) es obligatorio";

        /// <summary>
        /// The movement source required.
        /// </summary>
        public const string MovementSourceRequired = "7010-El origen del movimiento (MovementSource) es obligatorio";

        /// <summary>
        /// The movement destination required.
        /// </summary>
        public const string MovementDestinationRequired = "7011-El destino del movimiento (MovementDestination) es obligatorio";

        /// <summary>
        /// The measurement unit is mandatory.
        /// </summary>
        public const string MeasurementUnitRequired = "7012-La unidad de medida del atributo (MeasurementUnit) es obligatoria";

        /// <summary>
        /// The measurement unit for backup movement is mandatory.
        /// </summary>
        public const string MeasurementUnitRequiredForBackup = "7013-La unidad de medida del movimiento de respaldo (MeasurementUnit) es obligatorio";

        /// <summary>
        /// The inventory identifier required.
        /// </summary>
        public const string InventoryIdRequired = "7014-El identificador del inventario (InventoryId) es obligatorio";

        /// <summary>
        /// The inventory date required.
        /// </summary>
        public const string InventoryDateRequired = "7015-La fecha del inventario (InventoryDate) es obligatoria";

        /// <summary>
        /// The attribute description required.
        /// </summary>
        public const string AttributeIdRequired = "7016-El identificador del atributo (AttributeId) es obligatorio";

        /// <summary>
        /// Gets the PRODUCT REQUIREDVALIDATION.
        /// </summary>
        public const string ProductsRequired = "7017-Productos (Products) es obligatorio";

        /// <summary>
        /// The attribute value required.
        /// </summary>
        public const string AttributeValueRequired = "7018-El valor del atributo (AttributeValue) es obligatorio";

        /// <summary>
        /// The value attribute unit required.
        /// </summary>
        public const string ValueAttributeUnitRequired = "7019-La unidad de medida del atributo (ValueAttributeUnit) es obligatoria";

        /// <summary>
        /// The owner identifier required.
        /// </summary>
        public const string OwnerIdRequired = "7020-El identificador del propietario (OwnerId) es obligatorio";

        /// <summary>
        /// The ownership value required.
        /// </summary>
        public const string OwnershipValueRequired = "7021-El valor de la propiedad (OwnershipValue) es obligatorio";

        /// <summary>
        /// The ownership value unit required.
        /// </summary>
        public const string OwnershipValueUnitRequired = "7022-La unidad del valor de la propiedad (OwnershipValueUnit) es obligatoria";

        /// <summary>
        /// The Classification is mandatory.
        /// </summary>
        public const string MovementClassificationIsMandatory = "7023-La clasificación del movimiento (Classification) es obligatoria";

        /// <summary>
        /// The movement start time is mandatory.
        /// </summary>
        public const string MovementStartTimeIsMandatory = "7024-La hora de inicio del movimiento (StartTime) es obligatoria";

        /// <summary>
        /// The movement end time is mandatory.
        /// </summary>
        public const string MovementEndTimeIsMandatory = "7025-La hora final del movimiento (EndTime) es obligatoria";

        /// <summary>
        /// Gets the PRODUCT ID REQUIRED VALIDATION.
        /// </summary>
        public const string ProductIdRequired = "7026-El identificador del producto (ProductId) es obligatorio";

        /// <summary>
        /// Gets the MOVEMENT SOURCE PRODUCT ID REQUIRED VALIDATION.
        /// </summary>
        public const string MovementSourceProductIdRequired = "7027-El identificador del producto origen (SourceProductId) es obligatorio";

        /// <summary>
        /// Gets the MOVEMENT SOURCE PRODUCTTYPE REQUIREDVALIDATION REQUIRED VALIDATION.
        /// </summary>
        public const string MovementSourceProductTypeIdRequired = "7028-El identificador del tipo de producto origen (SourceProductTypeId) es obligatorio";

        /// <summary>
        /// Gets the PRODUCT TYPE ID REQUIRED VALIDATION.
        /// </summary>
        public const string ProductTypeIdRequired = "7029-El identificador de tipo de producto (ProductType) es obligatorio";

        /// <summary>
        /// The products volume required.
        /// </summary>
        public const string ProductsVolumeRequired = "7030-El volumen (NetStandardQuantity) es obligatorio";

        /// <summary>
        /// The node identifier required.
        /// </summary>
        public const string NodeIdRequired = "7031-El identificador del nodo (NodeId) es obligatorio";

        /// <summary>
        /// Gets the SourceNodeId REQUIREDVALIDATION.
        /// </summary>
        public const string SourceNodeIdRequired = "7032-El nodo origen (SourceNodeId) es obligatorio";

        /// <summary>
        /// Gets the DestinationNodeId REQUIREDVALIDATION.
        /// </summary>
        public const string DestinationNodeIdRequired = "7033-El nodo destino (DestinationNodeId) es obligatorio";

        /// <summary>
        /// Gets the DestinationProductId REQUIREDVALIDATION.
        /// </summary>
        public const string DestinationProductIdRequired = "7034-El producto destino (DestinationProductId) es obligatorio";

        /// <summary>
        /// Gets the DestinationProductTypeId REQUIREDVALIDATION.
        /// </summary>
        public const string DestinationProductTypeIdRequired = "7035-El tipo de producto del producto destino (DestinationProductTypeId) es obligatorio";

        /// <summary>
        /// The observation length exceeded.
        /// </summary>
        public const string ObservationLengthExceeded = "7036-Las observaciones (Observations) pueden contener máximo 150 caracteres";

        /// <summary>
        /// The source system length exceeded.
        /// </summary>
        public const string SourceSystemLengthExceeded = "7037-El nombre del sistema origen (SourceSystem) admite hasta 25 caracteres";

        /// <summary>
        /// The destination system length exceeded.
        /// </summary>
        public const string DestinationSystemLengthExceeded = "7038-DestinationSystem admite hasta 25 caracteres";

        /// <summary>
        /// The inventory id length exceeded.
        /// </summary>
        public const string InventoryIdLengthExceeded = "7039-Identificador del inventario (InventoryId) admite hasta 50 caracteres";

        /// <summary>
        /// The event type length exceeded.
        /// </summary>
        public const string EventTypeLengthExceeded = "7040-El tipo de evento (EventType) puede contener máximo 25 caracteres";

        /// <summary>
        /// The event type length exceeded for inventory.
        /// </summary>
        public const string EventTypeLengthExceededForInventory = "7041-El tipo de evento (EventType) puede contener máximo 10 caracteres";

        /// <summary>
        /// The scenario length exceeded.
        /// </summary>
        public const string ScenarioLengthExceeded = "7042-El escenario (Scenario) puede contener máximo 50 caracteres";

        /// <summary>
        /// The movement classification length max 30 characters.
        /// </summary>
        public const string ClassificationLengthExceeded = "7043-La clasificación del movimiento (Classification) puede contener máximo 30 caracteres";

        /// <summary>
        /// The tank name length exceeded.
        /// </summary>
        public const string TankNameLengthExceeded = "7044-El Tanque (Tank) puede contener máximo 20 caracteres";

        /// <summary>
        /// The movement identifier length exceeded.
        /// </summary>
        public const string MovementIdentifierLengthExceeded = "7045-El identificador del movimiento (MovementId) admite hasta 50 caracteres";

        /// <summary>
        /// The movement identifier length exceeded.
        /// </summary>
        public const string MovementIdentifierLengthExceededForBackup = "7046-El identificador del movimiento de respaldo (BackupMovementId) admite hasta 50 caracteres";

        /// <summary>
        /// The global movement identifier length exceeded.
        /// </summary>
        public const string GlobalMovementIdentifierLengthExceeded = "7047-El identificador del movimiento global (GlobalMovementId) admite hasta 50 caracteres";

        /// <summary>
        /// The balance status length exceeded.
        /// </summary>
        public const string BalanceStatusLengthExceeded = "7048-El estado del balance (BalanceStatus) admite hasta 50 caracteres";

        /// <summary>
        /// The balance status length exceeded.
        /// </summary>
        public const string SapProcessStatusLengthExceeded = "7049-El estado del proceso (SapProcessStatus) admite hasta 50 caracteres";

        /// <summary>
        /// The movement type identifier length exceeded.
        /// </summary>
        public const string MovementTypeIdentifierLengthExceeded = "7050-El identificador del tipo de movimiento (MovementTypeId) admite hasta 150 caracteres";

        /// <summary>
        /// The measurement unit length exceeded.
        /// </summary>
        public const string MeasurementUnitLengthExceeded = "7051-La unidad de medida (MeasurementUnit) admite hasta 50 caracteres";

        /// <summary>
        /// The segment length exceeded.
        /// </summary>
        public const string SegmentLengthExceeded = "7052-El identificador del segmento (SegmentId) admite hasta 150 caracteres";

        /// <summary>
        /// The attributeId length exceeded.
        /// </summary>
        public const string AttributeIdLengthExceeded = "7053-El identificador del atributo (AttributeId) admite hasta 150 caracteres";

        /// <summary>
        /// The attribute type length exceeded.
        /// </summary>
        public const string AttributeTypeLengthExceeded = "7054-El tipo de atributo (AttributeType) admite hasta 150 caracteres";

        /// <summary>
        /// The attribute value length exceeded.
        /// </summary>
        public const string AttributeValueLengthExceeded = "7055-El valor del atributo (AttributeValue) admite hasta 150 caracteres";

        /// <summary>
        /// The value attribute unit length exceeded.
        /// </summary>
        public const string ValueAttributeUnitLengthExceeded = "7056-La unidad del valor del atributo (ValueAttributeUnit) admite hasta 50 caracteres";

        /// <summary>
        /// The attribute description length exceeded.
        /// </summary>
        public const string AttributeDescriptionLengthExceeded = "7057-La descripción del atributo (AttributeDescription) puede contener máximo 150 caracteres";

        /// <summary>
        /// The ownerId length exceeded.
        /// </summary>
        public const string OwnerIdLengthExceeded = "7058-El identificador del propietario (OwnerId) admite hasta 150 caracteres";

        /// <summary>
        /// The ownership value unit length exceeded.
        /// </summary>
        public const string OwnershipValueUnitLengthExceeded = "7059-La unidad de la propiedad (OwnershipValueUnit) admite hasta 50 caracteres";

        /// <summary>
        /// The source nodeId length exceeded.
        /// </summary>
        public const string SourceNodeIdLengthExceeded = "7060-El identificador del nodo origen (SourceNodeId) admite hasta 150 caracteres";

        /// <summary>
        /// The source storage locationId length exceeded.
        /// </summary>
        public const string SourceStorageLocationIdLengthExceeded = "7061-El identificador del almacén de origen (SourceStorageLocationId) admite hasta 150 caracteres";

        /// <summary>
        /// The source productId length exceeded.
        /// </summary>
        public const string SourceProductIdLengthExceeded = "7062-El identificador del producto origen (SourceProductId) admite hasta 150 caracteres";

        /// <summary>
        /// The source product typeId length exceeded.
        /// </summary>
        public const string SourceProductTypeIdLengthExceeded = "7063-El identificador del tipo de producto origen (SourceProductTypeId) admite hasta 150 caracteres";

        /// <summary>
        /// The productId length exceeded.
        /// </summary>
        public const string ProductIdLengthExceeded = "7064-El identificador del producto (ProductId) admite hasta 150 caracteres";

        /// <summary>
        /// The product typeId length exceeded.
        /// </summary>
        public const string ProductTypeLengthExceeded = "7065-Tipo Producto (ProductTypeId) admite hasta 150 caracteres";

        /// <summary>
        /// The source nodeId length exceeded.
        /// </summary>
        public const string DestinationNodeIdLengthExceeded = "7066-El identificador del nodo de destino (DestinationNodeId) admite hasta 150 caracteres";

        /// <summary>
        /// The source destination locationId length exceeded.
        /// </summary>
        public const string DestinationStorageLocationIdLengthExceeded = "7067-El identificador del almacén de destino (DestinationStorageLocationId) admite hasta 150 caracteres";

        /// <summary>
        /// The destination productId length exceeded.
        /// </summary>
        public const string DestinationProductIdLengthExceeded = "7068-El identificador del producto de destino (DestinationProductId) admite hasta 150 caracteres";

        /// <summary>
        /// The destination product typeId length exceeded.
        /// </summary>
        public const string DestinationProductTypeIdLengthExceeded = "7069-El identificador del tipo de producto de destino (DestinationProductTypeId) admite hasta 150 caracteres";

        /// <summary>
        /// The nodeId length exceeded.
        /// </summary>
        public const string NodeIdLengthExceeded = "7070-El identificador del nodo (NodeId) admite hasta 150 caracteres";

        /// <summary>
        /// The operator length exceeded.
        /// </summary>
        public const string OperatorLengthExceeded = "7071-El operador (OperatorId) admite hasta 150 caracteres";

        /// <summary>
        /// The more than max limit records found.
        /// </summary>
        public const string MoreThanMaxLimitRecordsFound = "7072-Solo se admiten hasta {0} registros por llamada";

        /// <summary>
        /// The ScenarioId Value Range Failed.
        /// </summary>
        public const string ScenarioIdValueRangeFailed = "7074-El escenario suministrado no es válido.";

        /// <summary>
        /// The Movement Source and destination is mandatory.
        /// </summary>
        public const string BothSourceDestinationMandatory = "7075-Es obligatorio reportar información del origen o del destino. (Ambas no pueden estar vacías).";

        /// <summary>
        /// The Batch Id length exceeded.
        /// </summary>
        public const string BatchIdLengthExceeded = "7076-El identificador del batch (BatchId) puede contener máximo 25 caracteres";

        /// <summary>
        /// The version length exceeded.
        /// </summary>
        public const string VersionLengthExceeded = "7077-Versión (Version) admite hasta 50 caracteres";

        /// <summary>
        /// The IsOfficial is mandatory.
        /// </summary>
        public const string IsOfficialIsMandatory = "7078-Es Oficial (IsOfficial) es obligatorio";

        /// <summary>
        /// The system length exceeded.
        /// </summary>
        public const string SystemLengthExceeded = "7079-Sistema (System) admite hasta 150 caracteres";

        /// <summary>
        /// The GlobalMovementId is mandatory.
        /// </summary>
        public const string GlobalMovementIdIsMandatory = "7080-Id Movimiento Global (GlobalMovementId) es obligatorio";

        /// <summary>
        /// The Official Information key.
        /// </summary>
        public const string OfficialInformationKey = "OfficialInformation";

        /// <summary>
        /// The system key.
        /// </summary>
        public const string SystemKey = "System";

        /// <summary>
        /// The tank key.
        /// </summary>
        public const string TankKey = "Tank";

        /// <summary>
        /// The uncertainty key.
        /// </summary>
        public const string UncertaintyKey = "Uncertainty";

        /// <summary>
        /// The net standard quantity key.
        /// </summary>
        public const string NetStandardQuantityKey = "NetStandardQuantity";

        /// <summary>
        /// The gross standard quantity key.
        /// </summary>
        public const string GrossStandardQuantityKey = "GrossStandardQuantity";

        /// <summary>
        /// The MovementDestination key.
        /// </summary>
        public const string MovementDestination = "MovementDestination";

        /// <summary>
        /// The MovementSource key.
        /// </summary>
        public const string MovementSource = "MovementSource";

        /// <summary>
        /// The DestinationProductId key.
        /// </summary>
        public const string DestinationProductId = "DestinationProductId";

        /// <summary>
        /// The SourceProductId key.
        /// </summary>
        public const string SourceProductId = "SourceProductId";

        /// <summary>
        /// The GlobalMovementId is mandatory.
        /// </summary>
        public const string DestinationProductIdIsMandatory = "7081-Id Producto Destino (DestinationProductId) es obligatorio";

        /// <summary>
        /// The is official.
        /// </summary>
        public const string IsOfficial = "IsOfficial";

        /// <summary>
        /// The backup movement identifier.
        /// </summary>
        public const string BackupMovementId = "BackupMovementId";

        /// <summary>
        /// The global movement identifier.
        /// </summary>
        public const string GlobalMovementId = "GlobalMovementId";

        /// <summary>
        /// The source system.
        /// </summary>
        public const string SourceSystem = "SourceSystem";

        /// <summary>
        /// The scenario required.
        /// </summary>
        public const string ScenarioRequired = "7082-El escenario (ScenarioId) es obligatorio";

        /// <summary>
        /// The source system required.
        /// </summary>
        public const string SourceSystemRequired = "7083-El campo 'SOURCESYSTEM' es obligatorio";

        /// <summary>
        /// The source system length exceeded.
        /// </summary>
        public const string SourceSystemLengthExceededPurchaseSale = "7084-El campo (SOURCESYSTEM) admite hasta 20 caracteres";

        /// <summary>
        /// The date receivied PO required.
        /// </summary>
        public const string DateReceivedPoRequired = "7085-El campo 'DATERECEIVEDPO' es obligatorio";

        /// <summary>
        /// The event sap PO required.
        /// </summary>
        public const string EventSapPoRequired = "7086-El campo 'EVENT_SAPPO' es obligatorio";

        /// <summary>
        /// The Event Sap Po length exceeded.
        /// </summary>
        public const string EventSapPoLengthExceeded = "7087-El campo (EVENT_SAPPO) admite hasta 10 caracteres";

        /// <summary>
        /// The message id required.
        /// </summary>
        public const string MessageIdRequired = "7088-El campo 'MESSAGEID' es obligatorio";

        /// <summary>
        /// The Number Order required.
        /// </summary>
        public const string NumberOrderRequired = "7089-El campo 'NUMBERORDER' es obligatorio";

        /// <summary>
        /// The Number Order length exceeded.
        /// </summary>
        public const string NumberOrderLengthExceeded = "7090-El campo (NUMBERORDER) admite hasta 10 caracteres";

        /// <summary>
        /// The Type Order required.
        /// </summary>
        public const string TypeOrderRequired = "7091-El campo 'TYPEORDER' es obligatorio";

        /// <summary>
        /// The Type Order length exceeded.
        /// </summary>
        public const string TypeOrderLengthExceeded = "7092-El campo (TYPEORDER) admite hasta 4 caracteres";

        /// <summary>
        /// The Organization Id length exceeded.
        /// </summary>
        public const string OrganizationIdLengthExceeded = "7093-El campo (ORGANIZATIONID) admite hasta 40 caracteres";

        /// <summary>
        /// The Client Id required.
        /// </summary>
        public const string ClientIdRequired = "7094-El campo 'CLIENTID' es obligatorio";

        /// <summary>
        /// The Position Id required.
        /// </summary>
        public const string PositionIdRequired = "7095-El campo 'ID_POSITION' es obligatorio";

        /// <summary>
        /// The Material length exceeded.
        /// </summary>
        public const string MaterialLengthExceeded = "7096-El campo (MATERIAL) admite hasta 40 caracteres";

        /// <summary>
        /// The Material required.
        /// </summary>
        public const string MaterialRequired = "7097-El campo 'MATERIAL' es obligatorio";

        /// <summary>
        /// The Quantity length exceeded.
        /// </summary>
        public const string QuantityLengthExceeded = "7098-El campo (QUANTITY) admite hasta 19 caracteres";

        /// <summary>
        /// The Quantity Oum required.
        /// </summary>
        public const string QuantityRequired = "7099-El campo 'QUANTITY' es obligatorio";

        /// <summary>
        /// The Quantity Oum length exceeded.
        /// </summary>
        public const string QuantityOumLengthExceeded = "7100-El campo (QUANTITYOUM) admite hasta 3 caracteres";

        /// <summary>
        /// The Quantity required.
        /// </summary>
        public const string QuantityOumRequired = "7101-El campo 'QUANTITYOUM' es obligatorio";

        /// <summary>
        /// The Start Time required.
        /// </summary>
        public const string StartTimeRequired = "7102-El campo 'STARTTIME' es obligatorio";

        /// <summary>
        /// The End Time required.
        /// </summary>
        public const string EndTimeRequired = "7103-El campo 'ENDTIME' es obligatorio";

        /// <summary>
        /// The Destination Location length exceeded.
        /// </summary>
        public const string DestinationLocationLengthExceeded = "7104-El campo (DESTINATIONLOCATIONID) admite hasta 40 caracteres";

        /// <summary>
        /// The Destination Location required.
        /// </summary>
        public const string DestinationLocationIdRequired = "7105-El campo 'DESTINATIONLOCATIONID' es obligatorio";

        /// <summary>
        /// The Destination Location length exceeded.
        /// </summary>
        public const string DestinationStorageLocationIdLengthExceededSale = "7106-El campo (DESTINATIONSTORAGELOCATIONID) admite hasta 3 caracteres";

        /// <summary>
        /// The Destination Location required.
        /// </summary>
        public const string DestinationStorageLocationIdRequired = "7107-El campo 'DESTINATIONSTORAGELOCATIONID' es obligatorio";

        /// <summary>
        /// The more than max limit records found.
        /// </summary>
        public const string MoreThanMaxLimitPositionsFound = "7108-Solo se admiten hasta {0} registros por llamada";

        /// <summary>
        /// The more than max limit records found.
        /// </summary>
        public const string MoreThanMaxLimitPositionsCode = "7108";

        /// <summary>
        /// The event required.
        /// </summary>
        public const string EventRequired = "7109-El campo EVENT es obligatorio";

        /// <summary>
        /// The message identifier length exceeded.
        /// </summary>
        public const string MessageIdLength = "7110-El campo MESSAGEID admite hasta 32 caracteres";

        /// <summary>
        /// The purchase orders required.
        /// </summary>
        public const string PurchaseOrdersRequired = "7111-El campo 'PURCHASEORDERS' es obligatorio";

        /// <summary>
        /// The purchase order required.
        /// </summary>
        public const string PurchaseOrderRequired = "7112-El campo 'PURCHASEORDER' es obligatorio";

        /// <summary>
        /// The purchase order identifier required.
        /// </summary>
        public const string PurchaseOrderIdRequired = "7113-El campo 'PURCHASEORDERID' es obligatorio";

        /// <summary>
        /// The purchase order type required.
        /// </summary>
        public const string PurchaseOrderTypeRequired = "7114-El campo 'PURCHASEORDERTYPE' es obligatorio";

        /// <summary>
        /// The purchase items required.
        /// </summary>
        public const string PurchaseItemsRequired = "7115-El campo 'PURCHASEITEMS' es obligatorio";

        /// <summary>
        /// The purchase item required.
        /// </summary>
        public const string PurchaseItemRequired = "7116-El campo 'PURCHASEITEM' es obligatorio";

        /// <summary>
        /// The tolerance required.
        /// </summary>
        public const string ToleranceRequired = "7118-El campo 'TOLERANCE' es obligatorio";

        /// <summary>
        /// The name required.
        /// </summary>
        public const string NameRequired = "7119-El campo 'NAME' es obligatorio";

        /// <summary>
        /// The start period required.
        /// </summary>
        public const string StartPeriodRequired = "7120-El campo 'STARTPERIOD' es obligatorio";

        /// <summary>
        /// The end period required.
        /// </summary>
        public const string EndPeriodRequired = "7121-El campo 'ENDPERIOD' es obligatorio";

        /// <summary>
        /// The value required.
        /// </summary>
        public const string ValueRequired = "7122-El campo 'VALUE' es obligatorio";

        /// <summary>
        /// The property required.
        /// </summary>
        public const string PropertyRequired = "7123-El campo 'PROPERTY' es obligatorio";

        /// <summary>
        /// The uom required.
        /// </summary>
        public const string UomRequired = "7124-El campo 'UOM' es obligatorio";

        /// <summary>
        /// The period required.
        /// </summary>
        public const string PeriodRequired = "7125-El campo 'PERIOD' es obligatorio";

        /// <summary>
        /// The status required.
        /// </summary>
        public const string StatusRequired = "7126-El campo 'STATUS' es obligatorio";

        /// <summary>
        /// The provider required.
        /// </summary>
        public const string ProviderRequired = "7127-El campo Provider es obligatorio";

        /// <summary>
        /// The owner required.
        /// </summary>
        public const string OwnerRequired = "7128-El campo OWNER es obligatorio";

        /// <summary>
        /// The society required.
        /// </summary>
        public const string SocietyRequired = "7129-El campo SOCIETY es obligatorio";

        /// <summary>
        /// The identifier required.
        /// </summary>
        public const string IdRequired = "7131-El campo ID es obligatorio";

        /// <summary>
        /// The commodity required.
        /// </summary>
        public const string CommodityRequired = "7132-El campo COMMODITY es obligatorio";

        /// <summary>
        /// The locations required.
        /// </summary>
        public const string LocationsRequired = "7133-El campo LOCATIONS es obligatorio";

        /// <summary>
        /// The destination required.
        /// </summary>
        public const string DestinationRequired = "7134-El campo DESTINATION es obligatorio";

        /// <summary>
        /// The criterion required.
        /// </summary>
        public const string CriterionRequired = "7135-El campo CRITERION es obligatorio";

        /// <summary>
        /// The maximum two decimals required.
        /// </summary>
        public const string MaxTwoDecimals = "7136-Admite máximo dos decimales.";

        /// <summary>
        /// The expression regular for maximum two decimals.
        /// </summary>
        public const string ExpresionMaxTwoDecimals = @"^\d+.?\d{0,2}$";

        /// <summary>
        /// The name length exceeded.
        /// </summary>
        public const string NameLength = "7137-El campo NAME admite hasta 20 caracteres";

        /// <summary>
        /// The end period should greather than start period.
        /// </summary>
        public const string EndPeriodGreaterThanStartPeriod = "7138-EndPeriod debe ser mayor que StartPeriod";

        /// <summary>
        /// The destination location required.
        /// </summary>
        public const string DestinationLocationRequired = "7139-El campo DESTINATIONLOCATION es obligatorio";

        /// <summary>
        /// The destination node required.
        /// </summary>
        public const string DestinationNodeRequired = "7140-El campo DESTINATIONNODE es obligatorio";

        /// <summary>
        /// The Default Frequency Sales.
        /// </summary>
        public const string DefaultFrequencySales = "Mensual";

        /// <summary>
        /// The TypeOrderTraslados.
        /// </summary>
        public const string TypeOrderTransfer = "Traslados";

        /// <summary>
        /// The TypeOrderCompra.
        /// </summary>
        public const string TypeOrderPurchase = "Compra";

        /// <summary>
        /// The TypeOrderAutoConsumoZaut.
        /// </summary>
        public const string TypeOrderSelfConsumptionCode = "ZAUT";

        /// <summary>
        /// The TypeOrderAutoConsumoZaut1.
        /// </summary>
        public const string TypeOrderSelfConsumptionLlenader = "ZAUT1";

        /// <summary>
        /// The Default Frequency Sales.
        /// </summary>
        public const string DefaultFrequency = "Mensual";

        /// <summary>
        /// The Biweekly Frequency Sales.
        /// </summary>
        public const string BiweeklyFrequency = "Quincenal";

        /// <summary>
        /// The Daily Frequency Sales.
        /// </summary>
        public const string DailyFrequency = "Diario";

        /// <summary>
        /// The property purchase-percentage.
        /// </summary>
        public const string PropertyPurchasePercentage = "Compra-Porcentaje";

        /// <summary>
        /// The property purchase-volume.
        /// </summary>
        public const string PropertyPurchaseVolume = "Compra-Volumen";

        /// <summary>
        /// The property invalid.
        /// </summary>
        public const string PropertyInvalid = "7141-El campo Property solo admite los valores Compra-Porcentaje o Compra-Volumen";

        /// <summary>
        /// The value invalid for purchase-percentage.
        /// </summary>
        public const string ValueInvalidForPurchasePercentage = "7142-El campo Value no es válido para Compra-Porcentaje";

        /// <summary>
        /// The uom invalid for purchase-percentage.
        /// </summary>
        public const string UomInvalidForPurchasePercentage = "7143-El campo Uom no es válido para Compra-Porcentaje";

        /// <summary>
        /// The value uom for purchase-percentage.
        /// </summary>
        public const string ValueUomForPurchasePercentage = "%";

        /// <summary>
        /// The event length exceeded to 21.
        /// </summary>
        public const string EventLengthExceeded = "7145-El campo EVENT admite hasta 21 caracteres";

        /// <summary>
        /// The status active.
        /// </summary>
        public const string StatusActive = "Activa";

        /// <summary>
        /// The status unauthorized.
        /// </summary>
        public const string StatusUnauthorized = "Desautorizada";

        /// <summary>
        /// The status invalid.
        /// </summary>
        public const string StatusInvalid = "7146-El campo Status es inválido";

        /// <summary>
        /// The TypeOrder Autoconsumo.
        /// </summary>
        public const string TypeOrderSelfConsumption = "Autoconsumo";

        /// <summary>
        /// The TypeOrder Venta.
        /// </summary>
        public const string TypeOrderSale = "Venta";

        /// <summary>
        /// The EvenSap Create.
        /// </summary>
        public const string EventSapCreate = "Crear";

        /// <summary>
        /// The EvenSap Update.
        /// </summary>
        public const string EventSapUpdate = "Modificar";

        /// <summary>
        /// The EvenSap Delete.
        /// </summary>
        public const string EventSapDelete = "Eliminar";

        /// <summary>
        /// The positionStatus Invalid.
        /// </summary>
        public const string InvalidPositionStatus = "7147-El campo PositionStatus no admite el valor enviado";

        /// <summary>
        /// The Event Invalid.
        /// </summary>
        public const string InvalidEventSapPo = "7149-El campo 'EVENT' no admite el valor enviado";

        /// <summary>
        /// The StatusMessage is required.
        /// </summary>
        public const string StatusMessageRequired = "7150-El campo 'StatusMessage' es requerido";

        /// <summary>
        /// The StatusMessage length exceeded.
        /// </summary>
        public const string StatusMessageLengthExceeded = "7151-El campo (StatusMessage) admite hasta 5 caracteres";

        /// <summary>
        /// The IdMessage is required.
        /// </summary>
        public const string IdMessageRequired = "7152-El campo 'IdMessage' es requerido";

        /// <summary>
        /// The IdMessage length exceeded.
        /// </summary>
        public const string IdMessageLengthExceeded = "7153-El campo (IdMessage) admite hasta 20 caracteres";

        /// <summary>
        /// The Information is required.
        /// </summary>
        public const string InformationRequired = "7154-El campo 'Information' es requerido";

        /// <summary>
        /// The Information length exceeded.
        /// </summary>
        public const string InformationLengthExceeded = "7155-El campo (Information) admite hasta 200 caracteres";

        /// <summary>
        /// The MovementId is required.
        /// </summary>
        public const string MovementIdRequired = "7156-El campo 'MovementId' es requerido";

        /// <summary>
        /// The MovementId length exceeded.
        /// </summary>
        public const string MovementIdLengthExceeded = "7157-El campo (MovementId) admite hasta 20 caracteres";

        /// <summary>
        /// The DestinationSystem is required.
        /// </summary>
        public const string DestinationSystemRequired = "7158-El campo 'DestinationSystem' es requerido";

        /// <summary>
        /// The DestinationSystem length exceeded.
        /// </summary>
        public const string SapDestinationSystemLengthExceeded = "7159-El campo (DestinationSystem) admite hasta 20 caracteres";

        /// <summary>
        /// The DateReceivedSystem is required.
        /// </summary>
        public const string DateReceivedSystemRequired = "7160-El campo 'DateReceivedSystem' es requerido";

        /// <summary>
        /// The SourceSystem length exceeded.
        /// </summary>
        public const string SapSourceSystemLengthExceeded = "7161-El campo (SourceSystem) admite hasta 20 caracteres";

        /// <summary>
        /// The TransactionId is required.
        /// </summary>
        public const string TransactionIdRequired = "7162-El campo (TransactionId) es requerido";

        /// <summary>
        /// The TransactionId length exceeded.
        /// </summary>
        public const string TransactionIdLengthExceeded = "7163-El campo (TransactionId) admite hasta 10 caracteres";

        /// <summary>
        /// The facilities required.
        /// </summary>
        public const string FacilitiesRequired = "7164-El campo FACILITIES es obligatorio";

        /// <summary>
        /// The facility required.
        /// </summary>
        public const string FacilityRequired = "7165-El campo FACILITY es obligatorio";

        /// <summary>
        /// The DestinationSystem length exceeded.
        /// </summary>
        public const string SaleDestinationSystemLengthExceeded = "7166-El campo (DestinationSystem) admite hasta 10 caracteres";

        /// <summary>
        /// The DateOrder length exceeded.
        /// </summary>
        public const string DateOrderRequired = "7167-El campo DATEORDER es obligatorio";

        /// <summary>
        /// The Invalid Frequency Sales.
        /// </summary>
        public const string InvalidFrequency = "7164-El campo Frequency solo admite los valores Diario, Mensual, Quincenal o el campo vacío";

        /// <summary>
        /// The DescriptionStatus length exceeded.
        /// </summary>
        public const string DescriptionStatusLengthExceeded = "7165-El campo (DESCRIPTIONSTATUS) admite hasta 20 caracteres";

        /// <summary>
        /// The system required.
        /// </summary>
        public const string SystemRequired = "7166-El campo 'SYSTEM' es obligatorio";

        /// <summary>
        /// The system length exceeded.
        /// </summary>
        public const string SystemLengthExceededPurchase = "7167-El campo (SYSTEM) admite hasta 20 caracteres";

        /// <summary>
        /// The logistic movement not found.
        /// </summary>
        public const string LogisticMomeventNotFound = "7168-No se encontró un movimiento logístico con el MovementId: {0}";

        /// <summary>
        /// The logistic movement is invalid.
        /// </summary>
        public const string LogisticMomeventIsInvalidStatus = "7169-El movimiento logístico no cumple con los parametros para ser procesado";
    }
}
