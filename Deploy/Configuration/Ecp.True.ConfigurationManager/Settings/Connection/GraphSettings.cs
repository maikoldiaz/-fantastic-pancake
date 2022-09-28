// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphSettings.cs" company="Microsoft">
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
    /// The Graph Connection Settings.
    /// </summary>
    public class GraphSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphSettings"/> class.
        /// </summary>
        public GraphSettings()
        {
            this.GraphApiPath = "https://graph.microsoft.com/beta";
            this.GraphScope = "User.ReadBasic.All";
            this.ClientSecret = "#GraphADClientSecret#";
        }

        /// <summary>
        /// Gets or sets the graph api path.
        /// </summary>
        /// <value>
        /// The graph api path.
        /// </value>
        public string GraphApiPath { get; set; }

        /// <summary>
        /// Gets or sets the graph api scope.
        /// </summary>
        /// <value>
        /// The graph api scope.
        /// </value>
        public string GraphScope { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        public string TenantId { get; set; }

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
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "Graph.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.GraphApiPath = input.GetStringValueOrDefault(nameof(this.GraphApiPath), this.GraphApiPath, this.Key);
            this.GraphScope = input.GetStringValueOrDefault(nameof(this.GraphScope), this.GraphScope, this.Key);
        }
    }
}
