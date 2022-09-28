// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessorBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Core.Attributes;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The processor Base.
    /// </summary>
    [IoCRegistration(true)]
    public class ProcessorBase : IProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessorBase"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        protected ProcessorBase(
            IRepositoryFactory factory)
        {
            this.RepositoryFactory = factory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessorBase"/> class.
        /// </summary>
        protected ProcessorBase()
        {
        }

        /// <summary>
        /// Gets the repository factory.
        /// </summary>
        /// <value>
        /// The repository factory.
        /// </value>
        protected IRepositoryFactory RepositoryFactory { get; }

        /// <summary>
        /// Queries all entities.
        /// Use this method for OData enabled queries.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        /// The entity query.
        /// </returns>
        public Task<IQueryable<TEntity>> QueryAllAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties)
            where TEntity : class, IEntity
        {
            return this.CreateRepository<TEntity>().QueryAllAsync(predicate, includeProperties);
        }

        /// <summary>
        /// Queries the view asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The View Query.</returns>
        public Task<IQueryable<TEntity>> QueryViewAsync<TEntity>()
            where TEntity : class, IEntity
        {
            return this.CreateRepository<TEntity>().ExecuteViewAsync();
        }

        /// <summary>
        /// Creates the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>
        /// The repository.
        /// </returns>
        protected IRepository<TEntity> CreateRepository<TEntity>()
            where TEntity : class, IEntity
        {
            return this.RepositoryFactory.CreateRepository<TEntity>();
        }
    }
}
