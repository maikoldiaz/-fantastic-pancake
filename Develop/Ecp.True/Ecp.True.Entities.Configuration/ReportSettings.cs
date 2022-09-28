// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportSettings.cs" company="Microsoft">
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
    using System.Collections.Generic;

    /// <summary>
    /// The report config.
    /// </summary>
    public class ReportSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportSettings" /> class.
        /// </summary>
        public ReportSettings()
        {
            this.Reports = new Dictionary<string, ReportDetails>();
        }

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
        /// Gets or sets the principal identifier.
        /// </summary>
        /// <value>
        /// The principal identifier.
        /// </value>
        public string PrincipalId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        public string Scope { get; set; }

        /// <summary>
        /// Gets the reports.
        /// </summary>
        /// <value>
        /// The reports.
        /// </value>
        public IDictionary<string, ReportDetails> Reports { get; private set; }
    }
}
