// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Repositories
{
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Repositories.Specialized;
    using EfCore.Models;

    /// <summary>
    /// The repository factory.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Interfaces.IRepositoryFactory" />
    public class RepositoryFactory : IRepositoryFactory
    {
        /// <summary>
        /// The data context.
        /// </summary>
        private readonly ISqlDataContext dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryFactory" /> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="businessContext">The business context.</param>
        public RepositoryFactory(ISqlDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        /// <summary>
        /// Gets the movement repository.
        /// </summary>
        /// <value>
        /// The movement repository.
        /// </value>
        public IMovementRepository MovementRepository => new MovementRepository(new SqlDataAccess<Movement>(this.dataContext));

        /// <summary>
        /// Gets the inventory product repository.
        /// </summary>
        /// <value>
        /// The inventory product repository.
        /// </value>
        public IInventoryProductRepository InventoryProductRepository => new InventoryProductRepository(new SqlDataAccess<InventoryProduct>(this.dataContext));

        /// <summary>
        /// Gets the movement repository.
        /// </summary>
        /// <value>
        /// The movement repository.
        /// </value>
        public ITicketInfoRepository TicketInfoRepository => new TicketInfoRepository(
            new SqlDataAccess<Ticket>(this.dataContext),
            new SqlDataAccess<Movement>(this.dataContext),
            new SqlDataAccess<InventoryProduct>(this.dataContext));

        /// <inheritdoc/>
        public IHomologationRepository HomologationRepository => new HomologationRepository(new SqlDataAccess<Homologation>(this.dataContext));

        /// <inheritdoc/>
        public INodeRepository NodeRepository => new NodeRepository(new SqlDataAccess<Node>(this.dataContext), new SqlDataAccess<NodeTag>(this.dataContext));

        /// <inheritdoc/>
        public INodeOwnershipRepository NodeOwnershipRepository =>
            new NodeOwnershipRepository(
                new SqlDataAccess<NodeConnectionProductOwner>(this.dataContext),
                new SqlDataAccess<NodeConnectionProduct>(this.dataContext),
                new SqlDataAccess<NodeConnection>(this.dataContext),
                new SqlDataAccess<Node>(this.dataContext),
                new SqlDataAccess<NodeStorageLocation>(this.dataContext),
                new SqlDataAccess<StorageLocationProduct>(this.dataContext),
                new SqlDataAccess<StorageLocationProductOwner>(this.dataContext),
                new SqlDataAccess<Product>(this.dataContext));

        /// <inheritdoc/>
        public IRepository<TEntity> CreateRepository<TEntity>()
            where TEntity : class, IEntity
        {
            return new Repository<TEntity>(new SqlDataAccess<TEntity>(this.dataContext));
        }
    }
}