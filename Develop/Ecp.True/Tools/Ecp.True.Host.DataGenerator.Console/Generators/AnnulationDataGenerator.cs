// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnulationDataGenerator.cs" company="Microsoft">
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
    /// The AnnulationDataGenerator.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.DataGenerator.Console.Interfaces.IDataGenerator" />
    public class AnnulationDataGenerator : DataGeneratorBase, IDataGenerator
    {
        /// <summary>
        /// The annulation repository.
        /// </summary>
        private readonly IRepository<Annulation> annulationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnulationDataGenerator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public AnnulationDataGenerator(IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.annulationRepository = unitOfWork.CreateRepository<Annulation>();
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

            var annulation = GetAnnulation(parameters);
            this.annulationRepository.Insert(annulation);
            return Task.FromResult(annulation.AnnulationId);
        }

        /// <summary>
        /// Gets the node tag.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The Annulation.</returns>
        private static Annulation GetAnnulation(IDictionary<string, object> parameters)
        {
            var annulation = new Annulation
            {
                SourceMovementTypeId = GetInt(parameters, "SourceMovementTypeId", 156),
                AnnulationMovementTypeId = GetInt(parameters, "AnnulationMovementTypeId", 154),
                SourceNodeId = GetInt(parameters, "SourceNodeId", 1),
                DestinationNodeId = GetInt(parameters, "DestinationNodeId", 3),
                SourceProductId = GetInt(parameters, "SourceProductId", 1),
                DestinationProductId = GetInt(parameters, "DestinationProductId", 3),
                IsActive = !parameters.TryGetValue("IsActive", out object isActive) || Convert.ToBoolean(isActive, CultureInfo.InvariantCulture),
            };

            return annulation;
        }
    }
}
