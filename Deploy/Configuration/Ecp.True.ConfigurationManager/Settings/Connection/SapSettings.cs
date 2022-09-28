// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapSettings.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ConfigurationManager.Settings
{
    using Ecp.True.ConfigurationManager.Console.Settings;
    using Ecp.True.ConfigurationManager.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// The SapSettings.
    /// </summary>
    public class SapSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SapSettings"/> class.
        /// </summary>
        public SapSettings()
        {
            this.Password = "#SapPassword#"; // pragma: allowlist secret
            this.TransferPointPath = $"RESTAdapter/true/puntos_transferencia";
            this.UploadStatusPath = $"RESTAdapter/true/respuesta_mov_inv";
            this.UploadStatusContractPath = $"RESTAdapter/TRUE/RespuestaGenerica";
            this.MappingPath = $"RESTAdapter/true/get_puntos_transferencia";
            this.RetryInterval = 2;
            this.RetryCount = 6;
            this.SapRecordsMaxLimit = 2000;
            this.PurchasesPositionsMaxLimit = 999;
            this.SalesPositionsMaxLimit = 999;
            this.SendLogisticMovementsPath = $"RESTAdapter/OrdenamientoInventarios/TRUE";
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        public string BasePath { get; set; }

        /// <summary>
        /// Gets or sets the transfer point path.
        /// </summary>
        /// <value>
        /// The transfer point.
        /// </value>
        public string TransferPointPath { get; set; }

        /// <summary>
        /// Gets or sets the upload status path.
        /// </summary>
        /// <value>
        /// The upload status path.
        /// </value>
        public string UploadStatusPath { get; set; }

        /// <summary>
        /// Gets or sets the upload status contract path.
        /// </summary>
        /// <value>
        /// The upload status contract path.
        /// </value>
        public string UploadStatusContractPath { get; set; }

        /// <summary>
        /// Gets or sets the mapping path.
        /// </summary>
        /// <value>
        /// The mapping path.
        /// </value>
        public string MappingPath { get; set; }

        /// <summary>
        /// Gets or sets the retry count.
        /// </summary>
        /// <value>
        /// The retry count.
        /// </value>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the retry interval.
        /// </summary>
        /// <value>
        /// The retry interval.
        /// </value>
        public int RetryInterval { get; set; }

        /// <summary>
        /// Gets or sets the sap records max limit.
        /// </summary>
        /// <value>
        /// The sap records max limit.
        /// </value>
        public int? SapRecordsMaxLimit { get; set; }

        /// <summary>
        /// Gets or sets the sales positions max limit.
        /// </summary>
        /// <value>
        /// The sales positions max limit.
        /// </value>
        public int? SalesPositionsMaxLimit { get; set; }

        /// <summary>
        /// Gets or sets the purchases positions records max limit.
        /// </summary>
        /// <value>
        /// The purchases positions max limit.
        /// </value>
        public int? PurchasesPositionsMaxLimit { get; set; }

        /// <summary>
        /// Gets or sets the logistic movement endpoint.
        /// </summary>
        /// <value>
        /// The logistic movement endpoint.
        /// </value>
        public string SendLogisticMovementsPath { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "Sap.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.Username = input.GetStringValueOrDefault(nameof(this.Username), this.Username, this.Key);
            this.Password = input.GetStringValueOrDefault(nameof(this.Password), this.Password, this.Key);
            this.BasePath = input.GetStringValueOrDefault(nameof(this.BasePath), this.BasePath, this.Key);
            this.TransferPointPath = input.GetStringValueOrDefault(nameof(this.TransferPointPath), this.TransferPointPath, this.Key);
            this.RetryCount = input.GetIntValueOrDefault(nameof(this.RetryCount), this.RetryCount, this.Key);
            this.RetryInterval = input.GetIntValueOrDefault(nameof(this.RetryInterval), this.RetryInterval, this.Key);
            this.MappingPath = input.GetStringValueOrDefault(nameof(this.MappingPath), this.MappingPath, this.Key);
            this.SapRecordsMaxLimit = input.GetIntValueOrDefault(nameof(this.SapRecordsMaxLimit), this.SapRecordsMaxLimit, this.Key);
            this.SalesPositionsMaxLimit = input.GetIntValueOrDefault(nameof(this.SalesPositionsMaxLimit), this.SalesPositionsMaxLimit, this.Key);
            this.PurchasesPositionsMaxLimit = input.GetIntValueOrDefault(nameof(this.PurchasesPositionsMaxLimit), this.PurchasesPositionsMaxLimit, this.Key);
            this.SendLogisticMovementsPath = input.GetStringValueOrDefault(nameof(this.SendLogisticMovementsPath), this.SendLogisticMovementsPath, this.Key);
        }
    }
}
