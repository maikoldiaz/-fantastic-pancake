// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceBusSettings.cs" company="Microsoft">
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
    /// The Retry Settings.
    /// </summary>
    /// <seealso cref="Ecp.True.ConfigurationManager.Console.Settings.SettingsBase" />
    public class ServiceBusSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusSettings"/> class.
        /// </summary>
        public ServiceBusSettings()
        {
            this.ExcelQueueName = "excel";
            this.InventoryQueueName = "inventory";
            this.LossesQueueName = "losses";
            this.MovementsQueueName = "movements";
            this.SpecialMovementsQueueName = "specialmovements";
            this.ConnectionString = "#IntServiceBusConnectionString#";
            this.MaximumBackOff = 30;
            this.MinimumBackOff = 10;
            this.MaximumRetryCount = 3;
        }

        /// <summary>
        /// Gets or sets the name of the excel queue.
        /// </summary>
        /// <value>
        /// The name of the excel queue.
        /// </value>
        public string ExcelQueueName { get; set; }

        /// <summary>
        /// Gets or sets the name of the inventory queue.
        /// </summary>
        /// <value>
        /// The name of the inventory queue.
        /// </value>
        public string InventoryQueueName { get; set; }

        /// <summary>
        /// Gets or sets the name of the losses queue.
        /// </summary>
        /// <value>
        /// The name of the losses queue.
        /// </value>
        public string LossesQueueName { get; set; }

        /// <summary>
        /// Gets or sets the name of the movements queue.
        /// </summary>
        /// <value>
        /// The name of the movements queue.
        /// </value>
        public string MovementsQueueName { get; set; }

        /// <summary>
        /// Gets or sets the name of the special movements queue.
        /// </summary>
        /// <value>
        /// The name of the special movements queue.
        /// </value>
        public string SpecialMovementsQueueName { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the Tenant ID.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the minimum back off.
        /// </summary>
        /// <value>
        /// The minimum back off.
        /// </value>
        public int MinimumBackOff { get; set; }

        /// <summary>
        /// Gets or sets the maximum back off.
        /// </summary>
        /// <value>
        /// The maximum back off.
        /// </value>
        public int MaximumBackOff { get; set; }

        /// <summary>
        /// Gets or sets the maximum retry count.
        /// </summary>
        /// <value>
        /// The maximum retry count.
        /// </value>
        public int MaximumRetryCount { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "ServiceBus.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.ExcelQueueName = input.GetStringValueOrDefault(nameof(this.ExcelQueueName), this.ExcelQueueName, this.Key);
            this.InventoryQueueName = input.GetStringValueOrDefault(nameof(this.InventoryQueueName), this.InventoryQueueName, this.Key);
            this.LossesQueueName = input.GetStringValueOrDefault(nameof(this.LossesQueueName), this.LossesQueueName, this.Key);
            this.MovementsQueueName = input.GetStringValueOrDefault(nameof(this.MovementsQueueName), this.MovementsQueueName, this.Key);
            this.SpecialMovementsQueueName = input.GetStringValueOrDefault(nameof(this.SpecialMovementsQueueName), this.SpecialMovementsQueueName, this.Key);
            this.ConnectionString = input.GetStringValueOrDefault(nameof(this.ConnectionString), this.ConnectionString, this.Key);
            this.Namespace = input.GetStringValueOrDefault(nameof(this.Namespace), this.Namespace, this.Key);
            this.TenantId = input.GetStringValueOrDefault(nameof(this.TenantId), this.TenantId, this.Key);
            this.MaximumRetryCount = input.GetIntValueOrDefault(nameof(this.MaximumRetryCount), this.MaximumRetryCount, this.Key);
            this.MinimumBackOff = input.GetIntValueOrDefault(nameof(this.MinimumBackOff), this.MinimumBackOff, this.Key);
            this.MaximumBackOff = input.GetIntValueOrDefault(nameof(this.MaximumBackOff), this.MaximumBackOff, this.Key);
        }
    }
}
