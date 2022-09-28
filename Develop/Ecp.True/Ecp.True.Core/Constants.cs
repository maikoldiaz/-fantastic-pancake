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

namespace Ecp.True.Core
{
    /// <summary>
    /// The Constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The retried.
        /// </summary>
        public const string Retried = "RETIRADO";

        /// <summary>
        /// The failed parsing.
        /// </summary>
        public const string FailedParsing = "ParsingFailure";

        /// <summary>
        /// The failed parsing.
        /// </summary>
        public const string FailedRetry = "RetryFailure";

        /// <summary>
        /// The group type node.
        /// </summary>
        public const int GroupTypeNode = 13;

        /// <summary>
        /// The type node.
        /// </summary>
        public const int TypeNodeCategoryElement = 1;

        /// <summary>
        /// The type node.
        /// </summary>
        public const int TypeSegmentCategoryElement = 2;

        /// <summary>
        /// The group type product.
        /// </summary>
        public const int GroupTypeProduct = 14;

        /// <summary>
        /// The group type storage location.
        /// </summary>
        public const int GroupTypeStorageLocation = 15;

        /// <summary>
        /// The invalid arguments.
        /// </summary>
        public const string InvalidArguments = "Cannot construct argument object array.";

        /// <summary>
        /// The movement contract name.
        /// </summary>
        public const string MovementContractName = "MovementFactory";

        /// <summary>
        /// The inventory contract name.
        /// </summary>
        public const string InventoryContractName = "InventoryFactory";

        /// <summary>
        /// The inventory contract name.
        /// </summary>
        public const int EcopetrolCategoryElementId = 30;

        /// <summary>
        /// The inventory ownership contract name.
        /// </summary>
        public const string InventoryOwnershipContractName = "InventoryOwnership";

        /// <summary>
        /// The movement ownership contract name.
        /// </summary>
        public const string MovementOwnershipContractName = "MovementOwnership";

        /// <summary>
        /// The blockchain event name.
        /// </summary>
        public const string BlockchainEventName = "TrueLog";

        /// <summary>
        /// Gets the Movement destination required validation.
        /// </summary>
        /// <value>
        /// The MOVEMENT DESTINATION REQUIRED VALIDATION.
        /// </value>
        public const string MovementDestinationRequired = "El destino del movimiento es obligatorio";

        /// <summary>
        /// Gets the Movement source required validation.
        /// </summary>
        /// <value>
        /// The MOVEMENT source REQUIRED VALIDATION.
        /// </value>
        public const string MovementSourceRequired = "El origen del movimiento es obligatorio";

        /// <summary>
        /// The function not found.
        /// </summary>
        public const string FunctionNotFound = "Function not found";

        /// <summary>
        /// The parameters mismatch.
        /// </summary>
        public const string ParametersMismatch = "Parameters do not match";

        /// <summary>
        /// The argument null.
        /// </summary>
        public const string ArgumentNull = "Cannot construct argument object array.";

        /// <summary>
        /// The contract creation error.
        /// </summary>
        public const string ContractCreationError = "Error creating contract action message";

        /// <summary>
        /// The conciliation node error.
        /// </summary>
        public const string ConciliationGenericNodeError = "No se encontró el nodo conciliación en el segmento genérico";

        /// <summary>
        /// The contract function failed.
        /// </summary>
        public const string ContractFunctionFailed = "Execute Smart Contract Function Failed";

        /// <summary>
        /// The contract transaction failed.
        /// </summary>
        public const string ContractTransactionFailed = "Always failing transaction";

        /// <summary>
        /// The SourceSystem.
        /// </summary>
        public const string SourceSystem = "SourceSystem";

        /// <summary>
        /// The Message Id.
        /// </summary>
        public const string MessageId = "MessageId";

        /// <summary>
        /// The Movement Id.
        /// </summary>
        public const string MovementId = "MovementId";

        /// <summary>
        /// The inventory identifier.
        /// </summary>
        public const string InventoryId = "InventoryId";

        /// <summary>
        /// The Purchase Id.
        /// </summary>
        public const string PurchaseId = "PurchaseId";

        /// <summary>
        /// The Sale identifier.
        /// </summary>
        public const string SaleId = "SaleId";

        /// <summary>
        /// The node identifier.
        /// </summary>
        public const string NodeId = "NodeId";

        /// <summary>
        /// The inventory date.
        /// </summary>
        public const string InventoryDate = "InventoryDate";

        /// <summary>
        /// The batch identifier.
        /// </summary>
        public const string BatchId = "BatchId";

        /// <summary>
        /// The tank name.
        /// </summary>
        public const string TankName = "TankName";

        /// <summary>
        /// The tank name identifier key.
        /// </summary>
        public const string TankNameKey = "Tanque";

        /// <summary>
        /// The scenario identifier key.
        /// </summary>
        public const string ScenarioIdKey = "IdEscenario";

        /// <summary>
        /// The scenario identifier key.
        /// </summary>
        public const string NetStandardVolumeKey = "CantidadNeta";

        /// <summary>
        /// The scenario identifier key.
        /// </summary>
        public const string GrossStandardVolumeKey = "CantidadBruta";

        /// <summary>
        /// The scenario identifier key.
        /// </summary>
        public const string Version = "Version";

        /// <summary>
        /// The system identifier key.
        /// </summary>
        public const string SystemIdKey = "Sistema";

        /// <summary>
        /// The system identifier key.
        /// </summary>
        public const string SystemId = "SystemId";

        /// <summary>
        /// The operator.
        /// </summary>
        public const string OperatorId = "OperatorId";

        /// <summary>
        /// The operator key.
        /// </summary>
        public const string OperatorKey = "Operador";

        /// <summary>
        /// The tolerance identifier key.
        /// </summary>
        public const string ToleranceKey = "Incertidumbre";

        /// <summary>
        /// The inventory identifier key.
        /// </summary>
        public const string InventoryIdKey = "IdInventario";

        /// <summary>
        /// The node identifier key.
        /// </summary>
        public const string NodeIdKey = "IdNodo";

        /// <summary>
        /// The product key.
        /// </summary>
        public const string ProductKey = "Producto";

        /// <summary>
        /// The product key.
        /// </summary>
        public const string MovementIdKey = "IdMovimiento";

        /// <summary>
        /// The inventory date.
        /// </summary>
        public const string InventoryDateKey = "FechaInventario";

        /// <summary>
        /// The BackupMovement.
        /// </summary>
        public const string BackupMovement = "BackupMovement";

        /// <summary>
        /// The not available.
        /// </summary>
        public const string NotAvailable = "NA";

