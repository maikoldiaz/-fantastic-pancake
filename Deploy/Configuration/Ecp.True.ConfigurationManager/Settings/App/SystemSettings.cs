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
namespace Ecp.True.ConfigurationManager.Console.Settings
{
    using Ecp.True.ConfigurationManager.Console.Entities;
    using Ecp.True.ConfigurationManager.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// The System Settings.
    /// </summary>
    /// <seealso cref="Ecp.True.ConfigurationManager.Console.Settings.SettingsBase" />
    public class SystemSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSettings"/> class.
        /// </summary>
        public SystemSettings()
        {
            this.ControlLimit = 1.98M;
            this.StandardUncertaintyPercentage = 0.2M;
            this.AcceptableBalancePercentage = 1.2M;
            this.RegistrationValidDays = 4;
            this.CurrentMonthValidDays = 4;
            this.PreviousMonthValidDays = 7;
            this.UnbalanceReportValidDays = 45;
            this.NodeStatusReportValidDays = 40;
            this.OfficialDeltaReportDefaultYears = 5;

            this.WarningLimit = 1.65M;
            this.ActionLimit = 2.00M;
            this.ToleranceLimit = 3.00M;
            this.NodeTagGracePeriodInMonths = 6;
            this.OwnershipStrategy = 2;
            this.AnsConfigurationDays = 5;
            this.AuditReportValidDays = 62;
            this.MaxSessionTimeoutDurationInMins = 60;
            this.ReportsCleanupDurationInHours = 12;
            this.AutocompleteItemsCount = 150;

            this.CutOff = new DateConfig(120);
            this.OwnershipCalculation = new DateConfig(120);
            this.OwnershipNode = new DateConfig(120);
            this.TransportFileUpload = new DateConfig(30, 40);
            this.LogisticsOwnership = new DateConfig(40, 60);
            this.Error = new DateConfig(40);
            this.Delta = new DateConfig(40);
            this.OfficialDelta = new DateConfig(365, 5);
            this.OfficialDeltaPerNode = new DateConfig(90);
            this.OfficialSiv = new DateConfig(365, 5);

            this.ValidateContractDays = 180;
            this.MaxNodeCostCenterBatchCreation = 10;
            this.MaxNodeConnectionCreationEdition = 10;
            this.MaxDeviationPercentage = 3.00M;
            this.MaxMonthsSendToSapOperativeReport = 3;
            this.MaxProductStorageLocationMappingCreationEdition = 10;

            this.MaxCutOffRetryCount = 3;
            this.MaxOwnershipRetryCount = 3;
            this.CutOffRetryIntervalInSeconds = 300;
            this.OwnerShipRetryIntervalInSeconds = 300;
            this.CutOffRetryStrategy = 1;
            this.OwnerShipRetryStrategy = 1;
        }

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
        public int? RegistrationValidDays { get; set; }

        /// <summary>
        /// Gets or sets the valid days for current month.
        /// </summary>
        public int? CurrentMonthValidDays { get; set; }

        /// <summary>
        /// Gets or sets the valid days for previous month.
        /// </summary>
        public int? PreviousMonthValidDays { get; set; }

        /// <summary>
        /// Gets or sets the acceptable date range for inventory and movement registration.
        /// </summary>
        public DateConfig CutOff { get; set; }

        /// <summary>
        /// Gets or sets the acceptable date range for inventory and movement registration.
        /// </summary>
        public DateConfig OwnershipCalculation { get; set; }

        /// <summary>
        /// Gets or sets the acceptable date range for inventory and movement registration.
        /// </summary>
        public DateConfig OwnershipNode { get; set; }

        /// <summary>
        /// Gets or sets the acceptable date range for inventory and movement registration.
        /// </summary>
        public DateConfig TransportFileUpload { get; set; }

        /// <summary>
        /// Gets or sets the default error last days.
        /// </summary>
        public DateConfig Error { get; set; }

        /// <summary>
        /// Gets or sets the logistics ownership.
        /// </summary>
        public DateConfig LogisticsOwnership { get; set; }

        /// <summary>
        /// Gets or sets the number of days for which delta calculation result needs to be generated.
        /// </summary>
        public DateConfig Delta { get; set; }

        /// <summary>
        /// Gets or sets the number of days for which official delta calculation result needs to be displayed.
        /// </summary>
        public DateConfig OfficialDelta { get; set; }

        /// <summary>
        /// Gets or sets the number of days for which official delta per node calculation result needs to be displayed.
        /// </summary>
        public DateConfig OfficialDeltaPerNode { get; set; }

