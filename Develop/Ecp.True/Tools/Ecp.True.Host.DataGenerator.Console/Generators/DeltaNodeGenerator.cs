// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNodeGenerator.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;

    /// <summary>
    /// delta node generator.
    /// </summary>
    public class DeltaNodeGenerator : DataGeneratorBase, IDataGenerator
    {
        /// <summary>
        /// The movement repository.
        /// </summary>
        private readonly IRepository<DeltaNode> deltaNodeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaNodeGenerator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeltaNodeGenerator(IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.deltaNodeRepository = unitOfWork.CreateRepository<DeltaNode>();
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
            var node = new DeltaNode
            {
                Status = OwnershipNodeStatusType.APPROVED,
                TicketId = GetInt(parameters, "TicketId", 1),
                NodeId = GetInt(parameters, "NodeId", 1),
            };

            this.deltaNodeRepository.Insert(node);

            return Task.FromResult(node.DeltaNodeId);
        }
    }
}
