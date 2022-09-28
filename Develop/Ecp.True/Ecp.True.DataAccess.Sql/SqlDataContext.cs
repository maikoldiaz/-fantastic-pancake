// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlDataContext.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ecp.True.DataAccess.Sql.Tests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ecp.True.Host.Functions.Core.Tests")]

namespace Ecp.True.DataAccess.Sql
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Entities.Admin;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Debug;

    /// <summary>
    /// The SQL data context.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [IoCRegistration(IoCLifetime.Hierarchical)]
    public class SqlDataContext : DbContext, ISqlDataContext
    {
        /// <summary>
        /// The debug logger factory.
        /// </summary>
        public static readonly ILoggerFactory DebugLoggerFactory = new LoggerFactory(
            new[] { new DebugLoggerProvider() },
            new LoggerFilterOptions
            {
                MinLevel = LogLevel.Information,
            });

        /// <summary>
        /// The connection factory.
        /// </summary>
        private readonly IConnectionFactory connectionFactory;

        /// <summary>
        /// The audit service.
        /// </summary>
        private readonly IAuditService auditService;

        /// <summary>
        /// The business context.
        /// </summary>
        private readonly IBusinessContext businessContext;

        /// <summary>
        /// The SQL token provider.
        /// </summary>
        private readonly ISqlTokenProvider sqlTokenProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataContext" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="auditService">The audit service.</param>
        /// <param name="businessContext">The business context.</param>
        /// <param name="sqlTokenProvider">The SQL token provider.</param>
        public SqlDataContext(IConnectionFactory connectionFactory, IAuditService auditService, IBusinessContext businessContext, ISqlTokenProvider sqlTokenProvider)
        {
            this.connectionFactory = connectionFactory;
            this.auditService = auditService;
            this.businessContext = businessContext;
            this.sqlTokenProvider = sqlTokenProvider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="auditService">The audit service.</param>
        /// <param name="businessContext">The business context.</param>
        /// <param name="sqlTokenProvider">The SQL token provider.</param>
        public SqlDataContext(DbContextOptions options, IAuditService auditService, IBusinessContext businessContext, ISqlTokenProvider sqlTokenProvider)
            : base(options)
        {
            this.auditService = auditService;
            this.businessContext = businessContext;
            this.sqlTokenProvider = sqlTokenProvider;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            var clearables = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted).ToList();

            clearables.ForEach(x => x.State = EntityState.Detached);
        }

        /// <summary>
        /// Saves the context asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// Number of rows effected.
        /// </returns>
        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Update common date and user properties.
                this.UpdateEntries();
                await this.TryPopulateUserAsync().ConfigureAwait(false);

                var auditLogs = this.auditService.GetAuditLogs(this.ChangeTracker);
                this.AddRange(auditLogs.Where(a => a.LogType != "Insert"));

                var count = await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                var inserts = auditLogs.Where(a => a.LogType == "Insert");
                inserts.ForEach(i =>
                {
                    var entity = i.EntityEntry as EntityEntry;
                    var property = entity.Properties.Where(p => p.Metadata.IsPrimaryKey());
                    var value = property.Select(t => t.CurrentValue.ToString()).First();

                    i.Identity = value;
                    if (i.Field == property.First().Metadata.Name)
                    {
                        i.NewValue = value;
                    }

                    property = entity.Properties.Where(p => p.Metadata.Name == i.Field);
                    var propertyValue = property.Select(t => t.CurrentValue).First();

                    if (propertyValue != null && i.NewValue != null && i.NewValue != propertyValue.ToString())
                    {
                        i.NewValue = propertyValue.ToString();
                    }
                });

                this.AddRange(inserts);
                count += await this.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return count;
            }
            catch
            {
                this.Clear();
                throw;
            }
        }

        /// <summary>
        /// Sets the access token.
        /// </summary>
        public void SetAccessToken()
        {
            if (this.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                return;
            }

            var connection = (SqlConnection)this.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed)
            {
                connection.AccessToken = this.sqlTokenProvider.AccessToken;
            }
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ArgumentValidators.ThrowIfNull(optionsBuilder, nameof(optionsBuilder));

            if (!optionsBuilder.IsConfigured)
            {
                var connStr = this.connectionFactory.SqlConnectionConfig.ConnectionString;
                var retryCount = this.connectionFactory.SqlConnectionConfig.MaxRetryCount;
                var retryInterval = TimeSpan.FromSeconds(this.connectionFactory.SqlConnectionConfig.RetryIntervalInSecs);
                var timeout = this.connectionFactory.SqlConnectionConfig.CommandTimeoutInSecs;

                optionsBuilder.UseSqlServer(connStr, b => b.CommandTimeout(timeout).EnableRetryOnFailure(retryCount, retryInterval, null));
                optionsBuilder.UseLoggerFactory(DebugLoggerFactory);
            }
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentValidators.ThrowIfNull(modelBuilder, nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

        private static bool CheckIfEntryIsAdded(EntityEntry entry)
        {
            return entry.State == EntityState.Added && entry.Metadata.FindProperty("CreatedDate") != null && entry.Metadata.FindProperty("CreatedBy") != null;
        }

        private void UpdateEntries()
        {
            var saveTime = DateTime.UtcNow.ToTrue();
            this.ChangeTracker.Entries().ForEach((entry) =>
            {
                if (CheckIfEntryIsAdded(entry))
                {
                    entry.Property("CreatedDate").CurrentValue = saveTime;
                    entry.Property("CreatedBy").CurrentValue = this.businessContext.UserId ?? "System";
                }

                if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    if (entry.Metadata.FindProperty("LastModifiedDate") != null && entry.Metadata.FindProperty("LastModifiedBy") != null)
                    {
                        entry.Property("LastModifiedDate").CurrentValue = saveTime;
                        entry.Property("LastModifiedBy").CurrentValue = this.businessContext.UserId ?? "System";
                    }

                    if (entry.Metadata.FindProperty("RowVersion") != null)
                    {
                        entry.Property("RowVersion").OriginalValue = entry.Property("RowVersion").CurrentValue;
                    }
                }
            });
        }

        private async Task TryPopulateUserAsync()
        {
            if (string.IsNullOrWhiteSpace(this.businessContext.Email))
            {
                return;
            }

            var repo = this.Set<User>();

            // If user changes the name in AD, it will be created as new entry.
            var existing = await repo.SingleOrDefaultAsync(u => u.Email == this.businessContext.Email && u.Name == this.businessContext.UserId).ConfigureAwait(false);

            if (existing == null)
            {
                var user = new User
                {
                    Email = this.businessContext.Email,
                    Name = this.businessContext.UserId,
                    CreatedBy = this.businessContext.UserId,
                    CreatedDate = DateTime.UtcNow.ToTrue(),
                };
                this.Add(user);
            }
        }
    }
}