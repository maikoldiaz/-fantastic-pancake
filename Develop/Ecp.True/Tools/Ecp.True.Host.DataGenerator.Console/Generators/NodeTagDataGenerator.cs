// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeTagDataGenerator.cs" company="Microsoft">
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
    using Ecp.True.Host.DataGenerator.Console.Interfaces;
    using EfCore.Models;

    /// <summary>
    /// The NodeTagDataGenerator.
    /// </summary>
    /// <seealso cref="IDataGenerator" />
    public class NodeTagDataGenerator : DataGeneratorBase, IDataGenerator
    {
        /// <summary>
        /// The node tag repository.
        /// </summary>
        private readonly IRepository<NodeTag> nodeTagRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeTagDataGenerator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public NodeTagDataGenerator(IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.nodeTagRepository = unitOfWork.CreateRepository<NodeTag>();
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

            var nodeTag = GetNodeTag(parameters);
            this.nodeTagRepository.Insert(nodeTag);

            return Task.FromResult(nodeTag.NodeTagId);
        }

        /// <summary>
        /// Gets the node tag.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The NodeTag.</returns>
        private static NodeTag GetNodeTag(IDictionary<string, object> parameters)
        {
            return new NodeTag
            {
                NodeId = GetInt(parameters, "NodeId", 123),
                ElementId = GetInt(parameters, "ElementId", 10),
                StartDate = GetDate(parameters, "StartDate", DateTime.UtcNow.ToTrue()),
                EndDate = GetDate(parameters, "EndDate", DateTime.MaxValue.Date),
            };
        }
    }
}