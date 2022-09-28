// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Functions.Transform;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Transform.Input.Interfaces;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The Transform Processor.
    /// </summary>
    /// <seealso cref="ITransformProcessor" />
    public abstract class TransformProcessor : ITransformProcessor
    {
        /// <summary>
        /// The invalid message.
        /// </summary>
        private readonly string invalidMessage = "Invalid label supplied";

        /// <summary>
        /// The tranformed to canonical.
        /// </summary>
        private readonly string tranformedToCanonical = "Canonical tranformation complete";

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger logger;

        /// <summary>
        /// The input factory.
        /// </summary>
        private readonly IInputFactory inputFactory;

        /// <summary>
        /// The data service.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="inputFactory">The input factory.</param>
        /// <param name="dataService">The data service.</param>
        protected TransformProcessor(
            ITrueLogger logger,
            IInputFactory inputFactory,
            IDataService dataService)
        {
            this.logger = logger;
            this.inputFactory = inputFactory;
            this.dataService = dataService;
        }

        /// <inheritdoc/>
        public abstract InputType InputType { get; }

        /// <inheritdoc/>
        public virtual Task CompleteAsync(JArray homologatedArray, TrueMessage trueMessage)
        {
            return this.dataService.SaveAsync(homologatedArray, trueMessage);
        }

        /// <inheritdoc/>
        public virtual async Task<RegistrationData> TransformAsync(TrueMessage message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            FileRegistration fileRegistration = null;
            JArray output = null;

            if (message.IsRetry)
            {
                var fileRegistrationTransaction = await this.inputFactory.GetFileRegistrationTransactionAsync(message.FileRegistrationTransactionId).ConfigureAwait(false);
                fileRegistrationTransaction.IsRetry = true;
                fileRegistrationTransaction.StatusTypeId = StatusType.PROCESSING;
                message.AddFileRegistrationTransaction(fileRegistrationTransaction);
            }
            else
            {
                fileRegistration = await this.inputFactory.GetFileRegistrationAsync(message.MessageId).ConfigureAwait(false);
                message.FileRegistration = fileRegistration;
            }

            try
            {
                if (!message.IsValid && !message.IsRetry)
                {
                    this.logger.LogInformation(this.invalidMessage, message.MessageId);
                    throw new InvalidDataException(this.invalidMessage);
                }

                output = await this.DoTransformAsync(message).ConfigureAwait(false);
                this.logger.LogInformation(this.tranformedToCanonical, message.MessageId);

                if (fileRegistration != null)
                {
                    fileRegistration.IsParsed = true;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message, message.MessageId);

                if (fileRegistration != null)
                {
                    fileRegistration.IsParsed = false;
                }

                if (output != null)
                {
                    message.PopulatePendingTransactions(ex.Message, Constants.FailedParsing, Constants.TechnicalExceptionParsingErrorMessage, JObject.Parse(JsonConvert.SerializeObject(output)));
                }
                else
                {
                    message.PopulatePendingTransactions(ex.Message, Constants.FailedParsing, Constants.TechnicalExceptionParsingErrorMessage);
                }

                output = new JArray();
            }

            return new RegistrationData
            {
                Data = output,
                TrueMessage = message,
            };
        }

        /// <summary>
        /// Does transform asynchronously.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<JArray> DoTransformAsync(TrueMessage message);
    }
}