        /// <summary>
        /// The redirect path on authentication failure header.
        /// </summary>
        public const string RedirectPathOnAuthFailureHeader = "RedirectPathOnAuthFailure";

        /// <summary>
        /// The redirect path on authentication failure path.
        /// </summary>
        public const string RedirectPathOnAuthFailurePath = "/Account/SignOut";

        /// <summary>
        /// The true origin header.
        /// </summary>
        public const string TrueOriginHeader = "true-origin";

        /// <summary>
        /// The Type.
        /// </summary>
        public const string Type = "Type";

        /// <summary>
        /// The movement.
        /// </summary>
        public const string IsMovement = "IsMovement";

        /// <summary>
        /// The is homologated.
        /// </summary>
        public const string IsHomologated = "IsHomologated";

        /// <summary>
        /// The segment identifier.
        /// </summary>
        public const string SegmentId = "SegmentId";

        /// <summary>
        /// The inventory.
        /// </summary>
        public const string Inventory = "inventory";

        /// <summary>
        /// The event identifier.
        /// </summary>
        public const string EventId = "eventId";

        /// <summary>
        /// The movement.
        /// </summary>
        public const string Movement = "movement";

        /// <summary>
        /// The blockchain events.
        /// </summary>
        public const string BlockchainEvents = "blockchainevents";

        /// <summary>
        /// The register event queue.
        /// </summary>
        public const string Registerevent = "registerevents";

        /// <summary>
        /// The register contract queue.
        /// </summary>
        public const string Registercontract = "registercontracts";

        /// <summary>
        /// The ownership container.
        /// </summary>
        public const string OwnershipContainer = "validatedownership";

        /// <summary>
        /// The ownershipmovements.
        /// </summary>
        public const string Ownershipmovements = "ownershipmovements";

        /// <summary>
        /// The ownershipinventory.
        /// </summary>
        public const string Ownershipinventory = "ownershipinventory";

        /// <summary>
        /// Gets the Percentage validation message.
        /// </summary>
        /// <value>
        /// The Percentage validation message.
        /// </value>
        public const string PercentageValidationMessage = "El porcentaje debe ser un número entre 0 y 100";

        /// <summary>
        /// The calculation BLOB path.
        /// </summary>
        public const string CalculationBlobPath = "system/json/publishedmovements/";

        /// <summary>
        /// The Deadlettered Message BLOB path.
        /// </summary>
        public const string DeadletteredMessageBlobPath = "deadlettered/";

        /// <summary>
        /// The published ownership BLOB path.
        /// </summary>
        public const string PublishedOwnershipBlobPath = "system/json/publishedownership/";

        /// <summary>
        /// The movement BLOB path.
        /// </summary>
        public const string MovementBlobPath = "excel/json/movement/";

        /// <summary>
        /// The container name.
        /// </summary>
        public const string ContainerName = "true";

        /// <summary>
        /// The destination Node Id.
        /// </summary>
        public const string DestinationNodeId = "DestinationNodeId";

        /// <summary>
        /// The source Node Id.
        /// </summary>
        public const string SourceNodeId = "SourceNodeId";

        /// <summary>
        /// The start time.
        /// </summary>
        public const string StartTime = "StartTime";

        /// <summary>
        /// The end time.
        /// </summary>
        public const string EndTime = "EndTime";

        /// <summary>
        /// The Measurement Unit.
        /// </summary>
        public const string MeasurementUnit = "MeasurementUnit";

        /// <summary>
        /// The Contract Identifier.
        /// </summary>
        public const string ContractId = "ContractId";

        /// <summary>
        /// The Movement Type Identifier.
        /// </summary>
        public const string MovementTypeId = "MovementTypeId";

        /// <summary>
        /// The operational date.
        /// </summary>
        public const string OperationalDate = "OperationalDate";

        /// <summary>
        /// The scenario.
        /// </summary>
        public const string Scenario = "ScenarioId";

        /// <summary>
        /// The product identifier.
        /// </summary>
        public const string ProductId = "ProductId";

        /// <summary>
        /// The Source Product Id.
        /// </summary>
        public const string SourceProductId = "SourceProductId";

        /// <summary>
        /// The Destination Product Id.
        /// </summary>
        public const string DestinationProductId = "DestinationProductId";

        /// <summary>
        /// The Blob not found.
        /// </summary>
        public const string BlobNotFound = "Blob not found";

        /// <summary>
        /// Gets the maximum double value.
        /// </summary>
        /// <value>
        /// The maximum double value allowed.
        /// </value>
        public const string MaxDecimalAllowedValue = "9999999999999999.99";

        /// <summary>
        /// Gets the minimum double value.
        /// </summary>
        /// <value>
        /// The minimum double value allowed.
        /// </value>
        public const string MinDecimalAllowedValue = "-9999999999999999.99";

        /// <summary>
        /// Gets the minimum percentage value.
        /// </summary>
        /// <value>
        /// The minimum percentage value.
        /// </value>
        public const string MinPercentageValue = "0.01";

        /// <summary>
        /// Gets the maximum percentage value.
        /// </summary>
        /// <value>
        /// The maximum percentage value.
        /// </value>
        public const string MaxPercentageValue = "100.00";

        /// <summary>
        /// Gets the zero percentage value.
        /// </summary>
        /// <value>
        /// The zero percentage value.
        /// </value>
        public const string ZeroPercentageValue = "0.00";

        /// <summary>
        /// The report header view name.
        /// </summary>
        public const string ReportHeaderViewName = "ReportHeaderDetails";

        /// <summary>
        /// The report template view name.
        /// </summary>
        public const string ReportTemplateViewName = "ReportTemplateDetails";

        /// <summary>
        /// The kpi data view name.
        /// </summary>
        public const string KPIDataViewName = "KPIDataByCategoryElementNode";

        /// <summary>
        /// The kpi previous data view name.
        /// </summary>
        public const string KPIPreviousDataViewName = "KPIPreviousDateDataByCategoryElementNode";

        /// <summary>
        /// The dim date calculated table name.
        /// </summary>
        public const string DimDateCalculatedTableName = "DimDate";

        /// <summary>
        /// The product table name.
        /// </summary>
        public const string ProductTableName = "Product";

        /// <summary>
        /// The sap mapping detail table name.
        /// </summary>
        public const string SapMappingDetailTableName = "SapMappingDetail";

        /// <summary>
        /// The movements by product view name.
        /// </summary>
        public const string MovementsByProductViewName = "MovementsByProductWithoutOwner";

        /// <summary>
        /// The movement details view name.
        /// </summary>
        public const string MovementDetailsViewName = "MovementDetailsWithoutOwner";

