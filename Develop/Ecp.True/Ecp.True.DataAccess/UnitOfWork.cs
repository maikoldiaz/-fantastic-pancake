// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWork.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The unit of work.
    /// </summary>
    /// <seealso cref="IUnitOfWork" />
    public sealed class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The data context.
        /// </summary>
        private readonly IDataContext dataContext;

        /// <summary>
        /// The repository factory.
        /// </summary>
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        public UnitOfWork(IDataContext dataContext, IRepositoryFactory repositoryFactory)
        {
            this.dataContext = dataContext;
            this.repositoryFactory = repositoryFactory;
        }

        /// <inheritdoc/>
        public IMovementRepository MovementRepository => this.repositoryFactory.MovementRepository;

        /// <inheritdoc/>
        public IInventoryProductRepository InventoryProductRepository => this.repositoryFactory.InventoryProductRepository;

       /// <inheritdoc/>
        public ITicketInfoRepository TicketInfoRepository => this.repositoryFactory.TicketInfoRepository;

        /// <inheritdoc/>
        public IRepository<TEntity> CreateRepository<TEntity>()
            where TEntity : class, IEntity
        {
            return this.repositoryFactory.CreateRepository<TEntity>();
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// 1, when saved successfully, 0 otherwise.
        /// </returns>
        public Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return this.dataContext.SaveAsync(cancellationToken);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.dataContext.Dispose();
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.dataContext.Clear();
        }
    }
}