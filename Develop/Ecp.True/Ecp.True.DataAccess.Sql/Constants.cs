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

namespace Ecp.True.DataAccess.Sql
{
    /// <summary>
    /// The SQL constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The SQL server violation of unique constraint.
        /// </summary>
        public static readonly int UniqueConstraintCode = 2627;

        /// <summary>
        /// The identity insert code.
        /// </summary>
        public static readonly int IdentityInsertCode = 544;

        /// <summary>
        /// The foreign keyconstraints.
        /// </summary>
        public static readonly int ForeignKeyConstraintCode = 547;

        /// <summary>
        /// The not null constraint code.
        /// </summary>
        public static readonly int NotNullConstraintCode = 515;

        /// <summary>
        /// The one or more storage locations does not exists code.
        /// </summary>
        public static readonly int OneOrMoreStorageLocationsDoesNotExistsCode = 50001;

        /// <summary>
        /// The one or more products does not belong to database storage location code.
        /// </summary>
        public static readonly int OneOrMoreProductsDoesNotBelongToDatabaseStorageLocationCode = 50002;

        /// <summary>
        /// The one or more products does not belong to datatable storage location code.
        /// </summary>
        public static readonly int OneOrMoreProductsDoesNotBelogToDatatableStorageLocationCode = 50003;

        /// <summary>
        /// The duplicate node storage location identifier code.
        /// </summary>
        public static readonly int DuplicateNodeStorageLocationIdCode = 50004;

        /// <summary>
        /// The duplicate storage location product identifier code.
        /// </summary>
        public static readonly int DuplicateStorageLocationProductIdCode = 50005;

        /// <summary>
        /// The storage location name already exists within node code.
        /// </summary>
        public static readonly int StorageLocationNameAlreadyExistsWithinNodeCode = 50006;

        /// <summary>
        /// The duplicate storage location name for same node.
        /// </summary>
        public static readonly int DuplicateStorageLocationNameForSameNode = 50007;

        /// <summary>
        /// The duplicate row version.
        /// </summary>
        public static readonly int ConcurrentUpdate = 50008;

        /// <summary>
        /// The admin schema.
        /// </summary>
        public static readonly string AdminSchema = "admin";

        /// <summary>
        /// The off chain schema.
        /// </summary>
        public static readonly string OffchainSchema = "offchain";

        /// <summary>
        /// The analytics schema.
        /// </summary>
        public static readonly string AnalyticsSchema = "analytics";

        /// <summary>
        /// The audit schema.
        /// </summary>
        public static readonly string AuditSchema = "audit";

        /// <summary>
        /// The homologation error code.
        /// </summary>
        public static readonly int SqlCustomErrorCode = 51000;

        /// <summary>
        /// The duplicate node name error code.
        /// </summary>
        public static readonly int NodeNameMustBeUnique = 51001;

        /// <summary>
        /// The node calculation errors view.
        /// </summary>
        public static readonly string NodeCalculationErrorsView = "view_CalculationErrors";

        /// <summary>
        /// The no sap homologation for movement type.
        /// </summary>
        public static readonly string NoSapHomologationForMovementType = "NOT_SAP_HOMOLOGATION_FOUND_FOR_MOV_TYPE";

        /// <summary>
        /// The invalid combination to siv movement.
        /// </summary>
        public static readonly string InvalidCombinationToSivMovement = "INVALID_COMBINATION_TO_SIV_MOVEMENT";

        /// <summary>
        /// The tickets view.
        /// </summary>
        public static readonly string TicketsView = "view_Ticket";

        /// <summary>
        /// The ownership node data view.
        /// </summary>
        public static readonly string OwnershipNodeDataView = "view_OwnerShipNode";

        /// <summary>
        /// The ownership node data view.
        /// </summary>
        public static readonly string NodeConnectionProductRuleView = "view_NodeConnectionProductRule";

        /// <summary>
        /// The exception view.
        /// </summary>
        public static readonly string ExceptionView = "view_GetParsingErrors";

        /// <summary>
        /// The node rules view.
        /// </summary>
        public static readonly string NodeRulesView = "view_NodeRule";

        /// <summary>
        /// The node product rule.
        /// </summary>
        public static readonly string NodeProductRuleView = "view_NodeProductRule";

        /// <summary>
        /// The row concurrency conflict.
        /// </summary>
        public static readonly int RowConcurrencyConflict = 409;

        /// <summary>
        /// The delta node view.
        /// </summary>
        public static readonly string DeltaNodeView = "view_DeltaNode";

        /// <summary>
        /// The report execution view.
        /// </summary>
        public static readonly string ReportExecutionView = "view_ReportExecution";

        /// <summary>
        /// The report schema.
        /// </summary>
        public static readonly string ReportSchema = "report";
    }
}
