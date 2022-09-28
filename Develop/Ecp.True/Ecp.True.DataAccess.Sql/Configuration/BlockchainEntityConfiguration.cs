// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainEntityConfiguration.cs" company="Microsoft">
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
    using Ecp.True.Entities.Core;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The blockchain entity configuration.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Configuration.EntityConfiguration{TEntity}" />
    public class BlockchainEntityConfiguration<TEntity> : EntityConfiguration<TEntity>
        where TEntity : class, IBlockchainEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainEntityConfiguration{TEntity}"/> class.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="schemaName">The schema name.</param>
        /// <param name="hasIdentity">if set to <c>true</c> [has identity].</param>
        protected BlockchainEntityConfiguration(Expression<Func<TEntity, object>> propertyExpression, string schemaName, bool hasIdentity)
            : base(propertyExpression, schemaName, hasIdentity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainEntityConfiguration{TEntity}"/> class.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="schemaName">The schema name.</param>
        /// <param name="hasIdentity">if set to <c>true</c> [has identity].</param>
        /// <param name="tableName">The table name.</param>
        protected BlockchainEntityConfiguration(Expression<Func<TEntity, object>> propertyExpression, string schemaName, bool hasIdentity, string tableName)
            : base(propertyExpression, schemaName, hasIdentity, tableName)
        {
        }

        /// <inheritdoc/>
        protected override void DoConfigure(EntityTypeBuilder<TEntity> builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            builder.Property(x => x.TransactionHash).IsRequired(false).HasMaxLength(255);
            builder.Property(x => x.BlockNumber).IsRequired(false).HasMaxLength(255);
            builder.Property(x => x.BlockchainStatus).IsRequired().HasColumnType("int");
            builder.Property(x => x.RetryCount).IsRequired().HasColumnType("int").HasDefaultValue(0);
        }
    }
}