        /// <summary>
        /// The attribute details view name.
        /// </summary>
        public const string AttributeDetailsViewName = "AttributeDetailsWithoutOwner";

        /// <summary>
        /// The inventory details view name.
        /// </summary>
        public const string InventoryDetailsViewName = "InventoryDetailsWithoutOwner";

        /// <summary>
        /// The quality details view name.
        /// </summary>
        public const string QualityDetailsViewName = "QualityDetailsWithoutOwner";

        /// <summary>
        /// The kpi data with ownership view name.
        /// </summary>
        public const string KPIDataWithOwnershipViewName = "KPIDataByCategoryElementNodeWithOwnership";

        /// <summary>
        /// The kpi previous data with ownership view name.
        /// </summary>
        public const string KPIPreviousDataWithOwnershipViewName = "KPIPreviousDateDataByCategoryElementNodeWithOwner";

        /// <summary>
        /// The movements by product with ownership view name.
        /// </summary>
        public const string MovementsByProductWithOwnershipViewName = "MovementsByProductWithOwner";

        /// <summary>
        /// The movement details with ownership view name.
        /// </summary>
        public const string MovementDetailsWithOwnershipViewName = "MovementDetailsWithOwner";

        /// <summary>
        /// The movement details with ownership view name.
        /// </summary>
        public const string MovementDetailsWithOwnershipOtherSegmentViewName = "MovementDetailsWithOwnerOtherSegment";

        /// <summary>
        /// The attribute details with ownership view name.
        /// </summary>
        public const string AttributeDetailsWithOwnershipViewName = "AttributeDetailsWithOwner";

        /// <summary>
        /// The inventory details with ownership view name.
        /// </summary>
        public const string InventoryDetailsWithOwnershipViewName = "InventoryDetailsWithOwner";

        /// <summary>
        /// The quality details with ownership view name.
        /// </summary>
        public const string QualityDetailsWithOwnershipViewName = "QualityDetailsWithOwner";

        /// <summary>
        /// The balance control view name.
        /// </summary>
        public const string BalanceControlViewName = "BalanceControl";

        /// <summary>
        /// The deadLettered queue name.
        /// </summary>
        public const string OwnershipProcessName = "OwnershipProcessing";

        /// <summary>
        /// The deadLettered queue name.
        /// </summary>
        public const string OperationalCutOffProcessName = "OperationalCutOffProcessing";

        /// <summary>
        /// The re calculation queue.
        /// </summary>
        public const string ReCalculationQueue = "calculateOwnership";

        /// <summary>
        /// The custom failure status.
        /// </summary>
        public const string CustomFailureStatus = "CustomFailure";

        /// <summary>
        /// The inventory product unique identifier.
        /// </summary>
        public const string InventoryProductUniqueId = "InventoryProductUniqueId";

        /// <summary>
        /// The original identifier.
        /// </summary>
        public const string OriginalId = "OriginalId";

        /// <summary>
        /// The balance control view name.
        /// </summary>
        public const string OfficialDeltaBalanceViewName = "OfficialDeltaBalance";

        /// <summary>
        /// The balance control view name.
        /// </summary>
        public const string OfficialDeltaMovementViewName = "OfficialDeltaMovements";

        /// <summary>
        /// The balance control view name.
        /// </summary>
        public const string OfficialDeltaInventoryViewName = "OfficialDeltaInventory";

        /// <summary>
        /// The sap queue interval in secs.
        /// </summary>
        public const int SapQueueIntervalInSecs = 300;

        /// <summary>
        /// The sap Logistic queue interval in secs.
        /// </summary>
        public const int SapLogisticQueueIntervalInSecs = 300;

        /// <summary>
        /// The sap queue interval in secs.
        /// </summary>
        public const int ManualInvOfficial = 189;

        /// <summary>
        /// The sap queue interval in secs.
        /// </summary>
        public const int ManualMovOfficial = 190;

        /// <summary>
        /// The sap upload status.
        /// </summary>
        public const string SapUploadStatus = "SapUploadStatus";

        /// <summary>
        /// Gets the movement classification.
        /// </summary>
        /// <value>
        /// The movement classification.
        /// </value>
        public const string MovementClassification = "Movimiento";

        /// <summary>
        /// Gets the loss classification.
        /// </summary>
        /// <value>
        /// The loss classification.
        /// </value>
        public const string LossClassification = "PerdidaIdentificada";

        /// <summary>
        /// Gets the special movement classification.
        /// </summary>
        /// <value>
        /// The special movement classification.
        /// </value>
        public const string SpecialMovementClassification = "OperacionTrazable";

        /// <summary>
        /// The report header view name.
        /// </summary>
        public const string BackupMovementDetailsViewName = "BackupMovementDetailsWithoutOwner";

        /// <summary>
        /// The report header view name.
        /// </summary>
        public const string BackupMovementDetailsWithOwnerViewName = "BackupMovementDetailsWithOwner";

        /// <summary>
        /// The SuccessProcess.
        /// </summary>
        public const string SapSuccessProcess = "S";

        /// <summary>
        /// The SentProcess.
        /// </summary>
        public const string SapSentProcess = "Enviado";

        /// <summary>
        /// The FailedProcess.
        /// </summary>
        public const string SapFailedProcess = "Fallido";

        /// <summary>
        ///  Mapping doesn't exist.
        /// </summary>
        public const string MappingDoesNotExist = "MAPPING_DOES_NOT_EXIST";

        /// <summary>
        /// Mapping has movements.
        /// </summary>
        public const string MappingHasMovements = "MAPPING_HAS_MOVEMENTS";

        /// <summary>
        /// Error code for max positions in purchases and sales.
        /// </summary>
        public const string PositionsErrorCode = "7108";

        /// <summary>
        /// Gets the unhandled exception tag.
        /// </summary>
        /// <value>
        /// The unhandled exception tag.
        /// </value>
        public static string UnhandledExceptionTag => "174febf5-3a63-49d3-9646-761b225e57b6";

        /// <summary>
        /// Gets the unhandled error message.
        /// </summary>
        /// <value>
        /// The unhandled error message.
        /// </value>
        public static string UnhandledErrorMessage => "Token expired. Redirecting user to login.";

        /// <summary>
        /// Gets the client side request header.
        /// </summary>
        /// <value>
        /// The client side request header.
        /// </value>
        public static string ClientSideRequestHeader => "True-Origin";

        /// <summary>
        /// Gets the identity log tag.
        /// </summary>
        /// <value>
        /// The identity log tag.
        /// </value>
        public static string AuthenticationLogTag => "41ea742f-79f9-4950-8189-e00e15501c4f";

