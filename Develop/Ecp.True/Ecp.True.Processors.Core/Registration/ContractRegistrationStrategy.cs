// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractRegistrationStrategy.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Registration
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Proxies.Azure;
    using Newtonsoft.Json;

    /// <summary>
    /// The ContractRegistrationStrategy.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Registration.RegistrationStrategyBase" />
    public class ContractRegistrationStrategy : RegistrationStrategyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractRegistrationStrategy"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureClientFactory">The azure client factory.</param>
        public ContractRegistrationStrategy(
                 ITrueLogger logger,
                 IAzureClientFactory azureClientFactory)
                 : base(azureClientFactory, logger)
        {
        }

        /// <inheritdoc/>
        public override void Insert(IEnumerable<object> entities, IUnitOfWork unitOfWork)
        {
            this.Logger.LogInformation(JsonConvert.SerializeObject(entities));
        }

        /// <summary>
        /// Registers asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="unitOfWork">The unitOfWork.</param>
        /// <returns>The task.</returns>
        public override async Task RegisterAsync(object entity, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));

            var contractEntity = (Contract)entity;

            await RegistrationExecutorAsync(contractEntity, unitOfWork).ConfigureAwait(false);
        }

        /// <summary>
        /// Does the get contract asynchronous.
        /// </summary>
        /// <param name="contractObject">The contract object.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>The task.</returns>
        private static Task<Contract> DoGetContractAsync(Contract contractObject, IRepository<Contract> repository)
        {
            return repository.SingleOrDefaultAsync(
                x => x.DocumentNumber == contractObject.DocumentNumber &&
                x.Position == contractObject.Position &&
                x.IsDeleted == false);
        }

        /// <summary>
        /// Registers the create contract asynchronous.
        /// </summary>
        /// <param name="contractObject">The contract object.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        private static void RegisterCreateContract(Contract contractObject, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(contractObject, nameof(contractObject));

            var repository = unitOfWork.CreateRepository<Contract>();
            contractObject.IsDeleted = false;
            repository.Insert(contractObject);
        }

        /// <summary>
        /// Register the update Contract asynchronous.
        /// </summary>
        /// <param name="contractObject">The Contract.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// The boolean.
        /// </returns>
        private static async Task RegisterUpdateContractAsync(Contract contractObject, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(contractObject, nameof(contractObject));

            var repository = unitOfWork.CreateRepository<Contract>();
            var existing = await DoGetContractAsync(contractObject, repository).ConfigureAwait(false);

            existing.CopyFrom(contractObject);
            repository.Update(existing);
        }

        /// <summary>
        /// Register the delete Contract asynchronous.
        /// </summary>
        /// <param name="contractObject">The Contract.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>
        /// The boolean.
        /// </returns>
        private static async Task RegisterDeleteContractAsync(Contract contractObject, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(contractObject, nameof(contractObject));

            var repository = unitOfWork.CreateRepository<Contract>();
            var contractObj = await DoGetContractAsync(contractObject, repository).ConfigureAwait(false);
            contractObj.IsDeleted = true;
            contractObj.PositionStatus = contractObject.PositionStatus;
            contractObj.EventType = EventType.Delete.ToString("G");
            repository.Update(contractObj);
        }

        /// <summary>
        /// Registrations the executor asynchronous.
        /// </summary>
        /// <param name="contractEntity">The contract entity.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>The task.</returns>
        private static async Task RegistrationExecutorAsync(Contract contractEntity, IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(contractEntity, nameof(contractEntity));

            if (contractEntity.ActionType.EqualsIgnoreCase(EventType.Insert.ToString("G")))
            {
                RegisterCreateContract(contractEntity, unitOfWork);
            }

            if (contractEntity.ActionType.EqualsIgnoreCase(EventType.Update.ToString("G")))
            {
                await RegisterUpdateContractAsync(contractEntity, unitOfWork).ConfigureAwait(false);
            }

            if (contractEntity.ActionType.EqualsIgnoreCase(EventType.Delete.ToString("G")))
            {
                await RegisterDeleteContractAsync(contractEntity, unitOfWork).ConfigureAwait(false);
            }

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
