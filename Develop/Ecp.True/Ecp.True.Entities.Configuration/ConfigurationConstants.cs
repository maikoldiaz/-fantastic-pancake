// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationConstants.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The configuration constants.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ConfigurationConstants
    {
        /// <summary>
        /// The secret configuration prefix.
        /// </summary>
        public static readonly string SecretConfigurationPrefix = "Secret_";

        /// <summary>
        /// The file configuration prefix.
        /// </summary>
        public static readonly string FileConfigurationPrefix = "File_";

        /// <summary>
        /// The data store configuration prefix.
        /// </summary>
        public static readonly string DataStoreConfigurationPrefix = "Datastore_";

        /// <summary>
        /// The key vault name.
        /// </summary>
        public static readonly string KeyVaultName = FileConfigurationPrefix + "VaultName";

        /// <summary>
        /// The API service endpoint.
        /// </summary>
        public static readonly string ApiEndpoint = FileConfigurationPrefix + "ApiEndpoint";

        /// <summary>
        /// The generate request identifier.
        /// </summary>
        public static readonly string GenerateRequestId = FileConfigurationPrefix + "GenerateRequestId";

        /// <summary>
        /// The throw exceptions.
        /// </summary>
        public static readonly string ThrowExceptions = FileConfigurationPrefix + "ThrowExceptions";

        /// <summary>
        /// The allowed hosts.
        /// </summary>
        public static readonly string AllowedHosts = FileConfigurationPrefix + "AllowedHosts";

        /// <summary>
        /// The log level.
        /// </summary>
        public static readonly string LogLevel = FileConfigurationPrefix + "LogLevel";

        /// <summary>
        /// The homologation refresh interval in secs.
        /// </summary>
        public static readonly string HomologationRefreshIntervalInSecs = FileConfigurationPrefix + "HomologationRefreshIntervalInSecs";

        /// <summary>
        /// The command timeout in secs.
        /// </summary>
        public static readonly string CommandTimeoutInSecs = FileConfigurationPrefix + "CommandTimeoutInSecs";

        /// <summary>
        /// The reference data SQL connection string.
        /// </summary>
        public static readonly string SqlConnectionString = SecretConfigurationPrefix + "Sql.ConnectionString";

        /// <summary>
        /// The configuration storage connection string.
        /// </summary>
        public static readonly string ConfigConnectionString = SecretConfigurationPrefix + "Storage.ConnectionString";

        /// <summary>
        /// The configuration version.
        /// </summary>
        public static readonly string ConfigurationVersion = SecretConfigurationPrefix + "Configuration.Version";

        /// <summary>
        /// The SQL connection settings.
        /// </summary>
        public static readonly string SqlConnectionSettings = DataStoreConfigurationPrefix + "SqlConnection.Settings";

        /// <summary>
        /// The integration service bus configuration.
        /// </summary>
        public static readonly string ServiceBusSettings = DataStoreConfigurationPrefix + "ServiceBus.Settings";

        /// <summary>
        /// The integration service bus configuration.
        /// </summary>
        public static readonly string StorageSettings = DataStoreConfigurationPrefix + "Storage.Settings";

        /// <summary>
        /// The key vault retry settings.
        /// </summary>
        public static readonly string KeyVaultRetrySettings = DataStoreConfigurationPrefix + "KeyVault.RetrySettings";

        /// <summary>
        /// The blockchain settings.
        /// </summary>
        public static readonly string BlobRetrySettings = DataStoreConfigurationPrefix + "Blob.RetrySettings";

        /// <summary>
        /// The system settings.
        /// </summary>
        public static readonly string SystemSettings = DataStoreConfigurationPrefix + "System.Settings";

        /// <summary>
        /// The reconciliation settings.
        /// </summary>
        public static readonly string ReconciliationSettings = DataStoreConfigurationPrefix + "Reconciliation.Settings";

        /// <summary>
        /// The SAP PO settings.
        /// </summary>
        public static readonly string ShouldHomologate = FileConfigurationPrefix + "ShouldHomologate";

        /// <summary>
        /// The SAP PO settings.
        /// </summary>
        public static readonly string ShouldHomologatePurchase = FileConfigurationPrefix + "ShouldHomologatePurchase";

        /// <summary>
        /// The SAP PO settings.
        /// </summary>
        public static readonly string ShouldHomologateSale = FileConfigurationPrefix + "ShouldHomologateSale";

        /// <summary>
        /// The analytics client path.
        /// </summary>
        public static readonly string AnalyticsClientPath = FileConfigurationPrefix + "AnalyticsClientPath";

        /// <summary>
        /// The dummy analytics response.
        /// </summary>
        public static readonly string DummyAnalyticsResponse = FileConfigurationPrefix + "DummyAnalyticsResponse";

        /// <summary>
        /// The blockchain settings.
        /// </summary>
        public static readonly string BlockchainSettings = DataStoreConfigurationPrefix + "Blockchain.Settings";

        /// <summary>
        /// The report settings.
        /// </summary>
        public static readonly string ReportSettings = DataStoreConfigurationPrefix + "Report.Settings";

        /// <summary>
        /// The user role settings.
        /// </summary>
        public static readonly string UserRoleSettings = DataStoreConfigurationPrefix + "UserRole.Settings";

        /// <summary>
        /// The analysis settings.
        /// </summary>
        public static readonly string AnalysisSettings = DataStoreConfigurationPrefix + "Analysis.Settings";

        /// <summary>
        /// The flow settings.
        /// </summary>
        public static readonly string FlowSettings = DataStoreConfigurationPrefix + "Flow.Settings";

        /// <summary>
        /// The analytics settings.
        /// </summary>
        public static readonly string AnalyticsSettings = DataStoreConfigurationPrefix + "Analytics.Settings";

        /// <summary>
        /// The OwnershipRule settings.
        /// </summary>
        public static readonly string OwnershipRuleSettings = DataStoreConfigurationPrefix + "OwnershipRule.Settings";

        /// <summary>
        /// The Sap settings.
        /// </summary>
        public static readonly string SapSettings = DataStoreConfigurationPrefix + "Sap.Settings";

        /// <summary>
        /// The integration service bus configuration.
        /// </summary>
        public static readonly string IntegrationServiceBusConnectionString = FileConfigurationPrefix + "IntegrationServiceBusConnectionString";

        /// <summary>
        /// The support settings.
        /// </summary>
        public static readonly string SupportSettings = DataStoreConfigurationPrefix + "Support.Settings";

        /// <summary>
        /// The cache settings.
        /// </summary>
        public static readonly string CacheSettings = DataStoreConfigurationPrefix + "Cache.Settings";

        /// <summary>
        /// The availability settings.
        /// </summary>
        public static readonly string AvailabilitySettings = DataStoreConfigurationPrefix + "Availability.Settings";

        /// <summary>
        /// The module availability.
        /// </summary>
        public static readonly string ModuleAvailability = DataStoreConfigurationPrefix + "ModuleAvailability";

        /// <summary>
        /// The graph settings.
        /// </summary>
        public static readonly string GraphSettings = DataStoreConfigurationPrefix + "Graph.Settings";
    }
}