// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration.Entities;

    /// <summary>
    /// The Azure Configuration.
    /// </summary>
    public class AzureConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureConfiguration" /> class.
        /// </summary>
        /// <param name="storageSettings">The storage settings.</param>
        /// <param name="serviceBusSettings">The service bus settings.</param>
        public AzureConfiguration(StorageSettings storageSettings, ServiceBusSettings serviceBusSettings)
        {
            this.StorageSettings = storageSettings;
            this.ServiceBusSettings = serviceBusSettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureConfiguration" /> class.
        /// </summary>
        /// <param name="quorumProfile">The quorum profile.</param>
        /// <param name="analysisSettings">The analysis settings.</param>
        /// <param name="storageSettings">The storage settings.</param>
        /// <param name="serviceBusSettings">The service bus settings.</param>
        public AzureConfiguration(QuorumProfile quorumProfile, AnalysisSettings analysisSettings, StorageSettings storageSettings, ServiceBusSettings serviceBusSettings)
            : this(analysisSettings, storageSettings, serviceBusSettings)
        {
            ArgumentValidators.ThrowIfNull(quorumProfile, nameof(quorumProfile));

            this.QuorumProfile = quorumProfile;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureConfiguration"/> class.
        /// </summary>
        /// <param name="quorumProfile">The quorum profile.</param>
        /// <param name="storageSettings">The storage settings.</param>
        /// <param name="serviceBusSettings">The service bus settings.</param>
        public AzureConfiguration(QuorumProfile quorumProfile, StorageSettings storageSettings, ServiceBusSettings serviceBusSettings)
            : this(storageSettings, serviceBusSettings)
        {
            ArgumentValidators.ThrowIfNull(quorumProfile, nameof(quorumProfile));

            this.QuorumProfile = quorumProfile;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureConfiguration" /> class.
        /// </summary>
        /// <param name="analysisSettings">The analysis settings.</param>
        /// <param name="storageSettings">The storage settings.</param>
        /// <param name="serviceBusSettings">The service bus settings.</param>
        public AzureConfiguration(AnalysisSettings analysisSettings, StorageSettings storageSettings, ServiceBusSettings serviceBusSettings)
            : this(storageSettings, serviceBusSettings)
        {
            this.AnalysisSettings = analysisSettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureConfiguration" /> class.
        /// </summary>
        /// <param name="availabilitySettings">The availability settings.</param>
        /// <param name="storageSettings">The storage settings.</param>
        /// <param name="serviceBusSettings">The service bus settings.</param>
        public AzureConfiguration(AvailabilitySettings availabilitySettings, StorageSettings storageSettings, ServiceBusSettings serviceBusSettings)
            : this(storageSettings, serviceBusSettings)
        {
            this.AvailabilitySettings = availabilitySettings;
        }

        /// <summary>
        /// Gets or sets the storage settings.
        /// </summary>
        /// <value>
        /// The storage settings.
        /// </value>
        public StorageSettings StorageSettings { get; set; }

        /// <summary>
        /// Gets or sets the quorum profile.
        /// </summary>
        /// <value>
        /// The quorum profile.
        /// </value>
        public QuorumProfile QuorumProfile { get; set; }

        /// <summary>
        /// Gets or sets the analysis settings.
        /// </summary>
        /// <value>
        /// The analysis settings.
        /// </value>
        public AnalysisSettings AnalysisSettings { get; set; }

        /// <summary>
        /// Gets or sets the service bus settings.
        /// </summary>
        /// <value>
        /// The service bus settings.
        /// </value>
        public ServiceBusSettings ServiceBusSettings { get; set; }

        /// <summary>
        /// Gets or sets the availability settings.
        /// </summary>
        /// <value>
        /// The availability settings.
        /// </value>
        public AvailabilitySettings AvailabilitySettings { get; set; }
    }
}
