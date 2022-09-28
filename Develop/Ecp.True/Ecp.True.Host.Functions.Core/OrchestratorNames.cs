// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrchestratorNames.cs" company="Microsoft">
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
    /// The function names.
    /// </summary>
    public static class OrchestratorNames
    {
        /// <summary>
        /// The blockchain.
        /// </summary>
        public const string ConsolidationDataOrchestrator = "ConsolidationDataOrchestrator";

        /// <summary>
        /// The approvals.
        /// </summary>
        public const string OwnershipOrchestrator = "OwnershipOrchestrator";

        /// <summary>
        /// The Conciliation Ownership Orchestrator.
        /// </summary>
        public const string ConciliationOrchestrator = "ConciliationOrchestrator";

        /// <summary>
        /// The deadletter.
        /// </summary>
        public const string OfficialLogisticsOrchestrator = "OfficialLogisticsOrchestrator";

        /// <summary>
        /// The blockchain.
        /// </summary>
        public const string OperationalCutoffOrchestrator = "OperationalCutoffOrchestrator";

        /// <summary>
        /// The recalculate cut off.
        /// </summary>
        public const string RecalculateOperationalCutoffBalanceOrchestrator = "RecalculateOperationalCutoffBalanceOrchestrator";

        /// <summary>
        /// The cut off.
        /// </summary>
        public const string DeltaOrchestrator = "DeltaOrchestrator";

        /// <summary>
        /// The ownership.
        /// </summary>
        public const string OfficialDeltaDataOrchestrator = "OfficialDeltaDataOrchestrator";

        /// <summary>
        /// The sap.
        /// </summary>
        public const string JsonRegistrationOrchestrator = "JsonRegistrationOrchestrator";
    }
}
