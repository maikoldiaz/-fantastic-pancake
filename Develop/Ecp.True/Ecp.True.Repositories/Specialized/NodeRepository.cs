﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeRepository.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
// <auto-generated />
namespace Ecp.True.Repositories.Specialized
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using EfCore.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The Node Repository.
    /// </summary>
    /// <seealso cref="Ecp.True.Repositories.Repository{Ecp.True.Entities.Admin.Node}" />
    /// <seealso cref="Ecp.True.DataAccess.Interfaces.INodeRepository" />
    public class NodeRepository : Repository<Node>, INodeRepository
    {
        /// <summary>
        /// The SQL data access.
        /// </summary>
        private readonly ISqlDataAccess<Node> sqlDataAccess;

        /// <summary>
        /// The node tag data access.
        /// </summary>
        private readonly ISqlDataAccess<NodeTag> sqlNodeTagDataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeRepository"/> class.
        /// </summary>
        /// <param name="sqlDataAccess">The SQL data access.</param>
        /// <param name="sqlNodeTagDataAccess">The SQL node tag data access.</param>
        public NodeRepository(ISqlDataAccess<Node> sqlDataAccess, ISqlDataAccess<NodeTag> sqlNodeTagDataAccess)
            : base(sqlDataAccess)
        {
            this.sqlDataAccess = sqlDataAccess;
            this.sqlNodeTagDataAccess = sqlNodeTagDataAccess;
        }

        /// <summary>
        /// Gets the Nodes.
        /// </summary>
        /// <value>
        /// The Nodes.
        /// </value>
        private IQueryable<Node> Nodes => this.sqlDataAccess.EntitySet().Include(x => x.LogisticCenter).Include(x => x.Unit);

        /// <summary>
        /// Gets the homologation groups.
        /// </summary>
        /// <value>
        /// The homologation groups.
        /// </value>
        private IQueryable<NodeTag> NodeTags => this.sqlDataAccess.Set<NodeTag>().Include(x => x.CategoryElement);

        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        private IQueryable<Category> Categories => this.sqlDataAccess.Set<Category>();

        /// <summary>
        /// Gets the category elements.
        /// </summary>
        /// <value>
        /// The category elements.
        /// </value>
        private IQueryable<CategoryElement> CategoryElements => this.sqlDataAccess.Set<CategoryElement>();
        /// <summary>
        /// Filters the nodes asynchronous.
        /// </summary>
        /// <param name="nodesFilter">The nodes filter.</param>
        /// <returns>The collection of nodes.</returns>
        public IQueryable<Node> FilterNodes(NodesFilter nodesFilter)
        {
            ArgumentValidators.ThrowIfNull(nodesFilter, nameof(nodesFilter));
            var currentTime = DateTime.UtcNow.ToTrue();

            var query = from node in this.Nodes
                        join segmentNodeTag in this.NodeTags
                        on node.NodeId equals segmentNodeTag.NodeId
                        join typeNodeTag in this.NodeTags
                        on node.NodeId equals typeNodeTag.NodeId
                        join operatorNodeTag in this.NodeTags
                        on node.NodeId equals operatorNodeTag.NodeId
                        where segmentNodeTag.ElementId == nodesFilter.SegmentId
                        && segmentNodeTag.StartDate <= currentTime && segmentNodeTag.EndDate >= currentTime
                        select new { node, segmentNodeTag, typeNodeTag, operatorNodeTag,  };

            query = nodesFilter.NodeTypeIds.Any() ? from a in query
                                                    where nodesFilter.NodeTypeIds.Contains(a.typeNodeTag.ElementId)
                                                    && a.typeNodeTag.StartDate <= currentTime && a.typeNodeTag.EndDate >= currentTime
                                                    select a :
                         from a in query
                         where a.typeNodeTag.CategoryElement.CategoryId == (int)NodeTagType.NodeType && a.typeNodeTag.CategoryElement.IsActive.HasValue && a.typeNodeTag.CategoryElement.IsActive.Value
                         && a.typeNodeTag.StartDate <= currentTime && a.typeNodeTag.EndDate >= currentTime
                         select a;

            query = nodesFilter.OperatorIds.Any() ? from a in query
                                                    where nodesFilter.OperatorIds.Contains(a.operatorNodeTag.ElementId)
                                                    && a.operatorNodeTag.StartDate <= currentTime && a.operatorNodeTag.EndDate >= currentTime
                                                    select a :
                         from a in query
                         where a.operatorNodeTag.CategoryElement.CategoryId == (int)NodeTagType.Operator && a.operatorNodeTag.CategoryElement.IsActive.HasValue && a.operatorNodeTag.CategoryElement.IsActive.Value
                         && a.operatorNodeTag.StartDate <= currentTime && a.operatorNodeTag.EndDate >= currentTime
                         select a;

            var nodes = query.Select(a => BuildNode(a.node, a.segmentNodeTag, a.typeNodeTag, a.operatorNodeTag));
            return nodes;

        }

        /// <inheritdoc/>
        public async Task<Node> GetNodeWithSameOrderAsync(int nodeId, int segmentId, int order)
        {
            var currentTime = DateTime.UtcNow.ToTrue();
            var query = from nodeTag in this.NodeTags
                        where nodeTag.StartDate <= currentTime && nodeTag.EndDate >= currentTime
                        && nodeTag.ElementId == segmentId
                        join node in this.Nodes on nodeTag.NodeId equals node.NodeId
                        select node;
            return await query.SingleOrDefaultAsync(a => a.Order == order && a.NodeId != nodeId).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Node>> GetNodesWithSameOrHigherOrderAsync(int segmentId, int order)
        {
            var currentTime = DateTime.UtcNow.ToTrue();

            var query = from nodeTag in this.NodeTags
                        where nodeTag.StartDate <= currentTime && nodeTag.EndDate >= currentTime
                        && nodeTag.ElementId == segmentId
                        join node in this.Nodes on nodeTag.NodeId equals node.NodeId
                        where node.Order.Value >= order
                        select node;
            return await query.ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CategoryElement> GetSegmentDetailForNodeAsync(int nodeId)
        {
            var query = from tag in this.NodeTags
                        join el in this.CategoryElements
                        on tag.ElementId equals el.ElementId
                        join ct in this.Categories
                        on el.CategoryId equals ct.CategoryId
                        where tag.NodeId == nodeId && ct.Name == Ecp.True.Repositories.Constants.SegmentCategoryNameEs
                        select el;

            return await query.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CategoryElement> GetNodeTypeForNodeAsync(int nodeId)
        {
            var currentTime = DateTime.UtcNow.ToTrue();

            var query = from nodeTag in this.NodeTags
                        where nodeTag.StartDate <= currentTime && nodeTag.EndDate >= currentTime
                        join catElement in this.CategoryElements
                        on nodeTag.ElementId equals catElement.ElementId
                        join category in this.Categories
                        on catElement.CategoryId equals category.CategoryId
                        where nodeTag.NodeId == nodeId && category.Name == Ecp.True.Repositories.Constants.NodeTypeCategoryName
                        select catElement;

            return await query.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        private static Node BuildNode(Node node, NodeTag segmentNodeTag, NodeTag typeNodeTag, NodeTag operatorNodeTag)
        {
            segmentNodeTag.CategoryElement.NodeTags.Clear();
            typeNodeTag.CategoryElement.NodeTags.Clear();
            operatorNodeTag.CategoryElement.NodeTags.Clear();
            node.LogisticCenter?.Nodes.Clear();
            return new Node
            {
                NodeId = node.NodeId,
                Name = node.Name,
                Description = node.Description,
                IsActive = node.IsActive,
                SendToSap = node.SendToSap,
                ControlLimit = node.ControlLimit,
                LogisticCenterId = node.LogisticCenterId,
                LogisticCenter = node.LogisticCenter,
                NodeTypeId = typeNodeTag.ElementId,
                NodeType = typeNodeTag.CategoryElement,
                SegmentId = segmentNodeTag.ElementId,
                Segment = segmentNodeTag.CategoryElement,
                OperatorId = operatorNodeTag.ElementId,
                Operator = operatorNodeTag.CategoryElement,
                Order = node.Order,
                Capacity = node.Capacity,
                UnitId = node.UnitId,
                Unit = node.Unit,
                RowVersion = node.RowVersion
            };
        }
    }
}
