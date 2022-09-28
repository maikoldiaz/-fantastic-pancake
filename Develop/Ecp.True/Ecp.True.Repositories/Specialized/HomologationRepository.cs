// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationRepository.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Repositories.Specialized
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The Homologation Repository.
    /// </summary>
    public class HomologationRepository : Repository<Homologation>, IHomologationRepository
    {
        /// <summary>
        /// The sql data access.
        /// </summary>
        private readonly ISqlDataAccess<Homologation> sqlDataAccess;

        /// <summary>
        /// The queries.
        /// </summary>
        private readonly IDictionary<int, Tuple<IQueryable<HomologationDataMapping>, IQueryable<HomologationDataMapping>>> queries;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomologationRepository"/> class.
        /// </summary>
        /// <param name="sqlDataAccess">The sql data access.</param>
        public HomologationRepository(ISqlDataAccess<Homologation> sqlDataAccess)
            : base(sqlDataAccess)
        {
            this.sqlDataAccess = sqlDataAccess;

            this.queries = new Dictionary<int, Tuple<IQueryable<HomologationDataMapping>, IQueryable<HomologationDataMapping>>>();
            this.BuildQueries();
        }

        /// <summary>
        /// Gets the homologations.
        /// </summary>
        /// <value>
        /// The homologations.
        /// </value>
        private DbSet<Homologation> Homologations => this.sqlDataAccess.EntitySet();

        /// <summary>
        /// Gets the homologation groups.
        /// </summary>
        /// <value>
        /// The homologation groups.
        /// </value>
        private DbSet<HomologationGroup> HomologationGroups => this.sqlDataAccess.Set<HomologationGroup>();

        /// <summary>
        /// Gets the homologation data mappings.
        /// </summary>
        /// <value>
        /// The homologation groups.
        /// </value>
        private DbSet<HomologationDataMapping> HomologationDataMappings => this.sqlDataAccess.Set<HomologationDataMapping>();

        /// <inheritdoc/>
        public Task<HomologationGroup> GetHomologationByIdAndGroupIdAsync(int homologationId, int groupId)
        {
            var query = from hm in this.Homologations
                        join hmg in this.HomologationGroups on hm.HomologationId equals hmg.HomologationId
                        where hmg.GroupTypeId == groupId && hm.HomologationId == homologationId
                        select hmg;

            return query
                    .Include(h => h.HomologationObjects)
                    .Include(h => h.HomologationDataMapping)
                    .SingleOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<HomologationDataMapping>> GetHomologationGroupMappingsAsync(int homologationGroupId)
        {
            var query = from grp in this.HomologationGroups
                        where grp.HomologationGroupId == homologationGroupId
                        select grp;
            var entity = await query.Include(h => h.Homologation).FirstOrDefaultAsync().ConfigureAwait(false);

            if (entity == null)
            {
                throw new KeyNotFoundException(Entities.Constants.HomologationGroupDoesNotExists);
            }

            return await this.DoGetMappingsAsync(entity.GroupTypeId.Value, entity).ConfigureAwait(false);
        }

        private async Task<IEnumerable<HomologationDataMapping>> DoGetMappingsAsync(int groupTypeId, HomologationGroup group)
        {
            var groupQuery = this.queries.ContainsKey(groupTypeId) ?
                                    this.queries[groupTypeId] :
                                    this.GetQuery(this.sqlDataAccess.Set<CategoryElement>(), n => n.ElementId.ToString(CultureInfo.InvariantCulture), n => n.Name);

            var mappingQuery = group.Homologation.SourceSystemId == (int)SystemType.TRUE ? groupQuery.Item1 : groupQuery.Item2;
            var mappings = await mappingQuery.ToListAsync().ConfigureAwait(false);

            return mappings.Where(m => m.HomologationGroupId == group.HomologationGroupId);
        }

        private void BuildQueries()
        {
            this.queries.Add(
                Core.Constants.GroupTypeNode,
                this.GetQuery(this.sqlDataAccess.Set<Node>(), n => n.NodeId.ToString(CultureInfo.InvariantCulture), n => n.Name));
            this.queries.Add(
                Core.Constants.GroupTypeProduct,
                this.GetQuery(this.sqlDataAccess.Set<Product>(), n => n.ProductId, n => n.Name));
            this.queries.Add(
                Core.Constants.GroupTypeStorageLocation,
                this.GetQuery(this.sqlDataAccess.Set<StorageLocation>(), n => n.StorageLocationId, n => n.Name));
        }

        private Tuple<IQueryable<HomologationDataMapping>, IQueryable<HomologationDataMapping>>
            GetQuery<TInner>(IQueryable<TInner> other, Func<TInner, string> keyFunc, Func<TInner, string> valueFunc)
        {
            var sourceQuery = from hdm in this.HomologationDataMappings
                              join oth in other on hdm.SourceValue equals keyFunc(oth)
                              select new HomologationDataMapping
                              {
                                  Value = valueFunc(oth),
                                  SourceValue = hdm.SourceValue,
                                  DestinationValue = hdm.DestinationValue,
                                  HomologationDataMappingId = hdm.HomologationDataMappingId,
                                  HomologationGroupId = hdm.HomologationGroupId,
                              };

            var destinationQuery = from hdm in this.HomologationDataMappings
                                   join oth in other on hdm.DestinationValue equals keyFunc(oth)
                                   select new HomologationDataMapping
                                   {
                                      Value = valueFunc(oth),
                                      SourceValue = hdm.SourceValue,
                                      DestinationValue = hdm.DestinationValue,
                                      HomologationDataMappingId = hdm.HomologationDataMappingId,
                                      HomologationGroupId = hdm.HomologationGroupId,
                                   };
            return Tuple.Create(sourceQuery, destinationQuery);
        }
    }
}