        /// <summary>
        /// Gets or sets the number of days for which official siv result needs to be displayed.
        /// </summary>
        public DateConfig OfficialSiv { get; set; }

        /// <summary>
        /// Gets or sets the unbalance report valid days.
        /// </summary>
        public int? UnbalanceReportValidDays { get; set; }

        /// <summary>
        /// Gets or sets the warning limit.
        /// </summary>
        public decimal? WarningLimit { get; set; }

        /// <summary>
        /// Gets or sets the action limit.
        /// </summary>
        public decimal? ActionLimit { get; set; }

        /// <summary>
        /// Gets or sets the tolerance limit.
        /// </summary>
        public decimal? ToleranceLimit { get; set; }

        /// <summary>
        /// Gets or sets the base path.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string BasePath { get; set; }

        /// <summary>
        /// Gets or sets the node tag grace period in months.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public int? NodeTagGracePeriodInMonths { get; set; }

        /// <summary>
        /// Gets or sets the ownership strategy.
        /// </summary>
        /// <value>
        /// The ownership strategy.
        /// </value>
        public int OwnershipStrategy { get; set; }

        /// <summary>
        /// Gets or sets the node status report valid days.
        /// </summary>
        /// <value>
        /// The node status report valid days.
        /// </value>
        public int? NodeStatusReportValidDays { get; set; }

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
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "System.Settings";

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
        /// Gets or sets the maximum deviation percentaje.
        /// </summary>
        /// <value>
        /// Maximum deviation percentaje.
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

        /// <summary>Gets or sets the maximum Retry Count for Report Data Cut Off.</summary>
        /// <value>maximum Retry Count for Report Data Cut Off.</value>
        public int? MaxCutOffRetryCount { get; set; }

        /// <summary>Gets or sets the maximum Retry Count for Report Data OwnerShip.</summary>
        /// <value>maximum Retry Count for Report Data OwnerShip.</value>
        public int? MaxOwnershipRetryCount { get; set; }

        /// <summary>Gets or sets the maximum Retry Interval In Seconds for Report Data Cut Off.</summary>
        /// <value>maximum Retry Interval In Seconds for Report Data Cut Off.</value>
        public int? CutOffRetryIntervalInSeconds { get; set; }

        /// <summary>Gets or sets the maximum Retry Interval In Seconds for Report Data OwnerShip.</summary>
        /// <value>maximum Retry Interval In Seconds for Report Data OwnerShip.</value>
        public int? OwnerShipRetryIntervalInSeconds { get; set; }

        /// <summary>Gets or sets the Retry Strategy for Report Data Cut Off.</summary>
        /// <value>Retry Strategy for Report Data Cut Off.</value>
        public int? CutOffRetryStrategy { get; set; }

