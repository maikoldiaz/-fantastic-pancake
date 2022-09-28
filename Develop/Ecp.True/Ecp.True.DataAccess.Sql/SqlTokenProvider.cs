// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlTokenProvider.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql
{
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.DataAccess.Interfaces;
    using Microsoft.Azure.Services.AppAuthentication;

    /// <summary>
    /// SQL Token Provider.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.ISqlTokenProvider" />
    [IoCRegistration(IoCLifetime.Hierarchical)]
    public class SqlTokenProvider : ISqlTokenProvider
    {
        /// <summary>
        /// The connection factory.
        /// </summary>
        private readonly IConnectionFactory connectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlTokenProvider"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        public SqlTokenProvider(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        public async Task InitializeAsync()
        {
            ArgumentValidators.ThrowIfNull(this.connectionFactory, nameof(this.connectionFactory));
            ArgumentValidators.ThrowIfNull(this.connectionFactory.SqlConnectionConfig, nameof(this.connectionFactory.SqlConnectionConfig));

            this.AccessToken = await new AzureServiceTokenProvider().GetAccessTokenAsync(
                this.connectionFactory.SqlConnectionConfig.ResourceUrl,
                this.connectionFactory.SqlConnectionConfig.TenantId).ConfigureAwait(false);
        }
    }
}
