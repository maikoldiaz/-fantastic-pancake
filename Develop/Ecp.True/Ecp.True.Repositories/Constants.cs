// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>N
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Repositories
{
    /// <summary>
    /// The Constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The create update node store procedure name.
        /// </summary>
        public static readonly string NodeTagProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_NodeTag";

        /// <summary>
        /// The tag category type data table.
        /// </summary>
        public static readonly string NodeTagType = $"{DataAccess.Sql.Constants.AdminSchema}.NodeTagType";

        /// <summary>
        /// The unbalance data table.
        /// </summary>
        public static readonly string UnbalanceType = $"{DataAccess.Sql.Constants.AdminSchema}.UnbalanceType";

        /// <summary>
        /// The pending transaction error data table.
        /// </summary>
        public static readonly string PendingTransactionErrorsType = $"{DataAccess.Sql.Constants.AdminSchema}.PendingTransactionErrorMessagesType";

        /// <summary>
        /// The key value type.
        /// </summary>
        public static readonly string KeyValueType = $"{DataAccess.Sql.Constants.AdminSchema}.KeyValueType";

        /// <summary>
        /// The save ticket procedure name.
        /// </summary>
        public static readonly string SaveTicketProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveTicket";

        /// <summary>
        /// The group validation information procedure name.
        /// </summary>
        public static readonly string ValidateOwnershipInputs = $"{DataAccess.Sql.Constants.AdminSchema}.usp_ValidateOwnershipInputs";

        /// <summary>
        /// The validate logistic node procedure name.
        /// </summary>
        public static readonly string ValidateLogisticNodeTicket = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetLogisticNodeValidation";

        /// <summary>
        /// The movement details procedure name.
        /// </summary>
        public static readonly string MovementDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetMovementDetails";

        /// <summary>
        /// The inventory details procedure name.
        /// </summary>
        public static readonly string InventoryDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetInventoryDetails";

        /// <summary>
        /// The node connection configuration details procedure name.
        /// </summary>
        public static readonly string NodeConnectionConfigurationDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetConnectionConfigurationDetails";

        /// <summary>
        /// The node configuration details procedure name.
        /// </summary>
        public static readonly string NodeConfigurationDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetNodeConfigurationDetails";

        /// <summary>
        /// The previous inventory details procedure name.
        /// </summary>
        public static readonly string PreviousInventoryDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetInitialInventoryPropertyDetails";

        /// <summary>
        /// The previous movement details procedure name.
        /// </summary>
        public static readonly string PreviousMovementDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetInputMovementPropertyDetails";

        /// <summary>
        /// The save segment node details.
        /// </summary>
        public static readonly string SaveSegmentNodeDetails = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveOperationNode";

        /// <summary>
        /// The get transfer point movement details.
        /// </summary>
        public static readonly string GetTransferPointMovementDetails = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetPendingTransferMovement";

        /// <summary>
        /// The node calculation errors view.
        /// </summary>
        public static readonly string NodeCalculationErrorsView = $"{DataAccess.Sql.Constants.AdminSchema}.view_CalculationErrors";

        /// <summary>
        /// The event data.
        /// </summary>
        public static readonly string EventDataProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetEventDetails";

        /// <summary>
        /// The contract data.
        /// </summary>
        public static readonly string ContractDataProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetContractDetails";

        /// <summary>
        /// The cancellation movement data.
        /// </summary>
        public static readonly string CancellationMovementDataProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetCancellationMovementDetails";

        /// <summary>
        /// The delete movements procedure name.
        /// </summary>
        public static readonly string DeleteMovementsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_DeleteMovements";

        /// <summary>
        /// The unbalance data table.
        /// </summary>
        public static readonly string NodeListType = $"{DataAccess.Sql.Constants.AdminSchema}.NodeListType";

        /// <summary>
        /// The conciliation nodes data table.
        /// </summary>
        public static readonly string ConciliationNodeList = $"{DataAccess.Sql.Constants.AdminSchema}.ConciliationNodeList";

        /// <summary>
        /// The unbalance data table.
        /// </summary>
        public static readonly string MovementListType = $"{DataAccess.Sql.Constants.AdminSchema}.MovementListType";

        /// <summary>
        /// The movement node table type.
        /// </summary>
        public static readonly string MovementNodeType = $"{DataAccess.Sql.Constants.AdminSchema}.MovementNodeType";

        /// <summary>
        /// The unbalance data table.
        /// </summary>
        public static readonly string NodeIdColumnName = "NodeId";

        /// <summary>
        /// The source node data table.
        /// </summary>
        public static readonly string SourceNodeIdColumnName = "SourceNodeId";

        /// <summary>
        /// The destination node data table.
        /// </summary>
        public static readonly string DestinationNodeIdColumnName = "DestinationNodeId";

        /// <summary>
        /// The Transaccion Id data table.
        /// </summary>
        public static readonly string TransaccionIdColumnName = "MovementTransactionId";

        /// <summary>
        /// The get nodes procedure name.
        /// </summary>
        public static readonly string GetNodesProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetAllNodes";

        /// <summary>
        /// The get segment nodes procedure name.
        /// </summary>
        public static readonly string GetSegmentNodesProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetNodesForSegment";

        /// <summary>
        /// The get segment nodes procedure name.
        /// </summary>
        public static readonly string GetInitialNodesProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetInitialNodes";

        /// <summary>
        /// The get segment nodes procedure name.
        /// </summary>
        public static readonly string GetFinalNodesProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetFinalNodes";

        /// <summary>
        /// The get segment nodes procedure name.
        /// </summary>
        public static readonly string GetAllSystemsInSegmentProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetAllSystemInASegment";

        /// <summary>
        /// The get segment nodes procedure name.
        /// </summary>
        public static readonly string GetAllNodesInSystemProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetAllNodesInASystem";

        /// <summary>
        /// The inventory details procedure name.
        /// </summary>
        public static readonly string GetInventoriesProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetInventories";

        /// <summary>
        /// The movements procedure name.
        /// </summary>
        public static readonly string GetMovementsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetMovements";

        /// <summary>
        /// The logistics movement details procedure name.
        /// </summary>
        public static readonly string LogisticMovementDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetLogisticMovementDetails";

        /// <summary>
        /// The logistic inventory details procedure name.
        /// </summary>
        public static readonly string LogisticInventoryDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetLogisticInventoryDetails";

        /// <summary>
        /// The get error details procedure name.
        /// </summary>
        public static readonly string GetErrorDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetErrorDetails";

        /// <summary>
        /// The get ownership node balance summary procedure name.
        /// </summary>
        public static readonly string GetOwnershipNodeBalanceSummaryProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_BalanceSummaryWithOwnership";

        /// <summary>
        /// The get ownership node movement inventory details procedure name.
        /// </summary>
        public static readonly string GetOwnershipNodeMovementInventoryDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_OwnershipNodeMovementInventoryDetails";

        /// <summary>
        /// The save operative node relationship procedure name.
        /// </summary>
        public static readonly string SaveOperativeNodeRelationship = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveOperativeNodeRelationship";

        /// <summary>
        /// The save operational data without cut off.
        /// </summary>
        public static readonly string SaveOperationalDataWithoutCutOffForReportProcedure = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveOperationalDataWithoutCutOffForReport";

        /// <summary>
        /// The save non son segment inventory details.
        /// </summary>
        public static readonly string SaveNonSonSegmentInventoryDetails = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveNonSonSegmentInventoryDetails";

        /// <summary>
        /// The save non son segment inventory quality details.
        /// </summary>
        public static readonly string SaveNonSonSegmentInventoryQualityDetails = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveNonSonSegmentInventoryQualityDetails";

        /// <summary>
        /// The save non son segment movement details.
        /// </summary>
        public static readonly string SaveNonSonSegmentMovementDetails = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveNonSonSegmentMovementDetails";

        /// <summary>
        /// The save non son segment movement quality details.
        /// </summary>
        public static readonly string SaveNonSonSegmentMovementQualityDetails = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveNonSonSegmentMovementQualityDetails";

        /// <summary>
        /// The save non son segment.
        /// </summary>
        public static readonly string SaveNonSonSegment = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveNonSonSegment";

        /// <summary>
        /// The balance summary aggregate procedure name.
        /// </summary>
        public static readonly string BalanceSummaryAggregate = $"{DataAccess.Sql.Constants.AdminSchema}.usp_BalanceSummaryAggregate";

        /// <summary>
        /// The update official movement procedure name.
        /// </summary>
        public static readonly string UpdateOfficialMovementPendingApproval = $"{DataAccess.Sql.Constants.AdminSchema}.usp_UpdateOfficialMovementPendingApproval";

        /// <summary>
        /// The node type category name.
        /// </summary>
        public static readonly string NodeTypeCategoryName = "Tipo de Nodo";

        /// <summary>
        /// The graphical node procedure name.
        /// </summary>
        public static readonly string GraphicalNodeProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetGraphicalNode";

        /// <summary>
        /// The graphical node connection procedure name.
        /// </summary>
        public static readonly string GraphicalNodeConnectionProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetGraphicalNodeConnection";

        /// <summary>
        /// The bulk update rules procedure name.
        /// </summary>
        public static readonly string BulkUpdateRulesProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_BulkUpdateRules";

        /// <summary>
        /// The graphical node procedure name.
        /// </summary>
        public static readonly string ValidateInitialInventoriesForOwnershipProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_ValidateInitialInventoriesForOwnership";

        /// <summary>
        /// The node Id.
        /// </summary>
        public static readonly string KeyTypeColumnName = "Key";

        /// <summary>
        /// The unbalance data table.
        /// </summary>
        public static readonly string KeyType = $"{DataAccess.Sql.Constants.AdminSchema}.KeyType";

        /// <summary>
        /// The graphical node procedure name.
        /// </summary>
        public static readonly string GraphicalSourceNodesDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetGraphicalSourceNodesDetails";

        /// <summary>
        /// The graphical node connection procedure name.
        /// </summary>
        public static readonly string GraphicalSourceNodeConnectionsDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetGraphicalSourceNodeConnectionsDetails";

        /// <summary>
        /// The graphical node procedure name.
        /// </summary>
        public static readonly string GraphicalDestinationNodesDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetGraphicalDestinationNodesDetails";

        /// <summary>
        /// The graphical node connection procedure name.
        /// </summary>
        public static readonly string GraphicalDestinationNodeConnectionsDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetGraphicalDestinationNodeConnectionsDetails";

        /// <summary>
        /// The cleanup ownership and operational cut off data.
        /// </summary>
        public static readonly string OperationalCutOffAndOwnershipCleanupProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_OperationalCutOffAndOwnershipCleanup";

        /// <summary>
        /// The ticket node status procedure name.
        /// </summary>
        public static readonly string TicketNodeStatusProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetTicketNodeStatus ";

        /// <summary>
        /// The sap mapping detail procedure name.
        /// </summary>
        public static readonly string SapMappingDetailProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetSapMappingDetail ";

        /// <summary>
        /// The event contract report request procedure name.
        /// </summary>
        public static readonly string EventContractReportRequestProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetEventContractInformation";

        /// <summary>
        /// The event contract report request procedure name.
        /// </summary>
        public static readonly string MovementSendToSapReportRequestProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetMovementSendSapInformation";

        /// <summary>
        /// The ownership fail and cleanup procedure name.
        /// </summary>
        public static readonly string OwnershipFailAndCleanupProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_FailOwnershipTicket";

        /// <summary>
        /// The cutoff fail and cleanup procedure name.
        /// </summary>
        public static readonly string CutoffFailAndCleanupProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_FailCutoffTicket";

        /// <summary>
        /// The node configuration report request procedure name.
        /// </summary>
        public static readonly string NodeConfigurationReportRequestProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetNodeDetailsInformationForReport";

        /// <summary>
        /// The node configuration report request procedure name.
        /// </summary>
        public static readonly string NodeOficialStatusReportProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetNodeOficialStatus";

        /// <summary>
        /// The save operative movements with ownership percentage procedure name.
        /// </summary>
        public static readonly string SaveOperativeMovements = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveOperativeMovementsWithOwnershipPercentage";

        /// <summary>
        /// The original or updated inventories procedure name.
        /// </summary>
        public static readonly string OriginalOrUpdatedInventoriesProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetOriginalORUpdatedInventories";

        /// <summary>
        /// The original or updated movements procedure name.
        /// </summary>
        public static readonly string OriginalOrUpdatedMovementsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetOriginalORUpdatedMovements";

        /// <summary>
        /// The get official transfer point movements.
        /// </summary>
        public static readonly string GetOfficialTransferPointMovements = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetTransferPoints";

        /// <summary>
        /// The get delta exceptional details procedure name.
        /// </summary>
        public static readonly string GetDeltaExceptionsDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetDeltaErrorDetailsForMovAndInventories";

        /// <summary>
        /// The validate transfer point.
        /// </summary>
        public static readonly string ValidateTransferPoint = $"{DataAccess.Sql.Constants.AdminSchema}.usp_ValidateTransferPoint";

        /// <summary>
        /// The sap tracking type.
        /// </summary>
        public static readonly string SapTrackingType = $"{DataAccess.Sql.Constants.AdminSchema}.SapTrackingType";

        /// <summary>
        /// The update cut off comment procedure name.
        /// </summary>
        public static readonly string UpdateCutOffCommentProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_UpdateCutOffComment";

        /// <summary>
        /// The approve official node delta procedure name.
        /// </summary>
        public static readonly string ApproveOfficialNodeDeltaProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_ApproveOfficialNodeDelta";

        /// <summary>
        /// The reopen official node delta procedure name.
        /// </summary>
        public static readonly string GetDependentsOfficialNodeDeltaProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetDependentsOfOfficialNodeDelta";

        /// <summary>
        /// The cache region name.
        /// </summary>
        public static readonly string CacheRegionName = "East US";

        /// <summary>
        /// The get upload details sap.
        /// </summary>
        public static readonly string GetUploadDetailsSap = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetUploadDetailsSAP";

        /// <summary>
        /// The get upload contract details sap.
        /// </summary>
        public static readonly string GetUploadContractDetailsSap = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetUploadContractDetailsSAP";

        /// <summary>
        /// The segment category name in spanish.
        /// </summary>
        public static readonly string SegmentCategoryNameEs = "Segmento";

        /// <summary>
        /// The segment category name in english.
        /// </summary>
        public static readonly string SegmentCategoryNameEn = "Segment";

        /// <summary>
        /// The system category name in english.
        /// </summary>
        public static readonly string SystemCategoryNameEn = "System";

        /// <summary>
        /// The save operational data without cut off stored procedure.
        /// </summary>
        public static readonly string SaveOperationalDataWithoutCutOffProcedure = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveOperationalDataWithoutCutOffFor";

        /// <summary>
        /// The save operational movement data without cut off stored procedure.
        /// </summary>
        public static readonly string SaveOperationalMovementWithoutCutOffProcedure = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveOperationalMovementWithoutCutOffFor";

        /// <summary>
        /// The save inventory details without cut off stored procedure.
        /// </summary>
        public static readonly string SaveInventoryDetailsWithoutCutOffProcedure = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveInventoryDetailsWithoutCutOffFor";

        /// <summary>
        /// The save operational movement owner data without cut off stored procedure.
        /// </summary>
        public static readonly string SaveOperationalMovementOwnerWithoutCutOffProcedure = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveOperationalMovementOwnerWithoutCutOffFor";

        /// <summary>
        /// The save operational movement quality data without cut off stored procedure.
        /// </summary>
        public static readonly string SaveOperationalMovementQualityWithoutCutOffProcedure = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveOperationalMovementQualityWithoutCutOffFor";

        /// <summary>
        /// The save inventory quality details without cut off stored procedure.
        /// </summary>
        public static readonly string SaveInventoryQualityDetailsWithoutCutOffProcedure = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveInventoryQualityDetailsWithoutCutOffFor";

        /// <summary>
        /// The save inventory owner details without cut off stored procedure.
        /// </summary>
        public static readonly string SaveInventoryOwnerDetailsWithoutCutOffProcedure = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveInventoryOwnerDetailsWithoutCutOffFor";

        /// <summary>
        /// The get movements for consolidation.
        /// </summary>
        public static readonly string GetMovementsForConsolidation = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetMovementsForConsolidation";

        /// <summary>
        /// The get movement nodes for consolidation.
        /// </summary>
        public static readonly string GetMovementNodesForConsolidation = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetMovementNodesForConsolidation";

        /// <summary>
        /// Get official delta movements.
        /// </summary>
        public static readonly string GetOfficialDeltaMovements = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetOfficialBalanceMovements";

        /// <summary>
        /// Get official GetOfficialDeltaInventories.
        /// </summary>
        public static readonly string GetOfficialDeltaInventories = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetOfficialBalanceInventories";

        /// <summary>
        /// Get pending official movements.
        /// </summary>
        public static readonly string GetPendingNodeOfficialDeltaMovements = $"{DataAccess.Sql.Constants.AdminSchema}.usp_UpdateNodeAndGetDeltaMovements";

        /// <summary>
        /// The complete consolidation.
        /// </summary>
        public static readonly string CompleteConsolidation = $"{DataAccess.Sql.Constants.AdminSchema}.usp_CompleteConsolidation";

        /// <summary>
        /// The get official delta period.
        /// </summary>
        public static readonly string GetOfficialDeltaPeriod = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetOfficialDeltaPeriod";

        /// <summary>
        /// The get inventories for consolidation.
        /// </summary>
        public static readonly string GetInventoriesForConsolidation = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetInventoriesForConsolidation";

        /// <summary>
        /// The consolidation data cleanup.
        /// </summary>
        public static readonly string ConsolidationDataCleanup = $"{DataAccess.Sql.Constants.AdminSchema}.usp_ConsolidationDataCleanup";

        /// <summary>
        /// The get official delta exceptional details procedure name.
        /// </summary>
        public static readonly string GetOfficialDeltaExceptionsDetailsProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetOfficialDeltaErrorDetailsForNode";

        /// <summary>
        /// The save monthly official data without cut off report.
        /// </summary>
        public static readonly string SaveMonthlyOfficialDataWithoutCutOffReport = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMonthlyOfficialDataWithoutCutOffReport";

        /// <summary>
        /// The save monthly official data without cutoff.
        /// </summary>
        public static readonly string SaveMonthlyOfficialDataWithoutCutoff = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMonthlyOfficialDataWithoutCutoff";

        /// <summary>
        /// The save monthly official movement details without cut off.
        /// </summary>
        public static readonly string SaveMonthlyOfficialMovementDetailsWithoutCutOff = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMonthlyOfficialMovementDetailsWithoutCutOff";

        /// <summary>
        /// The save monthly official inventory details without cut off.
        /// </summary>
        public static readonly string SaveMonthlyOfficialInventoryDetailsWithoutCutOff = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMonthlyOfficialInventoryDetailsWithoutCutOff";

        /// <summary>
        /// The save monthly official inventory quality details without cut off.
        /// </summary>
        public static readonly string SaveMonthlyOfficialInventoryQualityDetailsWithoutCutOff = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMonthlyOfficialInventoryQualityDetailsWithoutCutOff";

        /// <summary>
        /// The save monthly official movement quality details without cut off.
        /// </summary>
        public static readonly string SaveMonthlyOfficialMovementQualityDetailsWithoutCutOff = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMonthlyOfficialMovementQualityDetailsWithoutCutOff";

        /// <summary>
        /// The save monthly official movement quality details without cut off.
        /// </summary>
        public static readonly string SaveMonthlyConsolidatedDeltaDataWithoutCutOffReport = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMonthlyConsolidatedDeltaDataWithoutCutOffReport";

        /// <summary>
        /// The save monthly consolidated delta data.
        /// </summary>
        public static readonly string SaveMonthlyConsolidatedDeltaData = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMonthlyConsolidatedDeltaData";

        /// <summary>
        /// The save monthly consolidated delta movement details without cut off.
        /// </summary>
        public static readonly string SaveMonthlyConsolidatedDeltaMovementDetailsWithoutCutOff = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMonthlyConsolidatedDeltaMovementDetailsWithoutCutOff";

        /// <summary>
        /// The save monthly consolidated delta inventory details without cut off.
        /// </summary>
        public static readonly string SaveMonthlyConsolidatedDeltaInventoryDetailsWithoutCutOff = $"{DataAccess.Sql.Constants.AdminSchema}." +
            $"                                                                                      usp_SaveMonthlyConsolidatedDeltaInventoryDetailsWithoutCutOff";

        /// <summary>
        /// The validate previous official period.
        /// </summary>
        public static readonly string ValidatePreviousOfficialPeriod = $"{DataAccess.Sql.Constants.AdminSchema}.usp_ValidatePreviousOfficialPeriod";

        /// <summary>
        /// The purge CutOff report data procedure name.
        /// </summary>
        public static readonly string PurgeCutoffreportdataProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_Cleanup_CutOff_ReportData";

        /// <summary>
        /// The save attribute details.
        /// </summary>
        public static readonly string SaveAttributeDetails = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveAttributeDetails";

        /// <summary>
        /// The save backup movement details.
        /// </summary>
        public static readonly string SaveBackupMovementDetails = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveBackupMovementDetails";

        /// <summary>
        /// The save movement details.
        /// </summary>
        public static readonly string SaveMovementDetails = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMovementDetails";

        /// <summary>
        /// The save inventory details.
        /// </summary>
        public static readonly string SaveInventoryDetails = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveInventoryDetails";

        /// <summary>
        /// The Save KPI data by category element node.
        /// </summary>
        public static readonly string SaveKPIDataByCategoryElementNode = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveKPIDataByCategoryElementNode";

        /// <summary>
        /// The save KPI previous date data by category element node.
        /// </summary>
        public static readonly string SaveKPIPreviousDateDataByCategoryElementNode = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveKPIPreviousDateDataByCategoryElementNode";

        /// <summary>
        /// The save movements by product.
        /// </summary>
        public static readonly string SaveMovementsByProduct = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMovementsByProduct";

        /// <summary>
        /// The save quality details.
        /// </summary>
        public static readonly string SaveQualityDetails = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveQualityDetails";

        /// <summary>
        /// The save attribute details with owner.
        /// </summary>
        public static readonly string DeleteMovementInformationForReport = $"{DataAccess.Sql.Constants.AdminSchema}.usp_DeleteMovementInformationForReport";

        /// <summary>
        /// The purge Ownership report data procedure name.
        /// </summary>
        public static readonly string PurgeOwnershipreportdataProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_Cleanup_Ownership_ReportData";

        /// <summary>
        /// The save attribute details with owner.
        /// </summary>
        public static readonly string SaveAttributeDetailsWithOwner = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveAttributeDetailsWithOwner";

        /// <summary>
        /// The save backup movement details with owner.
        /// </summary>
        public static readonly string SaveBackupMovementDetailsWithOwner = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveBackupMovementDetailsWithOwner";

        /// <summary>
        /// The save movement details with owner.
        /// </summary>
        public static readonly string SaveMovementDetailsWithOwner = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMovementDetailsWithOwner";

        /// <summary>
        /// The save movement details with owner.
        /// </summary>
        public static readonly string SaveMovementDetailsWithOwnerOtherSegment = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMovementDetailsWithOwnerOtherSegment";

        /// <summary>
        /// The save inventory details with owner.
        /// </summary>
        public static readonly string SaveInventoryDetailsWithOwner = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveInventoryDetailsWithOwner";

        /// <summary>
        /// The save KPI data by category element node with ownership.
        /// </summary>
        public static readonly string SaveKPIDataByCategoryElementNodeWithOwnership = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveKPIDataByCategoryElementNodeWithOwnership";

        /// <summary>
        /// The save KPI previous date data by category element node with owner.
        /// </summary>
        public static readonly string SaveKPIPreviousDateDataByCategoryElementNodeWithOwner = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveKPIPreviousDateDataByCategoryElementNodeWithOwner";

        /// <summary>
        /// The save movements by product with owner.
        /// </summary>
        public static readonly string SaveMovementsByProductWithOwner = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMovementsByProductWithOwner";

        /// <summary>
        /// The save quality details with owner.
        /// </summary>
        public static readonly string SaveQualityDetailsWithOwner = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveQualityDetailsWithOwner";

        /// <summary>
        /// The save official delta balance.
        /// </summary>
        public static readonly string SaveOfficialDeltaBalance = $"{DataAccess.Sql.Constants.ReportSchema}.usp_SaveOfficialDeltaBalance";

        /// <summary>
        /// The save official delta balance.
        /// </summary>
        public static readonly string SaveOfficialDeltaMovementDetails = $"{DataAccess.Sql.Constants.ReportSchema}.usp_SaveOfficialDeltaMovementDetails";

        /// <summary>
        /// The save official delta balance.
        /// </summary>
        public static readonly string SaveOfficialDeltaInventoryDetails = $"{DataAccess.Sql.Constants.ReportSchema}.usp_SaveOfficialDeltaInventoryDetails";

        /// <summary>
        /// The get unapproved official nodes.
        /// </summary>
        public static readonly string GetUnapprovedOfficialNodes = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetUnapprovedOfficialNodes";

        /// <summary>
        /// The get official delta inventory.
        /// </summary>
        public static readonly string GetDeltaOfficialInventory = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetDeltaOfficialInventory";

        /// <summary>
        /// The get official delta movements.
        /// </summary>
        public static readonly string GetDeltaOfficialMovement = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetDeltaOfficialMovements";

        /// <summary>
        /// The get official delta movements for nodes.
        /// </summary>
        public static readonly string GetDeltaOfficialMovementNodes = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetDeltaOfficialMovementsForNodes";

        /// <summary>
        /// The get official logistics movement.
        /// </summary>
        public static readonly string GetOfficialLogisticsMovement = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetOfficialLogisticsMovements";

        /// <summary>
        /// The purge operational data without history procedure name.
        /// </summary>
        public static readonly string PurgeOperationalDateWithoutCutoffProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_Cleanup_OperationalDataWithoutCutOff";

        /// <summary>
        /// The purge monthly official data without cut off procedure name.
        /// </summary>
        public static readonly string PurgeMonthlyOfficialDataWithoutCutOffProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_Cleanup_MonthlyOfficialDataWithoutCutOff";

        /// <summary>
        /// The purge non son segment data without cut off procedure name.
        /// </summary>
        public static readonly string PurgeNonSonSegmentDataWithoutCutOffProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_Cleanup_NonSonSegmentDataWithoutCutOff";

        /// <summary>
        /// The save balance control.
        /// </summary>
        public static readonly string SaveBalanceControl = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveBalanceControl";

        /// <summary>
        /// The get first time nodes procedure name.
        /// </summary>
        public static readonly string GetFirstTimeNodesProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetFirstTimeNodes";

        /// <summary>
        /// The purge inventory movement index procedure name.
        /// </summary>
        public static readonly string PurgeInventoryMovementIndexProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_Cleanup_InventoryMovementIndex";

        /// <summary>
        /// The update movement with official delta ticket.
        /// </summary>
        public static readonly string UpdateMovementWithOfficialDeltaTicket = $"{DataAccess.Sql.Constants.AdminSchema}.usp_UpdateMovementWithOfficialDeltaTicket";

        /// <summary>
        /// The update inventory product with official delta ticket.
        /// </summary>
        public static readonly string UpdateInventoryProductWithOfficialDeltaTicket = $"{DataAccess.Sql.Constants.AdminSchema}.usp_UpdateInventoryProductWithOfficialDeltaTicket";

        /// <summary>
        /// The save delta bulk movements.
        /// </summary>
        public static readonly string SaveDeltaBulkMovements = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveDeltaBulkMovements";

        /// <summary>
        /// The movement type.
        /// </summary>
        public static readonly string MovementType = $"{DataAccess.Sql.Constants.AdminSchema}.MovementType";

        /// <summary>
        /// The movement owner type.
        /// </summary>
        public static readonly string MovementOwnerType = $"{DataAccess.Sql.Constants.AdminSchema}.MovementOwnerType";

        /// <summary>
        /// The validate logistic node available.
        /// </summary>
        public static readonly string GetLogisticMovementNodeValidations = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetLogisticMovementNodeValidations";

        /// <summary>
        /// The read nodes conexion point.
        /// </summary>
        public static readonly string GetConnectionConciliationNodes = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetConnectionConciliationNodes";

        /// <summary>
        /// The Update Confirmed Logistic Movements.
        /// </summary>
        public static readonly string UpdateConfirmedLogisticMovements = $"{DataAccess.Sql.Constants.AdminSchema}.usp_UpdateConfirmedLogisticMovements";

        /// <summary>
        /// The logistict movement detail procedure.
        /// </summary>
        public static readonly string LogisticMovementDetailsForTicketProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetLogisticMovementsForTicket";

        /// <summary>
        /// The falied logistict movement detail procedure.
        /// </summary>
        public static readonly string FailedLogisticMovementProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetFailedLogisticMovement";

        /// <summary>
        /// The get conciliation transfer point movements.
        /// </summary>
        public static readonly string GetTransferPointsForConciliationNodes = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetTransferPointsForConciliationNodes";

        /// <summary>
        /// The calculate date for delta movements.
        /// </summary>
        public static readonly string CalculateDateForDeltaMovements = $"{DataAccess.Sql.Constants.AdminSchema}.usp_CalculateDateForDeltaMovements";

        /// <summary>
        /// The users and groups procedure name.
        /// </summary>
        public static readonly string UsersGroupsReportRequestProcedureName = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetUserGroup";

        /// <summary>
        /// The users groups report request table.
        /// </summary>
        public static readonly string UsersGroupsReportRequestTable = $"{DataAccess.Sql.Constants.AdminSchema}.UserRoleType";

        /// <summary>
        /// The users groups report request table.
        /// </summary>
        public static readonly string MenusRolDetailsReportRequestTable = $"{DataAccess.Sql.Constants.AdminSchema}.usp_SaveMenusRolDetails";

        /// <summary>
        /// The get first Storage for nodes.
        /// </summary>
        public static readonly string GetStorageNode = $"{DataAccess.Sql.Constants.AdminSchema}.usp_GetStorageNode";

        /// <summary>
        /// The get nodes status different to deltas.
        /// </summary>
        public static readonly string ValidateNodesStateDifferentDeltas = $"{DataAccess.Sql.Constants.AdminSchema}.usp_ValidateNodesStateDifferentDeltas";
    }
}