        /// <summary>
        /// Gets the type of the invalid data.
        /// </summary>
        /// <value>
        /// The type of the invalid data.
        /// </value>
        public static string InvalidDataType => "Debe ser de tipo:";

        /// <summary>
        /// Gets the frequency invalid value.
        /// </summary>
        /// <value>
        /// The frequency invalid value.
        /// </value>
        public static string FrequencyInvalidValue => "La frecuencia solo permite los valores: \"diaria\", \"semanal\", \"quincenal\" y \"mensual\"";

        /// <summary>
        /// Gets the node automatically approved.
        /// </summary>
        /// <value>
        /// The node automatically approved.
        /// </value>
        public static string NodeAutomaticallyApproved => "Nodo aprobado automáticamente";

        /// <summary>
        /// Gets the value for Insert event.
        /// </summary>
        /// <value>
        /// The Insert keyword.
        /// </value>
        public static string Insert => "Insert";

        /// <summary>
        /// Gets the ownership rule synchronize.
        /// </summary>
        /// <value>
        /// The ownership rule synchronize.
        /// </value>
        public static string OwnershipRulesSync => "OwnershipRulesSync";

        /// <summary>
        /// Gets the sap po node synchronize.
        /// </summary>
        /// <value>
        /// The sap po node synchronize.
        /// </value>
        public static string SapMappingSync => "SapMappingSync";

        /// <summary>
        /// Gets the sap synchronize.
        /// </summary>
        /// <value>
        /// The sap synchronize.
        /// </value>
        public static string SapSync => "SapSync";

        /// <summary>
        /// Gets the Deadletter.
        /// </summary>
        /// <value>
        /// The Deadletter constant.
        /// </value>
        public static string Deadletter => "Deadletter";

        /// <summary>
        /// Gets the validation failure message.
        /// </summary>
        /// <value>
        /// The validation failure message.
        /// </value>
        public static string ValidationFailureMessage => "El motor de reglas no está retornando la propiedad de todos los movimientos e inventarios del día del periodo.";

        /// <summary>
        /// Gets the incorrect datatype failure message.
        /// </summary>
        /// <value>
        /// The incorrect datatype failure message.
        /// </value>
        public static string IncorrectDatatypeFailureMessage => "Una o más columnas {0} presentan un tipo de dato incorrecto.";

        /// <summary>
        /// Gets the not found information.
        /// </summary>
        /// <value>
        /// The not found information.
        /// </value>
        public static string NotFoundInformation => "Información no encontrada en {0}.";

        /// <summary>
        /// Gets the validation failure message.
        /// </summary>
        /// <value>
        /// The validation failure message.
        /// </value>
        public static string EntryCancellationMovementType => "155";

        /// <summary>
        /// Gets the validation failure message.
        /// </summary>
        /// <value>
        /// The validation failure message.
        /// </value>
        public static string DepartureCancellationMovementType => "156";

        /// <summary>
        /// Gets the purchase movement type.
        /// </summary>
        /// <value>
        /// The purchase movement type.
        /// </value>
        public static string PurchaseMovementType => "COMPRA";

        /// <summary>
        /// Gets the sale movement type.
        /// </summary>
        /// <value>
        /// The movement type.
        /// </value>
        public static string SaleMovementType => "VENTA";

        /// <summary>
        /// Gets the purchase movement type Id.
        /// </summary>
        /// <value>
        /// The purchase movement type Id.
        /// </value>
        public static string PurchaseMovementTypeId => "49";

        /// <summary>
        /// Gets the sale movement type Id.
        /// </summary>
        /// <value>
        /// The movement type Id.
        /// </value>
        public static string SaleMovementTypeId => "50";

        /// <summary>
        /// Gets the FICO SourceSystem.
        /// </summary>
        /// <value>
        /// The FICO SourceSystem.
        /// </value>
        public static string FicoSourceSystem => "FICO";

        /// <summary>
        /// Gets the evacuation agreement type.
        /// </summary>
        /// <value>
        /// The evacuation agreement type.
        /// </value>
        public static string Evacuation => "EVACUACION";

        /// <summary>
        /// Gets the collaboration agreement type.
        /// </summary>
        /// <value>
        /// The collaboration agreement type.
        /// </value>
        public static string Collaboration => "COLABORACION";

        /// <summary>
        /// Gets the Movement Destination.
        /// </summary>
        public static string MovementDestination => "MovementDestination";

        /// <summary>
        /// Gets the Movement Source.
        /// </summary>
        public static string MovementSource => "MovementSource";

        /// <summary>
        /// Gets the Reconciliation.
        /// </summary>
        public static string Reconciliation => "Reconciliation";

        /// <summary>
        /// Gets the failure reconciliation.
        /// </summary>
        public static string FailureReconciliation => "FailureReconciliation";

        /// <summary>
        /// Gets the critical event name.
        /// </summary>
        public static string Critical => "Critical";

        /// <summary>
        /// Gets the tag identifier for analysis service refresh.
        /// </summary>
        /// <value>
        /// The tag identifier for analysis service refresh.
        /// </value>
        public static string AnalysisServiceRefreshTag => "AnalysisServiceRefresh";

        /// <summary>
        /// Gets the purge ownership history key.
        /// </summary>
        /// <value>
        /// The purge ownership history key.
        /// </value>
        public static string PurgeOwnershipHistoryKey => "PurgeOwnershipHistory";

        /// <summary>
        /// Gets the purge delta history key.
        /// </summary>
        /// <value>
        /// The purge delta history key.
        /// </value>
        public static string PurgeDeltaHistoryKey => "PurgeDeltaHistory";

        /// <summary>
        /// Gets the purge official delta history key.
        /// </summary>
        /// <value>
        /// The purge official delta history key.
        /// </value>
        public static string PurgeOfficialDeltaHistoryKey => "PurgeOfficialDeltaHistory";

        /// <summary>
        /// Gets the purge operational cut off history key.
        /// </summary>
        /// <value>
        /// The purge operational cut off history key.
        /// </value>
        public static string PurgeOperationalCutOffHistoryKey => "PurgeOperationalCutOffHistory";

        /// <summary>
        /// Gets the purge transform history.
        /// </summary>
        /// <value>
        /// The purge transform history.
        /// </value>
        public static string PurgeTransformHistory => "PurgeTransformHistory";

        /// <summary>
        /// Gets the powerbi tag.
        /// </summary>
        /// <value>
        /// The powerbi tag.
        /// </value>
        public static string PowerBiTag => "Powerbi";

        /// <summary>
        /// Gets the bearer.
        /// </summary>
        /// <value>
        /// The bearer.
        /// </value>
        public static string Bearer => "Bearer";

