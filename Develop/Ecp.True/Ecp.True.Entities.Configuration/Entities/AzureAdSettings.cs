// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureAdSettings.cs" company="Microsoft">
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
    /// The Azure AD settings.
    /// </summary>
    public class AzureAdSettings
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
        /// Gets or sets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public string Instance { get; set; }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the callback path.
        /// </summary>
        /// <value>
        /// The callback path.
        /// </value>
        public string CallbackPath { get; set; }

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        public string BasePath { get; set; }

        /// <summary>
        /// Gets or sets the scopes.
        /// </summary>
        /// <value>
        /// The scopes.
        /// </value>
        public string Scopes { get; set; }

        /// <summary>
        /// Gets or sets the graph resource identifier.
        /// </summary>
        /// <value>
        /// The graph resource identifier.
        /// </value>
        public string GraphResourceId { get; set; }

        /// <summary>
        /// Gets or sets the graph scopes.
        /// </summary>
        /// <value>
        /// The graph scopes.
        /// </value>
        public string GraphScopes { get; set; }
    }
}
