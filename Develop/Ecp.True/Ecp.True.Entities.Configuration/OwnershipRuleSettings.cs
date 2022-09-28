// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleSettings.cs" company="Microsoft">
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
    /// <summary>
    /// The Ownership Rule Configuration.
    /// </summary>
    public class OwnershipRuleSettings
    {
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
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        public string BasePath { get; set; }

        /// <summary>
        /// Gets or sets the registration path.
        /// </summary>
        /// <value>
        /// The registration path.
        /// </value>
        public string RegistrationPath { get; set; }

        /// <summary>
        /// Gets or sets the ownership rule path.
        /// </summary>
        /// <value>
        /// The ownership rule path.
        /// </value>
        public string OwnershipRulePath { get; set; }

        /// <summary>
        /// Gets or sets the refresh interval in hours.
        /// </summary>
        /// <value>
        /// The refresh interval in hours.
        /// </value>
        public int RefreshIntervalInHours { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ownership response should be stored to blob or not.
        /// </summary>
        /// <value>
        /// The value indicating whether ownership response should be stored to blob or not.
        /// </value>
        public bool ShouldStoreResponse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is compressed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is compressed; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompressed { get; set; }

        /// <summary>
        /// Gets or sets the delta base path.
        /// </summary>
        /// <value>
        /// The delta base path.
        /// </value>
        public string DeltaBasePath { get; set; }

        /// <summary>
        /// Gets or sets the delta API path.
        /// </summary>
        /// <value>
        /// The delta API path.
        /// </value>
        public string DeltaApiPath { get; set; }

        /// <summary>
        /// Gets or sets the delta base path.
        /// </summary>
        /// <value>
        /// The delta base path.
        /// </value>
        public string OfficialDeltaBasePath { get; set; }

        /// <summary>
        /// Gets or sets the delta API path.
        /// </summary>
        /// <value>
        /// The delta API path.
        /// </value>
        public string OfficialDeltaApiPath { get; set; }

        /// <summary>
        /// Gets or sets the timeout in minutes.
        /// </summary>
        /// <value>
        /// The timeout in minutes.
        /// </value>
        public int TimeoutInMinutes { get; set; }
    }
}