        /// <summary>
        /// Gets the default HTTP client.
        /// </summary>
        /// <value>
        /// The default HTTP client.
        /// </value>
        public static string DefaultHttpClient => "DefaultHttpClientName";

        /// <summary>
        /// Gets the percentage.
        /// </summary>
        /// <value>
        /// The percentage.
        /// </value>
        public static string Percentage => "PORCENTAJE";

        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public static string Volume => "VOLUMEN";

        /// <summary>
        /// Gets the validate movement ownership percentage failure message.
        /// </summary>
        /// <value>
        /// The validate movement ownership percentage failure message.
        /// </value>
        public static string ValidateMovementOwnershipPercentageFailureMessage => "La sumatoria de los porcentajes de propiedad para algunos movimientos no es igual al 100%.";

        /// <summary>
        /// Gets the validate inventory ownership percentage failure message.
        /// </summary>
        /// <value>
        /// The validate inventory ownership percentage failure message.
        /// </value>
        public static string ValidateInventoryOwnershipPercentageFailureMessage => "La sumatoria de los porcentajes de propiedad para algunos inventarios no es igual al 100%.";

        /// <summary>
        /// Gets the validate inventory ownership percentage failure message.
        /// </summary>
        /// <value>
        /// The validate inventory ownership percentage failure message.
        /// </value>
        public static string ValidateInventoryProductFailureMessage => "El inventario {0} no existe.";

        /// <summary>
        /// Gets the validate invalid node for inventory product failure message.
        /// </summary>
        /// <value>
        /// The validate invalid node for inventory product failure message.
        /// </value>
        public static string ValidateInvalidNodeForInventoryProductFailureMessage => "Identificador del nodo inválido para el inventario {0}.";

        /// <summary>
        /// Gets the validate movement failure message.
        /// </summary>
        /// <value>
        /// The validate movement failure message.
        /// </value>
        public static string ValidateMovementFailureMessage => "El movimiento {0} no existe.";

        /// <summary>
        /// Gets the validate invalid node for movement failure message.
        /// </summary>
        /// <value>
        /// The validate invalid node for movement failure message.
        /// </value>
        public static string ValidateInvalidNodeForMovementFailureMessage => "Identificador del nodo inválido para el movimiento {0}.";

        /// <summary>
        /// Gets the validation no inventory and movement result failure message.
        /// </summary>
        /// <value>
        /// The validation no inventory and movement result failure message.
        /// </value>
        public static string ValidationNoInventoryAndMovementResultFailureMessage => "El motor de reglas no está retornando información de movimientos e inventarios.";

        /// <summary>
        /// Gets the ownership failure message.
        /// </summary>
        /// <value>
        /// The ownership failure message.
        /// </value>
        public static string OwnershipFailureMessage => "Ocurrió un error al realizar el cálculo de propiedad, por favor intente nuevamente.";

        /// <summary>
        /// Gets the conciliation ownership failure message.
        /// </summary>
        /// <value>
        /// The ownership failure message.
        /// </value>
        public static string ConciliationFailureMessage => "Ocurrió un error al generar las colecciones, por favor intente nuevamente.";

        /// <summary>
        /// Gets the operational cut off failure message.
        /// </summary>
        /// <value>
        /// The operational cut off failure message.
        /// </value>
        public static string OperationalCutOffFailureMessage => "Ocurrió un error al realizar el corte operativo, por favor intente nuevamente.";

        /// <summary>
        /// Gets the logistics failure message.
        /// </summary>
        /// <value>
        /// The logistics failure message.
        /// </value>
        public static string LogisticsFailureMessage => "Ocurrió un error al realizar el logísticos, por favor intente nuevamente.";

        /// <summary>
        /// Gets the purging cut off message.
        /// </summary>
        /// <value>
        /// The purging cut off message.
        /// </value>
        public static string PurgingCutOffMessage => "Purging operational cut off history of instance id";

        /// <summary>
        /// Gets the purging ownership message.
        /// </summary>
        /// <value>
        /// The purging ownership message.
        /// </value>
        public static string PurgingOwnershipMessage => "Purging ownership history of instance id";

        /// <summary>
        /// Gets the purging delta message.
        /// </summary>
        /// <value>
        /// The purging delta message.
        /// </value>
        public static string PurgingDeltaMessage => "Purging delta history of instance id";

        /// <summary>
        /// Gets the purging official delta message.
        /// </summary>
        /// <value>
        /// The purging official delta message.
        /// </value>
        public static string PurgingOfficialDeltaMessage => "Purging official delta history of instance id";

        /// <summary>
        /// Gets the error message for unprocess ticket.
        /// </summary>
        /// <value>
        /// The error message for unprocess ticket.
        /// </value>
        public static string ErrorMessageForUnprocessTicket => "Error al calcular la propiedad del tiquete {0} en la fecha {1}";

        /// <summary>
        /// Gets the registration error message.
        /// </summary>
        /// <value>
        /// The registration error message.
        /// </value>
        public static string RegistrationErrorMessage => "Ocurrió un error al realizar el registro, por favor intente nuevamente.";

        /// <summary>
        /// Gets the Destination productId Is Mandatory.
        /// </summary>
        /// <value>
        /// The destination product Id error message.
        /// </value>
        public static string DestinationProductIdIsMandatory => "El identificador del producto destino es obligatorio";

        /// <summary>
        /// Gets the content of the gzip.
        /// </summary>
        /// <value>
        /// The content of the gzip.
        /// </value>
        public static string GzipContent => "gzip";

        /// <summary>
        /// Gets the instrumentation key.
        /// </summary>
        /// <value>
        /// The instrumentation key.
        /// </value>
        public static string InstrumentationKey => "InstrumentationKey";

        /// <summary>
        /// Gets the delta failure message.
        /// </summary>
        /// <value>
        /// The delta failure message.
        /// </value>
        public static string DeltaFailureMessage => "Se presentó un error inesperado en " +
            "el envío de movimientos e inventarios para el cálculo de deltas operativos. " +
            "Por favor ejecute nuevamente el proceso.";

        /// <summary>
        /// Gets the official delta failure message.
        /// </summary>
        /// <value>
        /// The official delta failure message.
        /// </value>
        public static string OfficialDeltaFailureMessage => "Se presentó un error técnico inesperado al enviar" +
            " la información al motor de reglas para el cálculo de deltas oficiales. Por favor ejecute nuevamente el proceso o comuníquese con la mesa de ayuda.";

        /// <summary>
        /// Gets the official delta failure message.
        /// </summary>
        /// <value>
        /// The official delta failure message.
        /// </value>
        public static string OfficialDeltaMovInvIdentificationFailureMessage => "Se presentó un error técnico inesperado durante el proceso de" +
            " identificación de información oficial. Por favor ejecute nuevamente el proceso o comuníquese con la mesa de ayuda.";

