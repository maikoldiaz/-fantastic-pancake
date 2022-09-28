// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AvailabilitySettings.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Ecp.True.Core;

    /// <summary>
    /// The availability Settings.
    /// </summary>
    public class AvailabilitySettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvailabilitySettings"/> class.
        /// </summary>
        public AvailabilitySettings()
        {
            this.ResourceGroups = new List<ResourceGroup>();
        }

        /// <summary>
        /// Gets or sets the subscription identifier.
        /// </summary>
        /// <value>
        /// The subscription identifier.
        /// </value>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets the resource groups.
        /// </summary>
        /// <value>
        /// The resources groups.
        /// </value>
        public ICollection<ResourceGroup> ResourceGroups { get; private set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the resource identifier.
        /// </summary>
        /// <value>
        /// The resource identifier.
        /// </value>
        public string ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the resource detail path.
        /// </summary>
        /// <value>
        /// The resource detail path.
        /// </value>
        public string ResourceDetailPath { get; set; }

        /// <summary>
        /// Gets or sets the availability path.
        /// </summary>
        /// <value>
        /// The availability path.
        /// </value>
        public string AvailabilityPath { get; set; }

        /// <summary>
        /// Gets or sets the metric path.
        /// </summary>
        /// <value>
        /// The metric path.
        /// </value>
        public string MetricPath { get; set; }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <value>
        /// The resource.
        /// </value>
        public string Resource
        {
            get
            {
                return $"{this.ResourceId}/.default";
            }
        }

        /// <summary>
        /// Builds the availability URI.
        /// </summary>
        /// <param name="resourceGroup">Resource group.</param>
        /// <returns>The uri.</returns>
        public Uri BuildAvailabilityUri(ResourceGroup resourceGroup)
        {
            ArgumentValidators.ThrowIfNull(resourceGroup, nameof(resourceGroup));
            return new Uri(string.Format(CultureInfo.InvariantCulture, this.AvailabilityPath, resourceGroup.SubscriptionId, resourceGroup.Name));
        }

        /// <summary>
        /// Builds the resource detail URI.
        /// </summary>
        /// <param name="resourceGroup">Resource group.</param>
        /// <returns>The uri.</returns>
        public Uri BuildResourceDetailUri(ResourceGroup resourceGroup)
        {
            ArgumentValidators.ThrowIfNull(resourceGroup, nameof(resourceGroup));
            return new Uri(string.Format(CultureInfo.InvariantCulture, this.ResourceDetailPath, resourceGroup.SubscriptionId, resourceGroup.Name));
        }

        /// <summary>
        /// Builds the metric URI.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <returns>The uri.</returns>
        public Uri BuildMetricUri(string resourceId)
        {
            return new Uri(string.Format(CultureInfo.InvariantCulture, this.MetricPath, resourceId));
        }
    }
}
