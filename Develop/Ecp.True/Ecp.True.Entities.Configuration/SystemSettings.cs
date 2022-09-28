// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemSettings.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    /// <summary>
    /// The system configuration.
    /// </summary>
    public class SystemSettings
    {
        /// <summary>
        /// Gets or sets the control limit.
        /// </summary>
        /// <value>
        /// The Control Limit.
        /// </value>
        public decimal? ControlLimit { get; set; }

        /// <summary>
        /// Gets or sets the standard uncertainty percentage.
        /// </summary>
        /// <value>
        /// The standard uncertainty percentage.
        /// </value>
        public decimal? StandardUncertaintyPercentage { get; set; }

        /// <summary>
        /// Gets or sets the acceptable balance percentage.
        /// </summary>
        /// <value>
        /// The acceptable balance percentage.
        /// </value>
        public decimal? AcceptableBalancePercentage { get; set; }

        /// <summary>
        /// Gets or sets the acceptable date range for inventory and movement registration.
        /// </summary>
        /// <value>
        /// The acceptable date range.
        public int? RegistrationValidDays { get; set; }

        /// <summary>
        /// Gets or sets the acceptable date range for inventory and movement registration.
        /// </summary>
        /// <value>
        /// The acceptable date range.
        public DateConfig CutOff { get; set; }

        /// <summary>
        /// Gets or sets the acceptable date range for inventory and movement registration.
        /// </summary>
        /// <value>
        /// The acceptable date range.
        public DateConfig OwnershipCalculation { get; set; }

        /// <summary>
        /// Gets or sets the acceptable date range for inventory and movement registration.
        /// </summary>
        /// <value>
        /// The acceptable date range.
        public DateConfig OwnershipNode { get; set; }

        /// <summary>
        /// Gets or sets the acceptable date range for inventory and movement registration.
        /// </summary>
        /// <value>
        /// The acceptable date range.
        public DateConfig TransportFileUpload { get; set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public DateConfig Error { get; set; }

        /// <summary>
        /// Gets or sets the logistics ownership.
        /// </summary>
        /// <value>
        /// The logistics ownership.
        /// </value>
        public DateConfig LogisticsOwnership { get; set; }

        /// <summary>
        /// Gets or sets the Delta calculation object.
        /// </summary>
        /// <value>
        /// The Delta calculation.
        /// </value>
        public DateConfig Delta { get; set; }

        /// <summary>
        /// Gets or sets the number of days for which official delta calculation result needs to be displayed.
        /// </summary>
        /// <value>
        /// The official delta.
        /// </value>
        public DateConfig OfficialDelta { get; set; }

        /// <summary>
        /// Gets or sets the official delta per node.
        /// </summary>
        /// <value>
        /// The official delta per node.
        /// </value>
        public DateConfig OfficialDeltaPerNode { get; set; }

        /// <summary>
        /// Gets or sets the number of days for which official siv result needs to be displayed.
        /// </summary>
        /// <value>
        /// The official siv.
        /// </value>
        public DateConfig OfficialSiv { get; set; }

        /// <summary>
        /// Gets or sets the valid days in current month for movement registration.
        /// </summary>
        /// <value>
        /// The acceptable date range.
        public int? CurrentMonthValidDays { get; set; }

        /// <summary>
        /// Gets or sets the valid days in previous month for movement registration.
        /// </summary>
        /// <value>
        /// The acceptable date range.
        public int? PreviousMonthValidDays { get; set; }

        /// <summary>
        /// Gets or sets the unbalance report valid days.
        /// </summary>
        /// <value>
        /// The unbalance report valid days.
        /// </value>
        public int? UnbalanceReportValidDays { get; set; }

        /// <summary>
        /// Gets or sets the warning limit.
        /// </summary>
        /// <value>
        /// The warning limit.
        /// </value>
        public decimal? WarningLimit { get; set; }

        /// <summary>
        /// Gets or sets the action limit.
        /// </summary>
        /// <value>
        /// The action limit.
        /// </value>
        public decimal? ActionLimit { get; set; }

        /// <summary>
        /// Gets or sets the tolerance limit.
        /// </summary>
        /// <value>
        /// The tolerance limit.
        /// </value>
        public decimal? ToleranceLimit { get; set; }

        /// <summary>
        /// Gets or sets the base path.
        /// </summary>
        /// <value>
        /// The base path.
        /// </value>
        public string BasePath { get; set; }

        /// <summary>
        /// Gets or sets the node tag grace period in months.
        /// </summary>
        /// <value>
        /// The node tag grace period.
        /// </value>
        public int? NodeTagGracePeriodInMonths { get; set; }

        /// <summary>
        /// Gets or sets the node status report valid days.
        /// </summary>
        /// <value>
        /// The node status report valid days.
        /// </value>
        public int? NodeStatusReportValidDays { get; set; }

        /// <summary>
        /// Gets or sets the ownership strategy.
        /// </summary>
        /// <value>
        /// The ownership strategy.
        /// </value>
        public int OwnershipStrategy { get; set; }

        /// <summary>
        /// Gets or sets the ANS days.
        /// </summary>
        /// <value>
        /// The ANS days.
        /// </value>
        public int? AnsConfigurationDays { get; set; }

        /// <summary>
        /// Gets or sets the audit report valid days.
        /// </summary>
        /// <value>
        /// The audit report valid days.
        /// </value>
        public int? AuditReportValidDays { get; set; }

        /// <summary>
        /// Gets or sets the official delta report default years.
        /// </summary>
        /// <value>
        /// The official delta report default years.
        /// </value>
        public int? OfficialDeltaReportDefaultYears { get; set; }

        /// <summary>
        /// Gets or sets the maximum duration of the session timeout.
        /// </summary>
        /// <value>
        /// The maximum duration of the session timeout.
        /// </value>
        public int? MaxSessionTimeoutDurationInMins { get; set; }

        /// <summary>
        /// Gets or sets the reports cleaning recurrence duration in hours.
        /// </summary>
        /// <value>
        /// The reports cleaning recurrence duration in hours.
        /// </value>
        public int? ReportsCleanupDurationInHours { get; set; }

        /// <summary>
        /// Gets or sets the autocomplete items count.
        /// </summary>
        /// <value>
        /// The autocomplete items count.
        /// </value>
        public int? AutocompleteItemsCount { get; set; }

        /// <summary>
        /// Gets or sets the days contract date validate.
        /// </summary>
        /// <value>
        /// Days contract date validate.
        /// </value>
        public int ValidateContractDays { get; set; }

        /// <summary>
        /// Gets or sets the maximun number of node cost center relations to be created at once.
        /// </summary>
        /// <value>
        /// The maximun number of node cost center relations to be created at once.
        /// </value>
        public int MaxNodeCostCenterBatchCreation { get; set; }

        /// <summary>
        /// Gets or sets the maximun number of node connection to be created or edited at once from the form (not the graphic designer).
        /// </summary>
        /// <value>
        /// The maximun number of node connection to be created or edited at once from the form (not the graphic designer).
        /// </value>
        public int MaxNodeConnectionCreationEdition { get; set; }

        /// <summary>
        /// Gets or sets the maximum deviation percentage.
        /// </summary>
        /// <value>
        /// Maximum deviation percentage.
        /// </value>
        public decimal? MaxDeviationPercentage { get; set; }

        /// <summary>
        /// Gets or sets the maximum range of months for the operational report of shipments to sap.
        /// </summary>
        /// <value>
        /// Maximum range of months for the operational report .
        /// </value>
        public int MaxMonthsSendToSapOperativeReport { get; set; }

        /// <summary>Gets or sets the maximum product storage location mapping creation edition.</summary>
        /// <value>The maximum product storage location mapping creation edition.</value>
        public int MaxProductStorageLocationMappingCreationEdition { get; set; }

        /// <summary>
        /// Gets or sets the maximum Retry Count for Report Data Cut Off.
        /// </summary>
        /// <value>
        /// maximum Retry Count for Report Data Cut Off.
        /// </value>
        public int? MaxCutOffRetryCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum Retry Count for Report Data OwnerShip.
        /// </summary>
        /// <value>
        /// maximum Retry Count for Report Data OwnerShip.
        /// </value>
        public int? MaxOwnershipRetryCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum Retry Interval In Seconds for Report Data Cut Off.
        /// </summary>
        /// <value>
        /// maximum Retry Interval In Seconds for Report Data Cut Off.
        /// </value>
        public int? CutOffRetryIntervalInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the maximum Retry Interval In Seconds for Report Data OwnerShip.
        /// </summary>
        /// <value>
        /// maximum Retry Interval In Seconds for Report Data OwnerShip.
        /// </value>
        public int? OwnerShipRetryIntervalInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the Retry Strategy for Report Data Cut Off.
        /// </summary>
        /// <value>
        /// Retry Strategy for Report Data Cut Off.
        /// </value>
        public int? CutOffRetryStrategy { get; set; }

        /// <summary>
        /// Gets or sets the Retry Strategy for Report Data OwnerShip.
        /// </summary>
        /// <value>
        /// Retry Strategy for Report Data OwnerShip.
        /// </value>
        public int? OwnerShipRetryStrategy { get; set; }
    }
}
