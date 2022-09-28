// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlobOperations.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The Movement validator Processor.
    /// </summary>
    /// <seealso cref="IBlobOperations" />
    public class BlobOperations : IBlobOperations
    {
        /// <summary>
        /// The blob processing.
        /// </summary>
        private readonly string blobProcessing = "The json is getting downloaded from blob";

        /// <summary>
        /// The entity creation.
        /// </summary>
        private readonly string entityCreation = "The entity is getting created from json";

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<BlobOperations> logger;

        /// <summary>
        /// The azureclient factory.
        /// </summary>
        private readonly IAzureClientFactory azureclientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobOperations" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="azureclientFactory">The azureclient factory.</param>
        public BlobOperations(
            ITrueLogger<BlobOperations> logger,
            IAzureClientFactory azureclientFactory)
        {
            this.logger = logger;
            this.azureclientFactory = azureclientFactory;
        }

        /// <inheritdoc/>
        public async Task<JToken> GetHomologatedJsonAsync(string blobPath, string uploadId)
        {
            try
            {
                this.logger.LogInformation(this.blobProcessing, uploadId);
                return await this.azureclientFactory.GetBlobStorageSaSClient(ContainerName.True, blobPath).ParseAsync<JToken>().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"{blobPath} {Constants.BlobNotFound}", uploadId);
                return null;
            }
        }

        /// <inheritdoc/>
        public Tuple<TObject, List<string>, object> GetHomologatedObject<TObject>(JToken token, string uploadId)
        {
            ArgumentValidators.ThrowIfNull(uploadId, nameof(uploadId));
            ArgumentValidators.ThrowIfNull(token, nameof(token));
            try
            {
                this.logger.LogInformation(this.entityCreation, uploadId);
                return Tuple.Create(token.ToObject<TObject>(), new List<string>(), null as object);
            }
            catch (JsonReaderException ex)
            {
                var errors = LogFailure(ex);
                return Tuple.Create(default(TObject), errors.ToList(), ex as object);
            }
            catch (Exception ex)
            {
                var errors = LogFailure(ex);
                return Tuple.Create(default(TObject), errors.ToList(), ex as object);
            }
        }

        /// <inheritdoc/>
        public Tuple<TObject, List<string>, object> DoGetContractToDelete<TObject>(JToken homologatedToken)
        {
            ArgumentValidators.ThrowIfNull(homologatedToken, nameof(homologatedToken));

            Contract contractObject = new Contract
            {
                DocumentNumber = (int)homologatedToken["DocumentNumber"],
                Position = (int)homologatedToken["Position"],
                PositionStatus = (string)homologatedToken["PositionStatus"],
                OriginMessageId = (string)homologatedToken["OriginMessageId"],
                ActionType = (string)homologatedToken["ActionType"],
            };

            JToken token = JObject.Parse(JsonConvert.SerializeObject(contractObject));

            return Tuple.Create(token.ToObject<TObject>(), new List<string>(), null as object);
        }

        /// <summary>
        /// Logs the deserialization error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>The file registration errors.</returns>
        private static IEnumerable<string> LogFailure(JsonReaderException ex)
        {
            var errorResponseList = new List<ErrorInfo>();
            var regex = @"(\w+) valid (\w+)";
            var m = Regex.Match(ex.Message, regex);
            if (m.Groups.Count < 2)
            {
                regex = @"(\w+) to (\w+)";
                m = Regex.Match(ex.Message, regex);
            }

            errorResponseList.Add(new ErrorInfo("4000", ex.Path + Constants.InvalidDataType + m.Groups[2]));
            var validationResult = new ValidationResult(errorResponseList);
            return validationResult.ErrorInfo.Select(x => x.Message);
        }

        /// <summary>
        /// Logs the deserialization error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>The file registration errors.</returns>
        private static IEnumerable<string> LogFailure(Exception ex)
        {
            var errorResponseList = new List<ErrorInfo>();

            errorResponseList.Add(new ErrorInfo("4000" + Constants.InvalidDataType + ex.Message));
            var validationResult = new ValidationResult(errorResponseList);
            return validationResult.ErrorInfo.Select(x => x.Message);
        }
    }
}