// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditService.cs" company="Microsoft">
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
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Core;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    /// <summary>
    /// The audit service.
    /// </summary>
    /// <seealso cref="Ecp.True.DataAccess.Sql.Interfaces.IAuditService" />
    public class AuditService : IAuditService
    {
        private readonly IDictionary<string, string> excludeColumn = new Dictionary<string, string>
        {
            { nameof(Entity.CreatedBy), string.Empty },
            { nameof(Entity.CreatedDate), string.Empty },
            { nameof(Entity.LastModifiedBy), string.Empty },
            { nameof(Entity.LastModifiedDate), string.Empty },
        };

        /// <inheritdoc/>
        public IEnumerable<AuditLog> GetAuditLogs(ChangeTracker tracker)
        {
            ArgumentValidators.ThrowIfNull(tracker, nameof(tracker));

            var logs = new List<AuditLog>();

            tracker.DetectChanges();
            var entries = tracker.Entries().Where(e => e.State != EntityState.Detached && e.State != EntityState.Unchanged);

            entries.ForEach(e => logs.AddRange(this.BuildAuditLogs(e)));
            return logs;
        }

        /// <summary>
        /// Processes the changes entities.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// Return thw list of audit entity.
        /// </returns>
        private IList<AuditLog> BuildAuditLogs(EntityEntry entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            var logs = new List<AuditLog>();

            var auditable = entity.Entity as AuditableEntity;
            if (auditable == null || auditable?.IsAuditable == false)
            {
                return logs;
            }

            logs.AddRange(this.DoBuildAuditLogs(entity));
            return logs;
        }

        /// <summary>
        /// Builds the audit logs.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The audit logs.</returns>
        private IEnumerable<AuditLog> DoBuildAuditLogs(EntityEntry entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            var logs = new List<AuditLog>();
            var newValues = entity.Properties
                                    .Where(p => entity.State == EntityState.Added || entity.State == EntityState.Modified)
                                    .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);

            var oldValues = entity.Properties
                                    .Where(p => entity.State == EntityState.Deleted || entity.State == EntityState.Modified)
                                    .ToDictionary(p => p.Metadata.Name, p => p.OriginalValue);

            var entries = this.FilterEntries(oldValues, newValues);

            var relationalInformation = entity.Metadata.Relational();
            var entityName = $"{relationalInformation.Schema}.{relationalInformation.TableName}";
            var logDate = DateTime.UtcNow.ToTrue();

            entries.ForEach(item =>
            {
                string state = string.Empty;
                string user = string.Empty;
                if (entity.State == EntityState.Modified)
                {
                    state = "Update";
                    user = newValues["LastModifiedBy"]?.ToString();
                }
                else if (entity.State == EntityState.Deleted)
                {
                    state = "Delete";
                    var lastModifiedBy = entity.Properties.SingleOrDefault(p => p.Metadata.Name == "LastModifiedBy");
                    user = lastModifiedBy != null ? lastModifiedBy.CurrentValue.ToString() : string.Empty;
                }
                else
                {
                    state = "Insert";
                    user = newValues["CreatedBy"]?.ToString();
                }

                user ??= "trueadmin";

                item.LogDate = logDate;
                item.Entity = entityName;
                item.Identity = entity.Properties.Where(p => p.Metadata.IsPrimaryKey()).Select(x => x.CurrentValue.ToString()).First();
                item.LogType = state;
                item.User = user;
                item.EntityEntry = entity;

                logs.Add(item);
            });

            return logs;
        }

        /// <summary>
        /// Filters the entries.
        /// </summary>
        /// <param name="oldValues">The old values.</param>
        /// <param name="newValues">The new values.</param>
        /// <returns>The filtered audit logs.</returns>
        private IEnumerable<AuditLog> FilterEntries(IDictionary<string, object> oldValues, IDictionary<string, object> newValues)
        {
            if (oldValues.Count == 0)
            {
                return newValues
                    .Where(item => !this.excludeColumn.ContainsKey(item.Key))
                    .Select(item => new AuditLog { Field = item.Key, NewValue = item.Value?.ToString() });
            }

            // Update operation.
            var audits = new List<AuditLog>();
            oldValues.Where(v => newValues.ContainsKey(v.Key)).ForEach(old =>
            {
                var newValue = newValues[old.Key]?.ToString();
                var oldValue = old.Value?.ToString();
                if (!this.excludeColumn.ContainsKey(old.Key) && !string.Equals(oldValue, newValue, StringComparison.OrdinalIgnoreCase))
                {
                    audits.Add(new AuditLog { Field = old.Key, NewValue = newValue, OldValue = oldValue });
                }
            });

            // Delete operation
            oldValues.Where(v => !newValues.ContainsKey(v.Key)).ForEach(old =>
            {
                var oldValue = old.Value?.ToString();
                if (!this.excludeColumn.ContainsKey(old.Key))
                {
                    audits.Add(new AuditLog { Field = old.Key, OldValue = oldValue });
                }
            });

            return audits;
        }
    }
}
