// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionDataGenerator.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Threading.Tasks;

    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;

    /// <summary>
    /// The NodeConnectionDataGenerator.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.DataGenerator.Console.Interfaces.IDataGenerator" />
    public class NodeConnectionDataGenerator : DataGeneratorBase, IDataGenerator
    {
        /// <summary>
        /// The node connection repository.
        /// </summary>
        private readonly IRepository<NodeConnection> nodeConnectionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConnectionDataGenerator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public NodeConnectionDataGenerator(IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.nodeConnectionRepository = unitOfWork.CreateRepository<NodeConnection>();
        }

        /// <summary>
        /// Generates the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The identity.
        /// </returns>
        public Task<int> GenerateAsync(IDictionary<string, object> parameters)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));

            var nodeConnection = GetNodeConnection(parameters);
            this.nodeConnectionRepository.Insert(nodeConnection);

            return Task.FromResult(nodeConnection.NodeConnectionId);
        }

        /// <summary>
        /// Gets the node connection.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The NodeConnection.</returns>
        private static NodeConnection GetNodeConnection(IDictionary<string, object> parameters)
        {
            return new NodeConnection
            {
                SourceNodeId = GetInt(parameters, "SourceNodeId", 123),
                DestinationNodeId = GetInt(parameters, "DestinationNodeId", 123),
                Description = "Node Connection - Data Generator",
                IsActive = true,
                IsDeleted = false,
                ControlLimit = GetDecimal(parameters, "ControlLimit", 0.2M),
                IsTransfer = parameters.TryGetValue("IsTransfer", out object isTransfer) && Convert.ToBoolean(isTransfer, CultureInfo.InvariantCulture),
                AlgorithmId = GetInt(parameters, "AlgorithmId", 1),
            };
        }
    }
}
