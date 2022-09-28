// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityConfiguration.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql.Configuration
{
    using System;
    using System.Linq.Expressions;
    using Ecp.True.Core;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Core;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The Base Configuration.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{TEntity}" />
    public abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// The schema name.
        /// </summary>
        private readonly string schemaName;

        /// <summary>
        /// The identity column.
        /// </summary>
        private readonly Expression<Func<TEntity, object>> propertyExpression;

        /// <summary>
        /// The has identity.
        /// </summary>
        private readonly bool hasIdentity;

        /// <summary>
        /// The table name.
        /// </summary>
        private readonly string tableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityConfiguration{TEntity}"/> class.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="schemaName">The schema name.</param>
        /// <param name="hasIdentity">if set to <c>true</c> [has identity].</param>
        protected EntityConfiguration(Expression<Func<TEntity, object>> propertyExpression, string schemaName, bool hasIdentity)
        {
            this.propertyExpression = propertyExpression;
            this.schemaName = schemaName;
            this.hasIdentity = hasIdentity;
            this.tableName = typeof(TEntity).Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityConfiguration{TEntity}"/> class.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="schemaName">The schema name.</param>
        /// <param name="hasIdentity">if set to <c>true</c> [has identity].</param>
        /// <param name="tableName">The table name.</param>
        protected EntityConfiguration(Expression<Func<TEntity, object>> propertyExpression, string schemaName, bool hasIdentity, string tableName)
        {
            this.propertyExpression = propertyExpression;
            this.schemaName = schemaName;
            this.hasIdentity = hasIdentity;
            this.tableName = tableName;
        }

        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.ToTable(this.tableName, this.schemaName);
            builder.HasKey(this.propertyExpression);

            if (this.hasIdentity)
            {
                builder.Property(this.propertyExpression)
                    .HasColumnName(this.propertyExpression.GetPropertyInfo()?.Name)
                    .HasColumnType("int")
                    .IsRequired()
                    .UseSqlServerIdentityColumn();
            }

            if (typeof(TEntity) != typeof(AuditLog))
            {
                builder.Property(x => x.CreatedBy).HasColumnName(@"CreatedBy").HasColumnType("nvarchar").HasMaxLength(60).IsRequired();
                builder.Property(x => x.CreatedDate).HasColumnName(@"CreatedDate").HasColumnType("datetime").IsRequired();
                builder.Property(x => x.LastModifiedBy).HasColumnName(@"LastModifiedBy").HasColumnType("nvarchar").HasMaxLength(60);
                builder.Property(x => x.LastModifiedDate).HasColumnName(@"LastModifiedDate").HasColumnType("datetime");
            }
            else
            {
                builder.Ignore(x => x.CreatedBy);
                builder.Ignore(x => x.CreatedDate);
                builder.Ignore(x => x.LastModifiedBy);
                builder.Ignore(x => x.LastModifiedDate);
            }

            if (typeof(TEntity).IsSubclassOf(typeof(EditableEntity)))
            {
                builder.Property("RowVersion").HasColumnName(@"RowVersion").IsRowVersion();
            }

            this.DoConfigure(builder);
        }

        /// <summary>
        /// Does the configure.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected abstract void DoConfigure(EntityTypeBuilder<TEntity> builder);
    }
}
