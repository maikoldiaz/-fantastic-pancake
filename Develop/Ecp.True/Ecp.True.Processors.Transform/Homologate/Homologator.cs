// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Homologator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Homologate
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Transform.Homologate.Interfaces;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The homologator class.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Transform.Homologate.IHomologator" />
    public class Homologator : IHomologator
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IHomologationMapper mapper;

        /// <summary>
        /// The transformation mapper.
        /// </summary>
        private readonly ITransformationMapper transformationMapper;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<Homologator> logger;

        /// <summary>
        /// The file registration transaction generator.
        /// </summary>
        private readonly IFileRegistrationTransactionGenerator fileRegistrationTransactionGenerator;

        /// <summary>
        /// The movement ID.
        /// </summary>
        private readonly string blobPath = "BlobPath";

        /// <summary>
        /// The output.
        /// </summary>
        private ConcurrentBag<JObject> output;

        /// <summary>
        /// Initializes a new instance of the <see cref="Homologator" /> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="transformationMapper">The transformation mapper.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fileRegistrationTransactionGenerator">The file registration transaction generator.</param>
        public Homologator(
            IHomologationMapper mapper,
            ITransformationMapper transformationMapper,
            ITrueLogger<Homologator> logger,
            IFileRegistrationTransactionGenerator fileRegistrationTransactionGenerator)
        {
            this.mapper = mapper;
            this.transformationMapper = transformationMapper;
            this.logger = logger;
            this.fileRegistrationTransactionGenerator = fileRegistrationTransactionGenerator;
        }

        /// <summary>
        /// The Homologate method.
        /// </summary>
        /// <param name="message">The true message.</param>
        /// <param name="data">The data.</param>
        /// <param name="shouldHomologate">if set to <c>true</c> [should homologate].</param>
        /// <returns>
        /// A <see cref="Task{TResult}" /> representing the result of the asynchronous operation.
        /// </returns>
        public async Task<JArray> HomologateAsync(TrueMessage message, JToken data, bool shouldHomologate)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            ArgumentValidators.ThrowIfNull(data, nameof(data));

            this.output = new ConcurrentBag<JObject>();
            var tasks = new List<Task>();

            this.fileRegistrationTransactionGenerator.UpdateFileRegistrationTransactions(message, (JArray)data);

            if (!shouldHomologate)
            {
                return JArray.FromObject(data.ToArray());
            }

            tasks.AddRange(data.Children().Select(d => Task.Run(() => this.Homologate(message, d))));
            await Task.WhenAll(tasks).ConfigureAwait(false);

            return JArray.FromObject(this.output.ToArray());
        }

        /// <summary>
        /// Homologate the single.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// Returns the task.
        /// </returns>
        public JArray HomologateObject(TrueMessage message, JToken data)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            ArgumentValidators.ThrowIfNull(data, nameof(data));

            if (message.SourceSystem == SystemType.SAP && !message.ShouldHomologate)
            {
                var result = new JArray { JObject.FromObject(data) };
                return JArray.Parse(result.ToString());
            }

            this.output = new ConcurrentBag<JObject>();
            this.Homologate(message, data);
            return JArray.FromObject(this.output.ToArray());
        }

        /// <summary>
        /// Builds the property.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="key">The key.</param>
        /// <param name="homologated">The homologated.</param>
        /// <returns>The JSON property with homologated value.</returns>
        private static JProperty BuildProperty(TrueMessage message, string key, object homologated)
        {
            if (homologated == null)
            {
                return new JProperty(key, default(object));
            }

            var strValue = homologated.ToString();
            if (message.TargetSystem == SystemType.TRUE && int.TryParse(strValue, out int val))
            {
                return new JProperty(key, val);
            }

            return new JProperty(key, homologated);
        }

        /// <summary>
        /// Gets the token value.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The token value.</returns>
        private static object GetTokenValue(JToken token)
        {
            if (token.Type == JTokenType.Null)
            {
                return null;
            }

            if (token.Type == JTokenType.String)
            {
                return token.Value<string>();
            }

            if (token.Type == JTokenType.Date)
            {
                return token.Value<DateTime>();
            }

            if (token.Type == JTokenType.Integer)
            {
                return token.Value<long>();
            }

            if (token.Type == JTokenType.Float)
            {
                return token.Value<decimal>();
            }

            if (token.Type == JTokenType.Boolean)
            {
                return token.Value<bool>();
            }

            return null;
        }

        /// <summary>
        /// Homologate the array.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="entity">The entity.</param>
        private void Homologate(TrueMessage message, JToken entity)
        {
            JToken result = null;
            string errorMessage = null;

            try
            {
                message.IsHomologated = entity[Constants.IsHomologated] != null && (bool)entity[Constants.IsHomologated];

                result = this.DoHomologateObject(message, (JObject)entity);
                this.transformationMapper.Transform(result);

                result[Constants.IsHomologated] = true;
            }
            catch (Exception ex) when (ex is KeyNotFoundException || ex is NotSupportedException)
            {
                this.logger.LogError(ex, ex.Message, message.MessageId);
                errorMessage = ex.Message;
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                var fileRegistrationTransaction = message.FileRegistration.FileRegistrationTransactions.FirstOrDefault(x => x.BlobPath == entity[this.blobPath].ToString());
                fileRegistrationTransaction.StatusTypeId = StatusType.FAILED;

                message.PopulatePendingTransactions(errorMessage, entity, fileRegistrationTransaction);

                result = entity;
                result[Constants.IsHomologated] = false;
            }

            this.output.Add((JObject)result);
        }

        /// <summary>
        /// Homologate the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="entity">The entity to homologate.</param>
        /// <returns>
        /// The homologated entity.
        /// </returns>
        private JObject DoHomologateObject(TrueMessage message, JObject entity)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            var result = new JObject();
            foreach (var item in entity)
            {
                if (item.Value != null && item.Value.Type == JTokenType.Array)
                {
                    var value = this.DoHomologateArray(message, (JArray)item.Value);
                    result.Add(item.Key, value);
                }
                else if (item.Value != null && item.Value.Type == JTokenType.Object)
                {
                    var value = this.DoHomologateObject(message, (JObject)item.Value);
                    result.Add(item.Key, value);
                }
                else
                {
                    var value = this.mapper.Homologate(message, item.Key, GetTokenValue(item.Value));
                    result.Add(BuildProperty(message, item.Key, value));
                }
            }

            return result;
        }

        /// <summary>
        /// Homologate the specified array.
        /// </summary>
        /// <param name="arr">The arr.</param>
        /// <returns>The homologate json array.</returns>
        private JArray DoHomologateArray(TrueMessage message, JArray arr)
        {
            var result = new JArray();
            foreach (var item in arr)
            {
                result.Add(this.DoHomologateObject(message, (JObject)item));
            }

            return result;
        }
    }
}