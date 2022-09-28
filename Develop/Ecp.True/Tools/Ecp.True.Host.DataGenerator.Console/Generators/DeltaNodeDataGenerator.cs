// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNodeDataGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;

    /// <summary>
    /// The DeltaNodeDataGenerator.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.DataGenerator.Console.Generators.DataGeneratorBase" />
    /// <seealso cref="IDataGenerator" />
    public class DeltaNodeDataGenerator : DataGeneratorBase, IDataGenerator
    {
        /// <summary>
        /// The delta node repository.
        /// </summary>
        private readonly IRepository<DeltaNode> deltaNodeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaNodeDataGenerator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeltaNodeDataGenerator(IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));

            this.deltaNodeRepository = unitOfWork.CreateRepository<DeltaNode>();
        }

        /// <summary>
        /// Generates the specified parameters asynchronously.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The identity.
        /// </returns>
        public Task<int> GenerateAsync(IDictionary<string, object> parameters)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));

            var deltaNode = GetDeltaNode(parameters);
            this.deltaNodeRepository.Insert(deltaNode);
            return Task.FromResult(deltaNode.DeltaNodeId);
        }

        private static DeltaNode GetDeltaNode(IDictionary<string, object> parameters)
        {
            var deltaNode = new DeltaNode
            {
                TicketId = GetInt(parameters, "TicketId", 1),
                NodeId = GetInt(parameters, "NodeId", 1),
                Status = parameters.TryGetValue("Status", out object status) ? (True.Entities.Enumeration.OwnershipNodeStatusType)status : True.Entities.Enumeration.OwnershipNodeStatusType.DELTAS,
                LastApprovedDate = GetDate(parameters, "LastApprovedDate", null),
                LastModifiedDate = DateTime.Today,
            };

            return deltaNode;
        }
    }
}