        /// <summary>
        /// Gets the official delta failure message.
        /// </summary>
        /// <value>
        /// The official delta failure message.
        /// </value>
        public static string OfficialDeltaCalculationFailureMessage => "Se presentó un error técnico inesperado en el precálculo de datos que se visualizarán" +
            " en el reporte del balance oficial por nodo. Por favor ejecute nuevamente el proceso.";

        /// <summary>
        /// Gets the no annulation error message.
        /// </summary>
        /// <value>
        /// The no annulation error message.
        /// </value>
        public static string NoAnnulationErrorMessage => "El tipo de movimiento {0} no tiene configurado un tipo de anulación";

        /// <summary>
        /// Gets the positive.
        /// </summary>
        /// <value>
        /// The positive.
        /// </value>
        public static string Positive => "POSITIVO";

        /// <summary>
        /// Gets the negative.
        /// </summary>
        /// <value>
        /// The negative.
        /// </value>
        public static string Negative => "NEGATIVO";

        /// <summary>
        /// Gets the igual.
        /// </summary>
        /// <value>
        /// The igual.
        /// </value>
        public static string Igual => "IGUAL";

        /// <summary>
        /// Gets the date string format.
        /// </summary>
        /// <value>
        /// The date string format.
        /// </value>
        public static string DateStringFormat => "yyyyMMdd";

        /// <summary>
        /// Gets the analytical process invoke fail.
        /// </summary>
        /// <value>
        /// The analytical process invoke fail.
        /// </value>
        public static string AnalyticalProcessInvokeFail => "ANALITYCAL_PROCESS_INVOKE_FAIL - Ocurrió un error en el envío de información analítica: " +
            "el proceso de aprobación fue exitoso, pero el envío a analítica presentó un problema y no se completó. Por favor contacte a la mesa de ayuda para mayor información.";

        /// <summary>
        /// Gets the not movemen transfer point.
        /// </summary>
        /// <value>
        /// The not movemen transfer point.
        /// </value>
        public static string MovemenTransferPoint => "It is a Transfer Point Movement";

        /// <summary>
        /// Gets the approve official node delta fail.
        /// </summary>
        /// <value>
        /// The approve official node delta fail.
        /// </value>
        public static string ApproveOfficialNodeDeltaFail => "Approve Official Node Delta Fail";

        /// <summary>
        /// Gets the ownership rule synchronize.
        /// </summary>
        /// <value>
        /// The ownership rule synchronize.
        /// </value>
        public static string AuditReportsSync => "AuditReportsSync";

        /// <summary>
        /// Gets the delta registration failed message.
        /// </summary>
        /// <value>
        /// The delta registration failed message.
        /// </value>
        public static string DeltaRegistrationFailedMessage => "No se realizó el registro del delta, debido a que los nodos del movimiento" +
                    " pertenecen a otro segmento en la fecha del corte operativo {0}";

        /// <summary>
        /// Gets the official delta  registration failed message.
        /// </summary>
        /// <value>
        /// The offcial delta registration failed message.
        /// </value>
        public static string OfficialDeltaFailedMessage => "El segmento no tiene información oficial pendiente en el período " +
            "de fechas.";

        /// <summary>
        /// Gets the transfer point BLOB storage path.
        /// </summary>
        /// <value>
        /// The transfer point BLOB storage path.
        /// </value>
        public static string TransferPointBlobStoragePath => "sap/transferpoints/out/{0}.json";

        /// <summary>
        /// Gets the Cancellation.
        /// </summary>
        public static string Cancellation => "ANULACION";

        /// <summary>
        /// Gets the official delta without approval in previous period.
        /// </summary>
        /// <value>
        /// The official delta without approval in previous period.
        /// </value>
        public static string OfficialDeltaWithoutApprovalInPreviousPeriod => "Existen balances oficiales de nodos sin aprobar para el período anterior";

        /// <summary>
        /// Gets the official delta calculation in progress.
        /// </summary>
        /// <value>
        /// The official delta calculation in progress.
        /// </value>
        public static string OfficialDeltaCalculationInProgress => "Se encuentra en procesamiento un cálculo de deltas oficiales para el segmento o la cadena.";

        /// <summary>
        /// Gets the json registration orchestrator failed.
        /// </summary>
        /// <value>
        /// The json registration orchestrator failed.
        /// </value>
        public static string JsonRegistrationOrchestratorFailed => "Json registration orchestrator failed.";

        /// <summary>
        /// Gets the excel registration orchestrator failed.
        /// </summary>
        /// <value>
        /// The excel registration orchestrator failed.
        /// </value>
        public static string ExcelRegistrationOrchestratorFailed => "Excel registration orchestrator failed.";

        /// <summary>
        /// Gets the xml registration orchestrator failed.
        /// </summary>
        /// <value>
        /// The xml registration orchestrator failed.
        /// </value>
        public static string XmlRegistrationOrchestratorFailed => "Xml registration orchestrator failed.";

        /// <summary>
        /// Gets the ownership orchestrator failed.
        /// </summary>
        /// <value>
        /// The ownership orchestrator failed.
        /// </value>
        public static string OwnershipOrchestratorFailed => "Exception occurred in ownership processing.";

        /// <summary>
        /// Gets the ownership orchestrator failed.
        /// </summary>
        /// <value>
        /// The ownership orchestrator failed.
        /// </value>
        public static string ConciliationOrchestratorFailed => "Exception occurred in conciliation processing.";

        /// <summary>
        /// Gets the ownership orchestrator failed.
        /// </summary>
        /// <value>
        /// The ownership orchestrator failed.
        /// </value>
        public static string ConciliationOwnershipFailed => "Exception occurred in ownership processing.";

        /// <summary>
        /// Gets the purge official delta history key.
        /// </summary>
        /// <value>
        /// The purge Consolidation history key.
        /// </value>
        public static string PurgeConsolidationHistoryKey => "PurgeConsolidationHistory";

        /// <summary>
        /// Gets the Consolidation failure message.
        /// </summary>
        /// <value>
        /// The Consolidation failure message.
        /// </value>
        public static string ConsolidationFailureMessage => "Se presentó un error técnico inesperado en la consolidación de movimientos e inventarios " +
            "del escenario operativo. Por favor ejecute nuevamente el proceso o comuníquese con la mesa de ayuda.";

        /// <summary>
        /// Gets the purging Consolidation message.
        /// </summary>
        /// <value>
        /// The purging Consolidation message.
        /// </value>
        public static string PurgingConsolidationMessage => "Purging official delta history of instance id";

