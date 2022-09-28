// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueueConstants.cs" company="Microsoft">
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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The QueueConstants.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class QueueConstants
    {
        /// <summary>
        /// Gets the consolidation queue.
        /// </summary>
        /// <value>
        /// The consolidation queue.
        /// </value>
        public const string ConsolidationQueue = "consolidation";

        /// <summary>
        /// Gets the official delta queue.
        /// </summary>
        /// <value>
        /// The official delta queue.
        /// </value>
        public const string OfficialDeltaQueue = "officialdelta";

        /// <summary>
        /// Gets the homologated movements queue.
        /// </summary>
        /// <value>
        /// The homologated movements queue.
        /// </value>
        public static string HomologatedMovementsQueue => "homologatedmovements";

        /// <summary>
        /// Gets the homologated inventory queue.
        /// </summary>
        /// <value>
        /// The homologated inventory queue.
        /// </value>
        public static string HomologatedInventoryQueue => "homologatedinventory";

        /// <summary>
        /// Gets the homologated events queue.
        /// </summary>
        /// <value>
        /// The homologated events queue.
        /// </value>
        public static string HomologatedEventsQueue => "homologatedevents";

        /// <summary>
        /// Gets the homologated contracts queue.
        /// </summary>
        /// <value>
        /// The homologated contracts queue.
        /// </value>
        public static string HomologatedContractsQueue => "homologatedcontracts";

        /// <summary>
        /// Gets the movements queue.
        /// </summary>
        /// <value>
        /// The movements queue.
        /// </value>
        public static string MovementsQueue => "movements";

        /// <summary>
        /// Gets the losses queue.
        /// </summary>
        /// <value>
        /// The losses queue.
        /// </value>
        public static string LossesQueue => "losses";

        /// <summary>
        /// Gets the special movements queue.
        /// </summary>
        /// <value>
        /// The special movements queue.
        /// </value>
        public static string SpecialMovementsQueue => "specialmovements";

        /// <summary>
        /// Gets the inventory queue.
        /// </summary>
        /// <value>
        /// The inventory queue.
        /// </value>
        public static string InventoryQueue => "inventory";

        /// <summary>
        /// Gets the movement and inventory excel queue.
        /// </summary>
        /// <value>
        /// The movement and inventory excel queue.
        /// </value>
        public static string MovementAndInventoryExcelQueue => "excel";

        /// <summary>
        /// Gets the excel event queue.
        /// </summary>
        /// <value>
        /// The excel event queue.
        /// </value>
        public static string ExcelEventQueue => "excelevent";

        /// <summary>
        /// Gets the excel contract queue.
        /// </summary>
        /// <value>
        /// The excel contract queue.
        /// </value>
        public static string ExcelContractQueue => "excelcontract";

        /// <summary>
        /// Gets the retry json queue.
        /// </summary>
        /// <value>
        /// The retry json queue.
        /// </value>
        public static string RetryJsonQueue => "json";

        /// <summary>
        /// Gets the reports data queue.
        /// </summary>
        /// <value>
        /// The reports data queue.
        /// </value>
        public static string ReportsDataQueue => "data";

        /// <summary>
        /// Gets the flow approval queue.
        /// </summary>
        /// <value>
        /// The flow approval queue.
        /// </value>
        public static string FlowApprovalQueue => "flowapprovals";

        /// <summary>
        /// Gets the calculate ownership queue.
        /// </summary>
        /// <value>
        /// The calculate ownership queue.
        /// </value>
        public static string CalculateOwnershipQueue => "calculateOwnership";

        /// <summary>
        /// Gets the calculate ownership queue.
        /// </summary>
        /// <value>
        /// The calculate ownership queue.
        /// </value>
        public static string RecalculateOwnershipBalanceQueue => "recalculateownershipbalance";

        /// <summary>
        /// Gets the conciliation queue.
        /// </summary>
        /// <value>
        /// The conciliation queue.
        /// </value>
        public static string ConciliationQueue => "conciliationqueue";

        /// <summary>
        /// Gets the ownership rule queue.
        /// </summary>
        /// <value>
        /// The ownership rule queue.
        /// </value>
        public static string OwnershipRuleQueue => "ownershiprulessync";

        /// <summary>
        /// Gets the ownership queue.
        /// </summary>
        /// <value>
        /// The ownership queue.
        /// </value>
        public static string OwnershipQueue => "ownership";

        /// <summary>
        /// Gets the approval queue.
        /// </summary>
        /// <value>
        /// The approval queue.
        /// </value>
        public static string ApprovalQueue => "approvals";

        /// <summary>
        /// Gets the logistics queue.
        /// </summary>
        /// <value>
        /// The logistics queue.
        /// </value>
        public static string LogisticsQueue => "logistics";

        /// <summary>
        /// Gets the operational cutoff queue.
        /// </summary>
        /// <value>
        /// The operational cutoff queue.
        /// </value>
        public static string OperationalCutoffQueue => "operationalcutoff";

        /// <summary>
        /// Gets the deadletter queue.
        /// </summary>
        /// <value>
        /// The deadletter queue.
        /// </value>
        public static string DeadletterQueue => "deadletter";

        /// <summary>
        /// Gets the name of the movement identifier target queue.
        /// </summary>
        /// <value>
        /// The name of the movement identifier target queue.
        /// </value>
        public static string BlockchainMovementQueue => "blockchainmovement";

        /// <summary>
        /// Gets the name of the ownership identifier target queue.
        /// </summary>
        /// <value>
        /// The name of the ownership identifier target queue.
        /// </value>
        public static string BlockchainOwnershipQueue => "blockchainownership";

        /// <summary>
        /// Gets the blockchain node queue.
        /// </summary>
        /// <value>
        /// The blockchain node queue.
        /// </value>
        public static string BlockchainNodeQueue => "blockchainnode";

        /// <summary>
        /// Gets the name of the blockchain node connection target.
        /// </summary>
        /// <value>
        /// The name of the blockchain node connection target.
        /// </value>
        public static string BlockchainNodeConnectionQueue => "blockchainnodeconnection";

        /// <summary>
        /// Gets the name of the node identifier second target queue.
        /// </summary>
        /// <value>
        /// The name of the node identifier second target queue.
        /// </value>
        public static string BlockchainNodeProductCalculationQueue => "blockchainnodeproductcalculation";

        /// <summary>
        /// Gets the name of the node identifier second target queue.
        /// </summary>
        /// <value>
        /// The name of the node identifier second target queue.
        /// </value>
        public static string BlockchainInventoryProductQueue => "blockchaininventoryproduct";

        /// <summary>
        /// Gets the delta queue.
        /// </summary>
        /// <value>
        /// The delta queue.
        /// </value>
        public static string DeltaQueue => "delta";

        /// <summary>
        /// Gets the sap queue.
        /// </summary>
        /// <value>
        /// The sap queue.
        /// </value>
        public static string SapQueue => "sap";

        /// <summary>
        /// Gets the offchain queue.
        /// </summary>
        /// <value>
        /// The offchain queue.
        /// </value>
        public static string OffchainQueue => "offchain";

        /// <summary>
        /// Gets the delta approvals queue.
        /// </summary>
        /// <value>
        /// The delta approvals queue.
        /// </value>
        public static string DeltaApprovalsQueue => "deltaapprovals";

        /// <summary>
        /// Gets the delta approvals queue.
        /// </summary>
        /// <value>
        /// The delta approvals queue.
        /// </value>
        public static string BlockChainOfficialQueue => "blockchainofficial";

        /// <summary>
        /// Gets the official logistics queue.
        /// </summary>
        /// <value>
        /// The official logistics queue.
        /// </value>
        public static string OfficialLogisticsQueue => "officiallogistics";

        /// <summary>
        /// Gets the blockchain node queue.
        /// </summary>
        /// <value>
        /// The blockchain node queue.
        /// </value>
        public static string BlockchainOwnerQueue => "blockchainowner";

        /// <summary>
        /// Gets the SapLogisticQueue node queue.
        /// </summary>
        /// <value>
        /// The SapLogisticQueue node queue.
        /// </value>
        public static string SapLogisticQueue => "saplogistic";

        /// <summary>
        /// Gets the recalculate operational cut off.
        /// </summary>
        /// <value>
        /// The recalculate operational cut off node queue.
        /// </value>
        public static string RecalculateOperationalCutoffBalanceQueue => "recalculateoperationalcutoffbalance";
    }
}