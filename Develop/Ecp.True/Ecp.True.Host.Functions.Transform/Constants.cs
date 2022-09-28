// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Host.Functions.Transform
{
    /// <summary>
    /// The Constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The source system name required.
        /// </summary>
        public const string SourceSystemNameRequired = "El nombre del sistema origen (SourceSystem) es obligatorio";

        /// <summary>
        /// The destination system name required.
        /// </summary>
        public const string DestinationSystemNameRequired = "DestinationSystem es obligatorio";

        /// <summary>
        /// The event type required.
        /// <summary>
        public const string EventTypeRequired = "El tipo de evento (EventType) es obligatorio";

        /// <summary>
        /// The movement identifier is mandatory.
        /// </summary>
        public const string MovementIdentifierRequired = "El identificador del movimiento (MovementId) es obligatorio";

        /// <summary>
        /// The movement type identifier is mandatory.
        /// </summary>
        public const string MovementTypeRequired = "El identificador del tipo de movimiento (MovementTypeId) es obligatorio";

        /// <summary>
        /// The operational date is mandatory.
        /// </summary>
        public const string OperationDateRequired = "La fecha operacional (OperationDate) es obligatoria";

        /// <summary>
        /// The period is mandatory.
        /// </summary>
        public const string PeriodIsMandatory = "El periodo (Period) es obligatorio";

        /// <summary>
        /// The net standard volume is mandatory.
        /// </summary>
        public const string NetStandardVolumeIsMandatory = "El volumen neto (NetStandardVolume) es obligatorio";

        /// <summary>
        /// The segment required.
        /// </summary>
        public const string SegmentRequired = "El segmento (Segment) es obligatorio";

        /// <summary>
        /// The movement source required.
        /// </summary>
        public const string MovementSourceRequired = "El origen del movimiento (MovementSource) es obligatorio";

        /// <summary>
        /// The movement destination required.
        /// </summary>
        public const string MovementDestinationRequired = "El destino del movimiento (MovementDestination) es obligatorio";

        /// <summary>
        /// The measurement unit is mandatory.
        /// </summary>
        public const string MeasurementUnitRequired = "La unidad de medida del atributo (MeasurementUnit) es obligatoria";

        /// <summary>
        /// The measurement unit for backup movement is mandatory.
        /// </summary>
        public const string MeasurementUnitRequiredForBackup = "La unidad de medida del movimiento de respaldo (MeasurementUnit) es obligatorio";

        /// <summary>
        /// The inventory identifier required.
        /// </summary>
        public const string InventoryIdRequired = "El identificador del inventario (InventoryId) es obligatorio";

        /// <summary>
        /// The inventory date required.
        /// </summary>
        public const string InventoryDateRequired = "La fecha del inventario (InventoryDate) es obligatoria";

        /// <summary>
        /// The attribute description required.
        /// </summary>
        public const string AttributeIdRequired = "El identificador del atributo (AttributeId) es obligatorio";

        /// <summary>
        /// Gets the PRODUCT REQUIREDVALIDATION.
        /// </summary>
        /// <value>
        /// The PRODUCT REQUIRED VALIDATION.
        /// </value>
        public const string ProductsRequired = "Productos (Products) es obligatorio";

        /// <summary>
        /// The attribute value required.
        /// </summary>
        public const string AttributeValueRequired = "El valor del atributo (AttributeValue) es obligatorio";

        /// <summary>
        /// The value attribute unit required.
        /// </summary>
        public const string ValueAttributeUnitRequired = "La unidad de medida del atributo (ValueAttributeUnit) es obligatoria";

        /// <summary>
        /// The owner identifier required.
        /// </summary>
        public const string OwnerIdRequired = "El identificador del propietario (OwnerId) es obligatorio";

        /// <summary>
        /// The ownership value required.
        /// </summary>
        public const string OwnershipValueRequired = "El valor de la propiedad (OwnershipValue) es obligatorio";

        /// <summary>
        /// The ownership value unit required.
        /// </summary>
        public const string OwnershipValueUnitRequired = "La unidad del valor de la propiedad (OwnershipValueUnit) es obligatoria";

        /// <summary>
        /// The Classification is mandatory.
        /// </summary>
        public const string MovementClassificationIsMandatory = "La clasificación del movimiento (Classification) es obligatoria";

        /// <summary>
        /// The movement start time is mandatory.
        /// </summary>
        public const string MovementStartTimeIsMandatory = "La hora de inicio del movimiento (StartTime) es obligatoria";

        /// <summary>
        /// The movement end time is mandatory.
        /// </summary>
        public const string MovementEndTimeIsMandatory = "La hora final del movimiento (EndTime) es obligatoria";

        /// <summary>
        /// Gets the productId REQUIRED VALIDATION.
        /// </summary>
        /// <value>
        /// The productId REQUIRED VALIDATION.
        /// </value>
        public const string ProductIdRequired = "El identificador del producto (ProductId) es obligatorio";

        /// <summary>
        /// Gets the MOVEMENT sourceProductId REQUIRED VALIDATION.
        /// </summary>
        /// <value>
        /// The MOVEMENT sourceProductId REQUIRED VALIDATION.
        /// </value>
        public const string MovementSourceProductIdRequired = "El identificador del producto origen (SourceProductId) es obligatorio";

        /// <summary>
        /// Gets the MOVEMENT SOURCE PRODUCTTYPE REQUIREDVALIDATION REQUIRED VALIDATION.
        /// </summary>
        /// <value>
        /// The MOVEMENT SOURCE PRODUCT TYPEID_REQUIREDVALIDATION REQUIRED VALIDATION.
        /// </value>
        public const string MovementSourceProductTypeIdRequired = "El identificador del tipo de producto origen (SourceProductTypeId) es obligatorio";

        /// <summary>
        /// Gets the productTypeId REQUIRED VALIDATION.
        /// </summary>
        /// <value>
        /// The productTypeId REQUIRED VALIDATION.
        /// </value>
        public const string ProductTypeIdRequired = "El identificador de tipo de producto (ProductType) es obligatorio";

        /// <summary>
        /// The products volume required.
        /// </summary>
        public const string ProductsVolumeRequired = "El volumen (ProductVolume) es obligatorio";

        /// <summary>
        /// The node identifier required.
        /// </summary>
        public const string NodeIdRequired = "El identificador del nodo (NodeId) es obligatorio";

        /// <summary>
        /// Gets the SourceNodeId REQUIREDVALIDATION.
        /// </summary>
        /// <value>
        /// The SourceNodeId REQUIREDVALIDATION.
        /// </value>
        public const string SourceNodeIdRequired = "El nodo origen (SourceNodeId) es obligatorio";

        /// <summary>
        /// Gets the DestinationNodeId REQUIREDVALIDATION.
        /// </summary>
        /// <value>
        /// The DestinationNodeId REQUIREDVALIDATION.
        /// </value>
        public const string DestinationNodeIdRequired = "El nodo destino (DestinationNodeId) es obligatorio";

        /// <summary>
        /// Gets the DestinationProductId REQUIREDVALIDATION.
        /// </summary>
        /// <value>
        /// The DestinationProductId REQUIREDVALIDATION.
        /// </value>
        public const string DestinationProductIdRequired = "El producto destino (DestinationProductId) es obligatorio";

        /// <summary>
        /// Gets the DestinationProductTypeId REQUIREDVALIDATION.
        /// </summary>
        /// <value>
        /// The DestinationProductTypeId REQUIREDVALIDATION.
        /// </value>
        public const string DestinationProductTypeIdRequired = "El tipo de producto del producto destino (DestinationProductTypeId) es obligatorio";

        /// <summary>
        /// The observation length exceeded.
        /// </summary>
        public const string ObservationLengthExceeded = "Las observaciones (Observations) pueden contener máximo 150 caracteres";

        /// <summary>
        /// The source system length exceeded.
        /// </summary>
        public const string SourceSystemLengthExceeded = "El nombre del sistema origen (SourceSystem) admite hasta 25 caracteres";

        /// <summary>
        /// The destination system length exceeded.
        /// </summary>
        public const string DestinationSystemLengthExceeded = "DestinationSystem admite hasta 25 caracteres";

        /// <summary>
        /// The inventory id length exceeded.
        /// </summary>
        public const string InventoryIdLengthExceeded = "InventoryId admite hasta 10 caracteres";

        /// <summary>
        /// The event type length exceeded.
        /// </summary>
        public const string EventTypeLengthExceeded = "El tipo de evento (EventType) puede contener máximo 25 caracteres";

        /// <summary>
        /// The event type length exceeded for inventory.
        /// </summary>
        public const string EventTypeLengthExceededForInventory = "El tipo de evento (EventType) puede contener máximo 10 caracteres";

        /// <summary>
        /// The scenario length exceeded.
        /// </summary>
        public const string ScenarioLengthExceeded = "El escenario (Scenario) puede contener máximo 50 caracteres";

        /// <summary>
        /// The movement classification length max 30 characters.
        /// </summary>
        public const string ClassificationLengthExceeded = "La clasificación del movimiento (Classification) puede contener máximo 30 caracteres";

        /// <summary>
        /// The tank name length exceeded.
        /// </summary>
        public const string TankNameLengthExceeded = "El Tanque (Tank) puede contener máximo 20 caracteres";

        /// <summary>
        /// The movement identifier length exceeded.
        /// </summary>
        public const string MovementIdentifierLengthExceeded = "El identificador del movimiento (MovementId) admite hasta 50 caracteres";

        /// <summary>
        /// The movement identifier length exceeded.
        /// </summary>
        public const string MovementIdentifierLengthExceededForBackup = "El identificador del movimiento de respaldo (MovementId) admite hasta 10 caracteres";

        /// <summary>
        /// The global movement identifier length exceeded.
        /// </summary>
        public const string GlobalMovementIdentifierLengthExceeded = "El identificador del movimiento global (GlobalMovementId) admite hasta 50 caracteres";

        /// <summary>
        /// The balance status length exceeded.
        /// </summary>
        public const string BalanceStatusLengthExceeded = "El estado del balance (BalanceStatus) admite hasta 50 caracteres";

        /// <summary>
        /// The balance status length exceeded.
        /// </summary>
        public const string SapProcessStatusLengthExceeded = "El estado del proceso (SapProcessStatus) admite hasta 50 caracteres";

        /// <summary>
        /// The movement type identifier length exceeded.
        /// </summary>
        public const string MovementTypeIdentifierLengthExceeded = "El identificador del tipo de movimiento (MovementTypeId) admite hasta 150 caracteres";

        /// <summary>
        /// The measurement unit length exceeded.
        /// </summary>
        public const string MeasurementUnitLengthExceeded = "La unidad de medida (MeasurementUnit) admite hasta 50 caracteres";

        /// <summary>
        /// The segment length exceeded.
        /// </summary>
        public const string SegmentLengthExceeded = "El identificador del segmento (Segment) admite hasta 150 caracteres";

        /// <summary>
        /// The attributeId length exceeded.
        /// </summary>
        public const string AttributeIdLengthExceeded = "El identificador del atributo (AttributeId) admite hasta 150 caracteres";

        /// <summary>
        /// The attribute type length exceeded.
        /// </summary>
        public const string AttributeTypeLengthExceeded = "El tipo de atributo (AttributeType) admite hasta 150 caracteres";

        /// <summary>
        /// The attribute value length exceeded.
        /// </summary>
        public const string AttributeValueLengthExceeded = "El valor del atributo (AttributeValue) admite hasta 150 caracteres";

        /// <summary>
        /// The value attribute unit length exceeded.
        /// </summary>
        public const string ValueAttributeUnitLengthExceeded = "La unidad del valor del atributo (ValueAttributeUnit) admite hasta 50 caracteres";

        /// <summary>
        /// The attribute description length exceeded.
        /// </summary>
        public const string AttributeDescriptionLengthExceeded = "La descripción del atributo (AttributeDescription) puede contener máximo 150 caracteres";

        /// <summary>
        /// The ownerId length exceeded.
        /// </summary>
        public const string OwnerIdLengthExceeded = "El identificador del propietario (OwnerId) admite hasta 150 caracteres";

        /// <summary>
        /// The ownership value unit length exceeded.
        /// </summary>
        public const string OwnershipValueUnitLengthExceeded = "La unidad de la propiedad (OwnershipValueUnit) admite hasta 50 caracteres";

        /// <summary>
        /// The source nodeId length exceeded.
        /// </summary>
        public const string SourceNodeIdLengthExceeded = "El identificador del nodo origen (SourceNodeId) admite hasta 150 caracteres";

        /// <summary>
        /// The source storage locationId length exceeded.
        /// </summary>
        public const string SourceStorageLocationIdLengthExceeded = " El identificador del almacén de origen (SourceStorageLocationId) admite hasta 150 caracteres";

        /// <summary>
        /// The source productId length exceeded.
        /// </summary>
        public const string SourceProductIdLengthExceeded = "El identificador del producto origen (SourceProductId) admite hasta 150 caracteres";

        /// <summary>
        /// The source product typeId length exceeded.
        /// </summary>
        public const string SourceProductTypeIdLengthExceeded = "El identificador del tipo de producto origen (SourceProductTypeId) admite hasta 150 caracteres";

        /// <summary>
        /// The productId length exceeded.
        /// </summary>
        public const string ProductIdLengthExceeded = "El identificador del producto (ProductId) admite hasta 150 caracteres";

        /// <summary>
        /// The product typeId length exceeded.
        /// </summary>
        public const string ProductTypeIdLengthExceeded = "El identificador del tipo de producto (ProductTypeId) admite hasta 150 caracteres";

        /// <summary>
        /// The source nodeId length exceeded.
        /// </summary>
        public const string DestinationNodeIdLengthExceeded = "El identificador del nodo de destino (DestinationNodeId) admite hasta 150 caracteres";

        /// <summary>
        /// The source destination locationId length exceeded.
        /// </summary>
        public const string DestinationStorageLocationIdLengthExceeded = "El identificador del almacén de destino (DestinationStorageLocationId) admite hasta 150 caracteres";

        /// <summary>
        /// The destination productId length exceeded.
        /// </summary>
        public const string DestinationProductIdLengthExceeded = "El identificador del producto de destino (DestinationProductId) admite hasta 150 caracteres";

        /// <summary>
        /// The destination product typeId length exceeded.
        /// </summary>
        public const string DestinationProductTypeIdLengthExceeded = "El identificador del tipo de producto de destino (DestinationProductTypeId) admite hasta 150 caracteres";

        /// <summary>
        /// The nodeId length exceeded.
        /// </summary>
        public const string NodeIdLengthExceeded = "El identificador del nodo (NodeId) admite hasta 150 caracteres";

        /// <summary>
        /// The operator length exceeded.
        /// </summary>
        public const string OperatorLengthExceeded = "El operador (Operator) admite hasta 150 caracteres";
    }
}