        /// <summary>
        /// Gets the cut off orchestrator failed.
        /// </summary>
        /// <value>
        /// The cut off orchestrator failed.
        /// </value>
        public static string CutOffOrchestratorFailed => "Exception occurred in cutoff processing.";

        /// <summary>
        /// Gets Base Retry Failed.
        /// </summary>
        /// <value>
        /// Base Retry Failed.
        /// </value>
        public static string BaseRetryFailed => "Exception occurred in Base Retry Failed.";

        /// <summary>
        /// Gets CutOff Retry Failed.
        /// </summary>
        /// <value>
        /// CutOff Retry Failed.
        /// </value>
        public static string CutOffRetryFailed => "Exception occurred in CutOff Retry Failed.";

        /// <summary>
        /// Gets Ownership Retry Failed.
        /// Gets the recalculate cut off orchestrator failed.
        /// </summary>
        /// <value>
        /// The recalculate cut off orchestrator failed.
        /// </value>
        public static string RecalculateCutOffOrchestratorFailed => "Exception occurred in recalculate cutoff processing.";

        /// <summary>
        /// Gets Retry Finalizer Failed.
        /// </summary>
        /// <value>
        /// Ownership Retry Failed.
        /// </value>
        public static string OwnershipRetryFailed => "Exception occurred in Ownership Retry Failed.";

        /// <summary>
        /// Gets the AllIncludingSourceAndDestination.
        /// </summary>
        /// <value>
        /// The AllIncludingSourceAndDestination.
        /// </value>
        public static string AllIncludingSourceAndDestination => "AllIncludingSourceAndDestination";

        /// <summary>
        /// Gets the full analysis service refresh type.
        /// </summary>
        /// <value>
        /// The the full analysis service refresh type.
        /// </value>
        public static string FullAnalysisServiceRefreshType => "full";

        /// <summary>
        /// Gets the data only analysis service refresh type.
        /// </summary>
        /// <value>
        /// The the data only analysis service refresh type.
        /// </value>
        public static string DataOnlyAnalysisServiceRefreshType => "dataOnly";

        /// <summary>
        ///  Gets the category elementId.
        /// </summary>
        /// <value>
        /// The category elementId.
        /// </value>
        public static int DeltaInventory => 187;

        /// <summary>
        /// Gets the official logistics failure message.
        /// </summary>
        /// <value>
        /// The official logistics failure message.
        /// </value>
        public static string OfficialLogisticsFailureMessage => "Ocurrió un error generando el archivo logístico, por favor intente nuevamente";

        /// <summary>
        /// Gets the services availability synchronize.
        /// </summary>
        /// <value>
        /// The services availability synchronize.
        /// </value>
        public static string ServicesAvailabilitySync => "ServicesAvailabilitySync";

        /// <summary>
        /// Gets the check availability settings asynchronous.
        /// </summary>
        /// <value>
        /// The check availability settings asynchronous.
        /// </value>
        public static string CheckAvailabilitySettingsAsync => "Configurations of the availability settings are not available.";

        /// <summary>
        /// Gets the check availability asynchronous.
        /// </summary>
        /// <value>
        /// The check availability asynchronous.
        /// </value>
        public static string CheckAvailabilityAsync => "Checking availability of the resources:";

        /// <summary>
        /// Gets the state of the unavailable.
        /// </summary>
        /// <value>
        /// The state of the unavailable.
        /// </value>
        public static string UnavailableState => "Unavailable";

        /// <summary>
        /// Gets the state of the available.
        /// </summary>
        /// <value>
        /// The state of the available.
        /// </value>
        public static string AvailableState => "Available";

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public static string Location => "East US";

        /// <summary>
        /// Gets the success status.
        /// </summary>
        /// <value>
        /// The success status.
        /// </value>
        public static string SuccessStatus => "Success";

        /// <summary>
        /// Gets the purge report execution.
        /// </summary>
        /// <value>
        /// The purge report execution.
        /// </value>
        public static string PurgeReportExecution => "PurgeReportExecution";

        /// <summary>
        /// Gets the sap technical error.
        /// </summary>
        /// <value>
        /// The sap technical error.
        /// </value>
        public static string SapTechnicalError => "Se presentó un error técnico inesperado en el llamado al servicio de puntos de transferencia.";

        /// <summary>
        /// Gets the sap technical error Movement Or Inventory.
        /// </summary>
        /// <value>
        /// The sap technical error Movement Or Inventory.
        /// </value>
        public static string SapTechnicalErrorMovementOrInventory => "Se presentó un error técnico inesperado en el llamado al servicio de Movimientos o inventarios.";

        /// <summary>
        /// Gets the sap technical error Contract.
        /// </summary>
        /// <value>
        /// The sap technical error Contract.
        /// </value>
        public static string SapTechnicalErrorContract => "Se presentó un error técnico inesperado en el llamado al servicio de Contractos.";

        /// <summary>
        /// Gets the sap technical error Send Logistic Movement.
        /// </summary>
        /// <value>
        /// The sap technical error Send Logistic Movement.
        /// </value>
        public static string SapTechnicalErrorSendLogisticMovement => "Se presentó un error técnico inesperado en el llamado al servicio de Envios de movimientos logisticos.";

        /// <summary>
        /// Gets the insufficient information for calculation.
        /// </summary>
        /// <value>
        /// The insufficient information for calculation.
        /// </value>
        public static string InsufficientInformationForCalculation => "Información insuficiente para calcular el delta";

        /// <summary>
        /// Gets the update negate.
        /// </summary>
        /// <value>
        /// The update negate.
        /// </value>
        public static string UpdateNegate => "UpdateNegate";

        /// <summary>
        /// Gets the update negate.
        /// </summary>
        /// <value>
        /// The update negate.
        /// </value>
        public static string OwnershipPercentageUnit => "%";

        /// <summary>
        /// Gets the update negate.
        /// </summary>
        /// <value>
        /// The update negate.
        /// </value>
        public static string OwnershipPercentageUnitId => "159";

        /// <summary>
        /// Gets the technical exception error message.
        /// </summary>
        /// <value>
        /// The technical exception error message.
        /// </value>
        public static string TechnicalExceptionErrorMessage =>
            "Se presentó un error técnico inesperado. Por favor ejecute nuevamente el proceso o comuníquese con la mesa de ayuda.";

        /// <summary>
        /// Gets the technical exception parsing error message.
        /// </summary>
        /// <value>
        /// The technical exception parsing error message.
        /// </value>
        public static string TechnicalExceptionParsingErrorMessage =>
            "Se presentó un error técnico inesperado al realizar el registro de la información. Por favor ejecute nuevamente el proceso o comuníquese con la mesa de ayuda.";

