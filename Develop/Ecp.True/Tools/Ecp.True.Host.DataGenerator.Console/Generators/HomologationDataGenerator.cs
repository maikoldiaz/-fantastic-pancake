// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationDataGenerator.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;

    /// <summary>
    /// The homologation data generator.
    /// </summary>
    public class HomologationDataGenerator : DataGeneratorBase, IDataGenerator
    {
        /// <summary>
        /// The movement repository.
        /// </summary>
        private readonly IRepository<Homologation> homologationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomologationDataGenerator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public HomologationDataGenerator(IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.homologationRepository = unitOfWork.CreateRepository<Homologation>();
        }

        /// <inheritdoc/>
        public async Task<int> GenerateAsync(IDictionary<string, object> parameters)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));
            var sourceSystemId = GetInt(parameters, "SourceSystemId", 1);
            var destinationSystemId = GetInt(parameters, "DestinationSystemId", 6);

            var homologation = await this.homologationRepository
                .SingleOrDefaultAsync(
                x => x.SourceSystemId == sourceSystemId && x.DestinationSystemId == destinationSystemId,
                "HomologationGroups",
                "HomologationGroups.Group",
                "HomologationGroups.HomologationObjects",
                "HomologationGroups.HomologationDataMapping").ConfigureAwait(false);

            homologation ??= new Homologation
            {
                SourceSystemId = sourceSystemId,
                DestinationSystemId = destinationSystemId,
            };

            var groupTypeId = GetInt(parameters, "GroupTypeId", 9);
            var homologationGroup = homologation.HomologationGroups.SingleOrDefault(x => x.GroupTypeId == groupTypeId);
            if (homologationGroup == null)
            {
                homologationGroup = new HomologationGroup
                {
                    GroupTypeId = groupTypeId,
                };

                homologation.HomologationGroups.Add(homologationGroup);
            }

            this.AddDataMappings(homologationGroup, (IDictionary<string, object>)parameters["DataMappings"]);
            this.AddObjects(homologationGroup, (IEnumerable<int>)parameters["Objects"]);
            if (homologation.HomologationId > 0)
            {
                this.homologationRepository.Update(homologation);
            }
            else
            {
                this.homologationRepository.Insert(homologation);
            }

            return homologation.HomologationId;
        }

        private void AddDataMappings(HomologationGroup homologationGroup, IDictionary<string, object> parameters)
        {
            foreach (var item in parameters)
            {
                if (!homologationGroup.HomologationDataMapping.Any(x => x.SourceValue == item.Key))
                {
                    homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping
                    {
                        SourceValue = item.Key,
                        DestinationValue = Convert.ToString(item.Value, CultureInfo.InvariantCulture),
                    });
                }
            }
        }

        private void AddObjects(HomologationGroup homologationGroup, IEnumerable<int> objects)
        {
            foreach (var item in objects)
            {
                if (!homologationGroup.HomologationObjects.Any(x => x.HomologationObjectTypeId == item))
                {
                    homologationGroup.HomologationObjects.Add(new HomologationObject
                    {
                        HomologationObjectTypeId = item,
                        IsRequiredMapping = true,
                    });
                }
            }
        }
    }
}
