// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlConnectionSettings.cs" company="Microsoft">
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
    using Ecp.True.ConfigurationManager.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// The SQL Connection Settings.
    /// </summary>
    public class SqlConnectionSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConnectionSettings"/> class.
        /// </summary>
        public SqlConnectionSettings()
        {
            this.MaxRetryCount = 3;
            this.RetryIntervalInSecs = 10;
            this.ConnectionString = "#MsiSqlConnectionString#";
            this.ResourceUrl = "https://database.windows.net/";
        }

        /// <summary>
        /// Gets or sets the maximum retry count.
        /// </summary>
        /// <value>
        /// The maximum retry count.
        /// </value>
        public int MaxRetryCount { get; set; }

        /// <summary>
        /// Gets or sets the retry interval in secs.
        /// </summary>
        /// <value>
        /// The retry interval in secs.
        /// </value>
        public int RetryIntervalInSecs { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the database URL.
        /// </summary>
        /// <value>
        /// The database URL.
        /// </value>
        public string ResourceUrl { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "SqlConnection.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.MaxRetryCount = input.GetIntValueOrDefault(nameof(this.MaxRetryCount), this.MaxRetryCount, this.Key);
            this.RetryIntervalInSecs = input.GetIntValueOrDefault(nameof(this.RetryIntervalInSecs), this.RetryIntervalInSecs, this.Key);
            this.ConnectionString = input.GetStringValueOrDefault(nameof(this.ConnectionString), this.ConnectionString, this.Key);
            this.ResourceUrl = input.GetStringValueOrDefault(nameof(this.ResourceUrl), this.ResourceUrl, this.Key);
            this.TenantId = input.GetStringValueOrDefault(nameof(this.TenantId), this.TenantId, this.Key);
        }
    }
}
