// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core
{
    using Ecp.True.Core.Attributes;
    using Ecp.True.DataAccess;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;

    /// <summary>
    /// The unit of work factory.
    /// </summary>
    /// <seealso cref="IUnitOfWorkFactory" />
    [IoCRegistration(IoCLifetime.Hierarchical)]
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        /// <summary>
        /// The data context.
        /// </summary>
        private readonly ISqlDataContext dataContext;

        /// <summary>
        /// The repository factory.
        /// </summary>
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkFactory" /> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        public UnitOfWorkFactory(ISqlDataContext dataContext, IRepositoryFactory repositoryFactory)
        {
            this.dataContext = dataContext;
            this.repositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <returns>
        /// The unit of work.
        /// </returns>
        public IUnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork(this.dataContext, this.repositoryFactory);
        }
    }
}