// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Repository.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Specifications;

    /// <summary>
    /// The repository type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    [IoCRegistration(true)]
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// The data access.
        /// </summary>
        private readonly IDataAccess<TEntity> dataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="dataAccess">The data access.</param>
        public Repository(IDataAccess<TEntity> dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        /// <inheritdoc/>
        public virtual void Insert(TEntity entity)
        {
            this.dataAccess.Insert(entity);
        }

        /// <inheritdoc/>
        public virtual void InsertAll(IEnumerable<TEntity> entities)
        {
            this.dataAccess.InsertAll(entities);
        }

        /// <inheritdoc/>
        public virtual void UpdateAll(IEnumerable<TEntity> entities)
        {
            this.dataAccess.UpdateAll(entities);
        }

        /// <inheritdoc/>
        public virtual void Update(TEntity entity)
        {
            this.dataAccess.Update(entity);
        }

        /// <inheritdoc/>
        public virtual void Upsert(IEnumerable<TEntity> existing, IEnumerable<TEntity> newEntities, Expression<Func<TEntity, bool>> predicate)
        {
            ArgumentValidators.ThrowIfNull(predicate, nameof(predicate));
            ArgumentValidators.ThrowIfNull(newEntities, nameof(newEntities));
            foreach (var entity in newEntities)
            {
                var trueEntity = existing.SingleOrDefault(predicate.Compile());
                if (trueEntity == null)
                {
                    this.dataAccess.Insert(trueEntity);
                }
                else
                {
                    trueEntity.CopyFrom(entity);
                    this.dataAccess.Update(trueEntity);
                }
            }
        }

        /// <inheritdoc/>
        public virtual void Delete(TEntity entity)
        {
            this.dataAccess.Delete(entity);
        }

        /// <inheritdoc/>
        public virtual void DeleteAll(IEnumerable<TEntity> entities)
        {
            this.dataAccess.DeleteAll(entities);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> GetByIdAsync(object id)
        {
            return this.dataAccess.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public virtual Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dataAccess.GetCountAsync(predicate);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dataAccess.FirstOrDefaultAsync(predicate);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties)
        {
            return this.dataAccess.FirstOrDefaultAsync(predicate, includeProperties);
        }

        /// <inheritdoc/>
        public virtual Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, params string[] includeProperties)
        {
            return this.dataAccess.FirstOrDefaultAsync(predicate, selector, includeProperties);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dataAccess.SingleOrDefaultAsync(predicate);
        }

        /// <inheritdoc/>
        public virtual Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties)
        {
            return this.dataAccess.SingleOrDefaultAsync(predicate, includeProperties);
        }

        /// <inheritdoc/>
        public virtual Task<TResult> SingleOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, params string[] includeProperties)
        {
            return this.dataAccess.SingleOrDefaultAsync(predicate, selector, includeProperties);
        }

        /// <inheritdoc/>
        public virtual Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties)
        {
            return this.dataAccess.GetAllAsync(predicate, includeProperties);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate, params string[] includeProperties)
            where T : class, IEntity
        {
            return this.dataAccess.GetAllAsync(predicate, includeProperties);
        }

        /// <inheritdoc/>
        public virtual Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, params string[] includeProperties)
        {
            return this.dataAccess.GetAllAsync(predicate, selector, includeProperties);
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> GetAllSpecificAsync(CompositeSpecification<TEntity> specification)
        {
            ArgumentValidators.ThrowIfNull(specification, nameof(specification));

            var includes = specification.IncludeProperties;
            var entities = await this.dataAccess.GetAllAsync(specification, includes.ToArray())
                .ConfigureAwait(false);

            return entities.Where(specification);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> OrderByAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, int? take)
        {
            return this.dataAccess.OrderByAsync(predicate, orderBy, take);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> OrderByDescendingAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, int? take)
        {
            return this.dataAccess.OrderByDescendingAsync(predicate, orderBy, take);
        }

        /// <inheritdoc/>
        public Task<IQueryable<TEntity>> QueryAllAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties)
        {
            return this.dataAccess.QueryAllAsync(predicate, includeProperties);
        }

        /// <inheritdoc/>
        public Task ExecuteAsync(object args, IDictionary<string, object> data)
        {
            return this.dataAccess.ExecuteAsync(args, data);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> ExecuteQueryAsync(object args, IDictionary<string, object> data)
        {
            return this.dataAccess.ExecuteQueryAsync(args, data);
        }

        /// <inheritdoc/>
        public Task<IQueryable<TEntity>> ExecuteViewAsync()
        {
            return this.dataAccess.ExecuteViewAsync();
        }
    }
}