// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActivityNames.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core
{
    /// <summary>
    /// The activity names.
    /// </summary>
    public static class ActivityNames
    {
        /// <summary>
        /// The calculate.
        /// </summary>
        public const string Calculate = "Calculate";

        /// <summary>
        /// The approvals.
        /// </summary>
        public const string Register = "Register";

        /// <summary>
        /// The deadletter.
        /// </summary>
        public const string CalculateSegment = "CalculateSegment";

        /// <summary>
        /// The blockchain.
        /// </summary>
        public const string CalculateSystem = "CalculateSystem";

        /// <summary>
        /// The cut off.
        /// </summary>
        public const string Complete = "Complete";

        /// <summary>
        /// The deadletter.
        /// </summary>
        public const string RecalculateCalculateSegment = "RecalculateCalculateSegment";

        /// <summary>
        /// The delete segment balance.
        /// </summary>
        public const string DeleteSegmentBalance = "DeleteSegmentBalance";

        /// <summary>
        /// The recalculate calculate system.
        /// </summary>
        public const string RecalculateCalculateSystem = "RecalculateCalculateSystem";

        /// <summary>
        /// The delete system balance.
        /// </summary>
        public const string DeleteSystemBalance = "DeleteSystemBalance";

        /// <summary>
        /// The cut off.
        /// </summary>
        public const string RecalculateComplete = "RecalculateComplete";

        /// <summary>
        /// The ownership.
        /// </summary>
        public const string FinalizeCutOff = "FinalizeCutOff";

        /// <summary>
        /// The recalculate.
        /// </summary>
        public const string RecalculateFinalizeCutOff = "RecalculateFinalizeCutOff";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string Consolidate = "Consolidate";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string CompleteConsolidation = "CompleteConsolidation";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string GetDelta = "GetDelta";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string RequestDelta = "RequestDelta";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string ProcessDelta = "ProcessDelta";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string CompleteDelta = "CompleteDelta";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string FinalizeDelta = "FinalizeDelta";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string BuildOfficialData = "BuildOfficialData";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string ExcludeData = "ExcludeData";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string RegisterNodeActivity = "RegisterNodeActivity";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string RequestOfficialDelta = "RequestOfficialDelta";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string ProcessOfficialDelta = "ProcessOfficialDelta";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string RegisterMovementsOfficialDelta = "RegisterMovementsOfficialDelta";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string CalculateOfficialDelta = "CalculateOfficialDelta";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string CompleteOfficialDelta = "CompleteOfficialDelta";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string ProcessOfficialLogisticsAnalytics = "ProcessOfficialLogisticsAnalytics";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string ProcessAnalytics = "ProcessAnalytics";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string RequestOwnershipData = "RequestOwnershipData";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string CalculateOwnershipData = "CalculateOwnershipData";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string CalculateOwnershipConciliationData = "CalculateOwnershipConciliationData";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string FinalizeOwnership = "FinalizeOwnership";

        /// <summary>
        /// The FinalizeConciliation.
        /// </summary>
        public const string FinalizeConciliation = "FinalizeConciliation";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string ExcelCanonicalTransform = "ExcelCanonicalTransform";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string ExcelHomologate = "ExcelHomologate";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string ExcelComplete = "ExcelComplete";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string JsonCanonicalTransform = "JsonCanonicalTransform";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string JsonHomologate = "JsonHomologate";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string JsonComplete = "JsonComplete";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string HandleFailure = "HandleFailure";

        /// <summary>
        /// The Consolidation.
        /// </summary>
        public const string HandleConsolidationFailure = "HandleConsolidationFailure";

        /// <summary>
        /// Handle Failure Conciliation.
        /// </summary>
        public const string ConciliationHandleFailure = "ConciliationHandleFailure";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string HandleOfficialDeltaFailure = "HandleOfficialDeltaFailure";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string OfficialLogisticsHandleFailure = "OfficialLogisticsHandleFailure";

        /// <summary>
        /// Start conciliation.
        /// </summary>
        public const string DoConciliation = "DoConciliation";

        /// <summary>
        /// Start conciliation.
        /// </summary>
        public const string ConciliationOwnership = "ConciliationOwnership";

        /// <summary>
        /// Delete Conciliation Movements.
        /// </summary>
        public const string DeleteConciliationMovementsOwnership = "DeleteConciliationMovementsOwnership";

        /// <summary>
        /// Delete Conciliation Movements.
        /// </summary>
        public const string UpdateConciliationOwnershipStatus = "UpdateConciliationOwnershipStatus";

        /// <summary>
        /// Validate Conciliation Node States.
        /// </summary>
        public const string ValidateConciliationNodeStates = "ValidateConciliationNodeStates";

        /// <summary>
        /// Delete Conciliation Movements.
        /// </summary>
        public const string DeleteConciliationMovements = "DeleteConciliationMovements";

        /// <summary>
        /// Update Conciliation Nodes.
        /// </summary>
        public const string UpdateConciliationNodes = "UpdateConciliationNodes";

        /// <summary>
        /// Update Conciliation Status Ticket Conciliation Movements.
        /// </summary>
        public const string UpdateConciliationStatusTicket = "UpdateConciliationStatusTicket";
    }
}
