// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlDataAccess.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Core;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The SQL data access.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    [IoCRegistration(true)]
    public class SqlDataAccess<TEntity> : ISqlDataAccess<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// The database context.
        /// </summary>
        private readonly ISqlDataContext dataContext;

        /// <summary>
        /// The data set.
        /// </summary>
        private readonly DbSet<TEntity> dataset;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccess{TEntity}" /> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="businessContext">The business context.</param>
        public SqlDataAccess(ISqlDataContext dataContext)
        {
            ArgumentValidators.ThrowIfNull(dataContext, nameof(dataContext));

            this.dataContext = dataContext;
            this.dataset = this.dataContext.Set<TEntity>();
            this.dataContext.SetAccessToken();
        }

        /// <inheritdoc/>
        public DbSet<TEntity> EntitySet()
        {
            return this.dataset;
        }

        /// <inheritdoc/>
        public DbSet<T> Set<T>()
            where T : class, IEntity
        {
            return this.dataContext.Set<T>();
        }

        /// <inheritdoc/>
        public void Insert(TEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            this.dataset.Add(entity);
        }

        /// <inheritdoc/>
        public void InsertAll(IEnumerable<TEntity> entities)
        {
            ArgumentValidators.ThrowIfNull(entities, nameof(entities));
            this.dataset.AddRange(entities);
        }

        /// <inheritdoc/>
        public void UpdateAll(IEnumerable<TEntity> entities)
        {
            ArgumentValidators.ThrowIfNull(entities, nameof(entities));
            this.dataset.UpdateRange(entities);
        }

        /// <inheritdoc/>
        public void Update(TEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            this.dataset.Attach(entity);
            this.dataContext.Entry(entity).State = EntityState.Modified;
        }

        /// <inheritdoc/>
        public void Delete(TEntity entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            this.dataset.Remove(entity);
        }

        /// <inheritdoc/>
        public void DeleteAll(IEnumerable<TEntity> entities)
        {
            ArgumentValidators.ThrowIfNull(entities, nameof(entities));
            this.dataset.RemoveRange(entities);
        }

        /// <inheritdoc/>
        public Task<TEntity> GetByIdAsync(object id)
        {
            return this.dataset.FindAsync(id);
        }

        /// <inheritdoc/>
        public Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return predicate == null ? this.dataset.LongCountAsync() : this.dataset.LongCountAsync(predicate);
        }

        /// <inheritdoc/>
        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return predicate != null
                ? this.dataset.FirstOrDefaultAsync(predicate)
                : this.dataset.FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties)
        {
            IQueryable<TEntity> query = this.dataset;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, params string[] includeProperties)
        {
            IQueryable<TEntity> query = this.dataset;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.Select(selector).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return predicate != null
                ? this.dataset.SingleOrDefaultAsync(predicate)
                : this.dataset.SingleOrDefaultAsync();
        }

        /// <inheritdoc/>
        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties)
        {
            IQueryable<TEntity> query = this.dataset;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.SingleOrDefaultAsync();
        }

        /// <inheritdoc/>
        public Task<TResult> SingleOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, params string[] includeProperties)
        {
            IQueryable<TEntity> query = this.dataset;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.Select(selector).SingleOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties)
        {
            IQueryable<TEntity> query = this.dataset;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<TResult, bool>> predicate, params string[] includeProperties)
            where TResult : class, IEntity
        {
            IQueryable<TResult> query = this.Set<TResult>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, params string[] includeProperties)
        {
            IQueryable<TEntity> query = this.dataset;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.Select(selector).ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> OrderByAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, int? take)
        {
            IQueryable<TEntity> query = this.dataset;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> OrderByDescendingAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, int? take)
        {
            IQueryable<TEntity> query = this.dataset;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = query.OrderByDescending(orderBy);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<IQueryable<TEntity>> QueryAllAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties)
        {
            IQueryable<TEntity> query = this.dataset;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return Task.FromResult(query);
        }

        /// <inheritdoc/>
        public Task ExecuteAsync(object args, IDictionary<string, object> data)
        {
            ArgumentValidators.ThrowIfNull(args, nameof(args));
            ArgumentValidators.ThrowIfNull(data, nameof(data));

            var sql = BuildSql(args.ToString(), data.Keys);
            var parameters = data.Select(BuildParameters);

            return this.dataContext.Database.ExecuteSqlCommandAsync(new RawSqlString(sql), parameters);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> ExecuteQueryAsync(object args, IDictionary<string, object> data)
        {
            ArgumentValidators.ThrowIfNull(args, nameof(args));
            ArgumentValidators.ThrowIfNull(data, nameof(data));

            var sql = BuildSql(args.ToString(), data.Keys);
            var parameters = data.Select(BuildParameters);

            return await this.dataContext.Query<TEntity>().FromSql(new RawSqlString(sql), parameters.ToArray<object>()).ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<IQueryable<TEntity>> ExecuteViewAsync()
        {
            return Task.FromResult(this.dataContext.Query<TEntity>().AsQueryable());
        }

        private static string BuildSql(string name, IEnumerable<string> keys)
        {
            var sb = new StringBuilder();

            // build input parameters
            var input = keys.Where(k => !k.StartsWith("@out_", StringComparison.OrdinalIgnoreCase));
            sb.Append(name).Append(' ').Append(string.Join(",", input));

            // build output parameters
            var output = keys.Where(k => k.StartsWith("@out_", StringComparison.OrdinalIgnoreCase));
            output.ForEach(o =>
            {
                sb.Append(',').Append(o).Append(' ').Append("OUTPUT");
            });

            return sb.ToString();
        }

        private static SqlParameter BuildParameters(KeyValuePair<string, object> parameter)
        {
            var direction = parameter.Key.StartsWith("@out_", StringComparison.OrdinalIgnoreCase) ? ParameterDirection.Output : ParameterDirection.Input;
            var p = new SqlParameter
            {
                ParameterName = parameter.Key,
                Direction = direction,
            };

            // Output parameters of int type are only supported as of now
            if (direction == ParameterDirection.Output)
            {
                p.SqlDbType = SqlDbType.Int;
            }

            if (direction == ParameterDirection.Input)
            {
                p.Value = parameter.Value ?? DBNull.Value;
            }

            if (!(parameter.Value is DataTable dt))
            {
                return p;
            }

            p.TypeName = dt.TableName;
            p.SqlDbType = SqlDbType.Structured;

            return p;
        }
    }
}