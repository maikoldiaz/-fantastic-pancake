// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryEntityConfiguration.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql.Configuration.Query
{
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The base query configuration.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{TEntity}" />
    public abstract class QueryEntityConfiguration<TEntity> : IQueryTypeConfiguration<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// The should Ignore.
        /// </summary>
        private readonly bool shouldIgnore;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryEntityConfiguration{TEntity}"/> class.
        /// </summary>
        /// <param name="shouldIgnore">should Ignore.</param>
        protected QueryEntityConfiguration()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryEntityConfiguration{TEntity}"/> class.
        /// </summary>
        /// <param name="shouldIgnore">should Ignore.</param>
        protected QueryEntityConfiguration(bool shouldIgnore)
        {
            this.shouldIgnore = shouldIgnore;
        }

        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(QueryTypeBuilder<TEntity> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            if (this.shouldIgnore)
            {
                builder.Ignore(x => x.CreatedBy);
                builder.Ignore(x => x.CreatedDate);
            }

            builder.Ignore(x => x.LastModifiedBy);
            builder.Ignore(x => x.LastModifiedDate);
            if (typeof(TEntity).IsSubclassOf(typeof(VersionableEntity)))
            {
                builder.Property("RowVersion").HasColumnName(@"RowVersion");
                builder.Ignore("Version");
            }

            this.DoConfigure(builder);
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected abstract void DoConfigure(QueryTypeBuilder<TEntity> builder);
    }
}
