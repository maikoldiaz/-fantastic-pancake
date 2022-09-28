// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataAccessBootsrapper.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core.Bootstrap
{
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.DataAccess;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Host.Functions.Core.Setup;
    using Ecp.True.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The configuration bootstrap service.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.Setup.BootstrapperBase" />
    [ExcludeFromCodeCoverage]
    public class DataAccessBootsrapper : BootstrapperBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessBootsrapper"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public DataAccessBootsrapper(IServiceCollection services)
            : base(services)
        {
        }

        /// <summary>
        /// Registers all DAL dependencies.
        /// </summary>
        public void Register()
        {
            this.RegisterScoped<ISqlDataContext, SqlDataContext>();
            this.RegisterScoped<ISqlTokenProvider, SqlTokenProvider>();
            this.RegisterSingleton<IConnectionFactory, ConnectionFactory>();
            this.RegisterTransient(typeof(ISqlDataAccess<>), typeof(SqlDataAccess<>));
            this.RegisterTransient(typeof(IDataAccess<>), typeof(SqlDataAccess<>));
            this.RegisterTransient<IRepositoryFactory, RepositoryFactory>();
            this.RegisterTransient(typeof(IRepository<>), typeof(Repository<>));
            this.RegisterTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
            this.RegisterTransient<IAuditService, AuditService>();
        }
    }
}