        /// <summary>
        /// Gets the ConfigurationErrore.
        /// </summary>
        /// <value>
        /// The ConfigurationError.
        /// </value>
        public static string ConfigurationError => "Error de configuración.";

        /// <summary>
        /// Gets the barrels.
        /// </summary>
        /// <value>
        /// The barrels.
        /// </value>
        public static int Barrels => 31;

        /// <summary>
        /// Gets frequency field.
        /// </summary>
        /// <value>
        /// The frequency field.
        /// </value>
        public static string Frequency => "Frequency";

        /// <summary>
        /// Gets EventSapCreate field.
        /// </summary>
        /// <value>
        /// The EventSapCreate field.
        /// </value>
        public static string EventSapCreate => "Crear";

        /// <summary>
        /// Gets EventSapUpdate field.
        /// </summary>
        /// <value>
        /// The EventSapUpdate field.
        /// </value>
        public static string EventSapUpdate => "Modificar";

        /// <summary>
        /// Gets EventSapDelete field.
        /// </summary>
        /// <value>
        /// The EventSapDelete field.
        /// </value>
        public static string EventSapDelete => "Eliminar";

        /// <summary>
        /// Gets EventInsert field.
        /// </summary>
        /// <value>
        /// The EventInsert field.
        /// </value>
        public static string EventInsert => "Insert";

        /// <summary>
        /// Gets LogisticEventCreation.
        /// </summary>
        /// <value>
        /// The LogisticEventCreation.
        /// </value>
        public static string LogisticEventCreation => "CREAR";

        /// <summary>
        /// Gets LogisticOfficialFit.
        /// </summary>
        /// <value>
        /// The LogisticOfficialFit.
        /// </value>
        public static string LogisticOfficialFit => "Ajuste oficial";

        /// <summary>
        /// Gets EventType field.
        /// </summary>
        /// <value>
        /// The EventType field.
        /// </value>
        public static string EventType => "CREACION";

        /// <summary>
        /// Gets DestinationSystem field.
        /// </summary>
        /// <value>
        /// The DestinationSystem field.
        /// </value>
        public static string DestinationSystem => "SAP S4 HANA";

        /// <summary>
        /// Gets the delta oficial identifier key.
        /// </summary>
        public static string DeltaOfficialPrefix => "DO-";

        /// <summary>
        /// Gets EmToleranceBce field.
        /// </summary>
        /// <value>
        /// The EmToleranceBce field.
        /// </value>
        public static int EmToleranceBce => 213;

        /// <summary>
        /// Gets SmToleranceBce field.
        /// </summary>
        /// <value>
        /// The SmToleranceBce field.
        /// </value>
        public static int SmToleranceBce => 214;

        /// <summary>
        /// Gets EmAjteBce field.
        /// </summary>
        /// <value>
        /// The EmAjteBce field.
        /// </value>
        public static int EmAjteBce => 215;

        /// <summary>
        /// Gets SmAjteBce field.
        /// </summary>
        /// <value>
        /// The SmAjteBce field.
        /// </value>
        public static int SmAjteBce => 216;

        /// <summary>
        /// Gets EmExportLoans field.
        /// </summary>
        /// <value>
        /// The EmExportLoans field.
        /// </value>
        public static int EmExportLoans => 217;

        /// <summary>
        /// Gets SmExportLoans field.
        /// </summary>
        /// <value>
        /// The SmExportLoans field.
        /// </value>
        public static int SmExportLoans => 219;

        /// <summary>
        /// Gets AnulEmLoansExport field.
        /// </summary>
        /// <value>
        /// The AnulEmLoansExport field.
        /// </value>
        public static int AnulEmLoansExport => 218;

        /// <summary>
        /// Gets AnulSmLoansExport field.
        /// </summary>
        /// <value>
        /// The AnulSmLoansExport field.
        /// </value>
        public static int AnulSmLoansExport => 220;

        /// <summary>
        /// Gets the no manual movements found for ticke exception message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public static string NoManualMovementsWereFoundForTicket => "NO_MANUAL_MOVEMENTS_FOUND_FOR_TICKET";

        /// <summary>
        /// Gets the delta node was not found exception message.
        /// </summary>
        /// <value>The Message.</value>
        public static string DeltaNodeNotFound => "DELTANODE_NOT_FOUND";

        /// <summary>
        /// Gets the product already exist exception message.
        /// </summary>
        public static string ProductAlreadyExists => "PRODUCT_ALREADY_EXISTS";

        /// <summary>
        /// Gets the product does not exist exception message.
        /// </summary>
        public static string ProductDoesNotExist => "PRODUCT_DOESNOT_EXIST";

        /// <summary>
        /// Gets the product with movements exception message.
        /// </summary>
        public static string ProductWithMovements => "PRODUCT_WITH_MOVEMENTS";

        /// <summary>
        /// Gets the product already exist exception message.
        /// </summary>
        public static string MappingAlreadyExists => "MAPPING_ALREADY_EXISTS";

        /// <summary>
        /// Gets the product is inactive exception message.
        /// </summary>
        public static string ProductIsInactive => "PRODUCT_IS_INACTIVE";

        /// <summary>
        /// Gets the storage location is inactive exception message.
        /// </summary>
        public static string StorageLocationIsInactive => "STORAGE_LOCATION_IS_INACTIVE";

        /// <summary>
        /// Gets the storage location does not exist exception message.
        /// </summary>
        public static string StorageLocationDoesNotExist => "STORAGE_LOCATION_DOES_NOT_EXIST";

        /// <summary>
        /// Gets the product with mappings exception message.
        /// </summary>
        public static string ProductWithMappings => "PRODUCT_WITH_MAPPINGS";

        /// <summary>
        /// Gets the product with configuration exception message.
        /// </summary>
        public static string ProductWithConfigurations => "PRODUCT_WITH_CONFIGURATIONS";

        /// <summary>
        /// Gets the TransactionNodeQuorum.
        /// </summary>
        public static string TransactionNodeQuorum => "onquorum.net";

        /// <summary>
        /// Gets the Http.
        /// </summary>
        public static string Http => "Http";

        /// <summary>
        /// Gets the Quorum.
        /// </summary>
        public static string Quorum => "quorum";

        /// <summary>
        /// Gets the QuorumBlockchainService.
        /// </summary>
        public static string QuorumBlockchainService => "Quorum Blockchain Service";

        /// <summary>
        /// Gets the Received.
        /// </summary>
        public static string Received => "Received";

        /// <summary>
        /// Gets the End.
        /// </summary>
        public static string End => "End";
    }
}