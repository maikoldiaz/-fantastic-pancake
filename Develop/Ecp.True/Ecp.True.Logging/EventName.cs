// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventName.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Logging
{
    /// <summary>
    /// The telemetry event names.
    /// </summary>
    public enum EventName
    {
        /// <summary>
        /// The movement reconcile failed.
        /// </summary>
        MovementReconcileFailed,

        /// <summary>
        /// The inventory reconcile failed.
        /// </summary>
        InventoryReconcileFailed,

        /// <summary>
        /// The unbalance reconcile failed.
        /// </summary>
        UnbalanceReconcileFailed,

        /// <summary>
        /// The ownership reconcile failed.
        /// </summary>
        OwnershipReconcileFailed,

        /// <summary>
        /// The node reconcile failed.
        /// </summary>
        NodeReconcileFailed,

        /// <summary>
        /// The node connection reconcile failed.
        /// </summary>
        NodeConnectionReconcileFailed,

        /// <summary>
        /// The offchain synchronize failed.
        /// </summary>
        OffchainSyncFailed,

        /// <summary>
        /// The ownership failure event.
        /// </summary>
        OwnershipFailureEvent,

        /// <summary>
        /// Gets the cutoff failure event.
        /// </summary>
        /// <value>
        /// The cutoff failure event.
        /// </value>
        CutoffFailureEvent,

        /// <summary>
        /// The official transfer point registration failure event.
        /// </summary>
        OfficialTransferPointRegistrationFailureEvent,

        /// <summary>
        /// The operative delta failure event.
        /// </summary>
        OperativeDeltaFailureEvent,

        /// <summary>
        /// The official delta failure event.
        /// </summary>
        OfficialDeltaFailureEvent,

        /// <summary>
        /// The registration failure event.
        /// </summary>
        RegistrationFailureEvent,

        /// <summary>
        /// The resources availability failed.
        /// </summary>
        ResourcesAvailabilityFailed,

        /// <summary>
        /// The metric availability failed.
        /// </summary>
        MetricAvailabilityFailed,

        /// <summary>
        /// The owner reconcile failed.
        /// </summary>
        OwnerReconcileFailed,

        /// <summary>
        /// The report execution failed.
        /// </summary>
        ReportExecutionFailed,

        /// <summary>
        /// The sap upload purchase failure event.
        /// </summary>
        SapUploadPurchaseFailureEvent,

        /// <summary>
        /// The sap upload sell failure event.
        /// </summary>
        SapUploadSellFailureEvent,

        /// <summary>
        /// The sap upload movement or inventory failure event.
        /// </summary>
        SapUploadMovementOrInventoryFailureEvent,

        /// <summary>
        /// The conciliation failure event.
        /// </summary>
        ConciliationFailureEvent,

        /// <summary>
        /// The sap upload movement or inventory failure event.
        /// </summary>
        SapUploadLogisticMovementFailureEvent,
    }
}
