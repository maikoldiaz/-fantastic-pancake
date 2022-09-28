// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeTagProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;
    using EfCore.Models;

    /// <summary>
    ///     The Node Processor.
    /// </summary>
    public class NodeTagProcessor : ProcessorBase, INodeTagProcessor
    {
        /// <summary>
        /// The business context.
        /// </summary>
        private readonly IBusinessContext businessContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeTagProcessor" /> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="businessContext">The business context.</param>
        public NodeTagProcessor(IRepositoryFactory repositoryFactory, IBusinessContext businessContext)
            : base(repositoryFactory)
        {
            this.businessContext = businessContext;
        }

        /// <summary>
        /// Saves the node asynchronous.
        /// </summary>
        /// <param name="taggedNodeInfo">The tagged node information.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        public Task TagNodeAsync(TaggedNodeInfo taggedNodeInfo)
        {
            ArgumentValidators.ThrowIfNull(taggedNodeInfo, nameof(taggedNodeInfo));
            var parameters = new Dictionary<string, object>();
            parameters.Add("@OperationType", taggedNodeInfo.OperationalType);
            parameters.Add("@ElementId", taggedNodeInfo.ElementId);
            parameters.Add("@InputDate", taggedNodeInfo.InputDate);
            taggedNodeInfo.TaggedNodes.ForEach(x =>
            {
                x.CreatedBy = this.businessContext.UserId;
                x.CreatedDate = DateTime.UtcNow.ToTrue();
                if (taggedNodeInfo.OperationalType == OperationalType.Change || taggedNodeInfo.OperationalType == OperationalType.Expire)
                {
                    x.LastModifiedDate = DateTime.UtcNow.ToTrue();
                    x.LastModifiedBy = this.businessContext.UserId;
                }
            });
            parameters.Add("@NodeTag", taggedNodeInfo.TaggedNodes.ToDataTable(Repositories.Constants.NodeTagType));
            return this.CreateRepository<NodeTag>().ExecuteAsync(Repositories.Constants.NodeTagProcedureName, parameters);
        }
    }
}