        /// <summary>Gets or sets the Retry Strategy for Report Data OwnerShip.</summary>
        /// <value>Retry Strategy for Report Data OwnerShip.</value>
        public int? OwnerShipRetryStrategy { get; set; }

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));

            this.ControlLimit = input.GetDecimalValueOrDefault(nameof(this.ControlLimit), this.ControlLimit, this.Key);
            this.StandardUncertaintyPercentage = input.GetDecimalValueOrDefault(nameof(this.StandardUncertaintyPercentage), this.StandardUncertaintyPercentage, this.Key);
            this.AcceptableBalancePercentage = input.GetDecimalValueOrDefault(nameof(this.AcceptableBalancePercentage), this.AcceptableBalancePercentage, this.Key);

            this.RegistrationValidDays = input.GetIntValueOrDefault(nameof(this.RegistrationValidDays), this.RegistrationValidDays, this.Key);
            this.CurrentMonthValidDays = input.GetIntValueOrDefault(nameof(this.CurrentMonthValidDays), this.CurrentMonthValidDays, this.Key);
            this.PreviousMonthValidDays = input.GetIntValueOrDefault(nameof(this.PreviousMonthValidDays), this.PreviousMonthValidDays, this.Key);
            this.UnbalanceReportValidDays = input.GetIntValueOrDefault(nameof(this.UnbalanceReportValidDays), this.UnbalanceReportValidDays, this.Key);
            this.NodeStatusReportValidDays = input.GetIntValueOrDefault(nameof(this.NodeStatusReportValidDays), this.NodeStatusReportValidDays, this.Key);
            this.AnsConfigurationDays = input.GetIntValueOrDefault(nameof(this.AnsConfigurationDays), this.AnsConfigurationDays, this.Key);
            this.AuditReportValidDays = input.GetIntValueOrDefault(nameof(this.AuditReportValidDays), this.AuditReportValidDays, this.Key);
            this.OfficialDeltaReportDefaultYears = input.GetIntValueOrDefault(nameof(this.OfficialDeltaReportDefaultYears), this.OfficialDeltaReportDefaultYears, this.Key);
            this.MaxSessionTimeoutDurationInMins = input.GetIntValueOrDefault(nameof(this.MaxSessionTimeoutDurationInMins), this.MaxSessionTimeoutDurationInMins, this.Key);
            this.ReportsCleanupDurationInHours = input.GetIntValueOrDefault(nameof(this.ReportsCleanupDurationInHours), this.ReportsCleanupDurationInHours, this.Key);

            this.WarningLimit = input.GetDecimalValueOrDefault(nameof(this.WarningLimit), this.WarningLimit, this.Key);
            this.ActionLimit = input.GetDecimalValueOrDefault(nameof(this.ActionLimit), this.ActionLimit, this.Key);
            this.ControlLimit = input.GetDecimalValueOrDefault(nameof(this.ControlLimit), this.ControlLimit, this.Key);
            this.ToleranceLimit = input.GetDecimalValueOrDefault(nameof(this.ToleranceLimit), this.ToleranceLimit, this.Key);

            this.BasePath = input.GetStringValueOrDefault(nameof(this.BasePath), this.BasePath, this.Key);
            this.OwnershipStrategy = input.GetIntValueOrDefault(nameof(this.OwnershipStrategy), this.OwnershipStrategy, this.Key);
            this.NodeTagGracePeriodInMonths = input.GetIntValueOrDefault(nameof(this.NodeTagGracePeriodInMonths), this.NodeTagGracePeriodInMonths, this.Key);
            this.AutocompleteItemsCount = input.GetIntValueOrDefault(nameof(this.AutocompleteItemsCount), this.AutocompleteItemsCount, this.Key);

            this.ValidateContractDays = input.GetIntValueOrDefault(nameof(this.ValidateContractDays), this.ValidateContractDays, this.Key);
            this.MaxDeviationPercentage = input.GetDecimalValueOrDefault(nameof(this.MaxDeviationPercentage), this.MaxDeviationPercentage, this.Key);

            this.MaxCutOffRetryCount = input.GetIntValueOrDefault(nameof(this.MaxCutOffRetryCount), this.MaxCutOffRetryCount, this.Key);
            this.MaxOwnershipRetryCount = input.GetIntValueOrDefault(nameof(this.MaxOwnershipRetryCount), this.MaxOwnershipRetryCount, this.Key);
            this.CutOffRetryIntervalInSeconds = input.GetIntValueOrDefault(nameof(this.CutOffRetryIntervalInSeconds), this.CutOffRetryIntervalInSeconds, this.Key);
            this.OwnerShipRetryIntervalInSeconds = input.GetIntValueOrDefault(nameof(this.OwnerShipRetryIntervalInSeconds), this.OwnerShipRetryIntervalInSeconds, this.Key);
            this.CutOffRetryStrategy = input.GetIntValueOrDefault(nameof(this.CutOffRetryStrategy), this.CutOffRetryStrategy, this.Key);
            this.OwnerShipRetryStrategy = input.GetIntValueOrDefault(nameof(this.OwnerShipRetryStrategy), this.OwnerShipRetryStrategy, this.Key);

            this.DoCopyDateConfig(input, nameof(this.CutOff), this.CutOff);
            this.DoCopyDateConfig(input, nameof(this.OwnershipCalculation), this.OwnershipCalculation);
            this.DoCopyDateConfig(input, nameof(this.OwnershipNode), this.OwnershipNode);
            this.DoCopyDateConfig(input, nameof(this.TransportFileUpload), this.TransportFileUpload);
            this.DoCopyDateConfig(input, nameof(this.Error), this.Error);
            this.DoCopyDateConfig(input, nameof(this.LogisticsOwnership), this.LogisticsOwnership);
            this.DoCopyDateConfig(input, nameof(this.Delta), this.Delta);
            this.DoCopyDateConfig(input, nameof(this.OfficialDelta), this.OfficialDelta);
            this.DoCopyDateConfig(input, nameof(this.OfficialDeltaPerNode), this.OfficialDeltaPerNode);
            this.DoCopyDateConfig(input, nameof(this.OfficialSiv), this.OfficialSiv);
        }

        private void DoCopyDateConfig(CopyInput input, string name, ICopyable config)
        {
            if (config == null || input.ShouldIgnore(this.Key, name))
            {
                return;
            }

            config.CopyFrom(input.ExistingSetting.GetValue(name));
        }
    }
}
